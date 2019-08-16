// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PushResponseBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Adapters;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Remote.Push;

    public class PushResponseBuilder
    {
        private readonly ISession session;
        private readonly PushRequest pushRequest;
        private readonly User user;

        public PushResponseBuilder(ISession session, User user, PushRequest pushRequest)
        {
            this.session = session;
            this.user = user;
            this.pushRequest = pushRequest;
        }

        public PushResponse Build()
        {
            var pushResponse = new PushResponse();

            Dictionary<string, IObject> objectByNewId = null;

            if (this.pushRequest.NewObjects != null && this.pushRequest.NewObjects.Length > 0)
            {
                objectByNewId = this.pushRequest.NewObjects.ToDictionary(
                    x => x.NI,
                    x =>
                        {
                            var cls = this.session.Database.MetaPopulation.FindClassByName(x.T);
                            return (IObject)Allors.ObjectBuilder.Build(this.session, cls);
                        });
            }

            if (this.pushRequest.Objects != null && this.pushRequest.Objects.Length > 0)
            {
                // bulk load all objects
                var objectIds = this.pushRequest.Objects.Select(v => v.I).ToArray();
                var objects = this.session.Instantiate(objectIds);

                if (objectIds.Length != objects.Length)
                {
                    var existingIds = objects.Select(v => v.Id.ToString());
                    var missingIds = objectIds.Where(v => !existingIds.Contains(v));
                    foreach (var missingId in missingIds)
                    {
                        pushResponse.AddMissingError(missingId);
                    }
                }

                if (!pushResponse.HasErrors)
                {
                    foreach (var pushRequestObject in this.pushRequest.Objects)
                    {
                        var obj = this.session.Instantiate(pushRequestObject.I);

                        if (!pushRequestObject.V.Equals(obj.Strategy.ObjectVersion.ToString()))
                        {
                            pushResponse.AddVersionError(obj);
                        }
                        else
                        {
                            var pushRequestRoles = pushRequestObject.Roles;
                            this.PushRequestRoles(pushRequestRoles, obj, pushResponse, objectByNewId);
                        }
                    }
                }
            }

            if (objectByNewId != null && !pushResponse.HasErrors)
            {
                var countOutstandingRoles = 0;
                int previousCountOutstandingRoles;
                do
                {
                    previousCountOutstandingRoles = countOutstandingRoles;
                    countOutstandingRoles = 0;

                    foreach (var pushRequestNewObject in this.pushRequest.NewObjects)
                    {
                        var obj = objectByNewId[pushRequestNewObject.NI];
                        var pushRequestRoles = pushRequestNewObject.Roles;
                        if (pushRequestRoles != null)
                        {
                            countOutstandingRoles += this.PushRequestRoles(pushRequestRoles, obj, pushResponse, objectByNewId, true);
                        }
                    }
                }
                while (countOutstandingRoles != previousCountOutstandingRoles);

                if (countOutstandingRoles > 0)
                {
                    foreach (var pushRequestNewObject in this.pushRequest.NewObjects)
                    {
                        var obj = objectByNewId[pushRequestNewObject.NI];
                        var pushRequestRoles = pushRequestNewObject.Roles;
                        if (pushRequestRoles != null)
                        {
                            this.PushRequestRoles(pushRequestRoles, obj, pushResponse, objectByNewId);
                        }
                    }
                }

                foreach (var newObject in objectByNewId.Values)
                {
                    ((Allors.Domain.Object)newObject).OnBuild();
                }

                foreach (var newObject in objectByNewId.Values)
                {
                    ((Allors.Domain.Object)newObject).OnPostBuild();
                }
            }

            var validation = this.session.Derive(false);

            if (validation.HasErrors)
            {
                pushResponse.AddDerivationErrors(validation);
            }

            if (!pushResponse.HasErrors)
            {
                if (objectByNewId != null)
                {
                    pushResponse.NewObjects = objectByNewId.Select(dictionaryEntry => new PushResponseNewObject
                    {
                        I = dictionaryEntry.Value.Id.ToString(),
                        NI = dictionaryEntry.Key
                    }).ToArray();
                }

                this.session.Commit();
            }

            return pushResponse;
        }

        private static void AddMissingRoles(IObject[] actualRoles, string[] requestedRoleIds, PushResponse pushResponse)
        {
            var actualRoleIds = actualRoles.Select(x => x.Id.ToString());
            var missingRoleIds = requestedRoleIds.Except(actualRoleIds);
            foreach (var missingRoleId in missingRoleIds)
            {
                pushResponse.AddMissingError(missingRoleId);
            }
        }

        private int PushRequestRoles(IList<PushRequestRole> pushRequestRoles, IObject obj, PushResponse pushResponse, Dictionary<string, IObject> objectByNewId, bool ignore = false)
        {
            var countOutstandingRoles = 0;
            foreach (var pushRequestRole in pushRequestRoles)
            {
                var composite = (Composite)obj.Strategy.Class;
                var roleTypes = composite.WorkspaceRoleTypes;
                var acl = new AccessControlList(obj, this.user);

                var roleTypeName = pushRequestRole.T;
                var roleType = roleTypes.FirstOrDefault(v => v.PropertyName.Equals(roleTypeName));

                if (roleType != null)
                {
                    if (acl.CanWrite(roleType))
                    {
                        if (roleType.ObjectType.IsUnit)
                        {
                            var unitType = (IUnit)roleType.ObjectType;
                            var role = pushRequestRole.S;
                            if (role is string)
                            {
                                role = Serialization.ReadString((string)role, unitType.UnitTag);
                            }

                            // Json.net deserializes number to long or double, instead of int. 
                            if (unitType.IsInteger && role != null && !(role is int))
                            {
                                role = Convert.ToInt32(role);
                            }

                            obj.Strategy.SetUnitRole(roleType.RelationType, role);
                        }
                        else
                        {
                            if (roleType.IsOne)
                            {
                                var roleId = (string)pushRequestRole.S;
                                if (string.IsNullOrEmpty(roleId))
                                {
                                    obj.Strategy.RemoveCompositeRole(roleType.RelationType);
                                }
                                else
                                {
                                    var role = this.GetRole(roleId, objectByNewId);
                                    if (role == null)
                                    {
                                        pushResponse.AddMissingError(roleId);
                                    }
                                    else
                                    {
                                        obj.Strategy.SetCompositeRole(roleType.RelationType, role);
                                    }
                                }
                            }
                            else
                            {
                                // Add
                                if (pushRequestRole.A != null)
                                {
                                    var roleIds = pushRequestRole.A;
                                    if (roleIds.Length != 0)
                                    {
                                        var roles = this.GetRoles(roleIds, objectByNewId);
                                        if (roles.Length != roleIds.Length)
                                        {
                                            AddMissingRoles(roles, roleIds, pushResponse);
                                        }
                                        else
                                        {
                                            foreach (var role in roles)
                                            {
                                                obj.Strategy.AddCompositeRole(roleType.RelationType, role);
                                            }
                                        }
                                    }
                                }

                                // Remove
                                if (pushRequestRole.R != null)
                                {
                                    var roleIds = pushRequestRole.R;
                                    if (roleIds.Length != 0)
                                    {
                                        var roles = this.GetRoles(roleIds, objectByNewId);
                                        if (roles.Length != roleIds.Length)
                                        {
                                            AddMissingRoles(roles, roleIds, pushResponse);
                                        }
                                        else
                                        {
                                            foreach (var role in roles)
                                            {
                                                obj.Strategy.RemoveCompositeRole(roleType.RelationType, role);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!ignore)
                        {
                            pushResponse.AddAccessError(obj);
                        }
                        else
                        {
                            countOutstandingRoles++;
                        }
                    }
                }
            }

            return countOutstandingRoles;
        }

        private IObject GetRole(string roleId, Dictionary<string, IObject> objectByNewId)
        {
            if (objectByNewId == null || !objectByNewId.TryGetValue(roleId, out var role))
            {
                role = this.session.Instantiate(roleId);
            }

            return role;
        }

        private IObject[] GetRoles(string[] roleIds, Dictionary<string, IObject> objectByNewId)
        {
            if (objectByNewId == null)
            {
                return this.session.Instantiate(roleIds);
            }

            var roles = new List<IObject>();
            List<string> existingRoleIds = null;
            foreach (var roleId in roleIds)
            {
                if (objectByNewId.TryGetValue(roleId, out var role))
                {
                    roles.Add(role);
                }
                else
                {
                    if (existingRoleIds == null)
                    {
                        existingRoleIds = new List<string>();
                    }

                    existingRoleIds.Add(roleId);
                }
            }

            if (existingRoleIds != null)
            {
                var existingRoles = this.session.Instantiate(existingRoleIds);
                roles.AddRange(existingRoles);
            }

            return roles.ToArray();
        }
    }
}

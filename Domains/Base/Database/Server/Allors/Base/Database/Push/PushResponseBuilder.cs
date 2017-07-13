using System;
using Allors;

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Adapters;
    using Allors.Domain;
    using Allors.Meta;

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
            var saveResponse = new PushResponse();

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
                this.session.Instantiate(objectIds);

                foreach (var saveRequestObject in this.pushRequest.Objects)
                {
                    var obj = this.session.Instantiate(saveRequestObject.I);

                    if (!saveRequestObject.V.Equals(obj.Strategy.ObjectVersion.ToString()))
                    {
                        saveResponse.AddVersionError(obj);
                    }
                    else
                    {
                        var saveRequestRoles = saveRequestObject.Roles;
                        this.SaveRequestRoles(saveRequestRoles, obj, saveResponse, objectByNewId);
                    }
                }
            }

            if (objectByNewId != null)
            {
                foreach (var saveRequestNewObject in this.pushRequest.NewObjects)
                {
                    var obj = objectByNewId[saveRequestNewObject.NI];
                    var saveRequestRoles = saveRequestNewObject.Roles;
                    if (saveRequestRoles != null)
                    {
                        this.SaveRequestRoles(saveRequestRoles, obj, saveResponse, objectByNewId);
                    }
                }

                foreach (Object newObject in objectByNewId.Values)
                {
                    newObject.OnBuild();
                }

                foreach (Object newObject in objectByNewId.Values)
                {
                    newObject.OnPostBuild();
                }
            }

            var validation = this.session.Derive(false);

            if (validation.HasErrors)
            {
                saveResponse.AddDerivationErrors(validation);
            }

            if (!saveResponse.HasErrors)
            {
                if (objectByNewId != null)
                {
                    saveResponse.NewObjects = objectByNewId.Select(dictionaryEntry => new PushResponseNewObject
                    {
                        I = dictionaryEntry.Value.Id.ToString(),
                        NI = dictionaryEntry.Key
                    }).ToArray();
                }

                this.session.Commit();
            }

            return saveResponse;
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

        private void SaveRequestRoles(IList<PushRequestRole> saveRequestRoles, IObject obj, PushResponse pushResponse, Dictionary<string, IObject> objectByNewId)
        {
            foreach (var saveRequestRole in saveRequestRoles)
            {
                var composite = (Composite)obj.Strategy.Class;
                var roleTypes = composite.WorkspaceRoleTypes;
                var acl = new AccessControlList(obj, this.user);

                var roleTypeName = saveRequestRole.T;
                var roleType = roleTypes.FirstOrDefault(v => v.PropertyName.Equals(roleTypeName));

                if (roleType != null)
                {
                    if (acl.CanWrite(roleType))
                    {
                        if (roleType.ObjectType.IsUnit)
                        {
                            var unitType = (IUnit)roleType.ObjectType;
                            var role = saveRequestRole.S;
                            if (role is string)
                            {
                                role = Serialization.ReadString((string)role, unitType.UnitTag);
                            }
                            //Json.net deserializes number to long, in stead of int. 
                            if (role is long)
                            {
                                role = Convert.ToInt32(role);
                            }
                            obj.Strategy.SetUnitRole(roleType.RelationType, role);
                        }
                        else
                        {
                            if (roleType.IsOne)
                            {
                                var roleId = (string)saveRequestRole.S;
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
                                if (saveRequestRole.A != null)
                                {
                                    var roleIds = saveRequestRole.A;
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
                                if (saveRequestRole.R != null)
                                {
                                    var roleIds = saveRequestRole.R;
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
                        pushResponse.AddAccessError(obj);
                    }
                }
            }
        }

        private IObject GetRole(string roleId, Dictionary<string, IObject> objectByNewId)
        {
            IObject role;
            if (objectByNewId == null || !objectByNewId.TryGetValue(roleId, out role))
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
                IObject role;
                if (objectByNewId.TryGetValue(roleId, out role))
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
                var existingRoles = this.session.Instantiate(existingRoleIds.ToArray());
                roles.AddRange(existingRoles);
            }

            return roles.ToArray();
        }
    }
}
// <copyright file="SessionObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Meta;

namespace Allors.Workspace
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Remote.Push;

    public interface ISessionObject
    {
        long Id { get; }

        long? Version { get; }

        Class ObjectType { get; }

        Session Session { get; }

        WorkspaceObject WorkspaceObject { get; set; }

        bool HasChanges { get; }

        bool CanRead(RoleType roleType);

        bool CanWrite(RoleType roleType);

        bool Exist(RoleType roleType);

        object Get(RoleType roleType);

        void Set(RoleType roleType, object value);

        void Add(RoleType roleType, ISessionObject value);

        void Remove(RoleType roleType, ISessionObject value);

        PushRequestObject Save();

        PushRequestNewObject SaveNew();

        void Reset();
    }

    public interface INewSessionObject : ISessionObject
    {
        long? NewId { get; set; }
    }

    public class SessionObject : INewSessionObject
    {
        private static readonly ISessionObject[] EmptySessionObjects = new ISessionObject[0];

        private Dictionary<RoleType, object> changedRoleByRoleType;
        private Dictionary<RoleType, object> roleByRoleType = new Dictionary<RoleType, object>();

        protected SessionObject(Session session) => this.Session = session;

        public Session Session { get; }

        public WorkspaceObject WorkspaceObject { get; set; }

        public Class ObjectType { get; set; }

        public long? NewId { get; set; }

        public long Id => this.WorkspaceObject?.Id ?? this.NewId ?? 0;

        public long? Version => this.WorkspaceObject?.Version;

        public bool HasChanges
        {
            get
            {
                if (this.NewId != null)
                {
                    return true;
                }

                return this.changedRoleByRoleType != null;
            }
        }

        public bool HasChangedRoles(params RoleType[] roleTypes)
        {
            if (roleTypes.Length == 0)
            {
                return this.HasChanges;
            }

            if (this.NewId != null)
            {
                // I am new in the session, and i have at least one of the requested roleTypes
                if (roleTypes.Any(v => this.roleByRoleType.ContainsKey(v)))
                {
                    return true;
                }

                return false;
            }

            if (this.changedRoleByRoleType != null)
            {
                if (roleTypes.Any(v => this.changedRoleByRoleType.ContainsKey(v)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanRead(RoleType roleType)
        {
            if (this.NewId != null)
            {
                return true;
            }

            return this.WorkspaceObject.CanRead(roleType.PropertyName);
        }

        public bool CanWrite(RoleType roleType)
        {
            if (this.NewId != null)
            {
                return true;
            }

            return this.WorkspaceObject.CanWrite(roleType.PropertyName);
        }

        public bool CanExecute(MethodType methodType)
        {
            if (this.NewId != null)
            {
                return true;
            }

            return this.WorkspaceObject.CanExecute(methodType.Name);
        }

        public bool Exist(RoleType roleType)
        {
            var value = this.Get(roleType);
            if (roleType.ObjectType.IsComposite && roleType.IsMany)
            {
                return ((IEnumerable<SessionObject>)value).Any();
            }

            return value != null;
        }

        public object Get(RoleType roleType)
        {
            if (!this.roleByRoleType.TryGetValue(roleType, out var value))
            {
                if (this.NewId == null)
                {
                    if (roleType.ObjectType.IsUnit)
                    {
                        this.WorkspaceObject.Roles.TryGetValue(roleType.PropertyName, out value);

                        if (value != null)
                        {
                            var unit = (Unit)roleType.ObjectType;

                            switch (unit.UnitTag)
                            {
                                case UnitTags.Binary:
                                    value = Convert.FromBase64String((string)value);
                                    break;

                                case UnitTags.DateTime:
                                    value = Convert.ToDateTime(value);
                                    break;

                                case UnitTags.Integer:
                                    value = Convert.ToInt32(value);
                                    break;

                                case UnitTags.Decimal:
                                    value = Convert.ToDecimal(value);
                                    break;

                                case UnitTags.Float:
                                    value = Convert.ToDouble(value);
                                    break;

                                case UnitTags.Unique:
                                    value = new Guid((string)value);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (roleType.IsOne)
                            {
                                this.WorkspaceObject.Roles.TryGetValue(roleType.PropertyName, out var role);
                                value = role != null ? this.Session.Get(long.Parse((string)role)) : null;
                            }
                            else
                            {
                                if (this.WorkspaceObject.Roles.TryGetValue(roleType.PropertyName, out var roles))
                                {
                                    if (roles is IList list)
                                    {
                                        var array = Array.CreateInstance(roleType.ObjectType.ClrType, list.Count);

                                        for (var i = 0; i < list.Count; i++)
                                        {
                                            var roleId = list[i];
                                            var role = this.Session.Get(long.Parse(roleId.ToString()));
                                            array.SetValue(role, i);
                                        }

                                        return array;
                                    }
                                }

                                return new ArrayList().ToArray(roleType.ObjectType.ClrType);
                            }
                        }
                        catch
                        {
                            var stringValue = "N/A";
                            try
                            {
                                stringValue = this.ToString();
                            }
                            catch
                            {
                                // ignored
                            }

                            throw new Exception($"Could not get role {roleType.PropertyName} from [objectType: ${this.ObjectType.Name}, id: ${this.Id}, value: '${stringValue}']");
                        }
                    }
                }
                else
                {
                    if (roleType.ObjectType.IsComposite && roleType.IsMany)
                    {
                        // TODO: Optimize
                        value = new ArrayList(EmptySessionObjects).ToArray(roleType.ObjectType.ClrType);
                    }
                }

                this.roleByRoleType[roleType] = value;
            }

            return value;
        }

        public void Set(RoleType roleType, object value)
        {
            var current = this.Get(roleType);
            if (roleType.ObjectType.IsUnit || roleType.IsOne)
            {
                if (object.Equals(current, value))
                {
                    return;
                }
            }
            else
            {
                if (value == null)
                {
                    value = EmptySessionObjects;
                }

                var currentCollection = (IList<object>)current;
                var valueCollection = (IList<object>)value;
                if (currentCollection.Count == valueCollection.Count && !currentCollection.Except(valueCollection).Any())
                {
                    return;
                }
            }

            if (this.changedRoleByRoleType == null)
            {
                this.changedRoleByRoleType = new Dictionary<RoleType, object>();
            }

            if (roleType.ObjectType.IsComposite && roleType.IsMany)
            {
                // TODO: Optimize
                value = new ArrayList((Array)value).ToArray(roleType.ObjectType.ClrType);
            }

            this.roleByRoleType[roleType] = value;
            this.changedRoleByRoleType[roleType] = value;
        }

        public void Add(RoleType roleType, ISessionObject value)
        {
            var roles = (ISessionObject[])this.Get(roleType);
            if (!roles.Contains(value))
            {
                roles = new List<ISessionObject>(roles) { value }.ToArray();
            }

            this.Set(roleType, roles);
        }

        public void Remove(RoleType roleType, ISessionObject value)
        {
            var roles = (ISessionObject[])this.Get(roleType);
            if (roles.Contains(value))
            {
                var newRoles = new List<ISessionObject>(roles);
                newRoles.Remove(value);
                roles = newRoles.ToArray();
            }

            this.Set(roleType, roles);
        }

        public PushRequestObject Save()
        {
            if (this.changedRoleByRoleType != null)
            {
                var data = new PushRequestObject
                {
                    I = this.Id.ToString(),
                    V = this.Version.ToString(),
                    Roles = this.SaveRoles()
                };

                return data;
            }

            return null;
        }

        public PushRequestNewObject SaveNew()
        {
            var data = new PushRequestNewObject
            {
                NI = this.NewId.ToString(),
                T = this.ObjectType.Name
            };

            if (this.changedRoleByRoleType != null)
            {
                data.Roles = this.SaveRoles();
            }

            return data;
        }

        public void Reset()
        {
            if (this.WorkspaceObject != null)
            {
                this.WorkspaceObject = this.WorkspaceObject.Workspace.Get(this.Id);
            }

            this.changedRoleByRoleType = null;

            this.roleByRoleType = new Dictionary<RoleType, object>();
        }

        private PushRequestRole[] SaveRoles()
        {
            var saveRoles = new List<PushRequestRole>();

            foreach (var keyValuePair in this.changedRoleByRoleType)
            {
                var roleType = keyValuePair.Key;
                var role = keyValuePair.Value;

                var saveRole = new PushRequestRole { T = roleType.PropertyName };

                if (roleType.ObjectType.IsUnit)
                {
                    saveRole.S = role;
                }
                else
                {
                    if (roleType.IsOne)
                    {
                        var sessionRole = (SessionObject)role;
                        saveRole.S = sessionRole?.Id.ToString();
                    }
                    else
                    {
                        var sessionRoles = (SessionObject[])role;
                        var roleIds = sessionRoles.Select(item => item.Id.ToString()).ToArray();
                        if (this.NewId != null)
                        {
                            saveRole.A = roleIds;
                        }
                        else
                        {
                            if (!this.WorkspaceObject.Roles.TryGetValue(roleType.PropertyName, out var originalRoleIdsObject))
                            {
                                saveRole.A = roleIds;
                            }
                            else
                            {
                                if (originalRoleIdsObject != null)
                                {
                                    var originalRoleIds = ((IEnumerable<object>)originalRoleIdsObject)
                                        .Select(v => v.ToString())
                                        .ToArray();
                                    saveRole.A = roleIds.Except(originalRoleIds).ToArray();
                                    saveRole.R = originalRoleIds.Except(roleIds).ToArray();
                                }
                                else
                                {
                                    saveRole.A = roleIds.ToArray();
                                }
                            }
                        }
                    }
                }

                saveRoles.Add(saveRole);
            }

            return saveRoles.ToArray();
        }
    }
}

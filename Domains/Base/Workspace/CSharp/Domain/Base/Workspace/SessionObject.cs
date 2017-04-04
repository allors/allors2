namespace Allors.Workspace {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Meta;

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

        Data.PushRequestObject Save();

        Data.PushRequestNewObject SaveNew();

        void Reset();
    }

    public interface INewSessionObject : ISessionObject {
        long? NewId { get; set; }
    }

    public class SessionObject : INewSessionObject
    {
        protected SessionObject(Session session)
        {
            this.Session = session;
        }

        public Session Session { get; }

        public WorkspaceObject WorkspaceObject { get; set; }
        public Class ObjectType { get; set; }

        public long? NewId { get; set; }

        private Dictionary<RoleType, object> changedRoleByRoleType;
        private Dictionary<RoleType, object> roleByRoleType = new Dictionary<RoleType, object>();

        public bool HasChanges {
            get
            {
                if (this.NewId != null)
                {
                    return true;
                }

                return this.changedRoleByRoleType != null;
            }
        }
 
        public long Id => this.WorkspaceObject?.Id ?? this.NewId.Value;

        public long? Version => this.WorkspaceObject?.Version;

        public bool CanRead(RoleType roleType) {
            if (this.NewId != null) {
                return true;
            }

            return this.WorkspaceObject.CanRead(roleType.PropertyName);
        }

        public bool CanWrite(RoleType roleType) {
            if (this.NewId != null) {
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
                return !((IEnumerable<SessionObject>)value).Any();
            }

            return value != null;
        }

        public object Get(RoleType roleType)
        {
            object value;
            if (!this.roleByRoleType.TryGetValue(roleType, out value))
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
                                object role;
                                this.WorkspaceObject.Roles.TryGetValue(roleType.PropertyName, out role);
                                value = role != null ? this.Session.Get(long.Parse((string)role)) : null;
                            }
                            else
                            {
                                object roles;
                                if (this.WorkspaceObject.Roles.TryGetValue(roleType.PropertyName, out roles))
                                {
                                    var list = roles as IList;
                                    if (list != null)
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

                                // TODO: Optimize
                                return Array.CreateInstance(roleType.ObjectType.ClrType, 0);
                            }
                        }
                        catch(Exception e)
                        {
                            var stringValue = "N/A";
                            try
                            {
                                stringValue = this.ToString();
                            }
                            catch { };

                            throw new Exception($"Could not get role {roleType.PropertyName} from [objectType: ${this.ObjectType.Name}, id: ${this.Id}, value: '${stringValue}']");
                        }
                    }
                }
                else
                {
                    if (roleType.ObjectType.IsComposite && roleType.IsMany)
                    {
                        // TODO: Optimize
                        value = Array.CreateInstance(roleType.ObjectType.ClrType, 0);
                    }
                }

                this.roleByRoleType[roleType] = value;
            }

            return value;
        }

        public void Set(RoleType roleType, object value) {
            if (this.changedRoleByRoleType == null) {
                this.changedRoleByRoleType = new Dictionary<RoleType, object>();
            }

            if (value == null) {
                if (roleType.ObjectType.IsComposite && roleType.IsMany) {
                    value = new ISessionObject[0];
                }
            }

            if (roleType.ObjectType.IsComposite && roleType.IsMany)
            {
                var untypedArray = (Array)value;
                var typedArray = Array.CreateInstance(roleType.ObjectType.ClrType);
                Array.Copy(untypedArray, typedArray, typedArray.Length);
                value = typedArray;
            }
            
            this.roleByRoleType[roleType] = value;
            this.changedRoleByRoleType[roleType] = value;
        }

        public void Add(RoleType roleType, ISessionObject value) {
            var roles = (ISessionObject[])this.Get(roleType);
            if (!roles.Contains(value)) {
                roles = new List<ISessionObject>(roles) { value }.ToArray();
            }

            this.Set(roleType, roles);
        }

        public void Remove(RoleType roleType, ISessionObject value) {
            var roles = (ISessionObject[])this.Get(roleType);
            if (roles.Contains(value))
            {
                var newRoles = new List<ISessionObject>(roles);
                newRoles.Remove(value);
                roles = newRoles.ToArray();
            }

            this.Set(roleType, roles);
        }

        public Data.PushRequestObject Save() {
            if (this.changedRoleByRoleType != null) {
                var data = new Data.PushRequestObject
                {
                    i = this.Id.ToString(),
                    v = this.Version.ToString(),
                    roles = this.SaveRoles()
                };

                return data;
            }

            return null;
        }

        public Data.PushRequestNewObject SaveNew() {
            var data = new Data.PushRequestNewObject
            {
                ni = this.NewId.ToString(),
                t = this.ObjectType.Name
            };

            if (this.changedRoleByRoleType != null) {
                data.roles = this.SaveRoles();
            }

            return data;
        }
        
        public void Reset() {
            if (this.WorkspaceObject != null) {
                this.WorkspaceObject = this.WorkspaceObject.Workspace.Get(this.Id);
            }

            this.changedRoleByRoleType = null;

            this.roleByRoleType = new Dictionary<RoleType, object>();
        }

        private Data.PushRequestRole[] SaveRoles()
        {
            var saveRoles = new List<Data.PushRequestRole>();

            foreach (var keyValuePair in this.changedRoleByRoleType)
            {
                var roleType = keyValuePair.Key;
                var role = keyValuePair.Value;

                var saveRole = new Data.PushRequestRole { t = roleType.PropertyName };

                if (roleType.ObjectType.IsUnit)
                {
                    saveRole.s = role;
                }
                else
                {
                    if (roleType.IsOne)
                    {
                        var sessionRole = (SessionObject)role;
                        saveRole.s = sessionRole?.Id.ToString() ?? sessionRole?.NewId?.ToString();
                    }
                    else
                    {
                        var sessionRoles = (SessionObject[])role;
                        var roleIds = sessionRoles.Select(item => item.Id.ToString()).ToArray();
                        if (this.NewId != null)
                        {
                            saveRole.a = roleIds;
                        }
                        else
                        {
                            object originalRoleIdsObject;
                            if (!this.WorkspaceObject.Roles.TryGetValue(roleType.PropertyName, out originalRoleIdsObject))
                            {
                                saveRole.a = roleIds;
                            }
                            else
                            {
                                if (originalRoleIdsObject != null)
                                {
                                    var originalRoleIds = ((IEnumerable<object>)originalRoleIdsObject)
                                        .Select(v => v.ToString())
                                        .ToArray();
                                    saveRole.a = roleIds.Except(originalRoleIds).ToArray();
                                    saveRole.r = originalRoleIds.Except(roleIds).ToArray();
                                }
                                else
                                {
                                    saveRole.a = roleIds.ToArray();
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
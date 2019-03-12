// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControlLists.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    public class AccessControlLists
    {
        private readonly PermissionCache permissionCache;

        private readonly User user;

        private readonly Dictionary<IObject, Permission[]> deniedPermissionsByObject;
        private readonly Dictionary<IObject, AccessControlCacheEntry[]> accessControlCacheEntriesByObject;

        public AccessControlLists(IEnumerable<IObject> objects, User user)
        {
            this.user = user;
            var session = user.Strategy.Session;

            this.permissionCache = session.GetCache<PermissionCache, PermissionCache>(() => new PermissionCache(session));

            var singleton = session.GetSingleton();
            var defaultSecurityTokens = new[] { singleton.DefaultSecurityToken };
            var defaultNewSecurityTokens = new[] { singleton.InitialSecurityToken ?? singleton.DefaultSecurityToken };

            var securityTokensByObject = new Dictionary<IObject, SecurityToken[]>();
            this.deniedPermissionsByObject = new Dictionary<IObject, Permission[]>();

            var securityTokens = new HashSet<SecurityToken>();

            foreach (var @object in objects.Cast<AccessControlledObject>())
            {
                SecurityToken[] objectSecurityTokens;
                Permission[] objectDeniedPermissions;
                if (@object is DelegatedAccessControlledObject controlledObject)
                {
                    var delegatedAccess = controlledObject.DelegateAccess();
                    objectSecurityTokens = delegatedAccess.SecurityTokens;

                    var delegatedAccessDeniedPermissions = delegatedAccess.DeniedPermissions;
                    objectDeniedPermissions = @object.DeniedPermissions.Any() ?
                                            delegatedAccessDeniedPermissions.Union(@object.DeniedPermissions).ToArray() :
                                            delegatedAccessDeniedPermissions.ToArray();
                }
                else
                {
                    objectSecurityTokens = @object.SecurityTokens;
                    objectDeniedPermissions = @object.DeniedPermissions.ToArray();
                }

                if (objectSecurityTokens == null || objectSecurityTokens.Length == 0)
                {
                    objectSecurityTokens = @object.Strategy.IsNewInSession ? defaultNewSecurityTokens : defaultSecurityTokens;
                }

                securityTokens.UnionWith(objectSecurityTokens);

                securityTokensByObject[@object] = objectSecurityTokens;
                this.deniedPermissionsByObject[@object] = objectDeniedPermissions;
            }

            var caches = session.GetCache<AccessControlCacheEntry>();
            var misses = securityTokens.SelectMany(v => v.AccessControls).Where(v => !caches.ContainsKey(v.Id)).Distinct().ToArray();

            if (misses.Length > 0)
            {
                session.Prefetch(Allors.Domain.AccessControlList.PrefetchPolicy, misses);
                foreach (var accessControl in misses)
                {
                    var cache = new AccessControlCacheEntry(accessControl);
                    caches[accessControl.Id] = cache;
                }
            }

            var accessControlCacheEntryByAccessControl = securityTokens.SelectMany(v => v.AccessControls).ToDictionary(v => v, v => caches[v.Id]);

            this.accessControlCacheEntriesByObject = new Dictionary<IObject, AccessControlCacheEntry[]>();
            foreach (var kvp in securityTokensByObject)
            {
                this.accessControlCacheEntriesByObject[kvp.Key] = kvp.Value
                    .SelectMany(v => v.AccessControls)
                    .Select(w => accessControlCacheEntryByAccessControl[w])
                    .Where(v => v.EffectiveUserIds.Contains(user.Id))
                    .ToArray();
            }
        }

        public IAccessControlList this[IObject @object] => new AccessControlList(this, @object);

        public class AccessControlList : IAccessControlList
        {
            private readonly AccessControlLists accessControlLists;

            private readonly Permission[] deniedPermissions;
            private readonly Dictionary<Guid, Dictionary<Operations, long>> permissionIdByOperationByOperandTypeId;

            private readonly AccessControlCacheEntry[] accessControlCacheEntries;

            internal AccessControlList(AccessControlLists accessControlLists, IObject @object)
            {
                this.accessControlLists = accessControlLists;

                this.deniedPermissions = accessControlLists.deniedPermissionsByObject[@object];
                this.permissionIdByOperationByOperandTypeId = accessControlLists.permissionCache.PermissionIdByOperationByOperandTypeIdByClassId[@object.Strategy.Class.Id];

                this.accessControlCacheEntries = this.accessControlLists.accessControlCacheEntriesByObject[@object];
            }

            public User User => this.accessControlLists.user;

            public bool CanRead(IPropertyType propertyType)
            {
                return this.IsPermitted(propertyType, Operations.Read);
            }

            public bool CanRead(ConcreteRoleType roleType)
            {
                return this.IsPermitted(roleType.RoleType, Operations.Read);
            }

            public bool CanWrite(IRoleType roleType)
            {
                return this.IsPermitted(roleType, Operations.Write);
            }

            public bool CanWrite(ConcreteRoleType roleType)
            {
                return this.IsPermitted(roleType.RoleType, Operations.Write);
            }

            public bool CanExecute(IMethodType methodType)
            {
                return this.IsPermitted(methodType, Operations.Execute);
            }

            public bool IsPermitted(IOperandType operandType, Operations operation)
            {
                if (this.deniedPermissions.Length > 0)
                {
                    if (this.deniedPermissions.Any(deniedPermission => deniedPermission.OperandTypePointer.Equals(operandType.Id) && deniedPermission.Operation.Equals(operation)))
                    {
                        return false;
                    }
                }

                if (this.permissionIdByOperationByOperandTypeId.TryGetValue(operandType.Id, out var permissionIdByOperation))
                {
                    if (permissionIdByOperation.TryGetValue(operation, out var permissionId))
                    {
                        if (this.accessControlCacheEntries.Any(v => v.EffectivePermissionIds.Contains(permissionId)))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}
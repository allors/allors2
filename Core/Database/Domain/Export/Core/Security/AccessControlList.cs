// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControlList.cs" company="Allors bvba">
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
    using Allors;
    using Allors.Meta;

    /// <summary>
    /// List of permissions for an object/user combination.
    /// </summary>
    public class AccessControlList : IAccessControlList
    {
        internal static readonly PrefetchPolicy PrefetchPolicy;

        private readonly Object @object;
        private readonly ISession session;

        private readonly Guid classId;

        private HashSet<long> permissionIds;
        private HashSet<long> deniedPermissions;

        private bool lazyLoaded;

        private Dictionary<Guid, Dictionary<Operations, long>> permissionIdByOperationByOperandTypeId;

        static AccessControlList()
        {
            PrefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(M.AccessControl.CacheId.RoleType)
                .WithRule(M.AccessControl.EffectiveUsers)
                .WithRule(M.AccessControl.EffectivePermissions)
                .Build();
        }

        public AccessControlList(IObject obj, User user)
        {
            this.User = user;
            this.session = this.User.Strategy.Session;
            this.@object = obj as Object;
            this.classId = this.@object?.Strategy.Class.Id ?? Guid.Empty;

            this.lazyLoaded = false;
        }

        public User User
        {
            get;
        }

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
            if (this.@object == null)
            {
                return operation == Operations.Read;
            }

            this.LazyLoad();

            if (this.permissionIdByOperationByOperandTypeId.TryGetValue(operandType.Id, out var permissionIdByOperation))
            {
                if (permissionIdByOperation.TryGetValue(operation, out var permissionId))
                {
                    if (this.deniedPermissions?.Contains(permissionId) == true)
                    {
                        return false;
                    }

                    return this.permissionIds.Contains(permissionId);
                }
            }

            return false;
        }

        private void LazyLoad()
        {
            if (!this.lazyLoaded)
            {
                SecurityToken[] securityTokens;
                if (this.@object is DelegatedAccessControlledObject controlledObject)
                {
                    var delegatedAccess = controlledObject.DelegateAccess();
                    securityTokens = delegatedAccess.SecurityTokens;

                    var delegatedAccessDeniedPermissions = delegatedAccess.DeniedPermissions;
                    if (delegatedAccessDeniedPermissions != null && delegatedAccessDeniedPermissions.Length > 0)
                    {
                        this.deniedPermissions = this.@object.DeniedPermissions.Count > 0 ?
                                                     new HashSet<long>(this.@object.DeniedPermissions.Union(delegatedAccessDeniedPermissions).Select(v => v.Id)) :
                                                     new HashSet<long>(delegatedAccessDeniedPermissions.Select(v => v.Id));
                    }
                    else if (this.@object.DeniedPermissions.Count > 0)
                    {
                        this.deniedPermissions = new HashSet<long>(this.@object.DeniedPermissions.Select(v => v.Id));
                    }
                }
                else
                {
                    securityTokens = this.@object.SecurityTokens;

                    if (this.@object.DeniedPermissions.Count > 0)
                    {
                        this.deniedPermissions = new HashSet<long>(this.@object.DeniedPermissions.Select(v => v.Id));
                    }
                }

                if (securityTokens == null || securityTokens.Length == 0)
                {
                    var singleton = this.session.GetSingleton();
                    securityTokens = this.@object.Strategy.IsNewInSession
                                          ? new[] { singleton.InitialSecurityToken ?? singleton.DefaultSecurityToken }
                                          : new[] { singleton.DefaultSecurityToken };
                }

                this.permissionIds = new HashSet<long>(this.Caches(securityTokens).SelectMany(v => v.EffectivePermissionIds));

                var permissionCache = this.session.GetCache<PermissionCache, PermissionCache>(() => new PermissionCache(this.session));
                this.permissionIdByOperationByOperandTypeId = permissionCache.PermissionIdByOperationByOperandTypeIdByClassId[this.classId];

                this.lazyLoaded = true;
            }
        }

        private IEnumerable<AccessControlCacheEntry> Caches(SecurityToken[] securityTokens)
        {
            List<AccessControl> misses = null;

            var caches = this.session.GetCache<AccessControlCacheEntry>();
            foreach (var accessControl in securityTokens.SelectMany(v => v.AccessControls).Distinct())
            {
                caches.TryGetValue(accessControl.Id, out var cache);
                if (cache == null || !accessControl.CacheId.Equals(cache.CacheId))
                {
                    if (misses == null)
                    {
                        misses = new List<AccessControl>();
                    }

                    misses.Add(accessControl);
                }
                else
                {
                    if (cache.EffectiveUserIds.Contains(this.User.Id))
                    {
                        yield return cache;
                    }
                }
            }

            if (misses != null)
            {
                if (misses.Count > 1)
                {
                    this.session.Prefetch(PrefetchPolicy, misses);
                }

                foreach (var accessControl in misses)
                {
                    var cache = new AccessControlCacheEntry(accessControl);
                    caches[accessControl.Id] = cache;

                    if (cache.EffectiveUserIds.Contains(this.User.Id))
                    {
                        yield return cache;
                    }
                }
            }
        }
    }
}

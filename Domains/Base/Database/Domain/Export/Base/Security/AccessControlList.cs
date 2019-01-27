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
        private static readonly PrefetchPolicy PrefetchPolicy;

        private readonly AccessControlledObject @object;
        private readonly ISession session;

        private readonly Guid classId;

        private HashSet<long> permissionIds;
        private Permission[] deniedPermissions;

        private bool lazyLoaded;

        private Dictionary<Guid, Dictionary<Operations, long>> PermissionIdByOperationByOperandTypeId;

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
            this.@object = (AccessControlledObject)obj;
            this.classId = obj.Strategy.Class.Id;

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

        public bool CanWrite(IRoleType roleType)
        {
            return this.IsPermitted(roleType, Operations.Write);
        }

        public bool CanExecute(IMethodType methodType)
        {
            return this.IsPermitted(methodType, Operations.Execute);
        }

        public bool IsPermitted(IOperandType operandType, Operations operation)
        {
            this.LazyLoad();

            if (this.deniedPermissions.Length > 0)
            {
                if (this.deniedPermissions.Any(deniedPermission => deniedPermission.OperandTypePointer.Equals(operandType.Id) && deniedPermission.Operation.Equals(operation)))
                {
                    return false;
                }
            }

            var permissionIdByOperation = this.PermissionIdByOperationByOperandTypeId[operandType.Id];
            if (permissionIdByOperation.TryGetValue(operation, out var permissionId))
            {
                return this.permissionIds.Contains(permissionId);
            }

            return false;
        }

        private void LazyLoad()
        {
            if (!this.lazyLoaded)
            {
                this.deniedPermissions = this.@object.DeniedPermissions.ToArray();

                SecurityToken[] securityTokens;
                if (this.@object is DelegatedAccessControlledObject controlledObject)
                {
                    var delegatedAccess = controlledObject.DelegateAccess();
                    securityTokens = delegatedAccess.SecurityTokens;

                    var delegatedAccessDeniedPermissions = delegatedAccess.DeniedPermissions;
                    if (delegatedAccessDeniedPermissions != null)
                    {
                        this.deniedPermissions = this.deniedPermissions.Union(delegatedAccessDeniedPermissions).ToArray();
                    }
                }
                else
                {
                    securityTokens = this.@object.SecurityTokens;
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
                this.PermissionIdByOperationByOperandTypeId = permissionCache.PermissionIdByOperationByOperandTypeIdByClassId[this.classId];

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
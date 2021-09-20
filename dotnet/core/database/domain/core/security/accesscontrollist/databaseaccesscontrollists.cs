// <copyright file="AccessControlListFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public class DatabaseAccessControlLists : IAccessControlLists
    {
        private static readonly PrefetchPolicy PrefetchPolicy = new PrefetchPolicyBuilder()
            .WithRule(M.AccessControl.CacheId.RoleType)
            .WithRule(M.AccessControl.EffectivePermissions)
            .Build();

        public DatabaseAccessControlLists(User user)
        {
            this.User = user;
            this.AclByObject = new Dictionary<IObject, IAccessControlList>();
            this.EffectivePermissionIdsByAccessControl = this.EffectivePermissionsByAccessControl();
        }

        public IEnumerable<AccessControl> AccessControls => this.AclByObject.SelectMany(v => v.Value.AccessControls).Distinct();

        public IReadOnlyDictionary<AccessControl, HashSet<long>> EffectivePermissionIdsByAccessControl { get; set; }

        public User User { get; }

        private Dictionary<IObject, IAccessControlList> AclByObject { get; }

        public IAccessControlList this[IObject @object]
        {
            get
            {
                if (!this.AclByObject.TryGetValue(@object, out var acl))
                {
                    acl = new AccessControlList(this, @object, false);
                    this.AclByObject.Add(@object, acl);
                }

                return acl;
            }
        }

        private Dictionary<AccessControl, HashSet<long>> EffectivePermissionsByAccessControl()
        {
            var session = this.User.Session();

            var effectivePermissionsByAccessControl = new Dictionary<AccessControl, HashSet<long>>();

            var caches = session.GetCache<AccessControlCacheEntry>();
            List<AccessControl> misses = null;
            foreach (AccessControl accessControl in this.User.AccessControlsWhereEffectiveUser)
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
                    effectivePermissionsByAccessControl.Add(accessControl, cache.EffectivePermissionIds);
                }
            }

            if (misses != null)
            {
                if (misses.Count > 1)
                {
                    session.Prefetch(PrefetchPolicy, misses);
                }

                foreach (var accessControl in misses)
                {
                    var cache = new AccessControlCacheEntry(accessControl);
                    caches[accessControl.Id] = cache;
                    effectivePermissionsByAccessControl.Add(accessControl, cache.EffectivePermissionIds);
                }
            }

            return effectivePermissionsByAccessControl;
        }
    }
}

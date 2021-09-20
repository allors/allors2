// <copyright file="RoleCache.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using System.Collections.Generic;
    using Meta;

    public class RoleCache : IRoleCache
    {
        private Dictionary<IRoleType, Dictionary<long, CachedUnitRole>> cachedUnitRoleByAssociationByRoleType;

        private Dictionary<IRoleType, Dictionary<long, CachedCompositeRole>> cachedCompositeRoleByAssociationByRoleType;

        private Dictionary<IRoleType, Dictionary<long, CachedCompositesRole>> cachedCompositesRoleByAssociationByRoleType;

        public RoleCache()
        {
            this.cachedUnitRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, CachedUnitRole>>();
            this.cachedCompositeRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, CachedCompositeRole>>();
            this.cachedCompositesRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, CachedCompositesRole>>();
        }

        public bool TryGetUnit(long association, object cacheId, IRoleType roleType, out object role)
        {
            if (this.cachedUnitRoleByAssociationByRoleType.TryGetValue(roleType, out var entryByAssociation) && entryByAssociation.TryGetValue(association, out var cachedUnitRole) && cachedUnitRole.CacheId.Equals(cacheId))
            {
                role = cachedUnitRole.Role;
                return true;
            }

            role = null;
            return false;
        }

        public void SetUnit(long association, object cacheId, IRoleType roleType, object role)
        {
            if (!this.cachedUnitRoleByAssociationByRoleType.TryGetValue(roleType, out var entryByAssociation))
            {
                entryByAssociation = new Dictionary<long, CachedUnitRole>();
                this.cachedUnitRoleByAssociationByRoleType[roleType] = entryByAssociation;
            }

            entryByAssociation[association] = new CachedUnitRole(cacheId, role);
        }

        public bool TryGetComposite(long association, object cacheId, IRoleType roleType, out long? role)
        {
            if (this.cachedCompositeRoleByAssociationByRoleType.TryGetValue(roleType, out var entryByAssociation) && entryByAssociation.TryGetValue(association, out var cachedCompositeRole) && cachedCompositeRole.CacheId.Equals(cacheId))
            {
                role = cachedCompositeRole.Role;
                return true;
            }

            role = null;
            return false;
        }

        public void SetComposite(long association, object cacheId, IRoleType roleType, long? role)
        {
            if (!this.cachedCompositeRoleByAssociationByRoleType.TryGetValue(roleType, out var entryByAssociation))
            {
                entryByAssociation = new Dictionary<long, CachedCompositeRole>();
                this.cachedCompositeRoleByAssociationByRoleType[roleType] = entryByAssociation;
            }

            entryByAssociation[association] = new CachedCompositeRole(cacheId, role);
        }

        public bool TryGetComposites(long association, object cacheId, IRoleType roleType, out long[] role)
        {
            if (this.cachedCompositesRoleByAssociationByRoleType.TryGetValue(roleType, out var entryByAssociation) && entryByAssociation.TryGetValue(association, out var cachedCompositesRole) && cachedCompositesRole.CacheId.Equals(cacheId))
            {
                role = cachedCompositesRole.Role;
                return true;
            }

            role = null;
            return false;
        }

        public void SetComposites(long association, object cacheId, IRoleType roleType, long[] role)
        {
            if (!this.cachedCompositesRoleByAssociationByRoleType.TryGetValue(roleType, out var entryByAssociation))
            {
                entryByAssociation = new Dictionary<long, CachedCompositesRole>();
                this.cachedCompositesRoleByAssociationByRoleType[roleType] = entryByAssociation;
            }

            entryByAssociation[association] = new CachedCompositesRole(cacheId, role);
        }

        public void Invalidate()
        {
            this.cachedUnitRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, CachedUnitRole>>();
            this.cachedCompositeRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, CachedCompositeRole>>();
            this.cachedCompositesRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, CachedCompositesRole>>();
        }

        public void Invalidate(long[] objectsToInvalidate)
        {
            foreach (var roleByAssociationEntry in this.cachedCompositeRoleByAssociationByRoleType)
            {
                var roleByAssociation = roleByAssociationEntry.Value;
                foreach (var objectToInvalidate in objectsToInvalidate)
                {
                    roleByAssociation.Remove(objectToInvalidate);
                }
            }

            foreach (var roleByAssociationEntry in this.cachedCompositeRoleByAssociationByRoleType)
            {
                var roleByAssociation = roleByAssociationEntry.Value;
                foreach (var objectToInvalidate in objectsToInvalidate)
                {
                    roleByAssociation.Remove(objectToInvalidate);
                }
            }

            foreach (var roleByAssociationEntry in this.cachedCompositesRoleByAssociationByRoleType)
            {
                var roleByAssociation = roleByAssociationEntry.Value;
                foreach (var objectToInvalidate in objectsToInvalidate)
                {
                    roleByAssociation.Remove(objectToInvalidate);
                }
            }
        }

        private class CachedUnitRole
        {
            internal CachedUnitRole(object cacheId, object role)
            {
                this.CacheId = cacheId;
                this.Role = role;
            }

            public object CacheId { get; }

            public object Role { get; }
        }

        private class CachedCompositeRole
        {
            internal CachedCompositeRole(object cacheId, long? role)
            {
                this.CacheId = cacheId;
                this.Role = role;
            }

            public object CacheId { get; }

            public long? Role { get; }
        }

        private class CachedCompositesRole
        {
            internal CachedCompositesRole(object cacheId, long[] role)
            {
                this.CacheId = cacheId;
                this.Role = role;
            }

            public object CacheId { get; }

            public long[] Role { get; }
        }
    }
}

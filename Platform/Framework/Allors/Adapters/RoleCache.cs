// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleCache.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters
{
    using System.Collections.Generic;
    using Allors.Meta;

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
            Dictionary<long, CachedUnitRole> entryByAssociation;
            if (this.cachedUnitRoleByAssociationByRoleType.TryGetValue(roleType, out entryByAssociation))
            {
                CachedUnitRole cachedUnitRole;
                if (entryByAssociation.TryGetValue(association, out cachedUnitRole))
                {
                    if (cachedUnitRole.CacheId.Equals(cacheId))
                    {
                        role = cachedUnitRole.Role;
                        return true;
                    }
                }
            }

            role = null;
            return false;
        }

        public void SetUnit(long association, object cacheId, IRoleType roleType, object role)
        {
            Dictionary<long, CachedUnitRole> entryByAssociation;
            if (!this.cachedUnitRoleByAssociationByRoleType.TryGetValue(roleType, out entryByAssociation))
            {
                entryByAssociation = new Dictionary<long, CachedUnitRole>();
                this.cachedUnitRoleByAssociationByRoleType[roleType] = entryByAssociation;
            }

            entryByAssociation[association] = new CachedUnitRole(cacheId, role);
        }

        public bool TryGetComposite(long association, object cacheId, IRoleType roleType, out long? role)
        {
            Dictionary<long, CachedCompositeRole> entryByAssociation;
            if (this.cachedCompositeRoleByAssociationByRoleType.TryGetValue(roleType, out entryByAssociation))
            {
                CachedCompositeRole cachedCompositeRole;
                if (entryByAssociation.TryGetValue(association, out cachedCompositeRole))
                {
                    if (cachedCompositeRole.CacheId.Equals(cacheId))
                    {
                        role = cachedCompositeRole.Role;
                        return true;
                    }
                }
            }

            role = null;
            return false;
        }

        public void SetComposite(long association, object cacheId, IRoleType roleType, long? role)
        {
            Dictionary<long, CachedCompositeRole> entryByAssociation;
            if (!this.cachedCompositeRoleByAssociationByRoleType.TryGetValue(roleType, out entryByAssociation))
            {
                entryByAssociation = new Dictionary<long, CachedCompositeRole>();
                this.cachedCompositeRoleByAssociationByRoleType[roleType] = entryByAssociation;
            }

            entryByAssociation[association] = new CachedCompositeRole(cacheId, role);
        }

        public bool TryGetComposites(long association, object cacheId, IRoleType roleType, out long[] role)
        {
            Dictionary<long, CachedCompositesRole> entryByAssociation;
            if (this.cachedCompositesRoleByAssociationByRoleType.TryGetValue(roleType, out entryByAssociation))
            {
                CachedCompositesRole cachedCompositesRole;
                if (entryByAssociation.TryGetValue(association, out cachedCompositesRole))
                {
                    if (cachedCompositesRole.CacheId.Equals(cacheId))
                    {
                        role = cachedCompositesRole.Role;
                        return true;
                    }
                }
            }

            role = null;
            return false;
        }

        public void SetComposites(long association, object cacheId, IRoleType roleType, long[] role)
        {
            Dictionary<long, CachedCompositesRole> entryByAssociation;
            if (!this.cachedCompositesRoleByAssociationByRoleType.TryGetValue(roleType, out entryByAssociation))
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
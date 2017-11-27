// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeSet.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the AllorsChangeSetMemory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Memory
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Adapters;

    public sealed class ChangeSet : IChangeSet
    {
        private readonly EmptySet<IRoleType> emptySet;

        private readonly HashSet<long> created;
        private readonly HashSet<long> deleted; 

        private readonly HashSet<long> associations;
        private readonly HashSet<long> roles;

        private readonly Dictionary<long, ISet<IRoleType>> roleTypesByAssociation;
        
        public ChangeSet()
        {
            this.emptySet = new EmptySet<IRoleType>(); 
            this.created = new HashSet<long>();
            this.deleted = new HashSet<long>();
            this.associations = new HashSet<long>();
            this.roles = new HashSet<long>();
            this.roleTypesByAssociation = new Dictionary<long, ISet<IRoleType>>();
        }

        public ISet<long> Created => this.created;

        public ISet<long> Deleted => this.deleted;

        public ISet<long> Associations => this.associations;

        public ISet<long> Roles => this.roles;

        public IDictionary<long, ISet<IRoleType>> RoleTypesByAssociation => this.roleTypesByAssociation;

        public ISet<IRoleType> GetRoleTypes(long association)
        {
            ISet<IRoleType> roleTypes;
            if (this.RoleTypesByAssociation.TryGetValue(association, out roleTypes))
            {
                return roleTypes;
            }

            return this.emptySet;
        }

        public void OnCreated(Strategy strategy)
        {
            this.created.Add(strategy.ObjectId);
        }

        public void OnDeleted(Strategy strategy)
        {
            this.deleted.Add(strategy.ObjectId);
        }

        public void OnChangingUnitRole(Strategy association, IRoleType roleType)
        {
            this.associations.Add(association.ObjectId);

            this.RoleTypes(association.ObjectId).Add(roleType);
        }

        public void OnChangingCompositeRole(Strategy association, IRoleType roleType, IObject previousRole, IObject newRole)
        {
            this.associations.Add(association.ObjectId);

            if (previousRole != null)
            {
                this.roles.Add(previousRole.Id);
            }

            if (newRole != null)
            {
                this.roles.Add(newRole.Id);
            }

            this.RoleTypes(association.ObjectId).Add(roleType);
        }

        public void OnChangingCompositesRole(Strategy association, IRoleType roleType, Strategy changedRole)
        {
            this.associations.Add(association.ObjectId);

            if (changedRole != null)
            {
                this.roles.Add(changedRole.ObjectId);
            }

            this.RoleTypes(association.ObjectId).Add(roleType);
        }

        public void OnChangingCompositesRole(Strategy association, IRoleType roleType, HashSet<Strategy> previousRoles)
        {
            this.associations.Add(association.ObjectId);

            if (previousRoles != null)
            {
                foreach (var role in previousRoles)
                {
                    this.roles.Add(role.ObjectId);
                }
            }

            this.RoleTypes(association.ObjectId).Add(roleType);
        }

        private ISet<IRoleType> RoleTypes(long associationId)
        {
            ISet<IRoleType> roleTypes;
            if (!this.RoleTypesByAssociation.TryGetValue(associationId, out roleTypes))
            {
                roleTypes = new HashSet<IRoleType>();
                this.RoleTypesByAssociation[associationId] = roleTypes;
            }

            return roleTypes;
        }
    }
}
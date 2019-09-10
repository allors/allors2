// <copyright file="ChangeSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsChangeSetMemory type.
// </summary>

namespace Allors.Adapters.Memory
{
    using System.Collections.Generic;

    using Allors.Adapters;

    using Allors.Meta;

    internal sealed class ChangeSet : IChangeSet
    {
        private static readonly EmptySet<IRoleType> EmptyRoleTypeSet = new EmptySet<IRoleType>();
        private static readonly EmptySet<IAssociationType> EmptyAssociationTypeSet = new EmptySet<IAssociationType>();

        private readonly HashSet<long> created;
        private readonly HashSet<long> deleted;

        private readonly HashSet<long> associations;
        private readonly HashSet<long> roles;

        private readonly Dictionary<long, ISet<IRoleType>> roleTypesByAssociation;

        private readonly Dictionary<long, ISet<IAssociationType>> associationTypesByRole;

        internal ChangeSet()
        {
            this.created = new HashSet<long>();
            this.deleted = new HashSet<long>();
            this.associations = new HashSet<long>();
            this.roles = new HashSet<long>();
            this.roleTypesByAssociation = new Dictionary<long, ISet<IRoleType>>();
            this.associationTypesByRole = new Dictionary<long, ISet<IAssociationType>>();
        }

        public ISet<long> Created => this.created;

        public ISet<long> Deleted => this.deleted;

        public ISet<long> Associations => this.associations;

        public ISet<long> Roles => this.roles;

        public IDictionary<long, ISet<IRoleType>> RoleTypesByAssociation => this.roleTypesByAssociation;

        public IDictionary<long, ISet<IAssociationType>> AssociationTypesByRole => this.associationTypesByRole;

        internal void OnCreated(long objectId) => this.created.Add(objectId);

        internal void OnDeleted(long objectId) => this.deleted.Add(objectId);

        internal void OnChangingUnitRole(long association, IRoleType roleType)
        {
            this.associations.Add(association);

            this.RoleTypes(association).Add(roleType);
        }

        internal void OnChangingCompositeRole(long association, IRoleType roleType, long? previousRole, long? newRole)
        {
            this.associations.Add(association);

            if (previousRole != null)
            {
                this.roles.Add(previousRole.Value);
                this.AssociationTypes(previousRole.Value).Add(roleType.AssociationType);
            }

            if (newRole != null)
            {
                this.roles.Add(newRole.Value);
                this.AssociationTypes(newRole.Value).Add(roleType.AssociationType);
            }

            this.RoleTypes(association).Add(roleType);
        }

        internal void OnChangingCompositesRole(long association, IRoleType roleType, Strategy changedRole)
        {
            this.associations.Add(association);

            if (changedRole != null)
            {
                this.roles.Add(changedRole.ObjectId);
                this.AssociationTypes(changedRole.ObjectId).Add(roleType.AssociationType);
            }

            this.RoleTypes(association).Add(roleType);
        }

        private ISet<IRoleType> RoleTypes(long associationId)
        {
            if (!this.RoleTypesByAssociation.TryGetValue(associationId, out var roleTypes))
            {
                roleTypes = new HashSet<IRoleType>();
                this.RoleTypesByAssociation[associationId] = roleTypes;
            }

            return roleTypes;
        }

        private ISet<IAssociationType> AssociationTypes(long roleId)
        {
            if (!this.AssociationTypesByRole.TryGetValue(roleId, out var associationTypes))
            {
                associationTypes = new HashSet<IAssociationType>();
                this.AssociationTypesByRole[roleId] = associationTypes;
            }

            return associationTypes;
        }
    }
}

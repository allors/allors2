// <copyright file="DerivationChangeSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors;
    using Allors.Meta;

    public class AccumulatedChangeSet : IAccumulatedChangeSet
    {
        internal AccumulatedChangeSet()
        {
            this.Created = new HashSet<long>();
            this.Deleted = new HashSet<long>();
            this.Associations = new HashSet<long>();
            this.Roles = new HashSet<long>();
            this.RoleTypesByAssociation = new Dictionary<long, ISet<IRoleType>>();
            this.AssociationTypesByRole = new Dictionary<long, ISet<IAssociationType>>();
        }

        public ISet<long> Created { get; }

        public ISet<long> Deleted { get; }

        public ISet<long> Associations { get; }

        public ISet<long> Roles { get; }

        public IDictionary<long, ISet<IRoleType>> RoleTypesByAssociation { get; }

        public IDictionary<long, ISet<IAssociationType>> AssociationTypesByRole { get; }

        public bool IsCreated(Object derivable) => this.Created.Contains(derivable.Id);

        public bool HasChangedRole(Object derivable, RoleType roleType)
        {
            this.RoleTypesByAssociation.TryGetValue(derivable.Id, out var changedRoleTypes);
            return changedRoleTypes?.Contains(roleType) ?? false;
        }

        public bool HasChangedRoles(Object derivable, params RoleType[] roleTypes)
        {
            this.RoleTypesByAssociation.TryGetValue(derivable.Id, out var changedRoleTypes);
            if (changedRoleTypes != null)
            {
                if (roleTypes.Length == 0 || roleTypes.Any(roleType => changedRoleTypes.Contains(roleType)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasChangedRoles(Object derivable, RelationKind relationKind)
        {
            Func<IRoleType, bool> check;
            switch (relationKind)
            {
                case RelationKind.Regular:
                    check = (roleType) => !roleType.RelationType.IsDerived && !roleType.RelationType.IsSynced;
                    break;

                case RelationKind.Derived:
                    check = (roleType) => roleType.RelationType.IsDerived;
                    break;

                case RelationKind.Synced:
                    check = (roleType) => roleType.RelationType.IsSynced;
                    break;

                default:
                    check = (roleType) => true;
                    break;
            }

            this.RoleTypesByAssociation.TryGetValue(derivable.Id, out var changedRoleTypes);
            if (changedRoleTypes != null)
            {
                if (changedRoleTypes.Any(roleType => check(roleType)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasChangedAssociation(Object derivable, AssociationType associationType)
        {
            this.AssociationTypesByRole.TryGetValue(derivable.Id, out var changedAssociationTypes);
            return changedAssociationTypes?.Contains(associationType) ?? false;
        }

        public bool HasChangedAssociations(Object derivable, params AssociationType[] associationTypes)
        {
            this.AssociationTypesByRole.TryGetValue(derivable.Id, out var changedAssociationTypes);
            if (changedAssociationTypes != null)
            {
                if (associationTypes.Length == 0 || associationTypes.Any(associationType => changedAssociationTypes.Contains(associationType)))
                {
                    return true;
                }
            }

            return false;
        }

        public void Add(IChangeSet changeSet)
        {
            this.Created.UnionWith(changeSet.Created);
            this.Deleted.UnionWith(changeSet.Deleted);
            this.Associations.UnionWith(changeSet.Associations);
            this.Roles.UnionWith(changeSet.Roles);

            foreach (var kvp in changeSet.RoleTypesByAssociation)
            {
                if (this.RoleTypesByAssociation.TryGetValue(kvp.Key, out var roleTypes))
                {
                    roleTypes.UnionWith(kvp.Value);
                }
                else
                {
                    this.RoleTypesByAssociation[kvp.Key] = new HashSet<IRoleType>(changeSet.RoleTypesByAssociation[kvp.Key]);
                }
            }

            foreach (var kvp in changeSet.AssociationTypesByRole)
            {
                if (this.AssociationTypesByRole.TryGetValue(kvp.Key, out var associationTypes))
                {
                    associationTypes.UnionWith(kvp.Value);
                }
                else
                {
                    this.AssociationTypesByRole[kvp.Key] = new HashSet<IAssociationType>(changeSet.AssociationTypesByRole[kvp.Key]);
                }
            }
        }
    }
}

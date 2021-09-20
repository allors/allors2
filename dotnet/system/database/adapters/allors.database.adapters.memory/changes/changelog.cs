// <copyright file="ChangeLog.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System.Collections.Generic;
    using System.Linq;
    using Collections;
    using Meta;

    internal sealed class ChangeLog
    {
        private readonly HashSet<Strategy> created;
        private readonly HashSet<IStrategy> deleted;

        private readonly Dictionary<Strategy, ISet<IRoleType>> roleTypesByAssociation;
        private readonly Dictionary<Strategy, ISet<IAssociationType>> associationTypesByRole;

        private readonly Dictionary<Strategy, Original> originalByStrategy;

        internal ChangeLog()
        {
            this.created = new HashSet<Strategy>();
            this.deleted = new HashSet<IStrategy>();

            this.roleTypesByAssociation = new Dictionary<Strategy, ISet<IRoleType>>();
            this.associationTypesByRole = new Dictionary<Strategy, ISet<IAssociationType>>();

            this.originalByStrategy = new Dictionary<Strategy, Original>();
        }

        internal void OnCreated(Strategy strategy) => this.created.Add(strategy);

        internal void OnDeleted(Strategy strategy) => this.deleted.Add(strategy);

        internal void OnChangingUnitRole(Strategy association, IRoleType roleType, object previousRole)
        {
            this.Original(association).OnChangingUnitRole(roleType, previousRole);

            this.RoleTypes(association).Add(roleType);
        }

        internal void OnChangingCompositeRole(Strategy association, IRoleType roleType, Strategy newRole, Strategy previousRole)
        {
            this.Original(association).OnChangingCompositeRole(roleType, previousRole);

            if (previousRole != null)
            {
                this.AssociationTypes(previousRole).Add(roleType.AssociationType);
            }

            if (newRole != null)
            {
                this.AssociationTypes(newRole).Add(roleType.AssociationType);
            }

            this.RoleTypes(association).Add(roleType);
        }

        internal void OnChangingCompositesRole(Strategy association, IRoleType roleType, Strategy changedRole, IEnumerable<Strategy> previousRole)
        {
            this.Original(association).OnChangingCompositesRole(roleType, previousRole);

            if (changedRole != null)
            {
                this.AssociationTypes(changedRole).Add(roleType.AssociationType);
            }

            this.RoleTypes(association).Add(roleType);
        }

        internal void OnChangingCompositeAssociation(Strategy role, IAssociationType associationType, Strategy previousAssociation)
            => this.Original(role).OnChangingCompositeAssociation(associationType, previousAssociation);

        internal void OnChangingCompositesAssociation(Strategy role, IAssociationType roleType, IEnumerable<Strategy> previousAssociation)
            => this.Original(role).OnChangingCompositesAssociation(roleType, previousAssociation);

        internal ChangeSet Checkpoint() =>
            new ChangeSet(
                this.created != null ? new HashSet<IObject>(this.created.Select(v => v.GetObject())) : null,
                this.deleted,
                this.RoleTypesByAssociation().ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                this.AssociationTypesByRole().ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            );

        private ISet<IRoleType> RoleTypes(Strategy associationId)
        {
            if (!this.roleTypesByAssociation.TryGetValue(associationId, out var roleTypes))
            {
                roleTypes = new HashSet<IRoleType>();
                this.roleTypesByAssociation[associationId] = roleTypes;
            }

            return roleTypes;
        }

        private ISet<IAssociationType> AssociationTypes(Strategy roleId)
        {
            if (!this.associationTypesByRole.TryGetValue(roleId, out var associationTypes))
            {
                associationTypes = new HashSet<IAssociationType>();
                this.associationTypesByRole[roleId] = associationTypes;
            }

            return associationTypes;
        }

        private Original Original(Strategy association)
        {
            if (this.originalByStrategy.TryGetValue(association, out var original))
            {
                return original;
            }

            original = new Original(association);
            this.originalByStrategy.Add(association, original);
            return original;
        }

        private IEnumerable<KeyValuePair<IObject, ISet<IRoleType>>> RoleTypesByAssociation()
        {
            foreach (var kvp in this.roleTypesByAssociation)
            {
                var strategy = kvp.Key;
                if (strategy.IsDeleted)
                {
                    continue;
                }

                var original = this.Original(kvp.Key);
                var roleTypes = kvp.Value;
                original.Trim(roleTypes);

                if (roleTypes.Count <= 0)
                {
                    continue;
                }

                var @object = strategy.GetObject();
                yield return new KeyValuePair<IObject, ISet<IRoleType>>(@object, roleTypes);
            }
        }

        private IEnumerable<KeyValuePair<IObject, ISet<IAssociationType>>> AssociationTypesByRole()
        {
            foreach (var kvp in this.associationTypesByRole)
            {
                var strategy = kvp.Key;
                if (strategy.IsDeleted)
                {
                    continue;
                }

                var original = this.Original(kvp.Key);
                var associationTypes = kvp.Value;
                original.Trim(associationTypes);

                if (associationTypes.Count <= 0)
                {
                    continue;
                }

                var @object = strategy.GetObject();
                yield return new KeyValuePair<IObject, ISet<IAssociationType>>(@object, associationTypes);
            }
        }
    }
}

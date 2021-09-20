// <copyright file="ChangeLog.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    internal sealed class Original
    {
        internal Original(Strategy strategy) => this.Strategy = strategy;

        internal Strategy Strategy { get; }

        internal Dictionary<IRoleType, object> OriginalUnitRoleByRoleType { get; private set; }

        internal Dictionary<IRoleType, Strategy> OriginalCompositeRoleByRoleType { get; private set; }

        internal Dictionary<IRoleType, Strategy[]> OriginalCompositesRoleByRoleType { get; private set; }

        internal Dictionary<IAssociationType, Strategy> OriginalCompositeAssociationByRoleType { get; private set; }

        internal Dictionary<IAssociationType, Strategy[]> OriginalCompositesAssociationByRoleType { get; private set; }

        internal void OnChangingUnitRole(IRoleType roleType, object previousRole)
        {
            this.OriginalUnitRoleByRoleType ??= new Dictionary<IRoleType, object>();

            if (!this.OriginalUnitRoleByRoleType.ContainsKey(roleType))
            {
                this.OriginalUnitRoleByRoleType.Add(roleType, previousRole);
            }
        }

        internal void OnChangingCompositeRole(IRoleType roleType, Strategy previousRole)
        {
            this.OriginalCompositeRoleByRoleType ??= new Dictionary<IRoleType, Strategy>();

            if (!this.OriginalCompositeRoleByRoleType.ContainsKey(roleType))
            {
                this.OriginalCompositeRoleByRoleType.Add(roleType, previousRole);
            }
        }

        internal void OnChangingCompositesRole(IRoleType roleType, IEnumerable<Strategy> previousRoles)
        {
            this.OriginalCompositesRoleByRoleType ??= new Dictionary<IRoleType, Strategy[]>();

            if (!this.OriginalCompositesRoleByRoleType.ContainsKey(roleType))
            {
                this.OriginalCompositesRoleByRoleType.Add(roleType, previousRoles?.ToArray());
            }
        }

        internal void OnChangingCompositeAssociation(IAssociationType associationType, Strategy previousAssociation)
        {
            this.OriginalCompositeAssociationByRoleType ??= new Dictionary<IAssociationType, Strategy>();

            if (!this.OriginalCompositeAssociationByRoleType.ContainsKey(associationType))
            {
                this.OriginalCompositeAssociationByRoleType.Add(associationType, previousAssociation);
            }
        }

        internal void OnChangingCompositesAssociation(IAssociationType associationType, IEnumerable<Strategy> previousAssociations)
        {
            this.OriginalCompositesAssociationByRoleType ??= new Dictionary<IAssociationType, Strategy[]>();

            if (!this.OriginalCompositesAssociationByRoleType.ContainsKey(associationType))
            {
                this.OriginalCompositesAssociationByRoleType.Add(associationType, previousAssociations?.ToArray());
            }
        }

        public void Trim(ISet<IRoleType> roleTypes)
        {
            foreach (var roleType in roleTypes.ToArray())
            {
                if (roleType.ObjectType.IsUnit)
                {
                    var originalRole = this.OriginalUnitRoleByRoleType[roleType];
                    if (this.Strategy.ShouldTrim(roleType, originalRole))
                    {
                        roleTypes.Remove(roleType);
                    }
                }
                else if (roleType.IsOne)
                {
                    var originalRole = this.OriginalCompositeRoleByRoleType[roleType];
                    if (this.Strategy.ShouldTrim(roleType, originalRole))
                    {
                        roleTypes.Remove(roleType);
                    }
                }
                else
                {
                    var originalRole = this.OriginalCompositesRoleByRoleType[roleType];
                    if (this.Strategy.ShouldTrim(roleType, originalRole))
                    {
                        roleTypes.Remove(roleType);
                    }
                }
            }
        }

        public void Trim(ISet<IAssociationType> associationTypes)
        {
            foreach (var associationType in associationTypes.ToArray())
            {
                if (associationType.IsOne)
                {
                    var originalAssociation = this.OriginalCompositeAssociationByRoleType[associationType];
                    if (this.Strategy.ShouldTrim(associationType, originalAssociation))
                    {
                        associationTypes.Remove(associationType);
                    }
                }
                else
                {
                    var originalAssociation = this.OriginalCompositesAssociationByRoleType[associationType];
                    if (this.Strategy.ShouldTrim(associationType, originalAssociation))
                    {
                        associationTypes.Remove(associationType);
                    }
                }
            }
        }
    }
}

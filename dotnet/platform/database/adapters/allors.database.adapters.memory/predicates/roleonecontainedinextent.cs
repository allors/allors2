// <copyright file="RoleOneContainedInExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using Adapters;
    using Allors.Meta;

    internal sealed class RoleOneContainedInExtent : Predicate
    {
        private readonly IRoleType roleType;
        private readonly Allors.Extent containingExtent;

        internal RoleOneContainedInExtent(ExtentFiltered extent, IRoleType roleType, Allors.Extent containingExtent)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleContainedIn(roleType, containingExtent);

            this.roleType = roleType;
            this.containingExtent = containingExtent;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var roleStrategy = strategy.GetCompositeRole(this.roleType.RelationType);

            if (roleStrategy == null)
            {
                return ThreeValuedLogic.False;
            }

            return this.containingExtent.Contains(roleStrategy)
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}

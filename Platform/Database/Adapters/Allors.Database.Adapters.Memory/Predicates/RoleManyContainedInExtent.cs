// <copyright file="RoleManyContainedInExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using Adapters;
    using Allors.Meta;

    internal sealed class RoleManyContainedInExtent : Predicate
    {
        private readonly IRoleType roleType;
        private readonly Allors.Extent containingExtent;

        internal RoleManyContainedInExtent(ExtentFiltered extent, IRoleType roleType, Allors.Extent containingExtent)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleContainedIn(roleType, containingExtent);

            this.roleType = roleType;
            this.containingExtent = containingExtent;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var roles = strategy.GetCompositeRoles(this.roleType.RelationType);

            if (roles.Count == 0)
            {
                return ThreeValuedLogic.False;
            }

            foreach (var role in roles)
            {
                if (this.containingExtent.Contains(role))
                {
                    return ThreeValuedLogic.True;
                }
            }

            return ThreeValuedLogic.False;
        }
    }
}

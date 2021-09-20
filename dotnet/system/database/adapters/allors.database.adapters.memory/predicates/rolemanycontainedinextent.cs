// <copyright file="RoleManyContainedInExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System.Linq;
    using Adapters;
    using Meta;

    internal sealed class RoleManyContainedInExtent : Predicate
    {
        private readonly IRoleType roleType;
        private readonly Allors.Database.Extent containingExtent;

        internal RoleManyContainedInExtent(ExtentFiltered extent, IRoleType roleType, Allors.Database.Extent containingExtent)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleContainedIn(roleType, containingExtent);

            this.roleType = roleType;
            this.containingExtent = containingExtent;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy) => strategy.GetCompositesRole<IObject>(this.roleType).Any(role => this.containingExtent.Contains(role)) ? ThreeValuedLogic.True : ThreeValuedLogic.False;
    }
}

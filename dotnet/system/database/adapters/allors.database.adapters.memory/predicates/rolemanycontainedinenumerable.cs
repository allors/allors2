// <copyright file="RoleManyContainedInEnumerable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System.Collections.Generic;
    using System.Linq;
    using Adapters;
    using Meta;

    internal sealed class RoleManyContainedInEnumerable : Predicate
    {
        private readonly IRoleType roleType;
        private readonly IEnumerable<IObject> containingEnumerable;

        internal RoleManyContainedInEnumerable(ExtentFiltered extent, IRoleType roleType, IEnumerable<IObject> containingEnumerable)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleContainedIn(roleType, containingEnumerable);

            this.roleType = roleType;
            this.containingEnumerable = containingEnumerable;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var containing = new HashSet<IObject>(this.containingEnumerable);

            return strategy.GetCompositesRole<IObject>(this.roleType).Any(role => containing.Contains(role)) ? ThreeValuedLogic.True : ThreeValuedLogic.False;
        }
    }
}

// <copyright file="RoleOneContainedInEnumerable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System.Collections.Generic;
    using Adapters;
    using Meta;

    internal sealed class RoleOneContainedInEnumerable : Predicate
    {
        private readonly IRoleType roleType;
        private readonly IEnumerable<IObject> containingEnumerable;

        internal RoleOneContainedInEnumerable(ExtentFiltered extent, IRoleType roleType, IEnumerable<IObject> containingEnumerable)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleContainedIn(roleType, containingEnumerable);

            this.roleType = roleType;
            this.containingEnumerable = containingEnumerable;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var containing = new HashSet<IObject>(this.containingEnumerable);
            var roleStrategy = strategy.GetCompositeRole(this.roleType);

            if (roleStrategy == null)
            {
                return ThreeValuedLogic.False;
            }

            return containing.Contains(roleStrategy)
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}


// <copyright file="RoleManyContainedInEnumerable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Memory
{
    using System.Collections.Generic;

    using Allors.Meta;
    using Adapters;

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

            var roles = strategy.GetCompositeRoles(this.roleType.RelationType);

            if (roles.Count == 0)
            {
                return ThreeValuedLogic.False;
            }

            foreach (var role in roles)
            {
                if (containing.Contains((IObject)role))
                {
                    return ThreeValuedLogic.True;
                }
            }

            return ThreeValuedLogic.False;
        }
    }
}

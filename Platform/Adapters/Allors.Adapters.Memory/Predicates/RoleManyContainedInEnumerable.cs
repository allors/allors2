// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleManyContainedInEnumerable.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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

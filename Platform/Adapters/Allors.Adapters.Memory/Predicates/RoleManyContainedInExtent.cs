// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleManyContainedInExtent.cs" company="Allors bvba">
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
    using Allors.Meta;
    using Adapters;

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
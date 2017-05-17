// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleLessThan.cs" company="Allors bvba">
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
    using System;
    using Allors.Meta;
    using Adapters;

    internal sealed class RoleLessThan : Predicate
    {
        private readonly ExtentFiltered extent;
        private readonly IRoleType roleType;
        private readonly object compare;

        internal RoleLessThan(ExtentFiltered extent, IRoleType roleType, object compare)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleLessThan(roleType, compare);

            this.extent = extent;
            this.roleType = roleType;
            this.compare = compare;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var compareValue = this.compare;

            var compareRole = this.compare as IRoleType;
            if (compareRole != null)
            {
                compareValue = strategy.GetInternalizedUnitRole(compareRole);
            }
            else
            {
                if (this.roleType.ObjectType is IUnit)
                {
                    compareValue = this.roleType.Normalize(this.compare);
                }
            }

            var comparable = strategy.GetInternalizedUnitRole(this.roleType) as IComparable;

            if (comparable == null)
            {
                return ThreeValuedLogic.Unknown;
            }

            return comparable.CompareTo(compareValue) < 0
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}
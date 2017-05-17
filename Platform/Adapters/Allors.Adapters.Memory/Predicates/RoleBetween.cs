// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleBetween.cs" company="Allors bvba">
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

    internal sealed class RoleBetween : Predicate
    {
        private readonly ExtentFiltered extent;
        private readonly IRoleType roleType;
        private readonly object first;
        private readonly object second;

        internal RoleBetween(ExtentFiltered extent, IRoleType roleType, object first, object second)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleBetween(roleType, first, second);

            this.extent = extent;
            this.roleType = roleType;

            this.first = first;
            this.second = second;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var firstValue = this.first;
            var secondValue = this.second;

            var firstRole = this.first as IRoleType;
            if (firstRole != null)
            {
                firstValue = strategy.GetInternalizedUnitRole(firstRole);
            }
            else
            {
                if (this.roleType.ObjectType is IUnit)
                {
                    firstValue = this.roleType.Normalize(this.first);
                }
            }

            var secondRole = this.second as IRoleType;
            if (secondRole != null)
            {
                secondValue = strategy.GetInternalizedUnitRole(secondRole);
            }
            else
            {
                if (this.roleType.ObjectType is IUnit)
                {
                    secondValue = this.roleType.Normalize(this.second);
                }
            }

            var comparable = strategy.GetInternalizedUnitRole(this.roleType) as IComparable;

            if (comparable == null)
            {
                return ThreeValuedLogic.Unknown;
            }

            return (comparable.CompareTo(firstValue) >= 0 && comparable.CompareTo(secondValue) <= 0)
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}
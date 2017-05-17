// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleCompositeEqualsValue.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Memory
{
    using Allors.Meta;
    using Adapters;

    internal sealed class RoleCompositeEqualsValue : Predicate
    {
        private readonly IRoleType roleType;
        private readonly object equals;

        internal RoleCompositeEqualsValue(ExtentFiltered extent, IRoleType roleType, object equals)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleEquals(roleType, equals);

            this.roleType = roleType;
            this.equals = equals;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            object value = strategy.GetCompositeRole(this.roleType.RelationType);

            if (value == null)
            {
                return ThreeValuedLogic.False;
            }

            object equalsValue = this.equals;

            if (this.equals is IRoleType)
            {
                var equalsRole = (IRoleType)this.equals;
                equalsValue = strategy.GetCompositeRole(equalsRole.RelationType);
            }

            if (equalsValue == null)
            {
                return ThreeValuedLogic.False;
            }

            return value.Equals(equalsValue)
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}
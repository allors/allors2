// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleInstanceOf.cs" company="Allors bvba">
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

    internal sealed class RoleInstanceof : Predicate
    {
        private readonly IRoleType roleType;
        private readonly IComposite objectType;

        internal RoleInstanceof(ExtentFiltered extent, IRoleType roleType, IComposite objectType)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleInstanceOf(roleType, objectType);

            this.roleType = roleType;
            this.objectType = objectType;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var role = strategy.GetCompositeRole(this.roleType.RelationType);

            if (role == null)
            {
                return ThreeValuedLogic.False;
            }

            // TODO: Optimize
            var roleObjectType = role.Strategy.Class;
            if (roleObjectType.Equals(this.objectType))
            {
                return ThreeValuedLogic.True;
            }

            var @interface = this.objectType as IInterface;
            return (@interface != null && roleObjectType.ExistSupertype(@interface))
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}

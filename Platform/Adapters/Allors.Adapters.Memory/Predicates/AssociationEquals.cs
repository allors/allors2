// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationEquals.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

using Allors;

namespace Allors.Adapters.Memory
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AssociationEquals : Predicate
    {
        private readonly IAssociationType associationType;
        private readonly IObject equals;

        internal AssociationEquals(ExtentFiltered extent, IAssociationType associationType, IObject equals)
        {
            extent.CheckForAssociationType(associationType);
            PredicateAssertions.AssertAssociationEquals(associationType, equals);

            this.associationType = associationType;
            this.equals = equals;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var association = strategy.GetCompositeAssociation(this.associationType.RelationType);
            return (association != null && association.Equals(this.equals))
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}
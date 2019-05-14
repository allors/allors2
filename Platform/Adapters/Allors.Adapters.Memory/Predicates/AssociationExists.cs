// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationExists.cs" company="Allors bvba">
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

    internal sealed class AssociationExists : Predicate
    {
        private readonly IAssociationType associationType;

        internal AssociationExists(ExtentFiltered extent, IAssociationType associationType)
        {
            extent.CheckForAssociationType(associationType);
            PredicateAssertions.ValidateAssociationExists(associationType);

            this.associationType = associationType;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            if (this.associationType.IsMany)
            {
                return strategy.ExistCompositeAssociations(this.associationType.RelationType)
                        ? ThreeValuedLogic.True
                        : ThreeValuedLogic.False;
            }

            return strategy.ExistCompositeAssociation(this.associationType.RelationType)
                    ? ThreeValuedLogic.True
                    : ThreeValuedLogic.False;
        }
    }
}
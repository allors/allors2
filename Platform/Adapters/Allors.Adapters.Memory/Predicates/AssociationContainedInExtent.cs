// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationContainedInExtent.cs" company="Allors bvba">
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

namespace Allors.Adapters.Memory
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AssociationContainedInExtent : Predicate
    {
        private readonly IAssociationType associationType;
        private readonly Allors.Extent containingExtent;

        internal AssociationContainedInExtent(ExtentFiltered extent, IAssociationType associationType, Allors.Extent containingExtent)
        {
            extent.CheckForAssociationType(associationType);
            PredicateAssertions.AssertAssociationContainedIn(associationType, containingExtent);

            this.associationType = associationType;
            this.containingExtent = containingExtent;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            if (this.associationType.IsMany)
            {
                var associations = strategy.GetCompositeAssociations(this.associationType.RelationType);
                foreach (var assoc in associations)
                {
                    if (this.containingExtent.Contains(assoc))
                    {
                        return ThreeValuedLogic.True;
                    }
                }

                return ThreeValuedLogic.False;
            }

            var association = strategy.GetCompositeAssociation(this.associationType.RelationType);
            if (association != null)
            {
                return this.containingExtent.Contains(association)
                           ? ThreeValuedLogic.True
                           : ThreeValuedLogic.False;
            }

            return ThreeValuedLogic.False;
        }
    }
}
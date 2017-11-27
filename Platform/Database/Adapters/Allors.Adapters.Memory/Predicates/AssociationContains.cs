// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationContains.cs" company="Allors bvba">
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

using Allors;

namespace Allors.Adapters.Memory
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AssociationContains : Predicate
    {
        private readonly IAssociationType associationType;
        private readonly IObject containedObject;

        internal AssociationContains(ExtentFiltered extent, IAssociationType associationType, IObject containedObject)
        {
            extent.CheckForAssociationType(associationType);
            PredicateAssertions.AssertAssociationContains(associationType, containedObject);

            this.associationType = associationType;
            this.containedObject = containedObject;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var associations = strategy.GetCompositeAssociations(this.associationType.RelationType);
            if (associations != null)
            {
                foreach (var association in associations)
                {
                    if (association.Equals(this.containedObject))
                    {
                        return ThreeValuedLogic.True;
                    }
                }
            }

            return ThreeValuedLogic.False;
        }
    }
}
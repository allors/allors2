// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationContainedInEnumerable.cs" company="Allors bvba">
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
    using System.Collections.Generic;

    using Allors.Meta;
    using Adapters;

    internal sealed class AssociationContainedInEnumerable : Predicate
    {
        private readonly IAssociationType associationType;
        private readonly IEnumerable<IObject> containingEnumerable;

        internal AssociationContainedInEnumerable(ExtentFiltered extent, IAssociationType associationType, IEnumerable<IObject> containingEnumerable)
        {
            extent.CheckForAssociationType(associationType);
            PredicateAssertions.AssertAssociationContainedIn(associationType, containingEnumerable);

            this.associationType = associationType;
            this.containingEnumerable = containingEnumerable;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var containing = new HashSet<IObject>(this.containingEnumerable);

            if (this.associationType.IsMany)
            {
                var associations = strategy.GetCompositeAssociations(this.associationType.RelationType);
                foreach (var assoc in associations)
                {
                    if (containing.Contains((IObject)assoc))
                    {
                        return ThreeValuedLogic.True;
                    }
                }

                return ThreeValuedLogic.False;
            }

            var association = strategy.GetCompositeAssociation(this.associationType.RelationType);
            if (association != null)
            {
                return containing.Contains(association)
                           ? ThreeValuedLogic.True
                           : ThreeValuedLogic.False;
            }

            return ThreeValuedLogic.False;
        }
    }
}
// <copyright file="AssociationContainedInEnumerable.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System.Collections.Generic;
    using Adapters;
    using Allors.Meta;

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

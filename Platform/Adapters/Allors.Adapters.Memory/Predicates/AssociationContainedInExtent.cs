// <copyright file="AssociationContainedInExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Memory
{
    using Allors.Adapters;
    using Allors.Meta;

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

// <copyright file="AssociationContains.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Memory
{
    using Allors.Adapters;
    using Allors.Meta;

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

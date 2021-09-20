// <copyright file="AssociationExists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using Adapters;
    using Meta;

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
                return strategy.ExistCompositesAssociation(this.associationType)
                        ? ThreeValuedLogic.True
                        : ThreeValuedLogic.False;
            }

            return strategy.ExistCompositeAssociation(this.associationType)
                    ? ThreeValuedLogic.True
                    : ThreeValuedLogic.False;
        }
    }
}

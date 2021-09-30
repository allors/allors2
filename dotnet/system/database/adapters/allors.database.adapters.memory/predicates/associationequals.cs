// <copyright file="AssociationEquals.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using Adapters;
    using Meta;

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
            var association = strategy.GetCompositeAssociation(this.associationType);
            return association?.Equals(this.@equals) == true
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}

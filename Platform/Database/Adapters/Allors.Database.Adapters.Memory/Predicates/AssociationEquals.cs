// <copyright file="AssociationEquals.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using Adapters;
    using Allors.Meta;

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

// <copyright file="AssociationContains.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System.Linq;
    using Adapters;
    using Meta;

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

        internal override ThreeValuedLogic Evaluate(Strategy strategy) => strategy.GetCompositesAssociation<IObject>(this.associationType).Contains(this.containedObject) ? ThreeValuedLogic.True : ThreeValuedLogic.False;
    }
}

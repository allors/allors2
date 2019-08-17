// <copyright file="AssociationInstanceOf.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Memory
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AssociationInstanceOf : Predicate
    {
        private readonly IAssociationType associationType;
        private readonly IObjectType objectType;

        internal AssociationInstanceOf(ExtentFiltered extent, IAssociationType associationType, IObjectType instanceObjectType)
        {
            extent.CheckForAssociationType(associationType);
            PredicateAssertions.ValidateAssociationInstanceof(associationType, instanceObjectType);

            this.associationType = associationType;
            this.objectType = instanceObjectType;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var association = strategy.GetCompositeAssociation(this.associationType.RelationType);

            if (association == null)
            {
                return ThreeValuedLogic.False;
            }

            // TODO: Optimize
            var associationObjectType = association.Strategy.Class;
            if (associationObjectType.Equals(this.objectType))
            {
                return ThreeValuedLogic.True;
            }

            var @interface = this.objectType as IInterface;
            return (@interface != null && associationObjectType.ExistSupertype(@interface))
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}

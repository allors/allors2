// <copyright file="InstanceOf.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using Adapters;
    using Meta;

    internal sealed class Instanceof : Predicate
    {
        private readonly IObjectType objectType;

        internal Instanceof(IObjectType objectType)
        {
            PredicateAssertions.ValidateInstanceof(objectType);

            this.objectType = objectType;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            if (strategy.UncheckedObjectType.Equals(this.objectType))
            {
                return ThreeValuedLogic.True;
            }

            return this.objectType is IInterface @interface && strategy.UncheckedObjectType.ExistSupertype(@interface)
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}

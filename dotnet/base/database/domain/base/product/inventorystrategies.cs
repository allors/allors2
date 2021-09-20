// <copyright file="InventoryStrategies.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Allors.Meta;

    public partial class InventoryStrategies
    {
        private static readonly Guid StandardId = new Guid("9DF77458-63C9-48FB-A100-1249B17C7945");

        private UniquelyIdentifiableSticky<InventoryStrategy> cache;

        public InventoryStrategy Standard => this.Cache[StandardId];

        private UniquelyIdentifiableSticky<InventoryStrategy> Cache => this.cache ??= new UniquelyIdentifiableSticky<InventoryStrategy>(this.Session);

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.SerialisedInventoryItemState);
            setup.AddDependency(this.Meta.ObjectType, M.NonSerialisedInventoryItemState);
        }

        protected override void BaseSetup(Setup setup)
        {
            var nonSerialisedStates = new NonSerialisedInventoryItemStates(this.Session);
            var serialisedStates = new SerialisedInventoryItemStates(this.Session);

            var merge = this.Cache.Merger().Action();

            merge(StandardId, v =>
            {
                v.Name = "Standard Inventory Strategy";

                v.AddAvailableToPromiseNonSerialisedState(nonSerialisedStates.Good);

                v.AddOnHandNonSerialisedState(nonSerialisedStates.Good);
                v.AddOnHandNonSerialisedState(nonSerialisedStates.SlightlyDamaged);
                v.AddOnHandNonSerialisedState(nonSerialisedStates.Defective);
                v.AddOnHandNonSerialisedState(nonSerialisedStates.Scrap);

                v.AddAvailableToPromiseSerialisedState(serialisedStates.Good);

                // Exclude serialisedStates.Sold
                // Exclude serialisedStates.InRent
                v.AddOnHandSerialisedState(serialisedStates.Good);
                v.AddOnHandSerialisedState(serialisedStates.SlightlyDamaged);
                v.AddOnHandSerialisedState(serialisedStates.Defective);
                v.AddOnHandSerialisedState(serialisedStates.Scrap);
            });
        }
    }
}

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

        private UniquelyIdentifiableSticky<InventoryStrategy> stateCache;

        public InventoryStrategy Standard => this.StateCache[StandardId];

        private UniquelyIdentifiableSticky<InventoryStrategy> StateCache => this.stateCache ??= new UniquelyIdentifiableSticky<InventoryStrategy>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var nonSerialisedStates = new NonSerialisedInventoryItemStates(this.Session);
            var serialisedStates = new SerialisedInventoryItemStates(this.Session);

            new InventoryStrategyBuilder(this.Session)
                .WithUniqueId(StandardId)
                .WithName("Standard Inventory Strategy")
                .Build();

            this.Standard.AddAvailableToPromiseNonSerialisedState(nonSerialisedStates.Good);

            this.Standard.AddOnHandNonSerialisedState(nonSerialisedStates.Good);
            this.Standard.AddOnHandNonSerialisedState(nonSerialisedStates.BeingRepaired);
            this.Standard.AddOnHandNonSerialisedState(nonSerialisedStates.SlightlyDamaged);
            this.Standard.AddOnHandNonSerialisedState(nonSerialisedStates.Defective);
            this.Standard.AddOnHandNonSerialisedState(nonSerialisedStates.Scrap);

            this.Standard.AddAvailableToPromiseSerialisedState(serialisedStates.Good);
            this.Standard.AddAvailableToPromiseSerialisedState(serialisedStates.Available);

            this.Standard.AddOnHandSerialisedState(serialisedStates.Good);
            this.Standard.AddOnHandSerialisedState(serialisedStates.BeingRepaired);
            this.Standard.AddOnHandSerialisedState(serialisedStates.SlightlyDamaged);
            this.Standard.AddOnHandSerialisedState(serialisedStates.Defective);
            this.Standard.AddOnHandSerialisedState(serialisedStates.Scrap);
            this.Standard.AddOnHandSerialisedState(serialisedStates.Available);
            // Exclude serialisedStates.Sold
            // Exclude serialisedStates.InRent
            this.Standard.AddOnHandSerialisedState(serialisedStates.Assigned);
        }

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.SerialisedInventoryItemState);
            setup.AddDependency(this.Meta.ObjectType, M.NonSerialisedInventoryItemState);
        }
    }
}

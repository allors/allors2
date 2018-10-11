// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryItemState.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using Meta;
    using System;

    public partial class InventoryStrategies
    {
        private static readonly Guid StandardId = new Guid("9DF77458-63C9-48FB-A100-1249B17C7945");

        private UniquelyIdentifiableSticky<InventoryStrategy> stateCache;

        public InventoryStrategy Standard => this.StateCache[StandardId];

        private UniquelyIdentifiableSticky<InventoryStrategy> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<InventoryStrategy>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var inventoryStates = new InventoryItemStates(this.Session);

            new InventoryStrategyBuilder(this.Session)
                .WithUniqueId(StandardId)
                .WithName("Standard Inventory Strategy")
                .Build();

            this.Standard.AddAvailableToPromiseNonSerialisedState(inventoryStates.Good);

            this.Standard.AddOnHandNonSerialisedState(inventoryStates.Good);
            this.Standard.AddOnHandNonSerialisedState(inventoryStates.BeingRepaired);
            this.Standard.AddOnHandNonSerialisedState(inventoryStates.SlightlyDamaged);
            this.Standard.AddOnHandNonSerialisedState(inventoryStates.Defective);
            this.Standard.AddOnHandNonSerialisedState(inventoryStates.Scrap);

            this.Standard.AddAvailableToPromiseSerialisedState(inventoryStates.Good);
            this.Standard.AddAvailableToPromiseSerialisedState(inventoryStates.Available);

            this.Standard.AddOnHandSerialisedState(inventoryStates.Good);
            this.Standard.AddOnHandSerialisedState(inventoryStates.BeingRepaired);
            this.Standard.AddOnHandSerialisedState(inventoryStates.SlightlyDamaged);
            this.Standard.AddOnHandSerialisedState(inventoryStates.Defective);
            this.Standard.AddOnHandSerialisedState(inventoryStates.Scrap);
            this.Standard.AddOnHandSerialisedState(inventoryStates.Available);
            // Exclude serialisedStates.Sold
            // Exclude serialisedStates.InRent
            this.Standard.AddOnHandSerialisedState(inventoryStates.Assigned);
        }

        protected override void AppsPrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.InventoryItemState);
        }
    }
}
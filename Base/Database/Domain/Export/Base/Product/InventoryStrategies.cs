// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonSerialisedInventoryItemStates.cs" company="Allors bvba">
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
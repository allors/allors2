// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerShipmentObjectStates.cs" company="Allors bvba">
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
    using System;

    public partial class CustomerShipmentObjectStates
    {
        private static readonly Guid CreatedId = new Guid("854AD6A0-B2D1-4b92-8C3D-E9E72DD19AFD");
        private static readonly Guid PickedId = new Guid("C63C5D25-F139-490f-86D1-2E9E51F5C0A5");
        private static readonly Guid PackedId = new Guid("DCABE845-A6F2-49d9-BBAE-06FB47012A21");
        private static readonly Guid ShippedId = new Guid("B8B115A4-6E5D-4400-BCA7-4224AE1708AA");
        private static readonly Guid DeliveredId = new Guid("B30666C1-9954-4ae1-8F94-A1591B7E35ED");
        private static readonly Guid CancelledId = new Guid("1F50B912-C778-4c99-84F9-12DACA1E54C1");
        private static readonly Guid OnHoldId = new Guid("268CB9A7-6965-47E8-AF89-8F915242C23D");

        private UniquelyIdentifiableCache<CustomerShipmentObjectState> stateCache;

        public CustomerShipmentObjectState Created => this.StateCache.Get(CreatedId);

        public CustomerShipmentObjectState Picked => this.StateCache.Get(PickedId);

        public CustomerShipmentObjectState Packed => this.StateCache.Get(PackedId);

        public CustomerShipmentObjectState Shipped => this.StateCache.Get(ShippedId);

        public CustomerShipmentObjectState Delivered => this.StateCache.Get(DeliveredId);

        public CustomerShipmentObjectState Cancelled => this.StateCache.Get(CancelledId);

        public CustomerShipmentObjectState OnHold => this.StateCache.Get(OnHoldId);

        private UniquelyIdentifiableCache<CustomerShipmentObjectState> StateCache => this.stateCache
                                                                                     ?? (this.stateCache = new UniquelyIdentifiableCache<CustomerShipmentObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new CustomerShipmentObjectStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new CustomerShipmentObjectStateBuilder(this.Session)
                .WithUniqueId(PickedId)
                .WithName("Picked")
                .Build();

            new CustomerShipmentObjectStateBuilder(this.Session)
                .WithUniqueId(PackedId)
                .WithName("Packed")
                .Build();

            new CustomerShipmentObjectStateBuilder(this.Session)
                .WithUniqueId(ShippedId)
                .WithName("Shipped")
                .Build();

            new CustomerShipmentObjectStateBuilder(this.Session)
                .WithUniqueId(DeliveredId)
                .WithName("Delivered")
                .Build();

            new CustomerShipmentObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new CustomerShipmentObjectStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On hold")
                .Build();
        }
    }
}
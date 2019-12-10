// <copyright file="InventoryTransactionReasons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class InventoryTransactionReasons
    {
        private static readonly Guid SalesOrderId = new Guid("81D45DC9-03D8-480b-B210-1CC641A9CBD3");
        private static readonly Guid OutgoingShipmentId = new Guid("C768FA78-0257-4e2d-B760-EC022D89BACF");
        private static readonly Guid IncomingShipmentId = new Guid("DBE99FCD-E81C-4AA6-97F2-F0E2E5B44D6B");
        private static readonly Guid TheftId = new Guid("21FA1D1D-F662-4c0e-A4F6-7BD3B5298A47");
        private static readonly Guid ShrinkageId = new Guid("CF6CCC79-7EE8-4755-A9C3-EC9A83649B55");
        private static readonly Guid UnknownId = new Guid("7A438996-B2DC-4b6d-8DDD-47690B06D9B6");
        private static readonly Guid StateId = new Guid("6790C5D4-7CC6-43c9-9CAC-48227021E7E9");
        private static readonly Guid PhysicalCountId = new Guid("971D0321-A86D-450C-ADAA-18B3C2114714");
        private static readonly Guid ReservationId = new Guid("D7785657-3771-40D3-9295-44DD3FC4FCC4");
        private static readonly Guid ConsumptionId = new Guid("FDDFE460-21E9-4A9E-808F-AB7B8E50F0A9");

        private UniquelyIdentifiableSticky<InventoryTransactionReason> cache;

        public InventoryTransactionReason SalesOrder => this.Cache[SalesOrderId];

        public InventoryTransactionReason OutgoingShipment => this.Cache[OutgoingShipmentId];

        public InventoryTransactionReason IncomingShipment => this.Cache[IncomingShipmentId];

        public InventoryTransactionReason Theft => this.Cache[TheftId];

        public InventoryTransactionReason Shrinkage => this.Cache[ShrinkageId];

        public InventoryTransactionReason Unknown => this.Cache[UnknownId];

        public InventoryTransactionReason State => this.Cache[StateId];

        public InventoryTransactionReason PhysicalCount => this.Cache[PhysicalCountId];

        public InventoryTransactionReason Reservation => this.Cache[ReservationId];

        public InventoryTransactionReason Consumption => this.Cache[ConsumptionId];

        private UniquelyIdentifiableSticky<InventoryTransactionReason> Cache => this.cache ??= new UniquelyIdentifiableSticky<InventoryTransactionReason>(this.Session);

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.SerialisedInventoryItemState);
            setup.AddDependency(this.Meta.ObjectType, M.NonSerialisedInventoryItemState);
        }

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;
            var serialisedStates = new SerialisedInventoryItemStates(this.Session);
            var nonSerialisedStates = new NonSerialisedInventoryItemStates(this.Session);

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(SalesOrderId, v =>
            {
                v.Name = "Sales Order";
                localisedName.Set(v, dutchLocale, "Bestelling");
                v.IsActive = true;
                v.IsManualEntryAllowed = false;
                v.IncreasesQuantityCommittedOut = true; // Increases Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = null; // Does not affect Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Good;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Good;
            });

            merge(OutgoingShipmentId, v =>
            {
                v.Name = "Outbound Shipment";
                localisedName.Set(v, dutchLocale, "Verscheping uitgaand");
                v.IsActive = true;
                v.IsManualEntryAllowed = false;
                v.IncreasesQuantityCommittedOut = false; // Decreases Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = false; // Decreases Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Good;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Good;
            });

            merge(IncomingShipmentId, v =>
            {
                v.Name = "Inbound Shipment";
                localisedName.Set(v, dutchLocale, "Verscheping inkomend");
                v.IsActive = true;
                v.IsManualEntryAllowed = false;
                v.IncreasesQuantityCommittedOut = null; // Decreases Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = true; // Decreases Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Good;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Good;
            });

            merge(TheftId, v =>
            {
                v.Name = "Theft";
                localisedName.Set(v, dutchLocale, "Diefstal");
                v.IsActive = true;
                v.IsManualEntryAllowed = true;
                v.IncreasesQuantityCommittedOut = null; // Does not affect Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = false; // Decreases Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Good;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Good;
            });

            merge(ShrinkageId, v =>
            {
                v.Name = "Theft";
                localisedName.Set(v, dutchLocale, "Shrinkage");
                v.IsActive = true;
                v.IsManualEntryAllowed = true;
                v.IncreasesQuantityCommittedOut = null; // Does not affect Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = false; // Decreases Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Good;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Good;
            });

            merge(UnknownId, v =>
            {
                v.Name = "Unknown";
                localisedName.Set(v, dutchLocale, "Onbekend");
                v.IsActive = true;
                v.IsManualEntryAllowed = true;
                v.IncreasesQuantityCommittedOut = null; // Does not affect Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = true; // Affects Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Good;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Good;
            });

            merge(StateId, v =>
            {
                v.Name = "State change";
                localisedName.Set(v, dutchLocale, "Status update");
                v.IsActive = true;
                v.IsManualEntryAllowed = false;
                v.IncreasesQuantityCommittedOut = null; // Does not affect Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = null; // Does not affect Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Scrap;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Scrap;
            });

            merge(PhysicalCountId, v =>
            {
                v.Name = "Physical Count";
                localisedName.Set(v, dutchLocale, "Stocktelling");
                v.IsActive = true;
                v.IsManualEntryAllowed = true;
                v.IncreasesQuantityCommittedOut = null; // Does not affect Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = true; // Increases Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Good;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Good;
            });

            merge(ConsumptionId, v =>
            {
                v.Name = "Consumption";
                localisedName.Set(v, dutchLocale, "Verbruik");
                v.IsActive = true;
                v.IsManualEntryAllowed = false;
                v.IncreasesQuantityCommittedOut = false; // Decreases Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = false; // Decreases Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Good;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Good;
            });

            merge(ReservationId, v =>
            {
                v.Name = "Reservation";
                localisedName.Set(v, dutchLocale, "Gereserveerd");
                v.IsActive = true;
                v.IsManualEntryAllowed = false;
                v.IncreasesQuantityCommittedOut = true; // Increases Quantity
                v.IncreasesQuantityExpectedIn = null; // Does not affect Quantity
                v.IncreasesQuantityOnHand = null; // Does not affect Quantity
                v.DefaultSerialisedInventoryItemState = serialisedStates.Good;
                v.DefaultNonSerialisedInventoryItemState = nonSerialisedStates.Good;
            });
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryTransactionReasons.cs" company="Allors bvba">
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

    public partial class InventoryTransactionReasons
    {
        private static readonly Guid OrderId = new Guid("81D45DC9-03D8-480b-B210-1CC641A9CBD3");
        private static readonly Guid ShipmentId = new Guid("C768FA78-0257-4e2d-B760-EC022D89BACF");
        private static readonly Guid TheftId = new Guid("21FA1D1D-F662-4c0e-A4F6-7BD3B5298A47");
        private static readonly Guid ShrinkageId = new Guid("CF6CCC79-7EE8-4755-A9C3-EC9A83649B55");
        private static readonly Guid UnknownId = new Guid("7A438996-B2DC-4b6d-8DDD-47690B06D9B6");
        private static readonly Guid RuinedId = new Guid("6790C5D4-7CC6-43c9-9CAC-48227021E7E9");
        private static readonly Guid PhysicalCountId = new Guid("971D0321-A86D-450C-ADAA-18B3C2114714");

        private UniquelyIdentifiableSticky<InventoryTransactionReason> cache;

        public InventoryTransactionReason Order => this.Cache[OrderId];

        public InventoryTransactionReason Shipment => this.Cache[ShipmentId];

        public InventoryTransactionReason Theft => this.Cache[TheftId];

        public InventoryTransactionReason Shrinkage => this.Cache[ShrinkageId];

        public InventoryTransactionReason Unknown => this.Cache[UnknownId];

        public InventoryTransactionReason Ruined => this.Cache[RuinedId];

        public InventoryTransactionReason PhysicalCount => this.Cache[PhysicalCountId];

        private UniquelyIdentifiableSticky<InventoryTransactionReason> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<InventoryTransactionReason>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;
            var serialisedStates = new SerialisedInventoryItemStates(this.Session);
            var nonSerialisedStates = new NonSerialisedInventoryItemStates(this.Session);

            new InventoryTransactionReasonBuilder(this.Session)
                .WithName("Order")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bestelling").WithLocale(dutchLocale).Build())
                .WithUniqueId(OrderId)
                .WithIsActive(true)
                .WithIsManualEntryAllowed(false)
                .WithAffectsQuantityCommittedOut(true)
                .WithAffectsQuantityExpectedIn(true)
                .WithAffectsQuantityOnHand(false)
                .WithDefaultSerialisedInventoryItemState(serialisedStates.Good)
                .WithDefaultNonSerialisedInventoryItemState(nonSerialisedStates.Good)
                .Build();

            new InventoryTransactionReasonBuilder(this.Session)
                .WithName("Shipment")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verscheping").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShipmentId)
                .WithIsActive(true)
                .WithIsManualEntryAllowed(false)
                .WithAffectsQuantityCommittedOut(true)
                .WithAffectsQuantityExpectedIn(true)
                .WithAffectsQuantityOnHand(true)
                .WithDefaultSerialisedInventoryItemState(serialisedStates.Good)
                .WithDefaultNonSerialisedInventoryItemState(nonSerialisedStates.Good)
                .Build();
            
            new InventoryTransactionReasonBuilder(this.Session)
                .WithName("Theft")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Diefstal").WithLocale(dutchLocale).Build())
                .WithUniqueId(TheftId)
                .WithIsActive(true)
                .WithIsManualEntryAllowed(true)
                .WithAffectsQuantityCommittedOut(true)
                .WithAffectsQuantityExpectedIn(true)
                .WithAffectsQuantityOnHand(true)
                .WithDefaultSerialisedInventoryItemState(serialisedStates.Good)
                .WithDefaultNonSerialisedInventoryItemState(nonSerialisedStates.Good)
                .Build();
            
            new InventoryTransactionReasonBuilder(this.Session)
                .WithName("Shrinkage")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Inkrimping").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShrinkageId)
                .WithIsActive(true)
                .WithIsManualEntryAllowed(false)
                .WithAffectsQuantityCommittedOut(true)
                .WithAffectsQuantityExpectedIn(true)
                .WithAffectsQuantityOnHand(true)
                .WithDefaultSerialisedInventoryItemState(serialisedStates.Good)
                .WithDefaultNonSerialisedInventoryItemState(nonSerialisedStates.Good)
                .Build();
            
            new InventoryTransactionReasonBuilder(this.Session)
                .WithName("Unknown")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Onbekend").WithLocale(dutchLocale).Build())
                .WithUniqueId(UnknownId)
                .WithIsActive(true)
                .WithIsManualEntryAllowed(false)
                .WithAffectsQuantityCommittedOut(true)
                .WithAffectsQuantityExpectedIn(true)
                .WithAffectsQuantityOnHand(true)
                .WithDefaultSerialisedInventoryItemState(serialisedStates.Good)
                .WithDefaultNonSerialisedInventoryItemState(nonSerialisedStates.Good)
                .Build();
            
            new InventoryTransactionReasonBuilder(this.Session)
                .WithName("Ruined")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vernield").WithLocale(dutchLocale).Build())
                .WithUniqueId(RuinedId)
                .WithIsActive(true)
                .WithIsManualEntryAllowed(false)
                .WithAffectsQuantityCommittedOut(true)
                .WithAffectsQuantityExpectedIn(true)
                .WithAffectsQuantityOnHand(true)
                .WithDefaultSerialisedInventoryItemState(serialisedStates.Scrap)
                .WithDefaultNonSerialisedInventoryItemState(nonSerialisedStates.Scrap)
                .Build();

            new InventoryTransactionReasonBuilder(this.Session)
                .WithName("Physical Count")
                .WithUniqueId(PhysicalCountId)
                .WithIsActive(true)
                .WithIsManualEntryAllowed(true)
                .WithAffectsQuantityCommittedOut(true)
                .WithAffectsQuantityExpectedIn(true)
                .WithAffectsQuantityOnHand(true)
                .WithDefaultSerialisedInventoryItemState(serialisedStates.Good)
                .WithDefaultNonSerialisedInventoryItemState(nonSerialisedStates.Good)
                .Build();
        }
    }
}

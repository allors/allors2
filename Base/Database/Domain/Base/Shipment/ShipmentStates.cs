// <copyright file="ShipmentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class ShipmentStates
    {
        public static readonly Guid CreatedId = new Guid("854AD6A0-B2D1-4b92-8C3D-E9E72DD19AFD");
        public static readonly Guid PickingId = new Guid("1D76DE65-4DE4-494D-8677-653B4D62AA42");
        public static readonly Guid PickedId = new Guid("C63C5D25-F139-490f-86D1-2E9E51F5C0A5");
        public static readonly Guid PackedId = new Guid("DCABE845-A6F2-49d9-BBAE-06FB47012A21");
        public static readonly Guid ShippedId = new Guid("B8B115A4-6E5D-4400-BCA7-4224AE1708AA");
        public static readonly Guid DeliveredId = new Guid("B30666C1-9954-4ae1-8F94-A1591B7E35ED");
        public static readonly Guid ReceivedId = new Guid("28EBB252-4DDB-4B14-80F5-765AE59254A0");
        public static readonly Guid CancelledId = new Guid("1F50B912-C778-4c99-84F9-12DACA1E54C1");
        public static readonly Guid OnHoldId = new Guid("268CB9A7-6965-47E8-AF89-8F915242C23D");

        private UniquelyIdentifiableSticky<ShipmentState> cache;

        public ShipmentState Created => this.Cache[CreatedId];

        public ShipmentState Picking => this.Cache[PickingId];

        public ShipmentState Picked => this.Cache[PickedId];

        public ShipmentState Packed => this.Cache[PackedId];

        public ShipmentState Shipped => this.Cache[ShippedId];

        public ShipmentState Delivered => this.Cache[DeliveredId];

        public ShipmentState Received => this.Cache[ReceivedId];

        public ShipmentState Cancelled => this.Cache[CancelledId];

        public ShipmentState OnHold => this.Cache[OnHoldId];

        private UniquelyIdentifiableSticky<ShipmentState> Cache => this.cache ??= new UniquelyIdentifiableSticky<ShipmentState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(CreatedId, v => v.Name = "Created");
            merge(PickingId, v => v.Name = "Picking");
            merge(PickedId, v => v.Name = "Picked");
            merge(PackedId, v => v.Name = "Packed");
            merge(ShippedId, v => v.Name = "Shipped");
            merge(DeliveredId, v => v.Name = "Delivered");
            merge(ReceivedId, v => v.Name = "Received");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(OnHoldId, v => v.Name = "On hold");
        }
    }
}

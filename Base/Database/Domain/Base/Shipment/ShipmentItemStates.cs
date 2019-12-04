// <copyright file="ShipmentItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class ShipmentItemStates
    {
        public static readonly Guid CreatedId = new Guid("E05818B1-2660-4879-B5A8-8CA96F324F7B");
        public static readonly Guid PickedId = new Guid("A8E2014F-C4CB-4A6F-8CCF-0875E439D1F3");
        public static readonly Guid PackedId = new Guid("91853258-C875-4F85-BD84-EF1EBD2E5930");
        public static readonly Guid ShippedId = new Guid("669515DF-3AD7-42EF-9247-ECE5A71785F0");
        public static readonly Guid DeliveredId = new Guid("59BEF1C1-3077-4FF4-AF4B-EAE0425233E1");
        public static readonly Guid ReceivedId = new Guid("2B0C8BC1-F841-487E-854D-4539A0FD33A0");

        private UniquelyIdentifiableSticky<ShipmentItemState> stateCache;

        public ShipmentItemState Created => this.StateCache[CreatedId];

        public ShipmentItemState Picked => this.StateCache[PickedId];

        public ShipmentItemState Packed => this.StateCache[PackedId];

        public ShipmentItemState Shipped => this.StateCache[ShippedId];

        public ShipmentItemState Delivered => this.StateCache[DeliveredId];

        public ShipmentItemState Received => this.StateCache[ReceivedId];

        private UniquelyIdentifiableSticky<ShipmentItemState> StateCache => this.stateCache
                                    ?? (this.stateCache = new UniquelyIdentifiableSticky<ShipmentItemState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(PickedId)
                .WithName("Picked")
                .Build();

            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(PackedId)
                .WithName("Packed")
                .Build();

            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(ShippedId)
                .WithName("Shipped")
                .Build();

            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(DeliveredId)
                .WithName("Delivered")
                .Build();

            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();
        }
    }
}

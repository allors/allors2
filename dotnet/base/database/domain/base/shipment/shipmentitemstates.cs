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
        public static readonly Guid PickingId = new Guid("F9043ADD-E106-4646-8B02-6B10EFBB2E87");
        public static readonly Guid PickedId = new Guid("A8E2014F-C4CB-4A6F-8CCF-0875E439D1F3");
        public static readonly Guid PackedId = new Guid("91853258-C875-4F85-BD84-EF1EBD2E5930");
        public static readonly Guid ShippedId = new Guid("4377658B-7D0E-4B57-BA8A-15AEB368EEE1");
        public static readonly Guid ReceivedId = new Guid("44B78834-64E8-498E-B2E4-E685950BBD95");

        private UniquelyIdentifiableSticky<ShipmentItemState> cache;

        public ShipmentItemState Created => this.Cache[CreatedId];

        public ShipmentItemState Picking => this.Cache[PickingId];

        public ShipmentItemState Picked => this.Cache[PickedId];

        public ShipmentItemState Packed => this.Cache[PackedId];

        public ShipmentItemState Shipped => this.Cache[ShippedId];

        public ShipmentItemState Received => this.Cache[ReceivedId];

        private UniquelyIdentifiableSticky<ShipmentItemState> Cache => this.cache ??= new UniquelyIdentifiableSticky<ShipmentItemState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(CreatedId, v => v.Name = "Created");
            merge(PickingId, v => v.Name = "Picking");
            merge(PickedId, v => v.Name = "Picked");
            merge(PackedId, v => v.Name = "Packed");
            merge(ShippedId, v => v.Name = "Shipped");
            merge(ReceivedId, v => v.Name = "Received");
        }
    }
}

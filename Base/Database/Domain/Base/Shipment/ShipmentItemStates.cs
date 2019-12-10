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

        private UniquelyIdentifiableSticky<ShipmentItemState> stateCache;

        public ShipmentItemState Created => this.StateCache[CreatedId];

        public ShipmentItemState Picking => this.StateCache[PickingId];

        public ShipmentItemState Picked => this.StateCache[PickedId];

        public ShipmentItemState Packed => this.StateCache[PackedId];

        private UniquelyIdentifiableSticky<ShipmentItemState> StateCache => this.stateCache
                                    ?? (this.stateCache = new UniquelyIdentifiableSticky<ShipmentItemState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(PickingId)
                .WithName("Picking")
                .Build();

            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(PickedId)
                .WithName("Picked")
                .Build();

            new ShipmentItemStateBuilder(this.Session)
                .WithUniqueId(PackedId)
                .WithName("Packed")
                .Build();
        }
    }
}

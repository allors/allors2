// <copyright file="ShipmentState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ShipmentState
    {
        public bool IsCreated => Equals(this.UniqueId, ShipmentStates.CreatedId);

        public bool IsPicking => Equals(this.UniqueId, ShipmentStates.PickingId);

        public bool IsPicked => Equals(this.UniqueId, ShipmentStates.PickedId);

        public bool IsPacked => Equals(this.UniqueId, ShipmentStates.PackedId);

        public bool IsShipped => Equals(this.UniqueId, ShipmentStates.ShippedId);

        public bool IsDelivered => Equals(this.UniqueId, ShipmentStates.DeliveredId);

        public bool IsReceived => Equals(this.UniqueId, ShipmentStates.ReceivedId);

        public bool IsCancelled => Equals(this.UniqueId, ShipmentStates.CancelledId);

        public bool IsOnHoldId => Equals(this.UniqueId, ShipmentStates.OnHoldId);
    }
}

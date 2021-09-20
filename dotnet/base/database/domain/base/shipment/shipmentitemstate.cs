// <copyright file="ShipmentItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ShipmentItemState
    {
        public bool Created => Equals(this.UniqueId, ShipmentItemStates.CreatedId);

        public bool Picked => Equals(this.UniqueId, ShipmentItemStates.PickedId);

        public bool Packed => Equals(this.UniqueId, ShipmentItemStates.PackedId);

        public bool Shipped => Equals(this.UniqueId, ShipmentItemStates.ShippedId);
    }
}

// <copyright file="PurchaseOrderItemShipmentState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PurchaseOrderItemShipmentState
    {
        public bool IsNotReceived => Equals(this.UniqueId, PurchaseOrderItemShipmentStates.NotReceivedId);

        public bool IsPartiallyReceived => Equals(this.UniqueId, PurchaseOrderItemShipmentStates.PartiallyReceivedId);

        public bool IsReceived => Equals(this.UniqueId, PurchaseOrderItemShipmentStates.ReceivedId);

        public bool IsNa => Equals(this.UniqueId, PurchaseOrderItemShipmentStates.NaId);
    }
}

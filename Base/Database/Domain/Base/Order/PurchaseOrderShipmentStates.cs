// <copyright file="PurchaseOrderShipmentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrderShipmentStates
    {
        public static readonly Guid NotReceivedId = new Guid("32267C1E-FB87-43E5-B2D8-6477FA1CA4C8");
        public static readonly Guid PartiallyReceivedId = new Guid("77ED251D-B004-41e7-B0C4-9769CF7AE73E");
        public static readonly Guid ReceivedId = new Guid("BCCB68CE-A517-44c6-ADDA-DBEB0464D575");

        private UniquelyIdentifiableSticky<PurchaseOrderShipmentState> cache;

        public PurchaseOrderShipmentState NotReceived => this.Cache[NotReceivedId];

        public PurchaseOrderShipmentState PartiallyReceived => this.Cache[PartiallyReceivedId];

        public PurchaseOrderShipmentState Received => this.Cache[ReceivedId];

        private UniquelyIdentifiableSticky<PurchaseOrderShipmentState> Cache => this.cache ??= new UniquelyIdentifiableSticky<PurchaseOrderShipmentState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(NotReceivedId, v => v.Name = "Not Received");
            merge(PartiallyReceivedId, v => v.Name = "Partially Received");
            merge(ReceivedId, v => v.Name = "Received");
        }
    }
}

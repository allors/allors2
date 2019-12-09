// <copyright file="PurchaseOrderItemShipmentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrderItemShipmentStates
    {
        internal static readonly Guid NotReceivedId = new Guid("CF26D4F9-E8AF-4A1D-9841-73B8C5266117");
        internal static readonly Guid PartiallyReceivedId = new Guid("C142144A-8CAE-4D2B-A56B-94BAF236227A");
        internal static readonly Guid ReceivedId = new Guid("AD66619F-BB48-42AF-B019-3E4028AD7B6B");

        private UniquelyIdentifiableSticky<PurchaseOrderItemShipmentState> cache;

        public PurchaseOrderItemShipmentState NotReceived => this.Cache[NotReceivedId];

        public PurchaseOrderItemShipmentState PartiallyReceived => this.Cache[PartiallyReceivedId];

        public PurchaseOrderItemShipmentState Received => this.Cache[ReceivedId];

        private UniquelyIdentifiableSticky<PurchaseOrderItemShipmentState> Cache => this.cache ??= new UniquelyIdentifiableSticky<PurchaseOrderItemShipmentState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(NotReceivedId, v => v.Name = "Not Received");
            merge(PartiallyReceivedId, v => v.Name = "Partially Received");
            merge(ReceivedId, v => v.Name = "Received");
        }
    }
}

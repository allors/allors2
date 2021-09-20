// <copyright file="SerialisedItemSoldOns.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SerialisedItemSoldOns
    {
        public static readonly Guid SalesOrderAcceptId = new Guid("b4f69621-7eb7-4763-928c-415cf3b4a4b5");
        public static readonly Guid SalesInvoiceSendId = new Guid("ba8e344d-0edd-4cb1-8a10-aae1ecdb57dc");
        public static readonly Guid CustomerShipmentShipId = new Guid("9cb9b456-8e10-4bcc-813d-3aa508c060e1");
        public static readonly Guid PurchaseInvoiceConfirmId = new Guid("c955b5bd-2fc5-45a2-bf97-7d3bfe9aecb1");
        public static readonly Guid PurchaseshipmentReceiveId = new Guid("30fac958-47e5-40ec-83cb-3d8e2593abbf");

        private UniquelyIdentifiableSticky<SerialisedItemSoldOn> cache;

        public SerialisedItemSoldOn SalesOrderAccept => this.Cache[SalesOrderAcceptId];

        public SerialisedItemSoldOn SalesInvoiceSend => this.Cache[SalesInvoiceSendId];

        public SerialisedItemSoldOn CustomerShipmentShip => this.Cache[CustomerShipmentShipId];

        public SerialisedItemSoldOn PurchaseInvoiceConfirm => this.Cache[PurchaseInvoiceConfirmId];

        public SerialisedItemSoldOn PurchaseshipmentReceive => this.Cache[PurchaseshipmentReceiveId];

        private UniquelyIdentifiableSticky<SerialisedItemSoldOn> Cache => this.cache ??= new UniquelyIdentifiableSticky<SerialisedItemSoldOn>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(SalesOrderAcceptId, v => v.Name = "SalesOrder Accept");
            merge(SalesInvoiceSendId, v => v.Name = "SalesInvoice Send");
            merge(CustomerShipmentShipId, v => v.Name = "CustomerShipment Ship");
            merge(PurchaseInvoiceConfirmId, v => v.Name = "PurchaseInvoice Confirm");
            merge(PurchaseshipmentReceiveId, v => v.Name = "Purchaseshipment Receive");
        }
    }
}

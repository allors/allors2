// <copyright file="Model.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.PurchaseInvoiceModel
{
    using System.Linq;

    public class Model
    {
        public Model(PurchaseInvoice invoice)
        {
            this.Invoice = new InvoiceModel(invoice);

            this.BilledTo = new BilledToModel((Organisation)invoice.BilledTo);
            this.BilledFrom = new BilledFromModel(invoice);
            this.ShipTo = new ShipToModel(invoice);

            this.InvoiceItems = invoice.PurchaseInvoiceItems.Select(v => new InvoiceItemModel(v)).ToArray();
            this.OrderAdjustments = invoice.OrderAdjustments.Select(v => new OrderAdjustmentModel(v)).ToArray();
            this.PurchaseOrders = invoice.PurchaseInvoiceItems.SelectMany(v => v.OrderItemBillingsWhereInvoiceItem).Select(v => new PurchaseOrderModel(((PurchaseOrderItem)v.OrderItem).PurchaseOrderWherePurchaseOrderItem)).ToArray();
        }

        public InvoiceModel Invoice { get; }

        public BilledToModel BilledTo { get; }

        public BilledFromModel BilledFrom { get; }

        public ShipToModel ShipTo { get; }

        public InvoiceItemModel[] InvoiceItems { get; }

        public PurchaseOrderModel[] PurchaseOrders { get; }

        public OrderAdjustmentModel[] OrderAdjustments { get; }
    }
}

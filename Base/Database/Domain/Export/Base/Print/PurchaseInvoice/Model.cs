// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Model.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
            this.PurchaseOrders = invoice.PurchaseInvoiceItems.SelectMany(v => v.OrderItemBillingsWhereInvoiceItem).Select(v => new PurchaseOrderModel(((PurchaseOrderItem)v.OrderItem).PurchaseOrderWherePurchaseOrderItem)).ToArray();
        }

        public InvoiceModel Invoice { get; }

        public BilledToModel BilledTo { get; }

        public BilledFromModel BilledFrom { get; }

        public ShipToModel ShipTo { get; }

        public InvoiceItemModel[] InvoiceItems { get; }

        public PurchaseOrderModel[] PurchaseOrders { get; }
    }
}

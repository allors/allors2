// <copyright file="Model.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.SalesInvoiceModel
{
    using System.Linq;

    public class Model
    {
        public Model(SalesInvoice invoice)
        {
            var session = invoice.Strategy.Session;

            this.Invoice = new InvoiceModel(invoice);
            this.BilledFrom = new BilledFromModel((Organisation)invoice.BilledFrom);
            this.BillTo = new BillToModel(invoice);
            this.ShipTo = new ShipToModel(invoice);

            this.InvoiceItems = invoice.SalesInvoiceItems.Select(v => new InvoiceItemModel(v)).ToArray();

            if (invoice.ExistOrderAdjustments)
            {
                this.OrderAdjustments = invoice.OrderAdjustments.Select(v => new OrderAdjustmentModel(v)).ToArray();
            }

            var paymentTerm = new InvoiceTermTypes(session).PaymentNetDays;
            this.SalesTerms = invoice.SalesTerms.Where(v => !v.TermType.Equals(paymentTerm)).Select(v => new SalesTermModel(v)).ToArray();
        }

        public InvoiceModel Invoice { get; }

        public BilledFromModel BilledFrom { get; }

        public BillToModel BillTo { get; }

        public ShipToModel ShipTo { get; }

        public InvoiceItemModel[] InvoiceItems { get; }

        public SalesTermModel[] SalesTerms { get; }

        public OrderAdjustmentModel[] OrderAdjustments { get; }
    }
}

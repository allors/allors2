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

namespace Allors.Domain.Print.SalesInvoiceModel
{
    using System.Linq;

    public class Model
    {
        public Model(SalesInvoice invoice)
        {
            this.Invoice = new InvoiceModel(invoice);
            this.BilledFrom = new BilledFromModel((Organisation)invoice.BilledFrom);
            this.BillTo = new BillToModel(invoice);
            this.ShipTo = new ShipToModel(invoice);

            this.InvoiceItems = invoice.SalesInvoiceItems.Select(v => new InvoiceItemModel(v)).ToArray();

            var paymentTerm = new InvoiceTermTypes(invoice.Strategy.Session).PaymentNetDays;
            this.SalesTerms = invoice.SalesTerms.Where(v => !v.TermType.Equals(paymentTerm)).Select(v => new SalesTermModel(v)).ToArray();
        }

        public InvoiceModel Invoice { get; }

        public BilledFromModel BilledFrom { get; }

        public BillToModel BillTo { get; }

        public ShipToModel ShipTo { get; }

        public InvoiceItemModel[] InvoiceItems { get; }

        public SalesTermModel[] SalesTerms { get; }
    }
}

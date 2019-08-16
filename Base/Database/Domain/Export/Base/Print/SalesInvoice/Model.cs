
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

            var paymentTerm = new InvoiceTermTypes(session).PaymentNetDays;
            this.SalesTerms = invoice.SalesTerms.Where(v => !v.TermType.Equals(paymentTerm)).Select(v => new SalesTermModel(v)).ToArray();

            string TakenByCountry = null;
            if (invoice.BilledFrom.PartyContactMechanisms?.FirstOrDefault(v => v.ContactPurposes.Any(p => Equals(p, new ContactMechanismPurposes(session).RegisteredOffice)))?.ContactMechanism is PostalAddress registeredOffice)
            {
                TakenByCountry = registeredOffice.Country.IsoCode;
            }

            if (TakenByCountry == "BE")
            {
                this.VatClause = invoice.DerivedVatClause?.LocalisedClause.First(v => v.Locale.Equals(new Locales(session).DutchNetherlands)).Text;

                if (Equals(invoice.DerivedVatClause, new VatClauses(session).BeArt14Par2))
                {
                    var shipToCountry = invoice.ShipToAddress?.Country?.Name;
                    this.VatClause = this.VatClause.Replace("{shipToCountry}", shipToCountry);
                }
            }
        }

        public InvoiceModel Invoice { get; }

        public BilledFromModel BilledFrom { get; }

        public BillToModel BillTo { get; }

        public ShipToModel ShipTo { get; }

        public InvoiceItemModel[] InvoiceItems { get; }

        public SalesTermModel[] SalesTerms { get; }

        public string VatClause { get; }
    }
}

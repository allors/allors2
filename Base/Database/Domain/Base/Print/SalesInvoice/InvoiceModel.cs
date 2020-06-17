// <copyright file="InvoiceModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.SalesInvoiceModel
{
    using System.Globalization;
    using System.Linq;

    public class InvoiceModel
    {
        public InvoiceModel(SalesInvoice invoice)
        {
            var session = invoice.Strategy.Session;
            var currencyIsoCode = invoice.Currency.IsoCode;

            this.Title = invoice.SalesInvoiceType.Equals(new SalesInvoiceTypes(session).CreditNote) ? "CREDIT NOTE" : "INVOICE";
            this.Description = invoice.Description;
            this.Currency = invoice.Currency.IsoCode;
            this.Number = invoice.InvoiceNumber;
            this.Date = invoice.InvoiceDate.ToString("yyyy-MM-dd");
            this.DueDate = invoice.DueDate?.ToString("yyyy-MM-dd");
            this.CustomerReference = invoice.CustomerReference;

            // TODO: Where does the currency come from?
            this.SubTotal = invoice.TotalBasePrice.ToString("N2", new CultureInfo("nl-BE"));
            this.Deposit = invoice.AdvancePayment.ToString("N2", new CultureInfo("nl-BE"));
            this.TotalExVat = invoice.TotalExVat.ToString("N2", new CultureInfo("nl-BE"));
            this.VatCharge = invoice.VatRegime?.VatRate?.Rate.ToString("n2");
            this.TotalVat = invoice.TotalVat.ToString("N2", new CultureInfo("nl-BE"));
            this.TotalIncVat = currencyIsoCode + " " + invoice.TotalIncVat.ToString("N2", new CultureInfo("nl-BE"));

            this.PaymentNetDays = invoice.PaymentNetDays;

            string TakenByCountry = null;
            if (invoice.BilledFrom.PartyContactMechanisms?.FirstOrDefault(v => v.ContactPurposes.Any(p => Equals(p, new ContactMechanismPurposes(session).RegisteredOffice)))?.ContactMechanism is PostalAddress registeredOffice)
            {
                TakenByCountry = registeredOffice.Country.IsoCode;
            }

            if (TakenByCountry == "BE")
            {
                this.VatClause = invoice.DerivedVatClause?.LocalisedClauses.First(v => v.Locale.Equals(new Locales(session).DutchBelgium)).Text;

                if (Equals(invoice.DerivedVatClause, new VatClauses(session).BeArt14Par2))
                {
                    var shipToCountry = invoice.ShipToAddress?.Country?.Name;
                    this.VatClause = this.VatClause.Replace("{shipToCountry}", shipToCountry);
                }
            }
        }

        public string Title { get; }

        public string Description { get; }

        public string Number { get; }

        public string Date { get; }

        public string DueDate { get; }

        public string CustomerReference { get; }

        public string SubTotal { get; }

        public string Deposit { get; }

        public string TotalExVat { get; }

        public string VatCharge { get; }

        public string TotalVat { get; }

        public string TotalIncVat { get; }

        public int PaymentNetDays { get; }

        public string VatClause { get; set; }

        public string Currency { get; set; }
    }
}

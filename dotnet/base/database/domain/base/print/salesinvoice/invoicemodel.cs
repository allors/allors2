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
            var currencyIsoCode = invoice.DerivedCurrency.IsoCode;

            this.Title = invoice.SalesInvoiceType.Equals(new SalesInvoiceTypes(session).CreditNote) ? "CREDIT NOTE" : "INVOICE";
            this.Description = invoice.Description;
            this.Currency = invoice.DerivedCurrency.IsoCode;
            this.Number = invoice.InvoiceNumber;
            this.Date = invoice.InvoiceDate.ToString("yyyy-MM-dd");
            this.DueDate = invoice.DueDate?.ToString("yyyy-MM-dd");
            this.CustomerReference = invoice.CustomerReference;

            // TODO: Where does the currency come from?
            this.SubTotal = invoice.TotalBasePrice.ToString("N2", new CultureInfo("nl-BE"));
            this.Deposit = invoice.AdvancePayment.ToString("N2", new CultureInfo("nl-BE"));
            this.TotalExVat = invoice.TotalExVat.ToString("N2", new CultureInfo("nl-BE"));
            this.VatRate = (invoice.DerivedVatRate?.Rate.ToString("n2"))
                ?? (invoice.ValidInvoiceItems.FirstOrDefault(v => v.ExistVatRate)?.VatRate.Rate.ToString("n2"))
                ?? "0";

            this.TotalVat = invoice.TotalVat.ToString("N2", new CultureInfo("nl-BE"));
            this.IrpfRate = invoice.DerivedIrpfRate?.Rate.ToString("n2");

            // IRPF is subtracted for total amount to pay
            var totalIrpf = invoice.TotalIrpf * -1;
            this.TotalIrpf = totalIrpf.ToString("N2", new CultureInfo("nl-BE"));
            this.PrintIrpf = invoice.TotalIrpf != 0;

            this.TotalIncVat = invoice.TotalIncVat.ToString("N2", new CultureInfo("nl-BE"));
            this.GrandTotal = invoice.GrandTotal.ToString("N2", new CultureInfo("nl-BE"));

            this.TotalToPay = currencyIsoCode + " " + (invoice.GrandTotal - invoice.AdvancePayment).ToString("N2", new CultureInfo("nl-BE"));

            this.PaymentNetDays = invoice.PaymentNetDays;

            string TakenByCountry = null;
            if (invoice.BilledFrom.PartyContactMechanisms?.FirstOrDefault(v => v.ContactPurposes.Any(p => Equals(p, new ContactMechanismPurposes(session).RegisteredOffice)))?.ContactMechanism is PostalAddress registeredOffice)
            {
                TakenByCountry = registeredOffice.Country.IsoCode;
            }

            if (TakenByCountry == "BE")
            {
                this.VatClause = invoice.DerivedVatClause?.LocalisedClauses.FirstOrDefault(v => v.Locale.Equals(new Locales(session).DutchBelgium))?.Text;

                if (this.VatClause != null && Equals(invoice.DerivedVatClause, new VatClauses(session).BeArt14Par2))
                {
                    var shipToCountry = invoice.DerivedShipToAddress?.Country?.Name;
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

        public string VatRate { get; }

        public string TotalVat { get; }

        public string IrpfRate { get; }

        public string TotalIrpf { get; }

        public string TotalIncVat { get; }

        public string GrandTotal { get; }

        public string TotalToPay { get; }

        public int PaymentNetDays { get; }

        public string VatClause { get; set; }

        public string Currency { get; set; }

        public bool PrintIrpf { get; }
    }
}

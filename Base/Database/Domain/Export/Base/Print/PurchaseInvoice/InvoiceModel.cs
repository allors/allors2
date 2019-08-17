// <copyright file="InvoiceModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.PurchaseInvoiceModel
{
    using System;

    public class InvoiceModel
    {
        public InvoiceModel(PurchaseInvoice invoice)
        {
            this.Description = invoice.Description;
            this.Number = invoice.InvoiceNumber;
            this.Date = invoice.InvoiceDate.ToString("yyyy-MM-dd");
            this.CustomerReference = invoice.CustomerReference;

            // TODO: Where does the currency come from?
            var currency = "€";
            this.SubTotal = invoice.TotalBasePrice.ToString("0.00") + " " + currency;
            this.TotalExVat = invoice.TotalExVat.ToString("0.00") + " " + currency;
            this.VatCharge = invoice.VatRegime?.VatRate?.Rate.ToString("n2");
            this.TotalVat = invoice.TotalVat.ToString("0.00") + " " + currency;
            this.TotalIncVat = invoice.TotalIncVat.ToString("0.00") + " " + currency;
        }

        public string Description { get; }
        public string Number { get; }
        public string Date { get; }
        public string CustomerReference { get; }
        public string SubTotal { get; }
        public string TotalExVat { get; }
        public string VatCharge { get; }
        public string TotalVat { get; }
        public string TotalIncVat { get; }
    }
}

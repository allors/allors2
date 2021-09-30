// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Print.ProductQuoteModel
{
    using System.Collections.Generic;
    using System.Globalization;

    public class QuoteModel
    {
        public QuoteModel(Quote quote, Dictionary<string, byte[]> imageByImageName)
        {
            this.Description = quote.Description;
            this.Number = quote.QuoteNumber;
            this.IssueDate = quote.IssueDate.ToString("yyyy-MM-dd");
            this.ValidFromDate = (quote.ValidFromDate ?? quote.IssueDate).ToString("yyyy-MM-dd");
            this.ValidThroughDate = quote.ValidThroughDate?.ToString("yyyy-MM-dd");

            // TODO: Where does the currency come from?
            var currency = "€";
            this.SubTotal = quote.TotalBasePrice.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
            this.TotalExVat = quote.TotalExVat.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
            this.VatRate = quote.DerivedVatRate?.Rate.ToString("n2");
            this.TotalVat = quote.TotalVat.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;

            // IRPF is subtracted for total amount to pay
            var totalIrpf = quote.TotalIrpf * -1;
            this.IrpfRate = quote.DerivedIrpfRate?.Rate.ToString("n2");
            this.TotalIrpf = totalIrpf.ToString("N2", new CultureInfo("nl-BE"));
            this.PrintIrpf = quote.TotalIrpf != 0;

            this.IrpfRate = quote.DerivedIrpfRate?.Rate.ToString("n2");
            this.TotalIrpf = quote.TotalIrpf.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
            this.TotalIncVat = quote.TotalIncVat.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
            this.GrandTotal = quote.GrandTotal.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
        }

        public string Description { get; }

        public string Number { get; }

        public string IssueDate { get; }

        public string ValidFromDate { get; }

        public string ValidThroughDate { get; }

        public string SubTotal { get; }

        public string TotalExVat { get; }

        public string VatRate { get; }

        public string TotalVat { get; }

        public string IrpfRate { get; }

        public string TotalIrpf{ get; }

        public string TotalIncVat { get; }

        public string GrandTotal { get; }

        public bool PrintIrpf { get; }
    }
}

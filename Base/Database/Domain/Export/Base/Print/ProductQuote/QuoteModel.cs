// <copyright file="QuoteModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.ProductQuoteModel
{
    public class QuoteModel
    {
        public QuoteModel(Quote quote)
        {
            this.Description = quote.Description;
            this.Number = quote.QuoteNumber;
            this.IssueDate = quote.IssueDate.ToString("yyyy-MM-dd");
            this.ValidFromDate = (quote.ValidFromDate ?? quote.IssueDate).ToString("yyyy-MM-dd");
            this.ValidThroughDate = quote.ValidThroughDate?.ToString("yyyy-MM-dd");

            // TODO: Where does the currency come from?
            var currency = "€";
            this.SubTotal = quote.TotalBasePrice.ToString("0.00") + " " + currency;
            this.TotalExVat = quote.TotalExVat.ToString("0.00") + " " + currency;
            this.VatCharge = quote.VatRegime?.VatRate?.Rate.ToString("n2");
            this.TotalVat = quote.TotalVat.ToString("0.00") + " " + currency;
            this.TotalIncVat = quote.TotalIncVat.ToString("0.00") + " " + currency;
        }

        public string Description { get; }
        public string Number { get; }
        public string IssueDate { get; }
        public string ValidFromDate { get; }
        public string ValidThroughDate { get; }
        public string SubTotal { get; }
        public string TotalExVat { get; }
        public string VatCharge { get; }
        public string TotalVat { get; }
        public string TotalIncVat { get; }
    }
}

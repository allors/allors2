// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteModel.cs" company="Allors bvba">
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

namespace Allors.Domain.ProductQuotePrint
{
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Sandwych.Reporting;

    public class QuoteModel
    {
        public QuoteModel(Quote quote)
        {
            this.Description = quote.Description;
            this.Number = quote.QuoteNumber;
            if (quote.ExistQuoteNumber)
            {
                var session = quote.Strategy.Session;
                var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                var barcode = barcodeService.Generate(quote.QuoteNumber, BarcodeType.CODE_128, 320, 80);
                this.Barcode = new ImageBlob("png", barcode);
            }

            this.IssueDate = quote.IssueDate.ToString("yyyy-MM-dd");
            this.ValidFromDate = (quote.ValidFromDate ?? quote.IssueDate).ToString("yyyy-MM-dd");
            this.ValidThroughDate = quote.ValidThroughDate?.ToString("yyyy-MM-dd");

            // TODO: Where does the currency come from?
            var currency = "€";
            this.Price = quote.Price.ToString("0.00") + " " + currency;
        }

        public string Description { get; }
        public string Number { get; }
        public ImageBlob Barcode { get; }
        public string IssueDate { get; }
        public string ValidFromDate { get; }
        public string ValidThroughDate { get; }
        public string Price { get; }
    }
}

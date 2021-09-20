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

namespace Allors.Domain.Print.ProductQuoteModel
{
    using System.Collections.Generic;
    using System.Linq;

    public class Model
    {
        public Model(ProductQuote quote, Dictionary<string, byte[]> images)
        {
            this.Quote = new QuoteModel(quote, images);

            this.Request = new RequestModel(quote, images);
            this.Issuer = new IssuerModel(quote, images);
            this.BillTo = new BillToModel(quote, images);
            this.Receiver = new ReceiverModel(quote, images);

            this.QuoteItems = quote.QuoteItems.Select(v => new QuoteItemModel(v, images)).ToArray();
            this.OrderAdjustments = quote.OrderAdjustments.Select(v => new OrderAdjustmentModel(v)).ToArray();
        }

        public QuoteModel Quote { get; }

        public RequestModel Request { get; }

        public IssuerModel Issuer { get; }

        public BillToModel BillTo { get; }

        public ReceiverModel Receiver { get; }

        public QuoteItemModel[] QuoteItems { get; }

        public OrderAdjustmentModel[] OrderAdjustments { get; }
    }
}

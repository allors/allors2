// <copyright file="Model.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.ProductQuoteModel
{
    using System.Linq;

    public class Model
    {
        public Model(ProductQuote quote)
        {
            this.Quote = new QuoteModel(quote);

            this.Request = new RequestModel(quote.Request);
            this.Issuer = new IssuerModel((Organisation)quote.Issuer);
            this.BillTo = new BillToModel(quote);
            this.Receiver = new ReceiverModel(quote);

            this.QuoteItems = quote.QuoteItems.Select(v => new QuoteItemModel(v)).ToArray();
        }

        public QuoteModel Quote { get; }

        public RequestModel Request { get; }

        public IssuerModel Issuer { get; }

        public BillToModel BillTo { get; }

        public ReceiverModel Receiver { get; }

        public QuoteItemModel[] QuoteItems { get; }
    }
}

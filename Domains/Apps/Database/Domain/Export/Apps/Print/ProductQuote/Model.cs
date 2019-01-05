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

namespace Allors.Domain.ProductQuotePrint
{
    using System.Linq;
    using Sandwych.Reporting;

    public class Model
    {
        public Model(ProductQuote quote)
        {
            this.Quote = new QuoteModel(quote);

            if (quote.Issuer?.ExistLogoImage == true)
            {
                this.Logo = new ImageBlob("png", quote.Issuer.LogoImage.MediaContent.Data);
            }
            else
            {
                var singleton = quote.Strategy.Session.GetSingleton();
                this.Logo = new ImageBlob("png", singleton.LogoImage.MediaContent.Data);
            }

            this.Request = new RequestModel(quote.Request);
            this.Issuer = new IssuerModel((Organisation)quote.Issuer);
            this.BillTo = new BillToModel(quote);
            this.Receiver = new ReceiverModel(quote);
            
            this.QuoteItems = quote.QuoteItems.Select(v => new QuoteItemModel(v)).ToArray();

        }

        public QuoteModel Quote { get; }

        public RequestModel Request { get; }

        public ImageBlob Logo { get; }

        public IssuerModel Issuer { get; }

        public BillToModel BillTo { get; }

        public ReceiverModel Receiver { get; }

        public QuoteItemModel[] QuoteItems { get; }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQuoteVersion.cs" company="Allors bvba">
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
namespace Allors.Domain
{
    public partial class ProductQuoteVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (ProductQuoteVersionBuilder) method.Builder;
            var productQuote = builder.ProductQuote;

            if (productQuote != null)
            {
                this.InternalComment = productQuote.InternalComment;
                this.RequiredResponseDate = productQuote.RequiredResponseDate;
                this.ValidFromDate = productQuote.ValidFromDate;
                this.QuoteTerms = productQuote.QuoteTerms;
                this.Issuer = productQuote.Issuer;
                this.ValidThroughDate = productQuote.ValidThroughDate;
                this.Description = productQuote.Description;
                this.Receiver = productQuote.Receiver;
                this.FullfillContactMechanism = productQuote.FullfillContactMechanism;
                this.Amount = productQuote.Amount;
                this.Currency = productQuote.Currency;
                this.IssueDate = productQuote.IssueDate;
                this.QuoteItems = productQuote.QuoteItems;
                this.QuoteNumber = productQuote.QuoteNumber;
                this.Request= productQuote.Request;
                this.CurrentObjectState = productQuote.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}
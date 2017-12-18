// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestForQuote.cs" company="Allors bvba">
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

using System;
using System.Linq;

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class RequestForQuote
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.RequestForQuote, M.RequestForQuote.RequestState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        private ProductQuote QuoteThis()
        {
            var productQuote = new ProductQuoteBuilder(this.Strategy.Session)
                .WithRequest(this)
                .WithContactPerson(this.ContactPerson)
                .WithDescription(this.Description)
                .WithReceiver(this.Originator)
                .WithRequiredResponseDate(this.RequiredResponseDate)
                .WithCurrency(this.Currency)
                .WithFullfillContactMechanism(this.FullfillContactMechanism)
                .WithComment(this.Comment)
                .WithInternalComment(this.InternalComment)
                .Build();

            var sourceItems = this.RequestItems.Where(i => i.RequestItemState.Equals(new RequestItemStates(this.Strategy.Session).Submitted)).ToArray();

            foreach (RequestItem requestItem in sourceItems)
            {
                requestItem.RequestItemState = new RequestItemStates(this.Strategy.Session).Quoted;

                productQuote.AddQuoteItem(
                    new QuoteItemBuilder(this.Strategy.Session)
                    .WithProduct(requestItem.Product)
                    .WithProductFeature(requestItem.ProductFeature)
                    .WithQuantity(requestItem.Quantity)
                    .WithUnitOfMeasure(requestItem.UnitOfMeasure)
                    .WithRequestItem(requestItem)
                    .WithComment(requestItem.Comment)
                    .WithInternalComment(requestItem.InternalComment).Build());
            }

            return productQuote;
        }

        public void AppsCreateQuote(RequestForQuoteCreateQuote Method)
        {
            this.RequestState = new RequestStates(this.Strategy.Session).Quoted;
            this.QuoteThis();
        }
    }
}

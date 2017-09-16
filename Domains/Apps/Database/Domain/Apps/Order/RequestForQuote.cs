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
    public partial class RequestForQuote
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.InternalComment, this.CurrentVersion.InternalComment);

            var isNewStateVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new RequestForQuoteVersionBuilder(this.Strategy.Session).WithRequestForQuote(this).Build();
                this.AddAllVersion(this.CurrentVersion);
            }

            if (isNewStateVersion)
            {
                this.CurrentStateVersion = CurrentVersion;
                this.AddAllStateVersion(this.CurrentStateVersion);
            }
        }

        private ProductQuote QuoteThis()
        {
            var productQuote = new ProductQuoteBuilder(this.Strategy.Session)
                .WithRequest(this)
                .WithDescription(this.Description)
                .WithReceiver(this.Originator)
                .WithIssuer(Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation)
                .WithRequiredResponseDate(this.RequiredResponseDate)
                .WithCurrency(this.Currency)
                .WithFullfillContactMechanism(this.FullfillContactMechanism)
                .WithComment(this.Comment)
                .WithInternalComment(this.InternalComment)
                .Build();

            var sourceItems = this.RequestItems.Where(i => i.CurrentObjectState.Equals(new RequestItemObjectStates(this.Strategy.Session).Submitted)).ToArray();

            foreach (RequestItem requestItem in sourceItems)
            {
                productQuote.AddQuoteItem(
                    new QuoteItemBuilder(this.Strategy.Session)
                    .WithProduct(requestItem.Product)
                    .WithProductFeature(requestItem.ProductFeature)
                    .WithQuantity(requestItem.Quantity)
                    .WithUnitOfMeasure(requestItem.UnitOfMeasure)
                    .WithRequestItem(requestItem)
                    .WithComment(requestItem.Comment)
                    .WithInternalComment(requestItem.InternalComment)
                    .Build()
                    );
            }

            return productQuote;
        }

        public void AppsCreateQuote(RequestForQuoteCreateQuote Method)
        {
            this.CurrentObjectState = new RequestObjectStates(this.Strategy.Session).Quoted;
            this.QuoteThis();
        }
    }
}

// <copyright file="RequestForQuote.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    using Allors.Meta;

    public partial class RequestForQuote
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.RequestForQuote, M.RequestForQuote.RequestState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void BaseOnDerive(ObjectOnDerive method) => this.Sync(this.Strategy.Session);

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            if (!this.ExistOriginator)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Submit, Operations.Execute));
            }

            var deletePermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
            if (this.IsDeletable())
            {
                this.RemoveDeniedPermission(deletePermission);
            }
            else
            {
                this.AddDeniedPermission(deletePermission);
            }
        }

        public void BaseCreateQuote(RequestForQuoteCreateQuote Method)
        {
            this.RequestState = new RequestStates(this.Strategy.Session).Quoted;
            this.QuoteThis();
        }

        private void Sync(ISession session)
        {
            // session.Prefetch(this.SyncPrefetch, this);
            foreach (RequestItem requestItem in this.RequestItems)
            {
                requestItem.Sync(this);
            }
        }

        private ProductQuote QuoteThis()
        {
            var productQuote = new ProductQuoteBuilder(this.Strategy.Session)
                .WithRequest(this)
                .WithIssuer(this.Recipient)
                .WithContactPerson(this.ContactPerson)
                .WithDescription(this.Description)
                .WithReceiver(this.Originator)
                .WithRequiredResponseDate(this.RequiredResponseDate)
                .WithAssignedCurrency(this.Currency)
                .WithFullfillContactMechanism(this.FullfillContactMechanism)
                .Build();

            var sourceItems = this.RequestItems.Where(i => i.RequestItemState.Equals(new RequestItemStates(this.Strategy.Session).Submitted)).ToArray();

            foreach (var requestItem in sourceItems)
            {
                requestItem.RequestItemState = new RequestItemStates(this.Strategy.Session).Quoted;

                productQuote.AddQuoteItem(
                    new QuoteItemBuilder(this.Strategy.Session)
                    .WithProduct(requestItem.Product)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.Session()).ProductItem)
                    .WithSerialisedItem(requestItem.SerialisedItem)
                    .WithProductFeature(requestItem.ProductFeature)
                    .WithQuantity(requestItem.Quantity)
                    .WithAssignedUnitPrice(requestItem.AssignedUnitPrice)
                    .WithUnitOfMeasure(requestItem.UnitOfMeasure)
                    .WithRequestItem(requestItem)
                    .WithInternalComment(requestItem.InternalComment).Build());
            }

            return productQuote;
        }
    }
}

// <copyright file="QuoteItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Resources;

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;

    public partial class QuoteItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.QuoteItem, M.QuoteItem.QuoteItemState),
            };

        public decimal LineTotal => this.Quantity * this.UnitPrice;

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public bool IsValid => !(this.QuoteItemState.IsCancelled || this.QuoteItemState.IsRejected);

        public bool WasValid => this.ExistLastObjectStates && !(this.LastQuoteItemState.IsCancelled || this.LastQuoteItemState.IsRejected);

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                method.SecurityTokens = this.SyncedQuote?.SecurityTokens.ToArray();
            }

            if (method.DeniedPermissions == null)
            {
                method.DeniedPermissions = this.SyncedQuote?.DeniedPermissions.ToArray();
            }
        }

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistQuoteItemState)
            {
                this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Submitted;
            }

            if (this.ExistProduct && !this.ExistInvoiceItemType)
            {
                this.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).ProductItem;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistQuoteWhereQuoteItem)
                {
                    iteration.AddDependency(this.QuoteWhereQuoteItem, this);
                    iteration.Mark(this.QuoteWhereQuoteItem);
                }

                if (this.ExistSerialisedItem)
                {
                    iteration.AddDependency(this.SerialisedItem, this);
                    iteration.Mark(this.SerialisedItem);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.InvoiceItemType.IsPartItem
                || this.InvoiceItemType.IsProductFeatureItem
                || this.InvoiceItemType.IsProductItem)
            {
                derivation.Validation.AssertAtLeastOne(this, M.QuoteItem.Product, M.QuoteItem.ProductFeature, M.QuoteItem.SerialisedItem, M.QuoteItem.Deliverable, M.QuoteItem.WorkEffort);
                derivation.Validation.AssertExistsAtMostOne(this, M.QuoteItem.Product, M.QuoteItem.ProductFeature, M.QuoteItem.Deliverable, M.QuoteItem.WorkEffort);
                derivation.Validation.AssertExistsAtMostOne(this, M.QuoteItem.SerialisedItem, M.QuoteItem.ProductFeature, M.QuoteItem.Deliverable, M.QuoteItem.WorkEffort);
            }
            else
            {
                this.Quantity = 1;
            }

            if (this.Product is UnifiedGood unifiedGood && unifiedGood.InventoryItemKind.Equals(new InventoryItemKinds(this.Session()).Serialised) && !this.ExistSerialisedItem)
            {
                derivation.Validation.AssertExists(this, this.Meta.SerialisedItem);
            }

            if (this.ExistSerialisedItem && this.Quantity != 1)
            {
                derivation.Validation.AddError(this, this.Meta.Quantity, ErrorMessages.SerializedItemQuantity);
            }

            if (!this.ExistDetails && this.ExistSerialisedItem)
            {
                this.Details = this.SerialisedItem.Details;
            }

            if (this.ExistRequestItem)
            {
                this.RequiredByDate = this.RequestItem.RequiredByDate;
            }

            if (!this.ExistUnitOfMeasure)
            {
                this.UnitOfMeasure = new UnitsOfMeasure(this.Strategy.Session).Piece;
            }

            this.SetAvailability(this.SerialisedItem);

            // CurrentVersion is Previous Version until PostDerive
            var previousSerialisedItem = this.CurrentVersion?.SerialisedItem;
            if (previousSerialisedItem != null && previousSerialisedItem != this.SerialisedItem)
            {
                this.SetAvailability(previousSerialisedItem);
            }
        }

        private void SetAvailability(SerialisedItem serialisedItem)
        {
            var quoted = serialisedItem?.QuoteItemsWhereSerialisedItem.Any(v => v.QuoteItemState.IsDraft
                        || v.QuoteItemState.IsSubmitted || v.QuoteItemState.IsApproved
                        || v.QuoteItemState.IsAwaitingAcceptance || v.QuoteItemState.IsAccepted);

            var ordered = serialisedItem?.SalesOrderItemsWhereSerialisedItem.Any(v => v.SalesOrderItemState.IsProvisional
                        || v.SalesOrderItemState.IsReadyForPosting || v.SalesOrderItemState.IsRequestsApproval
                        || v.SalesOrderItemState.IsAwaitingAcceptance || v.SalesOrderItemState.IsOnHold || v.SalesOrderItemState.IsInProcess);

            if (quoted.HasValue && quoted.Value)
            {
                serialisedItem.SerialisedItemAvailability = new SerialisedItemAvailabilities(this.Strategy.Session).OnQuote;
            }
            else if (ordered.HasValue && ordered.Value)
            {
                serialisedItem.SerialisedItemAvailability = new SerialisedItemAvailabilities(this.Strategy.Session).OnSalesOrder;
            }
            else if (this.ExistSerialisedItem)
            {
                serialisedItem.SerialisedItemAvailability = new SerialisedItemAvailabilities(this.Strategy.Session).Available;
            }
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistUnitPrice)
            {
                derivation.Validation.AddError(this, this.Meta.UnitPrice, ErrorMessages.UnitPriceRequired);
            }
        }

        public void BaseSend(QuoteItemSend method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).AwaitingAcceptance;

        public void BaseCancel(QuoteItemCancel method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Cancelled;

        public void BaseReject(QuoteItemReject method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Rejected;

        public void BaseOrder(QuoteItemOrder method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Ordered;

        public void BaseSubmit(QuoteItemSubmit method) => this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Submitted;

        public void Sync(Quote quote) => this.SyncedQuote = quote;
    }
}

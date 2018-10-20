// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteItem.cs" company="Allors bvba">
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
    using Meta;

    public partial class QuoteItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.QuoteItem, M.QuoteItem.QuoteItemState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistQuoteItemState)
            {
                this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Submitted;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistQuoteWhereQuoteItem)
            {
                derivation.AddDependency(this.QuoteWhereQuoteItem, this);
            }

            if (derivation.IsCreated(this) && this.ExistSerialisedInventoryItem)
            {
                this.Details = this.SerialisedInventoryItem.SerialisedItem.Details;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.QuoteItem.Product, M.QuoteItem.ProductFeature, M.QuoteItem.SerialisedInventoryItem, M.QuoteItem.Deliverable, M.QuoteItem.WorkEffort);
            derivation.Validation.AssertExistsAtMostOne(this, M.QuoteItem.Product, M.QuoteItem.ProductFeature, M.QuoteItem.SerialisedInventoryItem, M.QuoteItem.Deliverable, M.QuoteItem.WorkEffort);

            if (this.ExistRequestItem)
            {
                this.RequiredByDate = this.RequestItem.RequiredByDate;
            }

            if (!this.ExistUnitOfMeasure)
            {
                this.UnitOfMeasure = new UnitsOfMeasure(this.strategy.Session).Piece;
            }

            if (this.QuoteWhereQuoteItem.QuoteState.Equals(new QuoteStates(this.Strategy.Session).Cancelled))
            {
                if (this.QuoteItemState != new QuoteItemStates(this.Strategy.Session).Rejected)
                {
                    this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Cancelled;
                }
            }

            if (this.QuoteWhereQuoteItem.QuoteState.Equals(new QuoteStates(this.Strategy.Session).Rejected))
            {
                if (this.QuoteItemState != new QuoteItemStates(this.Strategy.Session).Cancelled)
                {
                    this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Rejected;
                }
            }
        }

        public void AppsCancel(QuoteItemCancel method)
        {
            this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Cancelled;
        }

        public void AppsReject(QuoteItemReject method)
        {
            this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Rejected;
        }

        public void AppsOrder(QuoteItemOrder method)
        {
            this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Ordered;
        }

        public void AppsSubmit(QuoteItemSubmit method)
        {
            this.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Submitted;
        }
    }
}
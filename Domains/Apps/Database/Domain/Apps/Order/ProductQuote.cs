// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQuote.cs" company="Allors bvba">
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
    using System.Linq;

    using Allors.Meta;

    public partial class ProductQuote
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.ProductQuote.QuoteState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        private SalesOrder OrderThis()
        {
            var salesOrder = new SalesOrderBuilder(this.Strategy.Session)
                .WithInternalComment(this.InternalComment)
                .WithQuote(this)
                .WithBillToCustomer(this.Receiver)
                .Build();

            var quoteItems = this.QuoteItems.Where(i => i.QuoteItemState.Equals(new QuoteItemStates(this.Strategy.Session).Submitted)).ToArray();

            foreach (QuoteItem quoteItem in quoteItems)
            {
                salesOrder.AddSalesOrderItem(
                    new SalesOrderItemBuilder(this.Strategy.Session)
                        .WithInternalComment(quoteItem.InternalComment)
                        .WithAssignedDeliveryDate(quoteItem.EstimatedDeliveryDate)
                        .WithActualUnitPrice(quoteItem.UnitPrice)
                        .WithProduct(quoteItem.Product)
                        .WithProductFeature(quoteItem.ProductFeature)
                        .WithQuantityOrdered(quoteItem.Quantity)
                        .Build()
                );
            }

            return salesOrder;
        }

        public void AppsOrder(ProductQuoteOrder Method)
        {
            this.QuoteState = new QuoteStates(this.Strategy.Session).Ordered;
            this.OrderThis();
        }
    }
}

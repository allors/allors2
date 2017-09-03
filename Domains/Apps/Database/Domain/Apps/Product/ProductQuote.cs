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

using System.Linq;

namespace Allors.Domain
{
    using System;

    public partial class ProductQuote
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        private SalesOrder OrderThis()
        {
            var salesOrder = new SalesOrderBuilder(this.Strategy.Session)
                .WithQuote(this)
                .WithBillToCustomer(this.Receiver)
                .Build();

            var quoteItems = this.QuoteItems.Where(i => i.CurrentObjectState.Equals(new QuoteItemObjectStates(this.Strategy.Session).Submitted)).ToArray();

            foreach (QuoteItem quoteItem in quoteItems)
            {
                var salesOrderItem = new SalesOrderItemBuilder(this.Strategy.Session)
                    .WithProduct(quoteItem.Product)
                    .WithProductFeature(quoteItem.ProductFeature)
                    .WithQuantityOrdered(quoteItem.Quantity)
                    .Build();

                if (quoteItem.UnitPrice > 0)
                {
                    salesOrderItem.ActualUnitPrice = quoteItem.UnitPrice;
                }

                salesOrder.AddSalesOrderItem(salesOrderItem);
            }

            return salesOrder;
        }

        public void AppsOrder(ProductQuoteOrder Method)
        {
            this.CurrentObjectState = new QuoteObjectStates(this.Strategy.Session).Ordered;
            this.OrderThis();
        }

    }
}

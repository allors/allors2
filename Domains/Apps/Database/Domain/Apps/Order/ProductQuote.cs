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
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public partial class ProductQuote
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.ProductQuote.QuoteState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var templateService = this.strategy.Session.ServiceProvider.GetRequiredService<ITemplateService>();

            var model = new PrintProductQuote
            {
                                ProductQuote = this,
                                Aviaco = this.strategy.Session.GetSingleton().InternalOrganisation
                            };

            this.PrintContent = templateService.Render("Templates/ProductQuote.cshtml", model).Result;
        }

        private SalesOrder OrderThis()
        {
            var salesOrder = new SalesOrderBuilder(this.Strategy.Session)
                .WithComment(this.Comment)
                .WithDescription(this.Description)
                .WithContactPerson(this.ContactPerson)
                .WithInternalComment(this.InternalComment)
                .WithQuote(this)
                .WithBillToCustomer(this.Receiver)
                .Build();

            var quoteItems = this.QuoteItems.Where(i => i.QuoteItemState.Equals(new QuoteItemStates(this.Strategy.Session).Submitted)).ToArray();

            foreach (QuoteItem quoteItem in quoteItems)

            {
                salesOrder.AddSalesOrderItem(
                    new SalesOrderItemBuilder(this.Strategy.Session)
                        .WithItemType(new SalesInvoiceItemTypes(this.strategy.Session).ProductItem)
                        .WithComment(quoteItem.Comment)
                        .WithInternalComment(quoteItem.InternalComment)
                        .WithAssignedDeliveryDate(quoteItem.EstimatedDeliveryDate)
                        .WithActualUnitPrice(quoteItem.UnitPrice)
                        .WithProduct(quoteItem.Product)
                        .WithProductFeature(quoteItem.ProductFeature)
                        .WithQuantityOrdered(quoteItem.Quantity).Build());
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

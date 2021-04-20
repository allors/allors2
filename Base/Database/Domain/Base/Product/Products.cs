// <copyright file="Products.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    using Allors.Meta;

    public partial class Products
    {
        private static decimal BaseCatalogPrice(
            SalesOrder salesOrder,
            SalesInvoice salesInvoice,
            Product product,
            DateTime date)
        {
            var productBasePrice = 0M;
            var productDiscount = 0M;
            var productSurcharge = 0M;

            var baseprices = new PriceComponent[0];
            if (product.ExistBasePrices)
            {
                baseprices = product.BasePrices;
            }

            var party = salesOrder != null ? salesOrder.ShipToCustomer : salesInvoice?.BillToCustomer;

            foreach (BasePrice priceComponent in baseprices)
            {
                if (priceComponent.FromDate <= date &&
                    (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= date))
                {
                    if (PriceComponents.BaseIsApplicable(new PriceComponents.IsApplicable
                    {
                        PriceComponent = priceComponent,
                        Customer = party,
                        Product = product,
                        SalesOrder = salesOrder,
                        SalesInvoice = salesInvoice,
                    }))
                    {
                        if (priceComponent.ExistPrice)
                        {
                            if (productBasePrice == 0 || priceComponent.Price < productBasePrice)
                            {
                                productBasePrice = priceComponent.Price ?? 0;
                            }
                        }
                    }
                }
            }

            var currentPriceComponents = new PriceComponents(product.Strategy.Session).CurrentPriceComponents(date);
            var priceComponents = product.GetPriceComponents(currentPriceComponents);

            foreach (var priceComponent in priceComponents)
            {
                if (priceComponent.Strategy.Class.Equals(M.DiscountComponent.ObjectType) || priceComponent.Strategy.Class.Equals(M.SurchargeComponent.ObjectType))
                {
                    if (PriceComponents.BaseIsApplicable(new PriceComponents.IsApplicable
                    {
                        PriceComponent = priceComponent,
                        Customer = party,
                        Product = product,
                        SalesOrder = salesOrder,
                        SalesInvoice = salesInvoice,
                    }))
                    {
                        if (priceComponent.Strategy.Class.Equals(M.DiscountComponent.ObjectType))
                        {
                            var discountComponent = (DiscountComponent)priceComponent;
                            decimal discount;

                            if (discountComponent.Price.HasValue)
                            {
                                discount = discountComponent.Price.Value;
                                productDiscount += discount;
                            }
                            else
                            {
                                var percentage = discountComponent.Percentage ?? 0;
                                discount = Rounder.RoundDecimal(productBasePrice * percentage / 100, 2);
                                productDiscount += discount;
                            }
                        }

                        if (priceComponent.Strategy.Class.Equals(M.SurchargeComponent.ObjectType))
                        {
                            var surchargeComponent = (SurchargeComponent)priceComponent;
                            decimal surcharge;

                            if (surchargeComponent.Price.HasValue)
                            {
                                surcharge = surchargeComponent.Price.Value;
                                productSurcharge += surcharge;
                            }
                            else
                            {
                                var percentage = surchargeComponent.Percentage ?? 0;
                                surcharge = Rounder.RoundDecimal(productBasePrice * percentage / 100, 2);
                                productSurcharge += surcharge;
                            }
                        }
                    }
                }
            }

            return productBasePrice - productDiscount + productSurcharge;
        }
    }
}

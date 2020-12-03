// <copyright file="PriceComponents.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public partial class PriceComponents
    {
        public static bool BaseIsApplicable(IsApplicable isApplicable)
        {
            var priceComponent = isApplicable.PriceComponent;
            var customer = isApplicable.Customer;
            var product = isApplicable.Product;
            var salesOrder = isApplicable.SalesOrder;
            var quantityOrdered = isApplicable.QuantityOrdered;
            var valueOrdered = isApplicable.ValueOrdered;
            var salesInvoice = isApplicable.SalesInvoice;

            var withGeographicBoundary = false;
            var geographicBoundaryValid = false;
            var withProductCategory = false;
            var productCategoryValid = false;
            var withPartyClassification = false;
            var partyClassificationValid = false;
            var withOrderKind = false;
            var orderKindValid = false;
            var withOrderQuantityBreak = false;
            var orderQuantityBreakValid = false;
            var withRevenueValueBreak = false;
            var revenueValueBreakValid = false;
            var withRevenueQuantityBreak = false;
            var revenueQuantityBreakValid = false;
            var withPackageQuantityBreak = false;
            var packageQuantityBreakValid = false;
            var withOrderValue = false;
            var orderValueValid = false;
            var withSalesChannel = false;
            var salesChannelValid = false;

            if (priceComponent.ExistGeographicBoundary)
            {
                withGeographicBoundary = true;

                PostalAddress postalAddress = null;
                if (salesOrder != null && salesOrder.ExistDerivedShipToAddress)
                {
                    postalAddress = salesOrder.DerivedShipToAddress;
                }

                if (salesInvoice != null && salesInvoice.ExistDerivedShipToAddress)
                {
                    postalAddress = salesInvoice.DerivedShipToAddress;
                }

                if (postalAddress == null && customer != null)
                {
                    postalAddress = customer.ShippingAddress;
                }

                if (postalAddress != null)
                {
                    foreach (GeographicBoundary geographicBoundary in postalAddress.PostalAddressBoundaries)
                    {
                        if (geographicBoundary.Equals(priceComponent.GeographicBoundary))
                        {
                            geographicBoundaryValid = true;
                        }
                    }
                }
            }

            if (priceComponent.ExistPartyClassification && customer != null)
            {
                withPartyClassification = true;

                foreach (PartyClassification partyClassification in customer.PartyClassifications)
                {
                    if (partyClassification.Equals(priceComponent.PartyClassification))
                    {
                        partyClassificationValid = true;
                    }
                }
            }

            if (priceComponent.ExistProductCategory)
            {
                withProductCategory = true;

                foreach (ProductCategory productCategory in product.ProductCategoriesWhereProduct)
                {
                    if (productCategory.Equals(priceComponent.ProductCategory))
                    {
                        productCategoryValid = true;
                    }
                }

                if (productCategoryValid == false)
                {
                    foreach (ProductCategory productCategory in product.ProductCategoriesWhereProduct)
                    {
                        foreach (ProductCategory ancestor in productCategory.ProductCategoriesWhereDescendant)
                        {
                            if (ancestor.Equals(priceComponent.ProductCategory))
                            {
                                productCategoryValid = true;
                            }
                        }
                    }
                }
            }

            if (priceComponent.ExistOrderKind)
            {
                withOrderKind = true;

                if (salesOrder != null && salesOrder.ExistOrderKind && salesOrder.OrderKind.Equals(priceComponent.OrderKind))
                {
                    orderKindValid = true;
                }
            }

            if (priceComponent.ExistSalesChannel)
            {
                withSalesChannel = true;

                SalesChannel channel = null;
                if (salesOrder != null)
                {
                    channel = salesOrder.SalesChannel;
                }

                if (salesInvoice != null)
                {
                    channel = salesInvoice.SalesChannel;
                }

                if (channel.Equals(priceComponent.SalesChannel))
                {
                    salesChannelValid = true;
                }
            }

            if (priceComponent.ExistOrderQuantityBreak)
            {
                withOrderQuantityBreak = true;

                if ((!priceComponent.OrderQuantityBreak.ExistFromAmount || priceComponent.OrderQuantityBreak.FromAmount <= quantityOrdered) &&
                    (!priceComponent.OrderQuantityBreak.ExistThroughAmount || priceComponent.OrderQuantityBreak.ThroughAmount >= quantityOrdered))
                {
                    orderQuantityBreakValid = true;
                }
            }

            if (priceComponent.ExistOrderValue)
            {
                withOrderValue = true;

                if ((!priceComponent.OrderValue.ExistFromAmount || priceComponent.OrderValue.FromAmount <= valueOrdered) &&
                    (!priceComponent.OrderValue.ExistThroughAmount || priceComponent.OrderValue.ThroughAmount >= valueOrdered))
                {
                    orderValueValid = true;
                }
            }

            if ((withGeographicBoundary && !geographicBoundaryValid) ||
                (withPartyClassification && !partyClassificationValid) ||
                (withProductCategory && !productCategoryValid) ||
                (withOrderKind && !orderKindValid) ||
                (withOrderQuantityBreak && !orderQuantityBreakValid) ||
                (withRevenueValueBreak && !revenueValueBreakValid) ||
                (withRevenueQuantityBreak && !revenueQuantityBreakValid) ||
                (withPackageQuantityBreak && !packageQuantityBreakValid) ||
                (withOrderValue && !orderValueValid) ||
                (withSalesChannel & !salesChannelValid))
            {
                return false;
            }

            return true;
        }

        public PriceComponent[] CurrentPriceComponents(DateTime date) =>
            this.Extent()
                .Where(v => v.FromDate <= date && (!v.ExistThroughDate || v.ThroughDate >= date))
                .ToArray();

        public partial class IsApplicable
        {
            public PriceComponent PriceComponent;

            public Party Customer;

            public Product Product;

            public SalesOrder SalesOrder = null;

            public decimal QuantityOrdered = 0;

            public decimal ValueOrdered = 0;

            public SalesInvoice SalesInvoice = null;
        }
    }
}

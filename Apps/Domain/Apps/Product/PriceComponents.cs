// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriceComponents.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System.Collections.Generic;

    public partial class PriceComponents
    {
        public partial class IsEligibleParams
        {
            public PriceComponent PriceComponent;

            public Party Customer;

            public Product Product;

            public SalesOrder SalesOrder = null;

            public decimal QuantityOrdered = 0;

            public decimal ValueOrdered = 0;

            public IEnumerable<PartyPackageRevenueHistory> PartyPackageRevenueHistoryList;

            public PartyRevenueHistory PartyRevenueHistory;

            public Dictionary<ProductCategory, PartyProductCategoryRevenueHistory> PartyProductCategoryRevenueHistoryByProductCategory;

            public SalesInvoice SalesInvoice = null;
        }

        private static bool AppsIsEligible(IsEligibleParams isEligibleParams)
        {
            var priceComponent = isEligibleParams.PriceComponent;
            var customer = isEligibleParams.Customer;
            var product = isEligibleParams.Product;
            var salesOrder = isEligibleParams.SalesOrder;
            var quantityOrdered = isEligibleParams.QuantityOrdered;
            var valueOrdered = isEligibleParams.ValueOrdered;
            var partyPackageRevenueHistoryList = isEligibleParams.PartyPackageRevenueHistoryList;
            var partyRevenueHistory = isEligibleParams.PartyRevenueHistory;
            var partyProductCategoryRevenueHistoryByProductCategory = isEligibleParams.PartyProductCategoryRevenueHistoryByProductCategory;
            var salesInvoice = isEligibleParams.SalesInvoice;

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
            var withSpecifiedFor = false;
            var specifiedForValid = false;

            if (priceComponent.ExistGeographicBoundary)
            {
                withGeographicBoundary = true;

                PostalAddress postalAddress = null;
                if (salesOrder != null && salesOrder.ExistShipToAddress)
                {
                    postalAddress = salesOrder.ShipToAddress;
                }

                if (salesInvoice != null && salesInvoice.ExistShipToAddress)
                {
                    postalAddress = salesInvoice.ShipToAddress;
                }

                if (postalAddress == null && customer != null)
                {
                    postalAddress = customer.ShippingAddress;
                }

                if (postalAddress != null)
                {
                    foreach (GeographicBoundary geographicBoundary in postalAddress.GeographicBoundaries)
                    {
                        if (geographicBoundary.Equals(priceComponent.GeographicBoundary))
                        {
                            geographicBoundaryValid = true;
                        }
                    }
                }
            }

            if (priceComponent.ExistSpecifiedFor)
            {
                withSpecifiedFor = true;

                InternalOrganisation specifiedFor = null;
                if (salesOrder != null)
                {
                    specifiedFor = salesOrder.TakenByInternalOrganisation;
                }

                if (salesInvoice != null)
                {
                    specifiedFor = salesInvoice.BilledFromInternalOrganisation;
                }

                if (specifiedFor != null && specifiedFor.Equals(priceComponent.SpecifiedFor))
                {
                    specifiedForValid = true;
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

                foreach (ProductCategory productCategory in product.ProductCategories)
                {
                    if (productCategory.Equals(priceComponent.ProductCategory))
                    {
                        productCategoryValid = true;
                    }
                }

                if (productCategoryValid == false)
                {
                    foreach (ProductCategory productCategory in product.ProductCategories)
                    {
                        foreach (ProductCategory ancestor in productCategory.Ancestors)
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

            if (priceComponent.ExistRevenueValueBreak)
            {
                withRevenueValueBreak = true;

                var revenueValueBreak = priceComponent.RevenueValueBreak;

                var revenue = 0M;
                if (priceComponent.ExistProductCategory && partyProductCategoryRevenueHistoryByProductCategory != null)
                {
                    if (partyProductCategoryRevenueHistoryByProductCategory.ContainsKey(priceComponent.ProductCategory))
                    {
                        revenue = partyProductCategoryRevenueHistoryByProductCategory[priceComponent.ProductCategory].Revenue;
                    }
                }
                else
                {
                    if (partyRevenueHistory != null)
                    {
                        revenue = partyRevenueHistory.Revenue;
                    }
                }

                if ((!revenueValueBreak.ExistFromAmount || revenueValueBreak.FromAmount <= revenue) &&
                    (!revenueValueBreak.ExistThroughAmount || revenueValueBreak.ThroughAmount >= revenue))
                {
                    revenueValueBreakValid = true;
                }
            }

            if (priceComponent.ExistRevenueQuantityBreak)
            {
                withRevenueQuantityBreak = true;

                var revenueQuantityBreak = priceComponent.RevenueQuantityBreak;

                var quantity = 0M;
                if (priceComponent.ExistProductCategory && partyProductCategoryRevenueHistoryByProductCategory != null)
                {
                    if (partyProductCategoryRevenueHistoryByProductCategory.ContainsKey(priceComponent.ProductCategory))
                    {
                        quantity = partyProductCategoryRevenueHistoryByProductCategory[priceComponent.ProductCategory].Quantity;
                    }
                }

                if ((!revenueQuantityBreak.ExistFrom || revenueQuantityBreak.From <= quantity) &&
                    (!revenueQuantityBreak.ExistThrough || revenueQuantityBreak.Through >= quantity))
                {
                    revenueQuantityBreakValid = true;
                }
            }

            if (priceComponent.ExistPackageQuantityBreak)
            {
                withPackageQuantityBreak = true;

                var packageQuantityBreak = priceComponent.PackageQuantityBreak;

                var quantity = 0;
                if (partyPackageRevenueHistoryList != null)
                {
                    foreach (var partyPackageRevenueHistory in partyPackageRevenueHistoryList)
                    {
                        if (partyPackageRevenueHistory.Revenue > 0)
                        {
                            quantity++;
                        }
                    }
                }

                if ((!packageQuantityBreak.ExistFrom || packageQuantityBreak.From <= quantity) &&
                    (!packageQuantityBreak.ExistThrough || packageQuantityBreak.Through >= quantity))
                {
                    packageQuantityBreakValid = true;
                }
            }

            if ((withGeographicBoundary && !geographicBoundaryValid) ||
                (withSpecifiedFor && !specifiedForValid) ||
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
    }
}
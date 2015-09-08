// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCategoryRevenueHistories.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;

    public partial class ProductCategoryRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var derivation = new Derivation(session);

            var productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<ProductCategory, Dictionary<DateTime, ProductCategoryRevenue>>>();

            var productCategoryRevenues = session.Extent<ProductCategoryRevenue>();

            foreach (ProductCategoryRevenue productCategoryRevenue in productCategoryRevenues)
            {
                var months = ((DateTime.UtcNow.Year - productCategoryRevenue.Year) * 12) + DateTime.UtcNow.Month - productCategoryRevenue.Month;
                if (months <= 12)
                {
                    var date = DateTimeFactory.CreateDate(productCategoryRevenue.Year, productCategoryRevenue.Month, 01);

                    Dictionary<ProductCategory, Dictionary<DateTime, ProductCategoryRevenue>> productCategoryRevenuesByPeriodByProductCategory;
                    if (!productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation.TryGetValue(productCategoryRevenue.InternalOrganisation, out productCategoryRevenuesByPeriodByProductCategory))
                    {
                        productCategoryRevenuesByPeriodByProductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, ProductCategoryRevenue>>();
                        productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation[productCategoryRevenue.InternalOrganisation] = productCategoryRevenuesByPeriodByProductCategory;
                    }

                    Dictionary<DateTime, ProductCategoryRevenue> productCategoryRevenuesByPeriod;
                    if (!productCategoryRevenuesByPeriodByProductCategory.TryGetValue(productCategoryRevenue.ProductCategory, out productCategoryRevenuesByPeriod))
                    {
                        productCategoryRevenuesByPeriod = new Dictionary<DateTime, ProductCategoryRevenue>();
                        productCategoryRevenuesByPeriodByProductCategory[productCategoryRevenue.ProductCategory] = productCategoryRevenuesByPeriod;
                    }

                    ProductCategoryRevenue revenue;
                    if (!productCategoryRevenuesByPeriod.TryGetValue(date, out revenue))
                    {
                        productCategoryRevenuesByPeriod.Add(date, productCategoryRevenue);
                    }
                }
            }

            var productCategoryRevenueHistoriesByProductCategoryByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<ProductCategory, ProductCategoryRevenueHistory>>();

            var productCategoryRevenueHistories = session.Extent<ProductCategoryRevenueHistory>();

            foreach (ProductCategoryRevenueHistory productCategoryRevenueHistory in productCategoryRevenueHistories)
            {
                productCategoryRevenueHistory.Revenue = 0;

                Dictionary<ProductCategory, ProductCategoryRevenueHistory> productCategoryRevenueHistoriesByProductCategory;
                if (!productCategoryRevenueHistoriesByProductCategoryByInternalOrganisation.TryGetValue(productCategoryRevenueHistory.InternalOrganisation, out productCategoryRevenueHistoriesByProductCategory))
                {
                    productCategoryRevenueHistoriesByProductCategory = new Dictionary<ProductCategory, ProductCategoryRevenueHistory>();
                    productCategoryRevenueHistoriesByProductCategoryByInternalOrganisation[productCategoryRevenueHistory.InternalOrganisation] = productCategoryRevenueHistoriesByProductCategory;
                }

                ProductCategoryRevenueHistory revenueHistory;
                if (!productCategoryRevenueHistoriesByProductCategory.TryGetValue(productCategoryRevenueHistory.ProductCategory, out revenueHistory))
                {
                    productCategoryRevenueHistoriesByProductCategory.Add(productCategoryRevenueHistory.ProductCategory, productCategoryRevenueHistory);
                }
            }

            foreach (var keyValuePair in productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation)
            {
                Dictionary<ProductCategory, ProductCategoryRevenueHistory> productCategoryRevenueHistoriesByProductCategory;
                if (!productCategoryRevenueHistoriesByProductCategoryByInternalOrganisation.TryGetValue(keyValuePair.Key, out productCategoryRevenueHistoriesByProductCategory))
                {
                    productCategoryRevenueHistoriesByProductCategory = new Dictionary<ProductCategory, ProductCategoryRevenueHistory>();
                    productCategoryRevenueHistoriesByProductCategoryByInternalOrganisation[keyValuePair.Key] = productCategoryRevenueHistoriesByProductCategory;
                }

                foreach (var valuePair in keyValuePair.Value)
                {
                    ProductCategoryRevenueHistory productCategoryRevenueHistory;

                    if (!productCategoryRevenueHistoriesByProductCategory.TryGetValue(valuePair.Key, out productCategoryRevenueHistory))
                    {
                        ProductCategoryRevenue partyRevenue = null;
                        foreach (var productCategoryRevenuesByPeriod in valuePair.Value)
                        {
                            partyRevenue = productCategoryRevenuesByPeriod.Value;
                            break;
                        }

                        productCategoryRevenueHistory = CreateProductCategoryRevenueHistory(session, partyRevenue);
                        productCategoryRevenueHistoriesByProductCategory.Add(productCategoryRevenueHistory.ProductCategory, productCategoryRevenueHistory);
                    }

                    foreach (var partyRevenueByPeriod in valuePair.Value)
                    {
                        var partyRevenue = partyRevenueByPeriod.Value;
                        productCategoryRevenueHistory.Revenue += partyRevenue.Revenue;
                    }

                    productCategoryRevenueHistory.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static ProductCategoryRevenueHistory CreateProductCategoryRevenueHistory(ISession session, ProductCategoryRevenue productCategoryRevenue)
        {
            return new ProductCategoryRevenueHistoryBuilder(session)
                        .WithCurrency(productCategoryRevenue.Currency)
                        .WithInternalOrganisation(productCategoryRevenue.InternalOrganisation)
                        .WithProductCategory(productCategoryRevenue.ProductCategory)
                        .WithRevenue(0)
                        .Build();
        }
    }
}

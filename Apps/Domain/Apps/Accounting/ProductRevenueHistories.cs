// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductRevenueHistories.cs" company="Allors bvba">
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


    public partial class ProductRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var derivation = new Derivation(session);

            var productRevenuesByPeriodByProductByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Product, Dictionary<DateTime, ProductRevenue>>>();

            var productRevenues = session.Extent<ProductRevenue>();

            foreach (ProductRevenue productRevenue in productRevenues)
            {
                var months = ((DateTime.UtcNow.Year - productRevenue.Year) * 12) + DateTime.UtcNow.Month - productRevenue.Month;
                if (months <= 12)
                {
                    var date = DateTimeFactory.CreateDate(productRevenue.Year, productRevenue.Month, 01);

                    Dictionary<Product, Dictionary<DateTime, ProductRevenue>> productRevenuesByPeriodByProduct;
                    if (!productRevenuesByPeriodByProductByInternalOrganisation.TryGetValue(productRevenue.InternalOrganisation, out productRevenuesByPeriodByProduct))
                    {
                        productRevenuesByPeriodByProduct = new Dictionary<Product, Dictionary<DateTime, ProductRevenue>>();
                        productRevenuesByPeriodByProductByInternalOrganisation[productRevenue.InternalOrganisation] = productRevenuesByPeriodByProduct;
                    }

                    Dictionary<DateTime, ProductRevenue> productRevenuesByPeriod;
                    if (!productRevenuesByPeriodByProduct.TryGetValue(productRevenue.Product, out productRevenuesByPeriod))
                    {
                        productRevenuesByPeriod = new Dictionary<DateTime, ProductRevenue>();
                        productRevenuesByPeriodByProduct[productRevenue.Product] = productRevenuesByPeriod;
                    }

                    ProductRevenue revenue;
                    if (!productRevenuesByPeriod.TryGetValue(date, out revenue))
                    {
                        productRevenuesByPeriod.Add(date, productRevenue);
                    }
                }
            }

            var productRevenueHistoriesByProductByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Product, ProductRevenueHistory>>();

            var productRevenueHistories = session.Extent<ProductRevenueHistory>();

            foreach (ProductRevenueHistory productRevenueHistory in productRevenueHistories)
            {
                productRevenueHistory.Revenue = 0;

                Dictionary<Product, ProductRevenueHistory> productRevenueHistoriesByProduct;
                if (!productRevenueHistoriesByProductByInternalOrganisation.TryGetValue(productRevenueHistory.InternalOrganisation, out productRevenueHistoriesByProduct))
                {
                    productRevenueHistoriesByProduct = new Dictionary<Product, ProductRevenueHistory>();
                    productRevenueHistoriesByProductByInternalOrganisation[productRevenueHistory.InternalOrganisation] = productRevenueHistoriesByProduct;
                }

                ProductRevenueHistory revenueHistory;
                if (!productRevenueHistoriesByProduct.TryGetValue(productRevenueHistory.Product, out revenueHistory))
                {
                    productRevenueHistoriesByProduct.Add(productRevenueHistory.Product, productRevenueHistory);
                }
            }

            foreach (var keyValuePair in productRevenuesByPeriodByProductByInternalOrganisation)
            {
                Dictionary<Product, ProductRevenueHistory> productRevenueHistoriesByProduct;
                if (!productRevenueHistoriesByProductByInternalOrganisation.TryGetValue(keyValuePair.Key, out productRevenueHistoriesByProduct))
                {
                    productRevenueHistoriesByProduct = new Dictionary<Product, ProductRevenueHistory>();
                    productRevenueHistoriesByProductByInternalOrganisation[keyValuePair.Key] = productRevenueHistoriesByProduct;
                }

                foreach (var valuePair in keyValuePair.Value)
                {
                    ProductRevenueHistory productRevenueHistory;

                    if (!productRevenueHistoriesByProduct.TryGetValue(valuePair.Key, out productRevenueHistory))
                    {
                        ProductRevenue partyRevenue = null;
                        foreach (var productRevenuesByPeriod in valuePair.Value)
                        {
                            partyRevenue = productRevenuesByPeriod.Value;
                            break;
                        }

                        productRevenueHistory = CreateProductRevenueHistory(session, partyRevenue);
                        productRevenueHistoriesByProduct.Add(productRevenueHistory.Product, productRevenueHistory);
                    }

                    foreach (var partyRevenueByPeriod in valuePair.Value)
                    {
                        var partyRevenue = partyRevenueByPeriod.Value;
                        productRevenueHistory.Revenue += partyRevenue.Revenue;
                    }

                    productRevenueHistory.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static ProductRevenueHistory CreateProductRevenueHistory(ISession session, ProductRevenue productRevenue)
        {
            return new ProductRevenueHistoryBuilder(session)
                        .WithCurrency(productRevenue.Currency)
                        .WithInternalOrganisation(productRevenue.InternalOrganisation)
                        .WithProduct(productRevenue.Product)
                        .WithRevenue(0)
                        .Build();
        }
    }
}

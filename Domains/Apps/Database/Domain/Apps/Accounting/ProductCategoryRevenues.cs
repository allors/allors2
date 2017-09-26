// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCategoryRevenues.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using Meta;

    public partial class ProductCategoryRevenues
    {
        public static ProductCategoryRevenue AppsFindOrCreateAsDependable(ISession session, PartyProductCategoryRevenue dependant)
        {
            var productCategoryRevenues = dependant.ProductCategory.ProductCategoryRevenuesWhereProductCategory;
            productCategoryRevenues.Filter.AddEquals(M.ProductCategoryRevenue.Year, dependant.Year);
            productCategoryRevenues.Filter.AddEquals(M.ProductCategoryRevenue.Month, dependant.Month);
            var productCategoryRevenue = productCategoryRevenues.First ?? new ProductCategoryRevenueBuilder(session)
                                                                                .WithProductCategory(dependant.ProductCategory)
                                                                                .WithYear(dependant.Year)
                                                                                .WithMonth(dependant.Month)
                                                                                .WithCurrency(dependant.Currency)
                                                                                .Build();
            return productCategoryRevenue;
        }

        public static void AppsFindOrCreateAsDependable(ISession session, ProductCategoryRevenue dependant)
        {
            foreach (ProductCategory parentCategory in dependant.ProductCategory.Parents)
            {
                var productCategoryRevenues = parentCategory.ProductCategoryRevenuesWhereProductCategory;
                productCategoryRevenues.Filter.AddEquals(M.ProductCategoryRevenue.Year, dependant.Year);
                productCategoryRevenues.Filter.AddEquals(M.ProductCategoryRevenue.Month, dependant.Month);
                var productCategoryRevenue = productCategoryRevenues.First ?? new ProductCategoryRevenueBuilder(session)
                                                                                    .WithProductCategory(parentCategory)
                                                                                    .WithYear(dependant.Year)
                                                                                    .WithMonth(dependant.Month)
                                                                                    .WithCurrency(dependant.Currency)
                                                                                    .Build();

                AppsFindOrCreateAsDependable(session, productCategoryRevenue);
            }
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var productCategoryRevenuesByPeriodByProductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, ProductCategoryRevenue>>();

            var productCategoryRevenues = session.Extent<ProductCategoryRevenue>();

            foreach (ProductCategoryRevenue productCategoryRevenue in productCategoryRevenues)
            {
                productCategoryRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(productCategoryRevenue.Year, productCategoryRevenue.Month, 01);
                
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

            var revenues = new HashSet<long>();

            var salesInvoices = session.Extent<SalesInvoice>();
            var year = 0;
            foreach (SalesInvoice salesInvoice in salesInvoices)
            {
                if (year != salesInvoice.InvoiceDate.Year)
                {
                    session.Commit();
                    year = salesInvoice.InvoiceDate.Year;
                }

                var date = DateTimeFactory.CreateDate(salesInvoice.InvoiceDate.Year, salesInvoice.InvoiceDate.Month, 01);

                foreach (SalesInvoiceItem salesInvoiceItem in salesInvoice.SalesInvoiceItems)
                {
                    if (salesInvoiceItem.ExistProduct && salesInvoiceItem.Product.ExistPrimaryProductCategory)
                    {
                        foreach (ProductCategory productCategory in salesInvoiceItem.Product.ProductCategories)
                        {
                            CreateProductCategoryRevenue(session, productCategoryRevenuesByPeriodByProductCategory, date, revenues, salesInvoiceItem, productCategory);
                        }
                    }
                }
            }

            foreach (ProductCategoryRevenue productCategoryRevenue in productCategoryRevenues)
            {
                if (!revenues.Contains(productCategoryRevenue.Id))
                {
                    productCategoryRevenue.Delete();
                }
            }
        }

        private static void CreateProductCategoryRevenue(
            ISession session,
            Dictionary<ProductCategory, Dictionary<DateTime, ProductCategoryRevenue>> productCategoryRevenuesByPeriodByProductCategory,
            DateTime date,
            HashSet<long> revenues,
            SalesInvoiceItem salesInvoiceItem,
            ProductCategory productCategory)
        {
            ProductCategoryRevenue productCategoryRevenue;
            Dictionary<DateTime, ProductCategoryRevenue> productCategoryRevenuesByPeriod;
            if (!productCategoryRevenuesByPeriodByProductCategory.TryGetValue(productCategory, out productCategoryRevenuesByPeriod))
            {
                productCategoryRevenue = CreateProductCategoryRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, productCategory);

                productCategoryRevenuesByPeriod = new Dictionary<DateTime, ProductCategoryRevenue> { { date, productCategoryRevenue } };

                productCategoryRevenuesByPeriodByProductCategory[productCategory] = productCategoryRevenuesByPeriod;
            }

            if (!productCategoryRevenuesByPeriod.TryGetValue(date, out productCategoryRevenue))
            {
                productCategoryRevenue = CreateProductCategoryRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, productCategory);
                productCategoryRevenuesByPeriod.Add(date, productCategoryRevenue);
            }

            revenues.Add(productCategoryRevenue.Id);
            productCategoryRevenue.Revenue += salesInvoiceItem.TotalExVat;

            foreach (ProductCategory parent in productCategory.Parents)
            {
                CreateProductCategoryRevenue(session, productCategoryRevenuesByPeriodByProductCategory, date, revenues, salesInvoiceItem, parent);
            }
        }

        private static ProductCategoryRevenue CreateProductCategoryRevenue(ISession session, SalesInvoice invoice, ProductCategory productCategory)
        {
            return new ProductCategoryRevenueBuilder(session)
                        .WithProductCategory(productCategory)
                        .WithProductCategoryName(productCategory.Description)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(Singleton.Instance(session).PreferredCurrency)
                        .Build();
        }
    }
}

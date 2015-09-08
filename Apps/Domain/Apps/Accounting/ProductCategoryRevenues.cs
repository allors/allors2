// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCategoryRevenues.cs" company="Allors bvba">
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


    public partial class ProductCategoryRevenues
    {
        public static ProductCategoryRevenue AppsFindOrCreateAsDependable(ISession session, PartyProductCategoryRevenue dependant)
        {
            var productCategoryRevenues = dependant.ProductCategory.ProductCategoryRevenuesWhereProductCategory;
            productCategoryRevenues.Filter.AddEquals(ProductCategoryRevenues.Meta.InternalOrganisation, dependant.InternalOrganisation);
            productCategoryRevenues.Filter.AddEquals(ProductCategoryRevenues.Meta.Year, dependant.Year);
            productCategoryRevenues.Filter.AddEquals(ProductCategoryRevenues.Meta.Month, dependant.Month);
            var productCategoryRevenue = productCategoryRevenues.First ?? new ProductCategoryRevenueBuilder(session)
                                                                                .WithInternalOrganisation(dependant.InternalOrganisation)
                                                                                .WithProductCategory(dependant.ProductCategory)
                                                                                .WithYear(dependant.Year)
                                                                                .WithMonth(dependant.Month)
                                                                                .WithCurrency(dependant.Currency)
                                                                                .WithRevenue(0M)
                                                                                .Build();
            return productCategoryRevenue;
        }

        public static void AppsFindOrCreateAsDependable(ISession session, ProductCategoryRevenue dependant)
        {
            foreach (ProductCategory parentCategory in dependant.ProductCategory.Parents)
            {
                var productCategoryRevenues = parentCategory.ProductCategoryRevenuesWhereProductCategory;
                productCategoryRevenues.Filter.AddEquals(ProductCategoryRevenues.Meta.InternalOrganisation, dependant.InternalOrganisation);
                productCategoryRevenues.Filter.AddEquals(ProductCategoryRevenues.Meta.Year, dependant.Year);
                productCategoryRevenues.Filter.AddEquals(ProductCategoryRevenues.Meta.Month, dependant.Month);
                var productCategoryRevenue = productCategoryRevenues.First ?? new ProductCategoryRevenueBuilder(session)
                                                                                    .WithInternalOrganisation(dependant.InternalOrganisation)
                                                                                    .WithProductCategory(parentCategory)
                                                                                    .WithYear(dependant.Year)
                                                                                    .WithMonth(dependant.Month)
                                                                                    .WithCurrency(dependant.Currency)
                                                                                    .WithRevenue(0M)
                                                                                    .Build();

                AppsFindOrCreateAsDependable(session, productCategoryRevenue);
            }
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation =
                new Dictionary<InternalOrganisation, Dictionary<ProductCategory, Dictionary<DateTime, ProductCategoryRevenue>>>();

            var productCategoryRevenues = session.Extent<ProductCategoryRevenue>();

            foreach (ProductCategoryRevenue productCategoryRevenue in productCategoryRevenues)
            {
                productCategoryRevenue.Revenue = 0;
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

            var revenues = new HashSet<ObjectId>();

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
                            CreateProductCategoryRevenue(session, productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation, date, revenues, salesInvoiceItem, productCategory);
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

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static void CreateProductCategoryRevenue(
            ISession session,
            Dictionary<InternalOrganisation, Dictionary<ProductCategory, Dictionary<DateTime, ProductCategoryRevenue>>> productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation,
            DateTime date,
            HashSet<ObjectId> revenues,
            SalesInvoiceItem salesInvoiceItem,
            ProductCategory productCategory)
        {
            ProductCategoryRevenue productCategoryRevenue;

            Dictionary<ProductCategory, Dictionary<DateTime, ProductCategoryRevenue>> productCategoryRevenuesByPeriodByProductCategory;
            if (!productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation.TryGetValue(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation, out productCategoryRevenuesByPeriodByProductCategory))
            {
                productCategoryRevenue = CreateProductCategoryRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, productCategory);

                productCategoryRevenuesByPeriodByProductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, ProductCategoryRevenue>>
                        {
                            {
                                productCategory,
                                new Dictionary<DateTime, ProductCategoryRevenue>
                                    {
                                        { date, productCategoryRevenue }
                                    }
                            }
                        };

                productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation[salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation] = productCategoryRevenuesByPeriodByProductCategory;
            }

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
                CreateProductCategoryRevenue(session, productCategoryRevenuesByPeriodByProductCategoryByInternalOrganisation, date, revenues, salesInvoiceItem, parent);
            }
        }

        private static ProductCategoryRevenue CreateProductCategoryRevenue(ISession session, SalesInvoice invoice, ProductCategory productCategory)
        {
            return new ProductCategoryRevenueBuilder(session)
                        .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                        .WithProductCategory(productCategory)
                        .WithProductCategoryName(productCategory.Description)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithRevenue(0M)
                        .Build();
        }
    }
}

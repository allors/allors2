// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepProductCategoryRevenues.cs" company="Allors bvba">
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


    public partial class SalesRepProductCategoryRevenues
    {
        public static SalesRepProductCategoryRevenue AppsFindOrCreateAsDependable(ISession session, SalesRepPartyProductCategoryRevenue dependant)
        {
            var salesRepProductCategoryRevenues = dependant.SalesRep.SalesRepProductCategoryRevenuesWhereSalesRep;
            salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.ProductCategory, dependant.ProductCategory);
            salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.Year, dependant.Year);
            salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.Month, dependant.Month);
            var salesRepProductCategoryRevenue = salesRepProductCategoryRevenues.First
                                                 ?? new SalesRepProductCategoryRevenueBuilder(session)
                                                                .WithProductCategory(dependant.ProductCategory)
                                                                .WithSalesRep(dependant.SalesRep)
                                                                .WithYear(dependant.Year)
                                                                .WithMonth(dependant.Month)
                                                                .WithCurrency(dependant.Currency)
                                                                .Build();
            return salesRepProductCategoryRevenue;
        }

        public static void AppsFindOrCreateAsDependable(ISession session, SalesRepProductCategoryRevenue dependant)
        {
            foreach (ProductCategory parentCategory in dependant.ProductCategory.Parents)
            {
                var salesRepProductCategoryRevenues = parentCategory.SalesRepProductCategoryRevenuesWhereProductCategory;
                salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.SalesRep, dependant.SalesRep);
                salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.Year, dependant.Year);
                salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.Month, dependant.Month);
                var salesRepProductCategoryRevenue = salesRepProductCategoryRevenues.First
                                                        ?? new SalesRepProductCategoryRevenueBuilder(session)
                                                                .WithSalesRep(dependant.SalesRep)
                                                                .WithProductCategory(parentCategory)
                                                                .WithYear(dependant.Year)
                                                                .WithMonth(dependant.Month)
                                                                .WithCurrency(dependant.Currency)
                                                                .Build();

                AppsFindOrCreateAsDependable(session, salesRepProductCategoryRevenue);
            }
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep = new Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>>();

            var salesRepProductCategoryRevenues = session.Extent<SalesRepProductCategoryRevenue>();
            foreach (SalesRepProductCategoryRevenue salesRepProductCategoryRevenue in salesRepProductCategoryRevenues)
            {
                salesRepProductCategoryRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(salesRepProductCategoryRevenue.Year, salesRepProductCategoryRevenue.Month, 01);

                Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>> salesRepProductCategoryRevenuesByPeriodByProductCategory;
                if (!salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep.TryGetValue(salesRepProductCategoryRevenue.SalesRep, out salesRepProductCategoryRevenuesByPeriodByProductCategory))
                {
                    salesRepProductCategoryRevenuesByPeriodByProductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>();
                    salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep[salesRepProductCategoryRevenue.SalesRep] = salesRepProductCategoryRevenuesByPeriodByProductCategory;
                }

                Dictionary<DateTime, SalesRepProductCategoryRevenue> salesRepProductCategoryRevenuesByPeriod;
                if (!salesRepProductCategoryRevenuesByPeriodByProductCategory.TryGetValue(salesRepProductCategoryRevenue.ProductCategory, out salesRepProductCategoryRevenuesByPeriod))
                {
                    salesRepProductCategoryRevenuesByPeriod = new Dictionary<DateTime, SalesRepProductCategoryRevenue>();
                    salesRepProductCategoryRevenuesByPeriodByProductCategory[salesRepProductCategoryRevenue.ProductCategory] = salesRepProductCategoryRevenuesByPeriod;
                }

                SalesRepProductCategoryRevenue revenue;
                if (!salesRepProductCategoryRevenuesByPeriod.TryGetValue(date, out revenue))
                {
                    salesRepProductCategoryRevenuesByPeriod.Add(date, salesRepProductCategoryRevenue);
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
                    if (salesInvoiceItem.ExistSalesRep && salesInvoiceItem.ExistProduct && salesInvoiceItem.Product.ExistPrimaryProductCategory)
                    {
                        foreach (ProductCategory productCategory in salesInvoiceItem.Product.ProductCategories)
                        {
                            CreateProductCategoryRevenue(session, salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep, date, revenues, salesInvoiceItem, productCategory);
                        }
                    }
                }
            }

            foreach (SalesRepProductCategoryRevenue salesRepProductCategoryRevenue in salesRepProductCategoryRevenues)
            {
                if (!revenues.Contains(salesRepProductCategoryRevenue.Id))
                {
                    salesRepProductCategoryRevenue.Delete();
                }
            }
        }

        private static void CreateProductCategoryRevenue(
            ISession session,
            Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>> salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep,
            DateTime date,
            HashSet<long> revenues,
            SalesInvoiceItem salesInvoiceItem,
            ProductCategory productCategory)
        {
            SalesRepProductCategoryRevenue salesRepProductCategoryRevenue;

            Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>> salesRepProductCategoryRevenuesByPeriodByProductCategory;
            if (!salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep.TryGetValue(salesInvoiceItem.SalesRep, out salesRepProductCategoryRevenuesByPeriodByProductCategory))
            {
                salesRepProductCategoryRevenue = CreateSalesRepProductCategoryRevenue(session, salesInvoiceItem, productCategory);

                salesRepProductCategoryRevenuesByPeriodByProductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>
                        {
                            {
                                productCategory, new Dictionary<DateTime, SalesRepProductCategoryRevenue>
                                    {
                                        { date, salesRepProductCategoryRevenue }
                                    }
                                }
                        };

                salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep[salesInvoiceItem.SalesRep] = salesRepProductCategoryRevenuesByPeriodByProductCategory;
            }

            Dictionary<DateTime, SalesRepProductCategoryRevenue> salesRepProductCategoryRevenuesByPeriod;
            if (!salesRepProductCategoryRevenuesByPeriodByProductCategory.TryGetValue(productCategory, out salesRepProductCategoryRevenuesByPeriod))
            {
                salesRepProductCategoryRevenue = CreateSalesRepProductCategoryRevenue(session, salesInvoiceItem, productCategory);

                salesRepProductCategoryRevenuesByPeriod = new Dictionary<DateTime, SalesRepProductCategoryRevenue> { { date, salesRepProductCategoryRevenue } };

                salesRepProductCategoryRevenuesByPeriodByProductCategory[productCategory] = salesRepProductCategoryRevenuesByPeriod;
            }

            if (!salesRepProductCategoryRevenuesByPeriod.TryGetValue(date, out salesRepProductCategoryRevenue))
            {
                salesRepProductCategoryRevenue = CreateSalesRepProductCategoryRevenue(session, salesInvoiceItem, productCategory);
                salesRepProductCategoryRevenuesByPeriod.Add(date, salesRepProductCategoryRevenue);
            }

            revenues.Add(salesRepProductCategoryRevenue.Id);
            salesRepProductCategoryRevenue.Revenue += salesInvoiceItem.TotalExVat;

            foreach (ProductCategory parent in productCategory.Parents)
            {
                CreateProductCategoryRevenue(session, salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep, date, revenues, salesInvoiceItem, parent);                            
            }
        }

        private static SalesRepProductCategoryRevenue CreateSalesRepProductCategoryRevenue(ISession session, SalesInvoiceItem item, ProductCategory productCategory)
        {
            return new SalesRepProductCategoryRevenueBuilder(session)
                        .WithSalesRep(item.SalesRep)
                        .WithProductCategory(productCategory)
                        .WithYear(item.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Year)
                        .WithMonth(item.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Month)
                        .WithCurrency(InternalOrganisation.Instance(session).PreferredCurrency)
                        .Build();
        }
    }
}

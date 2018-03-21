// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepPartyProductCategoryRevenues.cs" company="Allors bvba">
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

    public partial class SalesRepPartyProductCategoryRevenues
    {
        public static SalesRepPartyProductCategoryRevenue AppsFindOrCreateAsDependable(ISession session, SalesInvoiceItem salesInvoiceItem)
        {
            var salesInvoice = salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem;

            var salesRepPartyProductCategoryRevenues = salesInvoiceItem.SalesRep.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, salesInvoice.BillToCustomer);
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Year, salesInvoice.InvoiceDate.Year);
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Month, salesInvoice.InvoiceDate.Month);
            salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, salesInvoiceItem.Product.PrimaryProductCategory);
            var salesRepPartyProductCategoryRevenue = salesRepPartyProductCategoryRevenues.First
                                                      ?? new SalesRepPartyProductCategoryRevenueBuilder(session)
                                                                .WithParty(salesInvoice.BillToCustomer)
                                                                .WithSalesRep(salesInvoiceItem.SalesRep)
                                                                .WithProductCategory(salesInvoiceItem.Product.PrimaryProductCategory)
                                                                .WithYear(salesInvoice.InvoiceDate.Year)
                                                                .WithMonth(salesInvoice.InvoiceDate.Month)
                                                                .WithCurrency(salesInvoice.Currency)
                                                                .Build();

            SalesRepProductCategoryRevenues.AppsFindOrCreateAsDependable(session, salesRepPartyProductCategoryRevenue);
            SalesRepPartyRevenues.AppsFindOrCreateAsDependable(session, salesRepPartyProductCategoryRevenue);

            return salesRepPartyProductCategoryRevenue;
        }

        public static void AppsFindOrCreateAsDependable(ISession session, SalesRepPartyProductCategoryRevenue dependant)
        {
            foreach (ProductCategory parentCategory in dependant.ProductCategory.Parents)
            {
                var salesRepPartyProductCategoryRevenues = dependant.SalesRep.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
                salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, dependant.Party);
                salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Year, dependant.Year);
                salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Month, dependant.Month);
                salesRepPartyProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, parentCategory);
                var salesRepPartyProductCategoryRevenue = salesRepPartyProductCategoryRevenues.First
                                                          ?? new SalesRepPartyProductCategoryRevenueBuilder(session)
                                                                    .WithParty(dependant.Party)
                                                                    .WithSalesRep(dependant.SalesRep)
                                                                    .WithProductCategory(parentCategory)
                                                                    .WithYear(dependant.Year)
                                                                    .WithMonth(dependant.Month)
                                                                    .WithCurrency(dependant.Currency)
                                                                    .Build();

                SalesRepProductCategoryRevenues.AppsFindOrCreateAsDependable(session, salesRepPartyProductCategoryRevenue);

                AppsFindOrCreateAsDependable(session, salesRepPartyProductCategoryRevenue);
            }
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByPartyBySalesRep = new Dictionary<Party, Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>>>>();

            var salesRepPartyProductCategoryRevenues = session.Extent<SalesRepPartyProductCategoryRevenue>();

            foreach (SalesRepPartyProductCategoryRevenue salesRepPartyProductCategoryRevenue in salesRepPartyProductCategoryRevenues)
            {
                salesRepPartyProductCategoryRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(salesRepPartyProductCategoryRevenue.Year, salesRepPartyProductCategoryRevenue.Month, 01);

                Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>>> salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty;
                if (!salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByPartyBySalesRep.TryGetValue(salesRepPartyProductCategoryRevenue.SalesRep, out salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty))
                {
                    salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty = new Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>>>();
                    salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByPartyBySalesRep[salesRepPartyProductCategoryRevenue.SalesRep] = salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty;
                }

                Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>> salesRepPartyProductCategoryRevenuesByPeriodByProductCategory;
                if (!salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty.TryGetValue(salesRepPartyProductCategoryRevenue.Party, out salesRepPartyProductCategoryRevenuesByPeriodByProductCategory))
                {
                    salesRepPartyProductCategoryRevenuesByPeriodByProductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>>();
                    salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty[salesRepPartyProductCategoryRevenue.Party] = salesRepPartyProductCategoryRevenuesByPeriodByProductCategory;
                }

                Dictionary<DateTime, SalesRepPartyProductCategoryRevenue> salesRepPartyProductCategoryRevenuesByPeriod;
                if (!salesRepPartyProductCategoryRevenuesByPeriodByProductCategory.TryGetValue(salesRepPartyProductCategoryRevenue.ProductCategory, out salesRepPartyProductCategoryRevenuesByPeriod))
                {
                    salesRepPartyProductCategoryRevenuesByPeriod = new Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>();
                    salesRepPartyProductCategoryRevenuesByPeriodByProductCategory[salesRepPartyProductCategoryRevenue.ProductCategory] = salesRepPartyProductCategoryRevenuesByPeriod;
                }

                SalesRepPartyProductCategoryRevenue revenue;
                if (!salesRepPartyProductCategoryRevenuesByPeriod.TryGetValue(date, out revenue))
                {
                    salesRepPartyProductCategoryRevenuesByPeriod.Add(date, salesRepPartyProductCategoryRevenue);
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
                            CreateProductCategoryRevenue(session, salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByPartyBySalesRep, productCategory, date, salesInvoiceItem, revenues);
                        }
                    }
                }
            }

            foreach (SalesRepPartyProductCategoryRevenue salesRepPartyProductCategoryRevenue in salesRepPartyProductCategoryRevenues)
            {
                if (!revenues.Contains(salesRepPartyProductCategoryRevenue.Id))
                {
                    salesRepPartyProductCategoryRevenue.Delete();
                }
            }
        }

        private static void CreateProductCategoryRevenue(
            ISession session,
            Dictionary<Party, Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>>>> salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByPartyBySalesRep,
            ProductCategory productCategory,
            DateTime date,
            SalesInvoiceItem salesInvoiceItem,
            HashSet<long> revenues)
        {
            SalesRepPartyProductCategoryRevenue salesRepPartyProductCategoryRevenue;
            Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>>> salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty;
            if (!salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByPartyBySalesRep.TryGetValue(salesInvoiceItem.SalesRep, out salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty))
            {
                salesRepPartyProductCategoryRevenue = CreateSalesRepPartyProductCategoryRevenue(session, salesInvoiceItem, productCategory);

                salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty = new Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>>>
                        {
                            {
                                salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer,
                                new Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>>
                                    {
                                        {
                                            productCategory,
                                            new Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>
                                                {
                                                    { date, salesRepPartyProductCategoryRevenue }
                                                }
                                            }
                                    }
                                }
                        };

                salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByPartyBySalesRep[salesInvoiceItem.SalesRep] = salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty;
            }

            Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>> salesRepPartyProductCategoryRevenuesByPeriodByProductCategory;
            if (!salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty.TryGetValue(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer, out salesRepPartyProductCategoryRevenuesByPeriodByProductCategory))
            {
                salesRepPartyProductCategoryRevenue = CreateSalesRepPartyProductCategoryRevenue(session, salesInvoiceItem, productCategory);

                salesRepPartyProductCategoryRevenuesByPeriodByProductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>>
                        {
                            {
                                productCategory,
                                new Dictionary<DateTime, SalesRepPartyProductCategoryRevenue>
                                    {
                                        { date, salesRepPartyProductCategoryRevenue }
                                    }
                                }
                        };

                salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByParty[salesInvoiceItem.SalesRep] = salesRepPartyProductCategoryRevenuesByPeriodByProductCategory;
            }

            Dictionary<DateTime, SalesRepPartyProductCategoryRevenue> salesRepPartyProductCategoryRevenuesByPeriod;
            if (!salesRepPartyProductCategoryRevenuesByPeriodByProductCategory.TryGetValue(productCategory, out salesRepPartyProductCategoryRevenuesByPeriod))
            {
                salesRepPartyProductCategoryRevenue = CreateSalesRepPartyProductCategoryRevenue(session, salesInvoiceItem, productCategory);

                salesRepPartyProductCategoryRevenuesByPeriod = new Dictionary<DateTime, SalesRepPartyProductCategoryRevenue> { { date, salesRepPartyProductCategoryRevenue } };

                salesRepPartyProductCategoryRevenuesByPeriodByProductCategory[productCategory] = salesRepPartyProductCategoryRevenuesByPeriod;
            }

            if (!salesRepPartyProductCategoryRevenuesByPeriod.TryGetValue(date, out salesRepPartyProductCategoryRevenue))
            {
                salesRepPartyProductCategoryRevenue = CreateSalesRepPartyProductCategoryRevenue(session, salesInvoiceItem, productCategory);
                salesRepPartyProductCategoryRevenuesByPeriod.Add(date, salesRepPartyProductCategoryRevenue);
            }

            revenues.Add(salesRepPartyProductCategoryRevenue.Id);
            salesRepPartyProductCategoryRevenue.Revenue += salesInvoiceItem.TotalExVat;

            foreach (ProductCategory parent in productCategory.Parents)
            {
                CreateProductCategoryRevenue(session, salesRepPartyProductCategoryRevenuesByPeriodByProductCategoryByPartyBySalesRep, parent, date, salesInvoiceItem, revenues);
            }
        }

        private static SalesRepPartyProductCategoryRevenue CreateSalesRepPartyProductCategoryRevenue(ISession session, SalesInvoiceItem item, ProductCategory productCategory)
        {
            return new SalesRepPartyProductCategoryRevenueBuilder(session)
                        .WithParty(item.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer)
                        .WithSalesRep(item.SalesRep)
                        .WithProductCategory(productCategory)
                        .WithYear(item.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Year)
                        .WithMonth(item.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Month)
                        .WithCurrency(item.SalesInvoiceWhereSalesInvoiceItem.Currency)
                        .Build();
        }
    }
}

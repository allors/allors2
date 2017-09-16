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
            salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.InternalOrganisation, dependant.InternalOrganisation);
            salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.ProductCategory, dependant.ProductCategory);
            salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.Year, dependant.Year);
            salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.Month, dependant.Month);
            var salesRepProductCategoryRevenue = salesRepProductCategoryRevenues.First
                                                 ?? new SalesRepProductCategoryRevenueBuilder(session)
                                                                .WithInternalOrganisation(dependant.InternalOrganisation)
                                                                .WithProductCategory(dependant.ProductCategory)
                                                                .WithSalesRep(dependant.SalesRep)
                                                                .WithYear(dependant.Year)
                                                                .WithMonth(dependant.Month)
                                                                .WithCurrency(dependant.Currency)
                                                                .WithRevenue(0M)
                                                                .Build();
            return salesRepProductCategoryRevenue;
        }

        public static void AppsFindOrCreateAsDependable(ISession session, SalesRepProductCategoryRevenue dependant)
        {
            foreach (ProductCategory parentCategory in dependant.ProductCategory.Parents)
            {
                var salesRepProductCategoryRevenues = parentCategory.SalesRepProductCategoryRevenuesWhereProductCategory;
                salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.InternalOrganisation, dependant.InternalOrganisation);
                salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.SalesRep, dependant.SalesRep);
                salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.Year, dependant.Year);
                salesRepProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.Month, dependant.Month);
                var salesRepProductCategoryRevenue = salesRepProductCategoryRevenues.First
                                                        ?? new SalesRepProductCategoryRevenueBuilder(session)
                                                                .WithInternalOrganisation(dependant.InternalOrganisation)
                                                                .WithSalesRep(dependant.SalesRep)
                                                                .WithProductCategory(parentCategory)
                                                                .WithYear(dependant.Year)
                                                                .WithMonth(dependant.Month)
                                                                .WithCurrency(dependant.Currency)
                                                                .WithRevenue(0M)
                                                                .Build();

                AppsFindOrCreateAsDependable(session, salesRepProductCategoryRevenue);
            }
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRepByInternalOrganisation =
                new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>>>();

            var salesRepProductCategoryRevenues = session.Extent<SalesRepProductCategoryRevenue>();

            foreach (SalesRepProductCategoryRevenue salesRepProductCategoryRevenue in salesRepProductCategoryRevenues)
            {
                salesRepProductCategoryRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(salesRepProductCategoryRevenue.Year, salesRepProductCategoryRevenue.Month, 01);

                Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>> salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep;
                if (!salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRepByInternalOrganisation.TryGetValue(salesRepProductCategoryRevenue.InternalOrganisation, out salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep))
                {
                    salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep = new Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>>();
                    salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRepByInternalOrganisation[salesRepProductCategoryRevenue.InternalOrganisation] = salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep;
                }

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
                            CreateProductCategoryRevenue(session, salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRepByInternalOrganisation, date, revenues, salesInvoiceItem, productCategory);
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
            Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>>> salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRepByInternalOrganisation,
            DateTime date,
            HashSet<long> revenues,
            SalesInvoiceItem salesInvoiceItem,
            ProductCategory productCategory)
        {
            SalesRepProductCategoryRevenue salesRepProductCategoryRevenue;

            Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>> salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep;
            if (!salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRepByInternalOrganisation.TryGetValue(salesInvoiceItem.ISalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation, out salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep))
            {
                salesRepProductCategoryRevenue = CreateSalesRepProductCategoryRevenue(session, salesInvoiceItem, productCategory);

                salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep = new Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>>
                        {
                            {
                                salesInvoiceItem.SalesRep,
                                new Dictionary<ProductCategory, Dictionary<DateTime, SalesRepProductCategoryRevenue>>
                                    {
                                        {
                                            productCategory,
                                            new Dictionary<DateTime, SalesRepProductCategoryRevenue>
                                                {
                                                    { date, salesRepProductCategoryRevenue } 
                                                }
                                            }
                                    }
                                }
                        };

                salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRepByInternalOrganisation[salesInvoiceItem.ISalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation] = salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRep;
            }

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
                CreateProductCategoryRevenue(session, salesRepProductCategoryRevenuesByPeriodByProductCategoryBySalesRepByInternalOrganisation, date, revenues, salesInvoiceItem, parent);                            
            }
        }

        private static SalesRepProductCategoryRevenue CreateSalesRepProductCategoryRevenue(ISession session, SalesInvoiceItem item, ProductCategory productCategory)
        {
            return new SalesRepProductCategoryRevenueBuilder(session)
                        .WithInternalOrganisation(item.ISalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation)
                        .WithSalesRep(item.SalesRep)
                        .WithProductCategory(productCategory)
                        .WithYear(item.ISalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Year)
                        .WithMonth(item.ISalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Month)
                        .WithCurrency(item.ISalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithRevenue(0M)
                        .Build();
        }
    }
}

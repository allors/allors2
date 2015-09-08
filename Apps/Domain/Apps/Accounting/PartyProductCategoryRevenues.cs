// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyProductCategoryRevenues.cs" company="Allors bvba">
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


    public partial class PartyProductCategoryRevenues
    {
        public static void AppsOnDeriveRevenues(ISession session)
        {
            var partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation =
                new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>>>();

            var partyProductCategoryRevenues = session.Extent<PartyProductCategoryRevenue>();

            foreach (PartyProductCategoryRevenue partyProductCategoryRevenue in partyProductCategoryRevenues)
            {
                partyProductCategoryRevenue.Revenue = 0;
                partyProductCategoryRevenue.Quantity = 0;
                var date = DateTimeFactory.CreateDate(partyProductCategoryRevenue.Year, partyProductCategoryRevenue.Month, 01);

                Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>> partyProductCategoryRevenuesByPeriodByProductCategoryByParty;
                if (!partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation.TryGetValue(partyProductCategoryRevenue.InternalOrganisation, out partyProductCategoryRevenuesByPeriodByProductCategoryByParty))
                {
                    partyProductCategoryRevenuesByPeriodByProductCategoryByParty = new Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>>();
                    partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation[partyProductCategoryRevenue.InternalOrganisation] = partyProductCategoryRevenuesByPeriodByProductCategoryByParty;
                }

                Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>> partyProductCategoryRevenuesByPeriodByproductCategory;
                if (!partyProductCategoryRevenuesByPeriodByProductCategoryByParty.TryGetValue(partyProductCategoryRevenue.Party, out partyProductCategoryRevenuesByPeriodByproductCategory))
                {
                    partyProductCategoryRevenuesByPeriodByproductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>();
                    partyProductCategoryRevenuesByPeriodByProductCategoryByParty[partyProductCategoryRevenue.Party] = partyProductCategoryRevenuesByPeriodByproductCategory;
                }

                Dictionary<DateTime, PartyProductCategoryRevenue> partyProductCategoryRevenuesByPeriod;
                if (!partyProductCategoryRevenuesByPeriodByproductCategory.TryGetValue(partyProductCategoryRevenue.ProductCategory, out partyProductCategoryRevenuesByPeriod))
                {
                    partyProductCategoryRevenuesByPeriod = new Dictionary<DateTime, PartyProductCategoryRevenue>();
                    partyProductCategoryRevenuesByPeriodByproductCategory[partyProductCategoryRevenue.ProductCategory] = partyProductCategoryRevenuesByPeriod;
                }

                PartyProductCategoryRevenue revenue;
                if (!partyProductCategoryRevenuesByPeriod.TryGetValue(date, out revenue))
                {
                    partyProductCategoryRevenuesByPeriod.Add(date, partyProductCategoryRevenue);
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
                            CreateProductCategoryRevenue(session, partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation, date, revenues, salesInvoiceItem, productCategory);
                        }
                    }
                }
            }

            foreach (PartyProductCategoryRevenue partyProductCategoryRevenue in partyProductCategoryRevenues)
            {
                if (!revenues.Contains(partyProductCategoryRevenue.Id))
                {
                    partyProductCategoryRevenue.Delete();
                }
            }
        }

        public static void AppsFindOrCreateAsDependable(ISession session, PartyProductRevenue dependant)
        {
            foreach (ProductCategory productCategory in dependant.Product.ProductCategories)
            {
                var partyProductCategoryRevenues = dependant.Party.PartyProductCategoryRevenuesWhereParty;
                partyProductCategoryRevenues.Filter.AddEquals(Meta.InternalOrganisation, dependant.InternalOrganisation);
                partyProductCategoryRevenues.Filter.AddEquals(Meta.Year, dependant.Year);
                partyProductCategoryRevenues.Filter.AddEquals(Meta.Month, dependant.Month);
                partyProductCategoryRevenues.Filter.AddEquals(Meta.ProductCategory, productCategory);
                var partyProductCategoryRevenue = partyProductCategoryRevenues.First
                                                  ?? new PartyProductCategoryRevenueBuilder(session)
                                                            .WithInternalOrganisation(dependant.InternalOrganisation)
                                                            .WithParty(dependant.Party)
                                                            .WithProductCategory(productCategory)
                                                            .WithYear(dependant.Year)
                                                            .WithMonth(dependant.Month)
                                                            .WithCurrency(dependant.Currency)
                                                            .WithRevenue(0M)
                                                            .Build();

                ProductCategoryRevenues.AppsFindOrCreateAsDependable(session, partyProductCategoryRevenue);
                PartyPackageRevenues.AppsFindOrCreateAsDependable(session, partyProductCategoryRevenue);
            }
        }

        public static void AppsFindOrCreateAsDependable(ISession session, PartyProductCategoryRevenue dependant)
        {
            foreach (ProductCategory parentCategory in dependant.ProductCategory.Parents)
            {
                var partyProductCategoryRevenues = dependant.Party.PartyProductCategoryRevenuesWhereParty;
                partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.InternalOrganisation, dependant.InternalOrganisation);
                partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.Year, dependant.Year);
                partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.Month, dependant.Month);
                partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.ProductCategory, parentCategory);
                var partyProductCategoryRevenue = partyProductCategoryRevenues.First
                                                  ?? new PartyProductCategoryRevenueBuilder(session)
                                                            .WithInternalOrganisation(dependant.InternalOrganisation)
                                                            .WithParty(dependant.Party)
                                                            .WithProductCategory(parentCategory)
                                                            .WithYear(dependant.Year)
                                                            .WithMonth(dependant.Month)
                                                            .WithCurrency(dependant.Currency)
                                                            .WithRevenue(0M)
                                                            .Build();

                ProductCategoryRevenues.AppsFindOrCreateAsDependable(session, partyProductCategoryRevenue);

                AppsFindOrCreateAsDependable(session, partyProductCategoryRevenue);
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
            Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>>> partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation,
            DateTime date,
            HashSet<ObjectId> revenues,
            SalesInvoiceItem salesInvoiceItem,
            ProductCategory productCategory)
        {
            PartyProductCategoryRevenue partyProductCategoryRevenue;

            Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>> partyProductCategoryRevenuesByPeriodByProductCategoryByParty;
            if (!partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation.TryGetValue(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation, out partyProductCategoryRevenuesByPeriodByProductCategoryByParty))
            {
                partyProductCategoryRevenue = CreatePartyProductCategoryRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, productCategory);

                partyProductCategoryRevenuesByPeriodByProductCategoryByParty = new Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>>
                        {
                            {
                                salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer,
                                new Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>
                                    {
                                        {
                                            productCategory,
                                            new Dictionary<DateTime, PartyProductCategoryRevenue> { { date, partyProductCategoryRevenue } }
                                        }
                                    }
                                }
                        };

                partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation[salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation] = partyProductCategoryRevenuesByPeriodByProductCategoryByParty;
            }

            Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>> partyProductCategoryRevenuesByPeriodByProductCategory;
            if (!partyProductCategoryRevenuesByPeriodByProductCategoryByParty.TryGetValue(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer, out partyProductCategoryRevenuesByPeriodByProductCategory))
            {
                partyProductCategoryRevenue = CreatePartyProductCategoryRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, productCategory);

                partyProductCategoryRevenuesByPeriodByProductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>
                        {
                            {
                                productCategory,
                                new Dictionary<DateTime, PartyProductCategoryRevenue> { { date, partyProductCategoryRevenue } }
                            }
                        };

                partyProductCategoryRevenuesByPeriodByProductCategoryByParty[salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer] = partyProductCategoryRevenuesByPeriodByProductCategory;
            }

            Dictionary<DateTime, PartyProductCategoryRevenue> partyProductCategoryRevenuesByPeriod;
            if (!partyProductCategoryRevenuesByPeriodByProductCategory.TryGetValue(productCategory, out partyProductCategoryRevenuesByPeriod))
            {
                partyProductCategoryRevenue = CreatePartyProductCategoryRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, productCategory);

                partyProductCategoryRevenuesByPeriod = new Dictionary<DateTime, PartyProductCategoryRevenue> { { date, partyProductCategoryRevenue } };

                partyProductCategoryRevenuesByPeriodByProductCategory[productCategory] = partyProductCategoryRevenuesByPeriod;
            }

            if (!partyProductCategoryRevenuesByPeriod.TryGetValue(date, out partyProductCategoryRevenue))
            {
                partyProductCategoryRevenue = CreatePartyProductCategoryRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, productCategory);
                partyProductCategoryRevenuesByPeriod.Add(date, partyProductCategoryRevenue);
            }

            revenues.Add(partyProductCategoryRevenue.Id);
            partyProductCategoryRevenue.Revenue += salesInvoiceItem.TotalExVat;
            partyProductCategoryRevenue.Quantity += salesInvoiceItem.Quantity;

            foreach (ProductCategory parent in productCategory.Parents)
            {
                CreateProductCategoryRevenue(session, partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation, date, revenues, salesInvoiceItem, parent);
            }
        }

        private static PartyProductCategoryRevenue CreatePartyProductCategoryRevenue(ISession session, SalesInvoice invoice, ProductCategory category)
        {
            return new PartyProductCategoryRevenueBuilder(session)
                        .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                        .WithParty(invoice.BillToCustomer)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithProductCategory(category)
                        .WithRevenue(0M)
                        .WithQuantity(0M)
                        .Build();
        }
    }
}

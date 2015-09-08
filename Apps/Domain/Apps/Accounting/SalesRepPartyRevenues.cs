// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepPartyRevenues.cs" company="Allors bvba">
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


    public partial class SalesRepPartyRevenues
    {
        public static SalesRepPartyRevenue AppsFindOrCreateAsDependable(ISession session, SalesRepPartyProductCategoryRevenue dependant)
        {
            var salesRepPartyRevenues = dependant.SalesRep.SalesRepPartyRevenuesWhereSalesRep;
            salesRepPartyRevenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.InternalOrganisation, dependant.InternalOrganisation);
            salesRepPartyRevenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Party, dependant.Party);
            salesRepPartyRevenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Year, dependant.Year);
            salesRepPartyRevenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Month, dependant.Month);
            var salesRepPartyRevenue = salesRepPartyRevenues.First ?? new SalesRepPartyRevenueBuilder(session)
                                                                            .WithInternalOrganisation(dependant.InternalOrganisation)
                                                                            .WithParty(dependant.Party)
                                                                            .WithSalesRep(dependant.SalesRep)
                                                                            .WithYear(dependant.Year)
                                                                            .WithMonth(dependant.Month)
                                                                            .WithCurrency(dependant.Currency)
                                                                            .WithRevenue(0M)
                                                                            .Build();

            SalesRepRevenues.AppsFindOrCreateAsDependable(session, salesRepPartyRevenue);

            return salesRepPartyRevenue;
        }

        public static SalesRepPartyRevenue AppsFindOrCreateAsDependable(ISession session, SalesInvoiceItem salesInvoiceItem)
        {
            var salesRepPartyRevenues = salesInvoiceItem.SalesRep.SalesRepPartyRevenuesWhereSalesRep;
            salesRepPartyRevenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.InternalOrganisation, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation);
            salesRepPartyRevenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Party, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer);
            salesRepPartyRevenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Year, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Year);
            salesRepPartyRevenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Month, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Month);
            var salesRepPartyRevenue = salesRepPartyRevenues.First ?? new SalesRepPartyRevenueBuilder(session)
                                                                            .WithInternalOrganisation(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation)
                                                                            .WithParty(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer)
                                                                            .WithSalesRep(salesInvoiceItem.SalesRep)
                                                                            .WithYear(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Year)
                                                                            .WithMonth(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Month)
                                                                            .WithCurrency(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation.PreferredCurrency)
                                                                            .WithRevenue(0M)
                                                                            .Build();

            SalesRepRevenues.AppsFindOrCreateAsDependable(session, salesRepPartyRevenue);

            return salesRepPartyRevenue;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var salesRepPartyRevenuesByPeriodByPartyBySalesRepByInternalOrganisation =
                new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>>>>();

            var salesRepPartyRevenues = session.Extent<SalesRepPartyRevenue>();

            foreach (SalesRepPartyRevenue salesRepPartyRevenue in salesRepPartyRevenues)
            {
                salesRepPartyRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(salesRepPartyRevenue.Year, salesRepPartyRevenue.Month, 01);

                Dictionary<Party, Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>>> salesRepPartyRevenuesByPeriodByPartyBySalesRep;
                if (!salesRepPartyRevenuesByPeriodByPartyBySalesRepByInternalOrganisation.TryGetValue(salesRepPartyRevenue.InternalOrganisation, out salesRepPartyRevenuesByPeriodByPartyBySalesRep))
                {
                    salesRepPartyRevenuesByPeriodByPartyBySalesRep = new Dictionary<Party, Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>>>();
                    salesRepPartyRevenuesByPeriodByPartyBySalesRepByInternalOrganisation[salesRepPartyRevenue.InternalOrganisation] = salesRepPartyRevenuesByPeriodByPartyBySalesRep;
                }

                Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>> salesRepPartyRevenuesByPeriodByParty;
                if (!salesRepPartyRevenuesByPeriodByPartyBySalesRep.TryGetValue(salesRepPartyRevenue.Party, out salesRepPartyRevenuesByPeriodByParty))
                {
                    salesRepPartyRevenuesByPeriodByParty = new Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>>();
                    salesRepPartyRevenuesByPeriodByPartyBySalesRep[salesRepPartyRevenue.Party] = salesRepPartyRevenuesByPeriodByParty;
                }

                Dictionary<DateTime, SalesRepPartyRevenue> salesRepPartyRevenuesByPeriod;
                if (!salesRepPartyRevenuesByPeriodByParty.TryGetValue(salesRepPartyRevenue.Party, out salesRepPartyRevenuesByPeriod))
                {
                    salesRepPartyRevenuesByPeriod = new Dictionary<DateTime, SalesRepPartyRevenue>();
                    salesRepPartyRevenuesByPeriodByParty[salesRepPartyRevenue.Party] = salesRepPartyRevenuesByPeriod;
                }

                SalesRepPartyRevenue revenue;
                if (!salesRepPartyRevenuesByPeriod.TryGetValue(date, out revenue))
                {
                    salesRepPartyRevenuesByPeriod.Add(date, salesRepPartyRevenue);
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
                    if (salesInvoiceItem.ExistSalesRep && salesInvoiceItem.ExistProduct && salesInvoiceItem.Product.ExistPrimaryProductCategory)
                    {
                        SalesRepPartyRevenue salesRepPartyRevenue;

                        Dictionary<Party, Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>>> salesRepPartyRevenuesByPeriodByPartyBySalesRep;
                        if (!salesRepPartyRevenuesByPeriodByPartyBySalesRepByInternalOrganisation.TryGetValue(salesInvoice.BilledFromInternalOrganisation, out salesRepPartyRevenuesByPeriodByPartyBySalesRep))
                        {
                            salesRepPartyRevenue = CreateSalesRepPartyRevenue(session, salesInvoiceItem);

                            salesRepPartyRevenuesByPeriodByPartyBySalesRep = new Dictionary<Party, Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>>>
                                {
                                    {
                                        salesInvoiceItem.SalesRep,
                                        new Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>>
                                        {
                                            { 
                                                salesInvoice.BillToCustomer, new Dictionary<DateTime, SalesRepPartyRevenue>
                                                  {
                                                      { date, salesRepPartyRevenue }
                                                  }
                                            }
                                        }
                                    }
                                };

                            salesRepPartyRevenuesByPeriodByPartyBySalesRepByInternalOrganisation[salesInvoice.BilledFromInternalOrganisation] = salesRepPartyRevenuesByPeriodByPartyBySalesRep;
                        }

                        Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>> salesRepPartyRevenuesByPeriodByParty;
                        if (!salesRepPartyRevenuesByPeriodByPartyBySalesRep.TryGetValue(salesInvoiceItem.SalesRep, out salesRepPartyRevenuesByPeriodByParty))
                        {
                            salesRepPartyRevenue = CreateSalesRepPartyRevenue(session, salesInvoiceItem);

                            salesRepPartyRevenuesByPeriodByParty = new Dictionary<Party, Dictionary<DateTime, SalesRepPartyRevenue>>
                                {
                                    { salesInvoice.BillToCustomer, new Dictionary<DateTime, SalesRepPartyRevenue> { { date, salesRepPartyRevenue } } }
                                };

                            salesRepPartyRevenuesByPeriodByPartyBySalesRep[salesInvoiceItem.SalesRep] = salesRepPartyRevenuesByPeriodByParty;
                        }

                        Dictionary<DateTime, SalesRepPartyRevenue> salesRepPartyRevenuesByPeriod;
                        if (!salesRepPartyRevenuesByPeriodByParty.TryGetValue(salesInvoice.BillToCustomer, out salesRepPartyRevenuesByPeriod))
                        {
                            salesRepPartyRevenue = CreateSalesRepPartyRevenue(session, salesInvoiceItem);

                            salesRepPartyRevenuesByPeriod = new Dictionary<DateTime, SalesRepPartyRevenue>
                                {
                                    { date, salesRepPartyRevenue } 
                                };

                            salesRepPartyRevenuesByPeriodByParty[salesInvoice.BillToCustomer] = salesRepPartyRevenuesByPeriod;
                        }

                        if (!salesRepPartyRevenuesByPeriod.TryGetValue(date, out salesRepPartyRevenue))
                        {
                            salesRepPartyRevenue = CreateSalesRepPartyRevenue(session, salesInvoiceItem);
                            salesRepPartyRevenuesByPeriod.Add(date, salesRepPartyRevenue);
                        }

                        revenues.Add(salesRepPartyRevenue.Id);
                        salesRepPartyRevenue.Revenue += salesInvoiceItem.TotalExVat;
                    }
                }
            }

            foreach (SalesRepPartyRevenue salesRepPartyRevenue in salesRepPartyRevenues)
            {
                if (!revenues.Contains(salesRepPartyRevenue.Id))
                {
                    salesRepPartyRevenue.Delete();
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static SalesRepPartyRevenue CreateSalesRepPartyRevenue(ISession session, SalesInvoiceItem item)
        {
            return new SalesRepPartyRevenueBuilder(session)
                        .WithInternalOrganisation(item.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation)
                        .WithSalesRep(item.SalesRep)
                        .WithParty(item.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer)
                        .WithYear(item.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Year)
                        .WithMonth(item.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Month)
                        .WithCurrency(item.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithRevenue(0M)
                        .Build();
        }
    }
}

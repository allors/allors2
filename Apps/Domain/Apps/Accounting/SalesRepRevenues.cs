// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepRevenues.cs" company="Allors bvba">
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


    public partial class SalesRepRevenues
    {
        public static SalesRepRevenue AppsFindOrCreateAsDependable(ISession session, SalesRepPartyRevenue dependant)
        {
            var salesRepRevenues = dependant.SalesRep.SalesRepRevenuesWhereSalesRep;
            salesRepRevenues.Filter.AddEquals(SalesRepRevenues.Meta.InternalOrganisation, dependant.InternalOrganisation);
            salesRepRevenues.Filter.AddEquals(SalesRepRevenues.Meta.Year, dependant.Year);
            salesRepRevenues.Filter.AddEquals(SalesRepRevenues.Meta.Month, dependant.Month);
            var salesRepRevenue = salesRepRevenues.First ?? new SalesRepRevenueBuilder(session)
                                                                .WithInternalOrganisation(dependant.InternalOrganisation)
                                                                .WithSalesRep(dependant.SalesRep)
                                                                .WithYear(dependant.Year)
                                                                .WithMonth(dependant.Month)
                                                                .WithCurrency(dependant.Currency)
                                                                .WithRevenue(0M)
                                                                .Build();

            SalesRepCommissions.AppsFindOrCreateAsDependable(session, salesRepRevenue);

            return salesRepRevenue;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var salesRepRevenuesByPeriodBySalesRepByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<DateTime, SalesRepRevenue>>>();

            var salesRepRevenues = session.Extent<SalesRepRevenue>();

            foreach (SalesRepRevenue salesRepRevenue in salesRepRevenues)
            {
                salesRepRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(salesRepRevenue.Year, salesRepRevenue.Month, 01);

                Dictionary<Party, Dictionary<DateTime, SalesRepRevenue>> salesRepRevenuesByPeriodBySalesRep;
                if (!salesRepRevenuesByPeriodBySalesRepByInternalOrganisation.TryGetValue(salesRepRevenue.InternalOrganisation, out salesRepRevenuesByPeriodBySalesRep))
                {
                    salesRepRevenuesByPeriodBySalesRep = new Dictionary<Party, Dictionary<DateTime, SalesRepRevenue>>();
                    salesRepRevenuesByPeriodBySalesRepByInternalOrganisation[salesRepRevenue.InternalOrganisation] = salesRepRevenuesByPeriodBySalesRep;
                }

                Dictionary<DateTime, SalesRepRevenue> salesRepRevenuesByPeriod;
                if (!salesRepRevenuesByPeriodBySalesRep.TryGetValue(salesRepRevenue.SalesRep, out salesRepRevenuesByPeriod))
                {
                    salesRepRevenuesByPeriod = new Dictionary<DateTime, SalesRepRevenue>();
                    salesRepRevenuesByPeriodBySalesRep[salesRepRevenue.SalesRep] = salesRepRevenuesByPeriod;
                }

                SalesRepRevenue revenue;
                if (!salesRepRevenuesByPeriod.TryGetValue(date, out revenue))
                {
                    salesRepRevenuesByPeriod.Add(date, salesRepRevenue);
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
                    if (salesInvoiceItem.ExistSalesRep)
                    {
                        SalesRepRevenue salesRepRevenue;

                        Dictionary<Party, Dictionary<DateTime, SalesRepRevenue>> salesRepRevenuesByPeriodBySalesRep;
                        if (!salesRepRevenuesByPeriodBySalesRepByInternalOrganisation.TryGetValue(salesInvoice.BilledFromInternalOrganisation, out salesRepRevenuesByPeriodBySalesRep))
                        {
                            salesRepRevenue = CreateSalesRepRevenue(session, salesInvoiceItem);

                            salesRepRevenuesByPeriodBySalesRep = new Dictionary<Party, Dictionary<DateTime, SalesRepRevenue>>
                            {
                                { salesInvoiceItem.SalesRep, new Dictionary<DateTime, SalesRepRevenue> { { date, salesRepRevenue } } }
                            };

                            salesRepRevenuesByPeriodBySalesRepByInternalOrganisation[salesInvoice.BilledFromInternalOrganisation] = salesRepRevenuesByPeriodBySalesRep;
                        }

                        Dictionary<DateTime, SalesRepRevenue> salesRepRevenuesByPeriod;
                        if (!salesRepRevenuesByPeriodBySalesRep.TryGetValue(salesInvoiceItem.SalesRep, out salesRepRevenuesByPeriod))
                        {
                            salesRepRevenue = CreateSalesRepRevenue(session, salesInvoiceItem);

                            salesRepRevenuesByPeriod = new Dictionary<DateTime, SalesRepRevenue>
                            {
                                { date, salesRepRevenue } 
                            };

                            salesRepRevenuesByPeriodBySalesRep[salesInvoiceItem.SalesRep] = salesRepRevenuesByPeriod;
                        }

                        if (!salesRepRevenuesByPeriod.TryGetValue(date, out salesRepRevenue))
                        {
                            salesRepRevenue = CreateSalesRepRevenue(session, salesInvoiceItem);
                            salesRepRevenuesByPeriod.Add(date, salesRepRevenue);
                        }

                        revenues.Add(salesRepRevenue.Id);
                        salesRepRevenue.Revenue += salesInvoiceItem.TotalExVat;
                    }
                }
            }

            foreach (SalesRepRevenue salesRepRevenue in salesRepRevenues)
            {
                if (!revenues.Contains(salesRepRevenue.Id))
                {
                    salesRepRevenue.Delete();
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static SalesRepRevenue CreateSalesRepRevenue(ISession session, SalesInvoiceItem item)
        {
            return new SalesRepRevenueBuilder(session)
                        .WithInternalOrganisation(item.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation)
                        .WithSalesRep(item.SalesRep)
                        .WithYear(item.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Year)
                        .WithMonth(item.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate.Month)
                        .WithCurrency(item.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithRevenue(0M)
                        .Build();
        }
    }
}

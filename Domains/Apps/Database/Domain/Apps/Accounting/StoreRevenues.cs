// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoreRevenues.cs" company="Allors bvba">
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
    
    public partial class StoreRevenues
    {
        public static StoreRevenue AppsFindOrCreateAsDependable(ISession session, SalesInvoice invoice)
        {
            var storeRevenues = invoice.Store.StoreRevenuesWhereStore;
            storeRevenues.Filter.AddEquals(M.StoreRevenue.Year, invoice.InvoiceDate.Year);
            storeRevenues.Filter.AddEquals(M.StoreRevenue.Month, invoice.InvoiceDate.Month);
            var storeRevenue = storeRevenues.First ?? new StoreRevenueBuilder(session)
                                                            .WithStore((Store)session.Instantiate(invoice.Store))
                                                            .WithYear(invoice.InvoiceDate.Year)
                                                            .WithMonth(invoice.InvoiceDate.Month)
                                                            .WithCurrency(InternalOrganisation.Instance(session).PreferredCurrency)
                                                            .Build();

            InternalOrganisationRevenues.AppsFindOrCreateAsDependable(session, storeRevenue);

            return storeRevenue;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var storeRevenues = session.Extent<StoreRevenue>();
            Dictionary<Store, Dictionary<DateTime, StoreRevenue>> storeRevenuesByPeriodByStore = new Dictionary<Store, Dictionary<DateTime, StoreRevenue>>();

            foreach (StoreRevenue storeRevenue in storeRevenues)
            {
                storeRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(storeRevenue.Year, storeRevenue.Month, 01);

                if (!storeRevenuesByPeriodByStore.TryGetValue(storeRevenue.Store, out var storeRevenuesByPeriod))
                {
                    storeRevenuesByPeriod = new Dictionary<DateTime, StoreRevenue>();
                    storeRevenuesByPeriodByStore[storeRevenue.Store] = storeRevenuesByPeriod;
                }

                if (!storeRevenuesByPeriod.ContainsKey(date))
                {
                    storeRevenuesByPeriod.Add(date, storeRevenue);
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
                    StoreRevenue storeRevenue;

                    if (!storeRevenuesByPeriodByStore.TryGetValue(salesInvoice.Store, out var storeRevenuesByPeriod))
                    {
                        storeRevenue = CreateStoreRevenue(session, salesInvoice);

                        storeRevenuesByPeriod = new Dictionary<DateTime, StoreRevenue>
                        {
                            { date, storeRevenue } 
                        };

                        storeRevenuesByPeriodByStore[salesInvoice.Store] = storeRevenuesByPeriod;
                    }

                    if (!storeRevenuesByPeriod.TryGetValue(date, out storeRevenue))
                    {
                        storeRevenue = CreateStoreRevenue(session, salesInvoice);
                        storeRevenuesByPeriod.Add(date, storeRevenue);
                    }

                    revenues.Add(storeRevenue.Id);
                    storeRevenue.Revenue += salesInvoiceItem.TotalExVat;
                }
            }

            foreach (StoreRevenue storeRevenue in storeRevenues)
            {
                if (!revenues.Contains(storeRevenue.Id))
                {
                    storeRevenue.Delete();
                }
            }
        }

        private static StoreRevenue CreateStoreRevenue(ISession session, SalesInvoice invoice)
        {
            return new StoreRevenueBuilder(session)
                        .WithStore(invoice.Store)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(InternalOrganisation.Instance(session).PreferredCurrency)
                        .Build();
        }
    }
}

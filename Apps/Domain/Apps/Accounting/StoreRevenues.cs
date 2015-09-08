// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoreRevenues.cs" company="Allors bvba">
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


    public partial class StoreRevenues
    {
        public static StoreRevenue AppsFindOrCreateAsDependable(ISession session, SalesInvoice invoice)
        {
            var storeRevenues = invoice.Store.StoreRevenuesWhereStore;
            storeRevenues.Filter.AddEquals(Meta.InternalOrganisation, invoice.BilledFromInternalOrganisation);
            storeRevenues.Filter.AddEquals(Meta.Year, invoice.InvoiceDate.Year);
            storeRevenues.Filter.AddEquals(Meta.Month, invoice.InvoiceDate.Month);
            var storeRevenue = storeRevenues.First ?? new StoreRevenueBuilder(session)
                                                            .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                                                            .WithStore((Store)session.Instantiate(invoice.Store))
                                                            .WithYear(invoice.InvoiceDate.Year)
                                                            .WithMonth(invoice.InvoiceDate.Month)
                                                            .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                                                            .WithRevenue(0M)
                                                            .Build();

            InternalOrganisationRevenues.AppsFindOrCreateAsDependable(session, storeRevenue);

            return storeRevenue;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var storeRevenuesByPeriodByStoreByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Store, Dictionary<DateTime, StoreRevenue>>>();

            var storeRevenues = session.Extent<StoreRevenue>();

            foreach (StoreRevenue storeRevenue in storeRevenues)
            {
                storeRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(storeRevenue.Year, storeRevenue.Month, 01);

                Dictionary<Store, Dictionary<DateTime, StoreRevenue>> storeRevenuesByPeriodByStore;
                if (!storeRevenuesByPeriodByStoreByInternalOrganisation.TryGetValue(storeRevenue.InternalOrganisation, out storeRevenuesByPeriodByStore))
                {
                    storeRevenuesByPeriodByStore = new Dictionary<Store, Dictionary<DateTime, StoreRevenue>>();
                    storeRevenuesByPeriodByStoreByInternalOrganisation[storeRevenue.InternalOrganisation] = storeRevenuesByPeriodByStore;
                }

                Dictionary<DateTime, StoreRevenue> storeRevenuesByPeriod;
                if (!storeRevenuesByPeriodByStore.TryGetValue(storeRevenue.Store, out storeRevenuesByPeriod))
                {
                    storeRevenuesByPeriod = new Dictionary<DateTime, StoreRevenue>();
                    storeRevenuesByPeriodByStore[storeRevenue.Store] = storeRevenuesByPeriod;
                }

                StoreRevenue revenue;
                if (!storeRevenuesByPeriod.TryGetValue(date, out revenue))
                {
                    storeRevenuesByPeriod.Add(date, storeRevenue);
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
                    StoreRevenue storeRevenue;

                    Dictionary<Store, Dictionary<DateTime, StoreRevenue>> storeRevenuesByPeriodByStore;
                    if (!storeRevenuesByPeriodByStoreByInternalOrganisation.TryGetValue(salesInvoice.BilledFromInternalOrganisation, out storeRevenuesByPeriodByStore))
                    {
                        storeRevenue = CreateStoreRevenue(session, salesInvoice);

                        storeRevenuesByPeriodByStore = new Dictionary<Store, Dictionary<DateTime, StoreRevenue>>
                        {
                            { salesInvoice.Store, new Dictionary<DateTime, StoreRevenue> { { date, storeRevenue } } }
                        };

                        storeRevenuesByPeriodByStoreByInternalOrganisation[salesInvoice.BilledFromInternalOrganisation] = storeRevenuesByPeriodByStore;
                    }

                    Dictionary<DateTime, StoreRevenue> storeRevenuesByPeriod;
                    if (!storeRevenuesByPeriodByStore.TryGetValue(salesInvoice.Store, out storeRevenuesByPeriod))
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

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute }; 
            
            config.GrantAdministrator(this.ObjectType, full);
        }

        private static StoreRevenue CreateStoreRevenue(ISession session, SalesInvoice invoice)
        {
            return new StoreRevenueBuilder(session)
                        .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                        .WithStore(invoice.Store)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithRevenue(0M)
                        .Build();
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesChannelRevenues.cs" company="Allors bvba">
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


    public partial class SalesChannelRevenues
    {
        public static SalesChannelRevenue AppsFindOrCreateAsDependable(ISession session, SalesInvoice invoice)
        {
            SalesChannelRevenue salesChannelRevenue = null;
            if (invoice.ExistSalesChannel)
            {
                var salesChannelRevenues = invoice.SalesChannel.SalesChannelRevenuesWhereSalesChannel;
                salesChannelRevenues.Filter.AddEquals(SalesChannelRevenues.Meta.InternalOrganisation, invoice.BilledFromInternalOrganisation);
                salesChannelRevenues.Filter.AddEquals(SalesChannelRevenues.Meta.Year, invoice.InvoiceDate.Year);
                salesChannelRevenues.Filter.AddEquals(SalesChannelRevenues.Meta.Month, invoice.InvoiceDate.Month);
                salesChannelRevenue = salesChannelRevenues.First ?? new SalesChannelRevenueBuilder(session)
                                                                            .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                                                                            .WithSalesChannel((SalesChannel)session.Instantiate(invoice.SalesChannel))
                                                                            .WithYear(invoice.InvoiceDate.Year)
                                                                            .WithMonth(invoice.InvoiceDate.Month)
                                                                            .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                                                                            .WithRevenue(0M)
                                                                            .Build();
            }

            return salesChannelRevenue;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var salesChannelRevenuesByPeriodBySalesChannelByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<SalesChannel, Dictionary<DateTime, SalesChannelRevenue>>>();

            var salesChannelRevenues = session.Extent<SalesChannelRevenue>();

            foreach (SalesChannelRevenue salesChannelRevenue in salesChannelRevenues)
            {
                salesChannelRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(salesChannelRevenue.Year, salesChannelRevenue.Month, 01);

                Dictionary<SalesChannel, Dictionary<DateTime, SalesChannelRevenue>> salesChannelRevenuesByPeriodBySalesChannel;
                if (!salesChannelRevenuesByPeriodBySalesChannelByInternalOrganisation.TryGetValue(salesChannelRevenue.InternalOrganisation, out salesChannelRevenuesByPeriodBySalesChannel))
                {
                    salesChannelRevenuesByPeriodBySalesChannel = new Dictionary<SalesChannel, Dictionary<DateTime, SalesChannelRevenue>>();
                    salesChannelRevenuesByPeriodBySalesChannelByInternalOrganisation[salesChannelRevenue.InternalOrganisation] = salesChannelRevenuesByPeriodBySalesChannel;
                }

                Dictionary<DateTime, SalesChannelRevenue> salesChannelRevenuesByPeriod;
                if (!salesChannelRevenuesByPeriodBySalesChannel.TryGetValue(salesChannelRevenue.SalesChannel, out salesChannelRevenuesByPeriod))
                {
                    salesChannelRevenuesByPeriod = new Dictionary<DateTime, SalesChannelRevenue>();
                    salesChannelRevenuesByPeriodBySalesChannel[salesChannelRevenue.SalesChannel] = salesChannelRevenuesByPeriod;
                }

                SalesChannelRevenue revenue;
                if (!salesChannelRevenuesByPeriod.TryGetValue(date, out revenue))
                {
                    salesChannelRevenuesByPeriod.Add(date, salesChannelRevenue);
                }
            }

            var revenues = new HashSet<ObjectId>();

            var salesInvoices = session.Extent<SalesInvoice>();
            salesInvoices.Filter.AddExists(SalesInvoices.Meta.SalesChannel);

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
                    SalesChannelRevenue salesChannelRevenue;

                    Dictionary<SalesChannel, Dictionary<DateTime, SalesChannelRevenue>> salesChannelRevenuesByPeriodBySalesChannel;
                    if (!salesChannelRevenuesByPeriodBySalesChannelByInternalOrganisation.TryGetValue(salesInvoice.BilledFromInternalOrganisation, out salesChannelRevenuesByPeriodBySalesChannel))
                    {
                        salesChannelRevenue = CreateSalesChannelRevenue(session, salesInvoice);

                        salesChannelRevenuesByPeriodBySalesChannel = new Dictionary<SalesChannel, Dictionary<DateTime, SalesChannelRevenue>>
                        {
                            { salesInvoice.SalesChannel, new Dictionary<DateTime, SalesChannelRevenue> { { date, salesChannelRevenue } } }
                        };

                        salesChannelRevenuesByPeriodBySalesChannelByInternalOrganisation[salesInvoice.BilledFromInternalOrganisation] = salesChannelRevenuesByPeriodBySalesChannel;
                    }

                    Dictionary<DateTime, SalesChannelRevenue> salesChannelRevenuesByPeriod;
                    if (!salesChannelRevenuesByPeriodBySalesChannel.TryGetValue(salesInvoice.SalesChannel, out salesChannelRevenuesByPeriod))
                    {
                        salesChannelRevenue = CreateSalesChannelRevenue(session, salesInvoice);

                        salesChannelRevenuesByPeriod = new Dictionary<DateTime, SalesChannelRevenue>
                        {
                            { date, salesChannelRevenue } 
                        };

                        salesChannelRevenuesByPeriodBySalesChannel[salesInvoice.SalesChannel] = salesChannelRevenuesByPeriod;
                    }

                    if (!salesChannelRevenuesByPeriod.TryGetValue(date, out salesChannelRevenue))
                    {
                        salesChannelRevenue = CreateSalesChannelRevenue(session, salesInvoice);
                        salesChannelRevenuesByPeriod.Add(date, salesChannelRevenue);
                    }

                    revenues.Add(salesChannelRevenue.Id);
                    salesChannelRevenue.Revenue += salesInvoiceItem.TotalExVat;
                }
            }

            foreach (SalesChannelRevenue salesChannelRevenue in salesChannelRevenues)
            {
                if (!revenues.Contains(salesChannelRevenue.Id))
                {
                    salesChannelRevenue.Delete();
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static SalesChannelRevenue CreateSalesChannelRevenue(ISession session, SalesInvoice invoice)
        {
            return new SalesChannelRevenueBuilder(session)
                        .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                        .WithSalesChannel(invoice.SalesChannel)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithRevenue(0M)
                        .Build();
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesChannelRevenues.cs" company="Allors bvba">
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

    public partial class SalesChannelRevenues
    {
        public static SalesChannelRevenue AppsFindOrCreateAsDependable(ISession session, SalesInvoice invoice)
        {
            SalesChannelRevenue salesChannelRevenue = null;
            if (invoice.ExistSalesChannel)
            {
                var salesChannelRevenues = invoice.SalesChannel.SalesChannelRevenuesWhereSalesChannel;
                salesChannelRevenues.Filter.AddEquals(M.SalesChannelRevenue.Year, invoice.InvoiceDate.Year);
                salesChannelRevenues.Filter.AddEquals(M.SalesChannelRevenue.Month, invoice.InvoiceDate.Month);
                salesChannelRevenue = salesChannelRevenues.First ?? new SalesChannelRevenueBuilder(session)
                                                                            .WithSalesChannel((SalesChannel)session.Instantiate(invoice.SalesChannel))
                                                                            .WithYear(invoice.InvoiceDate.Year)
                                                                            .WithMonth(invoice.InvoiceDate.Month)
                                                                            .WithCurrency(invoice.Currency)
                                                                            .Build();
            }

            return salesChannelRevenue;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var salesChannelRevenuesByPeriodBySalesChannel = new Dictionary<SalesChannel, Dictionary<DateTime, SalesChannelRevenue>>();

            var salesChannelRevenues = session.Extent<SalesChannelRevenue>();

            foreach (SalesChannelRevenue salesChannelRevenue in salesChannelRevenues)
            {
                salesChannelRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(salesChannelRevenue.Year, salesChannelRevenue.Month, 01);
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

            var revenues = new HashSet<long>();

            var salesInvoices = session.Extent<SalesInvoice>();
            salesInvoices.Filter.AddExists(M.SalesInvoice.SalesChannel);

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

        private static SalesChannelRevenue CreateSalesChannelRevenue(ISession session, SalesInvoice invoice)
        {
            return new SalesChannelRevenueBuilder(session)
                        .WithSalesChannel(invoice.SalesChannel)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.Currency)
                        .Build();
        }
    }
}

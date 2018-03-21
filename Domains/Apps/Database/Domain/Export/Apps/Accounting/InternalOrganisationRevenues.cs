// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalOrganisationRevenues.cs" company="Allors bvba">
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

    public partial class InternalOrganisationRevenues
    {
        public static InternalOrganisationRevenue AppsFindOrCreateAsDependable(ISession session, StoreRevenue dependant)
        {
            var internalOrganisationRevenues = session.Extent<InternalOrganisationRevenue>();
            internalOrganisationRevenues.Filter.AddEquals(M.InternalOrganisationRevenue.Year, dependant.Year);
            internalOrganisationRevenues.Filter.AddEquals(M.InternalOrganisationRevenue.Month, dependant.Month);
            var internalOrganisationRevenue = internalOrganisationRevenues.First
                                              ?? new InternalOrganisationRevenueBuilder(session)
                                                        .WithYear(dependant.Year)
                                                        .WithMonth(dependant.Month)
                                                        .WithCurrency(dependant.Currency)
                                                        .Build();
            return internalOrganisationRevenue;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var internalOrganisationRevenues = session.Extent<InternalOrganisationRevenue>();
            Dictionary<DateTime, InternalOrganisationRevenue> internalOrganisationRevenuesByPeriod = new Dictionary<DateTime, InternalOrganisationRevenue>();

            foreach (InternalOrganisationRevenue internalOrganisationRevenue in internalOrganisationRevenues)
            {
                internalOrganisationRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(internalOrganisationRevenue.Year, internalOrganisationRevenue.Month, 01);

                if (!internalOrganisationRevenuesByPeriod.ContainsKey(date))
                {
                    internalOrganisationRevenuesByPeriod.Add(date, internalOrganisationRevenue);
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
                    InternalOrganisationRevenue internalOrganisationRevenue;

                    if (!internalOrganisationRevenuesByPeriod.TryGetValue(date, out internalOrganisationRevenue))
                    {
                        internalOrganisationRevenue = CreateInternalOrganisationRevenue(session, salesInvoice);
                        internalOrganisationRevenuesByPeriod[date] = internalOrganisationRevenue;
                    }

                    revenues.Add(internalOrganisationRevenue.Id);
                    internalOrganisationRevenue.Revenue += salesInvoiceItem.TotalExVat;
                }
            }

            foreach (InternalOrganisationRevenue internalOrganisationRevenue in internalOrganisationRevenues)
            {
                if (!revenues.Contains(internalOrganisationRevenue.Id))
                {
                    internalOrganisationRevenue.Delete();
                }
            }
        }

        private static InternalOrganisationRevenue CreateInternalOrganisationRevenue(ISession session, SalesInvoice invoice)
        {
            return new InternalOrganisationRevenueBuilder(session)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.Currency)
                        .Build();
        }
    }
}

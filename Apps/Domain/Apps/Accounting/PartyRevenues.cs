// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyRevenues.cs" company="Allors bvba">
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


    public partial class PartyRevenues
    {
        public static PartyRevenue AppsFindOrCreateAsDependable(ISession session, PartyProductRevenue dependant)
        {
            var partyRevenues = dependant.Party.PartyRevenuesWhereParty;
            partyRevenues.Filter.AddEquals(PartyRevenues.Meta.InternalOrganisation, dependant.InternalOrganisation);
            partyRevenues.Filter.AddEquals(PartyRevenues.Meta.Year, dependant.Year);
            partyRevenues.Filter.AddEquals(PartyRevenues.Meta.Month, dependant.Month);
            var partyRevenue = partyRevenues.First ?? new PartyRevenueBuilder(session)
                                                            .WithInternalOrganisation(dependant.InternalOrganisation)
                                                            .WithParty(dependant.Party)
                                                            .WithYear(dependant.Year)
                                                            .WithMonth(dependant.Month)
                                                            .WithCurrency(dependant.Currency)
                                                            .WithRevenue(0M)
                                                            .Build();
            return partyRevenue;
        }

        public static PartyRevenue AppsFindOrCreateAsDependable(ISession session, SalesInvoice invoice)
        {
            var partyRevenues = invoice.BilledFromInternalOrganisation.PartyRevenuesWhereParty;
            partyRevenues.Filter.AddEquals(PartyRevenues.Meta.Year, invoice.InvoiceDate.Year);
            partyRevenues.Filter.AddEquals(PartyRevenues.Meta.Month, invoice.InvoiceDate.Month);
            var partyRevenue = partyRevenues.First ?? new PartyRevenueBuilder(session)
                                                            .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                                                            .WithParty(invoice.BilledFromInternalOrganisation)
                                                            .WithYear(invoice.InvoiceDate.Year)
                                                            .WithMonth(invoice.InvoiceDate.Month)
                                                            .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                                                            .WithRevenue(0M)
                                                            .Build();

            return partyRevenue;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var partyRevenuesByPeriodByPartyByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<DateTime, PartyRevenue>>>();

            var partyRevenues = session.Extent<PartyRevenue>();

            foreach (PartyRevenue partyRevenue in partyRevenues)
            {
                partyRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(partyRevenue.Year, partyRevenue.Month, 01);

                Dictionary<Party, Dictionary<DateTime, PartyRevenue>> partyRevenuesByPeriodByParty;
                if (!partyRevenuesByPeriodByPartyByInternalOrganisation.TryGetValue(partyRevenue.InternalOrganisation, out partyRevenuesByPeriodByParty))
                {
                    partyRevenuesByPeriodByParty = new Dictionary<Party, Dictionary<DateTime, PartyRevenue>>();
                    partyRevenuesByPeriodByPartyByInternalOrganisation[partyRevenue.InternalOrganisation] = partyRevenuesByPeriodByParty;
                }

                Dictionary<DateTime, PartyRevenue> partyRevenuesByPeriod;
                if (!partyRevenuesByPeriodByParty.TryGetValue(partyRevenue.Party, out partyRevenuesByPeriod))
                {
                    partyRevenuesByPeriod = new Dictionary<DateTime, PartyRevenue>();
                    partyRevenuesByPeriodByParty[partyRevenue.Party] = partyRevenuesByPeriod;
                }

                PartyRevenue revenue;
                if (!partyRevenuesByPeriod.TryGetValue(date, out revenue))
                {
                    partyRevenuesByPeriod.Add(date, partyRevenue);
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
                    PartyRevenue partyRevenue;

                    Dictionary<Party, Dictionary<DateTime, PartyRevenue>> partyRevenuesByPeriodByParty;
                    if (!partyRevenuesByPeriodByPartyByInternalOrganisation.TryGetValue(salesInvoice.BilledFromInternalOrganisation, out partyRevenuesByPeriodByParty))
                    {
                        partyRevenue = CreatePartyRevenue(session, salesInvoice);

                        partyRevenuesByPeriodByParty = new Dictionary<Party, Dictionary<DateTime, PartyRevenue>>
                            {
                                { salesInvoice.BillToCustomer, new Dictionary<DateTime, PartyRevenue> { { date, partyRevenue } } }
                            };

                        partyRevenuesByPeriodByPartyByInternalOrganisation[salesInvoice.BilledFromInternalOrganisation] = partyRevenuesByPeriodByParty;
                    }

                    Dictionary<DateTime, PartyRevenue> partyRevenuesByPeriod;
                    if (!partyRevenuesByPeriodByParty.TryGetValue(salesInvoice.BillToCustomer, out partyRevenuesByPeriod))
                    {
                        partyRevenue = CreatePartyRevenue(session, salesInvoice);

                        partyRevenuesByPeriod = new Dictionary<DateTime, PartyRevenue>
                            {
                                { date, partyRevenue } 
                            };

                        partyRevenuesByPeriodByParty[salesInvoice.BillToCustomer] = partyRevenuesByPeriod;
                    }

                    if (!partyRevenuesByPeriod.TryGetValue(date, out partyRevenue))
                    {
                        partyRevenue = CreatePartyRevenue(session, salesInvoice);
                        partyRevenuesByPeriod.Add(date, partyRevenue);
                    }

                    revenues.Add(partyRevenue.Id);
                    partyRevenue.Revenue += salesInvoiceItem.TotalExVat;
                }
            }

            foreach (PartyRevenue partyRevenue in partyRevenues)
            {
                if (!revenues.Contains(partyRevenue.Id))
                {
                    partyRevenue.Delete();
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static PartyRevenue CreatePartyRevenue(ISession session, SalesInvoice invoice)
        {
            return new PartyRevenueBuilder(session)
                        .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                        .WithParty(invoice.BillToCustomer)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithRevenue(0M)
                        .Build();
        }
    }
}

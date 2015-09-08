// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepRevenueHistories.cs" company="Allors bvba">
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


    public partial class SalesRepRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var derivation = new Derivation(session);

            var salesRepRevenuesByPeriodBySalesRepByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Person, Dictionary<DateTime, SalesRepRevenue>>>();

            var salesRepRevenues = session.Extent<SalesRepRevenue>();

            foreach (SalesRepRevenue salesRepRevenue in salesRepRevenues)
            {
                var months = ((DateTime.UtcNow.Year - salesRepRevenue.Year) * 12) + DateTime.UtcNow.Month - salesRepRevenue.Month;
                if (months <= 12)
                {
                    var date = DateTimeFactory.CreateDate(salesRepRevenue.Year, salesRepRevenue.Month, 01);

                    Dictionary<Person, Dictionary<DateTime, SalesRepRevenue>> salesRepRevenuesByPeriodBySalesRep;
                    if (!salesRepRevenuesByPeriodBySalesRepByInternalOrganisation.TryGetValue(salesRepRevenue.InternalOrganisation, out salesRepRevenuesByPeriodBySalesRep))
                    {
                        salesRepRevenuesByPeriodBySalesRep = new Dictionary<Person, Dictionary<DateTime, SalesRepRevenue>>();
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
            }

            var salesRepRevenueHistoriesBySalesRepByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Person, SalesRepRevenueHistory>>();

            var salesRepRevenueHistories = session.Extent<SalesRepRevenueHistory>();

            foreach (SalesRepRevenueHistory salesRepRevenueHistory in salesRepRevenueHistories)
            {
                salesRepRevenueHistory.Revenue = 0;

                Dictionary<Person, SalesRepRevenueHistory> salesRepRevenueHistoriesBySalesRep;
                if (!salesRepRevenueHistoriesBySalesRepByInternalOrganisation.TryGetValue(salesRepRevenueHistory.InternalOrganisation, out salesRepRevenueHistoriesBySalesRep))
                {
                    salesRepRevenueHistoriesBySalesRep = new Dictionary<Person, SalesRepRevenueHistory>();
                    salesRepRevenueHistoriesBySalesRepByInternalOrganisation[salesRepRevenueHistory.InternalOrganisation] = salesRepRevenueHistoriesBySalesRep;
                }

                SalesRepRevenueHistory revenueHistory;
                if (!salesRepRevenueHistoriesBySalesRep.TryGetValue(salesRepRevenueHistory.SalesRep, out revenueHistory))
                {
                    salesRepRevenueHistoriesBySalesRep.Add(salesRepRevenueHistory.SalesRep, salesRepRevenueHistory);
                }
            }

            foreach (var keyValuePair in salesRepRevenuesByPeriodBySalesRepByInternalOrganisation)
            {
                Dictionary<Person, SalesRepRevenueHistory> salesRepRevenueHistoriesBySalesRep;
                if (!salesRepRevenueHistoriesBySalesRepByInternalOrganisation.TryGetValue(keyValuePair.Key, out salesRepRevenueHistoriesBySalesRep))
                {
                    salesRepRevenueHistoriesBySalesRep = new Dictionary<Person, SalesRepRevenueHistory>();
                    salesRepRevenueHistoriesBySalesRepByInternalOrganisation[keyValuePair.Key] = salesRepRevenueHistoriesBySalesRep;
                }

                foreach (var valuePair in keyValuePair.Value)
                {
                    SalesRepRevenueHistory salesRepRevenueHistory;

                    if (!salesRepRevenueHistoriesBySalesRep.TryGetValue(valuePair.Key, out salesRepRevenueHistory))
                    {
                        SalesRepRevenue partyRevenue = null;
                        foreach (var salesRepRevenuesByPeriod in valuePair.Value)
                        {
                            partyRevenue = salesRepRevenuesByPeriod.Value;
                            break;
                        }

                        salesRepRevenueHistory = CreateSalesRepRevenueHistory(session, partyRevenue);
                        salesRepRevenueHistoriesBySalesRep.Add(salesRepRevenueHistory.SalesRep, salesRepRevenueHistory);
                    }

                    foreach (var partyRevenueByPeriod in valuePair.Value)
                    {
                        var partyRevenue = partyRevenueByPeriod.Value;
                        salesRepRevenueHistory.Revenue += partyRevenue.Revenue;
                    }

                    salesRepRevenueHistory.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }


        private static SalesRepRevenueHistory CreateSalesRepRevenueHistory(ISession session, SalesRepRevenue salesRepRevenue)
        {
            return new SalesRepRevenueHistoryBuilder(session)
                        .WithCurrency(salesRepRevenue.Currency)
                        .WithInternalOrganisation(salesRepRevenue.InternalOrganisation)
                        .WithSalesRep(salesRepRevenue.SalesRep)
                        .WithRevenue(0)
                        .Build();
        }
    }
}

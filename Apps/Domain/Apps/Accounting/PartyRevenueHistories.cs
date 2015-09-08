// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyRevenueHistories.cs" company="Allors bvba">
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


    public partial class PartyRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var partyRevenuesByPeriodByPartyByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<DateTime, PartyRevenue>>>();

            var partyRevenues = session.Extent<PartyRevenue>();

            foreach (PartyRevenue partyRevenue in partyRevenues)
            {
                var months = ((DateTime.UtcNow.Year - partyRevenue.Year) * 12) + DateTime.UtcNow.Month - partyRevenue.Month;
                if (months <= 12)
                {
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
            }

            var partyRevenueHistoriesByPartyByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Party, PartyRevenueHistory>>();

            var partyRevenueHistories = session.Extent<PartyRevenueHistory>();

            foreach (PartyRevenueHistory partyRevenueHistory in partyRevenueHistories)
            {
                partyRevenueHistory.Revenue = 0;

                Dictionary<Party, PartyRevenueHistory> partyRevenueHistoriesByParty;
                if (!partyRevenueHistoriesByPartyByInternalOrganisation.TryGetValue(partyRevenueHistory.InternalOrganisation, out partyRevenueHistoriesByParty))
                {
                    partyRevenueHistoriesByParty = new Dictionary<Party, PartyRevenueHistory>();
                    partyRevenueHistoriesByPartyByInternalOrganisation[partyRevenueHistory.InternalOrganisation] = partyRevenueHistoriesByParty;
                }

                PartyRevenueHistory revenueHistory;
                if (!partyRevenueHistoriesByParty.TryGetValue(partyRevenueHistory.Party, out revenueHistory))
                {
                    partyRevenueHistoriesByParty.Add(partyRevenueHistory.Party, partyRevenueHistory);
                }
            }

            foreach (var keyValuePair in partyRevenuesByPeriodByPartyByInternalOrganisation)
            {
                Dictionary<Party, PartyRevenueHistory> partyRevenueHistoriesByParty;
                if (!partyRevenueHistoriesByPartyByInternalOrganisation.TryGetValue(keyValuePair.Key, out partyRevenueHistoriesByParty))
                {
                    partyRevenueHistoriesByParty = new Dictionary<Party, PartyRevenueHistory>();
                    partyRevenueHistoriesByPartyByInternalOrganisation[keyValuePair.Key] = partyRevenueHistoriesByParty;
                }

                foreach (var valuePair in keyValuePair.Value)
                {
                    PartyRevenueHistory partyRevenueHistory;

                    if (!partyRevenueHistoriesByParty.TryGetValue(valuePair.Key, out partyRevenueHistory))
                    {
                        PartyRevenue partyRevenue = null;
                        foreach (var partyRevenuesByPeriod in valuePair.Value)
                        {
                            partyRevenue = partyRevenuesByPeriod.Value;
                            break;
                        }

                        partyRevenueHistory = CreatePartyRevenueHistory(session, partyRevenue);
                        partyRevenueHistoriesByParty.Add(partyRevenueHistory.Party, partyRevenueHistory);
                    }

                    foreach (var partyRevenueByPeriod in valuePair.Value)
                    {
                        var partyRevenue = partyRevenueByPeriod.Value;
                        partyRevenueHistory.Revenue += partyRevenue.Revenue;
                    }
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static PartyRevenueHistory CreatePartyRevenueHistory(ISession session, PartyRevenue partyRevenue)
        {
            return new PartyRevenueHistoryBuilder(session)
                        .WithCurrency(partyRevenue.Currency)
                        .WithInternalOrganisation(partyRevenue.InternalOrganisation)
                        .WithParty(partyRevenue.Party)
                        .WithRevenue(0)
                        .Build();
        }
    }
}

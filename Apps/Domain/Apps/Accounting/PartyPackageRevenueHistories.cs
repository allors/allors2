// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyPackageRevenueHistories.cs" company="Allors bvba">
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


    public partial class PartyPackageRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation =
                new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>>>();

            var partyPackageRevenues = session.Extent<PartyPackageRevenue>();

            foreach (PartyPackageRevenue partyPackageRevenue in partyPackageRevenues)
            {
                var months = ((DateTime.UtcNow.Year - partyPackageRevenue.Year) * 12) + DateTime.UtcNow.Month - partyPackageRevenue.Month;
                if (months <= 12)
                {
                    var date = DateTimeFactory.CreateDate(partyPackageRevenue.Year, partyPackageRevenue.Month, 01);

                    Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>> partyPackageRevenuesByPeriodByPacakgeByParty;
                    if (!partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation.TryGetValue(partyPackageRevenue.InternalOrganisation, out partyPackageRevenuesByPeriodByPacakgeByParty))
                    {
                        partyPackageRevenuesByPeriodByPacakgeByParty = new Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>>();
                        partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation[partyPackageRevenue.InternalOrganisation] = partyPackageRevenuesByPeriodByPacakgeByParty;
                    }

                    Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>> partyPackageRevenuesByPeriodByPackage;
                    if (!partyPackageRevenuesByPeriodByPacakgeByParty.TryGetValue(partyPackageRevenue.Party, out partyPackageRevenuesByPeriodByPackage))
                    {
                        partyPackageRevenuesByPeriodByPackage = new Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>();
                        partyPackageRevenuesByPeriodByPacakgeByParty[partyPackageRevenue.Party] = partyPackageRevenuesByPeriodByPackage;
                    }

                    Dictionary<DateTime, PartyPackageRevenue> partyPackageRevenuesByPeriod;
                    if (!partyPackageRevenuesByPeriodByPackage.TryGetValue(partyPackageRevenue.Package, out partyPackageRevenuesByPeriod))
                    {
                        partyPackageRevenuesByPeriod = new Dictionary<DateTime, PartyPackageRevenue>();
                        partyPackageRevenuesByPeriodByPackage[partyPackageRevenue.Package] = partyPackageRevenuesByPeriod;
                    }

                    PartyPackageRevenue revenue;
                    if (!partyPackageRevenuesByPeriod.TryGetValue(date, out revenue))
                    {
                        partyPackageRevenuesByPeriod.Add(date, partyPackageRevenue);
                    }
                }
            }

            var partyPackageRevenueHistoriesByPackageByPartyByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<Package, PartyPackageRevenueHistory>>>();

            var partyPackageRevenueHistories = session.Extent<PartyPackageRevenueHistory>();

            foreach (PartyPackageRevenueHistory partyPackageRevenueHistory in partyPackageRevenueHistories)
            {
                partyPackageRevenueHistory.Revenue = 0;

                Dictionary<Party, Dictionary<Package, PartyPackageRevenueHistory>> partyPackageRevenueHistoriesByPackageByParty;
                if (!partyPackageRevenueHistoriesByPackageByPartyByInternalOrganisation.TryGetValue(partyPackageRevenueHistory.InternalOrganisation, out partyPackageRevenueHistoriesByPackageByParty))
                {
                    partyPackageRevenueHistoriesByPackageByParty = new Dictionary<Party, Dictionary<Package, PartyPackageRevenueHistory>>();
                    partyPackageRevenueHistoriesByPackageByPartyByInternalOrganisation[partyPackageRevenueHistory.InternalOrganisation] = partyPackageRevenueHistoriesByPackageByParty;
                }

                Dictionary<Package, PartyPackageRevenueHistory> partyPackageRevenueHistoriesByPackage;
                if (!partyPackageRevenueHistoriesByPackageByParty.TryGetValue(partyPackageRevenueHistory.Party, out partyPackageRevenueHistoriesByPackage))
                {
                    partyPackageRevenueHistoriesByPackage = new Dictionary<Package, PartyPackageRevenueHistory>();
                    partyPackageRevenueHistoriesByPackageByParty[partyPackageRevenueHistory.Party] = partyPackageRevenueHistoriesByPackage;
                }

                PartyPackageRevenueHistory revenueHistory;
                if (!partyPackageRevenueHistoriesByPackage.TryGetValue(partyPackageRevenueHistory.Package, out revenueHistory))
                {
                    partyPackageRevenueHistoriesByPackage.Add(partyPackageRevenueHistory.Package, partyPackageRevenueHistory);
                }
            }

            foreach (var keyValuePair in partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation)
            {
                Dictionary<Party, Dictionary<Package, PartyPackageRevenueHistory>> partyPackageRevenueHistoriesByPackageByParty;
                if (!partyPackageRevenueHistoriesByPackageByPartyByInternalOrganisation.TryGetValue(keyValuePair.Key, out partyPackageRevenueHistoriesByPackageByParty))
                {
                    partyPackageRevenueHistoriesByPackageByParty = new Dictionary<Party, Dictionary<Package, PartyPackageRevenueHistory>>();
                    partyPackageRevenueHistoriesByPackageByPartyByInternalOrganisation[keyValuePair.Key] = partyPackageRevenueHistoriesByPackageByParty;
                }

                foreach (var partyPackageRevenuesByPeriodByPackageByParty in keyValuePair.Value)
                {
                    Dictionary<Package, PartyPackageRevenueHistory> partyPackageRevenueHistoriesByPackage;
                    if (!partyPackageRevenueHistoriesByPackageByParty.TryGetValue(partyPackageRevenuesByPeriodByPackageByParty.Key, out partyPackageRevenueHistoriesByPackage))
                    {
                        partyPackageRevenueHistoriesByPackage = new Dictionary<Package, PartyPackageRevenueHistory>();
                        partyPackageRevenueHistoriesByPackageByParty[keyValuePair.Key] = partyPackageRevenueHistoriesByPackage;
                    }

                    foreach (var partyPackageRevenuesByPeriodByPackage in partyPackageRevenuesByPeriodByPackageByParty.Value)
                    {
                        PartyPackageRevenueHistory partyPackageRevenueHistory;

                        if (!partyPackageRevenueHistoriesByPackage.TryGetValue(partyPackageRevenuesByPeriodByPackage.Key, out partyPackageRevenueHistory))
                        {
                            PartyPackageRevenue partyPackageRevenue = null;
                            foreach (var partyPackageRevenuesByPeriod in partyPackageRevenuesByPeriodByPackage.Value)
                            {
                                partyPackageRevenue = partyPackageRevenuesByPeriod.Value;
                                break;
                            }

                            partyPackageRevenueHistory = CreatePartyRevenueHistory(session, partyPackageRevenue);
                            partyPackageRevenueHistoriesByPackage.Add(partyPackageRevenueHistory.Package, partyPackageRevenueHistory);
                        }

                        foreach (var partyPackageRevenueByPeriod in partyPackageRevenuesByPeriodByPackage.Value)
                        {
                            var partyPackageRevenue = partyPackageRevenueByPeriod.Value;
                            partyPackageRevenueHistory.Revenue += partyPackageRevenue.Revenue;
                        }

                        partyPackageRevenueHistory.AppsOnDeriveHistory();
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

        private static PartyPackageRevenueHistory CreatePartyRevenueHistory(ISession session, PartyPackageRevenue partyPackageRevenue)
        {
            return new PartyPackageRevenueHistoryBuilder(session)
                        .WithCurrency(partyPackageRevenue.Currency)
                        .WithInternalOrganisation(partyPackageRevenue.InternalOrganisation)
                        .WithParty(partyPackageRevenue.Party)
                        .WithPackage(partyPackageRevenue.Package)
                        .WithRevenue(0)
                        .Build();
        }
    }
}

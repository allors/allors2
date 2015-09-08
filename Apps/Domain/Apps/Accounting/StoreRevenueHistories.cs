// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoreRevenueHistories.cs" company="Allors bvba">
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


    public partial class StoreRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var derivation = new Derivation(session);

            var storeRevenuesByPeriodByStoreByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Store, Dictionary<DateTime, StoreRevenue>>>();

            var storeRevenues = session.Extent<StoreRevenue>();

            foreach (StoreRevenue storeRevenue in storeRevenues)
            {
                var months = ((DateTime.UtcNow.Year - storeRevenue.Year) * 12) + DateTime.UtcNow.Month - storeRevenue.Month;
                if (months <= 12)
                {
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
            }

            var storeRevenueHistoriesByStoreByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Store, StoreRevenueHistory>>();

            var storeRevenueHistories = session.Extent<StoreRevenueHistory>();

            foreach (StoreRevenueHistory storeRevenueHistory in storeRevenueHistories)
            {
                storeRevenueHistory.Revenue = 0;

                Dictionary<Store, StoreRevenueHistory> storeRevenueHistoriesByStore;
                if (!storeRevenueHistoriesByStoreByInternalOrganisation.TryGetValue(storeRevenueHistory.InternalOrganisation, out storeRevenueHistoriesByStore))
                {
                    storeRevenueHistoriesByStore = new Dictionary<Store, StoreRevenueHistory>();
                    storeRevenueHistoriesByStoreByInternalOrganisation[storeRevenueHistory.InternalOrganisation] = storeRevenueHistoriesByStore;
                }

                StoreRevenueHistory revenueHistory;
                if (!storeRevenueHistoriesByStore.TryGetValue(storeRevenueHistory.Store, out revenueHistory))
                {
                    storeRevenueHistoriesByStore.Add(storeRevenueHistory.Store, storeRevenueHistory);
                }
            }

            foreach (var keyValuePair in storeRevenuesByPeriodByStoreByInternalOrganisation)
            {
                Dictionary<Store, StoreRevenueHistory> storeRevenueHistoriesByStore;
                if (!storeRevenueHistoriesByStoreByInternalOrganisation.TryGetValue(keyValuePair.Key, out storeRevenueHistoriesByStore))
                {
                    storeRevenueHistoriesByStore = new Dictionary<Store, StoreRevenueHistory>();
                    storeRevenueHistoriesByStoreByInternalOrganisation[keyValuePair.Key] = storeRevenueHistoriesByStore;
                }

                foreach (var valuePair in keyValuePair.Value)
                {
                    StoreRevenueHistory storeRevenueHistory;

                    if (!storeRevenueHistoriesByStore.TryGetValue(valuePair.Key, out storeRevenueHistory))
                    {
                        StoreRevenue partyRevenue = null;
                        foreach (var storeRevenuesByPeriod in valuePair.Value)
                        {
                            partyRevenue = storeRevenuesByPeriod.Value;
                            break;
                        }

                        storeRevenueHistory = CreateStoreRevenueHistory(session, partyRevenue);
                        storeRevenueHistoriesByStore.Add(storeRevenueHistory.Store, storeRevenueHistory);
                    }

                    foreach (var partyRevenueByPeriod in valuePair.Value)
                    {
                        var partyRevenue = partyRevenueByPeriod.Value;
                        storeRevenueHistory.Revenue += partyRevenue.Revenue;
                    }

                    storeRevenueHistory.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };
            
            config.GrantAdministrator(this.ObjectType, full);
        }

        private static StoreRevenueHistory CreateStoreRevenueHistory(ISession session, StoreRevenue storeRevenue)
        {
            return new StoreRevenueHistoryBuilder(session)
                        .WithCurrency(storeRevenue.Currency)
                        .WithInternalOrganisation(storeRevenue.InternalOrganisation)
                        .WithStore(storeRevenue.Store)
                        .WithRevenue(0)
                        .Build();
        }
    }
}

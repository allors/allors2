// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesChannelRevenueHistories.cs" company="Allors bvba">
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

    public partial class SalesChannelRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var derivation = new Derivation(session);

            var salesChannelRevenuesByPeriodBySalesChannelByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<SalesChannel, Dictionary<DateTime, SalesChannelRevenue>>>();

            var salesChannelRevenues = session.Extent<SalesChannelRevenue>();

            foreach (SalesChannelRevenue salesChannelRevenue in salesChannelRevenues)
            {
                var months = ((DateTime.UtcNow.Year - salesChannelRevenue.Year) * 12) + DateTime.UtcNow.Month - salesChannelRevenue.Month;
                if (months <= 12)
                {
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
            }

            var salesChannelRevenueHistoriesBySalesChannelByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<SalesChannel, SalesChannelRevenueHistory>>();

            var salesChannelRevenueHistories = session.Extent<SalesChannelRevenueHistory>();

            foreach (SalesChannelRevenueHistory salesChannelRevenueHistory in salesChannelRevenueHistories)
            {
                salesChannelRevenueHistory.Revenue = 0;

                Dictionary<SalesChannel, SalesChannelRevenueHistory> salesChannelRevenueHistoriesBySalesChannel;
                if (!salesChannelRevenueHistoriesBySalesChannelByInternalOrganisation.TryGetValue(salesChannelRevenueHistory.InternalOrganisation, out salesChannelRevenueHistoriesBySalesChannel))
                {
                    salesChannelRevenueHistoriesBySalesChannel = new Dictionary<SalesChannel, SalesChannelRevenueHistory>();
                    salesChannelRevenueHistoriesBySalesChannelByInternalOrganisation[salesChannelRevenueHistory.InternalOrganisation] = salesChannelRevenueHistoriesBySalesChannel;
                }

                SalesChannelRevenueHistory revenueHistory;
                if (!salesChannelRevenueHistoriesBySalesChannel.TryGetValue(salesChannelRevenueHistory.SalesChannel, out revenueHistory))
                {
                    salesChannelRevenueHistoriesBySalesChannel.Add(salesChannelRevenueHistory.SalesChannel, salesChannelRevenueHistory);
                }
            }

            foreach (var keyValuePair in salesChannelRevenuesByPeriodBySalesChannelByInternalOrganisation)
            {
                Dictionary<SalesChannel, SalesChannelRevenueHistory> salesChannelRevenueHistoriesBySalesChannel;
                if (!salesChannelRevenueHistoriesBySalesChannelByInternalOrganisation.TryGetValue(keyValuePair.Key, out salesChannelRevenueHistoriesBySalesChannel))
                {
                    salesChannelRevenueHistoriesBySalesChannel = new Dictionary<SalesChannel, SalesChannelRevenueHistory>();
                    salesChannelRevenueHistoriesBySalesChannelByInternalOrganisation[keyValuePair.Key] = salesChannelRevenueHistoriesBySalesChannel;
                }

                foreach (var valuePair in keyValuePair.Value)
                {
                    SalesChannelRevenueHistory salesChannelRevenueHistory;

                    if (!salesChannelRevenueHistoriesBySalesChannel.TryGetValue(valuePair.Key, out salesChannelRevenueHistory))
                    {
                        SalesChannelRevenue partyRevenue = null;
                        foreach (var salesChannelRevenuesByPeriod in valuePair.Value)
                        {
                            partyRevenue = salesChannelRevenuesByPeriod.Value;
                            break;
                        }

                        salesChannelRevenueHistory = CreateSalesChannelRevenueHistory(session, partyRevenue);
                        salesChannelRevenueHistoriesBySalesChannel.Add(salesChannelRevenueHistory.SalesChannel, salesChannelRevenueHistory);
                    }

                    foreach (var partyRevenueByPeriod in valuePair.Value)
                    {
                        var partyRevenue = partyRevenueByPeriod.Value;
                        salesChannelRevenueHistory.Revenue += partyRevenue.Revenue;
                    }

                    salesChannelRevenueHistory.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static SalesChannelRevenueHistory CreateSalesChannelRevenueHistory(ISession session, SalesChannelRevenue salesChannelRevenue)
        {
            return new SalesChannelRevenueHistoryBuilder(session)
                        .WithCurrency(salesChannelRevenue.Currency)
                        .WithInternalOrganisation(salesChannelRevenue.InternalOrganisation)
                        .WithSalesChannel(salesChannelRevenue.SalesChannel)
                        .WithRevenue(0)
                        .Build();
        }
    }
}

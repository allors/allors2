// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyProductCategoryRevenueHistories.cs" company="Allors bvba">
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


    public partial class PartyProductCategoryRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation =
                new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>>>();

            var partyProductCategoryRevenues = session.Extent<PartyProductCategoryRevenue>();

            foreach (PartyProductCategoryRevenue partyProductCategoryRevenue in partyProductCategoryRevenues)
            {
                var months = ((DateTime.UtcNow.Year - partyProductCategoryRevenue.Year) * 12) + DateTime.UtcNow.Month - partyProductCategoryRevenue.Month;
                if (months <= 12)
                {
                    var date = DateTimeFactory.CreateDate(partyProductCategoryRevenue.Year, partyProductCategoryRevenue.Month, 01);

                    Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>> partyProductCategoryRevenuesByPeriodByProductCategoryByParty;
                    if (!partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation.TryGetValue(partyProductCategoryRevenue.InternalOrganisation, out partyProductCategoryRevenuesByPeriodByProductCategoryByParty))
                    {
                        partyProductCategoryRevenuesByPeriodByProductCategoryByParty = new Dictionary<Party, Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>>();
                        partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation[partyProductCategoryRevenue.InternalOrganisation] = partyProductCategoryRevenuesByPeriodByProductCategoryByParty;
                    }

                    Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>> partyProductCategoryRevenuesByPeriodByProductCategory;
                    if (!partyProductCategoryRevenuesByPeriodByProductCategoryByParty.TryGetValue(partyProductCategoryRevenue.Party, out partyProductCategoryRevenuesByPeriodByProductCategory))
                    {
                        partyProductCategoryRevenuesByPeriodByProductCategory = new Dictionary<ProductCategory, Dictionary<DateTime, PartyProductCategoryRevenue>>();
                        partyProductCategoryRevenuesByPeriodByProductCategoryByParty[partyProductCategoryRevenue.Party] = partyProductCategoryRevenuesByPeriodByProductCategory;
                    }

                    Dictionary<DateTime, PartyProductCategoryRevenue> partyProductCategoryRevenuesByPeriod;
                    if (!partyProductCategoryRevenuesByPeriodByProductCategory.TryGetValue(partyProductCategoryRevenue.ProductCategory, out partyProductCategoryRevenuesByPeriod))
                    {
                        partyProductCategoryRevenuesByPeriod = new Dictionary<DateTime, PartyProductCategoryRevenue>();
                        partyProductCategoryRevenuesByPeriodByProductCategory[partyProductCategoryRevenue.ProductCategory] = partyProductCategoryRevenuesByPeriod;
                    }

                    PartyProductCategoryRevenue revenue;
                    if (!partyProductCategoryRevenuesByPeriod.TryGetValue(date, out revenue))
                    {
                        partyProductCategoryRevenuesByPeriod.Add(date, partyProductCategoryRevenue);
                    }
                }
            }

            var partyProductCategoryRevenueHistoriesByProductCategoryByPartyByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<ProductCategory, PartyProductCategoryRevenueHistory>>>();

            var partyProductCategoryRevenueHistories = session.Extent<PartyProductCategoryRevenueHistory>();

            foreach (PartyProductCategoryRevenueHistory partyProductCategoryRevenueHistory in partyProductCategoryRevenueHistories)
            {
                partyProductCategoryRevenueHistory.Revenue = 0;

                Dictionary<Party, Dictionary<ProductCategory, PartyProductCategoryRevenueHistory>> partyProductCategoryRevenueHistoriesByProductCategoryByParty;
                if (!partyProductCategoryRevenueHistoriesByProductCategoryByPartyByInternalOrganisation.TryGetValue(partyProductCategoryRevenueHistory.InternalOrganisation, out partyProductCategoryRevenueHistoriesByProductCategoryByParty))
                {
                    partyProductCategoryRevenueHistoriesByProductCategoryByParty = new Dictionary<Party, Dictionary<ProductCategory, PartyProductCategoryRevenueHistory>>();
                    partyProductCategoryRevenueHistoriesByProductCategoryByPartyByInternalOrganisation[partyProductCategoryRevenueHistory.InternalOrganisation] = partyProductCategoryRevenueHistoriesByProductCategoryByParty;
                }

                Dictionary<ProductCategory, PartyProductCategoryRevenueHistory> partyProductCategoryRevenueHistoriesByProductCategory;
                if (!partyProductCategoryRevenueHistoriesByProductCategoryByParty.TryGetValue(partyProductCategoryRevenueHistory.Party, out partyProductCategoryRevenueHistoriesByProductCategory))
                {
                    partyProductCategoryRevenueHistoriesByProductCategory = new Dictionary<ProductCategory, PartyProductCategoryRevenueHistory>();
                    partyProductCategoryRevenueHistoriesByProductCategoryByParty[partyProductCategoryRevenueHistory.Party] = partyProductCategoryRevenueHistoriesByProductCategory;
                }

                PartyProductCategoryRevenueHistory revenueHistory;
                if (!partyProductCategoryRevenueHistoriesByProductCategory.TryGetValue(partyProductCategoryRevenueHistory.ProductCategory, out revenueHistory))
                {
                    partyProductCategoryRevenueHistoriesByProductCategory.Add(partyProductCategoryRevenueHistory.ProductCategory, partyProductCategoryRevenueHistory);
                }
            }

            foreach (var keyValuePair in partyProductCategoryRevenuesByPeriodByProductCategoryByPartyByInternalOrganisation)
            {
                Dictionary<Party, Dictionary<ProductCategory, PartyProductCategoryRevenueHistory>> partyProductCategoryRevenueHistoriesByProductCategoryByParty;
                if (!partyProductCategoryRevenueHistoriesByProductCategoryByPartyByInternalOrganisation.TryGetValue(keyValuePair.Key, out partyProductCategoryRevenueHistoriesByProductCategoryByParty))
                {
                    partyProductCategoryRevenueHistoriesByProductCategoryByParty = new Dictionary<Party, Dictionary<ProductCategory, PartyProductCategoryRevenueHistory>>();
                    partyProductCategoryRevenueHistoriesByProductCategoryByPartyByInternalOrganisation[keyValuePair.Key] = partyProductCategoryRevenueHistoriesByProductCategoryByParty;
                }

                foreach (var partyProductCategoryRevenuesByPeriodByProductCategoryByParty in keyValuePair.Value)
                {
                    Dictionary<ProductCategory, PartyProductCategoryRevenueHistory> partyProductCategoryRevenueHistoriesByProductCategory;
                    if (!partyProductCategoryRevenueHistoriesByProductCategoryByParty.TryGetValue(partyProductCategoryRevenuesByPeriodByProductCategoryByParty.Key, out partyProductCategoryRevenueHistoriesByProductCategory))
                    {
                        partyProductCategoryRevenueHistoriesByProductCategory = new Dictionary<ProductCategory, PartyProductCategoryRevenueHistory>();
                        partyProductCategoryRevenueHistoriesByProductCategoryByParty[keyValuePair.Key] = partyProductCategoryRevenueHistoriesByProductCategory;
                    }

                    foreach (var partyProductCategoryRevenuesByPeriodByProductCategory in partyProductCategoryRevenuesByPeriodByProductCategoryByParty.Value)
                    {
                        PartyProductCategoryRevenueHistory partyProductCategoryRevenueHistory;

                        if (!partyProductCategoryRevenueHistoriesByProductCategory.TryGetValue(partyProductCategoryRevenuesByPeriodByProductCategory.Key, out partyProductCategoryRevenueHistory))
                        {
                            PartyProductCategoryRevenue partyProductCategoryRevenue = null;
                            foreach (var partyProductCategoryRevenuesByPeriod in partyProductCategoryRevenuesByPeriodByProductCategory.Value)
                            {
                                partyProductCategoryRevenue = partyProductCategoryRevenuesByPeriod.Value;
                                break;
                            }

                            partyProductCategoryRevenueHistory = CreatePartyRevenueHistory(session, partyProductCategoryRevenue);
                            partyProductCategoryRevenueHistoriesByProductCategory.Add(partyProductCategoryRevenueHistory.ProductCategory, partyProductCategoryRevenueHistory);
                        }

                        foreach (var partyProductCategoryRevenueByPeriod in partyProductCategoryRevenuesByPeriodByProductCategory.Value)
                        {
                            var partyProductCategoryRevenue = partyProductCategoryRevenueByPeriod.Value;
                            partyProductCategoryRevenueHistory.Revenue += partyProductCategoryRevenue.Revenue;
                            partyProductCategoryRevenueHistory.Quantity += partyProductCategoryRevenue.Quantity;
                        }
                    }
                }
            }
        }

        public static Dictionary<ProductCategory, PartyProductCategoryRevenueHistory> PartyProductCategoryRevenueHistoryByProductCategory(InternalOrganisation internalOrganisation, Party party)
        {
            var partyProductCategoryRevenueHistoryByProductCategory = new Dictionary<ProductCategory, PartyProductCategoryRevenueHistory>();

            var partyProductCategoryRevenueHistories = party.PartyProductCategoryRevenueHistoriesWhereParty;
            partyProductCategoryRevenueHistories.Filter.AddEquals(PartyProductCategoryRevenueHistories.Meta.InternalOrganisation, internalOrganisation);

            foreach (PartyProductCategoryRevenueHistory partyProductCategoryRevenueHistory in partyProductCategoryRevenueHistories)
            {
                partyProductCategoryRevenueHistoryByProductCategory[partyProductCategoryRevenueHistory.ProductCategory] = partyProductCategoryRevenueHistory;
            }

            return partyProductCategoryRevenueHistoryByProductCategory;
        }


        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute }; 
            
            config.GrantAdministrator(this.ObjectType, full);
        }

        private static PartyProductCategoryRevenueHistory CreatePartyRevenueHistory(ISession session, PartyProductCategoryRevenue partyProductCategoryRevenue)
        {
            return new PartyProductCategoryRevenueHistoryBuilder(session)
                        .WithCurrency(partyProductCategoryRevenue.Currency)
                        .WithInternalOrganisation(partyProductCategoryRevenue.InternalOrganisation)
                        .WithParty(partyProductCategoryRevenue.Party)
                        .WithProductCategory(partyProductCategoryRevenue.ProductCategory)
                        .WithRevenue(0)
                        .WithQuantity(0)
                        .Build();
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyProductRevenueHistories.cs" company="Allors bvba">
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


    public partial class PartyProductRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var partyProductRevenuesByPeriodByProductByPartyByInternalOrganisation =
                new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<Product, Dictionary<DateTime, PartyProductRevenue>>>>();

            var partyProductRevenues = session.Extent<PartyProductRevenue>();

            foreach (PartyProductRevenue partyProductRevenue in partyProductRevenues)
            {
                var months = ((DateTime.UtcNow.Year - partyProductRevenue.Year) * 12) + DateTime.UtcNow.Month - partyProductRevenue.Month;
                if (months <= 12)
                {
                    var date = DateTimeFactory.CreateDate(partyProductRevenue.Year, partyProductRevenue.Month, 01);

                    Dictionary<Party, Dictionary<Product, Dictionary<DateTime, PartyProductRevenue>>> partyProductRevenuesByPeriodByProductByParty;
                    if (!partyProductRevenuesByPeriodByProductByPartyByInternalOrganisation.TryGetValue(partyProductRevenue.InternalOrganisation, out partyProductRevenuesByPeriodByProductByParty))
                    {
                        partyProductRevenuesByPeriodByProductByParty = new Dictionary<Party, Dictionary<Product, Dictionary<DateTime, PartyProductRevenue>>>();
                        partyProductRevenuesByPeriodByProductByPartyByInternalOrganisation[partyProductRevenue.InternalOrganisation] = partyProductRevenuesByPeriodByProductByParty;
                    }

                    Dictionary<Product, Dictionary<DateTime, PartyProductRevenue>> partyProductRevenuesByPeriodByproduct;
                    if (!partyProductRevenuesByPeriodByProductByParty.TryGetValue(partyProductRevenue.Party, out partyProductRevenuesByPeriodByproduct))
                    {
                        partyProductRevenuesByPeriodByproduct = new Dictionary<Product, Dictionary<DateTime, PartyProductRevenue>>();
                        partyProductRevenuesByPeriodByProductByParty[partyProductRevenue.Party] = partyProductRevenuesByPeriodByproduct;
                    }

                    Dictionary<DateTime, PartyProductRevenue> partyProductRevenuesByPeriod;
                    if (!partyProductRevenuesByPeriodByproduct.TryGetValue(partyProductRevenue.Product, out partyProductRevenuesByPeriod))
                    {
                        partyProductRevenuesByPeriod = new Dictionary<DateTime, PartyProductRevenue>();
                        partyProductRevenuesByPeriodByproduct[partyProductRevenue.Product] = partyProductRevenuesByPeriod;
                    }

                    PartyProductRevenue revenue;
                    if (!partyProductRevenuesByPeriod.TryGetValue(date, out revenue))
                    {
                        partyProductRevenuesByPeriod.Add(date, partyProductRevenue);
                    }
                }
            }

            var partyProductRevenueHistoriesByProductByPartyByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<Product, PartyProductRevenueHistory>>>();

            var partyProductRevenueHistories = session.Extent<PartyProductRevenueHistory>();

            foreach (PartyProductRevenueHistory partyProductRevenueHistory in partyProductRevenueHistories)
            {
                partyProductRevenueHistory.Revenue = 0;

                Dictionary<Party, Dictionary<Product, PartyProductRevenueHistory>> partyProductRevenueHistoriesByProductByParty;
                if (!partyProductRevenueHistoriesByProductByPartyByInternalOrganisation.TryGetValue(partyProductRevenueHistory.InternalOrganisation, out partyProductRevenueHistoriesByProductByParty))
                {
                    partyProductRevenueHistoriesByProductByParty = new Dictionary<Party, Dictionary<Product, PartyProductRevenueHistory>>();
                    partyProductRevenueHistoriesByProductByPartyByInternalOrganisation[partyProductRevenueHistory.InternalOrganisation] = partyProductRevenueHistoriesByProductByParty;
                }

                Dictionary<Product, PartyProductRevenueHistory> partyProductRevenueHistoriesByProduct;
                if (!partyProductRevenueHistoriesByProductByParty.TryGetValue(partyProductRevenueHistory.Party, out partyProductRevenueHistoriesByProduct))
                {
                    partyProductRevenueHistoriesByProduct = new Dictionary<Product, PartyProductRevenueHistory>();
                    partyProductRevenueHistoriesByProductByParty[partyProductRevenueHistory.Party] = partyProductRevenueHistoriesByProduct;
                }

                PartyProductRevenueHistory revenueHistory;
                if (!partyProductRevenueHistoriesByProduct.TryGetValue(partyProductRevenueHistory.Product, out revenueHistory))
                {
                    partyProductRevenueHistoriesByProduct.Add(partyProductRevenueHistory.Product, partyProductRevenueHistory);
                }
            }

            foreach (var keyValuePair in partyProductRevenuesByPeriodByProductByPartyByInternalOrganisation)
            {
                Dictionary<Party, Dictionary<Product, PartyProductRevenueHistory>> partyProductRevenueHistoriesByProductByParty;
                if (!partyProductRevenueHistoriesByProductByPartyByInternalOrganisation.TryGetValue(keyValuePair.Key, out partyProductRevenueHistoriesByProductByParty))
                {
                    partyProductRevenueHistoriesByProductByParty = new Dictionary<Party, Dictionary<Product, PartyProductRevenueHistory>>();
                    partyProductRevenueHistoriesByProductByPartyByInternalOrganisation[keyValuePair.Key] = partyProductRevenueHistoriesByProductByParty;
                }

                foreach (var partyProductRevenuesByPeriodByProductByParty in keyValuePair.Value)
                {
                    Dictionary<Product, PartyProductRevenueHistory> partyProductRevenueHistoriesByProduct;
                    if (!partyProductRevenueHistoriesByProductByParty.TryGetValue(partyProductRevenuesByPeriodByProductByParty.Key, out partyProductRevenueHistoriesByProduct))
                    {
                        partyProductRevenueHistoriesByProduct = new Dictionary<Product, PartyProductRevenueHistory>();
                        partyProductRevenueHistoriesByProductByParty[keyValuePair.Key] = partyProductRevenueHistoriesByProduct;
                    }

                    foreach (var partyProductRevenuesByPeriodByProduct in partyProductRevenuesByPeriodByProductByParty.Value)
                    {
                        PartyProductRevenueHistory partyProductRevenueHistory;

                        if (!partyProductRevenueHistoriesByProduct.TryGetValue(partyProductRevenuesByPeriodByProduct.Key, out partyProductRevenueHistory))
                        {
                            PartyProductRevenue partyProductRevenue = null;
                            foreach (var partyProductRevenuesByPeriod in partyProductRevenuesByPeriodByProduct.Value)
                            {
                                partyProductRevenue = partyProductRevenuesByPeriod.Value;
                                break;
                            }

                            partyProductRevenueHistory = CreatePartyRevenueHistory(session, partyProductRevenue);
                            partyProductRevenueHistoriesByProduct.Add(partyProductRevenueHistory.Product, partyProductRevenueHistory);
                        }

                        foreach (var partyProductRevenueByPeriod in partyProductRevenuesByPeriodByProduct.Value)
                        {
                            var partyProductRevenue = partyProductRevenueByPeriod.Value;
                            partyProductRevenueHistory.Revenue += partyProductRevenue.Revenue;
                            partyProductRevenueHistory.Quantity += partyProductRevenue.Quantity;
                        }
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

        private static PartyProductRevenueHistory CreatePartyRevenueHistory(ISession session, PartyProductRevenue partyProductRevenue)
        {
            return new PartyProductRevenueHistoryBuilder(session)
                        .WithCurrency(partyProductRevenue.Currency)
                        .WithInternalOrganisation(partyProductRevenue.InternalOrganisation)
                        .WithParty(partyProductRevenue.Party)
                        .WithProduct(partyProductRevenue.Product)
                        .WithRevenue(0)
                        .WithQuantity(0)
                        .Build();
        }
    }
}

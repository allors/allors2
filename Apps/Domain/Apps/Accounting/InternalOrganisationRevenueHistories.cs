// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalOrganisationRevenueHistories.cs" company="Allors bvba">
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
    using System.Security.Cryptography.X509Certificates;

    public partial class InternalOrganisationRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var derivation = new Derivation(session);
            var internalOrganisationRevenuesByPeriodByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<DateTime, InternalOrganisationRevenue>>();

            var internalOrganisationRevenues = session.Extent<InternalOrganisationRevenue>();

            foreach (InternalOrganisationRevenue internalOrganisationRevenue in internalOrganisationRevenues)
            {
                var months = ((DateTime.UtcNow.Year - internalOrganisationRevenue.Year) * 12) + DateTime.UtcNow.Month - internalOrganisationRevenue.Month;
                if (months <= 12)
                {
                    var date = DateTimeFactory.CreateDate(internalOrganisationRevenue.Year, internalOrganisationRevenue.Month, 01);

                    Dictionary<DateTime, InternalOrganisationRevenue> internalOrganisationRevenuesByPeriod;
                    if (!internalOrganisationRevenuesByPeriodByInternalOrganisation.TryGetValue(internalOrganisationRevenue.InternalOrganisation, out internalOrganisationRevenuesByPeriod))
                    {
                        internalOrganisationRevenuesByPeriod = new Dictionary<DateTime, InternalOrganisationRevenue>();
                        internalOrganisationRevenuesByPeriodByInternalOrganisation[internalOrganisationRevenue.InternalOrganisation] = internalOrganisationRevenuesByPeriod;
                    }

                    InternalOrganisationRevenue revenue;
                    if (!internalOrganisationRevenuesByPeriod.TryGetValue(date, out revenue))
                    {
                        internalOrganisationRevenuesByPeriod.Add(date, internalOrganisationRevenue);
                    }
                }
            }

            var internalOrganisationRevenueHistoriesByInternalOrganisation = new Dictionary<InternalOrganisation, InternalOrganisationRevenueHistory>();

            var internalOrganisationRevenueHistories = session.Extent<InternalOrganisationRevenueHistory>();

            foreach (InternalOrganisationRevenueHistory internalOrganisationRevenueHistory in internalOrganisationRevenueHistories)
            {
                internalOrganisationRevenueHistory.Revenue = 0;

                InternalOrganisationRevenueHistory revenueHistory;
                if (!internalOrganisationRevenueHistoriesByInternalOrganisation.TryGetValue(internalOrganisationRevenueHistory.InternalOrganisation, out revenueHistory))
                {
                    internalOrganisationRevenueHistoriesByInternalOrganisation.Add(internalOrganisationRevenueHistory.InternalOrganisation, internalOrganisationRevenueHistory);
                }
            }

            foreach (var keyValuePair in internalOrganisationRevenuesByPeriodByInternalOrganisation)
            {
                InternalOrganisationRevenueHistory internalOrganisationRevenueHistory;
                if (!internalOrganisationRevenueHistoriesByInternalOrganisation.TryGetValue(keyValuePair.Key, out internalOrganisationRevenueHistory))
                {
                    InternalOrganisationRevenue internalOrganisationRevenue = null;
                    foreach (var revenuesByPeriod in keyValuePair.Value)
                    {
                        internalOrganisationRevenue = revenuesByPeriod.Value;
                        break;
                    }

                    internalOrganisationRevenueHistory = CreateInternalOrganisationRevenueHistory(session, internalOrganisationRevenue);
                    internalOrganisationRevenueHistoriesByInternalOrganisation.Add(internalOrganisationRevenueHistory.InternalOrganisation, internalOrganisationRevenueHistory);
                }

                foreach (var revenueByPeriod in keyValuePair.Value)
                {
                    var internalOrganisationRevenue = revenueByPeriod.Value;
                    internalOrganisationRevenueHistory.Revenue += internalOrganisationRevenue.Revenue;
                }

                internalOrganisationRevenueHistory.OnDerive(x => x.WithDerivation(derivation));
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute }; 
            
            config.GrantAdministrator(this.ObjectType, full);
        }

        private static InternalOrganisationRevenueHistory CreateInternalOrganisationRevenueHistory(ISession session, InternalOrganisationRevenue internalOrganisationRevenue)
        {
            return new InternalOrganisationRevenueHistoryBuilder(session)
                        .WithCurrency(internalOrganisationRevenue.Currency)
                        .WithInternalOrganisation(internalOrganisationRevenue.InternalOrganisation)
                        .WithRevenue(0)
                        .Build();
        }
    }
}

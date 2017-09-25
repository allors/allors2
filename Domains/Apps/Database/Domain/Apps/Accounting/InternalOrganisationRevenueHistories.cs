// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalOrganisationRevenueHistories.cs" company="Allors bvba">
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

    public partial class InternalOrganisationRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var derivation = new NonLogging.Derivation(session);

            var internalOrganisationRevenues = session.Extent<InternalOrganisationRevenue>();

            foreach (InternalOrganisationRevenue internalOrganisationRevenue in internalOrganisationRevenues)
            {
                var months = ((DateTime.UtcNow.Year - internalOrganisationRevenue.Year) * 12) + DateTime.UtcNow.Month - internalOrganisationRevenue.Month;
                if (months <= 12)
                {
                    var date = DateTimeFactory.CreateDate(internalOrganisationRevenue.Year, internalOrganisationRevenue.Month, 01);

                    Dictionary<DateTime, InternalOrganisationRevenue> internalOrganisationRevenuesByPeriod = new Dictionary<DateTime, InternalOrganisationRevenue>();

                    InternalOrganisationRevenue revenue;
                    if (!internalOrganisationRevenuesByPeriod.TryGetValue(date, out revenue))
                    {
                        internalOrganisationRevenuesByPeriod.Add(date, internalOrganisationRevenue);
                    }
                }
            }

            var internalOrganisationRevenueHistory = session.Extent<InternalOrganisationRevenueHistory>().First;
            if (internalOrganisationRevenueHistory == null)
            {
                internalOrganisationRevenueHistory = new InternalOrganisationRevenueHistoryBuilder(session).Build();
            }
            else
            {
                internalOrganisationRevenueHistory.Revenue = 0;
            }

            foreach (InternalOrganisationRevenue internalOrganisationRevenue in internalOrganisationRevenues)
            {
                internalOrganisationRevenueHistory.Revenue += internalOrganisationRevenue.Revenue;
            }

            internalOrganisationRevenueHistory.OnDerive(x => x.WithDerivation(derivation));
        }
    }
}

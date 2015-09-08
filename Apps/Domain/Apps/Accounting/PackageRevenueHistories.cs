// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageRevenueHistories.cs" company="Allors bvba">
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

    public partial class PackageRevenueHistories
    {
        public static void AppsOnDeriveHistory(ISession session)
        {
            var derivation = new Derivation(session);

            var packageRevenuesByPeriodByPackageByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Package, Dictionary<DateTime, PackageRevenue>>>();

            var packageRevenues = session.Extent<PackageRevenue>();

            foreach (PackageRevenue packageRevenue in packageRevenues)
            {
                var months = ((DateTime.UtcNow.Year - packageRevenue.Year) * 12) + DateTime.UtcNow.Month - packageRevenue.Month;
                if (months <= 12)
                {
                    var date = DateTimeFactory.CreateDate(packageRevenue.Year, packageRevenue.Month, 01);

                    Dictionary<Package, Dictionary<DateTime, PackageRevenue>> packageRevenuesByPeriodByPackage;
                    if (!packageRevenuesByPeriodByPackageByInternalOrganisation.TryGetValue(packageRevenue.InternalOrganisation, out packageRevenuesByPeriodByPackage))
                    {
                        packageRevenuesByPeriodByPackage = new Dictionary<Package, Dictionary<DateTime, PackageRevenue>>();
                        packageRevenuesByPeriodByPackageByInternalOrganisation[packageRevenue.InternalOrganisation] = packageRevenuesByPeriodByPackage;
                    }

                    Dictionary<DateTime, PackageRevenue> packageRevenuesByPeriod;
                    if (!packageRevenuesByPeriodByPackage.TryGetValue(packageRevenue.Package, out packageRevenuesByPeriod))
                    {
                        packageRevenuesByPeriod = new Dictionary<DateTime, PackageRevenue>();
                        packageRevenuesByPeriodByPackage[packageRevenue.Package] = packageRevenuesByPeriod;
                    }

                    PackageRevenue revenue;
                    if (!packageRevenuesByPeriod.TryGetValue(date, out revenue))
                    {
                        packageRevenuesByPeriod.Add(date, packageRevenue);
                    }
                }
            }

            var packageRevenueHistoriesByPackageByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Package, PackageRevenueHistory>>();

            var packageRevenueHistories = session.Extent<PackageRevenueHistory>();

            foreach (PackageRevenueHistory packageRevenueHistory in packageRevenueHistories)
            {
                packageRevenueHistory.Revenue = 0;

                Dictionary<Package, PackageRevenueHistory> packageRevenueHistoriesByPackage;
                if (!packageRevenueHistoriesByPackageByInternalOrganisation.TryGetValue(packageRevenueHistory.InternalOrganisation, out packageRevenueHistoriesByPackage))
                {
                    packageRevenueHistoriesByPackage = new Dictionary<Package, PackageRevenueHistory>();
                    packageRevenueHistoriesByPackageByInternalOrganisation[packageRevenueHistory.InternalOrganisation] = packageRevenueHistoriesByPackage;
                }

                PackageRevenueHistory revenueHistory;
                if (!packageRevenueHistoriesByPackage.TryGetValue(packageRevenueHistory.Package, out revenueHistory))
                {
                    packageRevenueHistoriesByPackage.Add(packageRevenueHistory.Package, packageRevenueHistory);
                }
            }

            foreach (var keyValuePair in packageRevenuesByPeriodByPackageByInternalOrganisation)
            {
                Dictionary<Package, PackageRevenueHistory> packageRevenueHistoriesByPackage;
                if (!packageRevenueHistoriesByPackageByInternalOrganisation.TryGetValue(keyValuePair.Key, out packageRevenueHistoriesByPackage))
                {
                    packageRevenueHistoriesByPackage = new Dictionary<Package, PackageRevenueHistory>();
                    packageRevenueHistoriesByPackageByInternalOrganisation[keyValuePair.Key] = packageRevenueHistoriesByPackage;
                }

                foreach (var valuePair in keyValuePair.Value)
                {
                    PackageRevenueHistory packageRevenueHistory;

                    if (!packageRevenueHistoriesByPackage.TryGetValue(valuePair.Key, out packageRevenueHistory))
                    {
                        PackageRevenue partyRevenue = null;
                        foreach (var packageRevenuesByPeriod in valuePair.Value)
                        {
                            partyRevenue = packageRevenuesByPeriod.Value;
                            break;
                        }

                        packageRevenueHistory = CreatePackageRevenueHistory(session, partyRevenue);
                        packageRevenueHistoriesByPackage.Add(packageRevenueHistory.Package, packageRevenueHistory);
                    }

                    foreach (var partyRevenueByPeriod in valuePair.Value)
                    {
                        var partyRevenue = partyRevenueByPeriod.Value;
                        packageRevenueHistory.Revenue += partyRevenue.Revenue;
                    }

                    packageRevenueHistory.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        private static PackageRevenueHistory CreatePackageRevenueHistory(ISession session, PackageRevenue packageRevenue)
        {
            return new PackageRevenueHistoryBuilder(session)
                        .WithCurrency(packageRevenue.Currency)
                        .WithInternalOrganisation(packageRevenue.InternalOrganisation)
                        .WithPackage(packageRevenue.Package)
                        .WithRevenue(0)
                        .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}

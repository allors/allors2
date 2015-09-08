// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PackageRevenues.cs" company="Allors bvba">
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


    public partial class PackageRevenues
    {
        public static PackageRevenue AppsFindOrCreateAsDependable(ISession session, PartyPackageRevenue dependant)
        {
            var packageRevenues = dependant.Package.PackageRevenuesWherePackage;
            packageRevenues.Filter.AddEquals(Meta.InternalOrganisation, dependant.InternalOrganisation);
            packageRevenues.Filter.AddEquals(Meta.Year, dependant.Year);
            packageRevenues.Filter.AddEquals(Meta.Month, dependant.Month);
            var packageRevenue = packageRevenues.First ?? new PackageRevenueBuilder(session)
                                                                .WithInternalOrganisation(dependant.InternalOrganisation)
                                                                .WithPackage(dependant.Package)
                                                                .WithYear(dependant.Year)
                                                                .WithMonth(dependant.Month)
                                                                .WithCurrency(dependant.Currency)
                                                                .WithRevenue(0M)
                                                                .Build();

            return packageRevenue;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var packageRevenuesByPeriodByPackageByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Package, Dictionary<DateTime, PackageRevenue>>>();

            var packageRevenues = session.Extent<PackageRevenue>();

            foreach (PackageRevenue packageRevenue in packageRevenues)
            {
                packageRevenue.Revenue = 0;
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

            var revenues = new HashSet<ObjectId>();

            var salesInvoices = session.Extent<SalesInvoice>();
            var year = 0;
            foreach (SalesInvoice salesInvoice in salesInvoices)
            {
                if (year != salesInvoice.InvoiceDate.Year)
                {
                    session.Commit();
                    year = salesInvoice.InvoiceDate.Year;
                }

                var date = DateTimeFactory.CreateDate(salesInvoice.InvoiceDate.Year, salesInvoice.InvoiceDate.Month, 01);

                foreach (SalesInvoiceItem salesInvoiceItem in salesInvoice.SalesInvoiceItems)
                {
                    if (salesInvoiceItem.ExistProduct && salesInvoiceItem.Product.ExistPrimaryProductCategory && salesInvoiceItem.Product.PrimaryProductCategory.ExistPackage)
                    {
                        CreatePackageRevenue(session, packageRevenuesByPeriodByPackageByInternalOrganisation, date, revenues, salesInvoiceItem, salesInvoiceItem.Product.PrimaryProductCategory.Package);
                    }
                }
            }

            foreach (PackageRevenue packageRevenue in packageRevenues)
            {
                if (!revenues.Contains(packageRevenue.Id))
                {
                    packageRevenue.Delete();
                }
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static void CreatePackageRevenue(
            ISession session,
            Dictionary<InternalOrganisation, Dictionary<Package, Dictionary<DateTime, PackageRevenue>>> packageRevenuesByPeriodByPackageByInternalOrganisation,
            DateTime date,
            HashSet<ObjectId> revenues,
            SalesInvoiceItem salesInvoiceItem,
            Package package)
        {
            PackageRevenue packageRevenue;

            Dictionary<Package, Dictionary<DateTime, PackageRevenue>> packageRevenuesByPeriodByPackage;
            if (!packageRevenuesByPeriodByPackageByInternalOrganisation.TryGetValue(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation, out packageRevenuesByPeriodByPackage))
            {
                packageRevenue = CreatePackageRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, package);

                packageRevenuesByPeriodByPackage = new Dictionary<Package, Dictionary<DateTime, PackageRevenue>>
                        {
                            {
                                package,
                                new Dictionary<DateTime, PackageRevenue>
                                    {
                                        { date, packageRevenue }
                                    }
                            }
                        };

                packageRevenuesByPeriodByPackageByInternalOrganisation[salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation] = packageRevenuesByPeriodByPackage;
            }

            Dictionary<DateTime, PackageRevenue> packageRevenuesByPeriod;
            if (!packageRevenuesByPeriodByPackage.TryGetValue(package, out packageRevenuesByPeriod))
            {
                packageRevenue = CreatePackageRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, package);

                packageRevenuesByPeriod = new Dictionary<DateTime, PackageRevenue> { { date, packageRevenue } };

                packageRevenuesByPeriodByPackage[package] = packageRevenuesByPeriod;
            }

            if (!packageRevenuesByPeriod.TryGetValue(date, out packageRevenue))
            {
                packageRevenue = CreatePackageRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, package);
                packageRevenuesByPeriod.Add(date, packageRevenue);
            }

            revenues.Add(packageRevenue.Id);
            packageRevenue.Revenue += salesInvoiceItem.TotalExVat;
        }

        private static PackageRevenue CreatePackageRevenue(ISession session, SalesInvoice invoice, Package package)
        {
            return new PackageRevenueBuilder(session)
                        .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                        .WithPackage(package)
                        .WithPackageName(package.Name)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithRevenue(0M)
                        .Build();
        }
    }
}

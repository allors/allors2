// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyPackageRevenues.cs" company="Allors bvba">
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
    using Meta;

    public partial class PartyPackageRevenues
    {
        public static PartyPackageRevenue AppsFindOrCreateAsDependable(ISession session, PartyProductCategoryRevenue dependant)
        {
            if (dependant.ProductCategory.ExistPackage)
            {
                var partyPackageRevenues = dependant.Party.PartyPackageRevenuesWhereParty;
                partyPackageRevenues.Filter.AddEquals(M.PartyPackageRevenue.Year, dependant.Year);
                partyPackageRevenues.Filter.AddEquals(M.PartyPackageRevenue.Month, dependant.Month);
                partyPackageRevenues.Filter.AddEquals(M.PartyPackageRevenue.Package, dependant.ProductCategory.Package);
                var partyPackageRevenue = partyPackageRevenues.First
                                                ?? new PartyPackageRevenueBuilder(session)
                                                        .WithParty(dependant.Party)
                                                        .WithPackage(dependant.ProductCategory.Package)
                                                        .WithYear(dependant.Year)
                                                        .WithMonth(dependant.Month)
                                                        .WithCurrency(dependant.Currency)
                                                        .Build();

                PackageRevenues.AppsFindOrCreateAsDependable(session, partyPackageRevenue);

                return partyPackageRevenue;
            }

            return null;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var partyPackageRevenuesByPeriodByPackageByParty = new Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>>();

            var partyPackageRevenues = session.Extent<PartyPackageRevenue>();

            foreach (PartyPackageRevenue partyPackageRevenue in partyPackageRevenues)
            {
                partyPackageRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(partyPackageRevenue.Year, partyPackageRevenue.Month, 01);

                Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>> partyPackageRevenuesByPeriodByPackage;
                if (!partyPackageRevenuesByPeriodByPackageByParty.TryGetValue(partyPackageRevenue.Party, out partyPackageRevenuesByPeriodByPackage))
                {
                    partyPackageRevenuesByPeriodByPackage = new Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>();
                    partyPackageRevenuesByPeriodByPackageByParty[partyPackageRevenue.Party] = partyPackageRevenuesByPeriodByPackage;
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

            var revenues = new HashSet<long>();

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
                        CreatePackageRevenue(session, partyPackageRevenuesByPeriodByPackageByParty, date, revenues, salesInvoiceItem, salesInvoiceItem.Product.PrimaryProductCategory.Package);
                    }
                }
            }

            foreach (PartyPackageRevenue partyPackageRevenue in partyPackageRevenues)
            {
                if (!revenues.Contains(partyPackageRevenue.Id))
                {
                    partyPackageRevenue.Delete();
                }
            }
        }

        private static void CreatePackageRevenue(
            ISession session,
            Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>> partyPackageRevenuesByPeriodByPackageByParty,
            DateTime date,
            HashSet<long> revenues,
            SalesInvoiceItem salesInvoiceItem,
            Package package)
        {
            PartyPackageRevenue partyPackageRevenue;

            Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>> partyPackageRevenuesByPeriodByPackage;
            if (!partyPackageRevenuesByPeriodByPackageByParty.TryGetValue(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer, out partyPackageRevenuesByPeriodByPackage))
            {
                partyPackageRevenue = CreatePartyPackageRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, package);

                partyPackageRevenuesByPeriodByPackage = new Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>
                        {
                            {
                                package,
                                new Dictionary<DateTime, PartyPackageRevenue> { { date, partyPackageRevenue } }
                            }
                        };

                partyPackageRevenuesByPeriodByPackageByParty[salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer] = partyPackageRevenuesByPeriodByPackage;
            }

            Dictionary<DateTime, PartyPackageRevenue> partyPackageRevenuesByPeriod;
            if (!partyPackageRevenuesByPeriodByPackage.TryGetValue(package, out partyPackageRevenuesByPeriod))
            {
                partyPackageRevenue = CreatePartyPackageRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, package);

                partyPackageRevenuesByPeriod = new Dictionary<DateTime, PartyPackageRevenue> { { date, partyPackageRevenue } };

                partyPackageRevenuesByPeriodByPackage[package] = partyPackageRevenuesByPeriod;
            }

            if (!partyPackageRevenuesByPeriod.TryGetValue(date, out partyPackageRevenue))
            {
                partyPackageRevenue = CreatePartyPackageRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, package);
                partyPackageRevenuesByPeriod.Add(date, partyPackageRevenue);
            }

            revenues.Add(partyPackageRevenue.Id);
            partyPackageRevenue.Revenue += salesInvoiceItem.TotalExVat;
        }

        private static PartyPackageRevenue CreatePartyPackageRevenue(ISession session, SalesInvoice invoice, Package package)
        {
            return new PartyPackageRevenueBuilder(session)
                        .WithParty(invoice.BillToCustomer)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.Currency)
                        .WithPackage(package)
                        .Build();
        }
    }
}

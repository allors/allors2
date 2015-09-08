// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyPackageRevenues.cs" company="Allors bvba">
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


    public partial class PartyPackageRevenues
    {
        public static PartyPackageRevenue AppsFindOrCreateAsDependable(ISession session, PartyProductCategoryRevenue dependant)
        {
            if (dependant.ProductCategory.ExistPackage)
            {
                var partyPackageRevenues = dependant.Party.PartyPackageRevenuesWhereParty;
                partyPackageRevenues.Filter.AddEquals(PartyPackageRevenues.Meta.InternalOrganisation, dependant.InternalOrganisation);
                partyPackageRevenues.Filter.AddEquals(PartyPackageRevenues.Meta.Year, dependant.Year);
                partyPackageRevenues.Filter.AddEquals(PartyPackageRevenues.Meta.Month, dependant.Month);
                partyPackageRevenues.Filter.AddEquals(PartyPackageRevenues.Meta.Package, dependant.ProductCategory.Package);
                var partyPackageRevenue = partyPackageRevenues.First
                                                ?? new PartyPackageRevenueBuilder(session)
                                                        .WithInternalOrganisation(dependant.InternalOrganisation)
                                                        .WithParty(dependant.Party)
                                                        .WithPackage(dependant.ProductCategory.Package)
                                                        .WithYear(dependant.Year)
                                                        .WithMonth(dependant.Month)
                                                        .WithCurrency(dependant.Currency)
                                                        .WithRevenue(0M)
                                                        .Build();

                PackageRevenues.AppsFindOrCreateAsDependable(session, partyPackageRevenue);

                return partyPackageRevenue;
            }

            return null;
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            var partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation = new Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>>>();

            var partyPackageRevenues = session.Extent<PartyPackageRevenue>();

            foreach (PartyPackageRevenue partyPackageRevenue in partyPackageRevenues)
            {
                partyPackageRevenue.Revenue = 0;
                var date = DateTimeFactory.CreateDate(partyPackageRevenue.Year, partyPackageRevenue.Month, 01);

                Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>> partyPackageRevenuesByPeriodByPackageByParty;
                if (!partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation.TryGetValue(partyPackageRevenue.InternalOrganisation, out partyPackageRevenuesByPeriodByPackageByParty))
                {
                    partyPackageRevenuesByPeriodByPackageByParty = new Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>>();
                    partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation[partyPackageRevenue.InternalOrganisation] = partyPackageRevenuesByPeriodByPackageByParty;
                }

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
                        CreatePackageRevenue(session, partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation, date, revenues, salesInvoiceItem, salesInvoiceItem.Product.PrimaryProductCategory.Package);
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

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static void CreatePackageRevenue(
            ISession session,
            Dictionary<InternalOrganisation, Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>>> partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation,
            DateTime date,
            HashSet<ObjectId> revenues,
            SalesInvoiceItem salesInvoiceItem,
            Package package)
        {
            PartyPackageRevenue partyPackageRevenue;

            Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>> partyPackageRevenuesByPeriodByPackageByParty;
            if (!partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation.TryGetValue(salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation, out partyPackageRevenuesByPeriodByPackageByParty))
            {
                partyPackageRevenue = CreatePartyPackageRevenue(session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem, package);

                partyPackageRevenuesByPeriodByPackageByParty = new Dictionary<Party, Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>>
                        {
                            {
                                salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer,
                                new Dictionary<Package, Dictionary<DateTime, PartyPackageRevenue>>
                                    {
                                        {
                                            package,
                                            new Dictionary<DateTime, PartyPackageRevenue> { { date, partyPackageRevenue } }
                                        }
                                    }
                                }
                        };

                partyPackageRevenuesByPeriodByPackageByPartyByInternalOrganisation[salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation] = partyPackageRevenuesByPeriodByPackageByParty;
            }

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
                        .WithInternalOrganisation(invoice.BilledFromInternalOrganisation)
                        .WithParty(invoice.BillToCustomer)
                        .WithYear(invoice.InvoiceDate.Year)
                        .WithMonth(invoice.InvoiceDate.Month)
                        .WithCurrency(invoice.BilledFromInternalOrganisation.PreferredCurrency)
                        .WithPackage(package)
                        .WithRevenue(0M)
                        .Build();
        }
    }
}

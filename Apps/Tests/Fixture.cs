// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fixture.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Xml;

    using Allors.Domain;

    public class Fixture
    {
        private static string basicXml;
        private static string fullXml;

        public static string BasicXml
        {
            get
            {
                if (basicXml == null)
                {
                    SetupBasic();
                }

                return basicXml;
            }
        }

        public static string FullXml
        {
            get
            {
                if (fullXml == null)
                {
                    SetupFull();
                }

                return fullXml;
            }
        }

        private static void SetupBasic()
        {
            var configuration = new Adapters.Memory.IntegerId.Configuration { ObjectFactory = Config.ObjectFactory };
            Config.Default = new Adapters.Memory.IntegerId.Database(configuration);
            
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-GB");

            var database = Config.Default;
            database.Init();

            using (var session = database.CreateSession())
            {
                new Setup(session, null).Apply();
                new Security(session).Apply();

                session.Derive(true);
                session.Commit();

                using (var stringWriter = new StringWriter())
                {
                    using (var writer = new XmlTextWriter(stringWriter))
                    {
                        database.Save(writer);
                        basicXml = stringWriter.ToString();
                    }
                }
            }
        }

        private static void SetupFull()
        {
            var configuration = new Adapters.Memory.IntegerId.Configuration { ObjectFactory = Config.ObjectFactory };
            Config.Default = new Adapters.Memory.IntegerId.Database(configuration);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-GB");

            var database = Config.Default;
            database.Init();

            using (var session = database.CreateSession())
            {
                new Setup(session, null).Apply();
                new Security(session).Apply();

                session.Derive(true);
                session.Commit();

                using (var stringWriter = new StringWriter())
                {
                    using (var writer = new XmlTextWriter(stringWriter))
                    {
                        database.Save(writer);
                        basicXml = stringWriter.ToString();
                    }
                }

                var singleton = Singleton.Instance(session);
                singleton.Guest = new PersonBuilder(session).WithUserName("guest").WithLastName("guest").Build();

                var administrator = new PersonBuilder(session).WithUserName("administrator").WithLastName("Administrator").Build();

                var belgium = new Countries(session).CountryByIsoCode["BE"];
                var euro = belgium.Currency;

                var bank = new BankBuilder(session).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

                var ownBankAccount = new OwnBankAccountBuilder(session)
                    .WithBankAccount(new BankAccountBuilder(session).WithBank(bank)
                                        .WithCurrency(euro)
                                        .WithIban("BE68539007547034")
                                        .WithNameOnAccount("Koen")
                                        .Build())
                    .WithDescription("Main bank account")
                    .Build();

                var postalBoundary = new PostalBoundaryBuilder(session).WithLocality("Mechelen").WithCountry(belgium).Build();
                var postalAddress = new PostalAddressBuilder(session).WithAddress1("Kleine Nieuwedijkstraat 2").WithPostalBoundary(postalBoundary).Build();

                var billingAddress =
                    new PartyContactMechanismBuilder(session).WithContactMechanism(postalAddress).WithContactPurpose(
                        new ContactMechanismPurposes(session).BillingAddress).WithUseAsDefault(true).Build();

                var shippingAddress =
                    new PartyContactMechanismBuilder(session).WithContactMechanism(postalAddress).WithContactPurpose(
                        new ContactMechanismPurposes(session).ShippingAddress).WithUseAsDefault(true).Build();

                var internalOrganisation = new InternalOrganisationBuilder(session)
                    .WithLocale(new Locales(session).EnglishGreatBritain)
                    .WithName("internalOrganisation")
                    .WithPreferredCurrency(euro)
                    .WithIncomingShipmentNumberPrefix("incoming shipmentno: ")
                    .WithPurchaseInvoiceNumberPrefix("incoming invoiceno: ")
                    .WithPurchaseOrderNumberPrefix("purchase orderno: ")
                    .WithPartyContactMechanism(billingAddress)
                    .WithPartyContactMechanism(shippingAddress)
                    .WithEmployeeRole(new Roles(session).Administrator)
                    .WithEmployeeRole(new Roles(session).Procurement)
                    .WithEmployeeRole(new Roles(session).Sales)
                    .WithEmployeeRole(new Roles(session).Operations)
                    .WithEmployeeRole(new Roles(session).Administrator)
                    .WithPurchaseOrderTemplate(new StringTemplates(session).FindBy(UniquelyIdentifiables.Meta.UniqueId, PurchaseOrders.PurchaseOrderTemplateEnId))
                    .WithDefaultPaymentMethod(ownBankAccount)
                    .Build();

                Singleton.Instance(session).DefaultInternalOrganisation = internalOrganisation;

                var facility = new WarehouseBuilder(session).WithName("facility").WithOwner(internalOrganisation).Build();
                internalOrganisation.DefaultFacility = facility;

                new StoreBuilder(session)
                    .WithName("store")
                    .WithDefaultFacility(facility)
                    .WithOwner(internalOrganisation)
                    .WithOutgoingShipmentNumberPrefix("shipmentno: ")
                    .WithSalesInvoiceNumberPrefix("invoiceno: ")
                    .WithSalesOrderNumberPrefix("orderno: ")
                    .WithDefaultShipmentMethod(new ShipmentMethods(session).Ground)
                    .WithDefaultCarrier(new Carriers(session).Fedex)
                    .WithSalesInvoiceTemplate(new StringTemplates(session).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesInvoices.SalesInvoiceTemplateEnId))
                    .WithSalesInvoiceTemplate(new StringTemplates(session).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesInvoices.SalesInvoiceTemplateNlId))
                    .WithSalesOrderTemplate(new StringTemplates(session).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesOrders.SalesOrderTemplateEnId))
                    .WithSalesOrderTemplate(new StringTemplates(session).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesOrders.SalesOrderTemplateNlId))
                    .WithCustomerShipmentTemplate(new StringTemplates(session).FindBy(UniquelyIdentifiables.Meta.UniqueId, CustomerShipments.CustomerShipmentTemplateEnId))
                    .WithCustomerShipmentTemplate(new StringTemplates(session).FindBy(UniquelyIdentifiables.Meta.UniqueId, CustomerShipments.CustomerShipmentTemplateNlId))
                    .WithCreditLimit(500)
                    .WithPaymentGracePeriod(10)
                    .Build();

                var customer = new OrganisationBuilder(session).WithName("customer").WithLocale(singleton.DefaultLocale).Build();
                var supplier = new OrganisationBuilder(session).WithName("supplier").WithLocale(singleton.DefaultLocale).Build();
                var purchaser = new PersonBuilder(session).WithLastName("purchaser").WithUserName("purchaser").Build();
                var salesrep = new PersonBuilder(session).WithLastName("salesRep").WithUserName("salesRep").Build();
                var orderProcessor =
                    new PersonBuilder(session).WithLastName("orderProcessor").WithUserName("orderProcessor").Build();

                new CustomerRelationshipBuilder(session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).WithFromDate(DateTime.UtcNow).Build();

                new SupplierRelationshipBuilder(session).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).WithFromDate(DateTime.UtcNow).Build();

                new EmploymentBuilder(session).WithFromDate(DateTime.UtcNow).WithEmployee(purchaser).WithEmployer(internalOrganisation).Build();

                new EmploymentBuilder(session).WithFromDate(DateTime.UtcNow).WithEmployee(salesrep).WithEmployer(internalOrganisation).Build();

                new EmploymentBuilder(session).WithFromDate(DateTime.UtcNow).WithEmployee(orderProcessor).WithEmployer(internalOrganisation).Build();

                new SalesRepRelationshipBuilder(session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithSalesRepresentative(salesrep).Build();

                session.Derive(true);

                var administrators = new UserGroups(session).Administrators;
                administrators.AddMember(administrator);

                var usergroups = internalOrganisation.UserGroupsWhereParty;
                usergroups = internalOrganisation.UserGroupsWhereParty;
                usergroups.Filter.AddEquals(UserGroups.Meta.Parent, new Roles(session).Operations.UserGroupWhereRole);
                var userGroup = usergroups.First;

                userGroup.AddMember(orderProcessor);

                usergroups = internalOrganisation.UserGroupsWhereParty;
                usergroups.Filter.AddEquals(UserGroups.Meta.Parent, new Roles(session).Procurement.UserGroupWhereRole);
                userGroup = usergroups.First;

                userGroup.AddMember(purchaser);

                session.Derive(true);
                session.Commit();

                using (var stringWriter = new StringWriter())
                {
                    using (var writer = new XmlTextWriter(stringWriter))
                    {
                        database.Save(writer);
                        fullXml = stringWriter.ToString();
                    }
                }
            }
        }
    }
}
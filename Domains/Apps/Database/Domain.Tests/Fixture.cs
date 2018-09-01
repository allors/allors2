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
    using Domain;
    using Meta;

    public class Fixture
    {
        public static void Setup(IDatabase database)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            CultureInfo.CurrentUICulture = new CultureInfo("en-GB");

            using (var session = database.CreateSession())
            {
                new Setup(session, null).Apply();

                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);

                var singleton = session.GetSingleton();

                new UserGroups(session).Administrators.AddMember(administrator);

                singleton.Guest = new PersonBuilder(session).WithUserName("guest").WithLastName("guest").Build();

                session.Derive();
                session.Commit();

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

                var internalOrganisation = new OrganisationBuilder(session)
                    .WithIsInternalOrganisation(true)
                    .WithDoAccounting(false)
                    .WithName("internalOrganisation")
                    .WithPreferredCurrency(new Currencies(session).CurrencyByCode["EUR"])
                    .WithIncomingShipmentNumberPrefix("incoming shipmentno: ")
                    .WithPurchaseInvoiceNumberPrefix("incoming invoiceno: ")
                    .WithPurchaseOrderNumberPrefix("purchase orderno: ")
                    .WithDefaultCollectionMethod(ownBankAccount)
                    .WithSubAccountCounter(new CounterBuilder(session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build())
                    .Build();

                internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(postalAddress)
                    .WithContactPurpose(new ContactMechanismPurposes(session).GeneralCorrespondence)
                    .WithContactPurpose(new ContactMechanismPurposes(session).BillingAddress)
                    .Build());

                var facility = new FacilityBuilder(session)
                    .WithFacilityType(new FacilityTypes(session).Warehouse)
                    .WithName("facility")
                    .WithOwner(internalOrganisation)
                    .Build();

                internalOrganisation.DefaultFacility = facility;                

                var collectionMethod = new PaymentMethods(session).Extent().First;

                new StoreBuilder(session)
                    .WithName("store")
                    .WithBillingProcess(new BillingProcesses(session).BillingForShipmentItems)
                    .WithInternalOrganisation(internalOrganisation)
                    .WithOutgoingShipmentNumberPrefix("shipmentno: ")
                    .WithSalesInvoiceNumberPrefix("invoiceno: ")
                    .WithSalesOrderNumberPrefix("orderno: ")
                    .WithDefaultShipmentMethod(new ShipmentMethods(session).Ground)
                    .WithDefaultCarrier(new Carriers(session).Fedex)
                    .WithCreditLimit(500)
                    .WithPaymentGracePeriod(10)
                    .WithDefaultCollectionMethod(collectionMethod)
                    .WithIsImmediatelyPicked(false)
                    .Build();

                new ProductCategoryBuilder(session).WithName("Primary Category").Build();

                var customer = new OrganisationBuilder(session).WithName("customer").WithLocale(singleton.DefaultLocale).Build();
                var supplier = new OrganisationBuilder(session).WithName("supplier").WithLocale(singleton.DefaultLocale).Build();
                var purchaser = new PersonBuilder(session).WithLastName("purchaser").WithUserName("purchaser").Build();
                var salesrep = new PersonBuilder(session).WithLastName("salesRep").WithUserName("salesRep").Build();
                var orderProcessor = new PersonBuilder(session).WithLastName("orderProcessor").WithUserName("orderProcessor").Build();

                new CustomerRelationshipBuilder(session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).WithFromDate(DateTime.UtcNow).Build();

                new SupplierRelationshipBuilder(session).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).WithFromDate(DateTime.UtcNow).Build();

                new EmploymentBuilder(session).WithFromDate(DateTime.UtcNow).WithEmployee(purchaser).WithEmployer(internalOrganisation).Build();

                new EmploymentBuilder(session).WithFromDate(DateTime.UtcNow).WithEmployee(salesrep).WithEmployer(internalOrganisation).Build();

                new EmploymentBuilder(session).WithFromDate(DateTime.UtcNow).WithEmployee(orderProcessor).WithEmployer(internalOrganisation).Build();

                new SalesRepRelationshipBuilder(session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithSalesRepresentative(salesrep).Build();

                session.Derive();
                session.Commit();
            }
        }
    }
}
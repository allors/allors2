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

                var postalAddress = new PostalAddressBuilder(session)
                    .WithAddress1("Kleine Nieuwedijkstraat 2")
                    .WithLocality("Mechelen")
                    .WithCountry(belgium)
                    .Build();

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
                    .WithContactPurpose(new ContactMechanismPurposes(session).ShippingAddress)
                    .Build());

                var facility = new FacilityBuilder(session)
                    .WithFacilityType(new FacilityTypes(session).Warehouse)
                    .WithName("facility")
                    .WithOwner(internalOrganisation)
                    .Build();

                singleton.Settings.DefaultFacility = facility;

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

                new CustomerRelationshipBuilder(session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).WithFromDate(session.Now()).Build();

                new SupplierRelationshipBuilder(session).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).WithFromDate(session.Now()).Build();

                new EmploymentBuilder(session).WithFromDate(session.Now()).WithEmployee(purchaser).WithEmployer(internalOrganisation).Build();

                new EmploymentBuilder(session).WithFromDate(session.Now()).WithEmployee(salesrep).WithEmployer(internalOrganisation).Build();

                new EmploymentBuilder(session).WithFromDate(session.Now()).WithEmployee(orderProcessor).WithEmployer(internalOrganisation).Build();

                new SalesRepRelationshipBuilder(session).WithFromDate(session.Now()).WithCustomer(customer).WithSalesRepresentative(salesrep).Build();

                var vatRate21 = new VatRateBuilder(session).WithRate(21).Build();

                var good1 = new NonUnifiedGoodBuilder(session)
                    .WithProductIdentification(new ProductNumberBuilder(session)
                        .WithIdentification("1")
                        .WithProductIdentificationType(new ProductIdentificationTypes(session).Good).Build())
                    .WithVatRate(vatRate21)
                    .WithName("good1")
                    .WithUnitOfMeasure(new UnitsOfMeasure(session).Piece)
                    .WithPart(new NonUnifiedPartBuilder(session)
                        .WithProductIdentification(new PartNumberBuilder(session)
                            .WithIdentification("1")
                            .WithProductIdentificationType(new ProductIdentificationTypes(session).Part).Build())
                        .WithInventoryItemKind(new InventoryItemKinds(session).NonSerialised).Build())
                    .Build();

                var good2 = new NonUnifiedGoodBuilder(session)
                    .WithProductIdentification(new ProductNumberBuilder(session)
                        .WithIdentification("2")
                        .WithProductIdentificationType(new ProductIdentificationTypes(session).Good).Build())
                    .WithVatRate(vatRate21)
                    .WithName("good2")
                    .WithUnitOfMeasure(new UnitsOfMeasure(session).Piece)
                    .WithPart(new NonUnifiedPartBuilder(session)
                        .WithProductIdentification(new PartNumberBuilder(session)
                            .WithIdentification("2")
                            .WithProductIdentificationType(new ProductIdentificationTypes(session).Part).Build())
                        .WithInventoryItemKind(new InventoryItemKinds(session).NonSerialised).Build())
                    .Build();

                var good3 = new NonUnifiedGoodBuilder(session)
                    .WithProductIdentification(new ProductNumberBuilder(session)
                        .WithIdentification("3")
                        .WithProductIdentificationType(new ProductIdentificationTypes(session).Good).Build())
                    .WithVatRate(vatRate21)
                    .WithName("good3")
                    .WithUnitOfMeasure(new UnitsOfMeasure(session).Piece)
                    .WithPart(new NonUnifiedPartBuilder(session)
                        .WithProductIdentification(new PartNumberBuilder(session)
                            .WithIdentification("3")
                            .WithProductIdentificationType(new ProductIdentificationTypes(session).Part).Build())
                        .WithInventoryItemKind(new InventoryItemKinds(session).NonSerialised).Build())
                    .Build();

                var catMain = new ProductCategoryBuilder(session).WithName("main cat").Build();

                var cat1 = new ProductCategoryBuilder(session)
                    .WithName("cat for good1")
                    .WithPrimaryParent(catMain)
                    .WithProduct(good1)
                    .Build();

                var cat2 = new ProductCategoryBuilder(session)
                    .WithName("cat for good2")
                    .WithPrimaryParent(catMain)
                    .WithProduct(good2)
                    .WithProduct(good3)
                    .Build();

                session.Derive();
                session.Commit();
            }
        }
    }
}
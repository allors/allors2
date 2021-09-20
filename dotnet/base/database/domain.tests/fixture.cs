// <copyright file="Fixture.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.Globalization;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;

    public class Fixture
    {
        public static void Setup(IDatabase database, Config config)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            CultureInfo.CurrentUICulture = new CultureInfo("en-GB");

            using (var session = database.CreateSession())
            {
                new Setup(session, config).Apply();

                var administrator = new PersonBuilder(session).WithUserName("administrator").Build();

                new UserGroups(session).Administrators.AddMember(administrator);

                session.SetUser(administrator);

                session.Derive();
                session.Commit();

                var singleton = session.GetSingleton();

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
                    .WithLocale(new Locales(session).EnglishGreatBritain)
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
                    .WithAutoGenerateShipmentPackage(false)
                    .WithIsImmediatelyPacked(true)
                    .Build();

                new ProductCategoryBuilder(session).WithName("Primary Category").Build();

                internalOrganisation.CreateB2BCustomer(session.Faker());
                internalOrganisation.CreateB2CCustomer(session.Faker());
                internalOrganisation.CreateSupplier(session.Faker());
                internalOrganisation.CreateSubContractor(session.Faker());

                var purchaser = new PersonBuilder(session).WithFirstName("The").WithLastName("purchaser").WithUserName("purchaser").Build();
                var orderProcessor = new PersonBuilder(session).WithFirstName("The").WithLastName("orderProcessor").WithUserName("orderProcessor").Build();

                // Adding newly created persons to EmployeeUserGroup as employees do not have any permission when created
                var employeesUserGroup = new UserGroups(session).Employees;
                employeesUserGroup.AddMember(purchaser);
                employeesUserGroup.AddMember(orderProcessor);
                employeesUserGroup.AddMember(administrator);

                new UserGroups(session).Creators.AddMember(purchaser);
                new UserGroups(session).Creators.AddMember(orderProcessor);
                new UserGroups(session).Creators.AddMember(administrator);

                new EmploymentBuilder(session).WithFromDate(session.Now()).WithEmployee(purchaser).WithEmployer(internalOrganisation).Build();

                new EmploymentBuilder(session).WithFromDate(session.Now()).WithEmployee(orderProcessor).WithEmployer(internalOrganisation).Build();

                var good1 = new NonUnifiedGoodBuilder(session)
                    .WithProductIdentification(new ProductNumberBuilder(session)
                        .WithIdentification("1")
                        .WithProductIdentificationType(new ProductIdentificationTypes(session).Good).Build())
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

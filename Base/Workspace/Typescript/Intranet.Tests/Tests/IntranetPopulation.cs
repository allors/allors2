// <copyright file="IntranetPopulation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;

    public class IntranetPopulation
    {
        private readonly ISession Session;

        private readonly DirectoryInfo DataPath;

        public IntranetPopulation(ISession session, DirectoryInfo dataPath)
        {
            this.Session = session;
            this.DataPath = dataPath;
        }

        public void Execute()
        {
            var singleton = this.Session.GetSingleton();
            var dutchLocale = new Locales(this.Session).DutchNetherlands;
            singleton.AddAdditionalLocale(dutchLocale);

            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            var be = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");
            var us = new Countries(this.Session).FindBy(M.Country.IsoCode, "US");

            var email2 = new EmailAddressBuilder(this.Session)
                .WithElectronicAddressString("recipient@acme.com")
                .Build();

            var allorsLogo = this.DataPath + @"\www\admin\images\logo.png";

            var allors = Organisations.CreateInternalOrganisation(
                session: this.Session,
                name: "Allors BVBA",
                address: "Kleine Nieuwedijkstraat 4",
                postalCode: "2800",
                locality: "Mechelen",
                country: be,
                phone1CountryCode: "+32",
                phone1: "2 335 2335",
                phone1Purpose: new ContactMechanismPurposes(this.Session).GeneralPhoneNumber,
                phone2CountryCode: string.Empty,
                phone2: string.Empty,
                phone2Purpose: null,
                emailAddress: "email@allors.com",
                websiteAddress: "www.allors.com",
                taxNumber: "BE 0476967014",
                bankName: "ING",
                facilityName: "Allors warehouse 1",
                bic: "BBRUBEBB",
                iban: "BE89 3200 1467 7685",
                currency: euro,
                logo: allorsLogo,
                storeName: "Allors Store",
                billingProcess: new BillingProcesses(this.Session).BillingForOrderItems,
                outgoingShipmentNumberPrefix: "a-CS",
                salesInvoiceNumberPrefix: "a-SI",
                salesOrderNumberPrefix: "a-SO",
                purchaseOrderNumberPrefix: "a-PO",
                purchaseInvoiceNumberPrefix: "a-PI",
                requestNumberPrefix: "a-RFQ",
                quoteNumberPrefix: "a-Q",
                productNumberPrefix: "A-",
                workEffortPrefix: "a-WO-",
                creditNoteNumberPrefix: "a-CN-",
                isImmediatelyPicked: true,
                autoGenerateShipmentPackage: true,
                isImmediatelyPacked: true,
                isAutomaticallyShipped: true,
                autoGenerateCustomerShipment: true,
                isAutomaticallyReceived: false,
                autoGeneratePurchaseShipment: false,
                useCreditNoteSequence: true,
                requestCounterValue: 1,
                quoteCounterValue: 1,
                orderCounterValue: 1,
                purchaseOrderCounterValue: 1,
                purchaseInvoiceCounterValue: 1,
                invoiceCounterValue: 1,
                purchaseOrderNeedsApproval: true,
                purchaseOrderApprovalThresholdLevel1: 1000M,
                purchaseOrderApprovalThresholdLevel2: 5000M);

            var dipu = Organisations.CreateInternalOrganisation(
                session: this.Session,
                name: "Dipu BVBA",
                address: "Kleine Nieuwedijkstraat 2",
                postalCode: "2800",
                locality: "Mechelen",
                country: be,
                phone1CountryCode: "+32",
                phone1: "2 15 49 49 49",
                phone1Purpose: new ContactMechanismPurposes(this.Session).GeneralPhoneNumber,
                phone2CountryCode: string.Empty,
                phone2: string.Empty,
                phone2Purpose: null,
                emailAddress: "email@dipu.com",
                websiteAddress: "www.dipu.com",
                taxNumber: "BE 0445366489",
                bankName: "ING",
                facilityName: "Dipu Facility",
                bic: "BBRUBEBB",
                iban: "BE23 3300 6167 6391",
                currency: euro,
                logo: allorsLogo,
                storeName: "Dipu Store",
                billingProcess: new BillingProcesses(this.Session).BillingForOrderItems,
                outgoingShipmentNumberPrefix: "d-CS",
                salesInvoiceNumberPrefix: "d-SI",
                salesOrderNumberPrefix: "d-SO",
                purchaseOrderNumberPrefix: "d-PO",
                purchaseInvoiceNumberPrefix: "d-PI",
                requestNumberPrefix: "d-RFQ",
                quoteNumberPrefix: "d-Q",
                productNumberPrefix: "D-",
                workEffortPrefix: "d-WO-",
                creditNoteNumberPrefix: "d-CN-",
                isImmediatelyPicked: true,
                autoGenerateShipmentPackage: true,
                isImmediatelyPacked: true,
                isAutomaticallyShipped: true,
                autoGenerateCustomerShipment: true,
                isAutomaticallyReceived: false,
                autoGeneratePurchaseShipment: false,
                useCreditNoteSequence: true,
                requestCounterValue: 1,
                quoteCounterValue: 1,
                orderCounterValue: 1,
                purchaseOrderCounterValue: 1,
                purchaseInvoiceCounterValue: 1,
                invoiceCounterValue: 1,
                purchaseOrderNeedsApproval: false,
                purchaseOrderApprovalThresholdLevel1: null,
                purchaseOrderApprovalThresholdLevel2: null);

            singleton.Settings.DefaultFacility = allors.FacilitiesWhereOwner.First;
            var faker = this.Session.Faker();

            allors.CreateEmployee("letmein", faker);
            allors.CreateEmployee("letmein", faker);
            allors.CreateAdministrator("letmein", faker);
            allors.CreateAdministrator("letmein", faker);

            dipu.CreateEmployee("letmein", faker);
            dipu.CreateEmployee("letmein", faker);
            dipu.CreateAdministrator("letmein", faker);

            this.Session.Derive();

            var facility = new FacilityBuilder(this.Session)
                .WithName("Allors warehouse 2")
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithOwner(allors)
                .Build();

            var store = new StoreBuilder(this.Session).WithName("store")
                .WithInternalOrganisation(allors)
                .WithDefaultFacility(facility)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .Build();

            var productType = new ProductTypeBuilder(this.Session)
                .WithName($"Gizmo Serialised")
                .WithSerialisedItemCharacteristicType(new SerialisedItemCharacteristicTypeBuilder(this.Session)
                                            .WithName("Size")
                                            .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Afmeting").WithLocale(dutchLocale).Build())
                                            .Build())
                .WithSerialisedItemCharacteristicType(new SerialisedItemCharacteristicTypeBuilder(this.Session)
                                            .WithName("Weight")
                                            .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gewicht").WithLocale(dutchLocale).Build())
                                            .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Kilogram)
                                            .Build())
                .Build();

            var vatRate = new VatRateBuilder(this.Session).WithRate(21).Build();

            var brand = new BrandBuilder(this.Session).WithDefaults().Build();

            var good1 = new NonUnifiedGoodBuilder(this.Session).WithNonSerialisedPartDefaults(allors).Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(good1.Part)
                .WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).Unknown)
                .Build();

            var good2 = new NonUnifiedGoodBuilder(this.Session)
                .WithSerialisedPartDefaults(allors)
                .Build();

            var serialisedItem1 = new SerialisedItemBuilder(this.Session).WithDefaults(allors).Build();

            good2.Part.AddSerialisedItem(serialisedItem1);

            new SerialisedInventoryItemBuilder(this.Session)
                .WithPart(good2.Part)
                .WithSerialisedItem(serialisedItem1)
                .WithFacility(allors.StoresWhereInternalOrganisation.First.DefaultFacility)
                .Build();

            var good3 = new NonUnifiedGoodBuilder(this.Session)
                .WithNonSerialisedPartDefaults(allors)
                .Build();

            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("Best selling gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Meest verkochte gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("Big Gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Grote Gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory3 = new ProductCategoryBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("Small gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kleine gizmo's").WithLocale(dutchLocale).Build())
                .WithProduct(good1)
                .WithProduct(good2)
                .WithProduct(good3)
                .Build();

            new CatalogueBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("New gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Nieuwe gizmo's").WithLocale(dutchLocale).Build())
                .WithDescription("Latest in the world of Gizmo's")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Laatste in de wereld van Gizmo's").WithLocale(dutchLocale).Build())
                .WithProductCategory(productCategory1)
                .Build();

            this.Session.Derive();

            var acmePostalAddress = new PostalAddressBuilder(this.Session)
                .WithDefaults()
                .Build();

            var acmeBillingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(acmePostalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralCorrespondence)
                .WithUseAsDefault(true)
                .Build();

            var acmeInquiries = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("+1").WithContactNumber("111 222 333").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).OrderInquiriesPhone)
                .WithUseAsDefault(true)
                .Build();

            var acme = new OrganisationBuilder(this.Session)
                .WithName($"Acme")
                .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                .WithPartyContactMechanism(acmeBillingAddress)
                .WithPartyContactMechanism(acmeInquiries)
                .Build();

            var contact1Email = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new EmailAddressBuilder(this.Session).WithElectronicAddressString($"employee1@acme.com").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).PersonalEmailAddress)
                .WithUseAsDefault(true)
                .Build();

            var contact2PhoneNumber = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("+1").WithAreaCode("123").WithContactNumber("456").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber)
                .WithUseAsDefault(true)
                .Build();

            var contact1 = new PersonBuilder(this.Session)
                .WithFirstName($"John")
                .WithLastName($"Doe")
                .WithGender(new GenderTypes(this.Session).Male)
                .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                .WithPartyContactMechanism(contact1Email)
                .Build();

            var contact2 = new PersonBuilder(this.Session)
                .WithFirstName($"Jane")
                .WithLastName($"Doe")
                .WithGender(new GenderTypes(this.Session).Male)
                .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                .WithPartyContactMechanism(contact2PhoneNumber)
                .Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(acme)
                .WithInternalOrganisation(allors)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(acme)
                .WithInternalOrganisation(allors)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.Session)
                .WithOrganisation(acme)
                .WithContact(contact1)
                .WithContactKind(new OrganisationContactKinds(this.Session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.Session)
                .WithOrganisation(acme)
                .WithContact(contact2)
                .WithContactKind(new OrganisationContactKinds(this.Session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var administrator = (Person)new UserGroups(this.Session).Administrators.Members.First;

            new FaceToFaceCommunicationBuilder(this.Session)
                .WithDescription($"Meeting")
                .WithSubject($"meeting")
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithFromParty(contact1)
                .WithToParty(contact2)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new EmailCommunicationBuilder(this.Session)
                .WithDescription($"Email")
                .WithSubject($"email")
                .WithFromEmail(email2)
                .WithToEmail(email2)
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new LetterCorrespondenceBuilder(this.Session)
                .WithDescription($"Letter")
                .WithSubject($"letter")
                .WithFromParty(administrator)
                .WithToParty(contact1)
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new PhoneCommunicationBuilder(this.Session)
                .WithDescription($"Phone")
                .WithSubject($"phone")
                .WithFromParty(administrator)
                .WithToParty(contact1)
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            var salesOrderItem1 = new SalesOrderItemBuilder(this.Session)
                .WithDescription("first item")
                .WithProduct(good1)
                .WithAssignedUnitPrice(3000)
                .WithQuantityOrdered(1)
                .WithMessage(@"line1
line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var salesOrderItem2 = new SalesOrderItemBuilder(this.Session)
                .WithDescription("second item")
                .WithAssignedUnitPrice(2000)
                .WithQuantityOrdered(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var salesOrderItem3 = new SalesOrderItemBuilder(this.Session)
                .WithDescription("Fee")
                .WithAssignedUnitPrice(100)
                .WithQuantityOrdered(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Fee)
                .Build();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(allors)
                .WithBillToCustomer(acme)
                .WithBillToEndCustomerContactMechanism(acmeBillingAddress.ContactMechanism)
                .WithSalesOrderItem(salesOrderItem1)
                .WithSalesOrderItem(salesOrderItem2)
                .WithSalesOrderItem(salesOrderItem3)
                .WithCustomerReference("a reference number")
                .WithDescription("Sale of 1 used Aircraft Towbar")
                .WithVatRegime(new VatRegimes(this.Session).Assessable)
                .Build();

            var salesInvoiceItem1 = new SalesInvoiceItemBuilder(this.Session)
                .WithDescription("first item")
                .WithProduct(good1)
                .WithAssignedUnitPrice(3000)
                .WithQuantity(1)
                .WithMessage(@"line1
line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var salesInvoiceItem2 = new SalesInvoiceItemBuilder(this.Session)
                .WithDescription("second item")
                .WithAssignedUnitPrice(2000)
                .WithQuantity(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var salesInvoiceItem3 = new SalesInvoiceItemBuilder(this.Session)
                .WithDescription("Fee")
                .WithAssignedUnitPrice(100)
                .WithQuantity(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Fee)
                .Build();

            var salesInvoice = new SalesInvoiceBuilder(this.Session)
                .WithBilledFrom(allors)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(acme)
                .WithBillToContactMechanism(acme.PartyContactMechanisms[0].ContactMechanism)
                .WithBillToEndCustomerContactMechanism(acmeBillingAddress.ContactMechanism)
                .WithSalesInvoiceItem(salesInvoiceItem1)
                .WithSalesInvoiceItem(salesInvoiceItem2)
                .WithSalesInvoiceItem(salesInvoiceItem3)
                .WithCustomerReference("a reference number")
                .WithDescription("Sale of 1 used Aircraft Towbar")
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithVatRegime(new VatRegimes(this.Session).Assessable)
                .Build();

            var purchaseInvoiceItem1 = new PurchaseInvoiceItemBuilder(this.Session)
                .WithDescription("first item")
                .WithProduct(good1)
                .WithAssignedUnitPrice(3000)
                .WithQuantity(1)
                .WithMessage(@"line1
line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var purchaseInvoiceItem2 = new PurchaseInvoiceItemBuilder(this.Session)
                .WithDescription("second item")
                .WithAssignedUnitPrice(2000)
                .WithQuantity(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var purchaseInvoiceItem3 = new PurchaseInvoiceItemBuilder(this.Session)
                .WithDescription("Fee")
                .WithAssignedUnitPrice(100)
                .WithQuantity(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Fee)
                .Build();

            var purchaseInvoice = new PurchaseInvoiceBuilder(this.Session)
                .WithBilledTo(allors)
                .WithInvoiceNumber("1")
                .WithBilledFrom(acme)
                .WithPurchaseInvoiceItem(purchaseInvoiceItem1)
                .WithPurchaseInvoiceItem(purchaseInvoiceItem2)
                .WithPurchaseInvoiceItem(purchaseInvoiceItem3)
                .WithCustomerReference("a reference number")
                .WithDescription("Purchase of 1 used Aircraft Towbar")
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithVatRegime(new VatRegimes(this.Session).Assessable)
                .Build();

            var purchaseOrderItem1 = new PurchaseOrderItemBuilder(this.Session)
                .WithDescription("first purchase order item")
                .WithPart(good1.Part)
                .WithQuantityOrdered(1)
                .Build();

            var purchaseOrder = new PurchaseOrderBuilder(this.Session)
                .WithOrderedBy(allors)
                .WithTakenViaSupplier(acme)
                .WithPurchaseOrderItem(purchaseOrderItem1)
                .WithCustomerReference("reference 123")
                .WithFacility(facility)
                .Build();

            var workTask = new WorkTaskBuilder(this.Session)
                .WithTakenBy(allors)
                .WithCustomer(acme)
                .WithName("maintenance")
                .Build();

            new PositionTypeBuilder(this.Session)
                .WithTitle("Mechanic")
                .WithUniqueId(new Guid("E62A8F4B-8045-472E-AB18-E39C51A02696"))
                .Build();

            new PositionTypeRateBuilder(this.Session)
                .WithRate(100)
                .WithRateType(new RateTypes(this.Session).StandardRate)
                .WithFrequency(new TimeFrequencies(this.Session).Hour)
                .Build();

            this.Session.Derive();

            var customerSalesAgreement = new SalesAgreementBuilder(this.Session)
                .WithDescription("default payment terms")
                .WithAgreementTerm(new InvoiceTermBuilder(this.Session).WithTermType(new InvoiceTermTypes(this.Session).PaymentNetDays).WithTermValue("30").Build())
                .Build();

            var customer = new OrganisationBuilder(this.Session)
                .WithName("a customer")
                .WithAgreement(customerSalesAgreement)
                .WithTaxNumber("cust.tax number")
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(allors).WithFromDate(DateTime.Now.AddDays(-1)).Build();

            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithDefaults()
                .Build();

            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(contactMechanism)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralCorrespondence)
                .Build();
            customer.AddPartyContactMechanism(partyContactMechanism);

            var serialisedItem2 = new SerialisedItemBuilder(this.Session).WithSerialNumber("123").Build();

            var serialisedUnifiedGood = new UnifiedGoodBuilder(this.Session)
                .WithName("serialised good")
                .WithDefaultFacility(facility)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                .WithVatRate(new VatRates(this.Session).Extent().First(v => v.Rate == 0))
                .WithSerialisedItem(serialisedItem2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Kilogram)
                .Build();

            var unifiedGood = new UnifiedGoodBuilder(this.Session)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithVatRate(new VatRates(this.Session).Extent().First(v => v.Rate == 0))
                .Build();

            new SerialisedInventoryItemBuilder(this.Session)
                .WithSerialisedItem(serialisedItem2)
                .WithPart(serialisedUnifiedGood)
                .WithFacility(facility)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Kilogram)
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithSerialisedItem(serialisedItem2)
                .WithPart(serialisedUnifiedGood)
                .WithQuantity(1)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithDescription("first item")
                .WithProduct(serialisedUnifiedGood)
                .WithAssignedUnitPrice(3000)
                .WithQuantity(1)
                .WithMessage(@"line1
                line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var item2 = new SalesInvoiceItemBuilder(this.Session)
                .WithDescription("second item")
                .WithProduct(unifiedGood)
                .WithAssignedUnitPrice(2000)
                .WithQuantity(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var item3 = new SalesInvoiceItemBuilder(this.Session)
                .WithDescription("Fee")
                .WithAssignedUnitPrice(100)
                .WithQuantity(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Surcharge)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBilledFrom(allors)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(item1)
                .WithSalesInvoiceItem(item2)
                .WithSalesInvoiceItem(item3)
                .WithCustomerReference("a reference number")
                .WithDescription("Sale of 1 used Aircraft Towbar")
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithVatRegime(new VatRegimes(this.Session).Assessable)
                .Build();

            this.Session.Derive();

            var request = new RequestForQuoteBuilder(this.Session)
                .WithRecipient(allors)
                .WithEmailAddress("meknip@xs4all.nl")
                .WithTelephoneCountryCode("+31")
                .WithTelephoneNumber("0613568160")
                .WithDescription("anonymous request")
                .Build();

            var requestItem = new RequestItemBuilder(this.Session)
                .WithProduct(new Goods(this.Session).Extent().First)
                .WithQuantity(1)
                .Build();

            request.AddRequestItem(requestItem);

            var quote = new ProductQuoteBuilder(this.Session)
                .WithIssuer(allors)
                .WithDescription("quote")
                .WithReceiver(customer)
                .WithFullfillContactMechanism(customer.GeneralCorrespondence)
                .Build();

            var quoteItem = new QuoteItemBuilder(this.Session)
                .WithProduct(new Goods(this.Session).Extent().First)
                .WithQuantity(1)
                .WithAssignedUnitPrice(10)
                .Build();

            quote.AddQuoteItem(quoteItem);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithTakenBy(allors)
                .WithShipToCustomer(customer)
                .Build();

            var salesOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(unifiedGood)
                .WithQuantityOrdered(1)
                .WithAssignedUnitPrice(10)
                .Build();

            salesOrder.AddSalesOrderItem(salesOrderItem);

            for (int i = 0; i < 10; i++)
            {
                allors.CreateB2BCustomer(this.Session.Faker());
            }

            new CustomerShipmentBuilder(this.Session).WithDefaults(allors).Build();

            for (int i = 0; i < 10; i++)
            {
                allors.CreateSupplier(this.Session.Faker());
            }

            new PurchaseShipmentBuilder(this.Session).WithDefaults(allors).Build();

            this.Session.Derive();
        }

        private byte[] GetResourceBytes(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var manifestResourceName = assembly.GetManifestResourceNames().First(v => v.Contains(name));
            var resource = assembly.GetManifestResourceStream(manifestResourceName);
            if (resource != null)
            {
                using (var ms = new MemoryStream())
                {
                    resource.CopyTo(ms);
                    return ms.ToArray();
                }
            }

            return null;
        }

        private Template CreateOpenDocumentTemplate(byte[] content)
        {
            var media = new MediaBuilder(this.Session).WithInData(content).Build();
            var templateType = new TemplateTypes(this.Session).OpenDocumentType;
            var template = new TemplateBuilder(this.Session).WithMedia(media).WithTemplateType(templateType).Build();
            return template;
        }
    }
}

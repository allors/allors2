namespace Intranet.Tests
{
    using System;
    using System.IO;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    public class Population
    {
        private readonly ISession session;

        private DirectoryInfo dataPath;

        public Population(ISession session, DirectoryInfo dataPath)
        {
            this.session = session;
            this.dataPath = dataPath;
        }

        public void Execute()
        {
            var singleton = this.session.GetSingleton();
            var dutchLocale = new Locales(this.session).DutchNetherlands;
            singleton.AddAdditionalLocale(dutchLocale);

            var euro = new Currencies(this.session).FindBy(M.Currency.IsoCode, "EUR");

            var belgium = new Countries(this.session).FindBy(M.Country.IsoCode, "BE");
            var usa = new Countries(this.session).FindBy(M.Country.IsoCode, "US");

            var postalAddress = new PostalAddressBuilder(this.session)
                .WithAddress1("Kleine Nieuwedijkstraat 4")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.session).WithLocality("Mechelen").WithPostalCode("2800").WithCountry(belgium).Build())
                .Build();

            var phone = new TelecommunicationsNumberBuilder(this.session)
                .WithCountryCode("+32")
                .WithContactNumber("2 335 2335")
                .Build();

            var email = new EmailAddressBuilder(this.session)
                .WithElectronicAddressString("info@allors.com")
                .Build();

            var email2 = new EmailAddressBuilder(this.session)
                .WithElectronicAddressString("recipient@acme.com")
                .Build();

            var ing = new BankBuilder(this.session)
                .WithName("ING België")
                .WithBic("BBRUBEBB")
                .WithCountry(belgium)
                .Build();

            var bankaccount = new BankAccountBuilder(this.session)
                .WithBank(ing)
                .WithIban("BE89 3200 1467 7685")
                .WithNameOnAccount("Allors")
                .WithCurrency(euro)
                .Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.session).WithBankAccount(bankaccount).WithDescription("Hoofdbank").Build();

            var internalOrganisation = new OrganisationBuilder(this.session)
                .WithIsInternalOrganisation(true)
                .WithTaxNumber("BE 0476967014")
                .WithName("Allors")
                .WithBankAccount(bankaccount)
                .WithPreferredCurrency(new Currencies(this.session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithDefaultCollectionMethod(new OwnBankAccountBuilder(this.session).WithBankAccount(bankaccount).WithDescription("Hoofdbank").Build())
                .WithInvoiceSequence(new InvoiceSequences(this.session).EnforcedSequence)
                .WithFiscalYearStartMonth(01)
                .WithFiscalYearStartDay(01)
                .WithDoAccounting(false)
                .WithRequestNumberPrefix("requestno:")
                .WithQuoteNumberPrefix("quoteno: ")
                .Build();

            internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.session)
                .WithUseAsDefault(true)
                .WithContactMechanism(phone)
                .WithContactPurpose(new ContactMechanismPurposes(this.session).GeneralPhoneNumber)
                .Build());

            internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.session)
                .WithUseAsDefault(true)
                .WithContactMechanism(email)
                .WithContactPurpose(new ContactMechanismPurposes(this.session).GeneralEmail)
                .Build());

            internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.session)
                .WithUseAsDefault(true)
                .WithContactMechanism(postalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.session).GeneralCorrespondence)
                .Build());

            var logo = this.dataPath + @"\admin\images\logo.png";

            if (File.Exists(logo))
            {
                var fileInfo = new FileInfo(logo);

                var fileName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName).ToLowerInvariant();
                var content = File.ReadAllBytes(fileInfo.FullName);
                var image = new MediaBuilder(this.session).WithFileName(fileName).WithInData(content).Build();
                internalOrganisation.LogoImage = image;
            }

            var facility1 = new FacilityBuilder(this.session)
                .WithName("facility1")
                .WithDescription("Facility 1")
                .WithFacilityType(new FacilityTypes(this.session).Warehouse)
                .WithOwner(internalOrganisation)
                .Build();

            var facility2 = new FacilityBuilder(this.session)
                .WithName("facility1")
                .WithDescription("Facility 1")
                .WithFacilityType(new FacilityTypes(this.session).Warehouse)
                .WithOwner(internalOrganisation)
                .Build();

            internalOrganisation.DefaultFacility = facility1;

            new StoreBuilder(this.session)
                .WithName("Allors store")
                .WithOutgoingShipmentNumberPrefix("shipmentno: ")
                .WithSalesInvoiceNumberPrefix("invoiceno: ")
                .WithSalesOrderNumberPrefix("orderno: ")
                .WithDefaultCollectionMethod(ownBankAccount)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.session).Ground)
                .WithDefaultCarrier(new Carriers(this.session).Fedex)
                .WithBillingProcess(new BillingProcesses(this.session).BillingForShipmentItems)
                .WithSalesInvoiceCounter(new CounterBuilder(this.session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build())
                .WithIsImmediatelyPicked(true)
                .WithInternalOrganisation(internalOrganisation)
                .Build();

            var productType = new ProductTypeBuilder(this.session)
                .WithName($"Gizmo Serialized")
                .WithSerialisedInventoryItemCharacteristicType(new SerialisedInventoryItemCharacteristicTypeBuilder(this.session)
                                            .WithName("Size")
                                            .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Afmeting").WithLocale(dutchLocale).Build())
                                            .Build())
                .WithSerialisedInventoryItemCharacteristicType(new SerialisedInventoryItemCharacteristicTypeBuilder(this.session)
                                            .WithName("Weight")
                                            .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Gewicht").WithLocale(dutchLocale).Build())
                                            .WithUnitOfMeasure(new UnitsOfMeasure(this.session).Kilogram)
                                            .Build())
                .Build();

            var productCategory1 = new ProductCategoryBuilder(this.session)
                .WithInternalOrganisation(internalOrganisation)
                .WithName("Best selling gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Meest verkochte gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory2 = new ProductCategoryBuilder(this.session)
                .WithInternalOrganisation(internalOrganisation)
                .WithName("Big Gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Grote Gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory3 = new ProductCategoryBuilder(this.session)
                .WithInternalOrganisation(internalOrganisation)
                .WithName("Small gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Kleine gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            new CatalogueBuilder(this.session)
                .WithInternalOrganisation(internalOrganisation)
                .WithName("New gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Nieuwe gizmo's").WithLocale(dutchLocale).Build())
                .WithDescription("Latest in the world of Gizmo's")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.session).WithText("Laatste in de wereld van Gizmo's").WithLocale(dutchLocale).Build())
                .WithProductCategory(productCategory1)
                .Build();

            var brand1 = new BrandBuilder(this.session).WithName("brand 1").Build();

            var vatRate = new VatRateBuilder(this.session).WithRate(21).Build();

            var finishedGood1 = new PartBuilder(this.session)
                .WithName("finished good1")
                .WithPartId("1-1")
                .WithInventoryItemKind(new InventoryItemKinds(this.session).NonSerialised)
                .Build();

            var finishedGood2 = new PartBuilder(this.session)
                .WithName("finished good2")
                .WithPartId("1-2")
                .WithInventoryItemKind(new InventoryItemKinds(this.session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.session)
                .WithName("Tiny blue round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Zeer kleine blauwe ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect blue with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.session).WithText("Perfect blauw met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate)
                .WithPrimaryProductCategory(productCategory3)
                .WithPart(finishedGood1)
                .Build();

            new ProductFeatureApplicabilityBuilder(this.session)
                .WithFromDate(DateTime.UtcNow)
                .WithAvailableFor(good1)
                .WithProductFeature(new BrandBuilder(this.session).WithName("brand 1").Build())
                .WithProductFeatureApplicabilityKind(new ProductFeatureApplicabilityKinds(this.session).Required)
                .Build();

            var goodInventoryItem1 = new NonSerialisedInventoryItemBuilder(this.session).WithPart(finishedGood1).WithFacility(facility1).Build();
            goodInventoryItem1.AddInventoryItemTransaction(new InventoryItemTransactionBuilder(this.session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.session).Unknown).Build());
            var goodInventoryItem2 = new NonSerialisedInventoryItemBuilder(this.session).WithPart(finishedGood2).WithFacility(facility2).Build();
            goodInventoryItem2.AddInventoryItemTransaction(new InventoryItemTransactionBuilder(this.session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.session).Unknown).Build());

            var good2 = new GoodBuilder(this.session)
                .WithName("Tiny red round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Zeer kleine rode ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect red with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.session).WithText("Perfect rood met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithSku("10102")
                .WithVatRate(vatRate)
                .WithPrimaryProductCategory(productCategory3)
                .WithPart(finishedGood2)
                .Build();

            new ProductFeatureApplicabilityBuilder(this.session)
                .WithFromDate(DateTime.UtcNow)
                .WithAvailableFor(good2)
                .WithProductFeature(new BrandBuilder(this.session).WithName("brand 2").Build())
                .WithProductFeatureApplicabilityKind(new ProductFeatureApplicabilityKinds(this.session).Required)
                .Build();

            var good2InventoryItem = new SerialisedInventoryItemBuilder(this.session).WithPart(finishedGood2).WithSerialNumber("1").Build();

            var good3 = new GoodBuilder(this.session)
                .WithName("Tiny green round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Zeer kleine groene ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect red with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.session).WithText("Perfect groen met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithSku("10103")
                .WithVatRate(vatRate)
                .WithPrimaryProductCategory(productCategory3)
                .Build();

            new ProductFeatureApplicabilityBuilder(this.session)
                .WithFromDate(DateTime.UtcNow)
                .WithAvailableFor(good3)
                .WithProductFeature(new BrandBuilder(this.session).WithName("brand 3").Build())
                .WithProductFeatureApplicabilityKind(new ProductFeatureApplicabilityKinds(this.session).Required)
                .Build();

            this.session.Derive();

            var acmePostalAddress = new PostalAddressBuilder(this.session)
                .WithAddress1($"Acme address")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.session).WithLocality($"Acme city").WithPostalCode("1111").WithCountry(usa).Build())
                .Build();

            var acmeBillingAddress = new PartyContactMechanismBuilder(this.session)
                .WithContactMechanism(acmePostalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.session).GeneralCorrespondence)
                .WithUseAsDefault(true)
                .Build();

            var acmeInquiries = new PartyContactMechanismBuilder(this.session)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.session).WithCountryCode("+1").WithContactNumber("111 222 333").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.session).GeneralPhoneNumber)
                .WithContactPurpose(new ContactMechanismPurposes(this.session).OrderInquiriesPhone)
                .WithUseAsDefault(true)
                .Build();

            var acme = new OrganisationBuilder(this.session)
                .WithName($"Acme")
                .WithLocale(new Locales(this.session).EnglishUnitedStates)
                .WithPartyContactMechanism(acmeBillingAddress)
                .WithPartyContactMechanism(acmeInquiries)
                .Build();

            var contact1Email = new PartyContactMechanismBuilder(this.session)
                .WithContactMechanism(new EmailAddressBuilder(this.session).WithElectronicAddressString($"employee1@acme.com").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.session).PersonalEmailAddress)
                .WithUseAsDefault(true)
                .Build();

            var contact2PhoneNumber = new PartyContactMechanismBuilder(this.session)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.session).WithCountryCode("+1").WithAreaCode("123").WithContactNumber("456").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.session).GeneralPhoneNumber)
                .WithUseAsDefault(true)
                .Build();

            var contact1 = new PersonBuilder(this.session)
                .WithFirstName("contact1")
                .WithGender(new GenderTypes(this.session).Male)
                .WithLocale(new Locales(this.session).EnglishUnitedStates)
                .WithPartyContactMechanism(contact1Email)
                .Build();

            var contact2 = new PersonBuilder(this.session)
                .WithFirstName("contact2")
                .WithGender(new GenderTypes(this.session).Male)
                .WithLocale(new Locales(this.session).EnglishUnitedStates)
                .WithPartyContactMechanism(contact2PhoneNumber)
                .Build();

            new CustomerRelationshipBuilder(this.session)
                .WithCustomer(acme)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SupplierRelationshipBuilder(this.session)
                .WithSupplier(acme)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.session)
                .WithOrganisation(acme)
                .WithContact(contact1)
                .WithContactKind(new OrganisationContactKinds(this.session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.session)
                .WithOrganisation(acme)
                .WithContact(contact2)
                .WithContactKind(new OrganisationContactKinds(this.session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var administrator = (Person)new UserGroups(this.session).Administrators.Members.First;

            new FaceToFaceCommunicationBuilder(this.session)
                .WithDescription($"Meeting")
                .WithSubject($"meeting")
                .WithEventPurpose(new CommunicationEventPurposes(this.session).Meeting)
                .WithParticipant(contact1)
                .WithParticipant(contact2)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new EmailCommunicationBuilder(this.session)
                .WithDescription($"Email")
                .WithSubject("email")
                .WithOriginator(email)
                .WithAddressee(email2)
                .WithEventPurpose(new CommunicationEventPurposes(this.session).Meeting)
                .WithOwner(contact1)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new LetterCorrespondenceBuilder(this.session)
                .WithDescription($"Letter")
                .WithSubject("letter")
                .WithOriginator(administrator)
                .WithReceiver(contact1)
                .WithEventPurpose(new CommunicationEventPurposes(this.session).Meeting)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new PhoneCommunicationBuilder(this.session)
                .WithDescription($"Phone")
                .WithSubject("phone")
                .WithCaller(administrator)
                .WithReceiver(contact1)
                .WithEventPurpose(new CommunicationEventPurposes(this.session).Meeting)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            var salesOrderItem1 = new SalesOrderItemBuilder(this.session)
                .WithDescription("first item")
                .WithProduct(good1)
                .WithActualUnitPrice(3000)
                .WithQuantityOrdered(1)
                .WithMessage(@"line1
line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                .Build();

            var salesOrderItem2 = new SalesOrderItemBuilder(this.session)
                .WithDescription("second item")
                .WithActualUnitPrice(2000)
                .WithQuantityOrdered(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                .Build();

            var salesOrderItem3 = new SalesOrderItemBuilder(this.session)
                .WithDescription("Fee")
                .WithActualUnitPrice(100)
                .WithQuantityOrdered(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.session).Fee)
                .Build();

            var order = new SalesOrderBuilder(this.session)
                .WithBillToCustomer(acme)
                .WithBillToEndCustomerContactMechanism(acmeBillingAddress.ContactMechanism)
                .WithSalesOrderItem(salesOrderItem1)
                .WithSalesOrderItem(salesOrderItem2)
                .WithSalesOrderItem(salesOrderItem3)
                .WithCustomerReference("a reference number")
                .WithDescription("Sale of 1 used Aircraft Towbar")
                .WithVatRegime(new VatRegimes(this.session).Assessable)
                .Build();


            var salesInvoiceItem1 = new SalesInvoiceItemBuilder(this.session)
                .WithDescription("first item")
                .WithProduct(good1)
                .WithActualUnitPrice(3000)
                .WithQuantity(1)
                .WithMessage(@"line1
line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                .Build();

            var salesInvoiceItem2 = new SalesInvoiceItemBuilder(this.session)
                .WithDescription("second item")
                .WithActualUnitPrice(2000)
                .WithQuantity(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                .Build();

            var salesInvoiceItem3 = new SalesInvoiceItemBuilder(this.session)
                .WithDescription("Fee")
                .WithActualUnitPrice(100)
                .WithQuantity(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.session).Fee)
                .Build();

            var salesInvoice = new SalesInvoiceBuilder(this.session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(acme)
                .WithBillToContactMechanism(acme.PartyContactMechanisms[0].ContactMechanism)
                .WithBillToEndCustomerContactMechanism(acmeBillingAddress.ContactMechanism)
                .WithSalesInvoiceItem(salesInvoiceItem1)
                .WithSalesInvoiceItem(salesInvoiceItem2)
                .WithSalesInvoiceItem(salesInvoiceItem3)
                .WithCustomerReference("a reference number")
                .WithDescription("Sale of 1 used Aircraft Towbar")
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.session).SalesInvoice)
                .WithVatRegime(new VatRegimes(this.session).Assessable)
                .Build();

            var purchaseInvoiceItem1 = new PurchaseInvoiceItemBuilder(this.session)
                .WithDescription("first item")
                .WithProduct(good1)
                .WithActualUnitPrice(3000)
                .WithQuantity(1)
                .WithMessage(@"line1
line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                .Build();

            var purchaseInvoiceItem2 = new PurchaseInvoiceItemBuilder(this.session)
                .WithDescription("second item")
                .WithActualUnitPrice(2000)
                .WithQuantity(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                .Build();

            var purchaseInvoiceItem3 = new PurchaseInvoiceItemBuilder(this.session)
                .WithDescription("Fee")
                .WithActualUnitPrice(100)
                .WithQuantity(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.session).Fee)
                .Build();

            var pruchaseInvoice = new PurchaseInvoiceBuilder(this.session)
                .WithInvoiceNumber("1")
                .WithBilledFrom(acme)
                .WithBillToCustomer(internalOrganisation)
                .WithPurchaseInvoiceItem(purchaseInvoiceItem1)
                .WithPurchaseInvoiceItem(purchaseInvoiceItem2)
                .WithPurchaseInvoiceItem(purchaseInvoiceItem3)
                .WithCustomerReference("a reference number")
                .WithDescription("Purchase of 1 used Aircraft Towbar")
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.session).PurchaseInvoice)
                .WithVatRegime(new VatRegimes(this.session).Assessable)
                .Build();


            this.session.Derive();
        }
    }
}
namespace Allors
{
    using System;
    using System.IO;

    using Allors.Domain;
    using Allors.Meta;

    public class Demo
    {
        private readonly ISession Session;

        private DirectoryInfo DataPath;

        public Demo(ISession session, DirectoryInfo dataPath)
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

            var belgium = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");
            var usa = new Countries(this.Session).FindBy(M.Country.IsoCode, "US");

            var postalAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Kleine Nieuwedijkstraat 4")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("Mechelen").WithPostalCode("2800").WithCountry(belgium).Build())
                .Build();

            var phone = new TelecommunicationsNumberBuilder(this.Session)
                .WithCountryCode("+32")
                .WithContactNumber("2 335 2335")
                .Build();

            var email = new EmailAddressBuilder(this.Session)
                .WithElectronicAddressString("info@allors.com")
                .Build();

            var ing = new BankBuilder(this.Session)
                .WithName("ING België")
                .WithBic("BBRUBEBB")
                .WithCountry(belgium)
                .Build();

            var bankaccount = new BankAccountBuilder(this.Session)
                .WithBank(ing)
                .WithIban("BE89 3200 1467 7685")
                .WithNameOnAccount("Allors")
                .WithCurrency(euro)
                .Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session).WithBankAccount(bankaccount).WithDescription("Hoofdbank").Build();

            var internalOrganisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithTaxNumber("BE 0476967014")
                .WithName("Allors")
                .WithBankAccount(bankaccount)
                .WithPreferredCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithDefaultCollectionMethod(new OwnBankAccountBuilder(Session).WithBankAccount(bankaccount).WithDescription("Hoofdbank").Build())
                .WithInvoiceSequence(new InvoiceSequences(this.Session).EnforcedSequence)
                .WithFiscalYearStartMonth(01)
                .WithFiscalYearStartDay(01)
                .WithDoAccounting(false)
                .WithRequestNumberPrefix("requestno:")
                .WithQuoteNumberPrefix("quoteno: ")
                .Build();

            internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(phone)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber)
                .Build());

            internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(email)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralEmail)
                .Build());

            internalOrganisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(postalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralCorrespondence)
                .Build());

            var logo = this.DataPath + @"\admin\images\logo.png";

            if (File.Exists(logo))
            {
                var fileInfo = new FileInfo(logo);

                var fileName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName).ToLowerInvariant();
                var content = File.ReadAllBytes(fileInfo.FullName);
                var image = new MediaBuilder(this.Session).WithFileName(fileName).WithInData(content).Build();
                internalOrganisation.LogoImage = image;
            }

            var facility = new FacilityBuilder(this.Session)
                .WithName("Headquarters")
                .WithDescription("Allors HQ")
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithOwner(internalOrganisation)
                .Build();

            internalOrganisation.DefaultFacility = facility;

            new StoreBuilder(this.Session)
                .WithName("Allors store")
                .WithOutgoingShipmentNumberPrefix("shipmentno: ")
                .WithSalesInvoiceNumberPrefix("invoiceno: ")
                .WithSalesOrderNumberPrefix("orderno: ")
                .WithDefaultCollectionMethod(ownBankAccount)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithBillingProcess(new BillingProcesses(this.Session).BillingForShipmentItems)
                .WithSalesInvoiceCounter(new CounterBuilder(this.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build())
                .WithIsImmediatelyPicked(true)
                .WithInternalOrganisation(internalOrganisation)
                .Build();

            this.Session.Derive();

            var acmePostalAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Acme address 1")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("Acme city").WithPostalCode("1111").WithCountry(usa).Build())
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
                .WithName("Acme")
                .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                .WithPartyContactMechanism(acmeBillingAddress)
                .WithPartyContactMechanism(acmeInquiries)
                .Build();

            var contact1Email = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new EmailAddressBuilder(this.Session).WithElectronicAddressString("employee1@acme.com").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).PersonalEmailAddress)
                .WithUseAsDefault(true)
                .Build();

            var contact2PhoneNumber = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("+1").WithAreaCode("123").WithContactNumber("456").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber)
                .WithUseAsDefault(true)
                .Build();

            var contact1 = new PersonBuilder(this.Session)
                .WithFirstName("contact1")
                .WithGender(new GenderTypes(this.Session).Male)
                .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                .WithPartyContactMechanism(contact1Email)
                .Build();

            var contact2 = new PersonBuilder(this.Session)
                .WithFirstName("contact2")
                .WithGender(new GenderTypes(this.Session).Male)
                .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                .WithPartyContactMechanism(contact2PhoneNumber)
                .Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(acme)
                .WithInternalOrganisation(internalOrganisation)
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

            new FaceToFaceCommunicationBuilder(this.Session)
                .WithDescription("Meeting")
                .WithSubject("review")
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithParticipant(contact1)
                .WithParticipant(contact2)
                .WithOwner(new People(this.Session).FindBy(M.Person.UserName, "administrator1"))
                .WithActualStart(DateTime.UtcNow)
                .Build();

            var kg = new UnitOfMeasureBuilder(this.Session)
                .WithName("Kilograms")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kg").WithLocale(dutchLocale).Build())
                .Build();

            new ProductTypeBuilder(this.Session)
                .WithName("Gizmo")
                .WithSerialisedInventoryItemCharacteristicType(new SerialisedInventoryItemCharacteristicTypeBuilder(this.Session)
                                            .WithName("Size")
                                            .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Afmeting").WithLocale(dutchLocale).Build())
                                            .Build())
                .WithSerialisedInventoryItemCharacteristicType(new SerialisedInventoryItemCharacteristicTypeBuilder(this.Session)
                                            .WithName("Weight")
                                            .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gewicht").WithLocale(dutchLocale).Build())
                                            .WithUnitOfMeasure(kg)
                                            .Build())
                .Build();

            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithName("Best selling gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Meest verkochte gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithName("Big Gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Grote Gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory3 = new ProductCategoryBuilder(this.Session)
                .WithName("Small gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kleine gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            new CatalogueBuilder(this.Session)
                .WithName("New gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Nieuwe gizmo's").WithLocale(dutchLocale).Build())
                .WithDescription("Latest in the world of Gizmo's")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Laatste in de wereld van Gizmo's").WithLocale(dutchLocale).Build())
                .WithProductCategory(productCategory1)
                .Build();

            var model1Brand1 = new ModelBuilder(this.Session)
                .WithName("model 1")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("model 1").WithLocale(dutchLocale).Build())
                .Build();

            var model2Brand1 = new ModelBuilder(this.Session)
                .WithName("model 1")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("model 2").WithLocale(dutchLocale).Build())
                .Build();

            var brand1 = new BrandBuilder(this.Session).WithName("brand 1").WithModel(model1Brand1).WithModel(model2Brand1).Build();

            var good = new GoodBuilder(this.Session)
                .WithName("Tiny blue round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zeer kleine blauwe ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect blue with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Perfect blauw met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithPrimaryProductCategory(productCategory3)
                .WithStandardFeature(brand1)
                .WithStandardFeature(model1Brand1)
                .Build();

            var goodInventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();
            goodInventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            var salesOrderItem1 = new SalesOrderItemBuilder(this.Session)
                .WithDescription("first item")
                .WithProduct(good)
                .WithActualUnitPrice(3000)
                .WithQuantityOrdered(1)
                .WithMessage(@"line1
line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var salesOrderItem2 = new SalesOrderItemBuilder(this.Session)
                .WithDescription("second item")
                .WithActualUnitPrice(2000)
                .WithQuantityOrdered(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var salesOrderItem3 = new SalesOrderItemBuilder(this.Session)
                .WithDescription("Fee")
                .WithActualUnitPrice(100)
                .WithQuantityOrdered(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Fee)
                .Build();

            var order = new SalesOrderBuilder(this.Session)
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
                .WithProduct(good)
                .WithActualUnitPrice(3000)
                .WithQuantity(1)
                .WithMessage(@"line1
line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var salesInvoiceItem2 = new SalesInvoiceItemBuilder(this.Session)
                .WithDescription("second item")
                .WithActualUnitPrice(2000)
                .WithQuantity(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var salesInvoiceItem3 = new SalesInvoiceItemBuilder(this.Session)
                .WithDescription("Fee")
                .WithActualUnitPrice(100)
                .WithQuantity(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Fee)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
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

            this.Session.Derive();

            var orderContent = order.PrintContent;
            File.WriteAllText(@"c:\temp\order.html", orderContent);

            var invoiceContent = invoice.PrintContent;
            File.WriteAllText(@"c:\temp\invoice.html", invoiceContent);
        }

        private void SetupUser(string email, string firstName, string lastName, string password)
        {
            var userEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString(email).Build();

            var person = new PersonBuilder(this.Session)
                .WithUserName(email)
                .WithFirstName(firstName)
                .WithLastName(lastName)
                .WithUserEmail(userEmail.ElectronicAddressString)
                .WithUserEmailConfirmed(true)
                .Build();

            person.AddPartyContactMechanism(
                new PartyContactMechanismBuilder(this.Session)
                    .WithContactMechanism(userEmail)
                    .WithContactPurpose(new ContactMechanismPurposes(this.Session).PersonalEmailAddress)
                    .WithUseAsDefault(true)
                    .Build());

            new EmploymentBuilder(this.Session).WithEmployee(person).Build();

            new UserGroups(this.Session).Administrators.AddMember(person);
            new UserGroups(this.Session).Creators.AddMember(person);

            person.SetPassword(password);
        }
    }
}
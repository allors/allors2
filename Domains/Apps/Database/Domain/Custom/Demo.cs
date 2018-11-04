namespace Allors
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Allors.Domain;
    using Allors.Meta;

    public class Demo
    {
        private readonly ISession Session;

        private readonly DirectoryInfo DataPath;

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

            var be = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");
            var us = new Countries(this.Session).FindBy(M.Country.IsoCode, "US");

            var email2 = new EmailAddressBuilder(this.Session)
                .WithElectronicAddressString("recipient@acme.com")
                .Build();

            var allorsLogo = this.DataPath + @"\www\admin\images\logo.png";

            var allors = this.SetupInternalOrganisation(
                name: "Allors BVBA",
                address: "Kleine Nieuwedijkstraat 4",
                postalCode: "2800",
                locality: "Mechelen",
                country: be,
                countryCode: "+32",
                phone: "2 335 2335",
                emailAddress: "email@allors.com",
                websiteAddress: "www.allors.com",
                taxNumber: "BE 0476967014",
                bankName: "ING",
                facilityName: "Allors Facility",
                bic: "BBRUBEBB",
                iban: "BE89 3200 1467 7685",
                currency: euro,
                logo: allorsLogo,
                requestNumberPrefix: "requestno:",
                quoteNumberPrefix: "quoteno:",
                articleNumberPrefix: "A-",
                requestCounterValue: 1,
                quoteCounterValue: 1);

            var dipu = this.SetupInternalOrganisation(
                name: "Dipu BVBA",
                address: "Kleine Nieuwedijkstraat 2",
                postalCode: "2800",
                locality: "Mechelen",
                country: be,
                countryCode: "+32",
                phone: "2 15 49 49 49",
                emailAddress: "email@dipu.com",
                websiteAddress: "www.dipu.com",
                taxNumber: "BE 0445366489",
                bankName: "ING",
                facilityName: "Dipu Facility",
                bic: "BBRUBEBB",
                iban: "BE23 3300 6167 6391",
                currency: euro,
                logo: allorsLogo,
                requestNumberPrefix: string.Empty,
                quoteNumberPrefix: string.Empty,
                articleNumberPrefix: string.Empty,
                requestCounterValue: 1,
                quoteCounterValue: 1);

            var productType = new ProductTypeBuilder(this.Session)
                .WithName($"Gizmo Serialized")
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
                .Build();

            new CatalogueBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("New gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Nieuwe gizmo's").WithLocale(dutchLocale).Build())
                .WithDescription("Latest in the world of Gizmo's")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Laatste in de wereld van Gizmo's").WithLocale(dutchLocale).Build())
                .WithProductCategory(productCategory1)
                .Build();

            var vatRate = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new PartBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithGoodIdentification(new SkuBuilder(this.Session)
                    .WithIdentification("10101")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Sku).Build())
                .WithGoodIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Part).Build())
                .WithName("finished good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithName("Tiny blue round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zeer kleine blauwe ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect blue with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Perfect blauw met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate)
                .WithPrimaryProductCategory(productCategory3)
                .WithPart(finishedGood)
                .WithGoodIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("Art 1")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Good).Build())
                .Build();

            new InventoryItemTransactionBuilder(this.Session).WithPart(finishedGood).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();

            var finishedGood2 = new PartBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("finished good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                .WithProductType(productType)
                .WithGoodIdentification(new SkuBuilder(this.Session)
                    .WithIdentification("10102")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Sku).Build())
                .WithGoodIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P2")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Part).Build())
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithName("Tiny red round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zeer kleine rode ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect red with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Perfect rood met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithGoodIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("Art 2")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(vatRate)
                .WithPrimaryProductCategory(productCategory3)
                .WithPart(finishedGood2)
                .Build();

            var serialisedItem = new SerialisedItemBuilder(this.Session).WithSerialNumber("1").Build();
            new SerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood2).WithSerialisedItem(serialisedItem).WithFacility(allors.DefaultFacility).Build();

            var finishedGood3 = new PartBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithGoodIdentification(new SkuBuilder(this.Session)
                    .WithIdentification("10103")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Sku).Build())
                .WithGoodIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P3")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Part).Build())
                .WithName("finished good3")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good3 = new GoodBuilder(this.Session)
                .WithName("Tiny green round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zeer kleine groene ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect red with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Perfect groen met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithGoodIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("Art 3")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(vatRate)
                .WithPrimaryProductCategory(productCategory3)
                .WithPart(finishedGood3)
                .Build();

            this.Session.Derive();

            for (int i = 0; i < 100; i++)
            {
                var acmePostalAddress = new PostalAddressBuilder(this.Session)
                    .WithAddress1($"Acme{i} address 1")
                    .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality($"Acme{i} city").WithPostalCode("1111").WithCountry(us).Build())
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
                    .WithName($"Acme{i}")
                    .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                    .WithPartyContactMechanism(acmeBillingAddress)
                    .WithPartyContactMechanism(acmeInquiries)
                    .Build();

                var contact1Email = new PartyContactMechanismBuilder(this.Session)
                    .WithContactMechanism(new EmailAddressBuilder(this.Session).WithElectronicAddressString($"employee1@acme{i}.com").Build())
                    .WithContactPurpose(new ContactMechanismPurposes(this.Session).PersonalEmailAddress)
                    .WithUseAsDefault(true)
                    .Build();

                var contact2PhoneNumber = new PartyContactMechanismBuilder(this.Session)
                    .WithContactMechanism(new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("+1").WithAreaCode("123").WithContactNumber("456").Build())
                    .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber)
                    .WithUseAsDefault(true)
                    .Build();

                var contact1 = new PersonBuilder(this.Session)
                    .WithFirstName($"John{i}")
                    .WithLastName($"Doe{i}")
                    .WithGender(new GenderTypes(this.Session).Male)
                    .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                    .WithPartyContactMechanism(contact1Email)
                    .Build();

                var contact2 = new PersonBuilder(this.Session)
                    .WithFirstName($"Jane{i}")
                    .WithLastName($"Doe{i}")
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
                    .WithDescription($"Meeting ${i}")
                    .WithSubject($"meeting ${i}")
                    .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                    .WithParticipant(contact1)
                    .WithParticipant(contact2)
                    .WithOwner(administrator)
                    .WithActualStart(DateTime.UtcNow)
                    .Build();

                new EmailCommunicationBuilder(this.Session)
                    .WithDescription($"Email ${i}")
                    .WithSubject($"email ${i}")
                    .WithOriginator(email2)
                    .WithAddressee(email2)
                    .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(DateTime.UtcNow)
                    .Build();

                new LetterCorrespondenceBuilder(this.Session)
                    .WithDescription($"Letter ${i}")
                    .WithSubject($"letter ${i}")
                    .WithOriginator(administrator)
                    .WithReceiver(contact1)
                    .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(DateTime.UtcNow)
                    .Build();

                new PhoneCommunicationBuilder(this.Session)
                    .WithDescription($"Phone ${i}")
                    .WithSubject($"phone ${i}")
                    .WithCaller(administrator)
                    .WithReceiver(contact1)
                    .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(DateTime.UtcNow)
                    .Build();

                var salesOrderItem1 = new SalesOrderItemBuilder(this.Session)
                    .WithDescription("first item")
                    .WithProduct(good1)
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
                    .WithActualUnitPrice(3000)
                    .WithQuantity(1)
                    .WithMessage(@"line1
line2")
                    .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                    .Build();

                var purchaseInvoiceItem2 = new PurchaseInvoiceItemBuilder(this.Session)
                    .WithDescription("second item")
                    .WithActualUnitPrice(2000)
                    .WithQuantity(2)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                    .Build();

                var purchaseInvoiceItem3 = new PurchaseInvoiceItemBuilder(this.Session)
                    .WithDescription("Fee")
                    .WithActualUnitPrice(100)
                    .WithQuantity(1)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Fee)
                    .Build();

                var purchaseInvoice = new PurchaseInvoiceBuilder(this.Session)
                    .WithBilledTo(allors)
                    .WithInvoiceNumber("1")
                    .WithBilledFrom(acme)
                    .WithBillToCustomer(allors)
                    .WithPurchaseInvoiceItem(purchaseInvoiceItem1)
                    .WithPurchaseInvoiceItem(purchaseInvoiceItem2)
                    .WithPurchaseInvoiceItem(purchaseInvoiceItem3)
                    .WithCustomerReference("a reference number")
                    .WithDescription("Purchase of 1 used Aircraft Towbar")
                    .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                    .WithVatRegime(new VatRegimes(this.Session).Assessable)
                    .Build();
            }

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

        private Organisation SetupInternalOrganisation(
            string name,
            string address,
            string postalCode,
            string locality,
            Country country,
            string countryCode,
            string phone,
            string emailAddress,
            string websiteAddress,
            string taxNumber,
            string bankName,
            string facilityName,
            string bic,
            string iban,
            Currency currency,
            string logo,
            string requestNumberPrefix,
            string quoteNumberPrefix,
            string articleNumberPrefix,
            int? requestCounterValue,
            int? quoteCounterValue)
        {
            var postalAddress1 = new PostalAddressBuilder(this.Session)
                    .WithAddress1(address)
                    .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithPostalCode(postalCode).WithLocality(locality).WithCountry(country).Build())
                    .Build();

            TelecommunicationsNumber phoneNumber = null;
            if (!string.IsNullOrEmpty(phone))
            {
                phoneNumber = new TelecommunicationsNumberBuilder(this.Session).WithCountryCode(countryCode).WithContactNumber(phone).Build();
            }

            var email = new EmailAddressBuilder(this.Session)
                .WithElectronicAddressString(emailAddress)
                .Build();

            var webSite = new WebAddressBuilder(this.Session)
                .WithElectronicAddressString(websiteAddress)
                .Build();

            var bank = new BankBuilder(this.Session).WithName(bankName).WithBic(bic).WithCountry(country).Build();
            var bankaccount = new BankAccountBuilder(this.Session)
                .WithBank(bank)
                .WithIban(iban)
                .WithNameOnAccount(name)
                .WithCurrency(currency)
                .Build();

            var organisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithTaxNumber(taxNumber)
                .WithName(name)
                .WithBankAccount(bankaccount)
                .WithDefaultCollectionMethod(new OwnBankAccountBuilder(this.Session).WithBankAccount(bankaccount).WithDescription("Huisbank").Build())
                .WithPreferredCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithInvoiceSequence(new InvoiceSequences(this.Session).EnforcedSequence)
                .WithFiscalYearStartMonth(01)
                .WithFiscalYearStartDay(01)
                .WithDoAccounting(false)
                .WithRequestNumberPrefix(requestNumberPrefix)
                .WithQuoteNumberPrefix(quoteNumberPrefix)
                .WithInternetAddress(webSite)
                .Build();

            if (requestCounterValue != null)
            {
                organisation.RequestCounter = new CounterBuilder(this.Session).WithValue(requestCounterValue).Build();
            }

            if (quoteCounterValue != null)
            {
                organisation.QuoteCounter = new CounterBuilder(this.Session).WithValue(quoteCounterValue).Build();
            }

            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(email)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralEmail)
                .Build());
            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(postalAddress1)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralCorrespondence)
                .Build());

            if (phoneNumber != null)
            { 
                organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(phoneNumber)
                    .WithContactPurpose(new ContactMechanismPurposes(this.Session).SalesOffice)
                    .Build());
            }

            if (File.Exists(logo))
            {
                var fileInfo = new FileInfo(logo);

                var fileName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName).ToLowerInvariant();
                var content = File.ReadAllBytes(fileInfo.FullName);
                var image = new MediaBuilder(this.Session).WithFileName(fileName).WithInData(content).Build();
                organisation.LogoImage = image;
            }

            var magazijn = new FacilityBuilder(this.Session)
                .WithName(facilityName)
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithOwner(organisation)
                .Build();

            organisation.DefaultFacility = magazijn;

            var paymentMethod = new OwnBankAccountBuilder(this.Session).WithBankAccount(bankaccount).WithDescription("Hoofdbank").Build();

            new StoreBuilder(this.Session)
                .WithName($"{organisation.Name} store")
                .WithOutgoingShipmentNumberPrefix("shipmentno: ")
                .WithSalesInvoiceNumberPrefix("invoiceno: ")
                .WithSalesOrderNumberPrefix("orderno: ")
                .WithDefaultCollectionMethod(paymentMethod)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithBillingProcess(new BillingProcesses(this.Session).BillingForShipmentItems)
                .WithSalesInvoiceCounter(new CounterBuilder(this.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build())
                .WithIsImmediatelyPicked(true)
                .WithInternalOrganisation(organisation)
                .Build();

            return organisation;
        }
    }
}
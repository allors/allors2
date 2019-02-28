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
                facilityName: "Allors Warehouse 1",
                bic: "BBRUBEBB",
                iban: "BE89 3200 1467 7685",
                currency: euro,
                logo: allorsLogo,
                storeName: "Allors Store",
                billingProcess: new BillingProcesses(this.Session).BillingForOrderItems,
                outgoingShipmentNumberPrefix: "a-CS",
                salesInvoiceNumberPrefix: "a-SI",
                salesOrderNumberPrefix: "a-SO",
                requestNumberPrefix: "a-RFQ",
                quoteNumberPrefix: "a-Q",
                productNumberPrefix: "A-",
                requestCounterValue: 1,
                quoteCounterValue: 1,
                orderCounterValue: 1,
                invoiceCounterValue: 1);

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
                requestNumberPrefix: "d-RFQ",
                quoteNumberPrefix: "d-Q",
                productNumberPrefix: "D-",
                requestCounterValue: 1,
                quoteCounterValue: 1,
                orderCounterValue: 1,
                invoiceCounterValue: 1);

            singleton.Settings.DefaultFacility = allors.FacilitiesWhereOwner.First;

            this.SetupUser(allors, "employee1@allors.com", "first", "allors employee", "letmein");
            this.SetupUser(dipu, "employee1@allors.com", "first", "dipu employee", "letmein");

            new FacilityBuilder(this.Session)
                .WithName("Allors warehouse 2")
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithOwner(allors)
                .Build();

            var manufacturer = new OrganisationBuilder(this.Session).WithName("Gizmo inc.").WithIsManufacturer(true).Build();

            var productType = new ProductTypeBuilder(this.Session)
                .WithName($"Gizmo")
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

            var brand = new BrandBuilder(this.Session)
                .WithName("brand1")
                .WithModel(new ModelBuilder(this.Session).WithName("model1").Build())
                .Build();

            var finishedGood = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new SkuIdentificationBuilder(this.Session)
                    .WithIdentification("10101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Sku).Build())
                .WithName("finished good")
                .WithBrand(brand)
                .WithModel(brand.Models[0])
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithManufacturedBy(manufacturer)
                .Build();

            var good1 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("G1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithName("Tiny blue round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zeer kleine blauwe ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect blue with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Perfect blauw met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate)
                .WithPart(finishedGood)
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(finishedGood)
                .WithFacility(allors.FacilitiesWhereOwner.First)
                .WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).Unknown)
                .Build();

            var finishedGood2 = new NonUnifiedPartBuilder(this.Session)
                .WithName("finished good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                .WithProductType(productType)
                .WithProductIdentification(new SkuIdentificationBuilder(this.Session)
                    .WithIdentification("10102")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Sku).Build())
                .WithManufacturedBy(manufacturer)
                .Build();

            var good2 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("G2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithName("Tiny red round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zeer kleine rode ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect red with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Perfect rood met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate)
                .WithPart(finishedGood2)
                .Build();

            var serialisedItem = new SerialisedItemBuilder(this.Session).WithSerialNumber("1").WithAvailableForSale(true).Build();
            finishedGood2.AddSerialisedItem(serialisedItem);

            new InventoryItemTransactionBuilder(this.Session)
                .WithSerialisedItem(serialisedItem)
                .WithFacility(allors.FacilitiesWhereOwner.First)
                .WithQuantity(1)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithSerialisedInventoryItemState(new SerialisedInventoryItemStates(this.Session).Available)
                .Build();

            var finishedGood3 = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new SkuIdentificationBuilder(this.Session)
                    .WithIdentification("10103")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Sku).Build())
                .WithName("finished good3")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithManufacturedBy(manufacturer)
                .Build();

            var good3 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("G3")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithName("Tiny green round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zeer kleine groene ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect red with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Perfect groen met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate)
                .WithPart(finishedGood3)
                .Build();

            var finishedGood4 = new NonUnifiedPartBuilder(this.Session)
                .WithName("finished good4")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                .WithProductType(productType)
                .WithManufacturedBy(manufacturer)
                .Build();

            var good4 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("G4")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithName("Tiny purple round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zeer kleine paarse ronde gizmo").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate)
                .WithPart(finishedGood4)
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
                .WithProduct(good4)
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
                    .WithTaxNumber($"{1000 + i}-{1000 + i}-{1000 + i}")
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
                    .WithDescription($"Meeting {i}")
                    .WithSubject($"meeting {i}")
                    .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                    .WithFromParty(contact1)
                    .WithToParty(contact2)
                    .WithOwner(administrator)
                    .WithActualStart(DateTime.UtcNow)
                    .Build();

                new EmailCommunicationBuilder(this.Session)
                    .WithDescription($"Email {i}")
                    .WithSubject($"email {i}")
                    .WithFromEmail(email2)
                    .WithToEmail(email2)
                    .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(DateTime.UtcNow)
                    .Build();

                new LetterCorrespondenceBuilder(this.Session)
                    .WithDescription($"Letter {i}")
                    .WithSubject($"letter {i}")
                    .WithFromParty(administrator)
                    .WithToParty(contact1)
                    .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(DateTime.UtcNow)
                    .Build();

                new PhoneCommunicationBuilder(this.Session)
                    .WithDescription($"Phone {i}")
                    .WithSubject($"phone {i}")
                    .WithFromParty(administrator)
                    .WithToParty(contact1)
                    .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(DateTime.UtcNow)
                    .Build();

                var requestForQuote = new RequestForQuoteBuilder(this.Session)
                    .WithEmailAddress($"customer{i}@acme.com")
                    .WithTelephoneNumber("+1 234 56789")
                    .WithRecipient(allors)
                    .Build();

                var requestItem = new RequestItemBuilder(this.Session)
                    .WithSerialisedItem(serialisedItem)
                    .WithProduct(serialisedItem.PartWhereSerialisedItem.NonUnifiedGoodsWherePart.FirstOrDefault())
                    .WithComment($"Comment {i}")
                    .WithQuantity(1)
                    .Build();

                requestForQuote.AddRequestItem(requestItem);

                var productQuote = new ProductQuoteBuilder(this.Session)
                    .WithIssuer(allors)
                    .WithReceiver(acme)
                    .WithFullfillContactMechanism(acmePostalAddress)
                    .Build();

                var quoteItem = new QuoteItemBuilder(this.Session)
                    .WithSerialisedItem(serialisedItem)
                    .WithProduct(serialisedItem.PartWhereSerialisedItem.NonUnifiedGoodsWherePart.FirstOrDefault())
                    .WithComment($"Comment {i}")
                    .WithQuantity(1)
                    .Build();

                productQuote.AddQuoteItem(quoteItem);

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

                var exw = new IncoTermTypes(this.Session).Exw;
                var incoTerm = new IncoTermBuilder(this.Session).WithTermType(exw).WithTermValue("XW").Build();

                var salesInvoice = new SalesInvoiceBuilder(this.Session)
                    .WithBilledFrom(allors)
                    .WithBillToCustomer(acme)
                    .WithBillToContactPerson(contact1)
                    .WithBillToContactMechanism(acme.PartyContactMechanisms[0].ContactMechanism)
                    .WithBillToEndCustomerContactMechanism(acmeBillingAddress.ContactMechanism)
                    .WithShipToContactPerson(contact2)
                    .WithSalesInvoiceItem(salesInvoiceItem1)
                    .WithSalesInvoiceItem(salesInvoiceItem2)
                    .WithSalesInvoiceItem(salesInvoiceItem3)
                    .WithCustomerReference("a reference number")
                    .WithDescription("Sale of 1 used Aircraft Towbar")
                    .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                    .WithSalesTerm(incoTerm)
                    .WithVatRegime(new VatRegimes(this.Session).Assessable)
                    .Build();

                for (var j = 0; j < 30; j++)
                {
                    var salesInvoiceItem = new SalesInvoiceItemBuilder(this.Session)
                        .WithDescription("Extra Charge")
                        .WithActualUnitPrice(100 + j)
                        .WithQuantity(j)
                        .WithInvoiceItemType(new InvoiceItemTypes(this.Session).MiscCharge)
                        .Build();

                    salesInvoice.AddSalesInvoiceItem(salesInvoiceItem);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                var supplierPostalAddress = new PostalAddressBuilder(this.Session)
                    .WithAddress1($"Supplier{i} address 1")
                    .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality($"Supplier{i} city").WithPostalCode("1111").WithCountry(us).Build())
                    .Build();

                var supplierBillingAddress = new PartyContactMechanismBuilder(this.Session)
                    .WithContactMechanism(supplierPostalAddress)
                    .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralCorrespondence)
                    .WithUseAsDefault(true)
                    .Build();

                var supplier = new OrganisationBuilder(this.Session)
                    .WithName($"Supplier{i}")
                    .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                    .WithPartyContactMechanism(supplierBillingAddress)
                    .Build();

                new SupplierRelationshipBuilder(this.Session)
                    .WithSupplier(supplier)
                    .WithInternalOrganisation(allors)
                    .WithFromDate(DateTime.UtcNow)
                    .Build();

                var purchaseInvoiceItem1 = new PurchaseInvoiceItemBuilder(this.Session)
                    .WithDescription("first item")
                    .WithPart(finishedGood)
                    .WithActualUnitPrice(3000)
                    .WithQuantity(1)
                    .WithMessage(@"line1
line2")
                    .WithInvoiceItemType(new InvoiceItemTypes(this.Session).PartItem)
                    .Build();

                var purchaseInvoiceItem2 = new PurchaseInvoiceItemBuilder(this.Session)
                    .WithDescription("second item")
                    .WithActualUnitPrice(2000)
                    .WithQuantity(2)
                    .WithPart(finishedGood2)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.Session).PartItem)
                    .Build();

                var purchaseInvoiceItem3 = new PurchaseInvoiceItemBuilder(this.Session)
                    .WithDescription("Fee")
                    .WithActualUnitPrice(100)
                    .WithQuantity(1)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Fee)
                    .Build();

                var purchaseInvoice = new PurchaseInvoiceBuilder(this.Session)
                    .WithBilledTo(allors)
                    .WithBilledFrom(supplier)
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
                    .WithPart(finishedGood)
                    .WithQuantityOrdered(1)
                    .Build();

                var purchaseOrder = new PurchaseOrderBuilder(this.Session)
                    .WithOrderedBy(allors)
                    .WithTakenViaSupplier(supplier)
                    .WithPurchaseOrderItem(purchaseOrderItem1)
                    .WithCustomerReference("a reference number")
                    .Build();
            }

            var acme0 = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme0");

            var item = new SerialisedItemBuilder(this.Session)
                .WithSerialNumber("112")
                .WithSerialisedItemState(new SerialisedItemStates(this.Session).Sold)
                .WithAvailableForSale(false)
                .WithOwnedBy(acme0)
                .Build();

            finishedGood2.AddSerialisedItem(item);

            var worktask = new WorkTaskBuilder(this.Session)
                .WithTakenBy(allors)
                .WithCustomer(new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme0"))
                .WithName("maintenance")
                .Build();

            new WorkEffortFixedAssetAssignmentBuilder(this.Session)
                .WithFixedAsset(item)
                .WithAssignment(worktask)
                .Build();

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

        private void SetupUser(Organisation organisation, string email, string firstName, string lastName, string password)
        {
            var userEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString(email).Build();

            var be = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");

            var postalAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1($"{firstName} address")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality($"Mechelen").WithPostalCode("2800").WithCountry(be).Build())
                .Build();

            var generalCorrespondence = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(postalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralCorrespondence)
                .WithUseAsDefault(true)
                .Build();

            var person = new PersonBuilder(this.Session)
                .WithUserName(email)
                .WithFirstName(firstName)
                .WithLastName(lastName)
                .WithUserEmail(userEmail.ElectronicAddressString)
                .WithUserEmailConfirmed(true)
                .WithPartyContactMechanism(generalCorrespondence)
                .Build();

            person.AddPartyContactMechanism(
                new PartyContactMechanismBuilder(this.Session)
                    .WithContactMechanism(userEmail)
                    .WithContactPurpose(new ContactMechanismPurposes(this.Session).PersonalEmailAddress)
                    .WithUseAsDefault(true)
                    .Build());

            new EmploymentBuilder(this.Session).WithEmployee(person).WithEmployer(organisation).Build();

            new OrganisationContactRelationshipBuilder(this.Session)
                .WithOrganisation(organisation)
                .WithContact(person)
                .WithContactKind(new OrganisationContactKinds(this.Session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new UserGroups(this.Session).Administrators.AddMember(person);
            new UserGroups(this.Session).Creators.AddMember(person);

            person.SetPassword(password);
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Setup.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

using System;
using System.Linq;
using Allors.Domain;
using Allors.Meta;

namespace Allors
{
    public partial class Setup
    {
        private void CustomOnPrePrepare()
        {
        }

        private void CustomOnPostPrepare()
        {
        }

        private void CustomOnPreSetup()
        {
        }

        private void CustomOnPostSetup()
        {
            if (this.Config.Demo || this.Config.End2End)
            {
                this.Full();
            }
        }

        private void Full()
        {
            var singleton = this.session.GetSingleton();
            var dutchLocale = new Locales(this.session).DutchNetherlands;
            singleton.AddAdditionalLocale(dutchLocale);

            var administrator = (Person)new UserGroups(this.session).Administrators.Members.First;

            var euro = new Currencies(this.session).FindBy(M.Currency.IsoCode, "EUR");

            var be = new Countries(this.session).FindBy(M.Country.IsoCode, "BE");
            var us = new Countries(this.session).FindBy(M.Country.IsoCode, "US");

            var email2 = new EmailAddressBuilder(this.session)
                .WithElectronicAddressString("recipient@acme.com")
                .Build();

            var allorsLogo = this.Config.DataPath + @"\www\admin\images\logo.png";

            var allors = Organisations.CreateInternalOrganisation(
                session: this.session,
                name: "Allors BVBA",
                address: "Kleine Nieuwedijkstraat 4",
                postalCode: "2800",
                locality: "Mechelen",
                country: be,
                phone1CountryCode: "+32",
                phone1: "2 335 2335",
                phone1Purpose: new ContactMechanismPurposes(this.session).GeneralPhoneNumber,
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
                billingProcess: new BillingProcesses(this.session).BillingForOrderItems,
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
                isAutomaticallyShipped: true,
                useCreditNoteSequence: true,
                requestCounterValue: 1,
                quoteCounterValue: 1,
                orderCounterValue: 1,
                purchaseOrderCounterValue: 1,
                invoiceCounterValue: 1,
                purchaseInvoiceCounterValue: 1,
                purchaseOrderNeedsApproval: true,
                purchaseOrderApprovalThresholdLevel1: 1000M,
                purchaseOrderApprovalThresholdLevel2: 5000M);

            var dipu = Organisations.CreateInternalOrganisation(
                session: this.session,
                name: "Dipu BVBA",
                address: "Kleine Nieuwedijkstraat 2",
                postalCode: "2800",
                locality: "Mechelen",
                country: be,
                phone1CountryCode: "+32",
                phone1: "2 15 49 49 49",
                phone1Purpose: new ContactMechanismPurposes(this.session).GeneralPhoneNumber,
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
                billingProcess: new BillingProcesses(this.session).BillingForOrderItems,
                outgoingShipmentNumberPrefix: "d-CS",
                salesInvoiceNumberPrefix: "d-SI",
                salesOrderNumberPrefix: "d-SO",
                purchaseOrderNumberPrefix: "d-PO",
                purchaseInvoiceNumberPrefix: "d-PI",
                requestNumberPrefix: "d-RFQ",
                quoteNumberPrefix: "d-Q",
                productNumberPrefix: "D-",
                workEffortPrefix: "a-WO-",
                creditNoteNumberPrefix: "d-CN-",
                isImmediatelyPicked: true,
                isAutomaticallyShipped: true,
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


            // Give Administrator access
            new EmploymentBuilder(this.session).WithEmployee(administrator).WithEmployer(allors).Build();
            allors.AddProductQuoteApprover(administrator);
            allors.AddBlueCollarWorker(administrator);

            singleton.Settings.DefaultFacility = allors.FacilitiesWhereOwner.First;

            var allorsEmployee1 = this.CreatePerson(allors, "employee1@allors.com", "Allors", "Employee 1", "letmein");
            var allorsEmployee2 = this.CreatePerson(allors, "employee2@allors.com", "Allors", "Employee 2", "letmein");
            var allorsProductQuoteApprover = this.CreatePerson(allors, "productQuoteApprover@allors.com", "Allors", "ProductQuoteApprover", "letmein");
            var allorsPurchaseInvoiceApprover = this.CreatePerson(allors, "purchaseInvoiceApprover@allors.com", "Allors", "PurchaseInvoiceApprover", "letmein");
            var allorsPurchaseOrderApproverLevel1 = this.CreatePerson(allors, "purchaseOrderApproverLevel1@allors.com", "Allors", "PurchaseOrderApproverLevel1", "letmein");
            var allorsPurchaseOrderApproverLevel2 = this.CreatePerson(allors, "purchaseOrderApproverLevel2@allors.com", "Allors", "PurchaseOrderApproverLevel2", "letmein");

            allors.ProductQuoteApprovers = new[] { allorsProductQuoteApprover, administrator };
            allors.PurchaseInvoiceApprovers = new[] { allorsPurchaseInvoiceApprover, administrator };
            allors.PurchaseOrderApproversLevel1 = new[] { allorsPurchaseOrderApproverLevel1, administrator };
            allors.PurchaseOrderApproversLevel2 = new[] { allorsPurchaseOrderApproverLevel2, administrator };

            var dipuEmployee = this.CreatePerson(dipu, "employee@dipu.com", "first", "dipu employee", "letmein");
            var dipuProductQuoteApprover = this.CreatePerson(allors, "productQuoteApprover@dipu.com", "Dipu", "ProductQuoteApprover", "letmein");
            var dipuPurchaseInvoiceApprover = this.CreatePerson(allors, "purchaseInvoiceApprover@allors.com", "dipu", "PurchaseInvoiceApprover", "letmein");

            dipu.ProductQuoteApprovers = new[] { dipuProductQuoteApprover, administrator };
            dipu.PurchaseInvoiceApprovers = new[] { dipuPurchaseInvoiceApprover, administrator };

            new FacilityBuilder(this.session)
                .WithName("Allors warehouse 2")
                .WithFacilityType(new FacilityTypes(this.session).Warehouse)
                .WithOwner(allors)
                .Build();

            var manufacturer = new OrganisationBuilder(this.session).WithName("Gizmo inc.").WithIsManufacturer(true).Build();

            var productType = new ProductTypeBuilder(this.session)
                .WithName($"Gizmo")
                .WithSerialisedItemCharacteristicType(new SerialisedItemCharacteristicTypeBuilder(this.session)
                                            .WithName("Size")
                                            .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Afmeting").WithLocale(dutchLocale).Build())
                                            .Build())
                .WithSerialisedItemCharacteristicType(new SerialisedItemCharacteristicTypeBuilder(this.session)
                                            .WithName("Weight")
                                            .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Gewicht").WithLocale(dutchLocale).Build())
                                            .WithUnitOfMeasure(new UnitsOfMeasure(this.session).Kilogram)
                                            .Build())
                .Build();

            var vatRate = new VatRateBuilder(this.session).WithRate(21).Build();

            var brand = new BrandBuilder(this.session)
                .WithName("brand1")
                .WithModel(new ModelBuilder(this.session).WithName("model1").Build())
                .Build();

            var finishedGood = new NonUnifiedPartBuilder(this.session)
                .WithProductIdentification(new SkuIdentificationBuilder(this.session)
                    .WithIdentification("10101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.session).Sku).Build())
                .WithName("finished good")
                .WithBrand(brand)
                .WithModel(brand.Models[0])
                .WithInventoryItemKind(new InventoryItemKinds(this.session).NonSerialised)
                .WithManufacturedBy(manufacturer)
                .Build();

            var good1 = new NonUnifiedGoodBuilder(this.session)
                .WithProductIdentification(new ProductNumberBuilder(this.session)
                    .WithIdentification("G1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.session).Good).Build())
                .WithName("Tiny blue round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Zeer kleine blauwe ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect blue with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.session).WithText("Perfect blauw met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate)
                .WithPart(finishedGood)
                .Build();

            new InventoryItemTransactionBuilder(this.session)
                .WithPart(finishedGood)
                .WithFacility(allors.FacilitiesWhereOwner.First)
                .WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.session).Unknown)
                .Build();

            var finishedGood2 = new NonUnifiedPartBuilder(this.session)
                .WithName("finished good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.session).Serialised)
                .WithProductType(productType)
                .WithProductIdentification(new SkuIdentificationBuilder(this.session)
                    .WithIdentification("10102")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.session).Sku).Build())
                .WithManufacturedBy(manufacturer)
                .Build();

            var good2 = new NonUnifiedGoodBuilder(this.session)
                .WithProductIdentification(new ProductNumberBuilder(this.session)
                    .WithIdentification("G2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.session).Good).Build())
                .WithName("Tiny red round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Zeer kleine rode ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect red with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.session).WithText("Perfect rood met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate)
                .WithPart(finishedGood2)
                .Build();

            var serialisedItem = new SerialisedItemBuilder(this.session).WithSerialNumber("1").WithAvailableForSale(true).WithOwnedBy(allors).Build();
            finishedGood2.AddSerialisedItem(serialisedItem);

            new InventoryItemTransactionBuilder(this.session)
                .WithSerialisedItem(serialisedItem)
                .WithFacility(allors.FacilitiesWhereOwner.First)
                .WithQuantity(1)
                .WithReason(new InventoryTransactionReasons(this.session).IncomingShipment)
                .WithSerialisedInventoryItemState(new SerialisedInventoryItemStates(this.session).Available)
                .Build();

            var finishedGood3 = new NonUnifiedPartBuilder(this.session)
                .WithProductIdentification(new SkuIdentificationBuilder(this.session)
                    .WithIdentification("10103")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.session).Sku).Build())
                .WithName("finished good3")
                .WithInventoryItemKind(new InventoryItemKinds(this.session).NonSerialised)
                .WithManufacturedBy(manufacturer)
                .Build();

            var good3 = new NonUnifiedGoodBuilder(this.session)
                .WithProductIdentification(new ProductNumberBuilder(this.session)
                    .WithIdentification("G3")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.session).Good).Build())
                .WithName("Tiny green round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Zeer kleine groene ronde gizmo").WithLocale(dutchLocale).Build())
                .WithDescription("Perfect red with nice curves")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.session).WithText("Perfect groen met mooie rondingen").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate)
                .WithPart(finishedGood3)
                .Build();

            var finishedGood4 = new NonUnifiedPartBuilder(this.session)
                .WithName("finished good4")
                .WithInventoryItemKind(new InventoryItemKinds(this.session).Serialised)
                .WithProductType(productType)
                .WithManufacturedBy(manufacturer)
                .Build();

            var good4 = new NonUnifiedGoodBuilder(this.session)
                .WithProductIdentification(new ProductNumberBuilder(this.session)
                    .WithIdentification("G4")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.session).Good).Build())
                .WithName("Tiny purple round gizmo")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Zeer kleine paarse ronde gizmo").WithLocale(dutchLocale).Build())
                .WithVatRate(vatRate)
                .WithPart(finishedGood4)
                .Build();

            var productCategory1 = new ProductCategoryBuilder(this.session)
                .WithInternalOrganisation(allors)
                .WithName("Best selling gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Meest verkochte gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory2 = new ProductCategoryBuilder(this.session)
                .WithInternalOrganisation(allors)
                .WithName("Big Gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Grote Gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory3 = new ProductCategoryBuilder(this.session)
                .WithInternalOrganisation(allors)
                .WithName("Small gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Kleine gizmo's").WithLocale(dutchLocale).Build())
                .WithProduct(good1)
                .WithProduct(good2)
                .WithProduct(good3)
                .WithProduct(good4)
                .Build();

            new CatalogueBuilder(this.session)
                .WithInternalOrganisation(allors)
                .WithName("New gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.session).WithText("Nieuwe gizmo's").WithLocale(dutchLocale).Build())
                .WithDescription("Latest in the world of Gizmo's")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.session).WithText("Laatste in de wereld van Gizmo's").WithLocale(dutchLocale).Build())
                .WithProductCategory(productCategory1)
                .Build();

            this.session.Derive();

            for (int i = 0; i < 100; i++)
            {
                var acmePostalAddress = new PostalAddressBuilder(this.session)
                    .WithAddress1($"Acme{i} address 1")
                    .WithLocality($"Acme{i} city")
                    .WithPostalCode("1111")
                    .WithCountry(us)
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
                    .WithName($"Acme{i}")
                    .WithLocale(new Locales(this.session).EnglishUnitedStates)
                    .WithPartyContactMechanism(acmeBillingAddress)
                    .WithPartyContactMechanism(acmeInquiries)
                    .WithTaxNumber($"{1000 + i}-{1000 + i}-{1000 + i}")
                    .Build();

                var contact1Email = new PartyContactMechanismBuilder(this.session)
                    .WithContactMechanism(new EmailAddressBuilder(this.session).WithElectronicAddressString($"employee1@acme{i}.com").Build())
                    .WithContactPurpose(new ContactMechanismPurposes(this.session).PersonalEmailAddress)
                    .WithUseAsDefault(true)
                    .Build();

                var contact2PhoneNumber = new PartyContactMechanismBuilder(this.session)
                    .WithContactMechanism(new TelecommunicationsNumberBuilder(this.session).WithCountryCode("+1").WithAreaCode("123").WithContactNumber("456").Build())
                    .WithContactPurpose(new ContactMechanismPurposes(this.session).GeneralPhoneNumber)
                    .WithUseAsDefault(true)
                    .Build();

                var contact1 = new PersonBuilder(this.session)
                    .WithFirstName($"John{i}")
                    .WithLastName($"Doe{i}")
                    .WithGender(new GenderTypes(this.session).Male)
                    .WithLocale(new Locales(this.session).EnglishUnitedStates)
                    .WithPartyContactMechanism(contact1Email)
                    .Build();

                var contact2 = new PersonBuilder(this.session)
                    .WithFirstName($"Jane{i}")
                    .WithLastName($"Doe{i}")
                    .WithGender(new GenderTypes(this.session).Male)
                    .WithLocale(new Locales(this.session).EnglishUnitedStates)
                    .WithPartyContactMechanism(contact2PhoneNumber)
                    .Build();

                new CustomerRelationshipBuilder(this.session)
                    .WithCustomer(acme)
                    .WithInternalOrganisation(allors)
                    .WithFromDate(this.session.Now())
                    .Build();

                new OrganisationContactRelationshipBuilder(this.session)
                    .WithOrganisation(acme)
                    .WithContact(contact1)
                    .WithContactKind(new OrganisationContactKinds(this.session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                    .WithFromDate(this.session.Now())
                    .Build();

                new OrganisationContactRelationshipBuilder(this.session)
                    .WithOrganisation(acme)
                    .WithContact(contact2)
                    .WithContactKind(new OrganisationContactKinds(this.session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                    .WithFromDate(this.session.Now())
                    .Build();

                new FaceToFaceCommunicationBuilder(this.session)
                    .WithDescription($"Meeting {i}")
                    .WithSubject($"meeting {i}")
                    .WithEventPurpose(new CommunicationEventPurposes(this.session).Meeting)
                    .WithFromParty(contact1)
                    .WithToParty(contact2)
                    .WithOwner(administrator)
                    .WithActualStart(this.session.Now())
                    .Build();

                new EmailCommunicationBuilder(this.session)
                    .WithDescription($"Email {i}")
                    .WithSubject($"email {i}")
                    .WithFromEmail(email2)
                    .WithToEmail(email2)
                    .WithEventPurpose(new CommunicationEventPurposes(this.session).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(this.session.Now())
                    .Build();

                new LetterCorrespondenceBuilder(this.session)
                    .WithDescription($"Letter {i}")
                    .WithSubject($"letter {i}")
                    .WithFromParty(administrator)
                    .WithToParty(contact1)
                    .WithEventPurpose(new CommunicationEventPurposes(this.session).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(this.session.Now())
                    .Build();

                new PhoneCommunicationBuilder(this.session)
                    .WithDescription($"Phone {i}")
                    .WithSubject($"phone {i}")
                    .WithFromParty(administrator)
                    .WithToParty(contact1)
                    .WithEventPurpose(new CommunicationEventPurposes(this.session).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(this.session.Now())
                    .Build();

                var requestForQuote = new RequestForQuoteBuilder(this.session)
                    .WithEmailAddress($"customer{i}@acme.com")
                    .WithTelephoneNumber("+1 234 56789")
                    .WithRecipient(allors)
                    .Build();

                var requestItem = new RequestItemBuilder(this.session)
                    .WithSerialisedItem(serialisedItem)
                    .WithProduct(serialisedItem.PartWhereSerialisedItem.NonUnifiedGoodsWherePart.FirstOrDefault())
                    .WithComment($"Comment {i}")
                    .WithQuantity(1)
                    .Build();

                requestForQuote.AddRequestItem(requestItem);

                var productQuote = new ProductQuoteBuilder(this.session)
                    .WithIssuer(allors)
                    .WithReceiver(acme)
                    .WithFullfillContactMechanism(acmePostalAddress)
                    .Build();

                var quoteItem = new QuoteItemBuilder(this.session)
                    .WithSerialisedItem(serialisedItem)
                    .WithProduct(serialisedItem.PartWhereSerialisedItem.NonUnifiedGoodsWherePart.FirstOrDefault())
                    .WithComment($"Comment {i}")
                    .WithQuantity(1)
                    .WithAssignedUnitPrice(100)
                    .Build();

                productQuote.AddQuoteItem(quoteItem);

                var salesOrderItem1 = new SalesOrderItemBuilder(this.session)
                    .WithDescription("first item")
                    .WithProduct(good1)
                    .WithAssignedUnitPrice(3000)
                    .WithQuantityOrdered(1)
                    .WithMessage(@"line1
line2")
                    .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                    .Build();

                var salesOrderItem2 = new SalesOrderItemBuilder(this.session)
                    .WithDescription("second item")
                    .WithAssignedUnitPrice(2000)
                    .WithQuantityOrdered(2)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                    .Build();

                var salesOrderItem3 = new SalesOrderItemBuilder(this.session)
                    .WithDescription("Fee")
                    .WithAssignedUnitPrice(100)
                    .WithQuantityOrdered(1)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.session).Fee)
                    .Build();

                var order = new SalesOrderBuilder(this.session)
                    .WithTakenBy(allors)
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
                    .WithAssignedUnitPrice(3000)
                    .WithQuantity(1)
                    .WithMessage(@"line1
line2")
                    .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                    .Build();

                var salesInvoiceItem2 = new SalesInvoiceItemBuilder(this.session)
                    .WithDescription("second item")
                    .WithAssignedUnitPrice(2000)
                    .WithQuantity(2)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.session).ProductItem)
                    .Build();

                var salesInvoiceItem3 = new SalesInvoiceItemBuilder(this.session)
                    .WithDescription("Fee")
                    .WithAssignedUnitPrice(100)
                    .WithQuantity(1)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.session).Fee)
                    .Build();

                var exw = new IncoTermTypes(this.session).Exw;
                var incoTerm = new IncoTermBuilder(this.session).WithTermType(exw).WithTermValue("XW").Build();

                var salesInvoice = new SalesInvoiceBuilder(this.session)
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
                    .WithSalesInvoiceType(new SalesInvoiceTypes(this.session).SalesInvoice)
                    .WithSalesTerm(incoTerm)
                    .WithVatRegime(new VatRegimes(this.session).Assessable)
                    .Build();

                for (var j = 0; j < 30; j++)
                {
                    var salesInvoiceItem = new SalesInvoiceItemBuilder(this.session)
                        .WithDescription("Extra Charge")
                        .WithAssignedUnitPrice(100 + j)
                        .WithQuantity(j)
                        .WithInvoiceItemType(new InvoiceItemTypes(this.session).MiscCharge)
                        .Build();

                    salesInvoice.AddSalesInvoiceItem(salesInvoiceItem);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                var supplierPostalAddress = new PostalAddressBuilder(this.session)
                    .WithAddress1($"Supplier{i} address 1")
                    .WithLocality($"Supplier{i} city")
                    .WithPostalCode("1111")
                    .WithCountry(us)
                    .Build();

                var supplierBillingAddress = new PartyContactMechanismBuilder(this.session)
                    .WithContactMechanism(supplierPostalAddress)
                    .WithContactPurpose(new ContactMechanismPurposes(this.session).GeneralCorrespondence)
                    .WithUseAsDefault(true)
                    .Build();

                var supplier = new OrganisationBuilder(this.session)
                    .WithName($"Supplier{i}")
                    .WithLocale(new Locales(this.session).EnglishUnitedStates)
                    .WithPartyContactMechanism(supplierBillingAddress)
                    .Build();

                new SupplierRelationshipBuilder(this.session)
                    .WithSupplier(supplier)
                    .WithInternalOrganisation(allors)
                    .WithFromDate(this.session.Now())
                    .Build();

                var purchaseInvoiceItem1 = new PurchaseInvoiceItemBuilder(this.session)
                    .WithDescription("first item")
                    .WithPart(finishedGood)
                    .WithAssignedUnitPrice(3000)
                    .WithQuantity(1)
                    .WithMessage(@"line1
line2")
                    .WithInvoiceItemType(new InvoiceItemTypes(this.session).PartItem)
                    .Build();

                var purchaseInvoiceItem2 = new PurchaseInvoiceItemBuilder(this.session)
                    .WithDescription("second item")
                    .WithAssignedUnitPrice(2000)
                    .WithQuantity(2)
                    .WithPart(finishedGood2)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.session).PartItem)
                    .Build();

                var purchaseInvoiceItem3 = new PurchaseInvoiceItemBuilder(this.session)
                    .WithDescription("Fee")
                    .WithAssignedUnitPrice(100)
                    .WithQuantity(1)
                    .WithInvoiceItemType(new InvoiceItemTypes(this.session).Fee)
                    .Build();

                var purchaseInvoice = new PurchaseInvoiceBuilder(this.session)
                    .WithBilledTo(allors)
                    .WithBilledFrom(supplier)
                    .WithPurchaseInvoiceItem(purchaseInvoiceItem1)
                    .WithPurchaseInvoiceItem(purchaseInvoiceItem2)
                    .WithPurchaseInvoiceItem(purchaseInvoiceItem3)
                    .WithCustomerReference("a reference number")
                    .WithDescription("Purchase of 1 used Aircraft Towbar")
                    .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.session).PurchaseInvoice)
                    .WithVatRegime(new VatRegimes(this.session).Assessable)
                    .Build();

                var purchaseOrderItem1 = new PurchaseOrderItemBuilder(this.session)
                    .WithDescription("first purchase order item")
                    .WithPart(finishedGood)
                    .WithQuantityOrdered(1)
                    .Build();

                var purchaseOrder = new PurchaseOrderBuilder(this.session)
                    .WithOrderedBy(allors)
                    .WithTakenViaSupplier(supplier)
                    .WithPurchaseOrderItem(purchaseOrderItem1)
                    .WithCustomerReference("reference " + i)
                    .Build();
            }

            var acme0 = new Organisations(this.session).FindBy(M.Organisation.Name, "Acme0");

            var item = new SerialisedItemBuilder(this.session)
                .WithSerialNumber("112")
                .WithSerialisedItemState(new SerialisedItemStates(this.session).Sold)
                .WithAvailableForSale(false)
                .WithOwnedBy(acme0)
                .Build();

            finishedGood2.AddSerialisedItem(item);

            var workTask = new WorkTaskBuilder(this.session)
                .WithTakenBy(allors)
                .WithCustomer(new Organisations(this.session).FindBy(M.Organisation.Name, "Acme0"))
                .WithName("maintenance")
                .Build();

            new WorkEffortFixedAssetAssignmentBuilder(this.session)
                .WithFixedAsset(item)
                .WithAssignment(workTask)
                .Build();

            var part1 = this.CreatePart("P1");
            var part2 = this.CreatePart("P2");
            var part3 = this.CreatePart("P3");

            this.session.Derive();

            var workOrder = new WorkTaskBuilder(this.session)
                .WithName("Task")
                .WithTakenBy(allors)
                .WithFacility(new Facilities(this.session).Extent().First)
                .WithCustomer(acme0)
                .WithWorkEffortPurpose(new WorkEffortPurposes(this.session).Maintenance)
                .WithSpecialTerms("Net 45 Days")
                .Build();

            new WorkEffortFixedAssetAssignmentBuilder(this.session)
                .WithFixedAsset(item)
                .WithAssignment(workOrder)
                .WithComment("Busted tailpipe")
                .Build();

            this.CreateInventoryAssignment(workOrder, part1, 11);
            this.CreateInventoryAssignment(workOrder, part2, 12);
            this.CreateInventoryAssignment(workOrder, part3, 13);

            //// Work Effort Time Entries
            var yesterday = DateTimeFactory.CreateDateTime(this.session.Now().AddDays(-1));
            var laterYesterday = DateTimeFactory.CreateDateTime(yesterday.AddHours(3));

            var today = DateTimeFactory.CreateDateTime(this.session.Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(this.session.Now().AddDays(1));
            var laterTomorrow = DateTimeFactory.CreateDateTime(tomorrow.AddHours(6));

            var standardRate = new RateTypes(this.session).StandardRate;
            var overtimeRate = new RateTypes(this.session).OvertimeRate;

            var frequencies = new TimeFrequencies(this.session);

            var timeEntryYesterday1 = this.CreateTimeEntry(yesterday, laterYesterday, frequencies.Day, workOrder, standardRate);
            var timeEntryToday1 = this.CreateTimeEntry(today, laterToday, frequencies.Hour, workOrder, standardRate);
            var timeEntryTomorrow1 = this.CreateTimeEntry(tomorrow, laterTomorrow, frequencies.Minute, workOrder, overtimeRate);

            allorsEmployee1.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday1);
            allorsEmployee1.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday1);
            allorsEmployee1.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow1);

            var timeEntryYesterday2 = this.CreateTimeEntry(yesterday, laterYesterday, frequencies.Day, workOrder, standardRate);
            var timeEntryToday2 = this.CreateTimeEntry(today, laterToday, frequencies.Hour, workOrder, standardRate);
            var timeEntryTomorrow2 = this.CreateTimeEntry(tomorrow, laterTomorrow, frequencies.Minute, workOrder, overtimeRate);

            allorsEmployee2.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday2);
            allorsEmployee2.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday2);
            allorsEmployee2.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow2);

            var po = new PurchaseOrders(this.session).Extent().First;
            foreach (PurchaseOrderItem purchaseOrderItem in po.PurchaseOrderItems)
            {
                new WorkEffortPurchaseOrderItemAssignmentBuilder(this.session)
                    .WithPurchaseOrderItem(purchaseOrderItem)
                    .WithAssignment(workOrder)
                    .WithQuantity(1)
                    .Build();
            }

            this.session.Derive();
        }

        private Person CreatePerson(Organisation organisation, string email, string firstName, string lastName, string password)
        {
            var userEmail = new EmailAddressBuilder(this.session).WithElectronicAddressString(email).Build();

            var be = new Countries(this.session).FindBy(M.Country.IsoCode, "BE");

            var postalAddress = new PostalAddressBuilder(this.session)
                .WithAddress1($"{firstName} address")
                .WithLocality($"Mechelen")
                .WithPostalCode("2800")
                .WithCountry(be)
                .Build();

            var generalCorrespondence = new PartyContactMechanismBuilder(this.session)
                .WithContactMechanism(postalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.session).GeneralCorrespondence)
                .WithUseAsDefault(true)
                .Build();

            var person = new PersonBuilder(this.session)
                .WithUserName(email)
                .WithFirstName(firstName)
                .WithLastName(lastName)
                .WithUserEmail(userEmail.ElectronicAddressString)
                .WithUserEmailConfirmed(true)
                .WithPartyContactMechanism(generalCorrespondence)
                .Build();

            person.AddPartyContactMechanism(
                new PartyContactMechanismBuilder(this.session)
                    .WithContactMechanism(userEmail)
                    .WithContactPurpose(new ContactMechanismPurposes(this.session).PersonalEmailAddress)
                    .WithUseAsDefault(true)
                    .Build());

            new EmploymentBuilder(this.session).WithEmployee(person).WithEmployer(organisation).Build();

            new OrganisationContactRelationshipBuilder(this.session)
                .WithOrganisation(organisation)
                .WithContact(person)
                .WithContactKind(new OrganisationContactKinds(this.session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                .WithFromDate(this.session.Now())
                .Build();

            new UserGroups(this.session).Creators.AddMember(person);

            person.SetPassword(password);

            return person;
        }

        private Part CreatePart(string id) =>
            new NonUnifiedPartBuilder(this.session)
                .WithProductIdentification(new PartNumberBuilder(this.session)
                    .WithIdentification(id)
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.session).Part).Build())
                .Build();

        private WorkEffortInventoryAssignment CreateInventoryAssignment(WorkEffort workOrder, Part part, int quantity)
        {
            new InventoryItemTransactionBuilder(this.session)
                .WithPart(part)
                .WithReason(new InventoryTransactionReasons(this.session).IncomingShipment)
                .WithQuantity(quantity)
                .Build();

            return new WorkEffortInventoryAssignmentBuilder(this.session)
                .WithAssignment(workOrder)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(quantity)
                .Build();
        }

        private TimeEntry CreateTimeEntry(DateTime fromDate, DateTime throughDate, TimeFrequency frequency, WorkEffort workEffort, RateType rateType) =>
            new TimeEntryBuilder(this.session)
                .WithRateType(rateType)
                .WithFromDate(fromDate)
                .WithThroughDate(throughDate)
                .WithTimeFrequency(frequency)
                .WithWorkEffort(workEffort)
                .Build();
    }
}
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

            var serialisedItemSoldOns = new SerialisedItemSoldOn[] { new SerialisedItemSoldOns(this.Session).SalesInvoiceSend, new SerialisedItemSoldOns(this.Session).PurchaseInvoiceConfirm };

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
                purchaseOrderApprovalThresholdLevel2: 5000M,
                serialisedItemSoldOns: serialisedItemSoldOns,
                collectiveWorkEffortInvoice: true);

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
                purchaseOrderApprovalThresholdLevel2: null,
                serialisedItemSoldOns: serialisedItemSoldOns,
                collectiveWorkEffortInvoice: true);

            singleton.Settings.DefaultFacility = allors.FacilitiesWhereOwner.First;
            var faker = this.Session.Faker();

            allors.CreateEmployee("letmein", faker);
            allors.CreateAdministrator("letmein", faker);
            var allorsB2BCustomer = allors.CreateB2BCustomer(this.Session.Faker());
            var allorsB2CCustomer = allors.CreateB2CCustomer(this.Session.Faker());
            allors.CreateSupplier(this.Session.Faker());
            allors.CreateSubContractor(this.Session.Faker());

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(allorsB2BCustomer)
                .WithInternalOrganisation(dipu)
                .WithFromDate(this.Session.Now())
                .Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(allorsB2CCustomer)
                .WithInternalOrganisation(dipu)
                .WithFromDate(this.Session.Now())
                .Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(dipu)
                .WithInternalOrganisation(allors)
                .WithFromDate(this.Session.Now())
                .Build();

            dipu.CreateEmployee("letmein", faker);
            dipu.CreateAdministrator("letmein", faker);
            var dipuB2BCustomer = dipu.CreateB2BCustomer(this.Session.Faker());
            var dipuB2CCustomer = dipu.CreateB2CCustomer(this.Session.Faker());
            dipu.CreateSupplier(this.Session.Faker());
            dipu.CreateSubContractor(this.Session.Faker());

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(dipuB2BCustomer)
                .WithInternalOrganisation(allors)
                .WithFromDate(this.Session.Now())
                .Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(dipuB2CCustomer)
                .WithInternalOrganisation(allors)
                .WithFromDate(this.Session.Now())
                .Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(allors)
                .WithInternalOrganisation(dipu)
                .WithFromDate(this.Session.Now())
                .Build();

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

            var good_1 = new UnifiedGoodBuilder(this.Session).WithNonSerialisedDefaults(allors).Build();

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(good_1)
                .WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).Unknown)
                .Build();

            var good_2 = new NonUnifiedGoodBuilder(this.Session)
                .WithSerialisedPartDefaults(allors)
                .Build();

            var serialisedItem1 = new SerialisedItemBuilder(this.Session).WithDefaults(allors).Build();

            good_2.Part.AddSerialisedItem(serialisedItem1);

            this.Session.Derive();

            new SerialisedInventoryItemBuilder(this.Session)
                .WithPart(good_2.Part)
                .WithSerialisedItem(serialisedItem1)
                .WithFacility(allors.StoresWhereInternalOrganisation.First.DefaultFacility)
                .Build();

            var good_3 = new NonUnifiedGoodBuilder(this.Session)
                .WithNonSerialisedPartDefaults(allors)
                .Build();

            var good_4 = new UnifiedGoodBuilder(this.Session).WithSerialisedDefaults(allors).Build();

            var productCategory_1 = new ProductCategoryBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("Best selling gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Meest verkochte gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory_2 = new ProductCategoryBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("Big Gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Grote Gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory_3 = new ProductCategoryBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("Small gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kleine gizmo's").WithLocale(dutchLocale).Build())
                .WithProduct(good_1)
                .WithProduct(good_2)
                .WithProduct(good_3)
                .WithProduct(good_4)
                .Build();

            new CatalogueBuilder(this.Session)
                .WithInternalOrganisation(allors)
                .WithName("New gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Nieuwe gizmo's").WithLocale(dutchLocale).Build())
                .WithDescription("Latest in the world of Gizmo's")
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Laatste in de wereld van Gizmo's").WithLocale(dutchLocale).Build())
                .WithProductCategory(productCategory_1)
                .Build();

            this.Session.Derive();

            var administrator = (Person)new UserGroups(this.Session).Administrators.Members.First;

            new FaceToFaceCommunicationBuilder(this.Session)
                .WithDescription($"Meeting")
                .WithSubject($"meeting")
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithFromParty(allors.ActiveEmployees.First)
                .WithToParty(allors.ActiveCustomers.First)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new EmailCommunicationBuilder(this.Session)
                .WithDescription($"Email")
                .WithSubject($"email")
                .WithFromParty(allors.ActiveEmployees.First)
                .WithToParty(allors.ActiveCustomers.First)
                .WithFromEmail(allors.ActiveEmployees.First.GeneralEmail)
                .WithToEmail(allors.ActiveCustomers.First.GeneralEmail)
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new LetterCorrespondenceBuilder(this.Session)
                .WithDescription($"Letter")
                .WithSubject($"letter")
                .WithFromParty(administrator)
                .WithToParty(allors.ActiveCustomers.First)
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new PhoneCommunicationBuilder(this.Session)
                .WithDescription($"Phone")
                .WithSubject($"phone")
                .WithFromParty(administrator)
                .WithToParty(allors.ActiveCustomers.First)
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithOwner(administrator)
                .WithActualStart(DateTime.UtcNow)
                .Build();

            new SalesOrderBuilder(this.Session).WithOrganisationInternalDefaults(allors).Build();
            new SalesOrderBuilder(this.Session).WithOrganisationExternalDefaults(allors).Build();
            new SalesOrderBuilder(this.Session).WithPersonInternalDefaults(allors).Build();
            new SalesOrderBuilder(this.Session).WithPersonExternalDefaults(allors).Build();

            new SalesInvoiceBuilder(this.Session).WithSalesExternalB2BInvoiceDefaults(allors).Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good_1)
                .WithSupplier(allors.ActiveSuppliers.First)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            var purchaseInvoiceItem_1 = new PurchaseInvoiceItemBuilder(this.Session)
                .WithDescription("first item")
                .WithPart(good_1)
                .WithAssignedUnitPrice(3000)
                .WithQuantity(1)
                .WithMessage(@"line1
line2")
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var purchaseInvoiceItem_2 = new PurchaseInvoiceItemBuilder(this.Session)
                .WithDescription("second item")
                .WithAssignedUnitPrice(2000)
                .WithQuantity(2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .Build();

            var purchaseInvoiceItem_3 = new PurchaseInvoiceItemBuilder(this.Session)
                .WithDescription("Service")
                .WithAssignedUnitPrice(100)
                .WithQuantity(1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).Service)
                .Build();

            var purchaseInvoice = new PurchaseInvoiceBuilder(this.Session)
                .WithBilledTo(allors)
                .WithInvoiceNumber("1")
                .WithBilledFrom(allors.ActiveSuppliers.First)
                .WithPurchaseInvoiceItem(purchaseInvoiceItem_1)
                .WithPurchaseInvoiceItem(purchaseInvoiceItem_2)
                .WithPurchaseInvoiceItem(purchaseInvoiceItem_3)
                .WithCustomerReference("a reference number")
                .WithDescription("Purchase of 1 used Aircraft Towbar")
                .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(this.Session).PurchaseInvoice)
                .WithVatRegime(new VatRegimes(this.Session).Assessable21)
                .Build();

            var purchaseOrderItem_1 = new PurchaseOrderItemBuilder(this.Session)
                .WithDescription("first purchase order item")
                .WithPart(good_1)
                .WithQuantityOrdered(1)
                .Build();

            var purchaseOrder = new PurchaseOrderBuilder(this.Session)
                .WithOrderedBy(allors)
                .WithTakenViaSupplier(allors.ActiveSuppliers.First)
                .WithPurchaseOrderItem(purchaseOrderItem_1)
                .WithCustomerReference("reference 123")
                .WithStoredInFacility(facility)
                .Build();

            var workTask = new WorkTaskBuilder(this.Session)
                .WithTakenBy(allors)
                .WithCustomer(allors.ActiveCustomers.First)
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

            // Serialized RFQ with Serialized Unified-Good
            var serializedRFQ = new RequestForQuoteBuilder(this.Session).WithSerializedDefaults(allors).Build();

            // NonSerialized RFQ with NonSerialized Unified-Good
            var nonSerializedRFQ = new RequestForQuoteBuilder(this.Session).WithNonSerializedDefaults(allors).Build();

            var quote = new ProductQuoteBuilder(this.Session)
                .WithIssuer(allors)
                .WithDescription("quote")
                .WithReceiver(allors.ActiveCustomers.First)
                .WithFullfillContactMechanism(allors.ActiveCustomers.First.GeneralCorrespondence)
                .Build();

            this.Session.Derive();

            var quoteItem = new QuoteItemBuilder(this.Session)
                .WithProduct(new Goods(this.Session).Extent().First)
                .WithQuantity(1)
                .WithAssignedUnitPrice(10)
                .Build();

            quote.AddQuoteItem(quoteItem);

            this.Session.Derive();

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithTakenBy(allors)
                .WithShipToCustomer(allors.ActiveCustomers.First)
                .Build();

            this.Session.Derive();

            var salesOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good_1)
                .WithQuantityOrdered(1)
                .WithAssignedUnitPrice(10)
                .Build();

            salesOrder.AddSalesOrderItem(salesOrderItem);

            this.Session.Derive();

            new CustomerShipmentBuilder(this.Session).WithDefaults(allors).Build();

            this.Session.Derive();

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

// <copyright file="SingletonExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors
{
    using System;
    using System.IO;
    using System.Linq;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Bogus;

    public static class SingletonExtensions
    {
        public static void Full(this Singleton @this, DirectoryInfo dataPath, Faker faker)
        {
            var dutchLocale = new Locales(@this.Session()).DutchNetherlands;
            @this.AddAdditionalLocale(dutchLocale);

            var administrator = new PersonBuilder(@this.Session()).WithUserName("administrator").Build();
            new UserGroups(@this.Session()).Administrators.AddMember(administrator);
            new UserGroups(@this.Session()).Creators.AddMember(administrator);

            @this.Session().Derive();

            var euro = new Currencies(@this.Session()).FindBy(M.Currency.IsoCode, "EUR");

            var be = new Countries(@this.Session()).FindBy(M.Country.IsoCode, "BE");
            var us = new Countries(@this.Session()).FindBy(M.Country.IsoCode, "US");

            var allorsLogo = dataPath + @"\www\admin\images\logo.png";

            var serialisedItemSoldOns = new SerialisedItemSoldOn[] { new SerialisedItemSoldOns(@this.Session()).SalesInvoiceSend, new SerialisedItemSoldOns(@this.Session()).PurchaseInvoiceConfirm };

            var allors = Organisations.CreateInternalOrganisation(
                session: @this.Session(),
                name: "Allors BVBA",
                address: "Kleine Nieuwedijkstraat 4",
                postalCode: "2800",
                locality: "Mechelen",
                country: be,
                phone1CountryCode: "+32",
                phone1: "2 335 2335",
                phone1Purpose: new ContactMechanismPurposes(@this.Session()).GeneralPhoneNumber,
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
                logo: "allors.png",
                storeName: "Allors Store",
                billingProcess: new BillingProcesses(@this.Session()).BillingForOrderItems,
                outgoingShipmentNumberPrefix: "a-CS",
                salesInvoiceNumberPrefix: "a-SI-{year}-",
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
                requestCounterValue: 0,
                quoteCounterValue: 0,
                orderCounterValue: 0,
                purchaseOrderCounterValue: 0,
                invoiceCounterValue: 100,
                purchaseInvoiceCounterValue: 0,
                purchaseOrderNeedsApproval: true,
                purchaseOrderApprovalThresholdLevel1: 1000M,
                purchaseOrderApprovalThresholdLevel2: 5000M,
                serialisedItemSoldOns: serialisedItemSoldOns,
                collectiveWorkEffortInvoice: true,
                invoiceSequence: new InvoiceSequences(@this.Session()).EnforcedSequence,
                requestSequence: new RequestSequences(@this.Session()).EnforcedSequence,
                quoteSequence: new QuoteSequences(@this.Session()).EnforcedSequence,
                customerShipmentSequence: new CustomerShipmentSequences(@this.Session()).EnforcedSequence,
                purchaseShipmentSequence: new PurchaseShipmentSequences(@this.Session()).EnforcedSequence,
                workEffortSequence: new WorkEffortSequences(@this.Session()).EnforcedSequence);

            var dipu = Organisations.CreateInternalOrganisation(
                session: @this.Session(),
                name: "Dipu BVBA",
                address: "Kleine Nieuwedijkstraat 2",
                postalCode: "2800",
                locality: "Mechelen",
                country: be,
                phone1CountryCode: "+32",
                phone1: "2 15 49 49 49",
                phone1Purpose: new ContactMechanismPurposes(@this.Session()).GeneralPhoneNumber,
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
                logo: "allors.png",
                storeName: "Dipu Store",
                billingProcess: new BillingProcesses(@this.Session()).BillingForOrderItems,
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
                autoGenerateShipmentPackage: true,
                isImmediatelyPacked: true,
                isAutomaticallyShipped: true,
                autoGenerateCustomerShipment: true,
                isAutomaticallyReceived: false,
                autoGeneratePurchaseShipment: false,
                useCreditNoteSequence: true,
                requestCounterValue: 0,
                quoteCounterValue: 0,
                orderCounterValue: 0,
                purchaseOrderCounterValue: 0,
                purchaseInvoiceCounterValue: 0,
                invoiceCounterValue: 0,
                purchaseOrderNeedsApproval: false,
                purchaseOrderApprovalThresholdLevel1: null,
                purchaseOrderApprovalThresholdLevel2: null,
                serialisedItemSoldOns: serialisedItemSoldOns,
                collectiveWorkEffortInvoice: true,
                invoiceSequence: new InvoiceSequences(@this.Session()).EnforcedSequence,
                requestSequence: new RequestSequences(@this.Session()).EnforcedSequence,
                quoteSequence: new QuoteSequences(@this.Session()).EnforcedSequence,
                customerShipmentSequence: new CustomerShipmentSequences(@this.Session()).EnforcedSequence,
                purchaseShipmentSequence: new PurchaseShipmentSequences(@this.Session()).EnforcedSequence,
                workEffortSequence: new WorkEffortSequences(@this.Session()).EnforcedSequence);

            // Give Administrator access
            new EmploymentBuilder(@this.Session()).WithEmployee(administrator).WithEmployer(allors).Build();

            @this.Settings.DefaultFacility = allors.FacilitiesWhereOwner.First;

            var allorsEmployee1 = allors.CreateEmployee("letmein", faker);
            var allorsEmployee2 = allors.CreateEmployee("letmein", faker);
            var allorsProductQuoteApprover = allors.CreateEmployee("letmein", faker);
            var allorsPurchaseInvoiceApprover = allors.CreateEmployee("letmein", faker);
            var allorsPurchaseOrderApproverLevel1 = allors.CreateEmployee("letmein", faker);
            var allorsPurchaseOrderApproverLevel2 = allors.CreateEmployee("letmein", faker);

            var dipuEmployee = dipu.CreateEmployee("letmein", faker);
            var dipuProductQuoteApprover = dipu.CreateEmployee("letmein", faker);
            var dipuPurchaseInvoiceApprover = dipu.CreateEmployee("letmein", faker);

            new FacilityBuilder(@this.Session())
                .WithName("Allors warehouse 2")
                .WithFacilityType(new FacilityTypes(@this.Session()).Warehouse)
                .WithOwner(allors)
                .Build();

            var vatRate = new VatRateBuilder(@this.Session()).WithRate(21).Build();
            var manufacturer = new OrganisationBuilder(@this.Session()).WithManufacturerDefaults(faker).Build();

            allors.CreateSupplier(faker);
            allors.CreateSupplier(faker);

            allors.CreateSubContractor(faker);
            allors.CreateSubContractor(faker);

            @this.Session().Derive();

            var nonSerialisedPart1 = allors.CreateNonSerialisedNonUnifiedPart(faker);
            var nonSerialisedPart2 = allors.CreateNonSerialisedNonUnifiedPart(faker);
            var serialisedPart1 = allors.CreateSerialisedNonUnifiedPart(faker);
            var serialisedPart2 = allors.CreateSerialisedNonUnifiedPart(faker);

            var good1 = new NonUnifiedGoodBuilder(@this.Session()).WithNonSerialisedDefaults(allors).Build();

            var good2 = new NonUnifiedGoodBuilder(@this.Session()).WithSerialisedDefaults(allors).Build();

            var serialisedItem = new SerialisedItemBuilder(@this.Session()).WithDefaults(allors).Build();
            serialisedPart1.AddSerialisedItem(serialisedItem);

            new InventoryItemTransactionBuilder(@this.Session())
                .WithSerialisedItem(serialisedItem)
                .WithFacility(allors.FacilitiesWhereOwner.First)
                .WithQuantity(1)
                .WithReason(new InventoryTransactionReasons(@this.Session()).IncomingShipment)
                .WithSerialisedInventoryItemState(new SerialisedInventoryItemStates(@this.Session()).Good)
                .Build();

            var good3 = new NonUnifiedGoodBuilder(@this.Session()).WithNonSerialisedDefaults(allors).Build();

            var good4 = new NonUnifiedGoodBuilder(@this.Session()).WithSerialisedDefaults(allors).Build();

            var productCategory1 = new ProductCategoryBuilder(@this.Session())
                .WithInternalOrganisation(allors)
                .WithName("Best selling gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(@this.Session()).WithText("Meest verkochte gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory2 = new ProductCategoryBuilder(@this.Session())
                .WithInternalOrganisation(allors)
                .WithName("Big Gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(@this.Session()).WithText("Grote Gizmo's").WithLocale(dutchLocale).Build())
                .Build();

            var productCategory3 = new ProductCategoryBuilder(@this.Session())
                .WithInternalOrganisation(allors)
                .WithName("Small gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(@this.Session()).WithText("Kleine gizmo's").WithLocale(dutchLocale).Build())
                .WithProduct(good1)
                .WithProduct(good2)
                .WithProduct(good3)
                .WithProduct(good4)
                .Build();

            new CatalogueBuilder(@this.Session())
                .WithInternalOrganisation(allors)
                .WithName("New gizmo's")
                .WithLocalisedName(new LocalisedTextBuilder(@this.Session()).WithText("Nieuwe gizmo's").WithLocale(dutchLocale).Build())
                .WithDescription("Latest in the world of Gizmo's")
                .WithLocalisedDescription(new LocalisedTextBuilder(@this.Session()).WithText("Laatste in de wereld van Gizmo's").WithLocale(dutchLocale).Build())
                .WithProductCategory(productCategory1)
                .Build();

            @this.Session().Derive();

            var email2 = new EmailAddressBuilder(@this.Session())
                .WithElectronicAddressString("recipient@acme.com")
                .Build();

            for (var i = 0; i < 10; i++)
            {
                var b2BCustomer = allors.CreateB2BCustomer(faker);

                @this.Session().Derive();

                new FaceToFaceCommunicationBuilder(@this.Session())
                    .WithDescription($"Meeting {i}")
                    .WithSubject($"meeting {i}")
                    .WithEventPurpose(new CommunicationEventPurposes(@this.Session()).Meeting)
                    .WithFromParty(allors.CurrentContacts.First)
                    .WithToParty(b2BCustomer.CurrentContacts.First)
                    .WithOwner(administrator)
                    .WithActualStart(@this.Session().Now())
                    .Build();

                new EmailCommunicationBuilder(@this.Session())
                    .WithDescription($"Email {i}")
                    .WithSubject($"email {i}")
                    .WithFromParty(allors.CurrentContacts.First)
                    .WithToParty(b2BCustomer.CurrentContacts.First)
                    .WithFromEmail(allors.GeneralEmail)
                    .WithToEmail(email2)
                    .WithEventPurpose(new CommunicationEventPurposes(@this.Session()).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(@this.Session().Now())
                    .Build();

                new LetterCorrespondenceBuilder(@this.Session())
                    .WithDescription($"Letter {i}")
                    .WithSubject($"letter {i}")
                    .WithFromParty(administrator)
                    .WithToParty(b2BCustomer.CurrentContacts.First)
                    .WithEventPurpose(new CommunicationEventPurposes(@this.Session()).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(@this.Session().Now())
                    .Build();

                new PhoneCommunicationBuilder(@this.Session())
                    .WithDescription($"Phone {i}")
                    .WithSubject($"phone {i}")
                    .WithFromParty(administrator)
                    .WithToParty(b2BCustomer.CurrentContacts.First)
                    .WithEventPurpose(new CommunicationEventPurposes(@this.Session()).Meeting)
                    .WithOwner(administrator)
                    .WithActualStart(@this.Session().Now())
                    .Build();

                var requestForQuote = new RequestForQuoteBuilder(@this.Session())
                    .WithEmailAddress($"customer{i}@acme.com")
                    .WithTelephoneNumber("+1 234 56789")
                    .WithRecipient(allors)
                    .Build();

                var requestItem = new RequestItemBuilder(@this.Session())
                    .WithSerialisedItem(serialisedItem)
                    .WithProduct(serialisedItem.PartWhereSerialisedItem.NonUnifiedGoodsWherePart.FirstOrDefault())
                    .WithComment($"Comment {i}")
                    .WithQuantity(1)
                    .Build();

                requestForQuote.AddRequestItem(requestItem);

                var quote = new ProductQuoteBuilder(@this.Session()).WithSerializedDefaults(allors).Build();

                var salesOrderItem1 = new SalesOrderItemBuilder(@this.Session())
                    .WithDescription("first item")
                    .WithProduct(good1)
                    .WithAssignedUnitPrice(3000)
                    .WithQuantityOrdered(1)
                    .WithMessage(@"line1
line2")
                    .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).ProductItem)
                    .Build();

                var salesOrderItem2 = new SalesOrderItemBuilder(@this.Session())
                    .WithDescription("second item")
                    .WithAssignedUnitPrice(2000)
                    .WithQuantityOrdered(2)
                    .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).ProductItem)
                    .Build();

                var salesOrderItem3 = new SalesOrderItemBuilder(@this.Session())
                    .WithDescription("Service")
                    .WithAssignedUnitPrice(100)
                    .WithQuantityOrdered(1)
                    .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).Service)
                    .Build();

                var order = new SalesOrderBuilder(@this.Session())
                    .WithTakenBy(allors)
                    .WithBillToCustomer(b2BCustomer)
                    .WithAssignedBillToEndCustomerContactMechanism(b2BCustomer.BillingAddress)
                    .WithSalesOrderItem(salesOrderItem1)
                    .WithSalesOrderItem(salesOrderItem2)
                    .WithSalesOrderItem(salesOrderItem3)
                    .WithCustomerReference("a reference number")
                    .WithDescription("Sale of 1 used Aircraft Towbar")
                    .WithAssignedVatRegime(new VatRegimes(@this.Session()).DutchStandardTariff)
                    .Build();

                var salesInvoiceItem1 = new SalesInvoiceItemBuilder(@this.Session())
                    .WithDescription("first item")
                    .WithProduct(good1)
                    .WithAssignedUnitPrice(3000)
                    .WithQuantity(1)
                    .WithMessage(@"line1
line2")
                    .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).ProductItem)
                    .Build();

                var salesInvoiceItem2 = new SalesInvoiceItemBuilder(@this.Session())
                    .WithDescription("second item")
                    .WithAssignedUnitPrice(2000)
                    .WithQuantity(2)
                    .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).ProductItem)
                    .Build();

                var salesInvoiceItem3 = new SalesInvoiceItemBuilder(@this.Session())
                    .WithDescription("Service")
                    .WithAssignedUnitPrice(100)
                    .WithQuantity(1)
                    .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).Service)
                    .Build();

                var exw = new IncoTermTypes(@this.Session()).Exw;
                var incoTerm = new IncoTermBuilder(@this.Session()).WithTermType(exw).WithTermValue("XW").Build();

                var salesInvoice = new SalesInvoiceBuilder(@this.Session())
                    .WithBilledFrom(allors)
                    .WithBillToCustomer(b2BCustomer)
                    .WithBillToContactPerson(b2BCustomer.CurrentContacts.First)
                    .WithAssignedBillToContactMechanism(b2BCustomer.BillingAddress)
                    .WithSalesInvoiceItem(salesInvoiceItem1)
                    .WithSalesInvoiceItem(salesInvoiceItem2)
                    .WithSalesInvoiceItem(salesInvoiceItem3)
                    .WithCustomerReference("a reference number")
                    .WithDescription("Sale of 1 used Aircraft Towbar")
                    .WithSalesInvoiceType(new SalesInvoiceTypes(@this.Session()).SalesInvoice)
                    .WithSalesTerm(incoTerm)
                    .WithAssignedVatRegime(new VatRegimes(@this.Session()).DutchStandardTariff)
                    .Build();

                for (var j = 0; j < 3; j++)
                {
                    var salesInvoiceItem = new SalesInvoiceItemBuilder(@this.Session())
                        .WithDescription("Some service being delivered")
                        .WithAssignedUnitPrice(100 + j)
                        .WithQuantity(1)
                        .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).Service)
                        .Build();

                    salesInvoice.AddSalesInvoiceItem(salesInvoiceItem);
                }
            }

            @this.Session().Derive();

            for (var i = 0; i < 4; i++)
            {
                var supplier = faker.Random.ListItem(allors.CurrentSuppliers);

                var purchaseInvoiceItem1 = new PurchaseInvoiceItemBuilder(@this.Session())
                    .WithDescription("first item")
                    .WithPart(nonSerialisedPart1)
                    .WithAssignedUnitPrice(3000)
                    .WithQuantity(1)
                    .WithMessage(@"line1
line2")
                    .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).PartItem)
                    .Build();

                var purchaseInvoiceItem2 = new PurchaseInvoiceItemBuilder(@this.Session())
                    .WithDescription("second item")
                    .WithAssignedUnitPrice(2000)
                    .WithQuantity(2)
                    .WithPart(nonSerialisedPart2)
                    .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).PartItem)
                    .Build();

                var purchaseInvoiceItem3 = new PurchaseInvoiceItemBuilder(@this.Session())
                    .WithDescription("Service")
                    .WithAssignedUnitPrice(100)
                    .WithQuantity(1)
                    .WithInvoiceItemType(new InvoiceItemTypes(@this.Session()).Service)
                    .Build();

                var purchaseInvoice = new PurchaseInvoiceBuilder(@this.Session())
                    .WithBilledTo(allors)
                    .WithBilledFrom(supplier)
                    .WithPurchaseInvoiceItem(purchaseInvoiceItem1)
                    .WithPurchaseInvoiceItem(purchaseInvoiceItem2)
                    .WithPurchaseInvoiceItem(purchaseInvoiceItem3)
                    .WithCustomerReference("a reference number")
                    .WithDescription("Purchase of 1 used Aircraft Towbar")
                    .WithPurchaseInvoiceType(new PurchaseInvoiceTypes(@this.Session()).PurchaseInvoice)
                    .WithAssignedVatRegime(new VatRegimes(@this.Session()).DutchStandardTariff)
                    .Build();

                var purchaseOrderItem1 = new PurchaseOrderItemBuilder(@this.Session())
                    .WithDescription("first purchase order item")
                    .WithPart(nonSerialisedPart1)
                    .WithStoredInFacility(allors.FacilitiesWhereOwner.First)
                    .WithQuantityOrdered(1)
                    .Build();

                var purchaseOrder = new PurchaseOrderBuilder(@this.Session())
                    .WithOrderedBy(allors)
                    .WithTakenViaSupplier(supplier)
                    .WithPurchaseOrderItem(purchaseOrderItem1)
                    .WithCustomerReference("reference " + i)
                    .Build();
            }

            var anOrganisation = new Organisations(@this.Session()).FindBy(M.Organisation.IsInternalOrganisation, false);

            var item = new SerialisedItemBuilder(@this.Session())
                .WithSerialNumber("112")
                .WithSerialisedItemAvailability(new SerialisedItemAvailabilities(@this.Session()).Sold)
                .WithAvailableForSale(false)
                .WithOwnedBy(anOrganisation)
                .Build();

            nonSerialisedPart2.AddSerialisedItem(item);

            var workTask = new WorkTaskBuilder(@this.Session())
                .WithTakenBy(allors)
                .WithCustomer(anOrganisation)
                .WithName("maintenance")
                .Build();

            new WorkEffortFixedAssetAssignmentBuilder(@this.Session())
                .WithFixedAsset(item)
                .WithAssignment(workTask)
                .Build();

            var workOrderPart1 = allors.CreateNonSerialisedNonUnifiedPart(faker);
            var workOrderPart2 = allors.CreateNonSerialisedNonUnifiedPart(faker);
            var workOrderPart3 = allors.CreateNonSerialisedNonUnifiedPart(faker);

            @this.Session().Derive();

            var workOrder = new WorkTaskBuilder(@this.Session())
                .WithName("Task")
                .WithTakenBy(allors)
                .WithFacility(new Facilities(@this.Session()).Extent().First)
                .WithCustomer(anOrganisation)
                .WithWorkEffortPurpose(new WorkEffortPurposes(@this.Session()).Maintenance)
                .WithSpecialTerms("Net 45 Days")
                .Build();

            new WorkEffortFixedAssetAssignmentBuilder(@this.Session())
                .WithFixedAsset(item)
                .WithAssignment(workOrder)
                .WithComment("Busted tailpipe")
                .Build();

            workOrder.CreateInventoryAssignment(workOrderPart1, 11);
            workOrder.CreateInventoryAssignment(workOrderPart2, 12);
            workOrder.CreateInventoryAssignment(workOrderPart3, 13);

            //// Work Effort Time Entries
            var yesterday = DateTimeFactory.CreateDateTime(@this.Session().Now().AddDays(-1));
            var laterYesterday = DateTimeFactory.CreateDateTime(yesterday.AddHours(3));

            var today = DateTimeFactory.CreateDateTime(@this.Session().Now());
            var laterToday = DateTimeFactory.CreateDateTime(today.AddHours(4));

            var tomorrow = DateTimeFactory.CreateDateTime(@this.Session().Now().AddDays(1));
            var laterTomorrow = DateTimeFactory.CreateDateTime(tomorrow.AddHours(6));

            var standardRate = new RateTypes(@this.Session()).StandardRate;
            var overtimeRate = new RateTypes(@this.Session()).OvertimeRate;

            var frequencies = new TimeFrequencies(@this.Session());

            var timeEntryYesterday1 = workOrder.CreateTimeEntry(yesterday, laterYesterday, frequencies.Day, standardRate);
            var timeEntryToday1 = workOrder.CreateTimeEntry(today, laterToday, frequencies.Hour, standardRate);
            var timeEntryTomorrow1 = workOrder.CreateTimeEntry(tomorrow, laterTomorrow, frequencies.Minute, overtimeRate);

            allorsEmployee1.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday1);
            allorsEmployee1.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday1);
            allorsEmployee1.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow1);

            var timeEntryYesterday2 = workOrder.CreateTimeEntry(yesterday, laterYesterday, frequencies.Day, standardRate);
            var timeEntryToday2 = workOrder.CreateTimeEntry(today, laterToday, frequencies.Hour, standardRate);
            var timeEntryTomorrow2 = workOrder.CreateTimeEntry(tomorrow, laterTomorrow, frequencies.Minute, overtimeRate);

            allorsEmployee2.TimeSheetWhereWorker.AddTimeEntry(timeEntryYesterday2);
            allorsEmployee2.TimeSheetWhereWorker.AddTimeEntry(timeEntryToday2);
            allorsEmployee2.TimeSheetWhereWorker.AddTimeEntry(timeEntryTomorrow2);

            var po = new PurchaseOrders(@this.Session()).Extent().First;
            foreach (PurchaseOrderItem purchaseOrderItem in po.PurchaseOrderItems)
            {
                new WorkEffortPurchaseOrderItemAssignmentBuilder(@this.Session())
                    .WithPurchaseOrderItem(purchaseOrderItem)
                    .WithAssignment(workOrder)
                    .WithQuantity(1)
                    .Build();
            }

            @this.Session().Derive();
        }
    }
}

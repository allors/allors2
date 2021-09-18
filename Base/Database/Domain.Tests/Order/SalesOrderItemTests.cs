// <copyright file="SalesOrderItemTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;
    using Xunit;

    public class SalesOrderItemTests : DomainTest
    {
        private ProductCategory productCategory;
        private ProductCategory ancestorProductCategory;
        private ProductCategory parentProductCategory;
        private Good good;
        private readonly Good variantGood;
        private readonly Good variantGood2;
        private Good virtualGood;
        private Part part;
        private Colour feature1;
        private Colour feature2;
        private Organisation shipToCustomer;
        private Organisation billToCustomer;
        private Organisation supplier;
        private City kiev;
        private PostalAddress shipToContactMechanismMechelen;
        private PostalAddress shipToContactMechanismKiev;
        private BasePrice currentBasePriceGeoBoundary;
        private BasePrice currentGoodBasePrice;
        private BasePrice currentGood1Feature1BasePrice;
        private BasePrice currentFeature2BasePrice;
        private SupplierOffering goodPurchasePrice;
        private SupplierOffering virtualGoodPurchasePrice;
        private SalesOrder order;
        private VatRegime vatRegime;

        public SalesOrderItemTests()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            this.supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();

            this.vatRegime = new VatRegimes(this.Session).BelgiumStandard;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            this.kiev = new CityBuilder(this.Session).WithName("Kiev").Build();

            this.shipToContactMechanismMechelen = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            this.shipToContactMechanismKiev = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(this.kiev).WithAddress1("Dnieper").Build();
            this.shipToCustomer = new OrganisationBuilder(this.Session).WithName("shipToCustomer").Build();
            this.shipToCustomer.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                                                            .WithContactMechanism(this.shipToContactMechanismKiev)
                                                            .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                                                            .WithUseAsDefault(true)
                                                            .Build());

            this.billToCustomer = new OrganisationBuilder(this.Session)
                .WithName("billToCustomer")
                .WithPreferredCurrency(euro)

                .Build();

            this.billToCustomer.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                                                            .WithContactMechanism(this.shipToContactMechanismKiev)
                                                            .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                                                            .WithUseAsDefault(true)
                                                            .Build());

            this.part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.good = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("10101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(this.vatRegime)
                .WithName("good")
                .WithPart(this.part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(this.supplier)
                .WithFromDate(this.Session.Now())
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(this.billToCustomer).Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(this.shipToCustomer).Build();

            this.variantGood = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(this.vatRegime)
                .WithName("variant good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("2")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            this.variantGood2 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10102")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(this.vatRegime)
                .WithName("variant good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("3")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            this.virtualGood = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10103")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(this.vatRegime)
                .WithName("virtual good")
                .WithVariant(this.variantGood)
                .WithVariant(this.variantGood2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.ancestorProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("ancestor")
                .Build();

            this.parentProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("parent")
                .WithPrimaryParent(this.ancestorProductCategory)
                .Build();

            this.productCategory = new ProductCategoryBuilder(this.Session)
                .WithName("gizmo")
                .Build();

            this.productCategory.AddSecondaryParent(this.parentProductCategory);

            this.goodPurchasePrice = new SupplierOfferingBuilder(this.Session)
                .WithPart(this.part)
                .WithSupplier(this.supplier)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithFromDate(this.Session.Now())
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            this.virtualGoodPurchasePrice = new SupplierOfferingBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(this.Session.Now())
                .WithSupplier(this.supplier)
                .WithPrice(8)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.feature1 = new ColourBuilder(this.Session)
                .WithVatRegime(this.vatRegime)
                .WithName("white")
                .Build();

            this.feature2 = new ColourBuilder(this.Session)
                .WithName("black")
                .Build();

            this.currentBasePriceGeoBoundary = new BasePriceBuilder(this.Session)
                .WithDescription("current BasePriceGeoBoundary ")
                .WithGeographicBoundary(mechelen)
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(this.Session.Now())
                .Build();

            // previous basePrice for good
            new BasePriceBuilder(this.Session).WithDescription("previous good")
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for good
            new BasePriceBuilder(this.Session).WithDescription("future good")
                .WithProduct(this.good)
                .WithPrice(11)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            this.currentGoodBasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current good")
                .WithProduct(this.good)
                .WithPrice(10)
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature1
            new BasePriceBuilder(this.Session).WithDescription("previous feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(0.5M)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for feature1
            new BasePriceBuilder(this.Session).WithDescription("future feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(2.5M)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(2)
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature2
            new BasePriceBuilder(this.Session).WithDescription("previous feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(2)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for feature2
            new BasePriceBuilder(this.Session)
                .WithDescription("future feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(4)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            this.currentFeature2BasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(3)
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for good with feature1
            new BasePriceBuilder(this.Session).WithDescription("previous good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(4)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for good with feature1
            new BasePriceBuilder(this.Session)
                .WithDescription("future good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(6)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            this.currentGood1Feature1BasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(5)
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current variant good2")
                .WithProduct(this.variantGood2)
                .WithPrice(11)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.order = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void GivenOrderItemWithVatRegime_WhenDeriving_ThenDerivedVatRegimeIsFromOrderItem()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedVatRegime(new VatRegimes(this.Session).ZeroRated)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(1)
                .Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(orderItem.DerivedVatRegime, orderItem.DerivedVatRegime);
        }

        [Fact]
        public void GivenOrderItemWithoutVatRegime_WhenDeriving_ThenDerivedVatRegimeIsFromOrder()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedShipToAddress(this.shipToContactMechanismMechelen)
                .WithAssignedVatRegime(new VatRegimes(this.Session).ZeroRated)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(salesOrder.DerivedVatRegime, orderItem.DerivedVatRegime);
        }

        [Fact]
        public void GivenOrderItemWithoutVatRate_WhenDeriving_ThenItemDerivedVatRateIsFromOrderVatRegime()
        {
            this.InstantiateObjects(this.Session);

            var expected = new VatRates(this.Session).Belgium21;

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedShipToAddress(this.shipToContactMechanismMechelen)
                .WithAssignedVatRegime(new VatRegimes(this.Session).BelgiumStandard)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(salesOrder.DerivedVatRegime, orderItem.DerivedVatRegime);
            Assert.Equal(expected, orderItem.VatRate);
        }

        [Fact]
        public void GivenOrderItemWithAssignedDeliveryDate_WhenDeriving_ThenDeliveryDateIsOrderItemAssignedDeliveryDate()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.billToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedShipToAddress(this.shipToContactMechanismMechelen)
                .WithAssignedVatRegime(new VatRegimes(this.Session).ZeroRated)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(1)
                .WithAssignedDeliveryDate(this.Session.Now().AddMonths(1))
                .Build();

            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(orderItem.DerivedDeliveryDate, orderItem.AssignedDeliveryDate);
        }

        [Fact]
        public void GivenOrderItemWithoutDeliveryDate_WhenDeriving_ThenDerivedDeliveryDateIsOrderDeliveryDate()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.billToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedShipToAddress(this.shipToContactMechanismMechelen)
                .WithAssignedVatRegime(new VatRegimes(this.Session).ZeroRated)
                .WithDeliveryDate(this.Session.Now().AddMonths(1))
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(1)
                .Build();

            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(orderItem.DerivedDeliveryDate, salesOrder.DeliveryDate);
        }

        [Fact]
        public void GivenOrderItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.Session);

            var builder = new SalesOrderItemBuilder(this.Session);
            var orderItem = builder.Build();

            this.order.AddSalesOrderItem(orderItem);

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithProduct(this.good);
            orderItem = builder.Build();

            this.order.AddSalesOrderItem(orderItem);

            this.Session.Rollback();

            builder.WithQuantityOrdered(1);
            orderItem = builder.Build();

            this.order.AddSalesOrderItem(orderItem);

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithProductFeature(this.feature1);
            orderItem = builder.Build();

            this.order.AddSalesOrderItem(orderItem);

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrderItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            this.InstantiateObjects(this.Session);

            var item = new SalesOrderItemBuilder(this.Session).Build();

            Assert.Equal(new SalesOrderItemStates(this.Session).Provisional, item.SalesOrderItemState);
        }

        [Fact]
        public void GivenOrderItemWithOrderedWithFeature_WhenDeriving_ThenOrderedWithFeatureOrderItemMustBeForProductFeature()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithAssignedShipToAddress(this.shipToContactMechanismMechelen)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            var productOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            var productFeatureOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductFeatureItem)
                .WithProductFeature(this.feature1)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            productOrderItem.AddOrderedWithFeature(productFeatureOrderItem);
            salesOrder.AddSalesOrderItem(productOrderItem);
            salesOrder.AddSalesOrderItem(productFeatureOrderItem);

            Assert.False(this.Session.Derive(false).HasErrors);

            productFeatureOrderItem.RemoveProductFeature();
            productFeatureOrderItem.Product = this.good;

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrderItemWithoutCustomer_WhenDeriving_ShipToAddressIsNull()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session).WithBillToCustomer(this.billToCustomer).Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Null(orderItem.DerivedShipToAddress);
            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrderItemWithoutShipFromAddress_WhenDeriving_ThenDerivedShipFromAddressIsFromOrder()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithAssignedShipFromAddress(this.shipToContactMechanismMechelen)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(this.shipToContactMechanismMechelen, orderItem.DerivedShipFromAddress);
        }

        [Fact]
        public void GivenOrderItemWithoutShipToAddress_WhenDeriving_ThenDerivedShipToAddressIsFromOrder()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithAssignedShipToAddress(this.shipToContactMechanismMechelen)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(this.shipToContactMechanismMechelen, orderItem.DerivedShipToAddress);
        }

        [Fact]
        public void GivenOrderItemWithoutShipToParty_WhenDeriving_ThenDerivedShipToPartyIsFromOrder()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(this.shipToCustomer, orderItem.DerivedShipToParty);
        }

        [Fact]
        public void GivenOrderItemForGoodWithoutSelectedInventoryItem_WhenConfirming_ThenReservedFromNonSerialisedInventoryItemIsFromDefaultFacility()
        {
            this.InstantiateObjects(this.Session);

            var good2 = new Goods(this.Session).FindBy(M.Good.Name, "good2");

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(this.supplier)
                .WithFromDate(this.Session.Now())
                .Build();

            var good2PurchasePrice = new SupplierOfferingBuilder(this.Session)
                .WithPart(this.part)
                .WithSupplier(this.supplier)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithFromDate(this.Session.Now())
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            //// good with part as inventory item
            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good2)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            this.order.SetReadyForPosting();

            this.Session.Derive();

            order.Post();
            this.Session.Derive(true);

            order.Accept();
            this.Session.Derive(true);

            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), item1.ReservedFromNonSerialisedInventoryItem.Facility);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), item2.ReservedFromNonSerialisedInventoryItem.Facility);
        }

        //[Fact]
        //public void GivenConfirmedOrderItemForGood_WhenReservedFromNonSerialisedInventoryItemChangesValue_ThenQuantitiesAreMovedFromOldToNewInventoryItem()
        //{
        //    this.InstantiateObjects(this.Session);

        //    new InventoryItemTransactionBuilder(this.Session).WithQuantity(1).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

        //    this.Session.Derive();

        //    var secondWarehouse = new FacilityBuilder(this.Session)
        //        .WithName("affiliate warehouse")
        //        .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
        //        .WithOwner(this.InternalOrganisation)
        //        .Build();

        //    var order1 = new SalesOrderBuilder(this.Session)
        //        .WithShipToCustomer(this.shipToCustomer)
        //        .WithBillToCustomer(this.billToCustomer)
        //        .WithPartiallyShip(false)
        //        .Build();

        //    var salesOrderItem = new SalesOrderItemBuilder(this.Session)
        //        .WithProduct(this.good)
        //        .WithQuantityOrdered(3)
        //        .WithAssignedUnitPrice(5)
        //        .Build();

        //    order1.AddSalesOrderItem(salesOrderItem);

        //    this.Session.Derive();

        //    order1.Confirm();

        //    this.Session.Derive();

        //    order1.Send();

        //    this.Session.Derive(true);

        //    Assert.Equal(3, salesOrderItem.QuantityOrdered);
        //    Assert.Equal(0, salesOrderItem.QuantityShipped);
        //    Assert.Equal(0, salesOrderItem.QuantityPendingShipment);
        //    Assert.Equal(3, salesOrderItem.QuantityReserved);
        //    Assert.Equal(2, salesOrderItem.QuantityShortFalled);
        //    Assert.Equal(1, salesOrderItem.QuantityRequestsShipping);
        //    Assert.Equal(1, salesOrderItem.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
        //    Assert.Equal(0, salesOrderItem.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
        //    Assert.Equal(1, salesOrderItem.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

        //    var previous = salesOrderItem.ReservedFromNonSerialisedInventoryItem;

        //    var transaction = new InventoryItemTransactionBuilder(this.Session).WithFacility(secondWarehouse).WithPart(this.part).WithQuantity(1).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();

        //    this.Session.Derive();

        //    var current = transaction.InventoryItem as NonSerialisedInventoryItem;

        //    salesOrderItem.ReservedFromNonSerialisedInventoryItem = current;

        //    this.Session.Derive();

        //    Assert.Equal(3, salesOrderItem.QuantityOrdered);
        //    Assert.Equal(0, salesOrderItem.QuantityShipped);
        //    Assert.Equal(0, salesOrderItem.QuantityPendingShipment);
        //    Assert.Equal(3, salesOrderItem.QuantityReserved);
        //    Assert.Equal(2, salesOrderItem.QuantityShortFalled);
        //    Assert.Equal(1, salesOrderItem.QuantityRequestsShipping);
        //    Assert.Equal(0, previous.QuantityCommittedOut);
        //    Assert.Equal(1, previous.AvailableToPromise);
        //    Assert.Equal(1, previous.QuantityOnHand);
        //    Assert.Equal(1, current.QuantityCommittedOut);
        //    Assert.Equal(0, current.AvailableToPromise);
        //    Assert.Equal(1, current.QuantityOnHand);
        //}

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenOrderItemIsCancelled_ThenNonSerialisedInventoryQuantitiesAreReleased()
        {
            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(3).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            var salesOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(salesOrderItem);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive(true);

            order.Accept();
            this.Session.Derive(true);

            Assert.Equal(salesOrderItem.QuantityOrdered, salesOrderItem.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, salesOrderItem.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);

            this.Session.Derive();

            salesOrderItem.Cancel();

            this.Session.Derive();

            Assert.Equal(0, salesOrderItem.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(3, salesOrderItem.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(3, salesOrderItem.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenOrderItemIsRejected_ThenNonSerialisedInventoryQuantitiesAreReleased()
        {
            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(3).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            var salesOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(salesOrderItem);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive(true);

            order.Accept();
            this.Session.Derive(true);

            Assert.Equal(salesOrderItem.QuantityOrdered, salesOrderItem.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, salesOrderItem.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);

            this.Session.Derive();

            salesOrderItem.Reject();

            this.Session.Derive();

            Assert.Equal(0, salesOrderItem.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(3, salesOrderItem.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(3, salesOrderItem.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenOrderItemForGoodWithEnoughStockAvailable_WhenConfirming_ThenQuantitiesReservedAndRequestsShippingAreEqualToQuantityOrdered()
        {
            this.InstantiateObjects(this.Session);

            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(100)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            Assert.Equal(100, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(100, item.QuantityPendingShipment);
            Assert.Equal(100, item.QuantityReserved);
            Assert.Equal(0, item.QuantityShortFalled);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(100, item.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(10, item.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(110, item.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenOrderItemForGoodWithNotEnoughStockAvailable_WhenConfirming_ThenQuantitiesReservedAndRequestsShippingAreEqualToInventoryAvailableToPromise()
        {
            this.InstantiateObjects(this.Session);

            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment).WithPart(this.part).Build();

            this.Session.Derive();

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(120)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            Assert.Equal(120, item1.QuantityOrdered);
            Assert.Equal(0, item1.QuantityShipped);
            Assert.Equal(110, item1.QuantityPendingShipment);
            Assert.Equal(120, item1.QuantityReserved);
            Assert.Equal(10, item1.QuantityShortFalled);
            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(110, item1.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(110, item1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item2);
            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            Assert.Equal(120, item1.QuantityOrdered);
            Assert.Equal(0, item1.QuantityShipped);
            Assert.Equal(110, item1.QuantityPendingShipment);
            Assert.Equal(120, item1.QuantityReserved);
            Assert.Equal(10, item1.QuantityShortFalled);
            Assert.Equal(0, item1.QuantityRequestsShipping);

            Assert.Equal(10, item2.QuantityOrdered);
            Assert.Equal(0, item2.QuantityShipped);
            Assert.Equal(0, item2.QuantityPendingShipment);
            Assert.Equal(10, item2.QuantityReserved);
            Assert.Equal(10, item2.QuantityShortFalled);
            Assert.Equal(0, item2.QuantityRequestsShipping);
            Assert.Equal(110, item2.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item2.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(110, item2.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenShipmentIsCreated_ThenQuantitiesRequestsShippingIsSetToZero()
        {
            this.InstantiateObjects(this.Session);

            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(100)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            Assert.Equal(100, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(100, item.QuantityPendingShipment);
            Assert.Equal(100, item.QuantityReserved);
            Assert.Equal(0, item.QuantityShortFalled);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(100, item.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(10, item.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(110, item.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            Assert.Equal(100, item.OrderShipmentsWhereOrderItem[0].Quantity);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenQuantityOrderedIsDecreased_ThenQuantitiesReservedAndRequestsShippingAndInventoryAvailableToPromiseDecreaseEqually()
        {
            var store = this.Session.Extent<Store>().First;
            store.AutoGenerateCustomerShipment = false;

            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(100)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            this.Session.Commit();

            item.QuantityOrdered = 50;

            this.Session.Derive();

            Assert.Equal(50, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(50, item.QuantityReserved);
            Assert.Equal(0, item.QuantityShortFalled);
            Assert.Equal(50, item.QuantityRequestsShipping);
            Assert.Equal(50, item.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(60, item.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(110, item.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenQuantityOrderedIsDecreased_ThenQuantityMayNotBeLessThanQuantityShipped()
        {
            this.InstantiateObjects(this.Session);

            var manual = new OrderKindBuilder(this.Session).WithDescription("manual").WithScheduleManually(true).Build();

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(120)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(orderItem);
            this.order.OrderKind = manual;
            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipFromAddress(this.order.TakenBy.ShippingAddress)
                .WithShipToParty(this.order.ShipToCustomer)
                .WithShipToAddress(this.order.DerivedShipToAddress)
                .WithShipmentPackage(new ShipmentPackageBuilder(this.Session).Build())
                .WithShipmentMethod(this.order.DerivedShipmentMethod)
                .Build();

            this.Session.Derive();

            var shipmentItem = new ShipmentItemBuilder(this.Session)
                .WithGood(orderItem.Product as Good)
                .WithQuantity(100)
                .WithReservedFromInventoryItem(orderItem.ReservedFromNonSerialisedInventoryItem)
                .Build();

            shipment.AddShipmentItem(shipmentItem);

            new OrderShipmentBuilder(this.Session)
                .WithOrderItem(orderItem)
                .WithShipmentItem(shipment.ShipmentItems.First)
                .WithQuantity(100)
                .Build();

            this.Session.Derive();

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();
            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem item in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(item).WithQuantity(item.Quantity).Build());
            }

            this.Session.Derive();

            shipment.Ship();
            this.Session.Derive();

            Assert.Equal(100, orderItem.QuantityShipped);

            orderItem.QuantityOrdered = 90;
            var derivationLog = this.Session.Derive(false);

            Assert.True(derivationLog.HasErrors);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndItemsShortFalled_WhenQuantityOrderedIsDecreased_ThenItemsShortFalledIsDecreasedAndShipmentIsLeftUnchanged()
        {
            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(30)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            Assert.Equal(20, item.QuantityShortFalled);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].Quantity);

            item.QuantityOrdered = 11;

            this.Session.Derive();

            Assert.Equal(1, item.QuantityShortFalled);

            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(10, pickList.PickListItems[0].Quantity);

            item.QuantityOrdered = 10;

            this.Session.Derive();

            Assert.Equal(0, item.QuantityShortFalled);

            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(10, pickList.PickListItems[0].Quantity);
        }

        [Fact]
        public void GivenManuallyScheduledOrderItem_WhenScheduled_ThenQuantityCannotExceedInventoryAvailableToPromise()
        {
            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(3).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var manual = new OrderKindBuilder(this.Session).WithDescription("manual").WithScheduleManually(true).Build();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedShipToAddress(this.shipToContactMechanismMechelen)
                .WithOrderKind(manual)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(5)
                .WithAssignedUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(orderItem);
            this.Session.Derive();

            order1.SetReadyForPosting();
            this.Session.Derive();

            order1.Post();
            this.Session.Derive();

            order1.Accept();
            this.Session.Derive();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipFromAddress(this.order.TakenBy.ShippingAddress)
                .WithShipToParty(this.order.ShipToCustomer)
                .WithShipToAddress(this.order.DerivedShipToAddress)
                .WithShipmentPackage(new ShipmentPackageBuilder(this.Session).Build())
                .WithShipmentMethod(this.order.DerivedShipmentMethod)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var shipmentItem = new ShipmentItemBuilder(this.Session)
                .WithGood(orderItem.Product as Good)
                .WithQuantity(5)
                .Build();

            shipment.AddShipmentItem(shipmentItem);

            new OrderShipmentBuilder(this.Session)
                .WithOrderItem(orderItem)
                .WithShipmentItem(shipment.ShipmentItems.First)
                .WithQuantity(5)
                .Build();

            var derivationLog = this.Session.Derive(false);

            Assert.True(derivationLog.HasErrors);
            Assert.Contains(M.OrderShipment.Quantity, derivationLog.Errors[0].RoleTypes);

            this.Session.Rollback();

            shipmentItem = new ShipmentItemBuilder(this.Session)
                .WithGood(orderItem.Product as Good)
                .WithQuantity(3)
                .Build();

            shipment.AddShipmentItem(shipmentItem);

            new OrderShipmentBuilder(this.Session)
                .WithOrderItem(orderItem)
                .WithShipmentItem(shipment.ShipmentItems.First)
                .WithQuantity(3)
                .Build();

            derivationLog = this.Session.Derive();

            Assert.False(derivationLog.HasErrors);
            Assert.Equal(3, orderItem.QuantityPendingShipment);
        }

        private void InstantiateObjects(ISession session)
        {
            this.productCategory = (ProductCategory)session.Instantiate(this.productCategory);
            this.parentProductCategory = (ProductCategory)session.Instantiate(this.parentProductCategory);
            this.ancestorProductCategory = (ProductCategory)session.Instantiate(this.ancestorProductCategory);
            this.part = (Part)session.Instantiate(this.part);
            this.virtualGood = (Good)session.Instantiate(this.virtualGood);
            this.good = (Good)session.Instantiate(this.good);
            this.feature1 = (Colour)session.Instantiate(this.feature1);
            this.feature2 = (Colour)session.Instantiate(this.feature2);
            this.shipToCustomer = (Organisation)session.Instantiate(this.shipToCustomer);
            this.billToCustomer = (Organisation)session.Instantiate(this.billToCustomer);
            this.supplier = (Organisation)session.Instantiate(this.supplier);
            this.kiev = (City)session.Instantiate(this.kiev);
            this.shipToContactMechanismMechelen = (PostalAddress)session.Instantiate(this.shipToContactMechanismMechelen);
            this.shipToContactMechanismKiev = (PostalAddress)session.Instantiate(this.shipToContactMechanismKiev);
            this.currentBasePriceGeoBoundary = (BasePrice)session.Instantiate(this.currentBasePriceGeoBoundary);
            this.currentGoodBasePrice = (BasePrice)session.Instantiate(this.currentGoodBasePrice);
            this.currentGood1Feature1BasePrice = (BasePrice)session.Instantiate(this.currentGood1Feature1BasePrice);
            this.currentFeature2BasePrice = (BasePrice)session.Instantiate(this.currentFeature2BasePrice);
            this.goodPurchasePrice = (SupplierOffering)session.Instantiate(this.goodPurchasePrice);
            this.virtualGoodPurchasePrice = (SupplierOffering)session.Instantiate(this.virtualGoodPurchasePrice);
            this.currentGoodBasePrice = (BasePrice)session.Instantiate(this.currentGoodBasePrice);
            this.order = (SalesOrder)session.Instantiate(this.order);
            this.vatRegime = (VatRegime)session.Instantiate(this.vatRegime);
        }
    }

    [Trait("Category", "Security")]
    public class SalesOrderItemSecurityTests : DomainTest
    {
        private ProductCategory productCategory;
        private ProductCategory ancestorProductCategory;
        private ProductCategory parentProductCategory;
        private Good good;
        private readonly Good variantGood;
        private readonly Good variantGood2;
        private Good virtualGood;
        private Part part;
        private Colour feature1;
        private Colour feature2;
        private Organisation shipToCustomer;
        private Organisation billToCustomer;
        private Organisation supplier;
        private City kiev;
        private PostalAddress shipToContactMechanismMechelen;
        private PostalAddress shipToContactMechanismKiev;
        private BasePrice currentBasePriceGeoBoundary;
        private BasePrice currentGoodBasePrice;
        private BasePrice currentGood1Feature1BasePrice;
        private BasePrice currentFeature2BasePrice;
        private SupplierOffering goodPurchasePrice;
        private SupplierOffering virtualGoodPurchasePrice;
        private SalesOrder order;
        private VatRegime vatRegime;

        public override Config Config => new Config { SetupSecurity = true };

        public SalesOrderItemSecurityTests()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            this.supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();

            this.vatRegime = new VatRegimes(this.Session).BelgiumStandard;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            this.kiev = new CityBuilder(this.Session).WithName("Kiev").Build();

            this.shipToContactMechanismMechelen = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            this.shipToContactMechanismKiev = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(this.kiev).WithAddress1("Dnieper").Build();
            this.shipToCustomer = new OrganisationBuilder(this.Session).WithName("shipToCustomer").Build();
            this.shipToCustomer.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                                                            .WithContactMechanism(this.shipToContactMechanismKiev)
                                                            .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                                                            .WithUseAsDefault(true)
                                                            .Build());

            this.billToCustomer = new OrganisationBuilder(this.Session)
                .WithName("billToCustomer")
                .WithPreferredCurrency(euro)

                .Build();

            this.billToCustomer.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                                                            .WithContactMechanism(this.shipToContactMechanismKiev)
                                                            .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                                                            .WithUseAsDefault(true)
                                                            .Build());

            this.part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.good = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("10101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(this.vatRegime)
                .WithName("good")
                .WithPart(this.part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(this.supplier)
                .WithFromDate(this.Session.Now())
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(this.billToCustomer).Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(this.shipToCustomer).Build();

            this.variantGood = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(this.vatRegime)
                .WithName("variant good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("2")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            this.variantGood2 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10102")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(this.vatRegime)
                .WithName("variant good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("3")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            this.virtualGood = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10103")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRegime(this.vatRegime)
                .WithName("virtual good")
                .WithVariant(this.variantGood)
                .WithVariant(this.variantGood2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.ancestorProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("ancestor")
                .Build();

            this.parentProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("parent")
                .WithPrimaryParent(this.ancestorProductCategory)
                .Build();

            this.productCategory = new ProductCategoryBuilder(this.Session)
                .WithName("gizmo")
                .Build();

            this.productCategory.AddSecondaryParent(this.parentProductCategory);

            this.goodPurchasePrice = new SupplierOfferingBuilder(this.Session)
                .WithPart(this.part)
                .WithSupplier(this.supplier)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithFromDate(this.Session.Now())
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            this.virtualGoodPurchasePrice = new SupplierOfferingBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(this.Session.Now())
                .WithSupplier(this.supplier)
                .WithPrice(8)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.feature1 = new ColourBuilder(this.Session)
                .WithVatRegime(this.vatRegime)
                .WithName("white")
                .Build();

            this.feature2 = new ColourBuilder(this.Session)
                .WithName("black")
                .Build();

            this.currentBasePriceGeoBoundary = new BasePriceBuilder(this.Session)
                .WithDescription("current BasePriceGeoBoundary ")
                .WithGeographicBoundary(mechelen)
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(this.Session.Now())
                .Build();

            // previous basePrice for good
            new BasePriceBuilder(this.Session).WithDescription("previous good")
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for good
            new BasePriceBuilder(this.Session).WithDescription("future good")
                .WithProduct(this.good)
                .WithPrice(11)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            this.currentGoodBasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current good")
                .WithProduct(this.good)
                .WithPrice(10)
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature1
            new BasePriceBuilder(this.Session).WithDescription("previous feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(0.5M)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for feature1
            new BasePriceBuilder(this.Session).WithDescription("future feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(2.5M)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(2)
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature2
            new BasePriceBuilder(this.Session).WithDescription("previous feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(2)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for feature2
            new BasePriceBuilder(this.Session)
                .WithDescription("future feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(4)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            this.currentFeature2BasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(3)
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for good with feature1
            new BasePriceBuilder(this.Session).WithDescription("previous good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(4)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for good with feature1
            new BasePriceBuilder(this.Session)
                .WithDescription("future good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(6)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            this.currentGood1Feature1BasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(5)
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current variant good2")
                .WithProduct(this.variantGood2)
                .WithPrice(11)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.order = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCreated_ThenItemMayBeDeletedCancelledOrRejected()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(new SalesOrderItemStates(this.Session).Provisional, item.SalesOrderItemState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.True(acl.CanExecute(M.SalesOrderItem.Delete));
            Assert.True(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.True(acl.CanExecute(M.SalesOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsConfirmed_ThenItemMayBeCancelledOrRejectedButNotDeleted()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.order.SetReadyForPosting();

            this.Session.Derive();
            this.Session.Commit();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item.SalesOrderItemState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.True(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.True(acl.CanExecute(M.SalesOrderItem.Reject));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsPartiallyShipped_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(1).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)this.order.DerivedShipToAddress.ShipmentsWhereShipToAddress[0];

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();
            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            shipment.Ship();
            this.Session.Derive();

            Assert.Equal(new SalesOrderItemShipmentStates(this.Session).PartiallyShipped, item.SalesOrderItemShipmentState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCancelled_ThenItemMayNotBeCancelledOrRejected()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            item.Cancel();

            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).Cancelled, item.SalesOrderItemState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCancelled_ThenItemCanBeDeleted()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            item.Cancel();

            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).Cancelled, item.SalesOrderItemState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.True(acl.CanExecute(M.SalesOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsRejected_ThenItemMayNotBeCancelledOrRejected()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            item.Reject();

            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).Rejected, item.SalesOrderItemState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsRejected_ThenCanBeDeleted()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            item.Reject();

            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).Rejected, item.SalesOrderItemState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.True(acl.CanExecute(M.SalesOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCompleted_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            this.order.Ship();

            this.Session.Derive();

            var shipment = (CustomerShipment)this.order.DerivedShipToAddress.ShipmentsWhereShipToAddress[0];

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;
            pickList.SetPicked();

            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            shipment.Ship();

            this.Session.Derive();

            shipment.Invoice();

            this.Session.Derive();

            ((SalesInvoiceItem)shipment.ShipmentItems[0].ShipmentItemBillingsWhereShipmentItem[0].InvoiceItem).SalesInvoiceWhereSalesInvoiceItem.Send();

            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).Completed, item.SalesOrderItemState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsFinished_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SalesOrderState = new SalesOrderStates(this.Session).Finished;

            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).Finished, item.SalesOrderItemState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsPartiallyShipped_ThenProductChangeIsNotAllowed()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(1).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)this.order.DerivedShipToAddress.ShipmentsWhereShipToAddress[0];

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();
            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            shipment.Ship();
            this.Session.Derive();

            Assert.Equal(new SalesOrderItemShipmentStates(this.Session).PartiallyShipped, item.SalesOrderItemShipmentState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.False(acl.CanWrite(M.SalesOrderItem.Product));
        }

        [Fact]
        public void GivenOrderItem_WhenShippingInProgress_ThenCancelIsNotAllowed()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            User user = this.Administrator;
            this.Session.SetUser(user);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(1).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            this.order.Accept();
            this.Session.Derive();

            Assert.Equal(new SalesOrderItemShipmentStates(this.Session).InProgress, item.SalesOrderItemShipmentState);
            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[item];
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
        }

        private void InstantiateObjects(ISession session)
        {
            this.productCategory = (ProductCategory)session.Instantiate(this.productCategory);
            this.parentProductCategory = (ProductCategory)session.Instantiate(this.parentProductCategory);
            this.ancestorProductCategory = (ProductCategory)session.Instantiate(this.ancestorProductCategory);
            this.part = (Part)session.Instantiate(this.part);
            this.virtualGood = (Good)session.Instantiate(this.virtualGood);
            this.good = (Good)session.Instantiate(this.good);
            this.feature1 = (Colour)session.Instantiate(this.feature1);
            this.feature2 = (Colour)session.Instantiate(this.feature2);
            this.shipToCustomer = (Organisation)session.Instantiate(this.shipToCustomer);
            this.billToCustomer = (Organisation)session.Instantiate(this.billToCustomer);
            this.supplier = (Organisation)session.Instantiate(this.supplier);
            this.kiev = (City)session.Instantiate(this.kiev);
            this.shipToContactMechanismMechelen = (PostalAddress)session.Instantiate(this.shipToContactMechanismMechelen);
            this.shipToContactMechanismKiev = (PostalAddress)session.Instantiate(this.shipToContactMechanismKiev);
            this.currentBasePriceGeoBoundary = (BasePrice)session.Instantiate(this.currentBasePriceGeoBoundary);
            this.currentGoodBasePrice = (BasePrice)session.Instantiate(this.currentGoodBasePrice);
            this.currentGood1Feature1BasePrice = (BasePrice)session.Instantiate(this.currentGood1Feature1BasePrice);
            this.currentFeature2BasePrice = (BasePrice)session.Instantiate(this.currentFeature2BasePrice);
            this.goodPurchasePrice = (SupplierOffering)session.Instantiate(this.goodPurchasePrice);
            this.virtualGoodPurchasePrice = (SupplierOffering)session.Instantiate(this.virtualGoodPurchasePrice);
            this.currentGoodBasePrice = (BasePrice)session.Instantiate(this.currentGoodBasePrice);
            this.order = (SalesOrder)session.Instantiate(this.order);
            this.vatRegime = (VatRegime)session.Instantiate(this.vatRegime);
        }
    }
}

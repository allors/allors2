//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesOrderItemTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    using Meta;
    using Xunit;

    public class SalesOrderItemTests : DomainTest
    {
        private ProductCategory productCategory;
        private ProductCategory ancestorProductCategory;
        private ProductCategory parentProductCategory;
        private Good good;
        private Good variantGood;
        private Good variantGood2;
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
        private ProductPurchasePrice goodPurchasePrice;
        private ProductPurchasePrice virtualGoodPurchasePrice;
        private SupplierOffering goodSupplierOffering;
        private SalesOrder order;
        private VatRate vatRate21;

        public SalesOrderItemTests()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            this.supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();

            this.vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            this.kiev = new CityBuilder(this.Session).WithName("Kiev").Build();

            this.shipToContactMechanismMechelen = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            this.shipToContactMechanismKiev = new PostalAddressBuilder(this.Session).WithGeographicBoundary(this.kiev).WithAddress1("Dnieper").Build();
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

            this.part = new PartBuilder(this.Session)
                .WithGoodIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.ancestorProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("ancestor")
                .Build();

            this.parentProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("parent")
                .WithParent(this.ancestorProductCategory)
                .Build();

            this.productCategory = new ProductCategoryBuilder(this.Session)
                .WithName("gizmo")
                .Build();

            this.productCategory.AddParent(this.parentProductCategory);

            this.good = new GoodBuilder(this.Session)
                .WithGoodIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("10101")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(this.vatRate21)
                .WithName("good")
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithPart(this.part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(this.billToCustomer).Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(this.shipToCustomer).Build();

            //this.partyRevenueHistory = new PartyRevenueHistoryBuilder(this.DatabaseSession)
            //    .WithCurrency(euro)
            //    .WithSingleton(this.internalOrganisation)
            //    .WithParty(this.billToCustomer)
            //    .WithRevenue(100M)
            //    .Build();

            //this.productCategoryRevenueHistory = new PartyProductCategoryRevenueHistoryBuilder(this.DatabaseSession)
            //    .WithCurrency(euro)
            //    .WithSingleton(this.internalOrganisation)
            //    .WithParty(this.billToCustomer)
            //    .WithProductCategory(this.productCategory)
            //    .WithRevenue(100M)
            //    .WithQuantity(10)
            //    .Build();

            //this.parentProductCategoryRevenueHistory = new PartyProductCategoryRevenueHistoryBuilder(this.DatabaseSession)
            //    .WithCurrency(euro)
            //    .WithSingleton(this.internalOrganisation)
            //    .WithParty(this.billToCustomer)
            //    .WithProductCategory(this.parentProductCategory)
            //    .WithRevenue(100M)
            //    .WithQuantity(10)
            //    .Build();

            //this.ancestorProductCategoryRevenueHistory = new PartyProductCategoryRevenueHistoryBuilder(this.DatabaseSession)
            //    .WithCurrency(euro)
            //    .WithSingleton(this.internalOrganisation)
            //    .WithParty(this.billToCustomer)
            //    .WithProductCategory(this.ancestorProductCategory)
            //    .WithRevenue(100M)
            //    .WithQuantity(10)
            //    .Build();

            this.variantGood = new GoodBuilder(this.Session)
                .WithGoodIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10101")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(this.vatRate21)
                .WithName("variant good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithPart(new PartBuilder(this.Session)
                    .WithGoodIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("2")
                        .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            this.variantGood2 = new GoodBuilder(this.Session)
                .WithGoodIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10102")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(this.vatRate21)
                .WithName("variant good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithPart(new PartBuilder(this.Session)
                    .WithGoodIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("3")
                        .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            this.virtualGood = new GoodBuilder(this.Session)
                .WithGoodIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10103")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(this.vatRate21)
                .WithName("virtual good")
                .WithVariant(this.variantGood)
                .WithVariant(this.variantGood2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .Build();

            this.goodPurchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.goodSupplierOffering = new SupplierOfferingBuilder(this.Session)
                .WithPart(this.part)
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(this.goodPurchasePrice)
                .Build();

            this.virtualGoodPurchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(8)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.feature1 = new ColourBuilder(this.Session)
                .WithVatRate(this.vatRate21)
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
                .WithFromDate(DateTime.UtcNow)
                .Build();

            // previous basePrice for good
            new BasePriceBuilder(this.Session).WithDescription("previous good")
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for good
            new BasePriceBuilder(this.Session).WithDescription("future good")
                .WithProduct(this.good)
                .WithPrice(11)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.currentGoodBasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current good")
                .WithProduct(this.good)
                .WithPrice(10)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature1
            new BasePriceBuilder(this.Session).WithDescription("previous feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(0.5M)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for feature1
            new BasePriceBuilder(this.Session).WithDescription("future feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(2.5M)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(2)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature2
            new BasePriceBuilder(this.Session).WithDescription("previous feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(2)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for feature2
            new BasePriceBuilder(this.Session)
                .WithDescription("future feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(4)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.currentFeature2BasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(3)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for good with feature1
            new BasePriceBuilder(this.Session).WithDescription("previous good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(4)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for good with feature1
            new BasePriceBuilder(this.Session)
                .WithDescription("future good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(6)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.currentGood1Feature1BasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(5)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current variant good2")
                .WithProduct(this.variantGood2)
                .WithPrice(11)
                .WithFromDate(DateTime.UtcNow)
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
                .WithVatRegime(new VatRegimes(this.Session).Export)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(1)
                .Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(orderItem.VatRegime, orderItem.VatRegime);
        }

        [Fact]
        public void GivenOrderItemWithoutVatRegime_WhenDeriving_ThenDerivedVatRegimeIsFromOrder()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.Session).Export)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(salesOrder.VatRegime, orderItem.VatRegime);
        }

        [Fact]
        public void GivenOrderItemWithoutVatRate_WhenDeriving_ThenItemDerivedVatRateIsFromOrderVatRegime()
        {
            this.InstantiateObjects(this.Session);

            var expected = new VatRegimes(this.Session).Export.VatRate;

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.Session).Export)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(salesOrder.VatRegime, orderItem.VatRegime);
            Assert.Equal(expected, orderItem.DerivedVatRate);
        }

        [Fact]
        public void GivenOrderItemWithoutVatRateAndOrderWithoutVatRegime_WhenDeriving_ThenItemDerivedVatRateIsFromOrderedProduct()
        {
            this.InstantiateObjects(this.Session);

            var expected = this.good.VatRate;

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(salesOrder.VatRegime, orderItem.VatRegime);
            Assert.Equal(expected, orderItem.DerivedVatRate);
        }

        [Fact]
        public void GivenOrderItemWithAssignedDeliveryDate_WhenDeriving_ThenDeliveryDateIsOrderItemAssignedDeliveryDate()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.billToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.Session).Export)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(1)
                .WithAssignedDeliveryDate(DateTime.UtcNow.AddMonths(1))
                .Build();

            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(orderItem.DeliveryDate, orderItem.AssignedDeliveryDate);
        }

        [Fact]
        public void GivenOrderItemWithoutDeliveryDate_WhenDeriving_ThenDerivedDeliveryDateIsOrderDeliveryDate()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.billToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.Session).Export)
                .WithDeliveryDate(DateTime.UtcNow.AddMonths(1))
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(1)
                .Build();

            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(orderItem.DeliveryDate, salesOrder.DeliveryDate);
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

            Assert.Equal(new SalesOrderItemStates(this.Session).Created, item.SalesOrderItemState);
        }

        [Fact]
        public void GivenOrderItemWithOrderedWithFeature_WhenDeriving_ThenOrderedWithFeatureOrderItemMustBeForProductFeature()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            var productOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            var productFeatureOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductFeatureItem)
                .WithProductFeature(this.feature1)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
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

            Assert.Null(orderItem.ShipToAddress);
            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrderItemWithoutShipToAddress_WhenDeriving_ThenDerivedShipToAddressIsFromOrder()
        {
            this.InstantiateObjects(this.Session);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(this.shipToContactMechanismMechelen, orderItem.ShipToAddress);
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

            Assert.Equal(this.shipToCustomer, orderItem.ShipToParty);
        }

        [Fact]
        public void GivenOrderItemForGoodWithoutSelectedInventoryItem_WhenConfirming_ThenReservedFromNonSerialisedInventoryItemIsFromDefaultFacility()
        {
            this.InstantiateObjects(this.Session);

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new Goods(this.Session).FindBy(M.Good.Name, "good2");

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(this.part)
                .WithFromDate(DateTime.UtcNow)
                .WithSupplier(this.supplier)
                .WithProductPurchasePrice(good2PurchasePrice)
                .Build();

            //// good with part as inventory item
            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good2)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), item1.ReservedFromNonSerialisedInventoryItem.Facility);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), item2.ReservedFromNonSerialisedInventoryItem.Facility);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenReservedFromNonSerialisedInventoryItemChangesValue_ThenQuantitiesAreMovedFromOldToNewInventoryItem()
        {
            this.InstantiateObjects(this.Session);

            var secondWarehouse = new FacilityBuilder(this.Session)
                .WithName("affiliate warehouse")
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithOwner(this.InternalOrganisation)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            Assert.Equal(3, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityPicked);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(3, item.QuantityReserved);
            Assert.Equal(3, item.QuantityShortFalled);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(3, item.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(0, item.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            var previous = item.ReservedFromNonSerialisedInventoryItem;
            var current = new NonSerialisedInventoryItemBuilder(this.Session).WithFacility(secondWarehouse).WithPart(this.part).Build();

            item.ReservedFromNonSerialisedInventoryItem = current;

            this.Session.Derive();

            Assert.Equal(3, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityPicked);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(3, item.QuantityReserved);
            Assert.Equal(3, item.QuantityShortFalled);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, previous.QuantityCommittedOut);
            Assert.Equal(0, previous.AvailableToPromise);
            Assert.Equal(3, current.QuantityCommittedOut);
            Assert.Equal(0, current.AvailableToPromise);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenOrderItemIsCancelled_ThenNonSerialisedInventoryQuantitiesAreReleased()
        {
            this.InstantiateObjects(this.Session);

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            Assert.Equal(item.QuantityOrdered, item.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);

            var previous = item.ReservedFromNonSerialisedInventoryItem;

            this.Session.Derive();

            item.Cancel();

            this.Session.Derive();

            Assert.Equal(0, previous.QuantityCommittedOut);
            Assert.Equal(0, previous.AvailableToPromise);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenOrderItemIsRejected_ThenNonSerialisedInventoryQuantitiesAreReleased()
        {
            this.InstantiateObjects(this.Session);

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            Assert.Equal(item.QuantityOrdered, item.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);

            var previous = item.ReservedFromNonSerialisedInventoryItem;

            this.Session.Derive();

            item.Reject();

            this.Session.Derive();

            Assert.Equal(0, previous.QuantityCommittedOut);
            Assert.Equal(0, previous.AvailableToPromise);
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

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(new SalesOrderItemStates(this.Session).Created, item.SalesOrderItemState);
            var acl = new AccessControlList(item, this.Session.GetUser());
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

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.order.Confirm();

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item.SalesOrderItemState);
            var acl = new AccessControlList(item, this.Session.GetUser());
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

            this.SetIdentity("admin");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(1).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)this.order.ShipToAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");
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
            var acl = new AccessControlList(item, this.Session.GetUser());
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCancelled_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            item.Cancel();

            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).Cancelled, item.SalesOrderItemState);
            var acl = new AccessControlList(item, this.Session.GetUser());
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsRejected_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            item.Reject();

            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).Rejected, item.SalesOrderItemState);
            var acl = new AccessControlList(item, this.Session.GetUser());
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
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

            this.SetIdentity("admin");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            this.order.Ship();

            this.Session.Derive();

            var shipment = (CustomerShipment)this.order.ShipToAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");
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
            var acl = new AccessControlList(item, this.Session.GetUser());
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

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.SalesOrderState = new SalesOrderStates(this.Session).Finished;

            this.Session.Derive();

            Assert.Equal(new SalesOrderItemStates(this.Session).Finished, item.SalesOrderItemState);
            var acl = new AccessControlList(item, this.Session.GetUser());
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

            this.SetIdentity("admin");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(1).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)this.order.ShipToAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");
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
            var acl = new AccessControlList(item, this.Session.GetUser());
            Assert.False(acl.CanWrite(M.SalesOrderItem.Product));
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
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            Assert.Equal(100, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityPicked);
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

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(120)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            Assert.Equal(120, item1.QuantityOrdered);
            Assert.Equal(0, item1.QuantityPicked);
            Assert.Equal(0, item1.QuantityShipped);
            Assert.Equal(110, item1.QuantityPendingShipment);
            Assert.Equal(120, item1.QuantityReserved);
            Assert.Equal(10, item1.QuantityShortFalled);
            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(120, item1.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(110, item1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            Assert.Equal(120, item1.QuantityOrdered);
            Assert.Equal(0, item1.QuantityPicked);
            Assert.Equal(0, item1.QuantityShipped);
            Assert.Equal(110, item1.QuantityPendingShipment);
            Assert.Equal(120, item1.QuantityReserved);
            Assert.Equal(10, item1.QuantityShortFalled);
            Assert.Equal(0, item1.QuantityRequestsShipping);

            Assert.Equal(10, item2.QuantityOrdered);
            Assert.Equal(0, item2.QuantityPicked);
            Assert.Equal(0, item2.QuantityShipped);
            Assert.Equal(0, item2.QuantityPendingShipment);
            Assert.Equal(10, item2.QuantityReserved);
            Assert.Equal(10, item2.QuantityShortFalled);
            Assert.Equal(0, item2.QuantityRequestsShipping);
            Assert.Equal(130, item2.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
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
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            Assert.Equal(100, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityPicked);
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
            store.IsImmediatelyPicked = false;

            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(100)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            item.QuantityOrdered = 50;

            this.Session.Derive();

            Assert.Equal(50, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityPicked);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(50, item.QuantityPendingShipment);
            Assert.Equal(50, item.QuantityReserved);
            Assert.Equal(0, item.QuantityShortFalled);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(50, item.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(60, item.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(110, item.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenQuantityOrderedIsDecreased_ThenQuantityMayNotBeLessThanQuantityShippped()
        {
            this.InstantiateObjects(this.Session);

            var manual = new OrderKindBuilder(this.Session).WithDescription("manual").WithScheduleManually(true).Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(110).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(120)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);
            this.order.OrderKind = manual;
            this.Session.Derive();

            this.order.Confirm();
            this.Session.Derive();

            item.QuantityShipNow = 100;
            this.Session.Derive();

            var shipment = (CustomerShipment)this.order.ShipToAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");
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

            Assert.Equal(100, item.QuantityShipped);

            item.QuantityOrdered = 90;
            var derivationLog = this.Session.Derive(false);

            Assert.True(derivationLog.HasErrors);
        }

        [Fact]
        public void GivenOrderItemWithSalesRepForThisExactProductCategory_WhenDerivingSalesRep_ThenSalesRepForProductCategoryIsSelected()
        {
            this.InstantiateObjects(this.Session);

            var salesrep1 = new PersonBuilder(this.Session).WithLastName("salesrep for child product category").Build();
            var salesrep2 = new PersonBuilder(this.Session).WithLastName("salesrep for parent category").Build();
            var salesrep3 = new PersonBuilder(this.Session).WithLastName("salesrep for everything else").Build();
            var parentProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("parent")
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("child")
                .WithParent(parentProductCategory).
                Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(childProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(parentProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(this.order.ShipToCustomer)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            this.good.PrimaryProductCategory = childProductCategory;

            this.Session.Derive();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(orderItem.SalesRep, salesrep1);
        }

        [Fact]
        public void GivenOrderItemWithSalesRepForThisProductsParent_WhenDerivingSalesRep_ThenSalesRepForParentCategoryIsSelected()
        {
            this.InstantiateObjects(this.Session);

            var salesrep2 = new PersonBuilder(this.Session).WithLastName("salesrep for parent category").Build();
            var salesrep3 = new PersonBuilder(this.Session).WithLastName("salesrep for everything else").Build();
            var parentProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("parent")
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("child")
                .WithParent(parentProductCategory).
                Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(parentProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(this.order.ShipToCustomer)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            this.good.PrimaryProductCategory = childProductCategory;

            this.Session.Derive();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(orderItem.SalesRep, salesrep2);
        }

        [Fact]
        public void GivenOrderItemForProductWithoutCategory_WhenDerivingSalesRep_ThenSalesRepForCustomerIsSelected()
        {
            this.InstantiateObjects(this.Session);

            var salesrep1 = new PersonBuilder(this.Session).WithLastName("salesrep for child product category").Build();
            var salesrep2 = new PersonBuilder(this.Session).WithLastName("salesrep for parent category").Build();
            var salesrep3 = new PersonBuilder(this.Session).WithLastName("salesrep for everything else").Build();
            var parentProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("parent")
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("child")
                .WithParent(parentProductCategory).
                Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(childProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(parentProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(this.order.ShipToCustomer)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(orderItem);

            this.Session.Derive();

            Assert.Equal(orderItem.SalesRep, salesrep3);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndPendingPickList_WhenQuantityOrderedIsDecreased_ThenPickListQuantityAndShipmentQuantityAreDecreased()
        {
            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            item.QuantityOrdered = 3;

            this.Session.Derive();

            Assert.Equal(3, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(3, pickList.PickListItems[0].RequestedQuantity);
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
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            Assert.Equal(20, item.QuantityShortFalled);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            item.QuantityOrdered = 11;

            this.Session.Derive();

            Assert.Equal(1, item.QuantityShortFalled);

            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            item.QuantityOrdered = 9;

            this.Session.Derive();

            Assert.Equal(0, item.QuantityShortFalled);

            Assert.Equal(9, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(9, pickList.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndPendingPickList_WhenOrderItemIsCancelled_ThenPickListQuantityAndShipmentQuantityAreDecreased()
        {
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(2)
                .WithActualUnitPrice(5)
                .Build();

            var item3 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(7)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.order.AddSalesOrderItem(item2);
            this.order.AddSalesOrderItem(item3);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(2, shipment.ShipmentItems.Count);
            Assert.Equal(5, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(7, shipment.ShipmentItems[1].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(5, pickList.PickListItems[0].RequestedQuantity);
            Assert.Equal(7, pickList.PickListItems[1].RequestedQuantity);

            item1.Cancel();

            this.Session.Derive();

            Assert.Equal(2, shipment.ShipmentItems.Count);
            Assert.Equal(2, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(7, shipment.ShipmentItems[1].Quantity);

            Assert.Equal(2, pickList.PickListItems[0].RequestedQuantity);
            Assert.Equal(7, pickList.PickListItems[1].RequestedQuantity);

            item3.Cancel();

            this.Session.Derive();

            Assert.Equal(1, shipment.ShipmentItems.Count);
            Assert.Equal(2, shipment.ShipmentItems[0].Quantity);

            Assert.Equal(1, pickList.PickListItems.Count);
            Assert.Equal(2, pickList.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndAssignedPickList_WhenQuantityOrderedIsDecreased_ThenNegativePickListIsCreatedAndShipmentQuantityIsDecreased()
        {
            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

            this.Session.Derive();

            item.QuantityOrdered = 3;

            this.Session.Derive();

            var negativePickList = this.order.ShipToCustomer.PickListsWhereShipToParty[1];

            Assert.Equal(3, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);
            Assert.Equal(-7, negativePickList.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndAssignedPickList_WhenOrderItemIsCancelled_ThenNegativePickListIsCreatedAndShipmentItemIsDeleted()
        {
            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(2)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            this.order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)this.order.ShipToCustomer.ShipmentsWhereShipToParty.First;
            Assert.Equal(5, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(5, pickList.PickListItems[0].RequestedQuantity);

            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

            this.Session.Derive();

            item1.Cancel();

            this.Session.Derive();

            var negativePickList = this.order.ShipToCustomer.PickListsWhereShipToParty[1];

            Assert.Equal(1, shipment.ShipmentItems.Count);
            Assert.Equal(2, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(5, pickList.PickListItems[0].RequestedQuantity);
            Assert.Equal(-3, negativePickList.PickListItems[0].RequestedQuantity);
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
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithOrderKind(manual)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(5)
                .WithActualUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item1);
            this.Session.Derive();

            order1.Confirm();
            this.Session.Derive();

            item1.QuantityShipNow = 5;
            var derivationLog = this.Session.Derive(false);

            Assert.True(derivationLog.HasErrors);
            Assert.Contains(M.SalesOrderItem.QuantityShipNow, derivationLog.Errors[0].RoleTypes);

            item1.QuantityShipNow = 3;
            derivationLog = this.Session.Derive();

            Assert.False(derivationLog.HasErrors);
            Assert.Equal(3, item1.QuantityPendingShipment);
        }

        [Fact]
        public void GivenManuallyScheduledOrderItemWithPendingShipmentAndAssignedPickList_WhenQuantityRequestsShippingIsDecreased_ThenNegativePickListIsCreatedAndShipmentQuantityIsDecreased()
        {
            this.InstantiateObjects(this.Session);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(this.part).Build();

            this.Session.Derive();

            var manual = new OrderKindBuilder(this.Session).WithDescription("manual").WithScheduleManually(true).Build();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithOrderKind(manual)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item);

            this.Session.Derive();

            order1.Confirm();

            this.Session.Derive();

            item.QuantityShipNow = 10;

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

            item.QuantityShipNow = -7;
            this.Session.Derive();

            var negativePickList = order1.ShipToCustomer.PickListsWhereShipToParty[1];

            Assert.Equal(3, item.QuantityPendingShipment);
            Assert.Equal(3, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);
            Assert.Equal(-7, negativePickList.PickListItems[0].RequestedQuantity);
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
            this.goodPurchasePrice = (ProductPurchasePrice)session.Instantiate(this.goodPurchasePrice);
            this.virtualGoodPurchasePrice = (ProductPurchasePrice)session.Instantiate(this.virtualGoodPurchasePrice);
            this.currentGoodBasePrice = (BasePrice)session.Instantiate(this.currentGoodBasePrice);
            this.goodSupplierOffering = (SupplierOffering)session.Instantiate(this.goodSupplierOffering);
            this.order = (SalesOrder)session.Instantiate(this.order);
            this.vatRate21 = (VatRate)session.Instantiate(this.vatRate21);
        }
    }
}
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
    using System.Security.Principal;
    using System.Threading;
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
        private FinishedGood part;
        private Colour feature1;
        private Colour feature2;
        private InternalOrganisation internalOrganisation;
        private Organisation shipToCustomer;
        private Organisation billToCustomer;
        private PartyRevenueHistory partyRevenueHistory;
        private PartyProductCategoryRevenueHistory productCategoryRevenueHistory;
        private PartyProductCategoryRevenueHistory ancestorProductCategoryRevenueHistory;
        private PartyProductCategoryRevenueHistory parentProductCategoryRevenueHistory;
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
        private SupplierOffering virtualgoodSupplierOffering;
        private SalesOrder order;
        private VatRate vatRate21;

        
        public SalesOrderItemTests()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");

            this.internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            this.internalOrganisation.PreferredCurrency = euro;

            this.supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();

            this.vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            this.kiev = new CityBuilder(this.DatabaseSession).WithName("Kiev").Build();

            this.shipToContactMechanismMechelen = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            this.shipToContactMechanismKiev = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(this.kiev).WithAddress1("Dnieper").Build();
            this.shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("shipToCustomer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            this.shipToCustomer.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.DatabaseSession)
                                                            .WithContactMechanism(this.shipToContactMechanismKiev)
                                                            .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                                                            .WithUseAsDefault(true)
                                                            .Build());

            this.billToCustomer = new OrganisationBuilder(this.DatabaseSession)
                .WithName("billToCustomer")
                .WithPreferredCurrency(euro)
                .WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer)
                .Build();

            this.billToCustomer.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.DatabaseSession)
                                                            .WithContactMechanism(this.shipToContactMechanismKiev)
                                                            .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                                                            .WithUseAsDefault(true)
                                                            .Build());

            this.part = new FinishedGoodBuilder(this.DatabaseSession).WithName("part").Build();

            this.ancestorProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("ancestor").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            this.parentProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("parent").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(this.ancestorProductCategory)
                .Build();

            this.productCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            this.productCategory.AddParent(this.parentProductCategory);

            this.good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(this.vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithProductCategory(this.productCategory)
                .WithFinishedGood(this.part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(this.internalOrganisation)
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(this.billToCustomer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(this.shipToCustomer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            this.partyRevenueHistory = new PartyRevenueHistoryBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithInternalOrganisation(this.internalOrganisation)
                .WithParty(this.billToCustomer)
                .WithRevenue(100M)
                .Build();

            this.productCategoryRevenueHistory = new PartyProductCategoryRevenueHistoryBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithInternalOrganisation(this.internalOrganisation)
                .WithParty(this.billToCustomer)
                .WithProductCategory(this.productCategory)
                .WithRevenue(100M)
                .WithQuantity(10)
                .Build();

            this.parentProductCategoryRevenueHistory = new PartyProductCategoryRevenueHistoryBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithInternalOrganisation(this.internalOrganisation)
                .WithParty(this.billToCustomer)
                .WithProductCategory(this.parentProductCategory)
                .WithRevenue(100M)
                .WithQuantity(10)
                .Build();

            this.ancestorProductCategoryRevenueHistory = new PartyProductCategoryRevenueHistoryBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithInternalOrganisation(this.internalOrganisation)
                .WithParty(this.billToCustomer)
                .WithProductCategory(this.ancestorProductCategory)
                .WithRevenue(100M)
                .WithQuantity(10)
                .Build();

            this.variantGood = new GoodBuilder(this.DatabaseSession)
               .WithSku("v10101")
               .WithVatRate(this.vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("variant good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
               .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
               .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
               .Build();

            this.variantGood2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("v10102")
                .WithVatRate(this.vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("variant good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            this.virtualGood = new GoodBuilder(this.DatabaseSession)
                .WithSku("v10103")
                .WithVatRate(this.vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("virtual good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVariant(this.variantGood)
                .WithVariant(this.variantGood2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            this.goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.goodSupplierOffering = new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(this.goodPurchasePrice)
                .Build();

            this.virtualGoodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(8)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.virtualgoodSupplierOffering = new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(this.virtualGood)
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(this.virtualGoodPurchasePrice)
                .Build();

            this.feature1 = new ColourBuilder(this.DatabaseSession)
                .WithName("white")
                .WithVatRate(this.vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                                            .WithText("white")
                                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                                            .Build())
                .Build();

            this.feature2 = new ColourBuilder(this.DatabaseSession)
                .WithName("black")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                                            .WithText("black")
                                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                                            .Build())
                .Build();

            this.currentBasePriceGeoBoundary = new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(this.internalOrganisation)
                .WithDescription("current BasePriceGeoBoundary ")
                .WithGeographicBoundary(mechelen)
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            // previous basePrice for good
            new BasePriceBuilder(this.DatabaseSession).WithDescription("previous good")
                .WithSpecifiedFor(this.internalOrganisation)
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for good
            new BasePriceBuilder(this.DatabaseSession).WithDescription("future good")
                .WithSpecifiedFor(this.internalOrganisation)
                .WithProduct(this.good)
                .WithPrice(11)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.currentGoodBasePrice = new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(this.internalOrganisation)
                .WithDescription("current good")
                .WithProduct(this.good)
                .WithPrice(10)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature1
            new BasePriceBuilder(this.DatabaseSession).WithDescription("previous feature1")
                .WithSpecifiedFor(this.internalOrganisation)
                .WithProductFeature(this.feature1)
                .WithPrice(0.5M)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for feature1
            new BasePriceBuilder(this.DatabaseSession).WithDescription("future feature1")
                .WithSpecifiedFor(this.internalOrganisation)
                .WithProductFeature(this.feature1)
                .WithPrice(2.5M)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(this.internalOrganisation)
                .WithDescription("current feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(2)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature2
            new BasePriceBuilder(this.DatabaseSession).WithDescription("previous feature2")
                .WithSpecifiedFor(this.internalOrganisation)
                .WithProductFeature(this.feature2)
                .WithPrice(2)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for feature2
            new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(this.internalOrganisation)
                .WithDescription("future feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(4)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.currentFeature2BasePrice = new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(this.internalOrganisation)
                .WithDescription("current feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(3)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for good with feature1
            new BasePriceBuilder(this.DatabaseSession).WithDescription("previous good/feature1")
                .WithSpecifiedFor(this.internalOrganisation)
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(4)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for good with feature1
            new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(this.internalOrganisation)
                .WithDescription("future good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(6)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.currentGood1Feature1BasePrice = new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(this.internalOrganisation)
                .WithDescription("current good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(5)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(this.internalOrganisation)
                .WithDescription("current variant good2")
                .WithProduct(this.variantGood2)
                .WithPrice(11)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
        }

        [Fact]
        public void GivenOrderItemWithVatRegime_WhenDeriving_ThenDerivedVatRegimeIsFromOrderItem()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(1)
                .Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(orderItem.VatRegime, orderItem.VatRegime);
        }

        [Fact]
        public void GivenOrderItemWithoutVatRegime_WhenDeriving_ThenDerivedVatRegimeIsFromOrder()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(salesOrder.VatRegime, orderItem.VatRegime);
        }

        [Fact]
        public void GivenOrderItemWithoutVatRate_WhenDeriving_ThenItemDerivedVatRateIsFromOrderVatRegime()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var expected = new VatRegimes(this.DatabaseSession).Export.VatRate;

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(salesOrder.VatRegime, orderItem.VatRegime);
            Assert.Equal(expected, orderItem.DerivedVatRate);
        }

        [Fact]
        public void GivenOrderItemWithoutVatRateAndOrderWithoutVatRegime_WhenDeriving_ThenItemDerivedVatRateIsFromOrderedProduct()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var expected = this.good.VatRate;

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(salesOrder.VatRegime, orderItem.VatRegime);
            Assert.Equal(expected, orderItem.DerivedVatRate);
        }

        [Fact]
        public void GivenOrderItemWithAssignedDeliveryDate_WhenDeriving_ThenDeliveryDateIsOrderItemAssignedDeliveryDate()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithShipToCustomer(this.billToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(1)
                .WithAssignedDeliveryDate(DateTime.UtcNow.AddMonths(1))
                .Build();

            salesOrder.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(orderItem.DeliveryDate, orderItem.AssignedDeliveryDate);
        }

        [Fact]
        public void GivenOrderItemWithoutDeliveryDate_WhenDeriving_ThenDerivedDeliveryDateIsOrderDeliveryDate()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithShipToCustomer(this.billToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .WithDeliveryDate(DateTime.UtcNow.AddMonths(1))
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(1)
                .Build();

            salesOrder.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(orderItem.DeliveryDate, salesOrder.DeliveryDate);
        }

        [Fact]
        public void GivenOrderItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var builder = new SalesOrderItemBuilder(this.DatabaseSession);
            var orderItem = builder.Build();

            this.order.AddSalesOrderItem(orderItem);

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithProduct(this.good);
            orderItem = builder.Build();

            this.order.AddSalesOrderItem(orderItem);

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            builder.WithProductFeature(this.feature1);
            orderItem = builder.Build();

            this.order.AddSalesOrderItem(orderItem);

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrderItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var item = new SalesOrderItemBuilder(this.DatabaseSession).Build();

            Assert.Equal(new SalesOrderItemObjectStates(this.DatabaseSession).Created, item.CurrentObjectState);
        }

        [Fact]
        public void GivenOrderItemWithOrderedWithFeature_WhenDeriving_ThenOrderedWithFeatureOrderItemMustBeForProductFeature()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            var productOrderItem = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            var productFeatureOrderItem = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProductFeature(this.feature1)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            productOrderItem.AddOrderedWithFeature(productFeatureOrderItem);
            salesOrder.AddSalesOrderItem(productOrderItem);
            salesOrder.AddSalesOrderItem(productFeatureOrderItem);

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            productFeatureOrderItem.RemoveProductFeature();
            productFeatureOrderItem.Product = this.good;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrderItemWithoutCustomer_WhenDeriving_ShipToAddressIsNull()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession).Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Null(orderItem.ShipToAddress);
            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrderItemWithoutShipToAddress_WhenDeriving_ThenDerivedShipToAddressIsFromOrder()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(this.shipToContactMechanismMechelen, orderItem.ShipToAddress);
        }

        [Fact]
        public void GivenOrderItemWithoutShipToParty_WhenDeriving_ThenDerivedShipToPartyIsFromOrder()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(this.shipToCustomer, orderItem.ShipToParty);
        }

        [Fact]
        public void GivenOrderItemForGoodWithoutSelectedInventoryItem_WhenConfirming_ThenReservedFromInventoryItemIsFromDefaultFacility()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(this.vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation"))
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithFromDate(DateTime.UtcNow)
                .WithSupplier(this.supplier)
                .WithProductPurchasePrice(good2PurchasePrice)
                .Build();

            //// good with part as inventory item
            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(new Warehouses(this.DatabaseSession).FindBy(M.Warehouse.Name, "facility"), item1.ReservedFromInventoryItem.Facility);
            Assert.Equal(new Warehouses(this.DatabaseSession).FindBy(M.Warehouse.Name, "facility"), item2.ReservedFromInventoryItem.Facility);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenReservedFromInventoryItemChangesValue_ThenQuantitiesAreMovedFromOldToNewInventoryItem()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var secondWarehouse = new WarehouseBuilder(this.DatabaseSession)
                .WithName("affiliate warehouse")
                .WithOwner(this.internalOrganisation)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(3, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityPicked);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(3, item.QuantityReserved);
            Assert.Equal(3, item.QuantityShortFalled);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(3, item.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(0, item.ReservedFromInventoryItem.QuantityOnHand);

            var previous = item.ReservedFromInventoryItem;
            var current = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithFacility(secondWarehouse).WithPart(this.part).Build();

            item.ReservedFromInventoryItem = current;

            this.DatabaseSession.Derive();

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
            this.InstantiateObjects(this.DatabaseSession);

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(item.QuantityOrdered, item.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item.ReservedFromInventoryItem.AvailableToPromise);

            var previous = item.ReservedFromInventoryItem;

            this.DatabaseSession.Derive();

            item.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(0, previous.QuantityCommittedOut);
            Assert.Equal(0, previous.AvailableToPromise);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenOrderItemIsRejected_ThenNonSerialisedInventoryQuantitiesAreReleased()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(item.QuantityOrdered, item.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item.ReservedFromInventoryItem.AvailableToPromise);

            var previous = item.ReservedFromInventoryItem;

            this.DatabaseSession.Derive();

            item.Reject();

            this.DatabaseSession.Derive();

            Assert.Equal(0, previous.QuantityCommittedOut);
            Assert.Equal(0, previous.AvailableToPromise);
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCreated_ThenItemMayBeDeletedButNotCancelledOrRejected()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            Assert.Equal(new SalesOrderItemObjectStates(this.DatabaseSession).Created, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.True(acl.CanExecute(M.SalesOrderItem.Delete));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsConfirmed_ThenItemMayBeCancelledOrRejectedButNotDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.order.Confirm();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            Assert.Equal(new SalesOrderItemObjectStates(this.DatabaseSession).InProcess, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.True(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.True(acl.CanExecute(M.SalesOrderItem.Reject));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsPartiallyShipped_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.SetIdentity("admin");

            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart[0];
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(1).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)this.order.ShipToAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");
            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(); 
            
            shipment.Ship();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderItemObjectStates(this.DatabaseSession).PartiallyShipped, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCancelled_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            item.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderItemObjectStates(this.DatabaseSession).Cancelled, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsRejected_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
            
            this.InstantiateObjects(this.DatabaseSession);

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);
            
            this.DatabaseSession.Derive();

            item.Reject();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderItemObjectStates(this.DatabaseSession).Rejected, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCompleted_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.SetIdentity("admin");

            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart[0];
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(110).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();
            
            var shipment = (CustomerShipment)this.order.ShipToAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");
            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(); 
            
            shipment.Ship();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderItemObjectStates(this.DatabaseSession).Completed, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsFinished_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.SetIdentity("admin");

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Finish();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderItemObjectStates(this.DatabaseSession).Finished, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsPartiallyShipped_ThenProductChangeIsNotAllowed()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.SetIdentity("admin");

            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart[0];
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(1).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)this.order.ShipToAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");
            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(); 
            
            shipment.Ship();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderItemObjectStates(this.DatabaseSession).PartiallyShipped, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanWrite(M.SalesOrderItem.Product));
        }

        [Fact]
        public void GivenOrderItemForGoodWithEnoughStockAvailable_WhenConfirming_ThenQuantitiesReservedAndRequestsShippingAreEqualToQuantityOrdered()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.DatabaseSession.Instantiate(this.good.FinishedGood.IInventoryItemsWherePart[0]);
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(110).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(100)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(100, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityPicked);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(100, item.QuantityPendingShipment);
            Assert.Equal(100, item.QuantityReserved);
            Assert.Equal(0, item.QuantityShortFalled);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(100, item.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(10, item.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(110, item.ReservedFromInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenOrderItemForGoodWithNotEnoughStockAvailable_WhenConfirming_ThenQuantitiesReservedAndRequestsShippingAreEqualToInventoryAvailableToPromise()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.DatabaseSession.Instantiate(this.good.FinishedGood.IInventoryItemsWherePart[0]);
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(110).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(120)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive();

            this.order.Confirm();
            
            this.DatabaseSession.Derive();
            
            Assert.Equal(120, item1.QuantityOrdered);
            Assert.Equal(0, item1.QuantityPicked);
            Assert.Equal(0, item1.QuantityShipped);
            Assert.Equal(110, item1.QuantityPendingShipment);
            Assert.Equal(120, item1.QuantityReserved);
            Assert.Equal(10, item1.QuantityShortFalled);
            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(120, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(110, item1.ReservedFromInventoryItem.QuantityOnHand);

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

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
            Assert.Equal(130, item2.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item2.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(110, item2.ReservedFromInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenShipmentIsCreated_ThenQuantitiesRequestsShippingIsSetToZero()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.DatabaseSession.Instantiate(this.good.FinishedGood.IInventoryItemsWherePart[0]);
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(110).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(100)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(100, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityPicked);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(100, item.QuantityPendingShipment);
            Assert.Equal(100, item.QuantityReserved);
            Assert.Equal(0, item.QuantityShortFalled);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(100, item.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(10, item.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(110, item.ReservedFromInventoryItem.QuantityOnHand);

            Assert.Equal(100, item.OrderShipmentsWhereSalesOrderItem[0].Quantity);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenQuantityOrderedIsDecreased_ThenQuantitiesReservedAndRequestsShippingAndInventoryAvailableToPromiseDecreaseEqually()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.DatabaseSession.Instantiate(this.good.FinishedGood.IInventoryItemsWherePart[0]);
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(110).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(100)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            item.QuantityOrdered = 50;

            this.DatabaseSession.Derive();

            Assert.Equal(50, item.QuantityOrdered);
            Assert.Equal(0, item.QuantityPicked);
            Assert.Equal(0, item.QuantityShipped);
            Assert.Equal(50, item.QuantityPendingShipment);
            Assert.Equal(50, item.QuantityReserved);
            Assert.Equal(0, item.QuantityShortFalled);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(50, item.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(60, item.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(110, item.ReservedFromInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenQuantityOrderedIsDecreased_ThenQuantityMayNotBeLessThanQuantityShippped()
        {
            this.InstantiateObjects(this.DatabaseSession);
            
            var manual = new OrderKindBuilder(this.DatabaseSession).WithDescription("manual").WithScheduleManually(true).Build();

            var testPart = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").Build();
            var testgood = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(this.vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(testPart)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1InventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithPart(testPart).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(110).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(testgood)
                .WithQuantityOrdered(120)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);
            this.order.OrderKind = manual;
            this.DatabaseSession.Derive();

            this.order.Confirm();
            this.DatabaseSession.Derive();

            item.QuantityShipNow = 100;
            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)this.order.ShipToAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");
            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            shipment.Ship();
            this.DatabaseSession.Derive();

            Assert.Equal(100, item.QuantityShipped);

            item.QuantityOrdered = 90;
            var derivationLog = this.DatabaseSession.Derive(false);

            Assert.True(derivationLog.HasErrors);
        }

        [Fact]
        public void GivenOrderItemWithSalesRepForThisExactProductCategory_WhenDerivingSalesRep_ThenSalesRepForProductCategoryIsSelected()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesrep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for child product category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for parent category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for everything else").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var parentProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("parent").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("child").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(parentProductCategory).
                Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(childProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(parentProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(this.order.ShipToCustomer)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            this.good.PrimaryProductCategory = childProductCategory;

            this.DatabaseSession.Derive();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(orderItem.SalesRep, salesrep1);
        }

        [Fact]
        public void GivenOrderItemWithSalesRepForThisProductsParent_WhenDerivingSalesRep_ThenSalesRepForParentCategoryIsSelected()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesrep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for parent category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for everything else").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var parentProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("parent").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("child").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(parentProductCategory).
                Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(parentProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(this.order.ShipToCustomer)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            this.good.PrimaryProductCategory = childProductCategory;

            this.DatabaseSession.Derive();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(orderItem.SalesRep, salesrep2);
        }

        [Fact]
        public void GivenOrderItemForProductWithoutCategory_WhenDerivingSalesRep_ThenSalesRepForCustomerIsSelected()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesrep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for child product category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for parent category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for everything else").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var parentProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("parent").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("child").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(parentProductCategory).
                Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(childProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(this.order.ShipToCustomer)
                .WithProductCategory(parentProductCategory)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(this.order.ShipToCustomer)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            var orderItem = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(orderItem);

            this.DatabaseSession.Derive();

            Assert.Equal(orderItem.SalesRep, salesrep3);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndPendingPickList_WhenQuantityOrderedIsDecreased_ThenPickListQuantityAndShipmentQuantityAreDecreased()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart.First;
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(); 
            
            this.order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            item.QuantityOrdered = 3;

            this.DatabaseSession.Derive();

            Assert.Equal(3, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(3, pickList.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndItemsShortFalled_WhenQuantityOrderedIsDecreased_ThenItemsShortFalledIsDecreasedAndShipmentIsLeftUnchanged()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart.First;
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(30)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(20, item.QuantityShortFalled);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            item.QuantityOrdered = 11;

            this.DatabaseSession.Derive();

            Assert.Equal(1, item.QuantityShortFalled);

            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            item.QuantityOrdered = 9;

            this.DatabaseSession.Derive();

            Assert.Equal(0, item.QuantityShortFalled);

            Assert.Equal(9, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(9, pickList.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndPendingPickList_WhenOrderItemIsCancelled_ThenPickListQuantityAndShipmentQuantityAreDecreased()
        {
            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart.First;
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            good2inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(2)
                .WithActualUnitPrice(5)
                .Build();

            var item3 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithQuantityOrdered(7)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.order.AddSalesOrderItem(item2);
            this.order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(2, shipment.ShipmentItems.Count);
            Assert.Equal(5, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(7, shipment.ShipmentItems[1].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(5, pickList.PickListItems[0].RequestedQuantity);
            Assert.Equal(7, pickList.PickListItems[1].RequestedQuantity);

            item1.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(2, shipment.ShipmentItems.Count);
            Assert.Equal(2, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(7, shipment.ShipmentItems[1].Quantity);

            Assert.Equal(2, pickList.PickListItems[0].RequestedQuantity);
            Assert.Equal(7, pickList.PickListItems[1].RequestedQuantity);

            item3.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(1, shipment.ShipmentItems.Count);
            Assert.Equal(2, shipment.ShipmentItems[0].Quantity);

            Assert.Equal(1, pickList.PickListItems.Count);
            Assert.Equal(2, pickList.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndAssignedPickList_WhenQuantityOrderedIsDecreased_ThenNegativePickListIsCreatedAndShipmentQuantityIsDecreased()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart.First;
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(); 
            
            this.order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            this.DatabaseSession.Derive();

            item.QuantityOrdered = 3;

            this.DatabaseSession.Derive();

            var negativePickList = this.order.ShipToCustomer.PickListsWhereShipToParty[1];

            Assert.Equal(3, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);
            Assert.Equal(-7, negativePickList.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenOrderItemWithPendingShipmentAndAssignedPickList_WhenOrderItemIsCancelled_ThenNegativePickListIsCreatedAndShipmentItemIsDeleted()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart.First;
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(2)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive();
            
            this.order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)this.order.ShipToCustomer.ShipmentsWhereShipToParty.First;
            Assert.Equal(5, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(5, pickList.PickListItems[0].RequestedQuantity);

            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            this.DatabaseSession.Derive();
            
            item1.Cancel();

            this.DatabaseSession.Derive();

            var negativePickList = this.order.ShipToCustomer.PickListsWhereShipToParty[1];

            Assert.Equal(1, shipment.ShipmentItems.Count);
            Assert.Equal(2, shipment.ShipmentItems[0].Quantity);
            Assert.Equal(5, pickList.PickListItems[0].RequestedQuantity);
            Assert.Equal(-3, negativePickList.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenManuallyScheduledOrderItem_WhenScheduled_ThenQuantityCannotExceedInventoryAvailableToPromise()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart.First;
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(3).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var manual = new OrderKindBuilder(this.DatabaseSession).WithDescription("manual").WithScheduleManually(true).Build();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithOrderKind(manual)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(5)
                .WithActualUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item1);
            this.DatabaseSession.Derive(); 
            
            order1.Confirm();
            this.DatabaseSession.Derive();

            item1.QuantityShipNow = 5;          
            var derivationLog = this.DatabaseSession.Derive(false);

            Assert.True(derivationLog.HasErrors);
            Assert.Contains(M.SalesOrderItem.QuantityShipNow, derivationLog.Errors[0].RoleTypes);

            item1.QuantityShipNow = 3;
            derivationLog = this.DatabaseSession.Derive();

            Assert.False(derivationLog.HasErrors);
            Assert.Equal(3, item1.QuantityPendingShipment);
        }

        [Fact]
        public void GivenManuallyScheduledOrderItemWithPendingShipmentAndAssignedPickList_WhenQuantityRequestsShippingIsDecreased_ThenNegativePickListIsCreatedAndShipmentQuantityIsDecreased()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var inventoryItem = (NonSerialisedInventoryItem)this.part.IInventoryItemsWherePart.First;
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var manual = new OrderKindBuilder(this.DatabaseSession).WithDescription("manual").WithScheduleManually(true).Build();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithTakenByInternalOrganisation(this.internalOrganisation)
                .WithShipToAddress(this.shipToContactMechanismMechelen)
                .WithOrderKind(manual)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(); 
            
            order1.Confirm();
            
            this.DatabaseSession.Derive();

            item.QuantityShipNow = 10;

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            item.QuantityShipNow = -7;
            this.DatabaseSession.Derive();

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
            this.part = (FinishedGood)session.Instantiate(this.part);
            this.good = (Good)session.Instantiate(this.good);
            this.feature1 = (Colour)session.Instantiate(this.feature1);
            this.feature2 = (Colour)session.Instantiate(this.feature2);
            this.internalOrganisation = (InternalOrganisation)session.Instantiate(this.internalOrganisation);
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
            this.virtualgoodSupplierOffering = (SupplierOffering)session.Instantiate(this.virtualgoodSupplierOffering);
            this.order = (SalesOrder)session.Instantiate(this.order);
            this.vatRate21 = (VatRate)session.Instantiate(this.vatRate21);
        }
    }
}
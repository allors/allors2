// <copyright file="SalesOrderItemPricingTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System;
    using Allors.Meta;
    using Xunit;

    public class SalesOrderItemPricingTests : DomainTest
    {
        private ProductCategory productCategory;
        private ProductCategory ancestorProductCategory;
        private ProductCategory parentProductCategory;
        private Good good;
        private readonly Good variantGood;
        private readonly Good variantGood2;
        private readonly Good virtualGood;
        private Part part;
        private Colour feature1;
        private Colour feature2;
        private Singleton internalOrganisation;
        private Organisation shipToCustomer;
        private Organisation billToCustomer;
        private Organisation supplier;
        private City kiev;
        private PostalAddress shipToContactMechanismMechelen;
        private PostalAddress shipToContactMechanismKiev;
        private BasePrice currentBasePriceGeoBoundary;
        private BasePrice currentGoodBasePrice;
        private readonly BasePrice currentVirtualGoodBasePrice;
        private BasePrice currentGood1Feature1BasePrice;
        private BasePrice currentFeature2BasePrice;
        private SupplierOffering goodPurchasePrice;
        private SalesOrder order;
        private VatRate vatRate21;

        public SalesOrderItemPricingTests()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            this.internalOrganisation = this.Session.GetSingleton();

            this.supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();

            this.vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

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
            this.Session.Derive();

            this.billToCustomer = new OrganisationBuilder(this.Session)
                .WithName("billToCustomer")
                .WithPreferredCurrency(euro)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(this.billToCustomer).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(this.shipToCustomer).Build();

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
                .WithName("good")
                .WithPart(this.part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.productCategory.AddProduct(this.good);

            this.variantGood = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithName("variant good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("p1")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            this.parentProductCategory.AddProduct(this.variantGood);

            this.variantGood2 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v10102")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithName("variant good2")
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("p2")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            this.parentProductCategory.AddProduct(this.variantGood2);

            this.virtualGood = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithName("virtual good")
                .WithVariant(this.variantGood)
                .WithVariant(this.variantGood2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.parentProductCategory.AddProduct(this.virtualGood);

            this.goodPurchasePrice = new SupplierOfferingBuilder(this.Session)
                .WithPart(this.part)
                .WithSupplier(this.supplier)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            this.feature1 = new ColourBuilder(this.Session)
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
                .WithFromDate(this.Session.Now().AddMinutes(-1))
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
                .WithFromDate(this.Session.Now().AddMinutes(-1))
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
                .WithFromDate(this.Session.Now().AddMinutes(-1))
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
                .WithFromDate(this.Session.Now().AddMinutes(-1))
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
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.currentVirtualGoodBasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current virtual good")
                .WithProduct(this.virtualGood)
                .WithPrice(10)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current variant good2")
                .WithProduct(this.variantGood2)
                .WithPrice(11)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.order = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedBillToContactMechanism(this.shipToContactMechanismMechelen)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.order.SetReadyForPosting();
            this.Session.Derive();

            this.order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive(true);

            this.Session.Commit();
        }

        [Fact]
        public void GivenOrderItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            this.InstantiateObjects(this.Session);

            var item = new SalesOrderItemBuilder(this.Session).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build();

            Assert.Equal(new SalesOrderItemStates(this.Session).Provisional, item.SalesOrderItemState);
        }

        [Fact]
        public void GivenOrderItemForGood1WithActualPrice_WhenDerivingPrices_ThenUseActualPrice()
        {
            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(15)
                .WithAssignedVatRegime(new VatRegimes(this.Session).DutchStandardTariff)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.Session.Derive();

            Assert.Equal(10, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(15, item1.UnitPrice);
            Assert.Equal(3.15m, item1.UnitVat);
            Assert.Equal(30, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(45, item1.TotalExVat);
            Assert.Equal(9.45m, item1.TotalVat);
            Assert.Equal(54.45m, item1.TotalIncVat);

            Assert.Equal(30, this.order.TotalBasePrice);
            Assert.Equal(0, this.order.TotalDiscount);
            Assert.Equal(0, this.order.TotalSurcharge);
            Assert.Equal(45, this.order.TotalExVat);
            Assert.Equal(9.45m, this.order.TotalVat);
            Assert.Equal(54.45m, this.order.TotalIncVat);
            Assert.Equal(45, this.order.TotalListPrice);
        }

        [Fact]
        public void GivenOrderItemForGood1_WhenDerivingPrices_ThenUsePriceComponentsForGood1()
        {
            this.InstantiateObjects(this.Session);

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(quantityOrdered)
                .WithAssignedVatRegime(new VatRegimes(this.Session).DutchStandardTariff)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);
            Assert.Equal(Math.Round(item1.UnitPrice * this.vatRate21.Rate / 100, 2), item1.UnitVat);

            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, item1.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * this.vatRate21.Rate / 100, 2) * quantityOrdered, item1.TotalVat);

            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, this.order.TotalBasePrice);
            Assert.Equal(0, this.order.TotalDiscount);
            Assert.Equal(0, this.order.TotalSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, this.order.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * this.vatRate21.Rate / 100, 2) * quantityOrdered, this.order.TotalVat);
        }

        [Fact]
        public void GivenOrderItemForGood1WithFeature1_WhenDerivingPrices_ThenUsePriceComponentsForGood1WithFeature1()
        {
            this.InstantiateObjects(this.Session);

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductFeatureItem).WithProductFeature(this.feature1).WithQuantityOrdered(quantityOrdered).Build();
            item1.AddOrderedWithFeature(item2);
            this.order.AddSalesOrderItem(item1);
            this.order.AddSalesOrderItem(item2);
            this.Session.Derive();

            var expectedCalculatedUnitPrice = this.currentGoodBasePrice.Price;
            expectedCalculatedUnitPrice += this.currentGood1Feature1BasePrice.Price;

            var expectedTotalBasePrice = this.currentGoodBasePrice.Price * quantityOrdered;
            expectedTotalBasePrice += this.currentGood1Feature1BasePrice.Price * quantityOrdered;

            var expectedTotalExVat = this.currentGoodBasePrice.Price * quantityOrdered;
            expectedTotalExVat += this.currentGood1Feature1BasePrice.Price * quantityOrdered;

            Assert.Equal(expectedCalculatedUnitPrice, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(expectedCalculatedUnitPrice, item1.UnitPrice);
            Assert.Equal(expectedTotalBasePrice, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(expectedTotalExVat, item1.TotalExVat);

            Assert.Equal(this.currentGood1Feature1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1Feature1BasePrice.Price, item2.UnitPrice);
            Assert.Equal(this.currentGood1Feature1BasePrice.Price * quantityOrdered, item2.TotalBasePrice);
            Assert.Equal(0, item2.TotalDiscount);
            Assert.Equal(0, item2.TotalSurcharge);
            Assert.Equal(this.currentGood1Feature1BasePrice.Price * quantityOrdered, item2.TotalExVat);

            Assert.Equal(expectedTotalBasePrice, this.order.TotalBasePrice);
            Assert.Equal(0, this.order.TotalDiscount);
            Assert.Equal(0, this.order.TotalSurcharge);
            Assert.Equal(expectedTotalExVat, this.order.TotalExVat);
        }

        [Fact]
        public void GivenOrderItemForGood1WithFeature2_WhenDerivingPrices_ThenUsePriceComponentsForGood1AndFeature2()
        {
            this.InstantiateObjects(this.Session);

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductFeatureItem).WithProductFeature(this.feature2).WithQuantityOrdered(quantityOrdered).Build();
            item1.AddOrderedWithFeature(item2);
            this.order.AddSalesOrderItem(item1);
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            var expectedCalculatedUnitPrice = this.currentGoodBasePrice.Price;
            expectedCalculatedUnitPrice += this.currentFeature2BasePrice.Price;

            var expectedTotalBasePrice = this.currentGoodBasePrice.Price * quantityOrdered;
            expectedTotalBasePrice += this.currentFeature2BasePrice.Price * quantityOrdered;

            var expectedTotalExVat = this.currentGoodBasePrice.Price * quantityOrdered;
            expectedTotalExVat += this.currentFeature2BasePrice.Price * quantityOrdered;

            Assert.Equal(expectedCalculatedUnitPrice, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(expectedCalculatedUnitPrice, item1.UnitPrice);
            Assert.Equal(expectedTotalBasePrice, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(expectedTotalExVat, item1.TotalExVat);

            Assert.Equal(this.currentFeature2BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentFeature2BasePrice.Price, item2.UnitPrice);
            Assert.Equal(this.currentFeature2BasePrice.Price * quantityOrdered, item2.TotalBasePrice);
            Assert.Equal(0, item2.TotalDiscount);
            Assert.Equal(0, item2.TotalSurcharge);
            Assert.Equal(this.currentFeature2BasePrice.Price * quantityOrdered, item2.TotalExVat);

            Assert.Equal(expectedTotalBasePrice, this.order.TotalBasePrice);
            Assert.Equal(0, this.order.TotalDiscount);
            Assert.Equal(0, this.order.TotalSurcharge);
            Assert.Equal(expectedTotalExVat, this.order.TotalExVat);
        }

        [Fact]
        public void GivenProductWithMultipleBasePrices_WhenDeriving_ThenLowestUnitPriceMustBeCalculated()
        {
            this.InstantiateObjects(this.Session);

            this.order.Revise();
            this.Session.Derive();

            this.order.AssignedShipToAddress = this.shipToContactMechanismMechelen;
            this.Session.Derive();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(3).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentBasePriceGeoBoundary.Price, item1.UnitPrice);

            var order2 = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            this.Session.Derive();

            item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(3).Build();
            order2.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountAmountForGeoBoundary_WhenDeriving_ThenUseDiscountComponentsForGeoBoundary()
        {
            const decimal quantityOrdered = 3;
            const decimal amount = 1;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(Math.Round(amount * quantityOrdered / item1.TotalBasePrice * 100, 2), item1.TotalDiscountAsPercentage);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountPercentageForGeoBoundary_WhenDeriving_ThenUseDiscountComponentsForGeoBoundary()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(percentage, item1.TotalDiscountAsPercentage);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargeAdjustment_WhenDeriving_ThenCalculateSellingPriceUsingItemSurchargeAdjustment()
        {
            const decimal quantityOrdered = 3;
            const decimal discountAmount = 1;
            const decimal surchargePercentage = 5;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithPrice(discountAmount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(quantityOrdered)
                .WithSurchargeAdjustment(new SurchargeAdjustmentBuilder(this.Session).WithPercentage(surchargePercentage).Build())
                .Build();

            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var surcharge = Math.Round((price - discountAmount) * surchargePercentage / 100, 2);
            var adjustmentAmount = Math.Round(0 - discountAmount + surcharge, 2);

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(discountAmount, item1.UnitDiscount);
            Assert.Equal(surcharge, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + adjustmentAmount, item1.UnitPrice);
            Assert.Equal(adjustmentAmount * quantityOrdered, item1.TotalOrderAdjustment);
        }

        [Fact]
        public void GivenOrderItemWithSurchargeAmountForGeoBoundary_WhenDeriving_ThenUseSurchargeComponentsForGeoBoundary()
        {
            const decimal quantityOrdered = 3;
            const decimal amount = 1;

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargePercentageForGeoBoundary_WhenDeriving_ThenUseSurchargeComponentsForGeoBoundary()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountAmountForPartyClassification_WhenDeriving_ThenUseDiscountComponentsForPartyClassification()
        {
            const decimal quantityOrdered = 3;
            const decimal amount = 1;

            var classification = new IndustryClassificationBuilder(this.Session).WithName("gold customer").Build();
            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for party classification")
                .WithPartyClassification(classification)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            ((OrganisationDerivedRoles)this.billToCustomer).AddPartyClassification(classification);

            this.order.ShipToCustomer = this.shipToCustomer;

            this.Session.Derive();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(Math.Round(amount * quantityOrdered / item1.TotalBasePrice * 100, 2), item1.TotalDiscountAsPercentage);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountPercentageForPartyClassification_WhenDeriving_ThenUseDiscountComponentsForPartyClassification()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;

            var classification = new IndustryClassificationBuilder(this.Session).WithName("gold customer").Build();
            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for party classification")
                .WithPartyClassification(classification)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            ((OrganisationDerivedRoles)this.billToCustomer).AddPartyClassification(classification);

            this.order.ShipToCustomer = this.shipToCustomer;

            this.Session.Derive();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(percentage, item1.TotalDiscountAsPercentage);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargeAmountForPartyClassification_WhenDeriving_ThenUseSurchargeComponentsForPartyClassification()
        {
            const decimal quantityOrdered = 3;
            const decimal expected = 1;

            var classification = new IndustryClassificationBuilder(this.Session).WithName("gold customer").Build();
            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for party classification")
                .WithPartyClassification(classification)
                .WithProduct(this.good)
                .WithPrice(expected)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            ((OrganisationDerivedRoles)this.billToCustomer).AddPartyClassification(classification);

            this.order.ShipToCustomer = this.shipToCustomer;

            this.Session.Derive();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(expected, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + expected, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargePercentageForPartyClassification_WhenDeriving_ThenUseSurchargeComponentsForPartyClassification()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;

            var classification = new IndustryClassificationBuilder(this.Session).WithName("gold customer").Build();
            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for party classification")
                .WithPartyClassification(classification)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            ((OrganisationDerivedRoles)this.billToCustomer).AddPartyClassification(classification);

            this.order.ShipToCustomer = this.shipToCustomer;

            this.Session.Derive();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountAmountForAncestorProductCategory_WhenDeriving_ThenUseDiscountComponentsForAncestorProductCategory()
        {
            const decimal quantityOrdered = 3;
            const decimal expected = 1;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for ancestor category")
                .WithProductCategory(this.ancestorProductCategory)
                .WithProduct(this.good)
                .WithPrice(expected)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(expected, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(Math.Round(expected * quantityOrdered / item1.TotalBasePrice * 100, 2), item1.TotalDiscountAsPercentage);
            Assert.Equal(this.currentGoodBasePrice.Price - expected, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountPercentageForAncestorProductCategory_WhenDeriving_ThenUseDiscountComponentsForAncestorProductCategory()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for ancestor category")
                .WithProductCategory(this.ancestorProductCategory)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(percentage, item1.TotalDiscountAsPercentage);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountAmountForParentProductCategory_WhenDeriving_ThenUseDiscountComponentsForParentProductCategory()
        {
            const decimal quantityOrdered = 3;
            const decimal amount = 1;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for parent category")
                .WithProductCategory(this.parentProductCategory)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(Math.Round(amount * quantityOrdered / item1.TotalBasePrice * 100, 2), item1.TotalDiscountAsPercentage);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountPercentageForParentProductCategory_WhenDeriving_ThenUseDiscountComponentsForParentProductCategory()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for parent category")
                .WithProductCategory(this.parentProductCategory)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(percentage, item1.TotalDiscountAsPercentage);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountAmountForProductCategory_WhenDeriving_ThenUseDiscountComponentsForProductCategory()
        {
            const decimal quantityOrdered = 3;
            const decimal amount = 1;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for product category")
                .WithProductCategory(this.productCategory)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(Math.Round(amount * quantityOrdered / item1.TotalBasePrice * 100, 2), item1.TotalDiscountAsPercentage);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountPercentageForProductCategory_WhenDeriving_ThenUseDiscountComponentsForProductCategory()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for product category")
                .WithProductCategory(this.productCategory)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(percentage, item1.TotalDiscountAsPercentage);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargeAmountForProductCategory_WhenDeriving_ThenUseSurchargeComponentsForProductCategory()
        {
            const decimal quantityOrdered = 3;
            const decimal amount = 1;

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for product category")
                .WithProductCategory(this.productCategory)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargePercentageForProductCategory_WhenDeriving_ThenUseSurchargeComponentsForProductCategory()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for product category")
                .WithProductCategory(this.productCategory)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountAmountForOrderQuantityBreak_WhenDeriving_ThenUseDiscountComponentsForOrderQuantityBreak()
        {
            const decimal quantityOrdered1 = 3;
            const decimal quantityOrdered2 = 50;
            const decimal quantityOrdered3 = 50;
            const decimal amount1 = 1;
            const decimal amount2 = 3;

            var break1 = new OrderQuantityBreakBuilder(this.Session).WithFromAmount(50).WithThroughAmount(99).Build();
            var break2 = new OrderQuantityBreakBuilder(this.Session).WithFromAmount(100).Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for quantity break 1")
                .WithOrderQuantityBreak(break1)
                .WithProduct(this.good)
                .WithPrice(amount1)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for quantity break 2")
                .WithOrderQuantityBreak(break2)
                .WithProduct(this.good)
                .WithPrice(amount2)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered1).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered2).Build();
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount1, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount1, item2.UnitPrice);

            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered3).Build();
            this.order.AddSalesOrderItem(item3);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item2.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountPercentageForOrderQuantityBreak_WhenDeriving_ThenUseDiscountComponentsForOrderQuantityBreak()
        {
            const decimal quantityOrdered1 = 3;
            const decimal quantityOrdered2 = 50;
            const decimal quantityOrdered3 = 50;
            const decimal percentage1 = 5;
            const decimal percentage2 = 10;

            var break1 = new OrderQuantityBreakBuilder(this.Session).WithFromAmount(50).WithThroughAmount(99).Build();
            var break2 = new OrderQuantityBreakBuilder(this.Session).WithFromAmount(100).Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for quantity break 1")
                .WithOrderQuantityBreak(break1)
                .WithProduct(this.good)
                .WithPercentage(percentage1)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for quantity break 2")
                .WithOrderQuantityBreak(break2)
                .WithProduct(this.good)
                .WithPercentage(percentage2)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered1).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered2).Build();
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount1 = Math.Round(price * percentage1 / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount1, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount1, item2.UnitPrice);

            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered3).Build();
            this.order.AddSalesOrderItem(item3);

            this.Session.Derive();

            var price2 = this.currentGoodBasePrice.Price ?? 0;
            var amount2 = Math.Round(price2 * percentage2 / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item2.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargeAmountForOrderQuantityBreak_WhenDeriving_ThenUseSurchargeComponentsForOrderQuantityBreak()
        {
            const decimal quantityOrdered1 = 3;
            const decimal quantityOrdered2 = 50;
            const decimal quantityOrdered3 = 50;
            const decimal amount1 = 1;
            const decimal amount2 = 3;

            var break1 = new OrderQuantityBreakBuilder(this.Session).WithFromAmount(50).WithThroughAmount(99).Build();
            var break2 = new OrderQuantityBreakBuilder(this.Session).WithFromAmount(100).Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for quantity break 1")
                .WithOrderQuantityBreak(break1)
                .WithProduct(this.good)
                .WithPrice(amount1)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for quantity break 2")
                .WithOrderQuantityBreak(break2)
                .WithProduct(this.good)
                .WithPrice(amount2)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered1).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered2).Build();
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount1, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount1, item2.UnitPrice);

            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered3).Build();
            this.order.AddSalesOrderItem(item3);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item2.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargePercentageForOrderQuantityBreak_WhenDeriving_ThenUseSurchargeComponentsForOrderQuantityBreak()
        {
            const decimal quantityOrdered1 = 3;
            const decimal quantityOrdered2 = 50;
            const decimal quantityOrdered3 = 50;
            const decimal percentage1 = 5;
            const decimal percentage2 = 10;

            var break1 = new OrderQuantityBreakBuilder(this.Session).WithFromAmount(50).WithThroughAmount(99).Build();
            var break2 = new OrderQuantityBreakBuilder(this.Session).WithFromAmount(100).Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for quantity break 1")
                .WithOrderQuantityBreak(break1)
                .WithProduct(this.good)
                .WithPercentage(percentage1)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for quantity break 2")
                .WithOrderQuantityBreak(break2)
                .WithProduct(this.good)
                .WithPercentage(percentage2)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered1).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered2).Build();
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount1 = Math.Round(price * percentage1 / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount1, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount1, item2.UnitPrice);

            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered3).Build();
            this.order.AddSalesOrderItem(item3);

            this.Session.Derive();

            var price2 = this.currentGoodBasePrice.Price ?? 0;
            var amount2 = Math.Round(price2 * percentage2 / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item2.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountAmountForOrderValue_WhenDeriving_ThenUseDiscountComponentsForOrderValue()
        {
            const decimal quantityOrdered1 = 3;
            const decimal quantityOrdered2 = 3;
            const decimal quantityOrdered3 = 10;
            const decimal amount1 = 1;
            const decimal amount2 = 3;

            var value1 = new OrderValueBuilder(this.Session).WithFromAmount(50).WithThroughAmount(99).Build();
            var value2 = new OrderValueBuilder(this.Session).WithFromAmount(100).Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for order value 1")
                .WithOrderValue(value1)
                .WithProduct(this.good)
                .WithPrice(amount1)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for order value 1")
                .WithOrderValue(value2)
                .WithProduct(this.good)
                .WithPrice(amount2)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered1).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered2).Build();
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount1, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount1, item2.UnitPrice);

            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered3).Build();
            this.order.AddSalesOrderItem(item3);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item2.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountPercentageForOrderValue_WhenDeriving_ThenUseDiscountComponentsForOrderValue()
        {
            const decimal quantityOrdered1 = 3;
            const decimal quantityOrdered2 = 3;
            const decimal quantityOrdered3 = 10;
            const decimal percentage1 = 5;
            const decimal percentage2 = 10;

            var value1 = new OrderValueBuilder(this.Session).WithFromAmount(50).WithThroughAmount(99).Build();
            var value2 = new OrderValueBuilder(this.Session).WithFromAmount(100).Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for order value 1")
                .WithOrderValue(value1)
                .WithProduct(this.good)
                .WithPercentage(percentage1)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for order value 1")
                .WithOrderValue(value2)
                .WithProduct(this.good)
                .WithPercentage(percentage2)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered1).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered2).Build();
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount1 = Math.Round(price * percentage1 / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount1, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount1, item2.UnitPrice);

            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered3).Build();
            this.order.AddSalesOrderItem(item3);

            this.Session.Derive();

            var price2 = this.currentGoodBasePrice.Price ?? 0;
            var amount2 = Math.Round(price2 * percentage2 / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item2.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargeAmountForOrderValue_WhenDeriving_ThenUseSurchargeComponentsForOrderValue()
        {
            const decimal quantityOrdered1 = 3;
            const decimal quantityOrdered2 = 3;
            const decimal quantityOrdered3 = 10;
            const decimal amount1 = 1;
            const decimal amount2 = 3;

            var value1 = new OrderValueBuilder(this.Session).WithFromAmount(50).WithThroughAmount(99).Build();
            var value2 = new OrderValueBuilder(this.Session).WithFromAmount(100).Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for order value 1")
                .WithOrderValue(value1)
                .WithProduct(this.good)
                .WithPrice(amount1)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for order value 1")
                .WithOrderValue(value2)
                .WithProduct(this.good)
                .WithPrice(amount2)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered1).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered2).Build();
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount1, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount1, item2.UnitPrice);

            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered3).Build();
            this.order.AddSalesOrderItem(item3);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item2.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargePercentageForOrderValue_WhenDeriving_ThenUseSurchargeComponentsForOrderValue()
        {
            const decimal quantityOrdered1 = 3;
            const decimal quantityOrdered2 = 3;
            const decimal quantityOrdered3 = 10;
            const decimal percentage1 = 5;
            const decimal percentage2 = 10;

            var value1 = new OrderValueBuilder(this.Session).WithFromAmount(50).WithThroughAmount(99).Build();
            var value2 = new OrderValueBuilder(this.Session).WithFromAmount(100).Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for order value 1")
                .WithOrderValue(value1)
                .WithProduct(this.good)
                .WithPercentage(percentage1)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge good for order value 1")
                .WithOrderValue(value2)
                .WithProduct(this.good)
                .WithPercentage(percentage2)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered1).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered2).Build();
            this.order.AddSalesOrderItem(item2);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount1 = Math.Round(price * percentage1 / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount1, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount1, item2.UnitPrice);

            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered3).Build();
            this.order.AddSalesOrderItem(item3);

            this.Session.Derive();

            var price2 = this.currentGoodBasePrice.Price ?? 0;
            var amount2 = Math.Round(price2 * percentage2 / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item1.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item2.UnitPrice);

            Assert.Equal(this.currentGoodBasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountAmountForSalesType_WhenDeriving_ThenUseDiscountComponentsForSalesType()
        {
            const decimal quantityOrdered = 3;
            const decimal expected = 1;

            var email = new SalesChannels(this.Session).EmailChannel;
            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(email)
                .WithProduct(this.good)
                .WithPrice(expected)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.order.SalesChannel = email;

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(expected, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - expected, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithDiscountPercentageAndItemDiscountPercentage_WhenDeriving_ThenExtraDiscountIsCalculated()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;
            const decimal adjustmentPerc = 10;
            var discountAdjustment = new DiscountAdjustmentBuilder(this.Session).WithPercentage(adjustmentPerc).Build();

            var email = new SalesChannels(this.Session).EmailChannel;
            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(email)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.order.SalesChannel = email;

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(quantityOrdered)
                .WithDiscountAdjustment(discountAdjustment)
                .Build();

            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var discount = Math.Round(price * percentage / 100, 2);
            var discountedprice = price - discount;
            var adjustmentPercentage = discountAdjustment.Percentage ?? 0;
            discount += Math.Round(discountedprice * adjustmentPercentage / 100, 2);

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(discount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - discount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargeAmountForSalesType_WhenDeriving_ThenUseSurchargeComponentsForSalesType()
        {
            const decimal quantityOrdered = 3;
            const decimal amount = 1;

            var email = new SalesChannels(this.Session).EmailChannel;
            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(email)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.order.SalesChannel = email;

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargePercentageForSalesType_WhenDeriving_ThenUseSurchargeComponentsForSalesType()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;

            var email = new SalesChannels(this.Session).EmailChannel;
            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(email)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.order.SalesChannel = email;

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenOrderItemWithSurchargePercentageAndItemSurchargePercentage_WhenDeriving_ThenExtraSurchargeIsCalculated()
        {
            const decimal quantityOrdered = 3;
            const decimal percentage = 5;
            const decimal surchargePerc = 10;

            var surchargeAdjustment = new SurchargeAdjustmentBuilder(this.Session).WithPercentage(surchargePerc).Build();

            var email = new SalesChannels(this.Session).EmailChannel;
            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(email)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.order.SalesChannel = email;

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(quantityOrdered)
                .WithSurchargeAdjustment(surchargeAdjustment)
                .Build();

            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var surcharge = Math.Round(price * percentage / 100, 2);
            var surchargedPrice = price + surcharge;
            var adjustmentPercentage = surchargeAdjustment.Percentage ?? 0;
            surcharge += Math.Round(surchargedPrice * adjustmentPercentage / 100, 2);

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(surcharge, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price + surcharge, item1.UnitPrice);
        }

        [Fact]
        public void GivenBillToCustomerWithDifferentCurrency_WhenDerivingPrices_ThenCalculatePricesInPreferredCurrency()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var poundSterling = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "GBP");

            new ExchangeRateBuilder(this.Session)
                .WithValidFrom(this.Session.Now())
                .WithFromCurrency(euro)
                .WithToCurrency(poundSterling)
                .WithRate(0.8553M)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            Assert.Equal(euro, this.order.DerivedCurrency);

            this.billToCustomer.PreferredCurrency = poundSterling;

            var newOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(this.shipToCustomer)
                .WithBillToCustomer(this.billToCustomer)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            newOrder.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(poundSterling, newOrder.DerivedCurrency);

            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
        }

        [Fact]
        public void GivenOrderItemForVariantGood_WhenDerivingPrices_ThenUsePriceComponentsForVirtualGood()
        {
            this.InstantiateObjects(this.Session);

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.variantGood)
                .WithQuantityOrdered(quantityOrdered)
                .WithAssignedVatRegime(new VatRegimes(this.Session).DutchStandardTariff)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.Session.Derive();

            Assert.Equal(this.currentVirtualGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);
            Assert.Equal(Math.Round(item1.UnitPrice * this.vatRate21.Rate / 100, 2), item1.UnitVat);

            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, item1.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * this.vatRate21.Rate / 100, 2) * quantityOrdered, item1.TotalVat);

            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, this.order.TotalBasePrice);
            Assert.Equal(0, this.order.TotalDiscount);
            Assert.Equal(0, this.order.TotalSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, this.order.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * this.vatRate21.Rate / 100, 2) * quantityOrdered, this.order.TotalVat);
        }

        [Fact]
        public void GivenOrderItemWithDiscountAdjustment_WhenDeriving_ThenCalculateSellingPriceUsingItemDiscountAdjustment()
        {
            const decimal quantityOrdered = 3;
            const decimal amount = 1;
            const decimal adjustmentPercentage = 5;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(quantityOrdered)
                .WithDiscountAdjustment(new DiscountAdjustmentBuilder(this.Session).WithPercentage(adjustmentPercentage).Build())
                .Build();

            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            var price = this.currentGoodBasePrice.Price ?? 0;
            var adjustmentAmount = Math.Round((price - amount) * adjustmentPercentage / 100, 2);

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount + adjustmentAmount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount - adjustmentAmount, item1.UnitPrice);
            Assert.Equal((amount + adjustmentAmount) * quantityOrdered * -1, item1.TotalOrderAdjustment);
        }

        [Fact]
        public void GivenOrderItemWithDiscount_WhenDeriving_ThenUseGeneralDiscountNotBoundToProduct()
        {
            const decimal quantityOrdered = 3;
            const decimal amount = 1;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(this.good).WithQuantityOrdered(quantityOrdered).Build();
            this.order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenPurchasePriceInDifferenUnitOfMeasureComparedToProduct_WhenDerivingMarkupAndProfitMargin_ThenUnitOfMeasureConversionIsPerformed()
        {
            var pair = new UnitsOfMeasure(this.Session).Pair;
            var piece = new UnitsOfMeasure(this.Session).Piece;

            var fromPairToPiece = new UnitOfMeasureConversionBuilder(this.Session)
                .WithToUnitOfMeasure(piece)
                .WithConversionFactor(2).Build();

            var fromPieceToPair = new UnitOfMeasureConversionBuilder(this.Session)
                .WithToUnitOfMeasure(pair)
                .WithConversionFactor(0.5M).Build();

            pair.AddUnitOfMeasureConversion(fromPairToPiece);
            pair.AddUnitOfMeasureConversion(fromPieceToPair);

            this.goodPurchasePrice.UnitOfMeasure = pair;
            this.good.UnitOfMeasure = piece;

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            const decimal quantityOrdered = 6;
            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithQuantityOrdered(quantityOrdered)
                .WithAssignedVatRegime(new VatRegimes(this.Session).DutchStandardTariff)
                .Build();

            this.order.AddSalesOrderItem(item1);
            this.Session.Derive();

            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price, item1.UnitPrice);
            Assert.Equal(Math.Round(item1.UnitPrice * this.vatRate21.Rate / 100, 2), item1.UnitVat);

            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, item1.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * this.vatRate21.Rate / 100, 2) * quantityOrdered, item1.TotalVat);

            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, this.order.TotalBasePrice);
            Assert.Equal(0, this.order.TotalDiscount);
            Assert.Equal(0, this.order.TotalSurcharge);
            Assert.Equal(this.currentGoodBasePrice.Price * quantityOrdered, this.order.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * this.vatRate21.Rate / 100, 2) * quantityOrdered, this.order.TotalVat);

            var purchasePrice = this.goodPurchasePrice.Price * 0.5M;
        }

        private void InstantiateObjects(ISession session)
        {
            this.productCategory = (ProductCategory)session.Instantiate(this.productCategory);
            this.parentProductCategory = (ProductCategory)session.Instantiate(this.parentProductCategory);
            this.ancestorProductCategory = (ProductCategory)session.Instantiate(this.ancestorProductCategory);
            this.part = (Part)session.Instantiate(this.part);
            this.good = (Good)session.Instantiate(this.good);
            this.feature1 = (Colour)session.Instantiate(this.feature1);
            this.feature2 = (Colour)session.Instantiate(this.feature2);
            this.internalOrganisation = (Singleton)session.Instantiate(this.internalOrganisation);
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
            this.currentGoodBasePrice = (BasePrice)session.Instantiate(this.currentGoodBasePrice);
            this.order = (SalesOrder)session.Instantiate(this.order);
            this.vatRate21 = (VatRate)session.Instantiate(this.vatRate21);
        }
    }
}

// <copyright file="SalesInvoiceItemTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System;
    using Allors.Meta;
    using Xunit;

    public class SalesInvoiceItemTests : DomainTest
    {
        private Part finishedGood;
        private NonUnifiedGood good;
        private Colour feature1;
        private Colour feature2;
        private Singleton internalOrganisation;
        private Organisation billToCustomer;
        private Organisation shipToCustomer;
        private Organisation supplier;
        private SupplierOffering goodPurchasePrice;
        private City mechelen;
        private City kiev;
        private PostalAddress billToContactMechanismMechelen;
        private PostalAddress shipToContactMechanismKiev;
        private BasePrice currentBasePriceGeoBoundary;
        private BasePrice currentGood1BasePrice;
        private BasePrice currentFeature1BasePrice;
        private BasePrice currentGood1Feature1BasePrice;
        private SalesInvoice invoice;
        private VatRegime vatRegime;

        public SalesInvoiceItemTests()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            this.supplier = new OrganisationBuilder(this.Session).WithName("supplier").WithLocale(new Locales(this.Session).EnglishGreatBritain).Build();

            this.internalOrganisation = this.Session.GetSingleton();

            this.vatRegime = new VatRegimes(this.Session).BelgiumStandard;

            this.mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            this.kiev = new CityBuilder(this.Session).WithName("Kiev").Build();

            this.billToContactMechanismMechelen = new PostalAddressBuilder(this.Session).WithAddress1("Mechelen").WithPostalAddressBoundary(this.mechelen).Build();
            this.shipToContactMechanismKiev = new PostalAddressBuilder(this.Session).WithAddress1("Kiev").WithPostalAddressBoundary(this.kiev).Build();
            this.billToCustomer = new OrganisationBuilder(this.Session).WithName("billToCustomer").WithPreferredCurrency(euro).Build();

            this.shipToCustomer = new OrganisationBuilder(this.Session).WithName("shipToCustomer").WithPreferredCurrency(euro).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(this.billToCustomer).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(this.shipToCustomer).Build();

            this.Session.Derive();

            this.good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            this.finishedGood = this.good.Part;

            this.feature1 = new ColourBuilder(this.Session)
                .WithVatRegime(this.vatRegime)
                .WithName("white")
                .Build();

            this.feature2 = new ColourBuilder(this.Session)
                .WithName("black")
                .Build();

            this.goodPurchasePrice = new SupplierOfferingBuilder(this.Session)
                .WithPart(this.finishedGood)
                .WithSupplier(this.supplier)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            this.currentBasePriceGeoBoundary = new BasePriceBuilder(this.Session)
                .WithDescription("current BasePriceGeoBoundary")
                .WithGeographicBoundary(this.mechelen)
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .Build();

            // previous basePrice for good
            new BasePriceBuilder(this.Session).WithDescription("previous good baseprice")
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            this.currentGood1BasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current good baseprice")
                .WithProduct(this.good)
                .WithPrice(10)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            // future basePrice for good
            new BasePriceBuilder(this.Session).WithDescription("future good baseprice")
                .WithProduct(this.good)
                .WithPrice(11)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            // previous basePrice for feature1
            new BasePriceBuilder(this.Session).WithDescription("previous feature1 price")
                .WithProductFeature(this.feature1)
                .WithPrice(0.5M)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for feature1
            new BasePriceBuilder(this.Session).WithDescription("future feature1 price")
                .WithProductFeature(this.feature1)
                .WithPrice(2.5M)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            this.currentFeature1BasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current feature1 price")
                .WithProductFeature(this.feature1)
                .WithPrice(2)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature2
            new BasePriceBuilder(this.Session).WithDescription("previous feature2 price")
                .WithProductFeature(this.feature2)
                .WithPrice(2)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for feature2
            new BasePriceBuilder(this.Session)
                .WithDescription("future feature2 price")
                .WithProductFeature(this.feature2)
                .WithPrice(4)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current feature2 price")
                .WithProductFeature(this.feature2)
                .WithPrice(3)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for good with feature1
            new BasePriceBuilder(this.Session).WithDescription("previous good/feature1 baseprice")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(4)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            // future basePrice for good with feature1
            new BasePriceBuilder(this.Session)
                .WithDescription("future good/feature1 baseprice")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(6)
                .WithFromDate(this.Session.Now().AddYears(1))
                .Build();

            this.currentGood1Feature1BasePrice = new BasePriceBuilder(this.Session)
                .WithDescription("current good/feature1 baseprice")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(5)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.invoice = new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithAssignedBillToContactMechanism(this.billToContactMechanismMechelen)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedShipToAddress(this.shipToContactMechanismKiev)
                .WithShipToCustomer(this.shipToCustomer)
                .Build();

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void GivenInvoiceItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.Session);

            var builder = new SalesInvoiceItemBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithProduct(this.good);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithProductFeature(this.feature1);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenInvoiceItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            this.InstantiateObjects(this.Session);

            var item = new SalesInvoiceItemBuilder(this.Session).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithProduct(this.good).Build();

            this.Session.Derive();

            Assert.Equal(new SalesInvoiceItemStates(this.Session).ReadyForPosting, item.SalesInvoiceItemState);
            Assert.Equal(item.SalesInvoiceItemState, item.LastSalesInvoiceItemState);
            Assert.Equal(0, item.AmountPaid);
        }

        [Fact]
        public void GivenInvoiceItemWithoutVatRegime_WhenDeriving_ThenDerivedVatRegimeIsFromInvoice()
        {
            this.InstantiateObjects(this.Session);

            var productItem = new InvoiceItemTypes(this.Session).ProductItem;

            var salesInvoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedBillToContactMechanism(this.billToContactMechanismMechelen)
                .WithAssignedVatRegime(new VatRegimes(this.Session).ZeroRated)
                .Build();

            this.Session.Derive();

            var invoiceItem = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithQuantity(1).WithInvoiceItemType(productItem).Build();
            salesInvoice.AddSalesInvoiceItem(invoiceItem);

            this.Session.Derive();

            Assert.Equal(salesInvoice.DerivedVatRegime, invoiceItem.DerivedVatRegime);
        }

        [Fact]
        public void GivenInvoiceItemWithoutVatRegime_WhenDeriving_ThenItemDerivedVatRateIsFromInvoiceVatRegime()
        {
            this.InstantiateObjects(this.Session);

            var vatRate0 = new VatRates(this.Session).FindBy(M.VatRate.Rate, 0);

            var salesInvoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedBillToContactMechanism(this.billToContactMechanismMechelen)
                .WithAssignedVatRegime(new VatRegimes(this.Session).ZeroRated)
                .Build();

            this.Session.Derive();

            var invoiceItem = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(1).Build();
            salesInvoice.AddSalesInvoiceItem(invoiceItem);

            this.Session.Derive();

            Assert.Equal(salesInvoice.DerivedVatRegime, invoiceItem.DerivedVatRegime);
            Assert.Equal(vatRate0, invoiceItem.VatRate);
        }

        [Fact]
        public void GivenInvoiceItemWithoutIrpfRegime_WhenDeriving_ThenDerivedIrpfRegimeIsFromInvoice()
        {
            this.InstantiateObjects(this.Session);

            var productItem = new InvoiceItemTypes(this.Session).ProductItem;

            var salesInvoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedBillToContactMechanism(this.billToContactMechanismMechelen)
                .WithAssignedIrpfRegime(new IrpfRegimes(this.Session).Assessable19)
                .Build();

            this.Session.Derive();

            var invoiceItem = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithQuantity(1).WithInvoiceItemType(productItem).Build();
            salesInvoice.AddSalesInvoiceItem(invoiceItem);

            this.Session.Derive();

            Assert.Equal(salesInvoice.DerivedIrpfRegime, invoiceItem.DerivedIrpfRegime);
        }

        [Fact]
        public void GivenInvoiceItemWithoutIrpfRegime_WhenDeriving_ThenItemDerivedIrpfRateIsFromInvoiceIrpfRegime()
        {
            this.InstantiateObjects(this.Session);

            var irpfRate19 = new IrpfRates(this.Session).FindBy(M.IrpfRate.Rate, 19);

            var salesInvoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedBillToContactMechanism(this.billToContactMechanismMechelen)
                .WithAssignedIrpfRegime(new IrpfRegimes(this.Session).Assessable19)
                .Build();

            this.Session.Derive();

            var invoiceItem = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(1).Build();
            salesInvoice.AddSalesInvoiceItem(invoiceItem);

            this.Session.Derive();

            Assert.Equal(salesInvoice.DerivedIrpfRegime, invoiceItem.DerivedIrpfRegime);
            Assert.Equal(irpfRate19, invoiceItem.IrpfRate);
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

            const decimal quantity = 3;
            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithQuantity(quantity)
                .WithAssignedVatRegime(new VatRegimes(this.Session).DutchStandardTariff)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);
            Assert.Equal(Math.Round(item1.UnitPrice * 21 / 100, 2), item1.UnitVat);

            Assert.Equal(this.currentGood1BasePrice.Price * quantity, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price * quantity, item1.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * 21 / 100, 2) * quantity, item1.TotalVat);

            var purchasePrice = this.goodPurchasePrice.Price * 0.5M;

            Assert.Equal(this.currentGood1BasePrice.Price * quantity, this.invoice.TotalBasePrice);
            Assert.Equal(0, this.invoice.TotalDiscount);
            Assert.Equal(0, this.invoice.TotalSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price * quantity, this.invoice.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * 21 / 100, 2) * quantity, this.invoice.TotalVat);
        }

        [Fact]
        public void GivenInvoiceItemForGood1WithActualPrice_WhenDerivingPrices_ThenUseActualPrice()
        {
            this.InstantiateObjects(this.Session);

            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithQuantity(3)
                .WithAssignedUnitPrice(15)
                .WithAssignedVatRegime(new VatRegimes(this.Session).DutchStandardTariff)
                .WithAssignedIrpfRegime(new IrpfRegimes(this.Session).Assessable19)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

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
            Assert.Equal(8.55m, item1.TotalIrpf);
            Assert.Equal(45.90m, item1.GrandTotal);

            Assert.Equal(30, this.invoice.TotalBasePrice);
            Assert.Equal(0, this.invoice.TotalDiscount);
            Assert.Equal(0, this.invoice.TotalSurcharge);
            Assert.Equal(45, this.invoice.TotalExVat);
            Assert.Equal(9.45m, this.invoice.TotalVat);
            Assert.Equal(54.45m, this.invoice.TotalIncVat);
            Assert.Equal(45, this.invoice.TotalListPrice);
            Assert.Equal(8.55m, this.invoice.TotalIrpf);
            Assert.Equal(45.90m, this.invoice.GrandTotal);
        }

        [Fact]
        public void GivenInvoiceItemForGood1_WhenDerivingPrices_ThenUsePriceComponentsForGood1()
        {
            this.InstantiateObjects(this.Session);

            var irpfRegime = new IrpfRegimes(this.Session).Assessable19;

            const decimal quantity = 3;
            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithQuantity(quantity)
                .WithAssignedVatRegime(new VatRegimes(this.Session).DutchStandardTariff)
                .WithAssignedIrpfRegime(irpfRegime)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);
            Assert.Equal(Math.Round(item1.UnitPrice * 21 / 100, 2), item1.UnitVat);
            Assert.Equal(Math.Round(item1.UnitPrice * 19 / 100, 2), item1.UnitIrpf);

            Assert.Equal(this.currentGood1BasePrice.Price * quantity, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price * quantity, item1.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * 19 / 100, 2) * quantity, item1.TotalIrpf);

            Assert.Equal(this.currentGood1BasePrice.Price * quantity, this.invoice.TotalBasePrice);
            Assert.Equal(0, this.invoice.TotalDiscount);
            Assert.Equal(0, this.invoice.TotalSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price * quantity, this.invoice.TotalExVat);
            Assert.Equal(Math.Round(item1.UnitPrice * 21 / 100, 2) * quantity, this.invoice.TotalVat);
            Assert.Equal(Math.Round(item1.UnitPrice * 19 / 100, 2) * quantity, this.invoice.TotalIrpf);
        }

        [Fact]
        public void GivenInvoiceItemForFeature1_WhenDerivingPrices_ThenUsePriceComponentsForFeature1()
        {
            this.InstantiateObjects(this.Session);

            const decimal quantity = 3;
            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProductFeature(this.feature1).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductFeatureItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentFeature1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentFeature1BasePrice.Price, item1.UnitPrice);
            Assert.Equal(this.currentFeature1BasePrice.Price * quantity, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(this.currentFeature1BasePrice.Price * quantity, item1.TotalExVat);

            Assert.Equal(this.currentFeature1BasePrice.Price * quantity, this.invoice.TotalBasePrice);
            Assert.Equal(0, this.invoice.TotalDiscount);
            Assert.Equal(0, this.invoice.TotalSurcharge);
            Assert.Equal(this.currentFeature1BasePrice.Price * quantity, this.invoice.TotalExVat);
        }

        [Fact]
        public void GivenProductWithMultipleBasePrices_WhenDeriving_ThenLowestUnitPriceMustBeCalculated()
        {
            this.InstantiateObjects(this.Session);

            this.invoice.AssignedShipToAddress = this.billToContactMechanismMechelen;

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(3).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentBasePriceGeoBoundary.Price, item1.UnitPrice);

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedBillToContactMechanism(this.billToContactMechanismMechelen)
                .Build();

            item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(3).Build();
            invoice2.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscount_WhenDeriving_ThenUseGeneralDiscountNotBoundToProduct()
        {
            const decimal quantity = 3;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForGeoBoundary_WhenDeriving_ThenUseDiscountComponentsForGeoBoundary()
        {
            const decimal quantity = 3;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForGeoBoundary_WhenDeriving_ThenUseDiscountComponentsForGeoBoundary()
        {
            const decimal quantity = 3;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForGeoBoundary_WhenDeriving_ThenUseSurchargeComponentsForGeoBoundary()
        {
            const decimal quantity = 3;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForGeoBoundary_WhenDeriving_ThenUseSurchargeComponentsForGeoBoundary()
        {
            const decimal quantity = 3;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForPartyClassification_WhenDeriving_ThenUseDiscountComponentsForPartyClassification()
        {
            const decimal quantity = 3;
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

            this.invoice.ShipToCustomer = this.shipToCustomer;

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForPartyClassification_WhenDeriving_ThenUseDiscountComponentsForPartyClassification()
        {
            const decimal quantity = 3;
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

            this.invoice.ShipToCustomer = this.shipToCustomer;

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForPartyClassification_WhenDeriving_ThenUseSurchargeComponentsForPartyClassification()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            var classification = new IndustryClassificationBuilder(this.Session).WithName("gold customer").Build();
            new SurchargeComponentBuilder(this.Session)
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

            this.invoice.ShipToCustomer = this.shipToCustomer;

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForPartyClassification_WhenDeriving_ThenUseSurchargeComponentsForPartyClassification()
        {
            const decimal quantity = 3;
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

            this.invoice.ShipToCustomer = this.shipToCustomer;

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForProductCatergory_WhenDeriving_ThenUseDiscountComponentsForProductCatergory()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            var category = new ProductCategoryBuilder(this.Session)
                .WithName("gizmo")
                .Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for product category")
                .WithProductCategory(category)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            category.AddProduct(this.good);

            this.Session.Derive();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForProductCatergory_WhenDeriving_ThenUseDiscountComponentsForProductCatergory()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            var category = new ProductCategoryBuilder(this.Session)
                .WithName("gizmo")
                .Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for product category")
                .WithProductCategory(category)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            category.AddProduct(this.good);

            this.Session.Derive();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForProductCatergory_WhenDeriving_ThenUseSurchargeComponentsForProductCatergory()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            var category = new ProductCategoryBuilder(this.Session)
                .WithName("gizmo")
                .Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for product category")
                .WithProductCategory(category)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            category.AddProduct(this.good);

            this.Session.Derive();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForProductCatergory_WhenDeriving_ThenUseSurchargeComponentsForProductCatergory()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            var category = new ProductCategoryBuilder(this.Session)
                .WithName("gizmo")
                .Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for product category")
                .WithProductCategory(category)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            category.AddProduct(this.good);

            this.Session.Derive();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForOrderQuantityBreak_WhenDeriving_ThenUseDiscountComponentsForOrderQuantityBreak()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 50;
            const decimal quantity3 = 50;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item2.UnitPrice);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item2.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForOrderQuantityBreak_WhenDeriving_ThenUseDiscountComponentsForOrderQuantityBreak()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 50;
            const decimal quantity3 = 50;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount1 = Math.Round(price * percentage1 / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item2.UnitPrice);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            var price2 = this.currentGood1BasePrice.Price ?? 0;
            var amount2 = Math.Round(price2 * percentage2 / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item2.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForOrderQuantityBreak_WhenDeriving_ThenUseSurchargeComponentsForOrderQuantityBreak()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 50;
            const decimal quantity3 = 50;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item2.UnitPrice);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item2.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForOrderQuantityBreak_WhenDeriving_ThenUseSurchargeComponentsForOrderQuantityBreak()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 50;
            const decimal quantity3 = 50;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount1 = Math.Round(price * percentage1 / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item2.UnitPrice);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            var price2 = this.currentGood1BasePrice.Price ?? 0;
            var amount2 = Math.Round(price2 * percentage2 / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item2.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForOrderValue_WhenDeriving_ThenUseDiscountComponentsForOrderValue()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 3;
            const decimal quantity3 = 10;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item2.UnitPrice);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item2.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForOrderValue_WhenDeriving_ThenUseDiscountComponentsForOrderValue()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 3;
            const decimal quantity3 = 10;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount1 = Math.Round(price * percentage1 / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item2.UnitPrice);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            var price2 = this.currentGood1BasePrice.Price ?? 0;
            var amount2 = Math.Round(price2 * percentage2 / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item2.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForOrderValue_WhenDeriving_ThenUseSurchargeComponentsForOrderValue()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 3;
            const decimal quantity3 = 10;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item2.UnitPrice);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item2.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForOrderValue_WhenDeriving_ThenUseSurchargeComponentsForOrderValue()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 3;
            const decimal quantity3 = 10;
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitPrice);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount1 = Math.Round(price * percentage1 / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item2.UnitPrice);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            var price2 = this.currentGood1BasePrice.Price ?? 0;
            var amount2 = Math.Round(price2 * percentage2 / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item1.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item2.UnitPrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item3.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForSalesType_WhenDeriving_ThenUseDiscountComponentsForSalesType()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.Session).EmailChannel)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.invoice.SalesChannel = new SalesChannels(this.Session).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageAndItemDiscountPercentage_WhenDeriving_ThenExtraDiscountIsCalculated()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;
            const decimal adjustmentPerc = 10;

            var discountAdjustment = new DiscountAdjustmentBuilder(this.Session).WithPercentage(adjustmentPerc).Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.Session).EmailChannel)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.invoice.SalesChannel = new SalesChannels(this.Session).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithProduct(this.good)
                .WithQuantity(quantity)
                .WithDiscountAdjustment(discountAdjustment)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var discount = Math.Round(price * percentage / 100, 2);
            var discountedprice = price - discount;
            var adjustmentPercentage = discountAdjustment.Percentage ?? 0;
            discount += Math.Round(discountedprice * adjustmentPercentage / 100, 2);

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(discount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - discount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForSalesType_WhenDeriving_ThenUseSurchargeComponentsForSalesType()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.Session).EmailChannel)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.invoice.SalesChannel = new SalesChannels(this.Session).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForSalesType_WhenDeriving_ThenUseSurchargeComponentsForSalesType()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.Session).EmailChannel)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.invoice.SalesChannel = new SalesChannels(this.Session).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round(price * percentage / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.UnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageAndItemSurchargePercentage_WhenDeriving_ThenExtraSurchargeIsCalculated()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;
            const decimal adjustmentPerc = 10;

            var surchargeAdjustment = new SurchargeAdjustmentBuilder(this.Session).WithPercentage(adjustmentPerc).Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.Session).EmailChannel)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(this.Session.Now().AddMinutes(-1))
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            this.invoice.SalesChannel = new SalesChannels(this.Session).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithProduct(this.good)
                .WithQuantity(quantity)
                .WithSurchargeAdjustment(surchargeAdjustment)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var surcharge = Math.Round(price * percentage / 100, 2);
            var surchargedprice = price + surcharge;
            var adjustmentPercentage = surchargeAdjustment.Percentage ?? 0;
            surcharge += Math.Round(surchargedprice * adjustmentPercentage / 100, 2);

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(surcharge, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + surcharge, item1.UnitPrice);
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

            Assert.Equal(euro, this.invoice.DerivedCurrency);

            this.billToCustomer.PreferredCurrency = poundSterling;

            var newInvoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedBillToContactMechanism(this.billToContactMechanismMechelen)
                .Build();

            const decimal quantity = 3;
            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).WithQuantity(quantity).Build();
            newInvoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(poundSterling, newInvoice.DerivedCurrency);

            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
        }

 
        [Fact]
        public void GiveninvoiceItem_WhenPartialPaymentIsReceived_ThenInvoiceItemStateIsSetToPartiallyPaid()
        {
            this.InstantiateObjects(this.Session);

            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(this.good)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithQuantity(1)
                .WithAssignedUnitPrice(100M)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            new ReceiptBuilder(this.Session)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(item1).WithAmountApplied(50).Build())
                .WithEffectiveDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesInvoiceItemStates(this.Session).PartiallyPaid, item1.SalesInvoiceItemState);
        }

        [Fact]
        public void GiveninvoiceItem_WhenFullPaymentIsReceived_ThenInvoiceItemStateIsSetToPaid()
        {
            this.InstantiateObjects(this.Session);

            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithProduct(this.good)
                .WithQuantity(1)
                .WithAssignedUnitPrice(100M)
                .WithAssignedVatRegime(new VatRegimes(this.Session).DutchStandardTariff)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            new ReceiptBuilder(this.Session)
                .WithAmount(121)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(item1).WithAmountApplied(121).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesInvoiceItemStates(this.Session).Paid, item1.SalesInvoiceItemState);
        }

        private void InstantiateObjects(ISession session)
        {
            this.good = (NonUnifiedGood)session.Instantiate(this.good);
            this.finishedGood = (Part)session.Instantiate(this.finishedGood);
            this.feature1 = (Colour)session.Instantiate(this.feature1);
            this.feature2 = (Colour)session.Instantiate(this.feature2);
            this.internalOrganisation = (Singleton)session.Instantiate(this.internalOrganisation);
            this.billToCustomer = (Organisation)session.Instantiate(this.billToCustomer);
            this.shipToCustomer = (Organisation)session.Instantiate(this.shipToCustomer);
            this.supplier = (Organisation)session.Instantiate(this.supplier);
            this.mechelen = (City)session.Instantiate(this.mechelen);
            this.kiev = (City)session.Instantiate(this.kiev);
            this.billToContactMechanismMechelen = (PostalAddress)session.Instantiate(this.billToContactMechanismMechelen);
            this.shipToContactMechanismKiev = (PostalAddress)session.Instantiate(this.shipToContactMechanismKiev);
            this.goodPurchasePrice = (SupplierOffering)session.Instantiate(this.goodPurchasePrice);
            this.currentBasePriceGeoBoundary = (BasePrice)session.Instantiate(this.currentBasePriceGeoBoundary);
            this.currentFeature1BasePrice = (BasePrice)session.Instantiate(this.currentFeature1BasePrice);
            this.currentGood1BasePrice = (BasePrice)session.Instantiate(this.currentGood1BasePrice);
            this.currentGood1Feature1BasePrice = (BasePrice)session.Instantiate(this.currentGood1Feature1BasePrice);
            this.invoice = (SalesInvoice)session.Instantiate(this.invoice);
            this.vatRegime = (VatRegime)session.Instantiate(this.vatRegime);
        }
    }
}

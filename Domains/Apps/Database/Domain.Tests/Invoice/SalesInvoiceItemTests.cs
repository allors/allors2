//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesInvoiceItemTests.cs" company="Allors bvba">
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

    
    public class SalesInvoiceItemTests : DomainTest
    {
        private Good good;
        private Colour feature1;
        private Colour feature2;
        private Singleton internalOrganisation;
        private Organisation billToCustomer;
        private Organisation shipToCustomer;
        private Organisation supplier;
        private ProductPurchasePrice goodPurchasePrice;
        private SupplierOffering goodSupplierOffering;
        private City mechelen;
        private City kiev;
        private PostalAddress billToContactMechanismMechelen;
        private PostalAddress shipToContactMechanismKiev;
        private BasePrice currentBasePriceGeoBoundary;
        private BasePrice currentGood1BasePrice;
        private BasePrice currentFeature1BasePrice;
        private BasePrice currentGood1Feature1BasePrice;
        private SalesInvoice invoice;
        private VatRate vatRate21;

        public SalesInvoiceItemTests()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");

            this.supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain).WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();

            this.internalOrganisation = Singleton.Instance(this.DatabaseSession);

            this.vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            this.mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            this.kiev = new CityBuilder(this.DatabaseSession).WithName("Kiev").Build();

            this.billToContactMechanismMechelen = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Mechelen").WithGeographicBoundary(this.mechelen).Build();
            this.shipToContactMechanismKiev = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Kiev").WithGeographicBoundary(this.kiev).Build();
            this.billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("billToCustomer").WithPreferredCurrency(euro).WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

            this.shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("shipToCustomer").WithPreferredCurrency(euro).WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

            this.DatabaseSession.Derive();

            this.good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(this.vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
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

            this.goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.goodSupplierOffering = new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithProductPurchasePrice(this.goodPurchasePrice)
                .Build();

            this.currentBasePriceGeoBoundary = new BasePriceBuilder(this.DatabaseSession)
                .WithDescription("current BasePriceGeoBoundary")
                .WithGeographicBoundary(this.mechelen)
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .Build();

            // previous basePrice for good
            new BasePriceBuilder(this.DatabaseSession).WithDescription("previous good")
                .WithProduct(this.good)
                .WithPrice(8)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            this.currentGood1BasePrice = new BasePriceBuilder(this.DatabaseSession)
                .WithDescription("current good")
                .WithProduct(this.good)
                .WithPrice(10)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            // future basePrice for good
            new BasePriceBuilder(this.DatabaseSession).WithDescription("future good")
                .WithProduct(this.good)
                .WithPrice(11)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            // previous basePrice for feature1
            new BasePriceBuilder(this.DatabaseSession).WithDescription("previous feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(0.5M)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for feature1
            new BasePriceBuilder(this.DatabaseSession).WithDescription("future feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(2.5M)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.currentFeature1BasePrice = new BasePriceBuilder(this.DatabaseSession)
                .WithDescription("current feature1")
                .WithProductFeature(this.feature1)
                .WithPrice(2)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for feature2
            new BasePriceBuilder(this.DatabaseSession).WithDescription("previous feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(2)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for feature2
            new BasePriceBuilder(this.DatabaseSession)
                .WithDescription("future feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(4)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            new BasePriceBuilder(this.DatabaseSession)
                .WithDescription("current feature2")
                .WithProductFeature(this.feature2)
                .WithPrice(3)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            // previous basePrice for good with feature1
            new BasePriceBuilder(this.DatabaseSession).WithDescription("previous good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(4)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            // future basePrice for good with feature1
            new BasePriceBuilder(this.DatabaseSession)
                .WithDescription("future good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(6)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .Build();

            this.currentGood1Feature1BasePrice = new BasePriceBuilder(this.DatabaseSession)
                .WithDescription("current good/feature1")
                .WithProduct(this.good)
                .WithProductFeature(this.feature1)
                .WithPrice(5)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToContactMechanism(this.billToContactMechanismMechelen)
                .WithBillToCustomer(this.billToCustomer)
                .WithShipToAddress(this.shipToContactMechanismKiev)
                .WithShipToCustomer(this.shipToCustomer)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
        }

        [Fact]
        public void GivenInvoiceItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var builder = new SalesInvoiceItemBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithProduct(this.good);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithQuantity(1);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithProductFeature(this.feature1);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenInvoiceItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var item = new SalesInvoiceItemBuilder(this.DatabaseSession).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithProduct(this.good).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesInvoiceItemObjectStates(this.DatabaseSession).ReadyForPosting, item.CurrentObjectState);
            Assert.Equal(item.CurrentObjectState, item.LastObjectState);
            Assert.Equal(0, item.AmountPaid);
            Assert.Equal(0, item.Quantity);
        }

        [Fact]
        public void GivenInvoiceItemWithoutVatRegime_WhenDeriving_ThenDerivedVatRegimeIsFromOrder()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;

            var salesInvoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(this.billToCustomer)
                .WithBillToContactMechanism(this.billToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantity(1).WithSalesInvoiceItemType(productItem).Build();
            salesInvoice.AddSalesInvoiceItem(invoiceItem);

            this.DatabaseSession.Derive();
                
            Assert.Equal(salesInvoice.VatRegime, invoiceItem.VatRegime);
        }

        [Fact]
        public void GivenInvoiceItemWithoutVatRegime_WhenDeriving_ThenItemDerivedVatRateIsFromInvoiceVatRegime()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var vatRate0 = new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate, 0);

            var salesInvoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(this.billToCustomer)
                .WithBillToContactMechanism(this.billToContactMechanismMechelen)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(1).Build();
            salesInvoice.AddSalesInvoiceItem(invoiceItem);

            this.DatabaseSession.Derive();

            Assert.Equal(salesInvoice.VatRegime, invoiceItem.VatRegime);
            Assert.Equal(vatRate0, invoiceItem.DerivedVatRate);
        }

        [Fact]
        public void GivenPurchasePriceInDifferenUnitOfMeasureComparedToProduct_WhenDerivingMarkupAndProfitMargin_ThenUnitOfMeasureConversionIsPerformed()
        {
            var pair = new UnitsOfMeasure(this.DatabaseSession).Pair;
            var piece = new UnitsOfMeasure(this.DatabaseSession).Piece;

            var fromPairToPiece = new UnitOfMeasureConversionBuilder(this.DatabaseSession)
                .WithToUnitOfMeasure(piece)
                .WithConversionFactor(2).Build();

            var fromPieceToPair = new UnitOfMeasureConversionBuilder(this.DatabaseSession)
                .WithToUnitOfMeasure(pair)
                .WithConversionFactor(0.5M).Build();

            pair.AddUnitOfMeasureConversion(fromPairToPiece);
            pair.AddUnitOfMeasureConversion(fromPieceToPair);

            this.goodPurchasePrice.UnitOfMeasure = pair;
            this.good.UnitOfMeasure = piece;

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
            
            this.InstantiateObjects(this.DatabaseSession);

            const decimal quantity = 3;
            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(Math.Round((item1.CalculatedUnitPrice * this.vatRate21.Rate) / 100, 2), item1.UnitVat);
            Assert.Equal(this.goodPurchasePrice.Price * 0.5M, item1.UnitPurchasePrice);

            Assert.Equal(this.currentGood1BasePrice.Price * quantity, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price * quantity, item1.TotalExVat);
            Assert.Equal(Math.Round((item1.CalculatedUnitPrice * this.vatRate21.Rate) / 100, 2) * quantity, item1.TotalVat);

            var purchasePrice = this.goodPurchasePrice.Price * 0.5M;

            Assert.Equal(Math.Round(((item1.UnitBasePrice / purchasePrice) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / purchasePrice) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - purchasePrice) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - purchasePrice) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price * quantity, this.invoice.TotalBasePrice);
            Assert.Equal(0, this.invoice.TotalDiscount);
            Assert.Equal(0, this.invoice.TotalSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price * quantity, this.invoice.TotalExVat);
            Assert.Equal(Math.Round((item1.CalculatedUnitPrice * this.vatRate21.Rate) / 100, 2) * quantity, this.invoice.TotalVat);
            Assert.Equal(item1.UnitPurchasePrice, this.invoice.TotalPurchasePrice);
        }

        [Fact]
        public void GivenInvoiceItemForGood1WithActualPrice_WhenDerivingPrices_ThenUseActualPrice()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(3).WithActualUnitPrice(15).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(10, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(15, item1.CalculatedUnitPrice);
            Assert.Equal(3.15m, item1.UnitVat);
            Assert.Equal(45, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(45, item1.TotalExVat);
            Assert.Equal(9.45m, item1.TotalVat);
            Assert.Equal(54.45m, item1.TotalIncVat);
            Assert.Equal(7, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(45, this.invoice.TotalBasePrice);
            Assert.Equal(0, this.invoice.TotalDiscount);
            Assert.Equal(0, this.invoice.TotalSurcharge);
            Assert.Equal(45, this.invoice.TotalExVat);
            Assert.Equal(9.45m, this.invoice.TotalVat);
            Assert.Equal(54.45m, this.invoice.TotalIncVat);
            Assert.Equal(7, this.invoice.TotalPurchasePrice);
            Assert.Equal(15, this.invoice.TotalListPrice);
        }

        [Fact]
        public void GivenInvoiceItemForGood1_WhenDerivingPrices_ThenUsePriceComponentsForGood1()
        {
            this.InstantiateObjects(this.DatabaseSession);

            const decimal quantity = 3;
            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(Math.Round((item1.CalculatedUnitPrice * this.vatRate21.Rate) / 100, 2), item1.UnitVat);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(this.currentGood1BasePrice.Price * quantity, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price * quantity, item1.TotalExVat);
            Assert.Equal(Math.Round((item1.CalculatedUnitPrice * this.vatRate21.Rate) / 100, 2) * quantity, item1.TotalVat);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price * quantity, this.invoice.TotalBasePrice);
            Assert.Equal(0, this.invoice.TotalDiscount);
            Assert.Equal(0, this.invoice.TotalSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price * quantity, this.invoice.TotalExVat);
            Assert.Equal(Math.Round((item1.CalculatedUnitPrice * this.vatRate21.Rate) / 100, 2) * quantity, this.invoice.TotalVat);
            Assert.Equal(item1.UnitPurchasePrice, this.invoice.TotalPurchasePrice);
        }

        [Fact]
        public void GivenInvoiceItemForFeature1_WhenDerivingPrices_ThenUsePriceComponentsForFeature1()
        {
            this.InstantiateObjects(this.DatabaseSession);

            const decimal quantity = 3;
            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProductFeature(this.feature1).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductFeatureItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentFeature1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentFeature1BasePrice.Price, item1.CalculatedUnitPrice);
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
            this.InstantiateObjects(this.DatabaseSession);

            this.invoice.ShipToAddress = this.billToContactMechanismMechelen;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(3).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentBasePriceGeoBoundary.Price, item1.CalculatedUnitPrice);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(this.billToCustomer)
                .WithBillToContactMechanism(this.billToContactMechanismMechelen)
                .Build();

            item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(3).Build();
            invoice2.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscount_WhenDeriving_ThenUseGeneralDiscountNotBoundToProduct()
        {
            const decimal quantity = 3;
            const decimal amount = 1;
            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithPrice(amount)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForGeoBoundary_WhenDeriving_ThenUseDiscountComponentsForGeoBoundary()
        {
            const decimal quantity = 3;
            const decimal amount = 1;
            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForGeoBoundary_WhenDeriving_ThenUseDiscountComponentsForGeoBoundary()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price?? 0;
            var amount = Math.Round((price * percentage) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForGeoBoundary_WhenDeriving_ThenUseSurchargeComponentsForGeoBoundary()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForGeoBoundary_WhenDeriving_ThenUseSurchargeComponentsForGeoBoundary()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge good for geo boundary")
                .WithGeographicBoundary(this.kiev)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round((price * percentage) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForPartyClassification_WhenDeriving_ThenUseDiscountComponentsForPartyClassification()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            var classification = new IndustryClassificationBuilder(this.DatabaseSession).WithName("gold customer").Build();
            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for party classification")
                .WithPartyClassification(classification)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.billToCustomer.AddPartyClassification(classification);

            this.invoice.ShipToCustomer = this.shipToCustomer;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForPartyClassification_WhenDeriving_ThenUseDiscountComponentsForPartyClassification()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            var classification = new IndustryClassificationBuilder(this.DatabaseSession).WithName("gold customer").Build();
            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for party classification")
                .WithPartyClassification(classification)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.billToCustomer.AddPartyClassification(classification);

            this.invoice.ShipToCustomer = this.shipToCustomer;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round((price * percentage) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForPartyClassification_WhenDeriving_ThenUseSurchargeComponentsForPartyClassification()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            var classification = new IndustryClassificationBuilder(this.DatabaseSession).WithName("gold customer").Build();
            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for party classification")
                .WithPartyClassification(classification)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.billToCustomer.AddPartyClassification(classification);

            this.invoice.ShipToCustomer = this.shipToCustomer;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForPartyClassification_WhenDeriving_ThenUseSurchargeComponentsForPartyClassification()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            var classification = new IndustryClassificationBuilder(this.DatabaseSession).WithName("gold customer").Build();
            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for party classification")
                .WithPartyClassification(classification)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.billToCustomer.AddPartyClassification(classification);

            this.invoice.ShipToCustomer = this.shipToCustomer;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round((price * percentage) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForProductCatergory_WhenDeriving_ThenUseDiscountComponentsForProductCatergory()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            var category = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for product category")
                .WithProductCategory(category)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.good.AddProductCategory(category);

            this.DatabaseSession.Derive();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForProductCatergory_WhenDeriving_ThenUseDiscountComponentsForProductCatergory()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            var category = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for product category")
                .WithProductCategory(category)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.good.AddProductCategory(category);

            this.DatabaseSession.Derive();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round((price * percentage) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForProductCatergory_WhenDeriving_ThenUseSurchargeComponentsForProductCatergory()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            var category = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for product category")
                .WithProductCategory(category)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.good.AddProductCategory(category);

            this.DatabaseSession.Derive();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForProductCatergory_WhenDeriving_ThenUseSurchargeComponentsForProductCatergory()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            var category = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for product category")
                .WithProductCategory(category)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.good.AddProductCategory(category);

            this.DatabaseSession.Derive();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round((price * percentage) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForOrderQuantityBreak_WhenDeriving_ThenUseDiscountComponentsForOrderQuantityBreak()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 50;
            const decimal quantity3 = 50;
            const decimal amount1 = 1;
            const decimal amount2 = 3;

            var break1 = new OrderQuantityBreakBuilder(this.DatabaseSession).WithFromAmount(50).WithThroughAmount(99).Build();
            var break2 = new OrderQuantityBreakBuilder(this.DatabaseSession).WithFromAmount(100).Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for quantity break 1")
                .WithOrderQuantityBreak(break1)
                .WithProduct(this.good)
                .WithPrice(amount1)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for quantity break 2")
                .WithOrderQuantityBreak(break2)
                .WithProduct(this.good)
                .WithPrice(amount2)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item3.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item3.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item3.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item3.UnitBasePrice - this.goodPurchasePrice.Price) / item3.UnitBasePrice) * 100, 2), item3.InitialProfitMargin);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item3.CalculatedUnitPrice) * 100, 2), item3.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForOrderQuantityBreak_WhenDeriving_ThenUseDiscountComponentsForOrderQuantityBreak()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 50;
            const decimal quantity3 = 50;
            const decimal percentage1 = 5;
            const decimal percentage2 = 10;

            var break1 = new OrderQuantityBreakBuilder(this.DatabaseSession).WithFromAmount(50).WithThroughAmount(99).Build();
            var break2 = new OrderQuantityBreakBuilder(this.DatabaseSession).WithFromAmount(100).Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for quantity break 1")
                .WithOrderQuantityBreak(break1)
                .WithProduct(this.good)
                .WithPercentage(percentage1)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for quantity break 2")
                .WithOrderQuantityBreak(break2)
                .WithProduct(this.good)
                .WithPercentage(percentage2)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount1 = Math.Round((price * percentage1) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            var price2 = this.currentGood1BasePrice.Price ?? 0;
            var amount2 = Math.Round((price2 * percentage2) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item3.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item3.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item3.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item3.UnitBasePrice - this.goodPurchasePrice.Price) / item3.UnitBasePrice) * 100, 2), item3.InitialProfitMargin);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item3.CalculatedUnitPrice) * 100, 2), item3.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForOrderQuantityBreak_WhenDeriving_ThenUseSurchargeComponentsForOrderQuantityBreak()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 50;
            const decimal quantity3 = 50;
            const decimal amount1 = 1;
            const decimal amount2 = 3;

            var break1 = new OrderQuantityBreakBuilder(this.DatabaseSession).WithFromAmount(50).WithThroughAmount(99).Build();
            var break2 = new OrderQuantityBreakBuilder(this.DatabaseSession).WithFromAmount(100).Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge good for quantity break 1")
                .WithOrderQuantityBreak(break1)
                .WithProduct(this.good)
                .WithPrice(amount1)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge good for quantity break 2")
                .WithOrderQuantityBreak(break2)
                .WithProduct(this.good)
                .WithPrice(amount2)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item3.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item3.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item3.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item3.UnitBasePrice - this.goodPurchasePrice.Price) / item3.UnitBasePrice) * 100, 2), item3.InitialProfitMargin);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item3.CalculatedUnitPrice) * 100, 2), item3.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForOrderQuantityBreak_WhenDeriving_ThenUseSurchargeComponentsForOrderQuantityBreak()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 50;
            const decimal quantity3 = 50;
            const decimal percentage1 = 5;
            const decimal percentage2 = 10;

            var break1 = new OrderQuantityBreakBuilder(this.DatabaseSession).WithFromAmount(50).WithThroughAmount(99).Build();
            var break2 = new OrderQuantityBreakBuilder(this.DatabaseSession).WithFromAmount(100).Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge good for quantity break 1")
                .WithOrderQuantityBreak(break1)
                .WithProduct(this.good)
                .WithPercentage(percentage1)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge good for quantity break 2")
                .WithOrderQuantityBreak(break2)
                .WithProduct(this.good)
                .WithPercentage(percentage2)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount1 = Math.Round((price * percentage1) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            var price2 = this.currentGood1BasePrice.Price ?? 0;
            var amount2 = Math.Round((price2 * percentage2) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item3.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item3.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item3.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item3.UnitBasePrice - this.goodPurchasePrice.Price) / item3.UnitBasePrice) * 100, 2), item3.InitialProfitMargin);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item3.CalculatedUnitPrice) * 100, 2), item3.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForOrderValue_WhenDeriving_ThenUseDiscountComponentsForOrderValue()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 3;
            const decimal quantity3 = 10;
            const decimal amount1 = 1;
            const decimal amount2 = 3;

            var value1 = new OrderValueBuilder(this.DatabaseSession).WithFromAmount(50).WithThroughAmount(99).Build();
            var value2 = new OrderValueBuilder(this.DatabaseSession).WithFromAmount(100).Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for order value 1")
                .WithOrderValue(value1)
                .WithProduct(this.good)
                .WithPrice(amount1)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for order value 1")
                .WithOrderValue(value2)
                .WithProduct(this.good)
                .WithPrice(amount2)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item3.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item3.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item3.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item3.UnitBasePrice - this.goodPurchasePrice.Price) / item3.UnitBasePrice) * 100, 2), item3.InitialProfitMargin);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item3.CalculatedUnitPrice) * 100, 2), item3.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageForOrderValue_WhenDeriving_ThenUseDiscountComponentsForOrderValue()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 3;
            const decimal quantity3 = 10;
            const decimal percentage1 = 5;
            const decimal percentage2 = 10;

            var value1 = new OrderValueBuilder(this.DatabaseSession).WithFromAmount(50).WithThroughAmount(99).Build();
            var value2 = new OrderValueBuilder(this.DatabaseSession).WithFromAmount(100).Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for order value 1")
                .WithOrderValue(value1)
                .WithProduct(this.good)
                .WithPercentage(percentage1)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for order value 1")
                .WithOrderValue(value2)
                .WithProduct(this.good)
                .WithPercentage(percentage2)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount1 = Math.Round((price * percentage1) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount1, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount1, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount1, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            var price2 = this.currentGood1BasePrice.Price ?? 0;
            var amount2 = Math.Round((price2 * percentage2) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount2, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(amount2, item2.UnitDiscount);
            Assert.Equal(0, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(amount2, item3.UnitDiscount);
            Assert.Equal(0, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount2, item3.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item3.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item3.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item3.UnitBasePrice - this.goodPurchasePrice.Price) / item3.UnitBasePrice) * 100, 2), item3.InitialProfitMargin);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item3.CalculatedUnitPrice) * 100, 2), item3.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForOrderValue_WhenDeriving_ThenUseSurchargeComponentsForOrderValue()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 3;
            const decimal quantity3 = 10;
            const decimal amount1 = 1;
            const decimal amount2 = 3;

            var value1 = new OrderValueBuilder(this.DatabaseSession).WithFromAmount(50).WithThroughAmount(99).Build();
            var value2 = new OrderValueBuilder(this.DatabaseSession).WithFromAmount(100).Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge good for order value 1")
                .WithOrderValue(value1)
                .WithProduct(this.good)
                .WithPrice(amount1)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge good for order value 1")
                .WithOrderValue(value2)
                .WithProduct(this.good)
                .WithPrice(amount2)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item2.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item2.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item2.UnitBasePrice - this.goodPurchasePrice.Price) / item2.UnitBasePrice) * 100, 2), item2.InitialProfitMargin);
            Assert.Equal(Math.Round(((item2.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item2.CalculatedUnitPrice) * 100, 2), item2.MaintainedProfitMargin);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item3.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item3.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item3.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item3.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item3.UnitBasePrice - this.goodPurchasePrice.Price) / item3.UnitBasePrice) * 100, 2), item3.InitialProfitMargin);
            Assert.Equal(Math.Round(((item3.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item3.CalculatedUnitPrice) * 100, 2), item3.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForOrderValue_WhenDeriving_ThenUseSurchargeComponentsForOrderValue()
        {
            const decimal quantity1 = 3;
            const decimal quantity2 = 3;
            const decimal quantity3 = 10;
            const decimal percentage1 = 5;
            const decimal percentage2 = 10;

            var value1 = new OrderValueBuilder(this.DatabaseSession).WithFromAmount(50).WithThroughAmount(99).Build();
            var value2 = new OrderValueBuilder(this.DatabaseSession).WithFromAmount(100).Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
            .WithDescription("surcharge good for order value 1")
            .WithOrderValue(value1)
            .WithProduct(this.good)
            .WithPercentage(percentage1)
            .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
            .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
            .Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
            .WithDescription("surcharge good for order value 1")
            .WithOrderValue(value2)
            .WithProduct(this.good)
            .WithPercentage(percentage2)
            .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
            .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
            .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity1).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity2).Build();
            this.invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount1 = Math.Round((price * percentage1) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount1, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount1, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount1, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity3).Build();
            this.invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            var price2 = this.currentGood1BasePrice.Price ?? 0;
            var amount2 = Math.Round((price2 * percentage2) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount2, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item2.UnitBasePrice);
            Assert.Equal(0, item2.UnitDiscount);
            Assert.Equal(amount2, item2.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item2.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item2.UnitPurchasePrice);

            Assert.Equal(this.currentGood1BasePrice.Price, item3.UnitBasePrice);
            Assert.Equal(0, item3.UnitDiscount);
            Assert.Equal(amount2, item3.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount2, item3.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item3.UnitPurchasePrice);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountAmountForSalesType_WhenDeriving_ThenUseDiscountComponentsForSalesType()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).EmailChannel)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.invoice.SalesChannel = new SalesChannels(this.DatabaseSession).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(amount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithDiscountPercentageAndItemDiscountPercentage_WhenDeriving_ThenExtraDiscountIsCalculated()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;
            const decimal adjustmentPerc = 10;

            var discountAdjustment = new DiscountAdjustmentBuilder(this.DatabaseSession).WithPercentage(adjustmentPerc).Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).EmailChannel)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.invoice.SalesChannel = new SalesChannels(this.DatabaseSession).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithProduct(this.good)
                .WithQuantity(quantity)
                .WithDiscountAdjustment(discountAdjustment)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var discount = Math.Round((price * percentage) / 100, 2);
            var discountedprice = price - discount;
            var adjustmentPercentage = discountAdjustment.Percentage.HasValue ? discountAdjustment.Percentage.Value : 0;
            discount += Math.Round((discountedprice * adjustmentPercentage) / 100, 2);

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(discount, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price - discount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargeAmountForSalesType_WhenDeriving_ThenUseSurchargeComponentsForSalesType()
        {
            const decimal quantity = 3;
            const decimal amount = 1;

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).EmailChannel)
                .WithProduct(this.good)
                .WithPrice(amount)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.invoice.SalesChannel = new SalesChannels(this.DatabaseSession).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageForSalesType_WhenDeriving_ThenUseSurchargeComponentsForSalesType()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).EmailChannel)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.invoice.SalesChannel = new SalesChannels(this.DatabaseSession).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var amount = Math.Round((price * percentage) / 100, 2);
            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(amount, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + amount, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenInvoiceItemWithSurchargePercentageAndItemSurchargePercentage_WhenDeriving_ThenExtraSurchargeIsCalculated()
        {
            const decimal quantity = 3;
            const decimal percentage = 5;
            const decimal adjustmentPerc = 10;

            var surchargeAdjustment = new SurchargeAdjustmentBuilder(this.DatabaseSession).WithPercentage(adjustmentPerc).Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("discount good for sales type")
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).EmailChannel)
                .WithProduct(this.good)
                .WithPercentage(percentage)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            this.invoice.SalesChannel = new SalesChannels(this.DatabaseSession).EmailChannel;

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithProduct(this.good)
                .WithQuantity(quantity)
                .WithSurchargeAdjustment(surchargeAdjustment)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            var price = this.currentGood1BasePrice.Price ?? 0;
            var surcharge = Math.Round((price * percentage) / 100, 2);
            var surchargedprice = price + surcharge;
            var adjustmentPercentage = surchargeAdjustment.Percentage.HasValue ? surchargeAdjustment.Percentage.Value : 0;
            surcharge += Math.Round((surchargedprice * adjustmentPercentage) / 100, 2);

            Assert.Equal(this.currentGood1BasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(surcharge, item1.UnitSurcharge);
            Assert.Equal(this.currentGood1BasePrice.Price + surcharge, item1.CalculatedUnitPrice);
            Assert.Equal(this.goodPurchasePrice.Price, item1.UnitPurchasePrice);

            Assert.Equal(Math.Round(((item1.UnitBasePrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.InitialMarkupPercentage);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice / this.goodPurchasePrice.Price) - 1) * 100, 2), item1.MaintainedMarkupPercentage);
            Assert.Equal(Math.Round(((item1.UnitBasePrice - this.goodPurchasePrice.Price) / item1.UnitBasePrice) * 100, 2), item1.InitialProfitMargin);
            Assert.Equal(Math.Round(((item1.CalculatedUnitPrice - this.goodPurchasePrice.Price) / item1.CalculatedUnitPrice) * 100, 2), item1.MaintainedProfitMargin);
        }

        [Fact]
        public void GivenBillToCustomerWithDifferentCurrency_WhenDerivingPrices_ThenCalculatePricesInPreferredCurrency()
        {
            var poundSterling = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "GBP");

            const decimal conversionfactor = 0.8553M;
            var euroToPoundStirling = new UnitOfMeasureConversionBuilder(this.DatabaseSession)
                .WithConversionFactor(conversionfactor)
                .WithToUnitOfMeasure(poundSterling)
                .WithStartDate(DateTime.UtcNow)
                .Build();

            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            euro.AddUnitOfMeasureConversion(euroToPoundStirling);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            Assert.Equal(euro, this.invoice.CustomerCurrency);

            this.billToCustomer.PreferredCurrency = poundSterling;

            var newInvoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(this.billToCustomer)
                .WithBillToContactMechanism(this.billToContactMechanismMechelen)
                .Build();

            const decimal quantity = 3;
            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).WithQuantity(quantity).Build();
            newInvoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(poundSterling, newInvoice.CustomerCurrency);

            Assert.Equal(Math.Round(item1.TotalBasePrice * conversionfactor, 2), item1.TotalBasePriceCustomerCurrency);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(Math.Round(item1.TotalExVat * conversionfactor, 2), item1.TotalExVatCustomerCurrency);
        }

        [Fact]
        public void GivenInvoiceItemWithSalesRepForThisExactProductCategory_WhenDerivingSalesRep_ThenSalesRepForProductCategoryIsSelected()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var salesrep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for child product category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep2  = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for parent category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for everything else").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var parentProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("parent").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("child").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(parentProductCategory).
                Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(this.invoice.BillToCustomer)
                .WithProductCategory(childProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(this.invoice.BillToCustomer)
                .WithProductCategory(parentProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(this.invoice.BillToCustomer)
                .Build();

            this.good.AddProductCategory(childProductCategory);

            this.DatabaseSession.Derive();

            var invoiceItem = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            this.invoice.AddSalesInvoiceItem(invoiceItem);

            this.DatabaseSession.Derive();

            Assert.Equal(invoiceItem.SalesRep, salesrep1);
        }

        [Fact]
        public void GivenInvoiceItemWithSalesRepForThisProductsParent_WhenDerivingSalesRep_ThenSalesRepForParentCategoryIsSelected()
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
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(this.invoice.BillToCustomer)
                .WithProductCategory(childProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(this.invoice.BillToCustomer)
                .WithProductCategory(parentProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(this.invoice.BillToCustomer)
                .Build();

            this.good.AddProductCategory(parentProductCategory);

            this.DatabaseSession.Derive();

            var invoiceItem = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            this.invoice.AddSalesInvoiceItem(invoiceItem);

            this.DatabaseSession.Derive();

            Assert.Equal(invoiceItem.SalesRep, salesrep2);
        }

        [Fact]
        public void GivenInvoiceItemForProductWithoutCategory_WhenDerivingSalesRep_ThenSalesRepForCustomerIsSelected()
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
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(this.invoice.BillToCustomer)
                .WithProductCategory(childProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(this.invoice.BillToCustomer)
                .WithProductCategory(parentProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(this.invoice.BillToCustomer)
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            this.invoice.AddSalesInvoiceItem(invoiceItem);

            this.DatabaseSession.Derive();

            Assert.Equal(invoiceItem.SalesRep, salesrep3);
        }

        [Fact]
        public void GiveninvoiceItem_WhenPartialPaymentIsReceived_ThenInvoiceItemStateIsSetToPartiallyPaid()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithQuantity(1)
                .WithActualUnitPrice(100M)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(item1).WithAmountApplied(50).Build())
                .WithEffectiveDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesInvoiceItemObjectStates(this.DatabaseSession).PartiallyPaid, item1.CurrentObjectState);
        }

        [Fact]
        public void GiveninvoiceItem_WhenFullPaymentIsReceived_ThenInvoiceItemStateIsSetToPaid()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithProduct(this.good)
                .WithQuantity(1)
                .WithActualUnitPrice(100M)
                .Build();

            this.invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(121)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(item1).WithAmountApplied(121).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesInvoiceItemObjectStates(this.DatabaseSession).Paid, item1.CurrentObjectState);
        }

        private void InstantiateObjects(ISession session)
        {
            this.good = (Good)session.Instantiate(this.good);
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
            this.goodPurchasePrice = (ProductPurchasePrice)session.Instantiate(this.goodPurchasePrice);
            this.goodSupplierOffering = (SupplierOffering)session.Instantiate(this.goodSupplierOffering);
            this.currentBasePriceGeoBoundary = (BasePrice)session.Instantiate(this.currentBasePriceGeoBoundary);
            this.currentFeature1BasePrice = (BasePrice)session.Instantiate(this.currentFeature1BasePrice);
            this.currentGood1BasePrice = (BasePrice)session.Instantiate(this.currentGood1BasePrice);
            this.currentGood1Feature1BasePrice = (BasePrice)session.Instantiate(this.currentGood1Feature1BasePrice);
            this.invoice = (SalesInvoice)session.Instantiate(this.invoice);
            this.vatRate21 = (VatRate)session.Instantiate(this.vatRate21);
        }
    }
}
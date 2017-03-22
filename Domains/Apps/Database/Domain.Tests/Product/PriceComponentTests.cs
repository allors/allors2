//------------------------------------------------------------------------------------------------- 
// <copyright file="PriceComponentTests.cs" company="Allors bvba">
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
    using Xunit;

    
    public class PriceComponentTests : DomainTest
    {
        [Fact]
        public void GivenBasePrice_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("Gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var colorFeature = new ColourBuilder(this.DatabaseSession)
                .WithName("golden")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                                            .WithText("black")
                                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                                            .Build())
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new BasePriceBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPrice(1);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithProduct(good);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithProductFeature(colorFeature);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);
        }

        [Fact]
        public void GivenBasePriceForVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsUpdated()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var virtualGood = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("virtual gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var physicalGood = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("real gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            virtualGood.AddVariant(physicalGood);

            this.DatabaseSession.Derive(true);

            var basePrice = new BasePriceBuilder(this.DatabaseSession)
                .WithDescription("baseprice")
                .WithPrice(10)
                .WithProduct(virtualGood)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(1, physicalGood.VirtualProductPriceComponents.Count);
            Assert.Contains(basePrice, physicalGood.VirtualProductPriceComponents);
            Assert.False(virtualGood.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenBasePriceForNonVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsNull()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var physicalGood = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("real gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new BasePriceBuilder(this.DatabaseSession)
                .WithDescription("baseprice")
                .WithPrice(10)
                .WithProduct(physicalGood)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            Assert.False(physicalGood.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenDiscount_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var colorFeature = new ColourBuilder(this.DatabaseSession)
             .WithName("golden")
             .WithVatRate(vatRate21)
             .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                                         .WithText("black")
                                         .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                                         .Build())
             .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new DiscountComponentBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPrice(1);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);

            builder.WithProduct(good);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);

            builder.WithProductFeature(colorFeature);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPercentage(10);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);
        }

        [Fact]
        public void GivenDiscountForVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsUpdated()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var virtualService = new DeliverableBasedServiceBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("virtual service").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            var physicalService = new DeliverableBasedServiceBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("real service").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            virtualService.AddVariant(physicalService);

            this.DatabaseSession.Derive(true);

            var discount = new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount")
                .WithPrice(10)
                .WithProduct(virtualService)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(1, physicalService.VirtualProductPriceComponents.Count);
            Assert.Contains(discount, physicalService.VirtualProductPriceComponents);
            Assert.False(virtualService.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenDiscountForNonVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsNull()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var physicalService = new DeliverableBasedServiceBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("real service").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithDescription("discount")
                .WithPrice(10)
                .WithProduct(physicalService)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            Assert.False(physicalService.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenSurcharge_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var colorFeature = new ColourBuilder(this.DatabaseSession)
                .WithName("golden")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                                            .WithText("black")
                                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                                            .Build())
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new SurchargeComponentBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPrice(1);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);

            builder.WithProduct(good);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);

            builder.WithProductFeature(colorFeature);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);

            builder.WithPercentage(10);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);
        }

        [Fact]
        public void GivenSurchargeForVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsUpdated()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var virtualService = new TimeAndMaterialsServiceBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("virtual service").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            var physicalService = new TimeAndMaterialsServiceBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("real service").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            virtualService.AddVariant(physicalService);

            this.DatabaseSession.Derive(true);

            var surcharge = new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge")
                .WithPrice(10)
                .WithProduct(virtualService)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(1, physicalService.VirtualProductPriceComponents.Count);
            Assert.Contains(surcharge, physicalService.VirtualProductPriceComponents);
            Assert.False(virtualService.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenSurchargeForNonVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsNull()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var physicalService = new TimeAndMaterialsServiceBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("real service").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            new SurchargeComponentBuilder(this.DatabaseSession)
                .WithDescription("surcharge")
                .WithPrice(10)
                .WithProduct(physicalService)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            Assert.False(physicalService.ExistVirtualProductPriceComponents);
        }
    }
}

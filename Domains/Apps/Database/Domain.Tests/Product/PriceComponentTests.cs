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
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var good = new GoodBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gizmo").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var colorFeature = new ColourBuilder(this.Session)
                .WithName("golden")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session)
                                            .WithText("black")
                                            .WithLocale(this.Session.GetSingleton().DefaultLocale)
                                            .Build())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new BasePriceBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPrice(1);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithProduct(good);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithProductFeature(colorFeature);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenBasePriceForVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsUpdated()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var virtualGood = new GoodBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("virtual gizmo").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var physicalGood = new GoodBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("real gizmo").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            virtualGood.AddVariant(physicalGood);

            this.Session.Derive();

            var basePrice = new BasePriceBuilder(this.Session)
                .WithDescription("baseprice")
                .WithPrice(10)
                .WithProduct(virtualGood)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(1, physicalGood.VirtualProductPriceComponents.Count);
            Assert.Contains(basePrice, physicalGood.VirtualProductPriceComponents);
            Assert.False(virtualGood.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenBasePriceForNonVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsNull()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var physicalGood = new GoodBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("real gizmo").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new BasePriceBuilder(this.Session)
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
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var good = new GoodBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("gizmo").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var colorFeature = new ColourBuilder(this.Session)
             .WithName("golden")
             .WithVatRate(vatRate21)
             .WithLocalisedName(new LocalisedTextBuilder(this.Session)
                                         .WithText("black")
                                         .WithLocale(this.Session.GetSingleton().DefaultLocale)
                                         .Build())
             .Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new DiscountComponentBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPrice(1);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithProduct(good);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithProductFeature(colorFeature);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPercentage(10);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenDiscountForVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsUpdated()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var virtualService = new DeliverableBasedServiceBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("virtual service").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            var physicalService = new DeliverableBasedServiceBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("real service").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            virtualService.AddVariant(physicalService);

            this.Session.Derive();

            var discount = new DiscountComponentBuilder(this.Session)
                .WithDescription("discount")
                .WithPrice(10)
                .WithProduct(virtualService)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(1, physicalService.VirtualProductPriceComponents.Count);
            Assert.Contains(discount, physicalService.VirtualProductPriceComponents);
            Assert.False(virtualService.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenDiscountForNonVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsNull()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var physicalService = new DeliverableBasedServiceBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("real service").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            new DiscountComponentBuilder(this.Session)
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
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var good = new GoodBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("gizmo").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var colorFeature = new ColourBuilder(this.Session)
                .WithName("golden")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session)
                                            .WithText("black")
                                            .WithLocale(this.Session.GetSingleton().DefaultLocale)
                                            .Build())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new SurchargeComponentBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPrice(1);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithProduct(good);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithProductFeature(colorFeature);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithPercentage(10);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSurchargeForVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsUpdated()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var virtualService = new TimeAndMaterialsServiceBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("virtual service").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            var physicalService = new TimeAndMaterialsServiceBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("real service").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            virtualService.AddVariant(physicalService);

            this.Session.Derive();

            var surcharge = new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge")
                .WithPrice(10)
                .WithProduct(virtualService)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(1, physicalService.VirtualProductPriceComponents.Count);
            Assert.Contains(surcharge, physicalService.VirtualProductPriceComponents);
            Assert.False(virtualService.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenSurchargeForNonVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsNull()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var physicalService = new TimeAndMaterialsServiceBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("real service").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithVatRate(vatRate21)
                .Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge")
                .WithPrice(10)
                .WithProduct(physicalService)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            Assert.False(physicalService.ExistVirtualProductPriceComponents);
        }
    }
}

// <copyright file="PriceComponentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;

    using Xunit;

    public class PriceComponentTests : DomainTest
    {
        [Fact]
        public void GivenBasePrice_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            var colorFeature = new ColourBuilder(this.Session)
                .WithVatRegime(vatRegime)
                .WithName("black")
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

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithProductFeature(colorFeature);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(this.Session.Now());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenBasePriceForVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsUpdated()
        {
            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;

            var virtualGood = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("v101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithName("virtual gizmo")
                .WithVatRegime(vatRegime)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var physicalGood = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            virtualGood.AddVariant(physicalGood);

            this.Session.Derive();

            var basePrice = new BasePriceBuilder(this.Session)
                .WithDescription("baseprice")
                .WithPrice(10)
                .WithProduct(virtualGood)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Single(physicalGood.VirtualProductPriceComponents);
            Assert.Contains(basePrice, physicalGood.VirtualProductPriceComponents);
            Assert.False(virtualGood.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenBasePriceForNonVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsNull()
        {
            var physicalGood = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            new BasePriceBuilder(this.Session)
                .WithDescription("baseprice")
                .WithPrice(10)
                .WithProduct(physicalGood)
                .WithFromDate(this.Session.Now())
                .Build();

            Assert.False(physicalGood.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenDiscount_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            var colorFeature = new ColourBuilder(this.Session)
             .WithVatRegime(vatRegime)
             .WithName("black")
             .Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new DiscountComponentBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPrice(1);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(this.Session.Now());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithProduct(good);
            builder.Build();

            this.Session.Rollback();

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
            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var virtualService = new DeliverableBasedServiceBuilder(this.Session)
                .WithName("virtual service")
                .WithVatRegime(vatRegime)
                .Build();

            var physicalService = new DeliverableBasedServiceBuilder(this.Session)
                .WithName("real service")
                .WithVatRegime(vatRegime)
                .Build();

            virtualService.AddVariant(physicalService);

            this.Session.Derive();

            var discount = new DiscountComponentBuilder(this.Session)
                .WithDescription("discount")
                .WithPrice(10)
                .WithProduct(virtualService)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Single(physicalService.VirtualProductPriceComponents);
            Assert.Contains(discount, physicalService.VirtualProductPriceComponents);
            Assert.False(virtualService.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenDiscountForNonVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsNull()
        {
            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var physicalService = new DeliverableBasedServiceBuilder(this.Session)
                .WithName("real service")
                .WithVatRegime(vatRegime)
                .Build();

            new DiscountComponentBuilder(this.Session)
                .WithDescription("discount")
                .WithPrice(10)
                .WithProduct(physicalService)
                .WithFromDate(this.Session.Now())
                .Build();

            Assert.False(physicalService.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenSurcharge_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            var colorFeature = new ColourBuilder(this.Session)
                .WithVatRegime(vatRegime)
                .WithName("black")
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new SurchargeComponentBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPrice(1);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(this.Session.Now());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

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
        public void GivenSurchargeForVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsUpdated()
        {
            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var virtualService = new TimeAndMaterialsServiceBuilder(this.Session)
                .WithName("virtual service")
                .WithVatRegime(vatRegime)
                .Build();

            var physicalService = new TimeAndMaterialsServiceBuilder(this.Session)
                .WithName("real service")
                .WithVatRegime(vatRegime)
                .Build();

            virtualService.AddVariant(physicalService);

            this.Session.Derive();

            var surcharge = new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge")
                .WithPrice(10)
                .WithProduct(virtualService)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Single(physicalService.VirtualProductPriceComponents);
            Assert.Contains(surcharge, physicalService.VirtualProductPriceComponents);
            Assert.False(virtualService.ExistVirtualProductPriceComponents);
        }

        [Fact]
        public void GivenSurchargeForNonVirtualProduct_WhenDeriving_ThenProductVirtualProductPriceComponentIsNull()
        {
            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var physicalService = new TimeAndMaterialsServiceBuilder(this.Session)
                .WithName("real service")
                .WithVatRegime(vatRegime)
                .Build();

            new SurchargeComponentBuilder(this.Session)
                .WithDescription("surcharge")
                .WithPrice(10)
                .WithProduct(physicalService)
                .WithFromDate(this.Session.Now())
                .Build();

            Assert.False(physicalService.ExistVirtualProductPriceComponents);
        }
    }
}

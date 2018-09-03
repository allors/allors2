//------------------------------------------------------------------------------------------------- 
// <copyright file="SupplierOfferingTests.cs" company="Allors bvba">
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

    public class SupplierOfferingTests : DomainTest
    {
        [Fact]
        public void GivenSupplierOffering_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("organisation").Build();
            var part = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var purchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithFromDate(DateTime.UtcNow)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Commit();

            var builder = new SupplierOfferingBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithProductPurchasePrice(purchasePrice);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithSupplier(supplier);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithPart(part);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenNewGood_WhenDeriving_ThenNonSerialisedInventryItemIsCreatedForEveryFacility()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var internalOrganisation = this.InternalOrganisation;

            var secondFacility = new FacilityBuilder(this.Session)
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithOwner(this.InternalOrganisation)
                .WithName("second facility")
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var purchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithFromDate(DateTime.UtcNow)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good = new GoodBuilder(this.Session)
                .WithName("good")
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(finishedGood)
                .WithSupplier(supplier)
                .WithProductPurchasePrice(purchasePrice)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.Session.Derive(); 

            Assert.Equal(2, finishedGood.InventoryItemsWherePart.Count);
            Assert.Equal(1, internalOrganisation.DefaultFacility.InventoryItemsWhereFacility.Count);
            Assert.Equal(1, secondFacility.InventoryItemsWhereFacility.Count);
        }

        [Fact]
        public void GivenNewGoodBasedOnFinishedGood_WhenDeriving_ThenNonSerialisedInventryItemIsCreatedForEveryFinishedGoodAndFacility()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();

            var secondFacility = new FacilityBuilder(this.Session)
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithName("second facility")
                .WithOwner(this.InternalOrganisation)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var purchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithFromDate(DateTime.UtcNow)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good = new GoodBuilder(this.Session)
                .WithName("good")
                .WithSku("10101")
                .WithFinishedGood(finishedGood)
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(finishedGood)
                .WithSupplier(supplier)
                .WithProductPurchasePrice(purchasePrice)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.Session.Derive(); 

            Assert.Equal(2, good.FinishedGood.InventoryItemsWherePart.Count);
            Assert.Equal(1, this.InternalOrganisation.DefaultFacility.InventoryItemsWhereFacility.Count);
            Assert.Equal(1, secondFacility.InventoryItemsWhereFacility.Count);
        }
    }
}

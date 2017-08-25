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
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("organisation").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var part = new FinishedGoodBuilder(this.DatabaseSession).WithName("finishedGood").Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var purchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Commit();

            var builder = new SupplierOfferingBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithProduct(good);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithProductPurchasePrice(purchasePrice);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSupplier(supplier);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            builder.WithPart(part);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            var supplierOffering = builder.Build(); 
            supplierOffering.RemoveProduct();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenNewGood_WhenDeriving_ThenNonSerialisedInventryItemIsCreatedForEveryFacility()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            var secondFacility = new WarehouseBuilder(this.DatabaseSession).WithName("second facility").WithOwner(internalOrganisation).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var purchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithProductPurchasePrice(purchasePrice)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(); 

            Assert.Equal(2, good.InventoryItemVersionedsWhereGood.Count);
            Assert.Equal(1, internalOrganisation.DefaultFacility.InventoryItemVersionedsWhereFacility.Count);
            Assert.Equal(1, secondFacility.InventoryItemVersionedsWhereFacility.Count);
        }

        [Fact]
        public void GivenNewGoodCoredOnFinishedGood_WhenDeriving_ThenNonSerialisedInventryItemIsCreatedForEveryFinishedGoodAndFacility()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            var secondFacility = new WarehouseBuilder(this.DatabaseSession).WithName("second facility").WithOwner(internalOrganisation).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var finishedGood = new FinishedGoodBuilder(this.DatabaseSession)
                .WithName("part")
                .Build();

            var purchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithFinishedGood(finishedGood)
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithProductPurchasePrice(purchasePrice)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(); 

            Assert.Equal(2, good.FinishedGood.InventoryItemVersionedsWherePart.Count);
            Assert.Equal(1, internalOrganisation.DefaultFacility.InventoryItemVersionedsWhereFacility.Count);
            Assert.Equal(1, secondFacility.InventoryItemVersionedsWhereFacility.Count);
        }
    }
}

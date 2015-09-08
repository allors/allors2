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
    using NUnit.Framework;

    [TestFixture]
    public class SupplierOfferingTests : DomainTest
    {
        [Test]
        public void GivenSupplierOffering_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("organisation").Build();
            var part = new FinishedGoodBuilder(this.DatabaseSession).WithName("finishedGood").Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var purchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithPrice(1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Commit();

            var builder = new SupplierOfferingBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithProduct(good);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithProductPurchasePrice(purchasePrice);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSupplier(supplier);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            builder.WithPart(part);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            var supplierOffering = builder.Build(); 
            supplierOffering.RemoveProduct();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenNewGood_WhenDeriving_ThenNonSerializedInventryItemIsCreatedForEveryFacility()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            var secondFacility = new WarehouseBuilder(this.DatabaseSession).WithName("second facility").WithOwner(internalOrganisation).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var purchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithPrice(1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithProductPurchasePrice(purchasePrice)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(2, good.InventoryItemsWhereGood.Count);
            Assert.AreEqual(1, internalOrganisation.DefaultFacility.InventoryItemsWhereFacility.Count);
            Assert.AreEqual(1, secondFacility.InventoryItemsWhereFacility.Count);
        }

        [Test]
        public void GivenNewGoodCoredOnFinishedGood_WhenDeriving_ThenNonSerializedInventryItemIsCreatedForEveryFinishedGoodAndFacility()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
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
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithPrice(1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
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

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(2, good.FinishedGood.InventoryItemsWherePart.Count);
            Assert.AreEqual(1, internalOrganisation.DefaultFacility.InventoryItemsWhereFacility.Count);
            Assert.AreEqual(1, secondFacility.InventoryItemsWhereFacility.Count);
        }
    }
}

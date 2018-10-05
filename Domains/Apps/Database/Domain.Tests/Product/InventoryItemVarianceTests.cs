//------------------------------------------------------------------------------------------------- 
// <copyright file="NonSerialisedInventoryItemTests.cs" company="Allors bvba">
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
    using Meta;
    using Should;
    using System.Linq;
    using Xunit;

    public class InventoryItemVarianceTests : DomainTest
    {

        [Fact]
        public void GivenInventoryItem_WhenPositiveVariance_ThenQuantityOnHandIsRaised()
        {
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

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(finishedGood)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(0, good.QuantityOnHand);
            Assert.Equal(0, inventoryItem.QuantityOnHand);

            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(10).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();
            Assert.Equal(10, good.QuantityOnHand);
            Assert.Equal(10, inventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenSerializedInventoryItems_WhenVarianceContainsInvalidQuantity_ThenDerivationExceptionRaised()
        {
            // Arrange
            var kinds = new InventoryItemKinds(this.Session);
            var unitsOfMeasure = new UnitsOfMeasure(this.Session);
            var unknown = new VarianceReasons(this.Session).Unknown;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = CreateFinishedGood("FG1", kinds.Serialised);
            var good = CreateGood("10101", vatRate21, "good1", unitsOfMeasure.Piece, category, finishedGood);
            var serialItem1 = CreateSerialzedInventoryItem("1", finishedGood);
            var variance = CreateInventoryVariance(10, unknown);

            serialItem1.AddInventoryItemVariance(variance);

            // Act
            var derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
            derivation.Errors.SelectMany(e => e.RoleTypes).Contains(M.InventoryItemVariance.Quantity).ShouldBeTrue();

            // Re-Arrange
            variance.Quantity = -10;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
            derivation.Errors.SelectMany(e => e.RoleTypes).Contains(M.InventoryItemVariance.Quantity).ShouldBeTrue();
        }

        private FinishedGood CreateFinishedGood(string partId, InventoryItemKind kind)
            => new FinishedGoodBuilder(this.Session).WithPartId(partId).WithInventoryItemKind(kind).Build();

        private Good CreateGood(string sku, VatRate vatRate, string name, UnitOfMeasure uom, ProductCategory category, FinishedGood finishedGood)
            => new GoodBuilder(this.Session)
                .WithSku(sku)
                .WithVatRate(vatRate)
                .WithName(name)
                .WithUnitOfMeasure(uom)
                .WithPrimaryProductCategory(category)
                .WithFinishedGood(finishedGood)
                .Build();

        private SerialisedInventoryItem CreateSerialzedInventoryItem(string serialNumber, Part part)
            => new SerialisedInventoryItemBuilder(this.Session).WithSerialNumber(serialNumber).WithPart(part).Build();

        private InventoryItemVariance CreateInventoryVariance(int quantity, VarianceReason reason)
           => new InventoryItemVarianceBuilder(this.Session).WithQuantity(quantity).WithReason(reason).Build();
    }
}

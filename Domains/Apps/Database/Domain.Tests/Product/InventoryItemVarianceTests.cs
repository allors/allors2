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

    public class InventoryItemTransactionTests : DomainTest
    {

        [Fact]
        public void GivenInventoryItem_WhenPositiveVariance_ThenQuantityOnHandIsRaised()
        {
            var nonSerialized = new InventoryItemKinds(this.Session).NonSerialised;
            var unknown = new InventoryTransactionReasons(this.Session).Unknown;
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var piece = new UnitsOfMeasure(this.Session).Piece;
            var category = this.Session.Extent<ProductCategory>().First;

            var finishedGood = CreatePart("FG1", nonSerialized);

            this.Session.Derive(true);
            this.Session.Commit();

            var inventoryItem = (NonSerialisedInventoryItem)finishedGood.InventoryItemsWherePart.First();

            Assert.Equal(0, finishedGood.QuantityOnHand);
            Assert.Equal(0, inventoryItem.QuantityOnHand);

            var transaction = CreateInventoryTransaction(10, unknown, finishedGood);
            this.Session.Derive(true);

            Assert.Equal(10, finishedGood.QuantityOnHand);
            Assert.Equal(10, inventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenSerializedInventoryItems_WhenVarianceContainsInvalidQuantity_ThenDerivationExceptionRaised()
        {
            // Arrange
            var kinds = new InventoryItemKinds(this.Session);
            var unitsOfMeasure = new UnitsOfMeasure(this.Session);
            var unknown = new InventoryTransactionReasons(this.Session).Unknown;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = CreatePart("FG1", kinds.Serialised);
            var good = CreateGood("10101", vatRate21, "good1", unitsOfMeasure.Piece, category, finishedGood);
            var serialItem = new SerialisedItemBuilder(this.Session).WithSerialNumber("1").Build();
            var variance = CreateInventoryTransaction(10, unknown, finishedGood, serialItem);

            // Act
            var derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
            derivation.Errors.SelectMany(e => e.RoleTypes).Contains(M.InventoryItemTransaction.Quantity).ShouldBeTrue();

            // Re-Arrange
            variance.Quantity = -10;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();
            derivation.Errors.SelectMany(e => e.RoleTypes).Contains(M.InventoryItemTransaction.Quantity).ShouldBeTrue();
        }

        private Part CreatePart(string partId, InventoryItemKind kind)
            => new PartBuilder(this.Session).WithPartId(partId).WithInventoryItemKind(kind).Build();

        private Good CreateGood(string sku, VatRate vatRate, string name, UnitOfMeasure uom, ProductCategory category, Part part)
            => new GoodBuilder(this.Session)
                .WithSku(sku)
                .WithVatRate(vatRate)
                .WithName(name)
                .WithUnitOfMeasure(uom)
                .WithPrimaryProductCategory(category)
                .WithPart(part)
                .Build();

        private InventoryItemTransaction CreateInventoryTransaction(int quantity, InventoryTransactionReason reason, Part part)
           => new InventoryItemTransactionBuilder(this.Session).WithQuantity(quantity).WithReason(reason).WithPart(part).Build();

        private InventoryItemTransaction CreateInventoryTransaction(int quantity, InventoryTransactionReason reason, Part part, SerialisedItem serialisedItem)
           => new InventoryItemTransactionBuilder(this.Session).WithQuantity(quantity).WithReason(reason).WithPart(part).WithSerialisedItem(serialisedItem).Build();
    }
}

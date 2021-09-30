// <copyright file="InventoryItemVarianceTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;
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

            var finishedGood = this.CreatePart("FG1", nonSerialized);

            this.Session.Derive(true);
            this.Session.Commit();

            var inventoryItem = (NonSerialisedInventoryItem)finishedGood.InventoryItemsWherePart.First();

            Assert.Equal(0, finishedGood.QuantityOnHand);
            Assert.Equal(0, inventoryItem.QuantityOnHand);

            var transaction = this.CreateInventoryTransaction(10, unknown, finishedGood);
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

            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = this.CreatePart("FG1", kinds.Serialised);
            var good = this.CreateGood("10101", vatRegime, "good1", unitsOfMeasure.Piece, category, finishedGood);
            var serialItem = new SerialisedItemBuilder(this.Session).WithSerialNumber("1").Build();
            var variance = this.CreateInventoryTransaction(10, unknown, finishedGood, serialItem);

            // Act
            var derivation = this.Session.Derive(false);

            // Assert
            Assert.True(derivation.HasErrors);
            Assert.Contains(M.InventoryItemTransaction.Quantity, derivation.Errors.SelectMany(e => e.RoleTypes));

            // Re-Arrange
            variance.Quantity = -10;

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            Assert.True(derivation.HasErrors);
            Assert.Contains(M.InventoryItemTransaction.Quantity, derivation.Errors.SelectMany(e => e.RoleTypes));
        }

        private Part CreatePart(string partId, InventoryItemKind kind)
            => new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification(partId)
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build()).WithInventoryItemKind(kind).Build();

        private Good CreateGood(string sku, VatRegime vatRegime, string name, UnitOfMeasure uom, ProductCategory category, Part part)
            => new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new SkuIdentificationBuilder(this.Session)
                    .WithIdentification(sku)
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Sku).Build())
                .WithVatRegime(vatRegime)
                .WithName(name)
                .WithUnitOfMeasure(uom)
                .WithPart(part)
                .Build();

        private InventoryItemTransaction CreateInventoryTransaction(int quantity, InventoryTransactionReason reason, Part part)
           => new InventoryItemTransactionBuilder(this.Session).WithQuantity(quantity).WithReason(reason).WithPart(part).Build();

        private InventoryItemTransaction CreateInventoryTransaction(int quantity, InventoryTransactionReason reason, Part part, SerialisedItem serialisedItem)
           => new InventoryItemTransactionBuilder(this.Session).WithQuantity(quantity).WithReason(reason).WithPart(part).WithSerialisedItem(serialisedItem).Build();
    }
}

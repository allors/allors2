//------------------------------------------------------------------------------------------------- 
// <copyright file="SerialisedInventoryItemTests.cs" company="Allors bvba">
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
    using Xunit;
    using Should;
    using System.Linq;
    
    public class SerialisedInventoryItemTests : DomainTest
    {
        [Fact]
        public void GivenInventoryItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            // Arrange
            var part = new PartBuilder(this.Session).WithName("part")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                .WithPartId("1")
                .Build();

            this.Session.Derive(true);
            this.Session.Commit();

            var builder = new SerialisedInventoryItemBuilder(this.Session).WithPart(part);
            builder.Build();

            // Act
            var derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeTrue();

            // Re-arrange
            this.Session.Rollback();

            builder.WithSerialNumber("1");
            builder.Build();

            // Act
            derivation = this.Session.Derive(false);

            // Assert
            derivation.HasErrors.ShouldBeFalse();
        }

        [Fact]
        public void GivenInventoryItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            // Arrange
            var available = new SerialisedInventoryItemStates(this.Session).Available;
            var warehouse = new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse);
            var kinds = new InventoryItemKinds(this.Session);

            var finishedGood = CreatePart("1", kinds.Serialised);
            var serializedItem = new SerialisedInventoryItemBuilder(this.Session).WithSerialNumber("1").WithPart(finishedGood).Build();

            // Act
            this.Session.Derive(true);

            // Assert
            serializedItem.SerialisedInventoryItemState.ShouldEqual(available);
            serializedItem.Facility.ShouldEqual(warehouse);
        }

        [Fact]
        public void GivenFinishedGoodWithSerializedInventory_WhenDeriving_ThenQuantityOnHandUpdated()
        {
            // Arrange
            var available = new SerialisedInventoryItemStates(this.Session).Available;
            var warehouse = new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse);

            var kinds = new InventoryItemKinds(this.Session);
            var unitsOfMeasure = new UnitsOfMeasure(this.Session);
            var unknown = new InventoryTransactionReasons(this.Session).Unknown;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var serialPart = CreatePart("FG1", kinds.Serialised);
            var good = CreateGood("10101", vatRate21, "good1", unitsOfMeasure.Piece, category, serialPart);

            // Act
            this.Session.Derive(true);

            CreateInventoryTransaction(1, unknown, serialPart, "1");
            CreateInventoryTransaction(1, unknown, serialPart, "2");
            CreateInventoryTransaction(1, unknown, serialPart, "3");

            this.Session.Derive(true);

            // Assert
            serialPart.QuantityOnHand.ShouldEqual(3);
        }

        [Fact]
        public void GivenSerializedItemInMultipleFacilities_WhenDeriving_ThenMultipleQuantityOnHandTracked()
        {
            // Arrange
            var warehouseType = new FacilityTypes(this.Session).Warehouse;
            var warehouse1 = CreateFacility("WH1", warehouseType, this.InternalOrganisation);
            var warehouse2 = CreateFacility("WH2", warehouseType, this.InternalOrganisation);

            var serialized = new InventoryItemKinds(this.Session).Serialised;
            var piece = new UnitsOfMeasure(this.Session).Piece;
            var unknown = new InventoryTransactionReasons(this.Session).Unknown;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = CreatePart("FG1", serialized);
            var good = CreateGood("10101", vatRate21, "good1", piece, category, finishedGood);

            // Act
            this.Session.Derive(true);

            CreateInventoryTransaction(1, unknown, finishedGood, "1", warehouse1);
            CreateInventoryTransaction(1, unknown, finishedGood, "2", warehouse2);

            this.Session.Derive(true);

            // Assert
            var item1 = (SerialisedInventoryItem)new InventoryItems(this.Session).Extent().First(i => i.Facility.Equals(warehouse1));
            item1.QuantityOnHand.ShouldEqual(1);

            var item2 = (SerialisedInventoryItem)new InventoryItems(this.Session).Extent().First(i => i.Facility.Equals(warehouse2));
            item2.QuantityOnHand.ShouldEqual(1);

            finishedGood.QuantityOnHand.ShouldEqual(2);
        }

        private Facility CreateFacility(string name, FacilityType type, InternalOrganisation owner)
            => new FacilityBuilder(this.Session).WithName(name).WithFacilityType(type).WithOwner(owner).Build();

        private Good CreateGood(string sku, VatRate vatRate, string name, UnitOfMeasure uom, ProductCategory category, Part part)
            => new GoodBuilder(this.Session)
                .WithSku(sku)
                .WithVatRate(vatRate)
                .WithName(name)
                .WithUnitOfMeasure(uom)
                .WithPrimaryProductCategory(category)
                .WithPart(part)
                .Build();        

        private Part CreatePart(string partId, InventoryItemKind kind)
            => new PartBuilder(this.Session).WithPartId(partId).WithInventoryItemKind(kind).Build();

        private InventoryItemTransaction CreateInventoryTransaction(int quantity, InventoryTransactionReason reason, Part part, string serialNumber)
           => new InventoryItemTransactionBuilder(this.Session).WithQuantity(quantity).WithReason(reason).WithPart(part).WithSerialNumber(serialNumber).Build();

        private InventoryItemTransaction CreateInventoryTransaction(int quantity, InventoryTransactionReason reason, Part part, string serialNumber, Facility facility)
           => new InventoryItemTransactionBuilder(this.Session).WithQuantity(quantity).WithReason(reason).WithPart(part).WithSerialNumber(serialNumber).WithFacility(facility).Build();
    }
}

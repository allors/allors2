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
    using System;
    using Meta;
    using Xunit;
    using Should;


    public class NonSerialisedInventoryItemTests : DomainTest
    {
        [Fact]
        public void GivenInventoryItem_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(new PartBuilder(this.Session)
                            .WithPartId("1")
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new NonSerialisedInventoryItemStates(this.Session).Good, item.NonSerialisedInventoryItemState);
            Assert.Equal(item.LastNonSerialisedInventoryItemState, item.NonSerialisedInventoryItemState);
        }

        [Fact]
        public void GivenInventoryItem_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(new PartBuilder(this.Session)
                            .WithPartId("1")
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .Build())
                .Build();

            this.Session.Derive();

            Assert.Null(item.PreviousNonSerialisedInventoryItemState);
        }

        [Fact]
        public void GivenInventoryItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(new PartBuilder(this.Session)
                                .WithPartId("1")
                                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                                .Build())
                .Build();

            Session.Derive();

            Assert.Equal(0M, item.AvailableToPromise);
            Assert.Equal(0M, item.QuantityCommittedOut);
            Assert.Equal(0M, item.QuantityExpectedIn);
            Assert.Equal(0M, item.QuantityOnHand);
            Assert.Equal(new NonSerialisedInventoryItemStates(this.Session).Good, item.NonSerialisedInventoryItemState);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), item.Facility);
        }

        [Fact]
        public void GivenInventoryItemForPart_WhenDerived_ThenNameIsPartName()
        {
            var part = new PartBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(part)
                .Build();

            this.Session.Derive();

            Assert.Equal(part.Name, item.Name);
        }

        [Fact]
        public void GivenInventoryItemForPart_WhenDerived_ThenUnitOfMeasureIsPartUnitOfMeasure()
        {
            var uom = new UnitsOfMeasure(this.Session).Centimeter;
            var part = new PartBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(uom)
                .Build();

            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(part)
                .Build();

            this.Session.Derive();

            Assert.Equal(part.UnitOfMeasure, item.UnitOfMeasure);
        }

        [Fact]
        public void GivenInventoryItem_WhenQuantityOnHandIsRaised_ThenSalesOrderItemsWithQuantityShortFalledAreUpdated()
        {
            // Arrange
            var inventoryItemKinds = new InventoryItemKinds(this.Session);
            var unitsOfMeasure = new UnitsOfMeasure(this.Session);
            var varianceReasons = new VarianceReasons(this.Session);
            var contactMechanisms = new ContactMechanismPurposes(this.Session);

            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = CreatePart("1", inventoryItemKinds.NonSerialised);
            var good = CreateGood("10101", vatRate21, "good1", unitsOfMeasure.Piece, category, finishedGood);
            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(CreateInventoryVariance(5, varianceReasons.Unknown));

            this.Session.Derive(true);

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = CreateShipTo(mechelenAddress, contactMechanisms.ShippingAddress, true);
            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive(true);
            this.Session.Commit();

            var order1 = CreateSalesOrder(customer, customer, DateTime.UtcNow);
            var salesItem1 = CreateSalesOrderItem("item1", good, 10, 15);
            var salesItem2 = CreateSalesOrderItem("item2", good, 20, 15);

            order1.AddSalesOrderItem(salesItem1);
            order1.AddSalesOrderItem(salesItem2);

            var order2 = CreateSalesOrder(customer, customer, DateTime.UtcNow.AddDays(1));
            var salesitem3 = CreateSalesOrderItem("item3", good, 10, 15);
            var salesItem4 = CreateSalesOrderItem("item4", good, 20, 15);

            order2.AddSalesOrderItem(salesitem3);
            order2.AddSalesOrderItem(salesItem4);

            this.Session.Derive(true);
            this.Session.Commit();

            // Act
            order1.Confirm();
            order2.Confirm();

            this.Session.Derive(true);
            this.Session.Commit();

            // Assert
            salesItem1.QuantityRequestsShipping.ShouldEqual(0);
            salesItem1.QuantityPendingShipment.ShouldEqual(5);
            salesItem1.QuantityReserved.ShouldEqual(10);
            salesItem1.QuantityShortFalled.ShouldEqual(5);

            salesItem2.QuantityRequestsShipping.ShouldEqual(0);
            salesItem2.QuantityPendingShipment.ShouldEqual(0);
            salesItem2.QuantityReserved.ShouldEqual(20);
            salesItem2.QuantityShortFalled.ShouldEqual(20);

            salesitem3.QuantityRequestsShipping.ShouldEqual(0);
            salesitem3.QuantityPendingShipment.ShouldEqual(0);
            salesitem3.QuantityReserved.ShouldEqual(10);
            salesitem3.QuantityShortFalled.ShouldEqual(10);

            salesItem4.QuantityRequestsShipping.ShouldEqual(0);
            salesItem4.QuantityPendingShipment.ShouldEqual(0);
            salesItem4.QuantityReserved.ShouldEqual(20);
            salesItem4.QuantityShortFalled.ShouldEqual(20);

            salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise.ShouldEqual(0);
            salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand.ShouldEqual(5);

            // Re-arrange
            inventoryItem.AddInventoryItemVariance(CreateInventoryVariance(15, varianceReasons.Unknown));
            
            // Act
            this.Session.Derive(true);
            this.Session.Commit();

            // Assert
            // Orderitems are sorted as follows: item1, item2, item3, item4
            salesItem1.QuantityRequestsShipping.ShouldEqual(0);
            salesItem1.QuantityPendingShipment.ShouldEqual(10);
            salesItem1.QuantityReserved.ShouldEqual(10);
            salesItem1.QuantityShortFalled.ShouldEqual(0);

            salesItem2.QuantityRequestsShipping.ShouldEqual(0);
            salesItem2.QuantityPendingShipment.ShouldEqual(10);
            salesItem2.QuantityReserved.ShouldEqual(20);
            salesItem2.QuantityShortFalled.ShouldEqual(10);

            salesitem3.QuantityRequestsShipping.ShouldEqual(0);
            salesitem3.QuantityPendingShipment.ShouldEqual(0);
            salesitem3.QuantityReserved.ShouldEqual(10);
            salesitem3.QuantityShortFalled.ShouldEqual(10);

            salesItem4.QuantityRequestsShipping.ShouldEqual(0);
            salesItem4.QuantityPendingShipment.ShouldEqual(0);
            salesItem4.QuantityReserved.ShouldEqual(20);
            salesItem4.QuantityShortFalled.ShouldEqual(20);

            salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise.ShouldEqual(0);
            salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand.ShouldEqual(20);

            // Re-arrange
            inventoryItem.AddInventoryItemVariance(CreateInventoryVariance(85, varianceReasons.Unknown));
            
            // Act
            this.Session.Derive();
            this.Session.Commit();

            // Assert
            // Orderitems are sorted as follows: item2, item1, item4, item 3
            salesItem1.QuantityRequestsShipping.ShouldEqual(0);
            salesItem1.QuantityPendingShipment.ShouldEqual(10);
            salesItem1.QuantityReserved.ShouldEqual(10);
            salesItem1.QuantityShortFalled.ShouldEqual(0);

            salesItem2.QuantityRequestsShipping.ShouldEqual(0);
            salesItem2.QuantityPendingShipment.ShouldEqual(20);
            salesItem2.QuantityReserved.ShouldEqual(20);
            salesItem2.QuantityShortFalled.ShouldEqual(0);

            salesitem3.QuantityRequestsShipping.ShouldEqual(0);
            salesitem3.QuantityPendingShipment.ShouldEqual(10);
            salesitem3.QuantityReserved.ShouldEqual(10);
            salesitem3.QuantityShortFalled.ShouldEqual(0);

            salesItem4.QuantityRequestsShipping.ShouldEqual(0);
            salesItem4.QuantityPendingShipment.ShouldEqual(20);
            salesItem4.QuantityReserved.ShouldEqual(20);
            salesItem4.QuantityShortFalled.ShouldEqual(0);

            salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise.ShouldEqual(45);
            salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand.ShouldEqual(105);
        }

        [Fact]
        public void GivenInventoryItem_WhenQuantityOnHandIsDecreased_ThenSalesOrderItemsWithQuantityRequestsShippingAreUpdated()
        {
            // Arrange
            var inventoryItemKinds = new InventoryItemKinds(this.Session);
            var unitsOfMeasure = new UnitsOfMeasure(this.Session);
            var varianceReasons = new VarianceReasons(this.Session);
            var contactMechanisms = new ContactMechanismPurposes(this.Session);

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = CreatePart("1", inventoryItemKinds.NonSerialised);
            var good = CreateGood("10101", vatRate21, "good1", unitsOfMeasure.Piece, category, finishedGood);
            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(CreateInventoryVariance(5, varianceReasons.Ruined));  //TODO: Ruined available to ship?

            this.Session.Derive(true);

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = CreateShipTo(mechelenAddress, contactMechanisms.ShippingAddress, true);
            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();
            
            this.Session.Derive(true);

            var order = CreateSalesOrder(customer, customer, DateTime.UtcNow, false);
            var salesItem = CreateSalesOrderItem("item1", good, 10, 15);

            // Act
            order.AddSalesOrderItem(salesItem);
            this.Session.Derive(true);

            order.Confirm();
            this.Session.Derive(true);

            // Assert
            salesItem.QuantityRequestsShipping.ShouldEqual(5);
            salesItem.QuantityPendingShipment.ShouldEqual(0);
            salesItem.QuantityReserved.ShouldEqual(10);
            salesItem.QuantityShortFalled.ShouldEqual(5);
            
            // Rearrange
            inventoryItem.AddInventoryItemVariance(CreateInventoryVariance(-2, varianceReasons.Unknown));

            // Act
            this.Session.Derive();

            // Assert
            salesItem.QuantityRequestsShipping.ShouldEqual(3);
            salesItem.QuantityPendingShipment.ShouldEqual(0);
            salesItem.QuantityReserved.ShouldEqual(10);
            salesItem.QuantityShortFalled.ShouldEqual(7);
        }

        //[Fact]
        //public void ReportNonSerialisedInventory()
        //{
        //    var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
        //    var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

        //    new SupplierRelationshipBuilder(this.DatabaseSession)
        //        .WithSingleton(internalOrganisation)
        //        .WithSupplier(supplier)
        //        .WithFromDate(DateTime.UtcNow)
        //        .Build();

        //    var rawMaterial = new RawMaterialBuilder(this.DatabaseSession)
        //        .WithName("raw material")
        //        .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
        //        .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
        //        .Build();

        //    var level1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("level1").Build();
        //    var level2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("level2").WithParent(level1).Build();
        //    var level3 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("level3").WithParent(level2).Build();
        //    var category = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

        //    var good = new GoodBuilder(this.DatabaseSession)
        //        .WithName("Good")
        //        .WithSku("10101")
        //        .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
        //        .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
        //        .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
        //        .WithProductCategory(level3)
        //        .WithProductCategory(category)
        //        .Build();

        //    var purchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
        //        .WithFromDate(DateTime.UtcNow)
        //        .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
        //        .WithPrice(1)
        //        .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
        //        .Build();

        //    var goodItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession)
        //        .WithGood(good)
        //        .WithAvailableToPromise(120)
        //        .WithQuantityOnHand(120)
        //        .Build();

        //    var damagedItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession)
        //        .WithGood(good)
        //        .WithAvailableToPromise(120)
        //        .WithQuantityOnHand(120)
        //        .WithCurrentObjectState(new NonSerialisedInventoryItemStates(this.DatabaseSession).SlightlyDamaged)
        //        .Build();

        //    var partItem = (NonSerialisedInventoryItem)rawMaterial.InventoryItemsWherePart[0];

        //    new SupplierOfferingBuilder(this.DatabaseSession)
        //        .WithProduct(good)
        //        .WithPart(rawMaterial)
        //        .WithSupplier(supplier)
        //        .WithProductPurchasePrice(purchasePrice)
        //        .Build();

        //var valueByParameter = new Dictionary<Predicate, object>();

        //var preparedExtent = new Reports(this.DatabaseSession).FindByName(Constants.REPORTNONSERIALIZEDINVENTORY).PreparedExtent;
        //var parameters = preparedExtent.Parameters;

        //var extent = preparedExtent.Execute(valueByParameter);

        //Assert.Equal(3, extent.Count);
        //Assert.Contains(goodItem, extent);
        //Assert.Contains(damagedItem, extent);
        //Assert.Contains(partItem, extent);

        //valueByParameter[parameters[1]] = new NonSerialisedInventoryItemStates(this.DatabaseSession).SlightlyDamaged;

        //extent = preparedExtent.Execute(valueByParameter);

        //Assert.Equal(1, extent.Count);
        //Assert.Contains(damagedItem, extent);

        //valueByParameter.Clear();
        //valueByParameter[parameters[4]] = level1;

        //extent = preparedExtent.Execute(valueByParameter);

        //Assert.Equal(2, extent.Count);
        //Assert.Contains(goodItem, extent);
        //Assert.Contains(damagedItem, extent);
        //}

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

        private PartyContactMechanism CreateShipTo(ContactMechanism mechanism, ContactMechanismPurpose purpose, bool isDefault)
            => new PartyContactMechanismBuilder(this.Session).WithContactMechanism(mechanism).WithContactPurpose(purpose).WithUseAsDefault(isDefault).Build();

        private SalesOrder CreateSalesOrder(Person billTo, Person shipTo, DateTime deliveryDate)
            => new SalesOrderBuilder(this.Session).WithBillToCustomer(billTo).WithShipToCustomer(shipTo).WithDeliveryDate(deliveryDate).Build();

        private SalesOrder CreateSalesOrder(Person billTo, Person shipTo, DateTime deliveryDate, bool partialShip)
            => new SalesOrderBuilder(this.Session).WithBillToCustomer(billTo).WithShipToCustomer(shipTo).WithDeliveryDate(deliveryDate).WithPartiallyShip(partialShip).Build();

        private SalesOrderItem CreateSalesOrderItem(string description, Product product, decimal quantityOrdered, decimal unitPrice)
            => new SalesOrderItemBuilder(this.Session)
                .WithDescription(description)
                .WithProduct(product)
                .WithQuantityOrdered(quantityOrdered)
                .WithActualUnitPrice(unitPrice)
                .Build();

        private InventoryItemVariance CreateInventoryVariance(int quantity, VarianceReason reason)
            => new InventoryItemVarianceBuilder(this.Session).WithQuantity(quantity).WithReason(reason).Build();
    }
}

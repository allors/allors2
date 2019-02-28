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

    public class NonSerialisedInventoryItemTests : DomainTest
    {
        [Fact]
        public void GivenInventoryItem_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("1")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
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
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                            .WithProductIdentification(new PartNumberBuilder(this.Session)
                                .WithIdentification("1")
                                .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
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
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                                .WithProductIdentification(new PartNumberBuilder(this.Session)
                                    .WithIdentification("1")
                                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
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
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
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
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
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
            var varianceReasons = new InventoryTransactionReasons(this.Session);
            var contactMechanisms = new ContactMechanismPurposes(this.Session);

            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = CreatePart("1", inventoryItemKinds.NonSerialised);
            var good = CreateGood("10101", vatRate21, "good1", unitsOfMeasure.Piece, category, finishedGood);
            CreateInventoryTransaction(5, varianceReasons.Unknown, finishedGood);

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
            Assert.Equal(0, salesItem1.QuantityRequestsShipping);
            Assert.Equal(5, salesItem1.QuantityPendingShipment);
            Assert.Equal(10, salesItem1.QuantityReserved);
            Assert.Equal(5, salesItem1.QuantityShortFalled);

            Assert.Equal(0, salesItem2.QuantityRequestsShipping);
            Assert.Equal(0, salesItem2.QuantityPendingShipment);
            Assert.Equal(20, salesItem2.QuantityReserved);
            Assert.Equal(20, salesItem2.QuantityShortFalled);

            Assert.Equal(0, salesitem3.QuantityRequestsShipping);
            Assert.Equal(0, salesitem3.QuantityPendingShipment);
            Assert.Equal(10, salesitem3.QuantityReserved);
            Assert.Equal(10, salesitem3.QuantityShortFalled);

            Assert.Equal(0, salesItem4.QuantityRequestsShipping);
            Assert.Equal(0, salesItem4.QuantityPendingShipment);
            Assert.Equal(20, salesItem4.QuantityReserved);
            Assert.Equal(20, salesItem4.QuantityShortFalled);

            Assert.Equal(0, salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(5, salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            // Re-arrange
            CreateInventoryTransaction(15, varianceReasons.Unknown, finishedGood);
            
            // Act
            this.Session.Derive(true);
            this.Session.Commit();

            // Assert
            // Orderitems are sorted as follows: item1, item2, item3, item4
            Assert.Equal(0, salesItem1.QuantityRequestsShipping);
            Assert.Equal(10, salesItem1.QuantityPendingShipment);
            Assert.Equal(10, salesItem1.QuantityReserved);
            Assert.Equal(0, salesItem1.QuantityShortFalled);

            Assert.Equal(0, salesItem2.QuantityRequestsShipping);
            Assert.Equal(10, salesItem2.QuantityPendingShipment);
            Assert.Equal(20, salesItem2.QuantityReserved);
            Assert.Equal(10, salesItem2.QuantityShortFalled);

            Assert.Equal(0, salesitem3.QuantityRequestsShipping);
            Assert.Equal(0, salesitem3.QuantityPendingShipment);
            Assert.Equal(10, salesitem3.QuantityReserved);
            Assert.Equal(10, salesitem3.QuantityShortFalled);

            Assert.Equal(0, salesItem4.QuantityRequestsShipping);
            Assert.Equal(0, salesItem4.QuantityPendingShipment);
            Assert.Equal(20, salesItem4.QuantityReserved);
            Assert.Equal(20, salesItem4.QuantityShortFalled);

            Assert.Equal(0, salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(20, salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            // Re-arrange
            CreateInventoryTransaction(85, varianceReasons.Unknown, finishedGood);
            
            // Act
            this.Session.Derive();
            this.Session.Commit();

            // Assert
            // Orderitems are sorted as follows: item2, item1, item4, item 3
            Assert.Equal(0, salesItem1.QuantityRequestsShipping);
            Assert.Equal(10, salesItem1.QuantityPendingShipment);
            Assert.Equal(10, salesItem1.QuantityReserved);
            Assert.Equal(0, salesItem1.QuantityShortFalled);

            Assert.Equal(0, salesItem2.QuantityRequestsShipping);
            Assert.Equal(20, salesItem2.QuantityPendingShipment);
            Assert.Equal(20, salesItem2.QuantityReserved);
            Assert.Equal(0, salesItem2.QuantityShortFalled);

            Assert.Equal(0, salesitem3.QuantityRequestsShipping);
            Assert.Equal(10, salesitem3.QuantityPendingShipment);
            Assert.Equal(10, salesitem3.QuantityReserved);
            Assert.Equal(0, salesitem3.QuantityShortFalled);

            Assert.Equal(0, salesItem4.QuantityRequestsShipping);
            Assert.Equal(20, salesItem4.QuantityPendingShipment);
            Assert.Equal(20, salesItem4.QuantityReserved);
            Assert.Equal(0, salesItem4.QuantityShortFalled);

            Assert.Equal(45, salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(105, salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenInventoryItem_WhenQuantityOnHandIsDecreased_ThenSalesOrderItemsWithQuantityRequestsShippingAreUpdated()
        {
            // Arrange
            var inventoryItemKinds = new InventoryItemKinds(this.Session);
            var unitsOfMeasure = new UnitsOfMeasure(this.Session);
            var varianceReasons = new InventoryTransactionReasons(this.Session);
            var contactMechanisms = new ContactMechanismPurposes(this.Session);

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = CreatePart("1", inventoryItemKinds.NonSerialised);
            var good = CreateGood("10101", vatRate21, "good1", unitsOfMeasure.Piece, category, finishedGood);
            CreateInventoryTransaction(5, varianceReasons.Unknown, finishedGood);

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
            Assert.Equal(5, salesItem.QuantityRequestsShipping);
            Assert.Equal(0, salesItem.QuantityPendingShipment);
            Assert.Equal(10, salesItem.QuantityReserved);
            Assert.Equal(5, salesItem.QuantityShortFalled);
            
            // Rearrange
            CreateInventoryTransaction(-2, varianceReasons.Unknown, finishedGood);

            // Act
            this.Session.Derive();

            // Assert
            Assert.Equal(3, salesItem.QuantityRequestsShipping);
            Assert.Equal(0, salesItem.QuantityPendingShipment);
            Assert.Equal(10, salesItem.QuantityReserved);
            Assert.Equal(7, salesItem.QuantityShortFalled);
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

        //    var good = new NonUnifiedGoodBuilder(this.DatabaseSession)
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
            => new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification(partId)
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(kind).Build();

        private Good CreateGood(string sku, VatRate vatRate, string name, UnitOfMeasure uom, ProductCategory category, Part part)
            => new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new SkuIdentificationBuilder(this.Session)
                    .WithIdentification(sku)
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Sku).Build())
                .WithVatRate(vatRate)
                .WithName(name)
                .WithUnitOfMeasure(uom)
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
                .WithAssignedUnitPrice(unitPrice)
                .Build();

        private InventoryItemTransaction CreateInventoryTransaction(int quantity, InventoryTransactionReason reason, Part part)
            => new InventoryItemTransactionBuilder(this.Session).WithQuantity(quantity).WithReason(reason).WithPart(part).Build();
    }
}

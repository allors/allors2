//------------------------------------------------------------------------------------------------- 
// <copyright file="NonSerializedInventoryItemTests.cs" company="Allors bvba">
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
    public class NonSerializedInventoryItemTests : DomainTest
    {
        [Test]
        public void GivenInventoryItem_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var item = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
                .WithPart(new FinishedGoodBuilder(this.DatabaseSession).WithName("part").WithManufacturerId("10101").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new NonSerializedInventoryItemObjectStates(this.DatabaseSession).Good, item.CurrentObjectState);
            Assert.AreEqual(item.LastObjectState, item.CurrentObjectState);
        }

        [Test]
        public void GivenInventoryItem_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var item = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
                .WithPart(new FinishedGoodBuilder(this.DatabaseSession).WithName("part").WithManufacturerId("10101").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsNull(item.PreviousObjectState);
        }

        [Test]
        public void GivenInventoryItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new NonSerializedInventoryItemBuilder(this.DatabaseSession);
            var item = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPart(new FinishedGoodBuilder(this.DatabaseSession).WithName("part").WithManufacturerId("10101").Build());
            item = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            builder.WithGood(good);
            item = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            item.RemovePart();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenInventoryItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var item = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
                .WithPart(new FinishedGoodBuilder(this.DatabaseSession).WithName("part").WithManufacturerId("10101").Build())
                .Build();

            Assert.IsNotNull(item.AvailableToPromise);
            Assert.IsNotNull(item.QuantityCommittedOut);
            Assert.IsNotNull(item.QuantityExpectedIn);
            Assert.IsNotNull(item.QuantityOnHand);
            Assert.AreEqual(new NonSerializedInventoryItemObjectStates(this.DatabaseSession).Good, item.CurrentObjectState);
            Assert.AreEqual(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"), item.Facility);
        }

        [Test]
        public void GivenInventoryItemForPart_WhenDerived_ThenSkuIsEmpty()
        {
            var item = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
                .WithPart(new FinishedGoodBuilder(this.DatabaseSession).WithName("part").WithManufacturerId("10101").Build())
                .Build();

            Assert.IsNullOrEmpty(item.Sku);
        }

        [Test]
        public void GivenInventoryItemForGood_WhenDerived_ThenSkuIsFromGood()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var category = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var good = new GoodBuilder(this.DatabaseSession)
             .WithSku("10101")
             .WithVatRate(vatRate21)
             .WithName("good1")
             .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
             .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
             .WithPrimaryProductCategory(category)
             .Build();

            var item = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
                .WithGood(good)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(good.Sku, item.Sku);
        }

        [Test]
        public void GivenInventoryItemForPart_WhenDerived_ThenNameIsPartName()
        {
            var part = new FinishedGoodBuilder(this.DatabaseSession).WithName("part").WithManufacturerId("10101").Build();
            var item = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
                .WithPart(part)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(part.Name, item.Name);
        }

        [Test]
        public void GivenInventoryItemForGood_WhenDerived_ThenNameIsGoodName()
        {

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var category = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var good = new GoodBuilder(this.DatabaseSession)
             .WithSku("10101")
             .WithVatRate(vatRate21)
             .WithName("good1")
             .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
             .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
             .WithPrimaryProductCategory(category)
             .Build();

            var item = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
                .WithGood(good)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(good.Name, item.Name);
        }

        [Test]
        public void GivenInventoryItemForPart_WhenDerived_ThenUnitOfMeasureIsPartUnitOfMeasure()
        {
            var uom = new UnitsOfMeasure(this.DatabaseSession).Centimeter;
            var part = new FinishedGoodBuilder(this.DatabaseSession).WithName("part").WithManufacturerId("10101").WithUnitOfMeasure(uom).Build();
            var item = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
                .WithPart(part)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(part.UnitOfMeasure, item.UnitOfMeasure);
        }

        [Test]
        public void GivenInventoryItemForGood_WhenDerived_ThenUnitOfMeasureIsGoodUnitOfMeasure()
        {
            var uom = new UnitsOfMeasure(this.DatabaseSession).Centimeter;

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();
            var good = new GoodBuilder(this.DatabaseSession)
             .WithSku("10101")
             .WithVatRate(vatRate21)
             .WithName("good1")
             .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
             .WithUnitOfMeasure(uom)
             .WithPrimaryProductCategory(category)
             .Build();
            
            var item = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
                .WithGood(good)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(good.UnitOfMeasure, item.UnitOfMeasure);
        }

        [Test]
        public void GivenInventoryItem_WhenQuantityOnHandIsRaised_ThenSalesOrderItemsWithQuantityShortFalledAreUpdated()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var category = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var good = new GoodBuilder(this.DatabaseSession)
             .WithSku("10101")
             .WithVatRate(vatRate21)
             .WithName("good1")
             .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
             .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
             .WithPrimaryProductCategory(category)
             .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(5).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithDeliveryDate(DateTime.UtcNow)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithDescription("item1").WithProduct(good).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithDescription("item2").WithProduct(good).WithQuantityOrdered(20).WithActualUnitPrice(15).Build();
            order1.AddSalesOrderItem(item1);
            order1.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive(true); 
            
            order1.Confirm();

            this.DatabaseSession.Derive(true);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithDeliveryDate(DateTime.UtcNow.AddDays(1))
                .Build();

            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithDescription("item3").WithProduct(good).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithDescription("item4").WithProduct(good).WithQuantityOrdered(20).WithActualUnitPrice(15).Build();
            order2.AddSalesOrderItem(item3);
            order2.AddSalesOrderItem(item4);

            this.DatabaseSession.Derive(true);

            order2.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, item1.QuantityRequestsShipping);
            Assert.AreEqual(5, item1.QuantityPendingShipment);
            Assert.AreEqual(10, item1.QuantityReserved);
            Assert.AreEqual(5, item1.QuantityShortFalled);

            Assert.AreEqual(0, item2.QuantityRequestsShipping);
            Assert.AreEqual(0, item2.QuantityPendingShipment);
            Assert.AreEqual(20, item2.QuantityReserved);
            Assert.AreEqual(20, item2.QuantityShortFalled);

            Assert.AreEqual(0, item3.QuantityRequestsShipping);                
            Assert.AreEqual(0, item3.QuantityPendingShipment);
            Assert.AreEqual(10, item3.QuantityReserved);
            Assert.AreEqual(10, item3.QuantityShortFalled);
                
            Assert.AreEqual(0, item4.QuantityRequestsShipping);
            Assert.AreEqual(0, item4.QuantityPendingShipment);
            Assert.AreEqual(20, item4.QuantityReserved);
            Assert.AreEqual(20, item4.QuantityShortFalled);
                
            Assert.AreEqual(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.AreEqual(5, item1.ReservedFromInventoryItem.QuantityOnHand);

            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(15).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            //// Orderitems are sorted as follows: item1, item2, item3, item4
            Assert.AreEqual(0, item1.QuantityRequestsShipping);
            Assert.AreEqual(10, item1.QuantityPendingShipment);
            Assert.AreEqual(10, item1.QuantityReserved);
            Assert.AreEqual(0, item1.QuantityShortFalled);
                
            Assert.AreEqual(0, item2.QuantityRequestsShipping);
            Assert.AreEqual(10, item2.QuantityPendingShipment);
            Assert.AreEqual(20, item2.QuantityReserved);
            Assert.AreEqual(10, item2.QuantityShortFalled);
                
            Assert.AreEqual(0, item3.QuantityRequestsShipping);
            Assert.AreEqual(0, item3.QuantityPendingShipment);
            Assert.AreEqual(10, item3.QuantityReserved);
            Assert.AreEqual(10, item3.QuantityShortFalled);
                
            Assert.AreEqual(0, item4.QuantityRequestsShipping);
            Assert.AreEqual(0, item4.QuantityPendingShipment);
            Assert.AreEqual(20, item4.QuantityReserved);
            Assert.AreEqual(20, item4.QuantityShortFalled);
                
            Assert.AreEqual(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.AreEqual(20, item1.ReservedFromInventoryItem.QuantityOnHand);

            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(85).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            //// Orderitems are sorted as follows: item2, item1, item4, item 3
            Assert.AreEqual(0, item1.QuantityRequestsShipping);
            Assert.AreEqual(10, item1.QuantityPendingShipment);
            Assert.AreEqual(10, item1.QuantityReserved);
            Assert.AreEqual(0, item1.QuantityShortFalled);
                
            Assert.AreEqual(0, item2.QuantityRequestsShipping);
            Assert.AreEqual(20, item2.QuantityPendingShipment);
            Assert.AreEqual(20, item2.QuantityReserved);
            Assert.AreEqual(0, item2.QuantityShortFalled);
                
            Assert.AreEqual(0, item3.QuantityRequestsShipping);
            Assert.AreEqual(10, item3.QuantityPendingShipment);
            Assert.AreEqual(10, item3.QuantityReserved);
            Assert.AreEqual(0, item3.QuantityShortFalled);
                
            Assert.AreEqual(0, item4.QuantityRequestsShipping);
            Assert.AreEqual(20, item4.QuantityPendingShipment);
            Assert.AreEqual(20, item4.QuantityReserved);
            Assert.AreEqual(0, item4.QuantityShortFalled);
                
            Assert.AreEqual(45, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.AreEqual(105, item1.ReservedFromInventoryItem.QuantityOnHand);
        }

        [Test]
        public void GivenInventoryItem_WhenQuantityOnHandIsDecreased_ThenSalesOrderItemsWithQuantityRequestsShippingAreUpdated()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var category = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

            var good = new GoodBuilder(this.DatabaseSession)
             .WithSku("10101")
             .WithVatRate(vatRate21)
             .WithName("good1")
             .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
             .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
             .WithPrimaryProductCategory(category)
             .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(5).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithDeliveryDate(DateTime.UtcNow)
                .WithPartiallyShip(false)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithDescription("item1").WithProduct(good).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            
            this.DatabaseSession.Derive(true);
            
            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(5, item1.QuantityRequestsShipping);
            Assert.AreEqual(0, item1.QuantityPendingShipment);
            Assert.AreEqual(10, item1.QuantityReserved);
            Assert.AreEqual(5, item1.QuantityShortFalled);

            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(-2).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, item1.QuantityRequestsShipping);
            Assert.AreEqual(0, item1.QuantityPendingShipment);
            Assert.AreEqual(10, item1.QuantityReserved);
            Assert.AreEqual(7, item1.QuantityShortFalled);
        }
        
        //[Test]
        //public void ReportNonSerializedInventory()
        //{
        //    var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
        //    var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

        //    new SupplierRelationshipBuilder(this.DatabaseSession)
        //        .WithInternalOrganisation(internalOrganisation)
        //        .WithSupplier(supplier)
        //        .WithFromDate(DateTime.UtcNow)
        //        .Build();

        //    var rawMaterial = new RawMaterialBuilder(this.DatabaseSession)
        //        .WithName("raw material")
        //        .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
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
        //        .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
        //        .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
        //        .WithProductCategory(level3)
        //        .WithProductCategory(category)
        //        .Build();

        //    var purchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
        //        .WithFromDate(DateTime.UtcNow)
        //        .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
        //        .WithPrice(1)
        //        .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
        //        .Build();

        //    var goodItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
        //        .WithGood(good)
        //        .WithAvailableToPromise(120)
        //        .WithQuantityOnHand(120)
        //        .Build();

        //    var damagedItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession)
        //        .WithGood(good)
        //        .WithAvailableToPromise(120)
        //        .WithQuantityOnHand(120)
        //        .WithCurrentObjectState(new NonSerializedInventoryItemObjectStates(this.DatabaseSession).SlightlyDamaged)
        //        .Build();

        //    var partItem = (NonSerializedInventoryItem)rawMaterial.InventoryItemsWherePart[0];

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

            //Assert.AreEqual(3, extent.Count);
            //Assert.Contains(goodItem, extent);
            //Assert.Contains(damagedItem, extent);
            //Assert.Contains(partItem, extent);

            //valueByParameter[parameters[1]] = new NonSerializedInventoryItemObjectStates(this.DatabaseSession).SlightlyDamaged;

            //extent = preparedExtent.Execute(valueByParameter);

            //Assert.AreEqual(1, extent.Count);
            //Assert.Contains(damagedItem, extent);

            //valueByParameter.Clear();
            //valueByParameter[parameters[4]] = level1;

            //extent = preparedExtent.Execute(valueByParameter);

            //Assert.AreEqual(2, extent.Count);
            //Assert.Contains(goodItem, extent);
            //Assert.Contains(damagedItem, extent);
        //}
    }
}

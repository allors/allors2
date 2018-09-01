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
                .WithPart(new FinishedGoodBuilder(this.Session)
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
                .WithPart(new FinishedGoodBuilder(this.Session)
                            .WithPartId("1")
                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                            .Build())
                .Build();

            this.Session.Derive();

            Assert.Null(item.PreviousNonSerialisedInventoryItemState);
        }

        [Fact]
        public void GivenInventoryItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new NonSerialisedInventoryItemBuilder(this.Session);
            var item = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPart(new FinishedGoodBuilder(this.Session)
                    .WithPartId("2")
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                    .Build());
            item = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithPart(finishedGood);
            item = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            item.RemovePart();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenInventoryItem_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(new FinishedGoodBuilder(this.Session)
                                .WithPartId("1")
                                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                                .Build())
                .Build();

            Session.Derive();

            Assert.NotNull(item.AvailableToPromise);
            Assert.NotNull(item.QuantityCommittedOut);
            Assert.NotNull(item.QuantityExpectedIn);
            Assert.NotNull(item.QuantityOnHand);
            Assert.Equal(new NonSerialisedInventoryItemStates(this.Session).Good, item.NonSerialisedInventoryItemState);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), item.Facility);
        }

        [Fact]
        public void GivenInventoryItemForPart_WhenDerived_ThenNameIsPartName()
        {
            var part = new FinishedGoodBuilder(this.Session)
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
        public void GivenInventoryItemForGood_WhenDerived_ThenNameIsGoodName()
        {

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var category = new ProductCategoryBuilder(this.Session)
                .WithName("category")
                .Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(category)
                .WithFinishedGood(finishedGood)
                .Build();

            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(finishedGood)
                .Build();

            this.Session.Derive();

            Assert.Equal(good.Name, item.Name);
        }

        [Fact]
        public void GivenInventoryItemForPart_WhenDerived_ThenUnitOfMeasureIsPartUnitOfMeasure()
        {
            var uom = new UnitsOfMeasure(this.Session).Centimeter;
            var part = new FinishedGoodBuilder(this.Session)
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
        public void GivenInventoryItemForGood_WhenDerived_ThenUnitOfMeasureIsGoodUnitOfMeasure()
        {
            var uom = new UnitsOfMeasure(this.Session).Centimeter;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var category = new ProductCategoryBuilder(this.Session)
                .WithName("category")
                .Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(uom)
                .WithPrimaryProductCategory(category)
                .WithFinishedGood(finishedGood)
                .Build();  

            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(finishedGood)
                .Build();

            this.Session.Derive();

            Assert.Equal(good.UnitOfMeasure, item.UnitOfMeasure);
        }

        [Fact]
        public void GivenInventoryItem_WhenQuantityOnHandIsRaised_ThenSalesOrderItemsWithQuantityShortFalledAreUpdated()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var category = new ProductCategoryBuilder(this.Session)
                .WithName("category")
                .Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(category)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(5).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();
            this.Session.Commit();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithDeliveryDate(DateTime.UtcNow)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithDescription("item1").WithProduct(good).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithDescription("item2").WithProduct(good).WithQuantityOrdered(20).WithActualUnitPrice(15).Build();
            order1.AddSalesOrderItem(item1);
            order1.AddSalesOrderItem(item2);

            this.Session.Derive();
            this.Session.Commit();

            order1.Confirm();

            this.Session.Derive();

            var order2 = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithDeliveryDate(DateTime.UtcNow.AddDays(1))
                .Build();

            var item3 = new SalesOrderItemBuilder(this.Session).WithDescription("item3").WithProduct(good).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithDescription("item4").WithProduct(good).WithQuantityOrdered(20).WithActualUnitPrice(15).Build();
            order2.AddSalesOrderItem(item3);
            order2.AddSalesOrderItem(item4);

            this.Session.Derive();
            this.Session.Commit();

            order2.Confirm();

            this.Session.Derive();

            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(5, item1.QuantityPendingShipment);
            Assert.Equal(10, item1.QuantityReserved);
            Assert.Equal(5, item1.QuantityShortFalled);

            Assert.Equal(0, item2.QuantityRequestsShipping);
            Assert.Equal(0, item2.QuantityPendingShipment);
            Assert.Equal(20, item2.QuantityReserved);
            Assert.Equal(20, item2.QuantityShortFalled);

            Assert.Equal(0, item3.QuantityRequestsShipping);
            Assert.Equal(0, item3.QuantityPendingShipment);
            Assert.Equal(10, item3.QuantityReserved);
            Assert.Equal(10, item3.QuantityShortFalled);

            Assert.Equal(0, item4.QuantityRequestsShipping);
            Assert.Equal(0, item4.QuantityPendingShipment);
            Assert.Equal(20, item4.QuantityReserved);
            Assert.Equal(20, item4.QuantityShortFalled);

            Assert.Equal(0, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(5, item1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(15).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();
            this.Session.Commit();

            // Orderitems are sorted as follows: item1, item2, item3, item4
            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(10, item1.QuantityPendingShipment);
            Assert.Equal(10, item1.QuantityReserved);
            Assert.Equal(0, item1.QuantityShortFalled);

            Assert.Equal(0, item2.QuantityRequestsShipping);
            Assert.Equal(10, item2.QuantityPendingShipment);
            Assert.Equal(20, item2.QuantityReserved);
            Assert.Equal(10, item2.QuantityShortFalled);

            Assert.Equal(0, item3.QuantityRequestsShipping);
            Assert.Equal(0, item3.QuantityPendingShipment);
            Assert.Equal(10, item3.QuantityReserved);
            Assert.Equal(10, item3.QuantityShortFalled);

            Assert.Equal(0, item4.QuantityRequestsShipping);
            Assert.Equal(0, item4.QuantityPendingShipment);
            Assert.Equal(20, item4.QuantityReserved);
            Assert.Equal(20, item4.QuantityShortFalled);

            Assert.Equal(0, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(20, item1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(85).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();
            this.Session.Commit();

            //// Orderitems are sorted as follows: item2, item1, item4, item 3
            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(10, item1.QuantityPendingShipment);
            Assert.Equal(10, item1.QuantityReserved);
            Assert.Equal(0, item1.QuantityShortFalled);

            Assert.Equal(0, item2.QuantityRequestsShipping);
            Assert.Equal(20, item2.QuantityPendingShipment);
            Assert.Equal(20, item2.QuantityReserved);
            Assert.Equal(0, item2.QuantityShortFalled);

            Assert.Equal(0, item3.QuantityRequestsShipping);
            Assert.Equal(10, item3.QuantityPendingShipment);
            Assert.Equal(10, item3.QuantityReserved);
            Assert.Equal(0, item3.QuantityShortFalled);

            Assert.Equal(0, item4.QuantityRequestsShipping);
            Assert.Equal(20, item4.QuantityPendingShipment);
            Assert.Equal(20, item4.QuantityReserved);
            Assert.Equal(0, item4.QuantityShortFalled);

            Assert.Equal(45, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(105, item1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenInventoryItem_WhenQuantityOnHandIsDecreased_ThenSalesOrderItemsWithQuantityRequestsShippingAreUpdated()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var category = new ProductCategoryBuilder(this.Session)
                .WithName("category")
                .Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(category)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(5).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            var internalOrganisation = this.InternalOrganisation;


            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithDeliveryDate(DateTime.UtcNow)
                .WithPartiallyShip(false)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithDescription("item1").WithProduct(good).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(5, item1.QuantityRequestsShipping);
            Assert.Equal(0, item1.QuantityPendingShipment);
            Assert.Equal(10, item1.QuantityReserved);
            Assert.Equal(5, item1.QuantityShortFalled);

            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(-2).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            Assert.Equal(3, item1.QuantityRequestsShipping);
            Assert.Equal(0, item1.QuantityPendingShipment);
            Assert.Equal(10, item1.QuantityReserved);
            Assert.Equal(7, item1.QuantityShortFalled);
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
    }
}

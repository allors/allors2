// <copyright file="NonSerializedInventoryItemTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

using Allors.Domain.TestPopulation;

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;
    using Xunit;

    public class NonSerialisedInventoryItemTests : DomainTest
    {
        [Fact]
        public void GivenInventoryItem_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.Session.Derive();

            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(part)
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
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.Session.Derive();

            var item = new NonSerialisedInventoryItemBuilder(this.Session)
                .WithPart(part)
                .Build();

            this.Session.Derive();

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

            this.Session.Derive();

            Assert.Equal("Part 1 at facility with state Good", part.InventoryItemsWherePart.Single().Name);
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

            this.Session.Derive();

            Assert.Equal(part.UnitOfMeasure, part.InventoryItemsWherePart.Single().UnitOfMeasure);
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

            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = this.CreatePart("1", inventoryItemKinds.NonSerialised);
            var good = this.CreateGood("10101", vatRegime, "good1", unitsOfMeasure.Piece, category, finishedGood);

            this.Session.Derive();

            this.CreateInventoryTransaction(5, varianceReasons.Unknown, finishedGood);

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = this.CreateShipTo(mechelenAddress, contactMechanisms.ShippingAddress, true);
            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();
            this.Session.Commit();

            var order1 = this.CreateSalesOrder(customer, customer, this.Session.Now());
            var salesItem1 = this.CreateSalesOrderItem("item1", good, 10, 15);
            var salesItem2 = this.CreateSalesOrderItem("item2", good, 20, 15);

            order1.AddSalesOrderItem(salesItem1);
            order1.AddSalesOrderItem(salesItem2);

            var order2 = this.CreateSalesOrder(customer, customer, this.Session.Now().AddDays(1));
            var salesItem3 = this.CreateSalesOrderItem("item3", good, 10, 15);
            var salesItem4 = this.CreateSalesOrderItem("item4", good, 20, 15);

            order2.AddSalesOrderItem(salesItem3);
            order2.AddSalesOrderItem(salesItem4);

            this.Session.Derive();
            this.Session.Commit();

            // Act
            order1.SetReadyForPosting();
            this.Session.Derive(true);

            order1.Post();
            this.Session.Derive();

            order1.Accept();
            this.Session.Derive();

            Assert.Equal(0, salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(5, salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            order2.SetReadyForPosting();

            this.Session.Derive(true);

            order2.Post();
            this.Session.Derive();

            order2.Accept();
            this.Session.Derive();

            // Assert
            Assert.Equal(0, salesItem1.QuantityRequestsShipping);
            Assert.Equal(5, salesItem1.QuantityPendingShipment);
            Assert.Equal(10, salesItem1.QuantityReserved);
            Assert.Equal(5, salesItem1.QuantityShortFalled);

            Assert.Equal(0, salesItem2.QuantityRequestsShipping);
            Assert.Equal(0, salesItem2.QuantityPendingShipment);
            Assert.Equal(20, salesItem2.QuantityReserved);
            Assert.Equal(20, salesItem2.QuantityShortFalled);

            Assert.Equal(0, salesItem3.QuantityRequestsShipping);
            Assert.Equal(0, salesItem3.QuantityPendingShipment);
            Assert.Equal(10, salesItem3.QuantityReserved);
            Assert.Equal(10, salesItem3.QuantityShortFalled);

            Assert.Equal(0, salesItem4.QuantityRequestsShipping);
            Assert.Equal(0, salesItem4.QuantityPendingShipment);
            Assert.Equal(20, salesItem4.QuantityReserved);
            Assert.Equal(20, salesItem4.QuantityShortFalled);

            Assert.Equal(0, salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(5, salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            // Re-arrange
            this.CreateInventoryTransaction(15, varianceReasons.Unknown, finishedGood);

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

            Assert.Equal(0, salesItem3.QuantityRequestsShipping);
            Assert.Equal(0, salesItem3.QuantityPendingShipment);
            Assert.Equal(10, salesItem3.QuantityReserved);
            Assert.Equal(10, salesItem3.QuantityShortFalled);

            Assert.Equal(0, salesItem4.QuantityRequestsShipping);
            Assert.Equal(0, salesItem4.QuantityPendingShipment);
            Assert.Equal(20, salesItem4.QuantityReserved);
            Assert.Equal(20, salesItem4.QuantityShortFalled);

            Assert.Equal(0, salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(20, salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            // Re-arrange
            this.CreateInventoryTransaction(85, varianceReasons.Unknown, finishedGood);

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

            Assert.Equal(0, salesItem3.QuantityRequestsShipping);
            Assert.Equal(10, salesItem3.QuantityPendingShipment);
            Assert.Equal(10, salesItem3.QuantityReserved);
            Assert.Equal(0, salesItem3.QuantityShortFalled);

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

            var vatRegime = new VatRegimes(this.Session).BelgiumStandard;
            var category = new ProductCategoryBuilder(this.Session).WithName("category").Build();
            var finishedGood = this.CreatePart("1", inventoryItemKinds.NonSerialised);
            var good = this.CreateGood("10101", vatRegime, "good1", unitsOfMeasure.Piece, category, finishedGood);

            this.Session.Derive();

            this.CreateInventoryTransaction(5, varianceReasons.Unknown, finishedGood);

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = this.CreateShipTo(mechelenAddress, contactMechanisms.ShippingAddress, true);
            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = this.CreateSalesOrder(customer, customer, this.Session.Now(), false);
            var salesItem = this.CreateSalesOrderItem("item1", good, 10, 15);

            // Act
            order.AddSalesOrderItem(salesItem);
            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            // Assert
            Assert.Equal(5, salesItem.QuantityRequestsShipping);
            Assert.Equal(0, salesItem.QuantityPendingShipment);
            Assert.Equal(10, salesItem.QuantityReserved);
            Assert.Equal(5, salesItem.QuantityShortFalled);

            // Rearrange
            this.CreateInventoryTransaction(-2, varianceReasons.Unknown, finishedGood);

            // Act
            this.Session.Derive();

            // Assert
            Assert.Equal(3, salesItem.QuantityRequestsShipping);
            Assert.Equal(0, salesItem.QuantityPendingShipment);
            Assert.Equal(10, salesItem.QuantityReserved);
            Assert.Equal(7, salesItem.QuantityShortFalled);
        }

        [Fact]
        public void GivenInventoryItemForUnifiedGood_WhenQuantityOnHandIsRaised_ThenSalesOrderItemWithQuantityShortFalledIsUpdated()
        {
            var internalOrganisation = new Organisations(this.Session).Extent().First(v => Equals(v.Name, "internalOrganisation"));
            var unifiedGood = new UnifiedGoodBuilder(this.Session).WithNonSerialisedDefaults(internalOrganisation).Build();
            var customer = internalOrganisation.CreateB2BCustomer(this.Session.Faker());

            this.Session.Derive();

            this.CreateInventoryTransaction(5, new InventoryTransactionReasons(this.Session).Unknown, unifiedGood);

            this.Session.Derive();

            var order = this.CreateSalesOrder(customer, customer, this.Session.Now());
            var salesItem1 = this.CreateSalesOrderItem("item1", unifiedGood, 10, 15);
            order.AddSalesOrderItem(salesItem1);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive(true);

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            Assert.Equal(0, salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(5, salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            Assert.Equal(0, salesItem1.QuantityRequestsShipping);
            Assert.Equal(5, salesItem1.QuantityPendingShipment);
            Assert.Equal(10, salesItem1.QuantityReserved);
            Assert.Equal(5, salesItem1.QuantityShortFalled);

            Assert.Equal(0, salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(5, salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            // Re-arrange
            this.CreateInventoryTransaction(15, new InventoryTransactionReasons(this.Session).Unknown, unifiedGood);

            // Act
            this.Session.Derive(true);

            Assert.Equal(0, salesItem1.QuantityRequestsShipping);
            Assert.Equal(10, salesItem1.QuantityPendingShipment);
            Assert.Equal(10, salesItem1.QuantityReserved);
            Assert.Equal(0, salesItem1.QuantityShortFalled);

            Assert.Equal(10, salesItem1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(20, salesItem1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        // [Fact]
        // public void ReportNonSerialisedInventory()
        // {
        //    var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
        //    var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

        // new SupplierRelationshipBuilder(this.DatabaseSession)
        //        .WithSingleton(internalOrganisation)
        //        .WithSupplier(supplier)
        //        .WithFromDate(this.Session.Now())
        //        .Build();

        // var rawMaterial = new RawMaterialBuilder(this.DatabaseSession)
        //        .WithName("raw material")
        //        .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
        //        .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
        //        .Build();

        // var level1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("level1").Build();
        //    var level2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("level2").WithPrimaryParent(level1).Build();
        //    var level3 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("level3").WithPrimaryParent(level2).Build();
        //    var category = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build();

        // var good = new NonUnifiedGoodBuilder(this.DatabaseSession)
        //        .WithName("Good")
        //        .WithSku("10101")
        //        .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
        //        .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
        //        .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
        //        .WithProductCategory(level3)
        //        .WithProductCategory(category)
        //        .Build();

        // var purchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
        //        .WithFromDate(this.Session.Now())
        //        .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
        //        .WithPrice(1)
        //        .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
        //        .Build();

        // var goodItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession)
        //        .WithGood(good)
        //        .WithAvailableToPromise(120)
        //        .WithQuantityOnHand(120)
        //        .Build();

        // var damagedItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession)
        //        .WithGood(good)
        //        .WithAvailableToPromise(120)
        //        .WithQuantityOnHand(120)
        //        .WithCurrentObjectState(new NonSerialisedInventoryItemStates(this.DatabaseSession).SlightlyDamaged)
        //        .Build();

        // var partItem = (NonSerialisedInventoryItem)rawMaterial.InventoryItemsWherePart[0];

        // new SupplierOfferingBuilder(this.DatabaseSession)
        //        .WithProduct(good)
        //        .WithPart(rawMaterial)
        //        .WithSupplier(supplier)
        //        .WithProductPurchasePrice(purchasePrice)
        //        .Build();

        // var valueByParameter = new Dictionary<Predicate, object>();

        // var preparedExtent = new Reports(this.DatabaseSession).FindByName(Constants.REPORTNONSERIALIZEDINVENTORY).PreparedExtent;
        // var parameters = preparedExtent.Parameters;

        // var extent = preparedExtent.Execute(valueByParameter);

        // Assert.Equal(3, extent.Count);
        // Assert.Contains(goodItem, extent);
        // Assert.Contains(damagedItem, extent);
        // Assert.Contains(partItem, extent);

        // valueByParameter[parameters[1]] = new NonSerialisedInventoryItemStates(this.DatabaseSession).SlightlyDamaged;

        // extent = preparedExtent.Execute(valueByParameter);

        // Assert.Single(extent.Count);
        // Assert.Contains(damagedItem, extent);

        // valueByParameter.Clear();
        // valueByParameter[parameters[4]] = level1;

        // extent = preparedExtent.Execute(valueByParameter);

        // Assert.Equal(2, extent.Count);
        // Assert.Contains(goodItem, extent);
        // Assert.Contains(damagedItem, extent);
        // }
        private Part CreatePart(string partId, InventoryItemKind kind)
            => new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification(partId)
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(kind).Build();

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

        private PartyContactMechanism CreateShipTo(ContactMechanism mechanism, ContactMechanismPurpose purpose, bool isDefault)
            => new PartyContactMechanismBuilder(this.Session).WithContactMechanism(mechanism).WithContactPurpose(purpose).WithUseAsDefault(isDefault).Build();

        private SalesOrder CreateSalesOrder(Party billTo, Party shipTo, DateTime deliveryDate)
            => new SalesOrderBuilder(this.Session).WithBillToCustomer(billTo).WithShipToCustomer(shipTo).WithDeliveryDate(deliveryDate).Build();

        private SalesOrder CreateSalesOrder(Party billTo, Party shipTo, DateTime deliveryDate, bool partialShip)
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

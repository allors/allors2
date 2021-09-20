// <copyright file="PickListTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Xunit;

    public class PickListTests : DomainTest
    {
        [Fact]
        public void GivenPickListBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var pickList = new PickListBuilder(this.Session).Build();

            this.Session.Derive();

            Assert.Equal(new PickListStates(this.Session).Created, pickList.PickListState);
        }

        [Fact]
        public void GivenPickList_WhenPicked_ThenInventoryIsAdjustedAndOrderItemsQuantityPickedIsSet()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen)
                .WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var customer = new PersonBuilder(this.Session).WithLastName("person1")
                .WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good1.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(7)
                .Build();

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good2.Part).Build();

            this.Session.Derive();

            var good1Inventory = (NonSerialisedInventoryItem)good1.Part.InventoryItemsWherePart.First;
            var good2Inventory = (NonSerialisedInventoryItem)good2.Part.InventoryItemsWherePart.First;

            var colorWhite = new ColourBuilder(this.Session)
                .WithName("white")
                .Build();
            var extraLarge = new SizeBuilder(this.Session)
                .WithName("Extra large")
                .Build();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedVatRegime(new VatRegimes(this.Session).ZeroRated)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductFeatureItem)
                .WithProductFeature(colorWhite).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();

            var item3 = new SalesOrderItemBuilder(this.Session)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductFeatureItem)
                .WithProductFeature(extraLarge).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();

            item1.AddOrderedWithFeature(item2);
            item1.AddOrderedWithFeature(item3);

            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();

            var item5 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();

            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);
            order.AddSalesOrderItem(item5);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];

            shipment.Pick();
            this.Session.Derive();

            var pickList = good1.Part.InventoryItemsWherePart[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            //// item5: only 4 out of 5 are actually picked
            foreach (PickListItem pickListItem in pickList.PickListItems)
            {
                if (pickListItem.Quantity == 5)
                {
                    pickListItem.QuantityPicked = 4;
                }
            }

            pickList.SetPicked();

            this.Session.Derive();

            //// all orderitems have same physical finished good, so there is only one picklist item.
            Assert.Equal(1, item1.QuantityReserved);
            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(2, item4.QuantityReserved);
            Assert.Equal(0, item4.QuantityRequestsShipping);
            Assert.Equal(5, item5.QuantityReserved);
            Assert.Equal(0, item5.QuantityRequestsShipping);
            Assert.Equal(97, good1Inventory.QuantityOnHand);
            Assert.Equal(0, good1Inventory.QuantityCommittedOut);
            Assert.Equal(96, good2Inventory.QuantityOnHand);
            Assert.Equal(0, good2Inventory.QuantityCommittedOut);
        }

        [Fact]
        public void GivenPickList_WhenActualQuantityPickedIsLess_ThenShipmentItemQuantityIsAdjusted()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen)
                .WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var customer = new PersonBuilder(this.Session).WithLastName("person1")
                .WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good1.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(7)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good2.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(7)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .Build();

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good2.Part).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];
            shipment.Pick();
            this.Session.Derive();

            var pickList = good1.Part.InventoryItemsWherePart[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            //// item3: only 4 out of 5 are actually picked
            PickListItem adjustedPicklistItem = null;
            foreach (PickListItem pickListItem in pickList.PickListItems)
            {
                if (pickListItem.Quantity == 5)
                {
                    adjustedPicklistItem = pickListItem;
                }
            }

            var itemIssuance = adjustedPicklistItem.ItemIssuancesWherePickListItem[0];
            var shipmentItem = adjustedPicklistItem.ItemIssuancesWherePickListItem[0].ShipmentItem;

            Assert.Equal(2, shipment.ShipmentItems.Count);
            Assert.Equal(5, itemIssuance.Quantity);
            Assert.Equal(5, shipmentItem.Quantity);
            Assert.Equal(5, item3.QuantityPendingShipment);

            adjustedPicklistItem.QuantityPicked = 4;

            pickList.SetPicked();
            this.Session.Derive();

            // When SalesOrder is derived 1 quantity is requested for shipping (because inventory is available and quantity ordered (5) is greater then quantity pending shipment (4)
            // A new shipment item is created with quantity 1 and QuantityPendingShipment remains 5
            Assert.Equal(4, itemIssuance.Quantity);
            Assert.Equal(4, shipmentItem.Quantity);
            Assert.Equal(3, shipment.ShipmentItems.Count);
            Assert.Equal(1, shipment.ShipmentItems.Last().Quantity);
            Assert.Equal(5, item3.QuantityPendingShipment);
        }

        [Fact]
        public void GivenSalesOrder_WhenShipmentIsCreated_ThenOrderItemsAreAddedToPickList()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen)
                .WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();

            var customer = new PersonBuilder(this.Session).WithLastName("person1")
                .WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good1.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(7)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good2.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(7)
                .Build();

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good2.Part).Build();

            this.Session.Derive();

            var good1Inventory = good1.Part.InventoryItemsWherePart.First;
            var good2Inventory = good2.Part.InventoryItemsWherePart.First;

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1)
                .WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2)
                .WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5)
                .WithAssignedUnitPrice(15).Build();

            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];
            shipment.Pick();
            this.Session.Derive();

            var pickList = good1.Part.InventoryItemsWherePart[0].PickListItemsWhereInventoryItem[0]
                .PickListWherePickListItem;

            Assert.Equal(2, pickList.PickListItems.Count);

            var extent1 = pickList.PickListItems;
            extent1.Filter.AddEquals(M.PickListItem.InventoryItem, good1Inventory);
            Assert.Equal(3, extent1.First.Quantity);

            var extent2 = pickList.PickListItems;
            extent2.Filter.AddEquals(M.PickListItem.InventoryItem, good2Inventory);
            Assert.Equal(5, extent2.First.Quantity);
        }

        [Fact]
        public void GivenMultipleOrders_WhenCombinedPickListIsPicked_ThenSingleShipmentIsPickedState()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen)
                .WithAddress1("Haverwerf").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var customer = new PersonBuilder(this.Session).WithLastName("person1")
                .WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer)
                .Build();

            new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good1.Part)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(7)
                .WithSupplier(supplier)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good2.Part)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithPrice(7)
                .WithSupplier(supplier)
                .Build();

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good2.Part).Build();

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1)
                .WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2)
                .WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5)
                .WithAssignedUnitPrice(15).Build();

            order1.AddSalesOrderItem(item1);
            order1.AddSalesOrderItem(item2);
            order1.AddSalesOrderItem(item3);

            this.Session.Derive();

            order1.SetReadyForPosting();
            this.Session.Derive();

            order1.Post();
            this.Session.Derive();

            order1.Accept();
            this.Session.Derive();

            var order2 = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var itema = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1)
                .WithAssignedUnitPrice(15).Build();
            var itemb = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(1)
                .WithAssignedUnitPrice(15).Build();
            order2.AddSalesOrderItem(itema);
            order2.AddSalesOrderItem(itemb);

            this.Session.Derive();

            order2.SetReadyForPosting();
            this.Session.Derive();

            order2.Post();
            this.Session.Derive();

            order2.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];
            shipment.Pick();
            this.Session.Derive();

            var pickList = good1.Part.InventoryItemsWherePart[0].PickListItemsWhereInventoryItem[0]
                .PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();

            this.Session.Derive();

            Assert.Single(customer.ShipmentsWhereShipToParty);

            var customerShipment = (CustomerShipment)customer.ShipmentsWhereShipToParty.First;
            Assert.Equal(new ShipmentStates(this.Session).Picked, customerShipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerBuyingFromDifferentStores_WhenShipping_ThenPickListIsCreatedForEachStore()
        {
            var store1 = new Stores(this.Session).FindBy(M.Store.Name, "store");

            var store2 = new StoreBuilder(this.Session).WithName("second store")
                .WithDefaultFacility(new Facilities(this.Session).Extent().First)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithSalesOrderNumberPrefix("")
                .WithOutgoingShipmentNumberPrefix("")
                .WithIsImmediatelyPacked(true)
                .Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive(true);

            var order1 = new SalesOrderBuilder(this.Session)
                .WithStore(store1)
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(mechelenAddress)
                .WithShipToCustomer(customer)
                .Build();

            var order1Item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            order1.AddSalesOrderItem(order1Item);

            this.Session.Derive(true);

            order1.SetReadyForPosting();
            this.Session.Derive(true);

            order1.Post();
            this.Session.Derive();

            order1.Accept();
            this.Session.Derive();

            Assert.Single(customer.ShipmentsWhereShipToParty);

            var order2 = new SalesOrderBuilder(this.Session)
                .WithStore(store1)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order2Item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            order2.AddSalesOrderItem(order2Item);

            this.Session.Derive(true);

            order2.SetReadyForPosting();
            this.Session.Derive(true);

            order2.Post();
            this.Session.Derive();

            order2.Accept();
            this.Session.Derive();

            Assert.Single(customer.ShipmentsWhereShipToParty);

            var store1Shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress.First(v => v.Store.Equals(store1));

            store1Shipment.Pick();
            this.Session.Derive();

            var order3 = new SalesOrderBuilder(this.Session)
                .WithStore(store2)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order3Item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order3.AddSalesOrderItem(order3Item);

            this.Session.Derive(true);

            order3.SetReadyForPosting();
            this.Session.Derive(true);

            order3.Post();
            this.Session.Derive();

            order3.Accept();
            this.Session.Derive();

            var store2Shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress.First(v => v.Store.Equals(store2));

            store2Shipment.Pick();
            this.Session.Derive();

            var store1PickList = customer.PickListsWhereShipToParty.FirstOrDefault(v => v.Store.Equals(store1));
            var store2PickList = customer.PickListsWhereShipToParty.FirstOrDefault(v => v.Store.Equals(store2));

            Assert.Equal(2, customer.PickListsWhereShipToParty.Count);
            Assert.NotNull(store1PickList);
            Assert.Equal(3, store1PickList.PickListItems[0].Quantity);
            Assert.NotNull(store2PickList);
            Assert.Equal(5, store2PickList.PickListItems[0].Quantity);
        }
    }

    [Trait("Category", "Security")]
    public class PickListSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenPickList_WhenObjectStateIsCreated_ThenCheckTransitions()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            User user = this.Administrator;
            this.Session.SetUser(user);

            var pickList = new PickListBuilder(this.Session).Build();

            this.Session.Derive();

            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[pickList];
            Assert.True(acl.CanExecute(M.PickList.Cancel));
        }

        [Fact]
        public void GivenPickList_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            User user = this.OrderProcessor;
            this.Session.SetUser(user);

            var pickList = new PickListBuilder(this.Session).Build();

            this.Session.Derive();

            pickList.Cancel();

            this.Session.Derive();

            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[pickList];
            Assert.False(acl.CanExecute(M.PickList.Cancel));
            Assert.False(acl.CanExecute(M.PickList.SetPicked));
        }

        [Fact]
        public void GivenPickList_WhenObjectStateIsPicked_ThenCheckTransitions()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            User user = this.OrderProcessor;
            this.Session.SetUser(user);

            var pickList = new PickListBuilder(this.Session).Build();

            this.Session.Derive();

            pickList.SetPicked();

            this.Session.Derive();

            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[pickList];
            Assert.False(acl.CanExecute(M.PickList.Cancel));
            Assert.False(acl.CanExecute(M.PickList.SetPicked));
        }
    }
}

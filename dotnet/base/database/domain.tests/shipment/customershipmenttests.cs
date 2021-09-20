// <copyright file="CustomerShipmentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

using System.ComponentModel;
using Allors.Domain.TestPopulation;

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Xunit;

    public class CustomerShipmentTests : DomainTest
    {
        [Fact]
        public void GivenCustomerShipment_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Created, shipment.ShipmentState);
            Assert.Equal(shipment.LastShipmentState, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive();

            Assert.Null(shipment.PreviousShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();

            this.Session.Derive();
            this.Session.Commit();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(new PostalAddresses(this.Session).Extent().First)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Boat)
                .Build();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Created, shipment.ShipmentState);
            Assert.Equal(this.InternalOrganisation.ShippingAddress, shipment.ShipFromAddress);
            Assert.Equal(shipment.ShipFromParty, shipment.ShipFromParty);
            Assert.Equal(new Stores(this.Session).FindBy(M.Store.Name, "store"), shipment.Store);
        }

        [Fact]
        public void GivenCustomerShipment_WhenGettingShipmentNumberWithoutFormat_ThenShipmentNumberShouldBeReturned()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.Session).WithName("store")
                .WithDefaultFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithIsImmediatelyPacked(true)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment1 = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            this.Session.Derive();

            Assert.Equal("1", shipment1.ShipmentNumber);

            var shipment2 = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            this.Session.Derive();

            Assert.Equal("2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenCustomerShipment_WhenGettingShipmentNumberWithFormat_ThenFormattedShipmentNumberShouldBeReturned()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.InternalOrganisation.CustomerShipmentSequence = new CustomerShipmentSequences(this.Session).EnforcedSequence;
            var store = new StoreBuilder(this.Session).WithName("store")
                .WithDefaultFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .WithOutgoingShipmentNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithIsImmediatelyPacked(true)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment1 = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            this.Session.Derive();

            Assert.Equal("the format is 1", shipment1.ShipmentNumber);

            var shipment2 = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            this.Session.Derive();

            Assert.Equal("the format is 2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenShipFromWithoutShipmentNumberPrefix_WhenDeriving_ThenSortableShipmentNumberIsSet()
        {
            this.InternalOrganisation.StoresWhereInternalOrganisation.First.RemoveOutgoingShipmentNumberPrefix();
            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .Build();

            this.Session.Derive();

            Assert.Equal(int.Parse(shipment.ShipmentNumber), shipment.SortableShipmentNumber);
        }

        [Fact]
        public void GivenShipFromWithShipmentNumberPrefix_WhenDeriving_ThenSortableShipmentNumberIsSet()
        {
            this.InternalOrganisation.CustomerShipmentSequence = new CustomerShipmentSequences(this.Session).EnforcedSequence;
            this.InternalOrganisation.StoresWhereInternalOrganisation.First.OutgoingShipmentNumberPrefix = "prefix-";
            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .Build();

            this.Session.Derive();

            Assert.Equal(int.Parse(shipment.ShipmentNumber.Split('-')[1]), shipment.SortableShipmentNumber);
        }

        [Fact]
        public void GivenCustomerShipmentWithShipToCustomerWithShippingAddress_WhenDeriving_ThenShipToAddressMustExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithAddress1("Haverwerf 15").WithPostalAddressBoundary(mechelen).Build();

            var shippingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipToAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(shippingAddress);

            this.Session.Derive();

            var customerShipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive();

            Assert.Equal(shippingAddress.ContactMechanism, customerShipment.ShipToAddress);
        }

        [Fact]
        public void GivenCustomerShipment_WhenAllItemsArePutIntoShipmentPackages_ThenShipmentStateIsPacked()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithAddress1("Haverwerf 15").WithPostalAddressBoundary(mechelen).Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good2.Part).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedBillToContactMechanism(mechelenAddress)
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

            customer.PickListsWhereShipToParty.First.SetPicked();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Packed, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenAddingAndRemovingPackages_ThenPackageSequenceNumberIsRecalculated()
        {
            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToAddress(new PostalAddresses(this.Session).Extent().First)
                .WithShipToParty(new Organisations(this.Session).Extent().First)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Boat)
                .Build();

            var package1 = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package1);

            this.Session.Derive();

            Assert.Equal(1, package1.SequenceNumber);

            var package2 = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package2);

            this.Session.Derive();

            Assert.Equal(2, package2.SequenceNumber);

            var package3 = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package3);

            this.Session.Derive();

            Assert.Equal(3, package3.SequenceNumber);

            shipment.RemoveShipmentPackage(package1);

            this.Session.Derive();

            Assert.Equal(2, package2.SequenceNumber);
            Assert.Equal(3, package3.SequenceNumber);

            var package4 = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package4);

            this.Session.Derive();

            Assert.Equal(2, package2.SequenceNumber);
            Assert.Equal(3, package3.SequenceNumber);
            Assert.Equal(4, package4.SequenceNumber);

            shipment.RemoveShipmentPackage(package4);

            this.Session.Derive();

            var package5 = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package5);

            this.Session.Derive();

            Assert.Equal(2, package2.SequenceNumber);
            Assert.Equal(3, package3.SequenceNumber);
            Assert.Equal(4, package5.SequenceNumber);
        }

        [Fact]
        public void GivenCustomerShipment_WhenDeriving_ThenTotalShipmentValueIsCalculated()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;
            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good2.Part).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedBillToContactMechanism(mechelenAddress)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(75, shipment.ShipmentValue);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(10).WithAssignedUnitPrice(10).Build();
            order.AddSalesOrderItem(item2);

            //item2.Confirm();

            //this.Session.Derive();

            //Assert.Equal(175, shipment.ShipmentValue);
        }

        [Fact]
        public void GivenShipmentThreshold_WhenNewCustomerShipmentIsBelowThreshold_ThenShipmentIsSetOnHold()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;
            store.IsImmediatelyPicked = false;

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

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedBillToContactMechanism(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            Assert.Equal(new ShipmentStates(this.Session).OnHold, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenShipmentValueFallsBelowThreshold_ThenShipmentAndPendigPickListAreSetOnHold()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;
            store.IsImmediatelyPicked = false;

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

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedBillToContactMechanism(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).OnHold, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHold_WhenShipmentValueRisesAboveThreshold_ThenShipmentIsReleased()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;
            store.IsImmediatelyPicked = false;

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedBillToContactMechanism(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            Assert.Equal(new ShipmentStates(this.Session).OnHold, shipment.ShipmentState);

            item.QuantityOrdered = 10;

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Created, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHold_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
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
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedBillToContactMechanism(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(10).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();

            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Packed, shipment.ShipmentState);

            shipment.Hold();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).OnHold, shipment.ShipmentState);

            shipment.Ship();
            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).OnHold, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHold_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToCreated()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;
            store.IsImmediatelyPicked = false;

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

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).OnHold, shipment.ShipmentState);

            shipment.ReleasedManually = true;

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Created, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHoldWithPickedPickList_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToPicked()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;

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

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(10).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Picked, shipment.ShipmentState);

            shipment.Hold();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).OnHold, shipment.ShipmentState);

            shipment.Continue();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Picked, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHoldWithAllItemsPacked_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToPacked()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;

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
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();
            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).OnHold, shipment.ShipmentState);

            shipment.Continue();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Packed, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerBuyingFromDifferentStores_WhenShipping_ThenDifferentShipmentIsCreatedForEachStore()
        {
            new StoreBuilder(this.Session).WithName("second store")
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
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive(true);

            var order1 = new SalesOrderBuilder(this.Session)
                .WithStore(new Stores(this.Session).FindBy(M.Store.Name, "store"))
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
            this.Session.Derive(true);

            order1.Accept();
            this.Session.Derive();

            Assert.Single(customer.ShipmentsWhereShipToParty);

            var order2 = new SalesOrderBuilder(this.Session)
                .WithStore(new Stores(this.Session).FindBy(M.Store.Name, "store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order2Item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            order2.AddSalesOrderItem(order2Item);

            this.Session.Derive(true);

            order2.SetReadyForPosting();
            this.Session.Derive(true);

            order2.Post();
            this.Session.Derive(true);

            order2.Accept();
            this.Session.Derive();

            Assert.Single(customer.ShipmentsWhereShipToParty);

            var order3 = new SalesOrderBuilder(this.Session)
                .WithStore(new Stores(this.Session).FindBy(M.Store.Name, "second store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order3Item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            order3.AddSalesOrderItem(order3Item);

            this.Session.Derive(true);

            order3.SetReadyForPosting();
            this.Session.Derive(true);

            order3.Post();
            this.Session.Derive(true);

            order3.Accept();
            this.Session.Derive();

            Assert.Equal(2, customer.ShipmentsWhereShipToParty.Count);
        }

        [Fact]
        public void GivenCustomerShipment_WhenStateIsSetToShipped_ThenInvoiceIsCreated()
        {
            var assessable = new VatRegimes(this.Session).DutchStandardTariff;

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPartyContactMechanism(billToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedVatRegime(assessable)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            shipment.Ship();

            this.Session.Derive();

            var salesInvoiceitem =
                (SalesInvoiceItem)shipment.ShipmentItems[0].ShipmentItemBillingsWhereShipmentItem[0].InvoiceItem;
            var invoice = salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;

            Assert.NotNull(invoice);
        }

        [Fact]
        public void GivenCustomerShipmentWithoutOrder_WhenStateIsSetToShipped_ThenInventoryIsUpdated()
        {
            var good = new UnifiedGoodBuilder(this.Session).WithNonSerialisedDefaults(this.InternalOrganisation).Build();

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session)
                .WithQuantity(100)
                .WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount)
                .WithPart(good)
                .Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(mechelenAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            var shipmentItem = new ShipmentItemBuilder(this.Session).WithGood(good).WithQuantity(10).Build();
            shipment.AddShipmentItem(shipmentItem);

            this.Session.Derive();

            var inventory = good.InventoryItemsWherePart.First as NonSerialisedInventoryItem;

            Assert.Equal(100, good.QuantityOnHand);
            Assert.Equal(100, good.AvailableToPromise);
            Assert.Equal(100, inventory.QuantityOnHand);
            Assert.Equal(0, shipmentItem.QuantityPicked);
            Assert.Equal(0, shipmentItem.QuantityShipped);

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();
            this.Session.Derive();

            Assert.Equal(90, good.QuantityOnHand);
            Assert.Equal(90, good.AvailableToPromise);
            Assert.Equal(90, inventory.QuantityOnHand);
            Assert.Equal(10, shipmentItem.QuantityPicked);
            Assert.Equal(0, shipmentItem.QuantityShipped);

            var package = new ShipmentPackageBuilder(this.Session).Build();
            package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            shipment.AddShipmentPackage(package);

            this.Session.Derive();

            Assert.Equal(90, good.QuantityOnHand);
            Assert.Equal(90, good.AvailableToPromise);
            Assert.Equal(90, inventory.QuantityOnHand);
            Assert.Equal(10, shipmentItem.QuantityPicked);
            Assert.Equal(0, shipmentItem.QuantityShipped);

            shipment.Ship();
            this.Session.Derive();

            Assert.Equal(90, good.QuantityOnHand);
            Assert.Equal(90, good.AvailableToPromise);
            Assert.Equal(90, inventory.QuantityOnHand);
            Assert.Equal(10, shipmentItem.QuantityPicked);
            Assert.Equal(10, shipmentItem.QuantityShipped);
        }

        [Fact]
        public void GivenCustomerShipmentContainingOrderOnHold_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.Session).DutchStandardTariff;

            this.Session.Derive();
            this.Session.Commit();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            shipment.Pick();

            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();

            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            order.Hold();

            this.Session.Derive();

            shipment.Ship();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Packed, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentWithQuantityPackagedDifferentFromShippingQuantity_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.Session).DutchStandardTariff;

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
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;
            pickList.SetPicked();

            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity - 1).Build());
            }

            this.Session.Derive();

            shipment.Ship();

            Assert.Equal(new ShipmentStates(this.Session).Picked, shipment.ShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentWithValueBelowThreshold_WhenShippingToBelgium_ThenInvoiceIncludesCosts()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            new ShippingAndHandlingComponentBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeographicBoundary(mechelen)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipmentValue(new ShipmentValueBuilder(this.Session).WithThroughAmount(300M).Build())
                .WithCost(15M)
                .Build();

            new ShippingAndHandlingComponentBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithShipmentValue(new ShipmentValueBuilder(this.Session).WithThroughAmount(300M).Build())
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithCost(20M)
                .Build();

            new ShippingAndHandlingComponentBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithShipmentValue(new ShipmentValueBuilder(this.Session).WithThroughAmount(300M).Build())
                .WithShipmentMethod(new ShipmentMethods(this.Session).FirstClassAir)
                .WithCost(50M)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();

            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var billToContactMechanismMechelen = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Mechelen").Build();
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(billToContactMechanismMechelen)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();

            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            shipment.Ship();

            this.Session.Derive();

            var invoice = customer.SalesInvoicesWhereBillToCustomer.First;
            Assert.Equal(15M, invoice.TotalShippingAndHandling);
        }
    }

    [Trait("Category", "Security")]
    public class CustomerShipmentSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenCustomerShipment_WhenObjectStateIsCreated_ThenCheckTransitions()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            User user = this.Administrator;
            this.Session.SetUser(user);

            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive();

            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[shipment];
            Assert.Equal(new ShipmentStates(this.Session).Created, shipment.ShipmentState);
            Assert.True(acl.CanExecute(M.CustomerShipment.Cancel));
        }

        [Fact]
        public void GivenCustomerShipment_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            User user = this.OrderProcessor;
            this.Session.SetUser(user);

            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            shipment.Cancel();

            this.Session.Derive();

            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[shipment];
            Assert.False(acl.CanExecute(M.CustomerShipment.Cancel));
        }

        [Fact]
        public void GivenCustomerShipment_WhenObjectStateIsShipped_ThenCheckTransitions()
        {
            User user = this.Administrator;
            this.Session.SetUser(user);

            var assessable = new VatRegimes(this.Session).DutchStandardTariff;

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPartyContactMechanism(billToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithAssignedBillToContactMechanism(mechelenAddress)
                .WithAssignedVatRegime(assessable)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            order.SetReadyForPosting();
            this.Session.Derive();

            order.Post();
            this.Session.Derive();

            order.Accept();
            this.Session.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            shipment.Pick();
            this.Session.Derive();

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            shipment.Ship();

            this.Session.Derive();

            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[shipment];
            Assert.Equal(new ShipmentStates(this.Session).Shipped, shipment.ShipmentState);
            Assert.False(acl.CanExecute(M.CustomerShipment.Cancel));
            Assert.False(acl.CanWrite(M.Shipment.HandlingInstruction));
            Assert.True(acl.CanWrite(M.Shipment.ElectronicDocuments));
        }
    }
}

//------------------------------------------------------------------------------------------------- 
// <copyright file="CustomerShipmentTests.cs" company="Allors bvba">
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
    
    public class CustomerShipmentTests : DomainTest
    {
        [Fact]
        public void GivenCustomerShipment_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Created, shipment.CustomerShipmentState);
            Assert.Equal(shipment.LastCustomerShipmentState, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive();

            Assert.Null(shipment.PreviousCustomerShipmentState);
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

            Assert.Equal(new CustomerShipmentStates(this.Session).Created, shipment.CustomerShipmentState);
            Assert.Equal(this.InternalOrganisation.ShippingAddress, shipment.ShipFromAddress);
            Assert.Equal(shipment.ShipFromParty, shipment.ShipFromParty);
            Assert.Equal(new Stores(this.Session).FindBy(M.Store.Name, "store"), shipment.Store);
        }

        [Fact]
        public void GivenCustomerShipment_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();

            this.Session.Commit();

            var builder = new CustomerShipmentBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithShipToParty(customer);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithShipmentMethod(new ShipmentMethods(this.Session).Ground);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithShipToAddress(shipToAddress);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCustomerShipment_WhenGettingShipmentNumberWithoutFormat_ThenShipmentNumberShouldBeReturned()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.Session).WithName("store")
                .WithDefaultFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment1 = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.Equal("1", shipment1.ShipmentNumber);

            var shipment2 = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.Equal("2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenCustomerShipment_WhenGettingShipmentNumberWithFormat_ThenFormattedShipmentNumberShouldBeReturned()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.Session).WithName("store")
                .WithDefaultFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .WithOutgoingShipmentNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment1 = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.Equal("the format is 1", shipment1.ShipmentNumber);

            var shipment2 = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(new PersonBuilder(this.Session).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.Equal("the format is 2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenCustomerShipmentWithShipToCustomerWithShippingAddress_WhenDeriving_ThenShipToAddressMustExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

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
        public void GivenCustomerShipment_WhenObjectStateIsCreated_ThenCheckTransitions()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            this.SetIdentity("orderProcessor");
            //this.SetIdentity("Administrator");

            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive();

            var acl = new AccessControlList(shipment, this.Session.GetUser());
            Assert.Equal(new CustomerShipmentStates(this.Session).Created, shipment.CustomerShipmentState);
            Assert.True(acl.CanExecute(M.CustomerShipment.Cancel));
        }

        [Fact]
        public void GivenCustomerShipment_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            this.SetIdentity("orderProcessor");

            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            shipment.Cancel();

            this.Session.Derive();

            var acl = new AccessControlList(shipment, this.Session.GetUser());
            Assert.False(acl.CanExecute(M.CustomerShipment.Cancel));
        }

        [Fact]
        public void GivenCustomerShipment_WhenObjectStateIsShipped_ThenCheckTransitions()
        {
            this.SetIdentity("administrator");

            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

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
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithBillToContactMechanism(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

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

            var acl = new AccessControlList(shipment, this.Session.GetUser());
            Assert.Equal(new CustomerShipmentStates(this.Session).Shipped, shipment.CustomerShipmentState);
            Assert.False(acl.CanExecute(M.CustomerShipment.Cancel));
<<<<<<< HEAD
            Assert.False(acl.CanWrite(M.Shipment.HandlingInstruction));
=======
            Assert.False(acl.CanWrite(M.CustomerShipment.HandlingInstruction.RoleType));
>>>>>>> 38d89a4ba6bd64e68c381d7560e2f7dd9a78280e
        }

        [Fact]
        public void GivenCustomerShipment_WhenAllItemsArePutIntoShipmentPackages_ThenCustomerShipmentStateIsPacked()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood1 = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var finishedGood2 = new FinishedGoodBuilder(this.Session)
                .WithPartId("2")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood1)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood2)
                .Build();

            var good1InventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            var good2InventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithBillToContactMechanism(mechelenAddress)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];
            customer.PickListsWhereShipToParty.First.SetPicked();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Packed, shipment.CustomerShipmentState);
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

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood1 = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var finishedGood2 = new FinishedGoodBuilder(this.Session)
                .WithPartId("2")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood1)
                .Build();

            var good1InventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            var good2 = new GoodBuilder(this.Session)
                .WithSku("10202")
                .WithName("good2")
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood2)
                .Build();

            var good2InventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Theft).Build());

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
                .WithBillToContactMechanism(mechelenAddress)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(75, shipment.ShipmentValue);

            item1.QuantityOrdered = 3;

            this.Session.Derive();

            Assert.Equal(45, shipment.ShipmentValue);

            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(10).Build();
            order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(145, shipment.ShipmentValue);
        }

        [Fact]
        public void GivenShipmentThreshold_WhenNewCustomerShipmentIsBelowThreshold_ThenShipmentAndPicklistAreSetOnHold()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;
            store.IsImmediatelyPicked = false;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithBillToContactMechanism(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;

            Assert.Equal(new CustomerShipmentStates(this.Session).OnHold, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.Session).OnHold, pickList.PickListState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenShipmentValueFallsBelowThreshold_ThenShipmentAndPendigPicklistAreSetOnHold()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;
            store.IsImmediatelyPicked = false;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithBillToContactMechanism(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;

            Assert.Equal(new CustomerShipmentStates(this.Session).Created, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.Session).Created, pickList.PickListState);

            item.QuantityOrdered = 5;

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).OnHold, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.Session).OnHold, pickList.PickListState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHold_WhenShipmentValueRisesAboveThreshold_ThenShipmentAndPendigPicklistAreReleased()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;
            store.IsImmediatelyPicked = false;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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
                .WithBillToContactMechanism(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;

            Assert.Equal(new CustomerShipmentStates(this.Session).OnHold, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.Session).OnHold, pickList.PickListState);

            item.QuantityOrdered = 10;

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Created, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.Session).Created, pickList.PickListState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHold_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithVatRate(vatRate21)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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
                .WithBillToContactMechanism(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Packed, shipment.CustomerShipmentState);

            shipment.Hold();

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).OnHold, shipment.CustomerShipmentState);

            shipment.Ship();
            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).OnHold, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHold_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToCreated()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;
            store.IsImmediatelyPicked = false;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithVatRate(vatRate21)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).OnHold, shipment.CustomerShipmentState);

            shipment.ReleasedManually = true;

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Created, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHoldWithPickedPickList_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToPicked()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithVatRate(vatRate21)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            //var derivation = new Allors.Domain.Logging.Derivation(this.Session, new DerivationConfig { DerivationLogFunc = () => new DerivationLog() });
            //derivation.Derive();

            //var list = ((DerivationLog)derivation.DerivationLog).List;
            ////list.RemoveAll(v => !v.StartsWith("Dependency"));

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Picked, shipment.CustomerShipmentState);

            item.QuantityOrdered = 1;

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).OnHold, shipment.CustomerShipmentState);

            shipment.Continue();

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Picked, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHoldWithAllItemsPacked_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToPacked()
        {
            var store = this.Session.Extent<Store>().First;
            store.ShipmentThreshold = 100;

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithVatRate(vatRate21)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");
            pickList.SetPicked();

            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).OnHold, shipment.CustomerShipmentState);

            shipment.Continue();

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Packed, shipment.CustomerShipmentState);
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
                .Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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

            var order1 = new SalesOrderBuilder(this.Session)
                .WithStore(new Stores(this.Session).FindBy(M.Store.Name, "store"))
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(mechelenAddress)
                .WithShipToCustomer(customer)
                .Build();

            var order1Item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order1.AddSalesOrderItem(order1Item);

            this.Session.Derive();

            order1.Confirm();

            this.Session.Derive();

            Assert.Equal(1, customer.ShipmentsWhereShipToParty.Count);
            Assert.Equal(1, customer.PickListsWhereShipToParty.Count);

            var order2 = new SalesOrderBuilder(this.Session)
                .WithStore(new Stores(this.Session).FindBy(M.Store.Name, "store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order2Item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order2.AddSalesOrderItem(order2Item);

            this.Session.Derive();

            order2.Confirm();

            this.Session.Derive();

            Assert.Equal(1, customer.ShipmentsWhereShipToParty.Count);
            Assert.Equal(1, customer.PickListsWhereShipToParty.Count);

            var order3 = new SalesOrderBuilder(this.Session)
                .WithStore(new Stores(this.Session).FindBy(M.Store.Name, "second store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order3Item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order3.AddSalesOrderItem(order3Item);

            this.Session.Derive();

            order3.Confirm();

            this.Session.Derive();

            Assert.Equal(2, customer.ShipmentsWhereShipToParty.Count);
            Assert.Equal(1, customer.PickListsWhereShipToParty.Count);
        }

        [Fact]
        public void GivenCustomerShipment_WhenDeriving_ThenBillFromContactMechanismMustExist()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();

            this.Session.Derive();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(new PostalAddresses(this.Session).Extent().First)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Boat)
                .Build();

            this.Session.Derive();

            Assert.Equal(this.InternalOrganisation.BillingAddress, shipment.BillFromContactMechanism);
        }

        [Fact]
        public void GivenCustomerShipment_WhenStateIsSetToShipped_ThenInvoiceIsCreated()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

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
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

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

            var salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice = salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;

            Assert.NotNull(invoice);
        }

        [Fact]
        public void GivenCustomerShipmentWithPendingPickList_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithVatRate(vatRate21)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            shipment.Ship();

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Packed, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentContainingOrderOnHold_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            this.Session.Derive();
            this.Session.Commit();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

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

            Assert.Equal(new CustomerShipmentStates(this.Session).Packed, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentWithQuantityPackagedDifferentFromShippingQuantity_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithVatRate(vatRate21)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

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
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");
            pickList.SetPicked();

            this.Session.Derive();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            item.QuantityOrdered = 4;

            this.Session.Derive();

            shipment.Ship();

            Assert.Equal(new CustomerShipmentStates(this.Session).Picked, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentWithValueBelowThreshold_WhenShippingToBelgium_TheninvoiceIncludesCosts()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            new ShippingAndHandlingComponentBuilder(this.Session)
                .WithFromDate(DateTime.UtcNow)
                .WithGeographicBoundary(mechelen)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipmentValue(new ShipmentValueBuilder(this.Session).WithThroughAmount(300M).Build())
                .WithCost(15M)
                .Build();

            new ShippingAndHandlingComponentBuilder(this.Session)
                .WithFromDate(DateTime.UtcNow)
                .WithShipmentValue(new ShipmentValueBuilder(this.Session).WithThroughAmount(300M).Build())
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithCost(20M)
                .Build();

            new ShippingAndHandlingComponentBuilder(this.Session)
                .WithFromDate(DateTime.UtcNow)
                .WithShipmentValue(new ShipmentValueBuilder(this.Session).WithThroughAmount(300M).Build())
                .WithShipmentMethod(new ShipmentMethods(this.Session).FirstClassAir)
                .WithCost(50M)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithPartId("1")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good1")
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .WithFinishedGood(finishedGood)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithPart(finishedGood).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var billToContactMechanismMechelen = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Mechelen").Build();
            var internalOrganisation = this.InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanismMechelen)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

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
            Assert.Equal(15M, invoice.ShippingAndHandlingCharge.Amount);
        }
    }


    public class DerivationLog : Allors.Domain.Logging.ListDerivationLog
    {
        public override void AddedDerivable(Allors.Domain.Object derivable)
        {
            if (derivable is Permission)
            {

            }

            base.AddedDerivable(derivable);
        }

        /// <summary>
        /// The dependee is derived before the dependent object;
        /// </summary>
        public override void AddedDependency(Allors.Domain.Object dependent, Allors.Domain.Object dependee)
        {
            if (dependent is Permission || dependee is Permission)
            {

            }

            base.AddedDependency(dependent, dependee);
        }
    }

}
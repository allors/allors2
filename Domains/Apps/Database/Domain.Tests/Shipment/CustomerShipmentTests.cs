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
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Created, shipment.CustomerShipmentState);
            Assert.Equal(shipment.LastCustomerShipmentState, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Null(shipment.PreviousCustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipFromAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            internalOrganisation.ShippingAddress = shipFromAddress;

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(new PostalAddresses(this.DatabaseSession).Extent().First)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Boat)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Created, shipment.CustomerShipmentState);
            Assert.Equal(shipFromAddress, shipment.ShipFromAddress);
            Assert.Equal(shipment.ShipFromParty, shipment.ShipFromParty);
            Assert.Equal(new Stores(this.DatabaseSession).FindBy(M.Store.Name, "store"), shipment.Store);
        }

        [Fact]
        public void GivenCustomerShipment_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

            this.DatabaseSession.Commit();

            var builder = new CustomerShipmentBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithShipToParty(customer);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithShipToAddress(shipToAddress);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCustomerShipment_WhenGettingShipmentNumberWithoutFormat_ThenShipmentNumberShouldBeReturned()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Facilities(this.DatabaseSession).FindBy(M.Facility.FacilityType, new FacilityTypes(this.DatabaseSession).Warehouse))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment1 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.Equal("1", shipment1.ShipmentNumber);

            var shipment2 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.Equal("2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenCustomerShipment_WhenGettingShipmentNumberWithFormat_ThenFormattedShipmentNumberShouldBeReturned()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Facilities(this.DatabaseSession).FindBy(M.Facility.FacilityType, new FacilityTypes(this.DatabaseSession).Warehouse))
                .WithOutgoingShipmentNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment1 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.Equal("the format is 1", shipment1.ShipmentNumber);

            var shipment2 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.Equal("the format is 2", shipment2.ShipmentNumber);
        }

        [Fact]
        public void GivenCustomerShipmentWithShipToCustomerWithShippingAddress_WhenDeriving_ThenShipToAddressMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

            var shippingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(shipToAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(shippingAddress);

            this.DatabaseSession.Derive();

            var customerShipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(shippingAddress.ContactMechanism, customerShipment.ShipToAddress);
        }

        [Fact]
        public void GivenCustomerShipment_WhenObjectStateIsCreated_ThenCheckTransitions()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            this.SetIdentity("orderProcessor");
            //this.SetIdentity("Administrator");

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive();

            var acl = new AccessControlList(shipment, new Users(this.DatabaseSession).CurrentUser);
            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Created, shipment.CustomerShipmentState);
            Assert.True(acl.CanExecute(M.CustomerShipment.Cancel));
        }

        [Fact]
        public void GivenCustomerShipment_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            this.SetIdentity("orderProcessor");

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            shipment.Cancel();

            this.DatabaseSession.Derive();

            var acl = new AccessControlList(shipment, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.CustomerShipment.Cancel));
        }

        [Fact]
        public void GivenCustomerShipment_WhenObjectStateIsShipped_ThenCheckTransitions()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPartyContactMechanism(billToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            shipment.Ship();

            this.DatabaseSession.Derive();

            var acl = new AccessControlList(shipment, new Users(this.DatabaseSession).CurrentUser);
            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Shipped, shipment.CustomerShipmentState);
            Assert.False(acl.CanExecute(M.CustomerShipment.Cancel));
            Assert.False(acl.CanWrite(M.CustomerShipment.HandlingInstruction));
        }

        [Fact]
        public void GivenCustomerShipment_WhenAllItemsArePutIntoShipmentPackages_ThenCustomerShipmentStateIsPacked()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var good1InventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var good2InventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];
            customer.PickListsWhereShipToParty.First.SetPicked();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Packed, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenAddingAndRemovingPackages_ThenPackageSequenceNumberIsRecalculated()
        {
            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToAddress(new PostalAddresses(this.DatabaseSession).Extent().First)
                .WithShipToParty(new Organisations(this.DatabaseSession).Extent().First)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Boat)
                .Build();

            var package1 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package1);

            this.DatabaseSession.Derive();

            Assert.Equal(1, package1.SequenceNumber);

            var package2 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package2);

            this.DatabaseSession.Derive();

            Assert.Equal(2, package2.SequenceNumber);

            var package3 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package3);

            this.DatabaseSession.Derive();

            Assert.Equal(3, package3.SequenceNumber);

            shipment.RemoveShipmentPackage(package1);

            this.DatabaseSession.Derive();

            Assert.Equal(2, package2.SequenceNumber);
            Assert.Equal(3, package3.SequenceNumber);

            var package4 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package4);

            this.DatabaseSession.Derive();

            Assert.Equal(2, package2.SequenceNumber);
            Assert.Equal(3, package3.SequenceNumber);
            Assert.Equal(4, package4.SequenceNumber);

            shipment.RemoveShipmentPackage(package4);

            this.DatabaseSession.Derive();

            var package5 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package5);

            this.DatabaseSession.Derive();

            Assert.Equal(2, package2.SequenceNumber);
            Assert.Equal(3, package3.SequenceNumber);
            Assert.Equal(4, package5.SequenceNumber);
        }

        [Fact]
        public void GivenCustomerShipment_WhenDeriving_ThenTotalShipmentValueIsCalculated()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1InventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10202")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2InventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Theft).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(75, shipment.ShipmentValue);

            item1.QuantityOrdered = 3;

            this.DatabaseSession.Derive();

            Assert.Equal(45, shipment.ShipmentValue);

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(10).Build();
            order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive();

            Assert.Equal(145, shipment.ShipmentValue);
        }

        [Fact]
        public void GivenShipmentThreshold_WhenNewCustomerShipmentIsBelowThreshold_ThenShipmentAndPicklistAreSetOnHold()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).OnHold, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.DatabaseSession).OnHold, pickList.PickListState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenShipmentValueFallsBelowThreshold_ThenShipmentAndPendigPicklistAreSetOnHold()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Created, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.DatabaseSession).Created, pickList.PickListState);

            item.QuantityOrdered = 5;

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).OnHold, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.DatabaseSession).OnHold, pickList.PickListState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHold_WhenShipmentValueRisesAboveThreshold_ThenShipmentAndPendigPicklistAreReleased()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).OnHold, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.DatabaseSession).OnHold, pickList.PickListState);

            item.QuantityOrdered = 10;

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Created, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.DatabaseSession).Created, pickList.PickListState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHold_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Packed, shipment.CustomerShipmentState);

            shipment.Hold();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).OnHold, shipment.CustomerShipmentState);

            shipment.Ship();
            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).OnHold, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHoldWithPendingPickList_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToCreated()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).OnHold, shipment.CustomerShipmentState);

            shipment.ReleasedManually = true;

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Created, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHoldWithPickedPickList_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToPicked()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Picked, shipment.CustomerShipmentState);

            item.QuantityOrdered = 1;

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).OnHold, shipment.CustomerShipmentState);

            shipment.Continue();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Picked, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentOnHoldWithAllItemsPacked_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToPacked()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");
            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).OnHold, shipment.CustomerShipmentState);

            shipment.Continue();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Packed, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerBuyingFromDifferentStores_WhenShipping_ThenDifferentShipmentIsCreatedForEachStore()
        {
            new StoreBuilder(this.DatabaseSession).WithName("second store")
                .WithDefaultFacility(new Facilities(this.DatabaseSession).Extent().First)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .WithSalesOrderNumberPrefix("")
                .WithOutgoingShipmentNumberPrefix("")
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithStore(new Stores(this.DatabaseSession).FindBy(M.Store.Name, "store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order1Item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order1.AddSalesOrderItem(order1Item);

            this.DatabaseSession.Derive();

            order1.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(1, customer.ShipmentsWhereShipToParty.Count);
            Assert.Equal(1, customer.PickListsWhereShipToParty.Count);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithStore(new Stores(this.DatabaseSession).FindBy(M.Store.Name, "store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order2Item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order2.AddSalesOrderItem(order2Item);

            this.DatabaseSession.Derive();

            order2.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(1, customer.ShipmentsWhereShipToParty.Count);
            Assert.Equal(1, customer.PickListsWhereShipToParty.Count);

            var order3 = new SalesOrderBuilder(this.DatabaseSession)
                .WithStore(new Stores(this.DatabaseSession).FindBy(M.Store.Name, "second store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order3Item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order3.AddSalesOrderItem(order3Item);

            this.DatabaseSession.Derive();

            order3.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(2, customer.ShipmentsWhereShipToParty.Count);
            Assert.Equal(1, customer.PickListsWhereShipToParty.Count);
        }

        [Fact]
        public void GivenCustomerShipment_WhenDeriving_ThenBillFromContactMechanismMustExist()
        {
            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var address1 = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("internalOrganisation")
                .WithDefaultPaymentMethod(ownBankAccount)
                .Build();

            internalOrganisation.BillingAddress = address1;

            this.DatabaseSession.Derive();

            var shipment1 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(new PostalAddresses(this.DatabaseSession).Extent().First)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Boat)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(address1, shipment1.BillFromContactMechanism);
        }

        [Fact]
        public void GivenCustomerShipment_WhenStateIsSetToShipped_ThenInvoiceIsCreated()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPartyContactMechanism(billToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            shipment.Ship();

            this.DatabaseSession.Derive();

            var salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice = salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;

            Assert.NotNull(invoice);
        }

        [Fact]
        public void GivenCustomerShipmentWithPendingPickList_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            shipment.Ship();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Packed, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentContainingOrderOnHold_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            order.Hold();

            this.DatabaseSession.Derive();

            shipment.Ship();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Packed, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentWithQuantityPackagedDifferentFromShippingQuantity_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");
            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            item.QuantityOrdered = 4;

            this.DatabaseSession.Derive();

            shipment.Ship();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Picked, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenCustomerShipmentWithValueBelowThreshold_WhenShippingToBelgium_TheninvoiceIncludesCosts()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            new ShippingAndHandlingComponentBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeographicBoundary(mechelen)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithShipmentValue(new ShipmentValueBuilder(this.DatabaseSession).WithThroughAmount(300M).Build())
                .WithCost(15M)
                .Build();

            new ShippingAndHandlingComponentBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithShipmentValue(new ShipmentValueBuilder(this.DatabaseSession).WithThroughAmount(300M).Build())
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithCost(20M)
                .Build();

            new ShippingAndHandlingComponentBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithShipmentValue(new ShipmentValueBuilder(this.DatabaseSession).WithThroughAmount(300M).Build())
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).FirstClassAir)
                .WithCost(50M)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var billToContactMechanismMechelen = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Mechelen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanismMechelen)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            shipment.Ship();

            this.DatabaseSession.Derive();

            var invoice = customer.SalesInvoicesWhereBillToCustomer.First;
            Assert.Equal(15M, invoice.ShippingAndHandlingCharge.Amount);
        }
    }
}
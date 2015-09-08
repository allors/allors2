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
    using System.Security.Principal;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class CustomerShipmentTests : DomainTest
    {
        [Test]
        public void GivenCustomerShipment_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Created, shipment.CurrentObjectState);
            Assert.AreEqual(shipment.LastObjectState, shipment.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipment_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsNull(shipment.PreviousObjectState);
        }

        [Test]
        public void GivenCustomerShipment_WhenCancelled_ThenCurrentShipmentStatusEqualsCancelled()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, shipment.ShipmentStatuses.Count);
            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Created, shipment.CurrentShipmentStatus.CustomerShipmentObjectState);

            shipment.Cancel();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, shipment.ShipmentStatuses.Count);
            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Cancelled, shipment.CurrentShipmentStatus.CustomerShipmentObjectState);
        }

        [Test]
        public void GivenCustomerShipmentBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipFromAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var partyContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(shipFromAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.AddPartyContactMechanism(partyContactMechanism);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(new PostalAddresses(this.DatabaseSession).Extent().First)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Boat)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Created, shipment.CurrentObjectState);
            Assert.AreEqual(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"), shipment.ShipFromParty);
            Assert.AreEqual(shipFromAddress, shipment.ShipFromAddress);
            Assert.AreEqual(shipment.ShipFromParty, shipment.ShipFromParty);
            Assert.AreEqual(new Stores(this.DatabaseSession).FindBy(Stores.Meta.Name, "store"), shipment.Store);
        }

        [Test]
        public void GivenCustomerShipment_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();

            this.DatabaseSession.Commit();

            var builder = new CustomerShipmentBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithShipToParty(customer);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCarrier(new Carriers(this.DatabaseSession).Fedex);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithShipToAddress(shipToAddress);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenCustomerShipment_WhenGettingShipmentNumberWithoutFormat_ThenShipmentNumberShouldBeReturned()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"))
                .WithOwner(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment1 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.AreEqual("1", shipment1.ShipmentNumber);

            var shipment2 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build())
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.AreEqual("2", shipment2.ShipmentNumber);
        }

        [Test]
        public void GivenCustomerShipment_WhenGettingShipmentNumberWithFormat_ThenFormattedShipmentNumberShouldBeReturned()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"))
                .WithOwner(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithOutgoingShipmentNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var shipment1 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.AreEqual("the format is 1", shipment1.ShipmentNumber);

            var shipment2 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build())
                .WithShipToAddress(shipToAddress)
                .WithStore(store)
                .WithShipmentMethod(store.DefaultShipmentMethod)
                .Build();

            Assert.AreEqual("the format is 2", shipment2.ShipmentNumber);
        }

        [Test]
        public void GivenCustomerShipmentWithShipToCustomerWithShippingAddress_WhenDeriving_ThenShipToAddressMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

            var shippingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(shipToAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(shippingAddress);

            this.DatabaseSession.Derive(true);

            var customerShipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(shippingAddress.ContactMechanism, customerShipment.ShipToAddress);
        }

        [Test]
        public void GivenCustomerShipmentCreatedByOrderProcessor_WhenCurrentUserInSameOrderProcessorUserGroup_ThenAccessIsGranted()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var orderProcessor2 = new PersonBuilder(this.DatabaseSession).WithLastName("orderProcessor2").WithUserName("orderProcessor2").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var usergroups = internalOrganisation.UserGroupsWhereParty;
            var orderProcessorUserGroup = usergroups.First;

            new EmploymentBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithEmployee(orderProcessor2)
                .WithEmployer(internalOrganisation)
                .Build();

            orderProcessorUserGroup.AddMember(orderProcessor2);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Boat)
                .Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(shipment, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(CustomerShipments.Meta.ShipToParty));
            Assert.IsTrue(acl.CanRead(CustomerShipments.Meta.ShipToParty));
            Assert.IsTrue(acl.CanExecute(CustomerShipments.Meta.Cancel));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor2", "Forms"), new string[0]);
            acl = new AccessControlList(shipment, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(CustomerShipments.Meta.ShipToParty));
            Assert.IsTrue(acl.CanRead(CustomerShipments.Meta.ShipToParty));
            Assert.IsTrue(acl.CanExecute(CustomerShipments.Meta.Cancel));
        }

        [Test]
        public void GivenCustomerShipmentCreatedByOrderProcessor_WhenCurrentUserIsCustomerContact_ThenReadAccessIsGranted()
        {
            var customerContact = new PersonBuilder(this.DatabaseSession).WithUserName("customercontact").WithLastName("customercontact").Build();
            var employee = new PersonBuilder(this.DatabaseSession).WithUserName("employee").WithLastName("employee").Build();
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();

            new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(employee)
                .WithEmployer(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);
            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Boat)
                .Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(shipment, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(CustomerShipments.Meta.ShipToParty));
            Assert.IsTrue(acl.CanRead(CustomerShipments.Meta.ShipToParty));
            Assert.IsTrue(acl.CanExecute(CustomerShipments.Meta.Cancel));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact", "Forms"), new string[0]);
            acl = new AccessControlList(shipment, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.CanWrite(CustomerShipments.Meta.ShipToParty));
            Assert.IsTrue(acl.CanRead(CustomerShipments.Meta.ShipToParty));
            Assert.IsFalse(acl.CanExecute(CustomerShipments.Meta.Cancel));
        }

        [Test]
        public void GivenCustomerShipment_WhenObjectStateIsCreated_ThenCheckTransitions()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(shipment, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(CustomerShipments.Meta.Cancel));
        }

        [Test]
        public void GivenCustomerShipment_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();

            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            shipment.Cancel();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(shipment, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(CustomerShipments.Meta.Cancel));
        }

        [Test]
        public void GivenCustomerShipment_WhenObjectStateIsShipped_ThenCheckTransitions()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
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

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPartyContactMechanism(billToMechelen).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            var acl = new AccessControlList(shipment, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Shipped, shipment.CurrentObjectState);
            Assert.IsFalse(acl.CanExecute(CustomerShipments.Meta.Cancel));
            Assert.IsFalse(acl.CanWrite(CustomerShipments.Meta.HandlingInstruction));
        }

        [Test]
        public void GivenCustomerShipment_WhenAllItemsArePutIntoShipmentPackages_ThenCustomerShipmentStateIsPacked()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var good1InventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var good2InventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive(true);

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

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];
            customer.PickListsWhereShipToParty.First.SetPicked();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Packed, shipment.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipment_WhenAddingAndRemovingPackages_ThenPackageSequenceNumberIsRecalculated()
        {
            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToAddress(new PostalAddresses(this.DatabaseSession).Extent().First)
                .WithShipToParty(new Organisations(this.DatabaseSession).Extent().First)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Boat)
                .Build();

            var package1 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, package1.SequenceNumber);

            var package2 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package2);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, package2.SequenceNumber);

            var package3 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package3);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, package3.SequenceNumber);

            shipment.RemoveShipmentPackage(package1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, package2.SequenceNumber);
            Assert.AreEqual(3, package3.SequenceNumber);

            var package4 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package4);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, package2.SequenceNumber);
            Assert.AreEqual(3, package3.SequenceNumber);
            Assert.AreEqual(4, package4.SequenceNumber);

            shipment.RemoveShipmentPackage(package4);

            this.DatabaseSession.Derive(true);

            var package5 = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package5);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, package2.SequenceNumber);
            Assert.AreEqual(3, package3.SequenceNumber);
            Assert.AreEqual(4, package5.SequenceNumber);
        }

        [Test]
        public void GivenCustomerShipment_WhenDeriving_ThenTotalShipmentValueIsCalculated()
        {
            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.StoresWhereOwner.First.ShipmentThreshold = 100;

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1InventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10202")
                .WithName("good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2InventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Theft).Build());

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
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.AreEqual(75, shipment.ShipmentValue);

            item1.QuantityOrdered = 3;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(45, shipment.ShipmentValue);

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(10).Build();
            order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(145, shipment.ShipmentValue);
        }

        [Test]
        public void GivenShipmentThreshold_WhenNewCustomerShipmentIsBelowThreshold_ThenShipmentAndPicklistAreSetOnHold()
        {
            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.StoresWhereOwner.First.ShipmentThreshold = 100;

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).OnHold, shipment.CurrentObjectState);
            Assert.AreEqual(new PickListObjectStates(this.DatabaseSession).OnHold, pickList.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipment_WhenShipmentValueFallsBelowThreshold_ThenShipmentAndPendigPicklistAreSetOnHold()
        {
            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.StoresWhereOwner.First.ShipmentThreshold = 100;

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Created, shipment.CurrentObjectState);
            Assert.AreEqual(new PickListObjectStates(this.DatabaseSession).Created, pickList.CurrentObjectState);

            item.QuantityOrdered = 5;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).OnHold, shipment.CurrentObjectState);
            Assert.AreEqual(new PickListObjectStates(this.DatabaseSession).OnHold, pickList.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipmentOnHold_WhenShipmentValueRisesAboveThreshold_ThenShipmentAndPendigPicklistAreReleased()
        {
            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.StoresWhereOwner.First.ShipmentThreshold = 100;

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).OnHold, shipment.CurrentObjectState);
            Assert.AreEqual(new PickListObjectStates(this.DatabaseSession).OnHold, pickList.CurrentObjectState);

            item.QuantityOrdered = 10;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Created, shipment.CurrentObjectState);
            Assert.AreEqual(new PickListObjectStates(this.DatabaseSession).Created, pickList.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipmentOnHold_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Packed, shipment.CurrentObjectState);

            shipment.Hold();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).OnHold, shipment.CurrentObjectState);

            shipment.Ship();
            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).OnHold, shipment.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipmentOnHoldWithPendingPickList_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToCreated()
        {
            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.StoresWhereOwner.First.ShipmentThreshold = 100;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).OnHold, shipment.CurrentObjectState);

            shipment.ReleasedManually = true;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Created, shipment.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipmentOnHoldWithPickedPickList_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToPicked()
        {
            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.StoresWhereOwner.First.ShipmentThreshold = 100;

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(10).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Picked, shipment.CurrentObjectState);

            item.QuantityOrdered = 1;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).OnHold, shipment.CurrentObjectState);

            shipment.Continue();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Picked, shipment.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipmentOnHoldWithAllItemsPacked_WhenShipmentIsReleased_ThenShipmentObjecStateIsSetToPacked()
        {
            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.StoresWhereOwner.First.ShipmentThreshold = 100;

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");
            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).OnHold, shipment.CurrentObjectState);

            shipment.Continue();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Packed, shipment.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerBuyingFromDifferentStores_WhenShipping_ThenDifferentShipmentIsCreatedForEachStore()
        {
            new StoreBuilder(this.DatabaseSession).WithName("second store")
                .WithDefaultFacility(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.FacilitiesWhereOwner.First)
                .WithOwner(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .WithSalesOrderNumberPrefix("")
                .WithOutgoingShipmentNumberPrefix("")
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .WithStore(new Stores(this.DatabaseSession).FindBy(Stores.Meta.Name, "store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order1Item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order1.AddSalesOrderItem(order1Item);

            this.DatabaseSession.Derive(true);

            order1.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, customer.ShipmentsWhereShipToParty.Count);
            Assert.AreEqual(1, customer.PickListsWhereShipToParty.Count);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithStore(new Stores(this.DatabaseSession).FindBy(Stores.Meta.Name, "store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order2Item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order2.AddSalesOrderItem(order2Item);

            this.DatabaseSession.Derive(true);

            order2.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, customer.ShipmentsWhereShipToParty.Count);
            Assert.AreEqual(1, customer.PickListsWhereShipToParty.Count);

            var order3 = new SalesOrderBuilder(this.DatabaseSession)
                .WithStore(new Stores(this.DatabaseSession).FindBy(Stores.Meta.Name, "second store"))
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var order3Item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order3.AddSalesOrderItem(order3Item);

            this.DatabaseSession.Derive(true);

            order3.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, customer.ShipmentsWhereShipToParty.Count);
            Assert.AreEqual(1, customer.PickListsWhereShipToParty.Count);
        }

        [Test]
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
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var address1 = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var billingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(address1)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithName("internalOrganisation")
                .WithPreferredCurrency(euro)
                .WithDefaultPaymentMethod(ownBankAccount)
                .Build();

            internalOrganisation.AddPartyContactMechanism(billingAddress);

            this.DatabaseSession.Derive(true);

            var shipment1 = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithBillFromInternalOrganisation(internalOrganisation)
                .WithShipToParty(customer)
                .WithShipToAddress(new PostalAddresses(this.DatabaseSession).Extent().First)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Boat)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(address1, shipment1.BillFromContactMechanism);
        }

        [Test]
        public void GivenCustomerShipmentCreatedByOrderProcessor_WhenCurrentUserInAnotherOrderProcessorUserGroup_ThenAccessIsDenied()
        {
            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var orderProcessor2 = new PersonBuilder(this.DatabaseSession).WithLastName("orderProcessor2").WithUserName("orderProcessor2").Build();

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("employer2")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Operations)
                .WithPreferredCurrency(euro)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithPartyContactMechanism(billToMechelen)
                .Build();

            new EmploymentBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithEmployee(orderProcessor2)
                .WithEmployer(internalOrganisation)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var usergroups = internalOrganisation.UserGroupsWhereParty;
            usergroups.Filter.AddEquals(UserGroups.Meta.Parent, new Roles(this.DatabaseSession).Operations.UserGroupWhereRole);
            var orderProcessorUserGroup = usergroups.First;

            orderProcessorUserGroup.AddMember(orderProcessor2);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var shipment = new CustomerShipmentBuilder(this.DatabaseSession)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Boat)
                .Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(shipment, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(CustomerShipments.Meta.ShipToParty));
            Assert.IsTrue(acl.CanRead(CustomerShipments.Meta.ShipToParty));
            Assert.IsTrue(acl.CanExecute(CustomerShipments.Meta.Cancel));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor2", "Forms"), new string[0]);
            acl = new AccessControlList(shipment, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);
        }

        [Test]
        public void GivenCustomerShipment_WhenStateIsSetToShipped_ThenInvoiceIsCreated()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
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

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPartyContactMechanism(billToMechelen).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            this.DatabaseSession.Derive(true);

            var salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice = salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;

            Assert.IsNotNull(invoice);
        }

        [Test]
        public void GivenCustomerShipmentWithPendingPickList_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Packed, shipment.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipmentContainingOrderOnHold_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            order.Hold();

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Packed, shipment.CurrentObjectState);
        }

        [Test]
        public void GivenCustomerShipmentWithQuantityPackagedDifferentFromShippingQuantity_WhenTrySetStateToShipped_ThenActionIsNotAllowed()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate21;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21).Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");
            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            item.QuantityOrdered = 4;

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Picked, shipment.CurrentObjectState);
        }

        [Test]
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

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(vatRate21)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var billToContactMechanismMechelen = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Mechelen").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanismMechelen)
                .WithShipToCustomer(customer)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            this.DatabaseSession.Derive(true);

            var invoice = customer.SalesInvoicesWhereBillToCustomer.First;
            Assert.AreEqual(15M, invoice.ShippingAndHandlingCharge.Amount);
        }
    }
}
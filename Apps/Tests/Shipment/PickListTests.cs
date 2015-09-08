//------------------------------------------------------------------------------------------------- 
// <copyright file="PickListTests.cs" company="Allors bvba">
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

using System.Security.Principal;
using System.Threading;

namespace Allors.Domain
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class PickListTests : DomainTest
    {
        [Test]
        public void GivenPickListBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PickListObjectStates(this.DatabaseSession).Created, pickList.CurrentObjectState);
            Assert.AreEqual(pickList.CurrentPickListStatus.StartDateTime.Date, pickList.CreationDate.Date);
        }

        [Test]
        public void GivenPickListCreatedByOrderProcessor_WhenCurrentUserInSameOrderProcessorUserGroup_ThenAccessIsGranted()
        {
            var orderProcessor2 = new PersonBuilder(this.DatabaseSession).WithLastName("orderProcessor2").WithUserName("orderProcessor2").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var usergroups = internalOrganisation.UserGroupsWhereParty;
            usergroups.Filter.AddEquals(UserGroups.Meta.Parent, new Roles(this.DatabaseSession).Operations.UserGroupWhereRole);
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
            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(pickList, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(PickLists.Meta.Picker));
            Assert.IsTrue(acl.CanRead(PickLists.Meta.Picker));
            Assert.IsTrue(acl.CanExecute(PickLists.Meta.Cancel));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor2", "Forms"), new string[0]);
            acl = new AccessControlList(pickList, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(PickLists.Meta.Picker));
            Assert.IsTrue(acl.CanRead(PickLists.Meta.Picker));
            Assert.IsTrue(acl.CanExecute(PickLists.Meta.Cancel));
        }

        [Test]
        public void GivenPickList_WhenObjectStateIsCreated_ThenCheckTransitions()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(pickList, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(PickLists.Meta.Cancel));
        }

        [Test]
        public void GivenPickList_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive(true);

            pickList.Cancel();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(pickList, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(PickLists.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(PickLists.Meta.SetPicked));
        }

        [Test]
        public void GivenPickList_WhenObjectStateIsPicked_ThenCheckTransitions()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor", "Forms"), new string[0]);

            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive(true);

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(pickList, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(PickLists.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(PickLists.Meta.SetPicked));
        }

        [Test]
        public void GivenPickList_WhenPicked_ThenInventoryIsAdjustedAndOrderItemsQuantityPickedIsSet()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithProductPurchasePrice(good1PurchasePrice)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithProductPurchasePrice(good2PurchasePrice)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            var good1Inventory = (NonSerializedInventoryItem)good1.InventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var good2Inventory = (NonSerializedInventoryItem)good2.InventoryItemsWhereGood[0];
            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var colorWhite = new ColourBuilder(this.DatabaseSession)
                .WithName("white")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                            .WithText("white")
                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                            .Build())

                .Build();
            var extraLarge = new SizeBuilder(this.DatabaseSession)
                .WithName("extra large")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                            .WithText("Extra large")
                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                            .Build())
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProductFeature(colorWhite).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProductFeature(extraLarge).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            item1.AddOrderedWithFeature(item2);
            item1.AddOrderedWithFeature(item3);
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item5 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);
            order.AddSalesOrderItem(item5);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];

            var pickList = good1.InventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            //// item5: only 4 out of 5 are actually picked
            foreach (PickListItem pickListItem in pickList.PickListItems)
            {
                if (pickListItem.RequestedQuantity == 5)
                {
                    pickListItem.ActualQuantity = 4;
                }
            }

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            //// all orderitems have same physical finished good, so there is only one picklist item.
            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Picked, shipment.CurrentShipmentStatus.CustomerShipmentObjectState);
            Assert.AreEqual(new PickListObjectStates(this.DatabaseSession).Picked, pickList.CurrentPickListStatus.PickListObjectState);
            Assert.AreEqual(1, item1.QuantityPicked);
            Assert.AreEqual(0, item1.QuantityReserved);
            Assert.AreEqual(0, item1.QuantityRequestsShipping);
            Assert.AreEqual(2, item4.QuantityPicked);
            Assert.AreEqual(0, item4.QuantityReserved);
            Assert.AreEqual(0, item4.QuantityRequestsShipping);
            Assert.AreEqual(4, item5.QuantityPicked);
            Assert.AreEqual(1, item5.QuantityReserved);
            Assert.AreEqual(0, item5.QuantityRequestsShipping);
            Assert.AreEqual(97, good1Inventory.QuantityOnHand);
            Assert.AreEqual(0, good1Inventory.QuantityCommittedOut);
            Assert.AreEqual(96, good2Inventory.QuantityOnHand);
            Assert.AreEqual(1, good2Inventory.QuantityCommittedOut);
        }

        [Test]
        public void GivenPickList_WhenActualQuantityPickedIsLess_ThenShipmentItemQuantityIsAdjusted()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithProductPurchasePrice(good1PurchasePrice)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithProductPurchasePrice(good2PurchasePrice)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            var good1Inventory = (NonSerializedInventoryItem)good1.InventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var good2Inventory = (NonSerializedInventoryItem)good2.InventoryItemsWhereGood[0];
            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

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

            var pickList = good1.InventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            //// item3: only 4 out of 5 are actually picked
            PickListItem adjustedPicklistItem = null;
            foreach (PickListItem pickListItem in pickList.PickListItems)
            {
                if (pickListItem.RequestedQuantity == 5)
                {
                    adjustedPicklistItem = pickListItem;
                }
            }

            var itemIssuance = adjustedPicklistItem.ItemIssuancesWherePickListItem[0];
            var shipmentItem = adjustedPicklistItem.ItemIssuancesWherePickListItem[0].ShipmentItem;

            Assert.AreEqual(5, itemIssuance.Quantity);
            Assert.AreEqual(5, shipmentItem.Quantity);

            adjustedPicklistItem.ActualQuantity = 4;

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(4, itemIssuance.Quantity);
            Assert.AreEqual(4, shipmentItem.Quantity);
        }

        [Test]
        public void GivenSalesOrder_WhenShipmentIsCreated_ThenOrdertemsAreAddedToPickList()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithProductPurchasePrice(good1PurchasePrice)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithProductPurchasePrice(good2PurchasePrice)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            var good1Inventory = (NonSerializedInventoryItem)good1.InventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive(true);

            var good2Inventory = (NonSerializedInventoryItem)good2.InventoryItemsWhereGood[0];
            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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

            var pickList = good1.InventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;

            Assert.AreEqual(2, pickList.PickListItems.Count);

            var extent1 = pickList.PickListItems;
            extent1.Filter.AddEquals(PickListItems.Meta.InventoryItem, good1Inventory);
            Assert.AreEqual(3, extent1.First.RequestedQuantity);

            var extent2 = pickList.PickListItems;
            extent2.Filter.AddEquals(PickListItems.Meta.InventoryItem, good2Inventory);
            Assert.AreEqual(5, extent2.First.RequestedQuantity);
        }

        [Test]
        public void GivenMultipleOrders_WhenCombinedPickListIsPicked_ThenSingleShipmentIsPickedState()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithProductPurchasePrice(good1PurchasePrice)
                .WithFromDate(DateTime.UtcNow)
                .WithSupplier(supplier)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithProductPurchasePrice(good2PurchasePrice)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            var good1Inventory = (NonSerializedInventoryItem)good1.InventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive(true);

            var good2Inventory = (NonSerializedInventoryItem)good2.InventoryItemsWhereGood[0];
            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive(true);

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order1.AddSalesOrderItem(item1);
            order1.AddSalesOrderItem(item2);
            order1.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive(true);

            order1.Confirm();

            this.DatabaseSession.Derive(true);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var itema = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var itemb = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order2.AddSalesOrderItem(itema);
            order2.AddSalesOrderItem(itemb);

            this.DatabaseSession.Derive(true);

            order2.Confirm();

            this.DatabaseSession.Derive(true);

            var pickList = good1.InventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, customer.ShipmentsWhereBillToParty.Count);

            var customerShipment = customer.ShipmentsWhereBillToParty.First;
            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Picked, customerShipment.CurrentObjectState);
        }

        [Test]
        public void GivenPickListCreatedByOrderProcessor_WhenCurrentUserInAnotherOrderProcessorUserGroup_ThenAccessIsDenied()
        {
            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var orderProcessor2 = new PersonBuilder(this.DatabaseSession).WithLastName("orderProcessor2").WithUserName("orderProcessor2").Build();
            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("employer2")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Operations)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithPreferredCurrency(euro)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var usergroups = internalOrganisation.UserGroupsWhereParty;
            usergroups.Filter.AddEquals(UserGroups.Meta.Parent, new Roles(this.DatabaseSession).Operations.UserGroupWhereRole);
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
            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(pickList, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(PickLists.Meta.Picker));
            Assert.IsTrue(acl.CanRead(PickLists.Meta.Picker));
            Assert.IsTrue(acl.CanExecute(PickLists.Meta.Cancel));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("orderProcessor2", "Forms"), new string[0]);
            acl = new AccessControlList(pickList, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);
        }
    }
}
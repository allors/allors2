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
    using Meta;
    using Xunit;

    
    public class PickListTests : DomainTest
    {
        [Fact]
        public void GivenPickListBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new PickListObjectStates(this.DatabaseSession).Created, pickList.CurrentObjectState);
            Assert.Equal(pickList.CurrentPickListStatus.StartDateTime.Date, pickList.CreationDate.Date);
        }

        [Fact]
        public void GivenPickList_WhenObjectStateIsCreated_ThenCheckTransitions()
        {
            this.SetIdentity("orderProcessor");

            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive();

            var acl = new AccessControlList(pickList, new Users(this.DatabaseSession).CurrentUser);
            Assert.True(acl.CanExecute(M.PickList.Cancel));
        }

        [Fact]
        public void GivenPickList_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            this.SetIdentity("orderProcessor");

            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive();

            pickList.Cancel();

            this.DatabaseSession.Derive();

            var acl = new AccessControlList(pickList, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.PickList.Cancel));
            Assert.False(acl.CanExecute(M.PickList.SetPicked));
        }

        [Fact]
        public void GivenPickList_WhenObjectStateIsPicked_ThenCheckTransitions()
        {
            this.SetIdentity("orderProcessor");

            var pickList = new PickListBuilder(this.DatabaseSession).Build();

            this.DatabaseSession.Derive();

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            var acl = new AccessControlList(pickList, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.PickList.Cancel));
            Assert.False(acl.CanExecute(M.PickList.SetPicked));
        }

        [Fact]
        public void GivenPickList_WhenPicked_ThenInventoryIsAdjustedAndOrderItemsQuantityPickedIsSet()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation"))
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
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

            this.DatabaseSession.Derive();

            var good1Inventory = (NonSerialisedInventoryItem)good1.IInventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var good2Inventory = (NonSerialisedInventoryItem)good2.IInventoryItemsWhereGood[0];
            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

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
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
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

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];

            var pickList = good1.IInventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            //// item5: only 4 out of 5 are actually picked
            foreach (PickListItem pickListItem in pickList.PickListItems)
            {
                if (pickListItem.RequestedQuantity == 5)
                {
                    pickListItem.ActualQuantity = 4;
                }
            }

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            //// all orderitems have same physical finished good, so there is only one picklist item.
            Assert.Equal(new CustomerShipmentObjectStates(this.DatabaseSession).Picked, shipment.CurrentShipmentStatus.CustomerShipmentObjectState);
            Assert.Equal(new PickListObjectStates(this.DatabaseSession).Picked, pickList.CurrentPickListStatus.PickListObjectState);
            Assert.Equal(1, item1.QuantityPicked);
            Assert.Equal(0, item1.QuantityReserved);
            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(2, item4.QuantityPicked);
            Assert.Equal(0, item4.QuantityReserved);
            Assert.Equal(0, item4.QuantityRequestsShipping);
            Assert.Equal(4, item5.QuantityPicked);
            Assert.Equal(1, item5.QuantityReserved);
            Assert.Equal(0, item5.QuantityRequestsShipping);
            Assert.Equal(97, good1Inventory.QuantityOnHand);
            Assert.Equal(0, good1Inventory.QuantityCommittedOut);
            Assert.Equal(96, good2Inventory.QuantityOnHand);
            Assert.Equal(1, good2Inventory.QuantityCommittedOut);
        }

        [Fact]
        public void GivenPickList_WhenActualQuantityPickedIsLess_ThenShipmentItemQuantityIsAdjusted()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation"))
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
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

            this.DatabaseSession.Derive();

            var good1Inventory = (NonSerialisedInventoryItem)good1.IInventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var good2Inventory = (NonSerialisedInventoryItem)good2.IInventoryItemsWhereGood[0];
            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

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

            var pickList = good1.IInventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

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

            Assert.Equal(5, itemIssuance.Quantity);
            Assert.Equal(5, shipmentItem.Quantity);

            adjustedPicklistItem.ActualQuantity = 4;

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            Assert.Equal(4, itemIssuance.Quantity);
            Assert.Equal(4, shipmentItem.Quantity);
        }

        [Fact]
        public void GivenSalesOrder_WhenShipmentIsCreated_ThenOrdertemsAreAddedToPickList()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation"))
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
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

            this.DatabaseSession.Derive();

            var good1Inventory = (NonSerialisedInventoryItem)good1.IInventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive();

            var good2Inventory = (NonSerialisedInventoryItem)good2.IInventoryItemsWhereGood[0];
            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

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

            var pickList = good1.IInventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;

            Assert.Equal(2, pickList.PickListItems.Count);

            var extent1 = pickList.PickListItems;
            extent1.Filter.AddEquals(M.PickListItem.InventoryItem, good1Inventory);
            Assert.Equal(3, extent1.First.RequestedQuantity);

            var extent2 = pickList.PickListItems;
            extent2.Filter.AddEquals(M.PickListItem.InventoryItem, good2Inventory);
            Assert.Equal(5, extent2.First.RequestedQuantity);
        }

        [Fact]
        public void GivenMultipleOrders_WhenCombinedPickListIsPicked_ThenSingleShipmentIsPickedState()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation"))
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR"))
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

            this.DatabaseSession.Derive();

            var good1Inventory = (NonSerialisedInventoryItem)good1.IInventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive();

            var good2Inventory = (NonSerialisedInventoryItem)good2.IInventoryItemsWhereGood[0];
            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Ruined).Build());

            this.DatabaseSession.Derive();

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

            this.DatabaseSession.Derive();

            order1.Confirm();

            this.DatabaseSession.Derive();

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var itema = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var itemb = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            order2.AddSalesOrderItem(itema);
            order2.AddSalesOrderItem(itemb);

            this.DatabaseSession.Derive();

            order2.Confirm();

            this.DatabaseSession.Derive();

            var pickList = good1.IInventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            Assert.Equal(1, customer.ShipmentsWhereBillToParty.Count);

            var customerShipment = customer.ShipmentsWhereBillToParty.First;
            Assert.Equal(new CustomerShipmentObjectStates(this.DatabaseSession).Picked, customerShipment.CurrentObjectState);
        }
    }
}
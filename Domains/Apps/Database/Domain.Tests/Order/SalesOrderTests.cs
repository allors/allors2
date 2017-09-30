//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesOrderTests.cs" company="Allors bvba">
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

    public class SalesOrderTests : DomainTest
    {
        [Fact]
        public void GivenSalesOrderBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).Provisional, order.SalesOrderState);
            Assert.True(order.PartiallyShip);
            Assert.Equal(DateTime.UtcNow.Date, order.OrderDate.Date);
            Assert.Equal(DateTime.UtcNow.Date, order.EntryDate.Date);
            Assert.Equal(order.PreviousBillToCustomer, order.BillToCustomer);
            Assert.Equal(order.PreviousShipToCustomer, order.ShipToCustomer);
            Assert.Equal(order.VatRegime, order.BillToCustomer.VatRegime);
            Assert.Equal(new Stores(this.DatabaseSession).FindBy(M.Store.Name, "store"), order.Store);
            Assert.Equal(order.Store.DefaultPaymentMethod, order.PaymentMethod);
            Assert.Equal(order.Store.DefaultShipmentMethod, order.ShipmentMethod);
        }

        [Fact]
        public void GivenSalesOrderForItemsThatAreAvailable_WhenShipped_ThenOrderIsCompleted()
        {
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
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var good2InventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
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

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).Completed, order.SalesOrderState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Completed, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Completed, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Completed, item3.SalesOrderItemState);
        }
        [Fact]
        public void GivenSalesOrderShippedInMultipleParts_WhenPaymentsAreReceived_ThenObjectStateCorrespondingSalesOrderIsUpdated()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var good1Inventory = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(1).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var good2Inventory = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();

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
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).WithComment("item1").Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).WithComment("item2").Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).WithComment("item3").Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

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

            shipment.SetPacked();
            this.DatabaseSession.Derive();

            shipment.Ship();
            this.DatabaseSession.Derive();

            var salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice1 = (SalesInvoice)salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;
            invoice1.Send();

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(15)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(15).Build())
                .WithEffectiveDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).InProcess, order.SalesOrderState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Finished, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).InProcess, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).InProcess, item3.SalesOrderItemState);

            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            shipment = (CustomerShipment)item2.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            shipment.Ship();

            this.DatabaseSession.Derive();

            salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice2 = (SalesInvoice)salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;
            invoice2.Send();

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(30)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice2.SalesInvoiceItems[0]).WithAmountApplied(30).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).InProcess, order.SalesOrderState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Finished, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Finished, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).InProcess, item3.SalesOrderItemState);

            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            shipment = (CustomerShipment)item3.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive();

            package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive();

            shipment.Ship();

            this.DatabaseSession.Derive();

            salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice3 = salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(75)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice3.SalesInvoiceItems[0]).WithAmountApplied(75).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).Finished, order.SalesOrderState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Finished, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Finished, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Finished, item3.SalesOrderItemState);
        }

        [Fact]
        public void GivenPendingShipmentAndAssignedPickList_WhenNewOrderIsConfirmed_ThenNewPickListIsCreatedAndSingleOrderShipmentIsUpdated()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order1.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(3, shipment.ShipmentItems[0].Quantity);

            var pickList1 = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(3, pickList1.PickListItems[0].RequestedQuantity);

            pickList1.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            this.DatabaseSession.Derive();

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(2)
                .WithActualUnitPrice(5)
                .Build();

            order2.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order2.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(5, shipment.ShipmentItems.First.Quantity);

            var pickList2 = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[1].PickListItem.PickListWherePickListItem;
            Assert.Equal(2, pickList2.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenSalesOrderWithPendingShipmentAndAssignedPickList_WhenOrderIsCancelled_ThenNegativePickListIsCreatedAndSingleOrderShipmentIsCancelled()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

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
                .WithShipToAddress(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            pickList.Picker = new People(this.DatabaseSession).FindBy(M.Person.LastName, "orderProcessor");

            this.DatabaseSession.Derive();

            order.Cancel();

            this.DatabaseSession.Derive();

            var negativePickList = order.ShipToCustomer.PickListsWhereShipToParty[1];
            Assert.Equal(-10, negativePickList.PickListItems[0].RequestedQuantity);

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Cancelled, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenSalesOrderWithPickList_WhenOrderIsCancelled_ThenPickListIsCancelledAndSingleOrderShipmentIsCancelled()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

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

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order1.Confirm();

            this.DatabaseSession.Derive();

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
                .Build();

            order2.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order2.Confirm();

            this.DatabaseSession.Derive();

            var shipment = (CustomerShipment)customer.ShipmentsWhereBillToParty[0];
            Assert.Equal(30, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(30, pickList.PickListItems[0].RequestedQuantity);

            order1.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Created, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.DatabaseSession).Created, pickList.PickListState);
            Assert.Equal(20, pickList.PickListItems[0].RequestedQuantity);

            order2.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(new CustomerShipmentStates(this.DatabaseSession).Cancelled, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.DatabaseSession).Cancelled, pickList.PickListState);
        }

        [Fact]
        public void GivenSalesOrderOnHold_WhenInventoryBecomesAvailable_ThenOrderIsNotSelectedForShipment()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventory = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();

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
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            order.Hold();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityShortFalled);

            inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderOnHold_WhenOrderIsContinued_ThenOrderIsSelectedForShipment()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventory = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();

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
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            order.Hold();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityShortFalled);

            inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);

            order.Continue();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).InProcess, order.SalesOrderState);
            Assert.Equal(10, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderNotPartiallyShipped_WhenInComplete_ThenOrderIsNotSelectedForShipment()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventoryGood1 = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryGood1.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("20202")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventoryGood2 = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            inventoryGood2.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

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
                .WithPartiallyShip(false)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.False(customer.ExistShipmentsWhereShipToParty);

            Assert.Equal(10, item1.QuantityRequestsShipping);
            Assert.Equal(0, item1.QuantityPendingShipment);
            Assert.Equal(10, item1.QuantityShortFalled);

            Assert.Equal(10, item2.QuantityRequestsShipping);
            Assert.Equal(0, item2.QuantityPendingShipment);
            Assert.Equal(10, item2.QuantityShortFalled);

            inventoryGood1.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            Assert.False(customer.ExistShipmentsWhereShipToParty);

            Assert.Equal(20, item1.QuantityRequestsShipping);
            Assert.Equal(0, item1.QuantityPendingShipment);
            Assert.Equal(0, item1.QuantityShortFalled);

            Assert.Equal(10, item2.QuantityRequestsShipping);
            Assert.Equal(0, item2.QuantityPendingShipment);
            Assert.Equal(10, item2.QuantityShortFalled);

            inventoryGood2.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            Assert.True(customer.ExistShipmentsWhereShipToParty);

            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(20, item1.QuantityPendingShipment);
            Assert.Equal(0, item1.QuantityShortFalled);

            Assert.Equal(0, item2.QuantityRequestsShipping);
            Assert.Equal(20, item2.QuantityPendingShipment);
            Assert.Equal(0, item2.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderForStoreExceedingCreditLimit_WhenOrderIsConfirmed_ThenOrderRequestsApproval()
        {
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

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

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                
                .Build();



            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddYears(-1))
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(10).WithActualUnitPrice(100).WithSalesInvoiceItemType(productItem).Build();
            invoice.AddSalesInvoiceItem(invoiceItem);

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).RequestsApproval, order.SalesOrderState);
            Assert.Equal(0, item.QuantityReserved);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);

            order.Approve();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).InProcess, order.SalesOrderState);
            Assert.Equal(10, item.QuantityReserved);
            Assert.Equal(10, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderForCustomerExceedingCreditLimit_WhenOrderIsConfirmed_ThenOrderRequestsApproval()
        {
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            customer.CreditLimit = 100M;

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddYears(-1))
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(10).WithActualUnitPrice(11).WithSalesInvoiceItemType(productItem).Build();
            invoice.AddSalesInvoiceItem(invoiceItem);

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).RequestsApproval, order.SalesOrderState);
            Assert.Equal(0, item.QuantityReserved);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);

            order.Approve();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).InProcess, order.SalesOrderState);
            Assert.Equal(10, item.QuantityReserved);
            Assert.Equal(10, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderBelowOrderThreshold_WhenOrderIsConfirmed_ThenOrderIsNotShipped()
        {
            new Stores(this.DatabaseSession).Extent().First.OrderThreshold = 1;

            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(1)
                .WithActualUnitPrice(0.1M)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).RequestsApproval, order.SalesOrderState);
        }

        [Fact]
        public void GivenSalesOrderWithManualShipmentSchedule_WhenOrderIsConfirmed_ThenInventoryIsNotReservedAndOrderIsNotShipped()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            var manual = new OrderKindBuilder(this.DatabaseSession).WithDescription("manual").WithScheduleManually(true).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithOrderKind(manual)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(50)
                .WithActualUnitPrice(50)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).InProcess, order.SalesOrderState);
            Assert.Equal(0, item.QuantityReserved);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);
            Assert.Equal(100, inventoryItem.QuantityOnHand);
            Assert.Equal(100, inventoryItem.AvailableToPromise);
        }

        [Fact]
        public void GivenConfirmedOrder_WhenOrderIsRejected_ThenNonSerialisedInventoryQuantitiesAreReleased()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(3, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(7, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);

            order.Reject();

            this.DatabaseSession.Derive();

            Assert.Equal(0, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(0, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);
        }

        [Fact]
        public void GivenConfirmedOrder_WhenOrderIsCancelled_ThenNonSerialisedInventoryQuantitiesAreReleased()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(3, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(7, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);

            order.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(0, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(0, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);
        }

        [Fact]
        public void GivenSalesOrder_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).Provisional, order.SalesOrderState);
            Assert.Equal(order.LastSalesOrderState, order.SalesOrderState);
        }

        [Fact]
        public void GivenSalesOrder_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Null(order.PreviousSalesOrderState);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirmed_ThenCurrentOrderStatusMustBeDerived()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).Provisional, order.SalesOrderState);

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).InProcess, order.SalesOrderState);
        }

        [Fact]
        public void GivenSalesOrderWithCancelledItem_WhenDeriving_ThenCancelledItemIsNotInValidOrderItems()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.DatabaseSession.Derive();

            item4.Cancel();

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Fact]
        public void GivenSalesOrderBuilder_WhenBuild_ThenOrderMustBeValid()
        {
            new SalesOrderBuilder(this.DatabaseSession).Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesOrder_WhenGettingOrderNumberWithoutFormat_ThenOrderNumberShouldBeReturned()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Facilities(this.DatabaseSession).FindBy(M.Facility.FacilityType, new FacilityTypes(this.DatabaseSession).Warehouse))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            this.DatabaseSession.Derive();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal("1", order1.OrderNumber);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal("2", order2.OrderNumber);
        }

        [Fact]
        public void GivenSalesOrder_WhenGettingOrderNumberWithFormat_ThenFormattedOrderNumberShouldBeReturned()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Facilities(this.DatabaseSession).FindBy(M.Facility.FacilityType, new FacilityTypes(this.DatabaseSession).Warehouse))
                .WithSalesOrderNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            this.DatabaseSession.Derive();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal("the format is 1", order1.OrderNumber);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal("the format is 2", order2.OrderNumber);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenDerivedSalesRepMustExist()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var salesrep = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                
                .WithFromDate(DateTime.UtcNow)
                .Build();



            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCustomer(customer)
                .WithSalesRepresentative(salesrep)
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            order.AddSalesOrderItem(new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(10).Build());

            this.DatabaseSession.Derive();

            Assert.Contains(salesrep, order.SalesReps);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenTakenByContactMechanismMustExist()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var orderContact = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("orders@acme.com").Build();

            internalOrganisation.OrderAddress = orderContact;

            this.DatabaseSession.Derive();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(orderContact, order1.TakenByContactMechanism);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenBillFromContactMechanismMustExist()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var billingContact = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("orders@acme.com").Build();

            internalOrganisation.BillingAddress = billingContact;

            this.DatabaseSession.Derive();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(billingContact, order1.BillFromContactMechanism);
        }

        [Fact]
        public void GivenSalesOrderWithBillToCustomerWithPreferredCurrency_WhenBuild_ThenCurrencyIsFromCustomer()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var englischLocale = new Locales(this.DatabaseSession).EnglishGreatBritain;
            var poundSterling = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "GBP");

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithLocale(englischLocale).WithPreferredCurrency(poundSterling).WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(poundSterling, order.CustomerCurrency);

            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            customer.PreferredCurrency = euro;

            this.DatabaseSession.Derive();

            Assert.Equal(englischLocale.Country.Currency, order.CustomerCurrency);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenLocaleMustExist()
        {
            var englischLocale = new Locales(this.DatabaseSession).EnglishGreatBritain;

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession).WithBillToCustomer(customer).WithShipToAddress(shipToContactMechanism).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(englischLocale, order.Locale);
        }

        [Fact]
        public void GivenSalesOrder_WhenObjectStateIsProvisional_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            this.SetIdentity(Users.AdministratorUserName);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).Provisional, order.SalesOrderState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).CurrentUser);
            Assert.True(acl.CanExecute(M.SalesOrder.Confirm));
            Assert.True(acl.CanExecute(M.SalesOrder.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrder.Approve));
            Assert.False(acl.CanExecute(M.SalesOrder.Reject));
            Assert.False(acl.CanExecute(M.SalesOrder.Hold));
            Assert.False(acl.CanExecute(M.SalesOrder.Continue));
        }

        [Fact]
        public void GivenSalesOrder_WhenObjectStateIsInProcess_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            this.SetIdentity(Users.AdministratorUserName);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var acl = new AccessControlList(order, new Users(this.DatabaseSession).CurrentUser);
            Assert.True(acl.CanExecute(M.SalesOrder.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrder.Reject));
            Assert.False(acl.CanExecute(M.SalesOrder.Approve));
            Assert.True(acl.CanExecute(M.SalesOrder.Hold));
            Assert.False(acl.CanExecute(M.SalesOrder.Confirm));
            Assert.False(acl.CanExecute(M.SalesOrder.Continue));
        }

        [Fact]
        public void GivenSalesOrder_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;



            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            this.SetIdentity("customer");

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            order.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).Cancelled, order.SalesOrderState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesOrder.Confirm));
            Assert.False(acl.CanExecute(M.SalesOrder.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrder.Reject));
            Assert.False(acl.CanExecute(M.SalesOrder.Approve));
            Assert.False(acl.CanExecute(M.SalesOrder.Continue));
            Assert.False(acl.CanExecute(M.SalesOrder.Hold));
        }

        [Fact]
        public void GivenSalesOrder_WhenObjectStateIsRejected_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            this.SetIdentity("customer");

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            order.Reject();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).Rejected, order.SalesOrderState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesOrder.Confirm));
            Assert.False(acl.CanExecute(M.SalesOrder.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrder.Reject));
            Assert.False(acl.CanExecute(M.SalesOrder.Approve));
            Assert.False(acl.CanExecute(M.SalesOrder.Continue));
            Assert.False(acl.CanExecute(M.SalesOrder.Hold));
        }

        [Fact]
        public void GivenSalesOrder_WhenObjectStateIsFinished_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            this.SetIdentity("customer");

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            order.SalesOrderState = new SalesOrderStates(this.DatabaseSession).Finished;

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).Finished, order.SalesOrderState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesOrder.Confirm));
            Assert.False(acl.CanExecute(M.SalesOrder.Cancel));
            Assert.False(acl.CanExecute(M.SalesOrder.Reject));
            Assert.False(acl.CanExecute(M.SalesOrder.Approve));
            Assert.False(acl.CanExecute(M.SalesOrder.Continue));
            Assert.False(acl.CanExecute(M.SalesOrder.Hold));
        }

        [Fact]
        public void GivenSalesOrder_WhenObjectStateIsOnHold_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            this.SetIdentity(Users.AdministratorUserName);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            order.Hold();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            Assert.Equal(new SalesOrderStates(this.DatabaseSession).OnHold, order.SalesOrderState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).CurrentUser);
            Assert.True(acl.CanExecute(M.SalesOrder.Cancel));
            Assert.True(acl.CanExecute(M.SalesOrder.Continue));
            Assert.False(acl.CanExecute(M.SalesOrder.Confirm));
            Assert.False(acl.CanExecute(M.SalesOrder.Reject));
            Assert.False(acl.CanExecute(M.SalesOrder.Approve));
            Assert.False(acl.CanExecute(M.SalesOrder.Hold));
        }

        [Fact]
        public void GivenSalesOrderWithShippingAndHandlingAmount_WhenDeriving_ThenOrderTotalsMustIncludeShippingAndHandlingAmount()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.DatabaseSession).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(45, order.TotalBasePrice);
            Assert.Equal(0, order.TotalDiscount);
            Assert.Equal(0, order.TotalSurcharge);
            Assert.Equal(7.5m, order.TotalShippingAndHandling);
            Assert.Equal(0, order.TotalFee);
            Assert.Equal(52.5m, order.TotalExVat);
            Assert.Equal(11.03m, order.TotalVat);
            Assert.Equal(63.53m, order.TotalIncVat);
            Assert.Equal(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Fact]
        public void GivenSalesOrderWithShippingAndHandlingPercentage_WhenDeriving_ThenOrderTotalsMustIncludeShippingAndHandlingAmount()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;



            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.DatabaseSession).WithPercentage(5).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(45, order.TotalBasePrice);
            Assert.Equal(0, order.TotalDiscount);
            Assert.Equal(0, order.TotalSurcharge);
            Assert.Equal(2.25m, order.TotalShippingAndHandling);
            Assert.Equal(0, order.TotalFee);
            Assert.Equal(47.25m, order.TotalExVat);
            Assert.Equal(9.92m, order.TotalVat);
            Assert.Equal(57.17m, order.TotalIncVat);
            Assert.Equal(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Fact]
        public void GivenSalesOrderWithFeeAmount_WhenDeriving_ThenOrderTotalsMustIncludeFeeAmount()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.DatabaseSession).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithFee(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(45, order.TotalBasePrice);
            Assert.Equal(0, order.TotalDiscount);
            Assert.Equal(0, order.TotalSurcharge);
            Assert.Equal(0, order.TotalShippingAndHandling);
            Assert.Equal(7.5m, order.TotalFee);
            Assert.Equal(52.5m, order.TotalExVat);
            Assert.Equal(11.03m, order.TotalVat);
            Assert.Equal(63.53m, order.TotalIncVat);
            Assert.Equal(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Fact]
        public void GivenSalesOrderWithFeePercentage_WhenDeriving_ThenOrderTotalsMustIncludeFeeAmount()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.DatabaseSession).WithPercentage(5).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithFee(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(45, order.TotalBasePrice);
            Assert.Equal(0, order.TotalDiscount);
            Assert.Equal(0, order.TotalSurcharge);
            Assert.Equal(0, order.TotalShippingAndHandling);
            Assert.Equal(2.25m, order.TotalFee);
            Assert.Equal(47.25m, order.TotalExVat);
            Assert.Equal(9.92m, order.TotalVat);
            Assert.Equal(57.17m, order.TotalIncVat);
            Assert.Equal(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirming_ThenInventoryItemsQuantityCommittedOutAndAvailableToPromiseMustBeUpdated()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(6, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(3, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);
        }

        [Fact]
        public void GivenSalesOrder_WhenChangingItemQuantityToZero_ThenItemIsInvalid()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.DatabaseSession.Derive();
            Assert.Equal(4, order.ValidOrderItems.Count);

            order.Confirm();

            this.DatabaseSession.Derive();

            item4.QuantityOrdered = 0;

            this.DatabaseSession.Derive();

            Assert.Equal(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Fact]
        public void GivenSalesOrder_WhenOrderItemIsWithoutBasePrice_ThenItemIsInvalid()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            order.Confirm();

            this.DatabaseSession.Derive();

            item4.RemoveActualUnitPrice();

            this.DatabaseSession.Derive();

            Assert.Equal(0, item4.UnitBasePrice);
            Assert.Equal(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirming_ThenAllValidItemsAreInConfirmedState()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            order.Confirm();

            this.DatabaseSession.Derive();

            item4.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).InProcess, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).InProcess, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).InProcess, item3.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.DatabaseSession).Cancelled, item4.SalesOrderItemState);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirmed_ThenShipmentItemsAreCreated()
        {
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
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var good2InventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
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

            var shipment = customer.ShipmentsWhereBillToParty.First;

            Assert.Equal(2, shipment.ShipmentItems.Count);
            Assert.Equal(1, item1.OrderShipmentsWhereSalesOrderItem[0].Quantity);
            Assert.Equal(2, item2.OrderShipmentsWhereSalesOrderItem[0].Quantity);
            Assert.Equal(5, item3.OrderShipmentsWhereSalesOrderItem[0].Quantity);
        }

        [Fact]
        public void GivenSalesOrderWithMultipleRecipients_WhenConfirmed_ThenShipmentIsCreatedForEachRecipientAndPickListIsCreated()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var baal = new CityBuilder(this.DatabaseSession).WithName("Baal").Build();
            var baalAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(baal).WithAddress1("Haverwerf 15").Build();
            var shipToBaal = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(baalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var person1 = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var person2 = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPartyContactMechanism(shipToBaal).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(person1).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(person2).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part = new FinishedGoodBuilder(this.DatabaseSession)
                .WithName("part1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            this.DatabaseSession.Derive();

            var partInventory = (NonSerialisedInventoryItem)part.InventoryItemsWherePart[0];
            partInventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithFinishedGood(part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var colorBlack = new ColourBuilder(this.DatabaseSession)
                .WithName("white")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                                            .WithText("White")
                                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                                            .Build())
                .Build();

            var extraLarge = new SizeBuilder(this.DatabaseSession)
                .WithName("Extra large")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                                            .WithText("White")
                                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                                            .Build())
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(person1)
                .WithShipToCustomer(person1)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProductFeature(colorBlack).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProductFeature(extraLarge).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            item1.AddOrderedWithFeature(item2);
            item1.AddOrderedWithFeature(item3);
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item5 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).WithAssignedShipToParty(person2).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);
            order.AddSalesOrderItem(item5);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();

            var shipmentToMechelen = mechelenAddress.ShipmentsWhereShipToAddress[0];

            var shipmentToBaal = baalAddress.ShipmentsWhereShipToAddress[0];

            this.DatabaseSession.Derive();

            Assert.Equal(mechelenAddress, shipmentToMechelen.ShipToAddress);
            Assert.Equal(1, shipmentToMechelen.ShipmentItems.Count);
            Assert.Equal(3, shipmentToMechelen.ShipmentItems[0].Quantity);

            Assert.Equal(baalAddress, shipmentToBaal.ShipToAddress);
            Assert.Equal(1, shipmentToBaal.ShipmentItems.Count);
            Assert.Equal(good2, shipmentToBaal.ShipmentItems[0].Good);
            Assert.Equal(5, shipmentToBaal.ShipmentItems[0].Quantity);
        }

        [Fact]
        public void GivenSalesOrder_WhenShipToAndBillToAreSameCustomer_ThenDerivedCustomersIsSingleCustomer()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, order.Customers.Count);
            Assert.Equal(customer, order.Customers.First);
        }

        [Fact]
        public void GivenSalesOrder_WhenShipToAndBillToAreDifferentCustomers_ThenDerivedCustomersHoldsBothCustomers()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;


            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();



            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToCustomer(billToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(2, order.Customers.Count);
            Assert.Contains(billToCustomer, order.Customers);
            Assert.Contains(shipToCustomer, order.Customers);
        }

        [Fact]
        public void GivenSalesOrder_WhenDerivingSalesReps_ThenSalesRepsAreCollectedFromSalesOrderItems()
        {
            var salesrep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for child product category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for parent category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for everything else").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var parentProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("parent").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("child").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(parentProductCategory).
                Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("company").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();



            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(customer)
                .WithProductCategory(childProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(customer)
                .WithProductCategory(parentProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(customer)
                .Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithProductCategory(childProductCategory)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithProductCategory(parentProductCategory)
                .Build();

            var good3 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(1, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive();

            Assert.Equal(2, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);
            Assert.Contains(salesrep2, order.SalesReps);

            var item3 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good3)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive();

            Assert.Equal(3, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);
            Assert.Contains(salesrep2, order.SalesReps);
            Assert.Contains(salesrep3, order.SalesReps);
        }
    }
}
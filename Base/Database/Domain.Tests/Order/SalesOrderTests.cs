// <copyright file="SalesOrderTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;

    using Xunit;

    public class SalesOrderTests : DomainTest
    {
        [Fact]
        public void GivenSalesOrderBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            var internalOrganisation = this.InternalOrganisation;

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Provisional, order.SalesOrderState);
            Assert.True(order.PartiallyShip);
            Assert.Equal(this.Session.Now().Date, order.OrderDate.Date);
            Assert.Equal(this.Session.Now().Date, order.EntryDate.Date);
            Assert.Equal(order.PreviousBillToCustomer, order.BillToCustomer);
            Assert.Equal(order.PreviousShipToCustomer, order.ShipToCustomer);
            Assert.Equal(order.VatRegime, order.BillToCustomer.VatRegime);
            Assert.Equal(new Stores(this.Session).FindBy(M.Store.Name, "store"), order.Store);
            Assert.Equal(order.Store.DefaultCollectionMethod, order.PaymentMethod);
            Assert.Equal(order.Store.DefaultShipmentMethod, order.ShipmentMethod);
        }

        [Fact]
        public void GivenSalesOrderForItemsThatAreAvailable_WhenShipped_ThenOrderIsCompleted()
        {
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

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPartyContactMechanism(billToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good1.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good2.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            // var derivation = new Allors.Domain.Logging.Derivation(this.Session, new DerivationConfig { DerivationLogFunc = () => new DerivationLog() });
            // derivation.Derive();

            // var list = ((DerivationLog)derivation.DerivationLog).List;
            ////list.RemoveAll(v => !v.StartsWith("Dependency"));

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

            Assert.Equal(new SalesOrderStates(this.Session).Completed, order.SalesOrderState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Completed, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Completed, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Completed, item3.SalesOrderItemState);
        }

        [Fact]
        public void GivenSalesOrderShippedInMultipleParts_WhenPaymentsAreReceived_ThenObjectStateCorrespondingSalesOrderIsUpdated()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(1).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();

            this.Session.Derive();

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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).WithComment("item1").Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).WithComment("item2").Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5).WithAssignedUnitPrice(15).WithComment("item3").Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

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

            shipment.SetPacked();
            this.Session.Derive();

            shipment.Ship();
            this.Session.Derive();

            var salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].ShipmentItemBillingsWhereShipmentItem[0].InvoiceItem;
            var invoice1 = (SalesInvoice)salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;
            invoice1.Send();

            new ReceiptBuilder(this.Session)
                .WithAmount(15)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(15).Build())
                .WithEffectiveDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).InProcess, order.SalesOrderState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Finished, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item3.SalesOrderItemState);

            Assert.Equal(1, item1.QuantityShipped);
            Assert.Equal(0, item1.QuantityCommittedOut);
            Assert.Equal(0, item1.QuantityPendingShipment);
            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(0, item1.QuantityShortFalled);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();

            this.Session.Derive();

            shipment = (CustomerShipment)item2.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();

            this.Session.Derive();

            package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            shipment.Ship();

            this.Session.Derive();

            salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].ShipmentItemBillingsWhereShipmentItem[0].InvoiceItem;
            var invoice2 = (SalesInvoice)salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;
            invoice2.Send();

            new ReceiptBuilder(this.Session)
                .WithAmount(30)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice2.SalesInvoiceItems[0]).WithAmountApplied(30).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).InProcess, order.SalesOrderState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Finished, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Finished, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item3.SalesOrderItemState);

            Assert.Equal(1, item1.QuantityShipped);
            Assert.Equal(0, item1.QuantityCommittedOut);
            Assert.Equal(0, item1.QuantityPendingShipment);
            Assert.Equal(0, item1.QuantityRequestsShipping);
            Assert.Equal(0, item1.QuantityShortFalled);

            Assert.Equal(2, item2.QuantityShipped);
            Assert.Equal(0, item2.QuantityCommittedOut);
            Assert.Equal(0, item2.QuantityPendingShipment);
            Assert.Equal(0, item2.QuantityRequestsShipping);
            Assert.Equal(0, item2.QuantityShortFalled);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good2.Part).Build();

            this.Session.Derive();

            shipment = (CustomerShipment)item3.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();

            this.Session.Derive();

            package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            shipment.Ship();

            this.Session.Derive();

            salesInvoiceitem =
                (SalesInvoiceItem)shipment.ShipmentItems[0].ShipmentItemBillingsWhereShipmentItem[0].InvoiceItem;
            var invoice3 = salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;

            new ReceiptBuilder(this.Session)
                .WithAmount(75)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice3.SalesInvoiceItems[0]).WithAmountApplied(75).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Finished, order.SalesOrderState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Finished, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Finished, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Finished, item3.SalesOrderItemState);
        }

        [Fact]
        public void GivenPendingShipmentAndAssignedPickList_WhenNewOrderIsConfirmed_ThenNewPickListIsCreatedAndSingleOrderShipmentIsUpdated()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item);

            this.Session.Derive();

            order1.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(3, shipment.ShipmentItems[0].Quantity);

            var pickList1 = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(3, pickList1.PickListItems[0].Quantity);

            pickList1.Picker = this.OrderProcessor;

            this.Session.Derive();

            var order2 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(2)
                .WithAssignedUnitPrice(5)
                .Build();

            order2.AddSalesOrderItem(item);

            this.Session.Derive();

            order2.Confirm();

            this.Session.Derive();

            Assert.Equal(5, shipment.ShipmentItems.First.Quantity);

            var pickList2 = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[1].PickListItem.PickListWherePickListItem;
            Assert.Equal(2, pickList2.PickListItems[0].Quantity);
        }

        [Fact]
        public void GivenSalesOrderWithPendingShipmentAndAssignedPickList_WhenOrderIsCancelled_ThenNegativePickListIsCreatedAndSingleOrderShipmentIsCancelled()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good1)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].Quantity);

            pickList.Picker = this.OrderProcessor;

            this.Session.Derive();

            order.Cancel();

            this.Session.Derive();

            var negativePickList = order.ShipToCustomer.PickListsWhereShipToParty[1];
            Assert.Equal(-10, negativePickList.PickListItems[0].Quantity);

            Assert.Equal(new ShipmentStates(this.Session).Cancelled, shipment.ShipmentState);
        }

        [Fact]
        public void GivenSalesOrderWithPickList_WhenOrderIsCancelled_ThenPickListIsCancelledAndSingleOrderShipmentIsCancelled()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item);

            this.Session.Derive();

            order1.Confirm();

            this.Session.Derive();

            var order2 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(20)
                .WithAssignedUnitPrice(5)
                .Build();

            order2.AddSalesOrderItem(item);

            this.Session.Derive();

            order2.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)customer.ShipmentsWhereShipToParty[0];
            Assert.Equal(30, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(30, pickList.PickListItems[0].Quantity);

            order1.Cancel();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Created, shipment.ShipmentState);
            Assert.Equal(new PickListStates(this.Session).Created, pickList.PickListState);
            Assert.Equal(20, pickList.PickListItems[0].Quantity);

            order2.Cancel();

            this.Session.Derive();

            Assert.Equal(new ShipmentStates(this.Session).Cancelled, shipment.ShipmentState);
            Assert.Equal(new PickListStates(this.Session).Cancelled, pickList.PickListState);
        }

        [Fact]
        public void GivenSalesOrderOnHold_WhenInventoryBecomesAvailable_ThenOrderIsNotSelectedForShipment()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            order.Hold();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityCommittedOut);
            Assert.Equal(10, item.QuantityShortFalled);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityCommittedOut);
            Assert.Equal(10, item.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderOnHold_WhenOrderIsContinued_ThenOrderIsSelectedForShipment()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            order.Hold();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityShortFalled);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityShortFalled);

            order.Continue();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).InProcess, order.SalesOrderState);
            Assert.Equal(10, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderNotPartiallyShipped_WhenInComplete_ThenOrderIsNotSelectedForShipment()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good2.Part).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithPartiallyShip(false)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good1)
                .WithQuantityOrdered(20)
                .WithAssignedUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good2)
                .WithQuantityOrdered(20)
                .WithAssignedUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.False(customer.ExistShipmentsWhereShipToParty);

            Assert.Equal(10, item1.QuantityRequestsShipping);
            Assert.Equal(0, item1.QuantityPendingShipment);
            Assert.Equal(10, item1.QuantityShortFalled);

            Assert.Equal(10, item2.QuantityRequestsShipping);
            Assert.Equal(0, item2.QuantityPendingShipment);
            Assert.Equal(10, item2.QuantityShortFalled);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();

            this.Session.Derive();

            Assert.False(customer.ExistShipmentsWhereShipToParty);

            Assert.Equal(20, item1.QuantityRequestsShipping);
            Assert.Equal(0, item1.QuantityPendingShipment);
            Assert.Equal(0, item1.QuantityShortFalled);

            Assert.Equal(10, item2.QuantityRequestsShipping);
            Assert.Equal(0, item2.QuantityPendingShipment);
            Assert.Equal(10, item2.QuantityShortFalled);

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good2.Part).Build();

            this.Session.Derive();

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
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var productItem = new InvoiceItemTypes(this.Session).ProductItem;
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithFromDate(this.Session.Now().AddYears(-2))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithInvoiceDate(this.Session.Now().AddYears(-1))
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(10).WithAssignedUnitPrice(100).WithInvoiceItemType(productItem).Build();
            invoice.AddSalesInvoiceItem(invoiceItem);

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).RequestsApproval, order.SalesOrderState);
            Assert.Equal(0, item.QuantityReserved);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);

            order.Approve();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).InProcess, order.SalesOrderState);
            Assert.Equal(10, item.QuantityReserved);
            Assert.Equal(10, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderForCustomerExceedingCreditLimit_WhenOrderIsConfirmed_ThenOrderRequestsApproval()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var productItem = new InvoiceItemTypes(this.Session).ProductItem;
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            var customerRelationship = new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithFromDate(this.Session.Now().AddYears(-2))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var partyFinancial = customer.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship.InternalOrganisation));
            partyFinancial.CreditLimit = 100M;

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithInvoiceDate(this.Session.Now().AddYears(-1))
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(10).WithAssignedUnitPrice(11).WithInvoiceItemType(productItem).Build();
            invoice.AddSalesInvoiceItem(invoiceItem);

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).RequestsApproval, order.SalesOrderState);
            Assert.Equal(0, item.QuantityReserved);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);

            order.Approve();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).InProcess, order.SalesOrderState);
            Assert.Equal(10, item.QuantityReserved);
            Assert.Equal(10, item.QuantityPendingShipment);
            Assert.Equal(0, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderBelowOrderThreshold_WhenOrderIsConfirmed_ThenOrderIsNotShipped()
        {
            new Stores(this.Session).Extent().First.OrderThreshold = 1;

            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                .WithInternalOrganisation(this.InternalOrganisation)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(1)
                .WithAssignedUnitPrice(0.1M)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).RequestsApproval, order.SalesOrderState);
        }

        [Fact]
        public void GivenSalesOrderWithManualShipmentSchedule_WhenOrderIsConfirmed_ThenInventoryIsNotReservedAndOrderIsNotShipped()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good.Part).Build();

            this.Session.Derive();

            var inventoryItem = (NonSerialisedInventoryItem)good.Part.InventoryItemsWherePart.First;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var manual = new OrderKindBuilder(this.Session).WithDescription("manual").WithScheduleManually(true).Build();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithOrderKind(manual)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(50)
                .WithAssignedUnitPrice(50)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).InProcess, order.SalesOrderState);
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
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .WithBillToContactMechanism(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(3, item1.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(97, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(100, item1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            order.Reject();

            this.Session.Derive();

            Assert.Equal(0, item1.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(100, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(100, item1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenConfirmedOrder_WhenOrderIsCancelled_ThenNonSerialisedInventoryQuantitiesAreReleased()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .WithBillToContactMechanism(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(3, item1.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(97, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(100, item1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);

            order.Cancel();

            this.Session.Derive();

            Assert.Equal(0, item1.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(100, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(100, item1.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(0, item3.ReservedFromNonSerialisedInventoryItem.QuantityOnHand);
        }

        [Fact]
        public void GivenSalesOrder_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Provisional, order.SalesOrderState);
            Assert.Equal(order.LastSalesOrderState, order.SalesOrderState);
        }

        [Fact]
        public void GivenSalesOrder_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Null(order.PreviousSalesOrderState);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirmed_ThenCurrentOrderStatusMustBeDerived()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Provisional, order.SalesOrderState);

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).InProcess, order.SalesOrderState);
        }

        [Fact]
        public void GivenSalesOrderWithCancelledItem_WhenDeriving_ThenCancelledItemIsNotInValidOrderItems()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.Session.Derive();

            item4.Cancel();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Fact]
        public void GivenSalesOrderBuilder_WhenBuild_ThenOrderMustBeValid()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            new SalesOrderBuilder(this.Session).WithBillToCustomer(billToCustomer).WithTakenBy(this.InternalOrganisation).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesOrder_WhenGettingOrderNumberWithoutFormat_ThenOrderNumberShouldBeReturned()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var store = new Stores(this.Session).Extent().First(v => Equals(v.InternalOrganisation, this.InternalOrganisation));
            store.RemoveSalesOrderNumberPrefix();

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.Session.Derive();

            Assert.Equal("1", order1.OrderNumber);

            var order2 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.Session.Derive();

            Assert.Equal("2", order2.OrderNumber);
        }

        [Fact]
        public void GivenSalesOrder_WhenGettingOrderNumberWithFormat_ThenFormattedOrderNumberShouldBeReturned()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var store = new Stores(this.Session).Extent().First(v => Equals(v.InternalOrganisation, this.InternalOrganisation));
            store.SalesOrderNumberPrefix = "the format is ";

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.Session.Derive();

            Assert.Equal("the format is 1", order1.OrderNumber);

            var order2 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.Session.Derive();

            Assert.Equal("the format is 2", order2.OrderNumber);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenDerivedSalesRepMustExist()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var salesrep = new PersonBuilder(this.Session).WithLastName("salesrep").Build();

            this.Session.Derive();
            this.Session.Commit();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithFromDate(this.Session.Now())
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithCustomer(customer)
                .WithSalesRepresentative(salesrep)
                .Build();

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            order.AddSalesOrderItem(new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(1).WithAssignedUnitPrice(10).Build());

            this.Session.Derive();

            Assert.Contains(salesrep, order.SalesReps);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenTakenByContactMechanismMustExist()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            var internalOrganisation = this.InternalOrganisation;

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var orderContact = new EmailAddressBuilder(this.Session).WithElectronicAddressString("orders@acme.com").Build();

            var orderFrom = new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(orderContact)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).OrderAddress)
                .Build();

            internalOrganisation.AddPartyContactMechanism(orderFrom);

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(internalOrganisation.OrderAddress, order1.TakenByContactMechanism);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenBillFromContactMechanismMustExist()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            var internalOrganisation = this.InternalOrganisation;

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var billingContact = new EmailAddressBuilder(this.Session)
                .WithElectronicAddressString("orders@acme.com")
                .Build();

            var billingFrom = new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(billingContact)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .Build();

            internalOrganisation.AddPartyContactMechanism(billingFrom);

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(internalOrganisation.BillingAddress, order1.TakenByContactMechanism);
        }

        [Fact]
        public void GivenSalesOrderWithBillToCustomerWithPreferredCurrency_WhenBuild_ThenCurrencyIsFromCustomer()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var englischLocale = new Locales(this.Session).EnglishGreatBritain;
            var poundSterling = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "GBP");

            var customer = new OrganisationBuilder(this.Session).WithName("customer").WithLocale(englischLocale).WithPreferredCurrency(poundSterling).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(poundSterling, order.Currency);

            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            customer.PreferredCurrency = euro;

            this.Session.Derive();

            Assert.Equal(englischLocale.Country.Currency, order.Currency);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenLocaleMustExist()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToAddress(shipToContactMechanism)
                .Build();

            this.Session.Derive();

            Assert.Equal(this.Session.GetSingleton().DefaultLocale, order.Locale);
        }

        [Fact]
        public void GivenSalesOrderWithShippingAndHandlingAmount_WhenDeriving_ThenOrderTotalsMustIncludeShippingAndHandlingAmount()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.Session).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            var supplierOffering = new SupplierOfferingBuilder(this.Session)
                .WithPart(good.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(45, order.TotalBasePrice);
            Assert.Equal(0, order.TotalDiscount);
            Assert.Equal(0, order.TotalSurcharge);
            Assert.Equal(7.5m, order.TotalShippingAndHandling);
            Assert.Equal(0, order.TotalFee);
            Assert.Equal(52.5m, order.TotalExVat);
            Assert.Equal(11.03m, order.TotalVat);
            Assert.Equal(63.53m, order.TotalIncVat);
        }

        [Fact]
        public void GivenSalesOrderWithShippingAndHandlingPercentage_WhenDeriving_ThenOrderTotalsMustIncludeShippingAndHandlingAmount()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.Session).WithPercentage(5).WithVatRate(vatRate21).Build();

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            var supplierOffering = new SupplierOfferingBuilder(this.Session)
                .WithPart(good.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(45, order.TotalBasePrice);
            Assert.Equal(0, order.TotalDiscount);
            Assert.Equal(0, order.TotalSurcharge);
            Assert.Equal(2.25m, order.TotalShippingAndHandling);
            Assert.Equal(0, order.TotalFee);
            Assert.Equal(47.25m, order.TotalExVat);
            Assert.Equal(9.92m, order.TotalVat);
            Assert.Equal(57.17m, order.TotalIncVat);
        }

        [Fact]
        public void GivenSalesOrderWithFeeAmount_WhenDeriving_ThenOrderTotalsMustIncludeFeeAmount()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.Session).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            var supplierOffering = new SupplierOfferingBuilder(this.Session)
                .WithPart(good.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithFee(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(45, order.TotalBasePrice);
            Assert.Equal(0, order.TotalDiscount);
            Assert.Equal(0, order.TotalSurcharge);
            Assert.Equal(0, order.TotalShippingAndHandling);
            Assert.Equal(7.5m, order.TotalFee);
            Assert.Equal(52.5m, order.TotalExVat);
            Assert.Equal(11.03m, order.TotalVat);
            Assert.Equal(63.53m, order.TotalIncVat);
        }

        [Fact]
        public void GivenSalesOrderWithoutShipFromAddress_WhenDeriving_ThenUseTakenByShipFromAddress()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.Session).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithFee(adjustment)
                .Build();

            this.Session.Derive();

            Assert.NotNull(this.InternalOrganisation.ShippingAddress);
            Assert.Equal(order.ShipFromAddress, this.InternalOrganisation.ShippingAddress);
        }

        [Fact]
        public void GivenSalesOrderWithShipFromAddress_WhenDeriving_ThenUseOrderShipFromAddress()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipFrom = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.Session).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var order = new SalesOrderBuilder(this.Session)
                .WithShipFromAddress(shipFrom)
                .WithBillToCustomer(billToCustomer)
                .WithFee(adjustment)
                .Build();

            this.Session.Derive();

            Assert.Equal(order.ShipFromAddress, shipFrom);
        }

        [Fact]
        public void GivenSalesOrderWithFeePercentage_WhenDeriving_ThenOrderTotalsMustIncludeFeeAmount()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.Session).WithPercentage(5).WithVatRate(vatRate21).Build();

            var good = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            var supplierOffering = new SupplierOfferingBuilder(this.Session)
                .WithPart(good.Part)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithFee(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(45, order.TotalBasePrice);
            Assert.Equal(0, order.TotalDiscount);
            Assert.Equal(0, order.TotalSurcharge);
            Assert.Equal(0, order.TotalShippingAndHandling);
            Assert.Equal(2.25m, order.TotalFee);
            Assert.Equal(47.25m, order.TotalExVat);
            Assert.Equal(9.92m, order.TotalVat);
            Assert.Equal(57.17m, order.TotalIncVat);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirming_ThenInventoryItemsQuantityCommittedOutAndAvailableToPromiseMustBeUpdated()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(10).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good2.Part).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(6, item1.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(4, item1.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
            Assert.Equal(3, item3.ReservedFromNonSerialisedInventoryItem.QuantityCommittedOut);
            Assert.Equal(7, item3.ReservedFromNonSerialisedInventoryItem.AvailableToPromise);
        }

        [Fact]
        public void GivenSalesOrder_WhenChangingItemQuantityToZero_ThenItemIsInvalid()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.Session.Derive();
            Assert.Equal(4, order.ValidOrderItems.Count);

            order.Confirm();

            this.Session.Derive();

            item4.QuantityOrdered = 0;

            var derivationLog = this.Session.Derive(false);

            Assert.True(derivationLog.HasErrors);
            Assert.Contains(M.SalesOrderItem.QuantityOrdered, derivationLog.Errors[0].RoleTypes);
        }

        [Fact]
        public void GivenSalesOrder_WhenOrderItemIsWithoutBasePrice_ThenExceptionIsThrown()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            item4.RemoveAssignedUnitPrice();

            var derivationLog = this.Session.Derive(false);

            Assert.True(derivationLog.HasErrors);
            Assert.Contains(M.Priceable.UnitBasePrice, derivationLog.Errors[0].RoleTypes);

            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirming_ThenAllValidItemsAreInConfirmedState()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithAssignedUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            item4.Cancel();

            this.Session.Derive();

            Assert.Equal(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item3.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Cancelled, item4.SalesOrderItemState);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirmed_ThenShipmentItemsAreCreated()
        {
            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good2.Part).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithOrderKind(new OrderKindBuilder(this.Session).WithDescription("auto").WithScheduleManually(false).Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = customer.ShipmentsWhereShipToParty.First;

            Assert.Equal(2, shipment.ShipmentItems.Count);
            Assert.Equal(1, item1.OrderShipmentsWhereOrderItem[0].Quantity);
            Assert.Equal(2, item2.OrderShipmentsWhereOrderItem[0].Quantity);
            Assert.Equal(5, item3.OrderShipmentsWhereOrderItem[0].Quantity);
        }

        [Fact]
        public void GivenSalesOrderForSerialisedItem_WhenConfirmed_ThenShipmentItemIsCreated()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var good1 = new NonUnifiedGoodBuilder(this.Session)
                .WithName("good1")
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("good1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(vatRate21)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("1")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).Serialised)
                    .Build())
                .Build();

            var serialisedItem1 = new SerialisedItemBuilder(this.Session).WithSerialNumber("1").WithAvailableForSale(true).Build();
            good1.Part.AddSerialisedItem(serialisedItem1);

            new SerialisedInventoryItemBuilder(this.Session).WithFacility(this.InternalOrganisation.FacilitiesWhereOwner.First).WithPart(good1.Part).WithSerialisedItem(serialisedItem1).Build();

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            order.Confirm();

            //var derivation = new Logging.Derivation(this.Session, new DerivationConfig
            //    {
            //        DerivationLogFunc = () => new CustomListDerivationLog(),
            //    }
            //);

            //derivation.Derive();

            //var list = ((CustomListDerivationLog)derivation.DerivationLog).List;
            //list.RemoveAll(v => !v.StartsWith("Dependency"));

            this.Session.Derive();

            var shipment = customer.ShipmentsWhereShipToParty.First;

            Assert.Equal(1, shipment.ShipmentItems.Count);
            Assert.Equal(1, item1.OrderShipmentsWhereOrderItem[0].Quantity);
        }

        [Fact]
        public void GivenSalesOrderWithMultipleRecipients_WhenConfirmed_ThenShipmentIsCreatedForEachRecipientAndPickListIsCreated()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var baal = new CityBuilder(this.Session).WithName("Baal").Build();
            var baalAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(baal).WithAddress1("Haverwerf 15").Build();
            var shipToBaal = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(baalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var person1 = new PersonBuilder(this.Session).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).Build();
            var person2 = new PersonBuilder(this.Session).WithLastName("person2").WithPartyContactMechanism(shipToBaal).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(person1).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(person2).WithInternalOrganisation(this.InternalOrganisation).Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good2.Part).Build();

            this.Session.Derive();

            var colorBlack = new ColourBuilder(this.Session)
                .WithName("Black")
                .Build();

            var extraLarge = new SizeBuilder(this.Session)
                .WithName("Extra large")
                .Build();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(person1)
                .WithShipToCustomer(person1)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(new VatRegimes(this.Session).Export)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductFeatureItem).WithProductFeature(colorBlack).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductFeatureItem).WithProductFeature(extraLarge).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            item1.AddOrderedWithFeature(item2);
            item1.AddOrderedWithFeature(item3);
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            var item5 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5).WithAssignedUnitPrice(15).WithAssignedShipToParty(person2).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);
            order.AddSalesOrderItem(item5);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipmentToMechelen = mechelenAddress.ShipmentsWhereShipToAddress[0];

            var shipmentToBaal = baalAddress.ShipmentsWhereShipToAddress[0];

            this.Session.Derive();

            Assert.Equal(mechelenAddress, shipmentToMechelen.ShipToAddress);
            Assert.Single(shipmentToMechelen.ShipmentItems);
            Assert.Equal(3, shipmentToMechelen.ShipmentItems[0].Quantity);

            Assert.Equal(baalAddress, shipmentToBaal.ShipToAddress);
            Assert.Single(shipmentToBaal.ShipmentItems);
            Assert.Equal(good2, shipmentToBaal.ShipmentItems[0].Good);
            Assert.Equal(5, shipmentToBaal.ShipmentItems[0].Quantity);
        }

        [Fact]
        public void GivenSalesOrder_WhenShipToAndBillToAreSameCustomer_ThenDerivedCustomersIsSingleCustomer()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Single(order.Customers);
            Assert.Equal(customer, order.Customers.First);
        }

        [Fact]
        public void GivenSalesOrder_WhenShipToAndBillToAreDifferentCustomers_ThenDerivedCustomersHoldsBothCustomers()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var billToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var shipToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToCustomer(billToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(2, order.Customers.Count);
            Assert.Contains(billToCustomer, order.Customers);
            Assert.Contains(shipToCustomer, order.Customers);
        }

        [Fact]
        public void GivenSalesOrder_WhenDerivingSalesReps_ThenSalesRepsAreCollectedFromSalesOrderItems()
        {
            var salesrep1 = new PersonBuilder(this.Session).WithLastName("salesrep for child product category").Build();
            var salesrep2 = new PersonBuilder(this.Session).WithLastName("salesrep for parent category").Build();
            var salesrep3 = new PersonBuilder(this.Session).WithLastName("salesrep for everything else").Build();
            var parentProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("parent")
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("child")
                .WithPrimaryParent(parentProductCategory).
                Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var customer = new OrganisationBuilder(this.Session).WithName("company").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(customer)
                .WithProductCategory(childProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(customer)
                .WithProductCategory(parentProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(customer)
                .Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var good1 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("1")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            childProductCategory.AddProduct(good1);

            var good2 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("2")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            parentProductCategory.AddProduct(good2);

            var good3 = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new NonUnifiedPartBuilder(this.Session)
                    .WithProductIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("2")
                        .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            var firstCategory = this.Session.Extent<ProductCategory>().First;
            firstCategory.AddProduct(good3);

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good1)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(2, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);
            Assert.Contains(salesrep3, order.SalesReps);

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good2)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(3, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);
            Assert.Contains(salesrep2, order.SalesReps);
            Assert.Contains(salesrep3, order.SalesReps);

            var item3 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good3)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            Assert.Equal(3, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);
            Assert.Contains(salesrep2, order.SalesReps);
            Assert.Contains(salesrep3, order.SalesReps);
        }

        [Fact]
        public void GivenSalesOrderTakenByBelgianInternalOrganisationForRentingGoodsToBusinessCustomer_WhenDerived_ThenVatClauseIsSet()
        {
            var takenByAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["BE"])
                .Build();

            var takenByContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(takenByAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).RegisteredOffice)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var shipFromAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["BE"])
                .Build();

            var shipFromContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipFromAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var belgianInternalOrganisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Belgian InternalOrganisation")
                .WithPartyContactMechanism(takenByContactMechanism)
                .WithPartyContactMechanism(shipFromContactMechanism)
                .Build();

            new StoreBuilder(this.Session)
                .WithName("store")
                .WithBillingProcess(new BillingProcesses(this.Session).BillingForShipmentItems)
                .WithInternalOrganisation(belgianInternalOrganisation)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithDefaultCollectionMethod(new PaymentMethods(this.Session).Extent().First)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["US"])
                .Build();

            var shipToContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipToAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").WithPartyContactMechanism(shipToContactMechanism).WithVatRegime(new VatRegimes(this.Session).Export).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(belgianInternalOrganisation).Build();

            this.Session.Derive();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good1.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good2.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();

            this.Session.Derive();

            // seller is belgian company, renting good to customer
            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(belgianInternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(shipToAddress)
                .WithVatRegime(new VatRegimes(this.Session).ServiceB2B)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(new VatClauses(this.Session).ServiceB2B, order.DerivedVatClause);
        }

        [Fact]
        public void GivenSalesOrderTakenByBelgianInternalOrganisationForSellingToInsideEUBusinessCustomer_WhenDerived_ThenVatClauseIsSet()
        {
            var takenByAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["BE"])
                .Build();

            var takenByContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(takenByAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).RegisteredOffice)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var shipFromAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["BE"])
                .Build();

            var shipFromContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipFromAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var belgianInternalOrganisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Belgian InternalOrganisation")
                .WithPartyContactMechanism(takenByContactMechanism)
                .WithPartyContactMechanism(shipFromContactMechanism)
                .Build();

            new StoreBuilder(this.Session)
                .WithName("store")
                .WithBillingProcess(new BillingProcesses(this.Session).BillingForShipmentItems)
                .WithInternalOrganisation(belgianInternalOrganisation)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithDefaultCollectionMethod(new PaymentMethods(this.Session).Extent().First)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["NL"])
                .Build();

            var shipToContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipToAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").WithPartyContactMechanism(shipToContactMechanism).WithVatRegime(new VatRegimes(this.Session).IntraCommunautair).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(belgianInternalOrganisation).Build();

            this.Session.Derive();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good1.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good2.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();

            this.Session.Derive();

            // seller is belgian company, selling to EU customer, shipping From Belgium inside EU
            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(belgianInternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(shipToAddress)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(new VatClauses(this.Session).Intracommunautair, order.DerivedVatClause);
        }

        [Fact]
        public void GivenSalesOrderTakenByBelgianInternalOrganisationForSellingToOutsideEUBusinessCustomer_WhenDerived_ThenVatClauseIsSet()
        {
            var takenByAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["BE"])
                .Build();

            var takenByContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(takenByAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).RegisteredOffice)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var shipFromAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["BE"])
                .Build();

            var shipFromContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipFromAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var belgianInternalOrganisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Belgian InternalOrganisation")
                .WithPartyContactMechanism(takenByContactMechanism)
                .WithPartyContactMechanism(shipFromContactMechanism)
                .Build();

            new StoreBuilder(this.Session)
                .WithName("store")
                .WithBillingProcess(new BillingProcesses(this.Session).BillingForShipmentItems)
                .WithInternalOrganisation(belgianInternalOrganisation)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithDefaultCollectionMethod(new PaymentMethods(this.Session).Extent().First)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["US"])
                .Build();

            var shipToContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipToAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").WithPartyContactMechanism(shipToContactMechanism).WithVatRegime(new VatRegimes(this.Session).Export).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(belgianInternalOrganisation).Build();

            this.Session.Derive();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good1.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good2.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();

            this.Session.Derive();

            // seller is belgian company, selling to outside EU customer, shipping From Belgium outside EU, seller responsible for transport
            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(belgianInternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(shipToAddress)
                .WithSalesTerm(new IncoTermBuilder(this.Session).WithTermType(new IncoTermTypes(this.Session).Cif).Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(new VatClauses(this.Session).BeArt39Par1Item1, order.DerivedVatClause);
        }

        [Fact]
        public void GivenSalesOrderTakenByBelgianInternalOrganisationForSellingToOutsideEUBusinessCustomerExw_WhenDerived_ThenVatClauseIsSet()
        {
            var takenByAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["BE"])
                .Build();

            var takenByContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(takenByAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).RegisteredOffice)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var shipFromAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["BE"])
                .Build();

            var shipFromContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipFromAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var belgianInternalOrganisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Belgian InternalOrganisation")
                .WithPartyContactMechanism(takenByContactMechanism)
                .WithPartyContactMechanism(shipFromContactMechanism)
                .Build();

            new StoreBuilder(this.Session)
                .WithName("store")
                .WithBillingProcess(new BillingProcesses(this.Session).BillingForShipmentItems)
                .WithInternalOrganisation(belgianInternalOrganisation)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithDefaultCollectionMethod(new PaymentMethods(this.Session).Extent().First)
                .Build();

            var shipToAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("address")
                .WithLocality("city")
                .WithCountry(new Countries(this.Session).CountryByIsoCode["US"])
                .Build();

            var shipToContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipToAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").WithPartyContactMechanism(shipToContactMechanism).WithVatRegime(new VatRegimes(this.Session).Export).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(belgianInternalOrganisation).Build();

            this.Session.Derive();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            var good2 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good2");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good1.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithPart(good2.Part).WithReason(new InventoryTransactionReasons(this.Session).Unknown).Build();

            this.Session.Derive();

            // seller is belgian company, selling to outside EU customer, shipping From Belgium outside EU, customer responsible for transport
            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(belgianInternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(shipToAddress)
                .WithSalesTerm(new IncoTermBuilder(this.Session).WithTermType(new IncoTermTypes(this.Session).Exw).Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithAssignedUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithAssignedUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(new VatClauses(this.Session).BeArt39Par1Item2, order.DerivedVatClause);
        }
    }

    public class SalesOrderSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenSalesOrder_WhenObjectStateIsProvisional_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            User user = this.Administrator;
            this.Session.SetUser(user);

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(new SalesOrderStates(this.Session).Provisional, order.SalesOrderState);
            var acl = new AccessControlLists(this.Session.GetUser())[order];
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
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("customer").Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            User user = this.Administrator;
            this.Session.SetUser(user);

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();
            this.Session.Commit();

            var acl = new AccessControlLists(this.Session.GetUser())[order];
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
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("customer").Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            this.Session.SetUser(customer);

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Cancel();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Cancelled, order.SalesOrderState);
            var acl = new AccessControlLists(this.Session.GetUser())[order];
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
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            this.Session.SetUser(customer);

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Reject();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Rejected, order.SalesOrderState);
            var acl = new AccessControlLists(this.Session.GetUser())[order];
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
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            this.Session.SetUser(customer);

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            order.SalesOrderState = new SalesOrderStates(this.Session).Finished;

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Finished, order.SalesOrderState);
            var acl = new AccessControlLists(this.Session.GetUser())[order];
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
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            User user = this.Administrator;
            this.Session.SetUser(user);

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            order.Hold();

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            var acl = new AccessControlLists(this.Session.GetUser())[order];
            Assert.True(acl.CanExecute(M.SalesOrder.Cancel));
            Assert.True(acl.CanExecute(M.SalesOrder.Continue));
            Assert.False(acl.CanExecute(M.SalesOrder.Confirm));
            Assert.False(acl.CanExecute(M.SalesOrder.Reject));
            Assert.False(acl.CanExecute(M.SalesOrder.Approve));
            Assert.False(acl.CanExecute(M.SalesOrder.Hold));
        }
    }
}

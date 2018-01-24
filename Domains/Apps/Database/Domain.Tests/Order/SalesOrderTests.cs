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
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            var internalOrganisation = this.InternalOrganisation;

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Provisional, order.SalesOrderState);
            Assert.True(order.PartiallyShip);
            Assert.Equal(DateTime.UtcNow.Date, order.OrderDate.Date);
            Assert.Equal(DateTime.UtcNow.Date, order.EntryDate.Date);
            Assert.Equal(order.PreviousBillToCustomer, order.BillToCustomer);
            Assert.Equal(order.PreviousShipToCustomer, order.ShipToCustomer);
            Assert.Equal(order.VatRegime, order.BillToCustomer.VatRegime);
            Assert.Equal(new Stores(this.Session).FindBy(M.Store.Name, "store"), order.Store);
            Assert.Equal(order.Store.DefaultPaymentMethod, order.PaymentMethod);
            Assert.Equal(order.Store.DefaultShipmentMethod, order.ShipmentMethod);
        }

        [Fact]
        public void GivenSalesOrderForItemsThatAreAvailable_WhenShipped_ThenOrderIsCompleted()
        {
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(InternalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1InventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var good2InventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
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

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1Inventory = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good1).Build();
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(1).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate0)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good2Inventory = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good2).Build();

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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();


            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).WithComment("item1").Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).WithComment("item2").Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).WithComment("item3").Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

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

            shipment.SetPacked();
            this.Session.Derive();

            shipment.Ship();
            this.Session.Derive();

            var salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice1 = (SalesInvoice)salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;
            invoice1.Send();

            new ReceiptBuilder(this.Session)
                .WithAmount(15)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(15).Build())
                .WithEffectiveDate(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).InProcess, order.SalesOrderState);
            Assert.Equal(new SalesOrderItemStates(this.Session).Finished, item1.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item2.SalesOrderItemState);
            Assert.Equal(new SalesOrderItemStates(this.Session).InProcess, item3.SalesOrderItemState);

            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            shipment = (CustomerShipment)item2.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

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

            salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
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

            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            shipment = (CustomerShipment)item3.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

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

            salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
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

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(10).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

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
                .WithActualUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item);

            this.Session.Derive();

            order1.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(3, shipment.ShipmentItems[0].Quantity);

            var pickList1 = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(3, pickList1.PickListItems[0].RequestedQuantity);

            pickList1.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

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
                .WithActualUnitPrice(5)
                .Build();

            order2.AddSalesOrderItem(item);

            this.Session.Derive();

            order2.Confirm();

            this.Session.Derive();

            Assert.Equal(5, shipment.ShipmentItems.First.Quantity);

            var pickList2 = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[1].PickListItem.PickListWherePickListItem;
            Assert.Equal(2, pickList2.PickListItems[0].RequestedQuantity);
        }

        [Fact]
        public void GivenSalesOrderWithPendingShipmentAndAssignedPickList_WhenOrderIsCancelled_ThenNegativePickListIsCreatedAndSingleOrderShipmentIsCancelled()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(10).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.Equal(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(10, pickList.PickListItems[0].RequestedQuantity);

            pickList.Picker = new People(this.Session).FindBy(M.Person.LastName, "orderProcessor");

            this.Session.Derive();

            order.Cancel();

            this.Session.Derive();

            var negativePickList = order.ShipToCustomer.PickListsWhereShipToParty[1];
            Assert.Equal(-10, negativePickList.PickListItems[0].RequestedQuantity);

            Assert.Equal(new CustomerShipmentStates(this.Session).Cancelled, shipment.CustomerShipmentState);
        }

        [Fact]
        public void GivenSalesOrderWithPickList_WhenOrderIsCancelled_ThenPickListIsCancelledAndSingleOrderShipmentIsCancelled()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

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
                .WithActualUnitPrice(5)
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
                .WithActualUnitPrice(5)
                .Build();

            order2.AddSalesOrderItem(item);

            this.Session.Derive();

            order2.Confirm();

            this.Session.Derive();

            var shipment = (CustomerShipment)customer.ShipmentsWhereBillToParty[0];
            Assert.Equal(30, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.Equal(30, pickList.PickListItems[0].RequestedQuantity);

            order1.Cancel();

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Created, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.Session).Created, pickList.PickListState);
            Assert.Equal(20, pickList.PickListItems[0].RequestedQuantity);

            order2.Cancel();

            this.Session.Derive();

            Assert.Equal(new CustomerShipmentStates(this.Session).Cancelled, shipment.CustomerShipmentState);
            Assert.Equal(new PickListStates(this.Session).Cancelled, pickList.PickListState);
        }

        [Fact]
        public void GivenSalesOrderOnHold_WhenInventoryBecomesAvailable_ThenOrderIsNotSelectedForShipment()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventory = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            order.Hold();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityShortFalled);

            inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);
        }

        [Fact]
        public void GivenSalesOrderOnHold_WhenOrderIsContinued_ThenOrderIsSelectedForShipment()
        {
            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventory = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            order.Hold();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityShortFalled);

            inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            Assert.Equal(0, item.QuantityPendingShipment);
            Assert.Equal(10, item.QuantityRequestsShipping);
            Assert.Equal(0, item.QuantityShortFalled);

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

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventoryGood1 = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good1).Build();
            inventoryGood1.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(10).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("20202")
                .WithVatRate(vatRate0)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventoryGood2 = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good2).Build();
            inventoryGood2.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(10).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithPartiallyShip(false)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good1)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good2)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
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

            inventoryGood1.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            Assert.False(customer.ExistShipmentsWhereShipToParty);

            Assert.Equal(20, item1.QuantityRequestsShipping);
            Assert.Equal(0, item1.QuantityPendingShipment);
            Assert.Equal(0, item1.QuantityShortFalled);

            Assert.Equal(10, item2.QuantityRequestsShipping);
            Assert.Equal(0, item2.QuantityPendingShipment);
            Assert.Equal(10, item2.QuantityShortFalled);

            inventoryGood2.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

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
            var productItem = new SalesInvoiceItemTypes(this.Session).ProductItem;
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithFromDate(DateTime.UtcNow.AddYears(-2))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddYears(-1))
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(10).WithActualUnitPrice(100).WithSalesInvoiceItemType(productItem).Build();
            invoice.AddSalesInvoiceItem(invoiceItem);

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
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
            var productItem = new SalesInvoiceItemTypes(this.Session).ProductItem;
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var assessable = new VatRegimes(this.Session).Assessable;
            var vatRate0 = new VatRateBuilder(this.Session).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            customer.CreditLimit = 100M;

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithFromDate(DateTime.UtcNow.AddYears(-2))
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddYears(-1))
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(10).WithActualUnitPrice(11).WithSalesInvoiceItemType(productItem).Build();
            invoice.AddSalesInvoiceItem(invoiceItem);

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
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

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
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
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(1)
                .WithActualUnitPrice(0.1M)
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

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
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
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(50)
                .WithActualUnitPrice(50)
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.Session)
                .WithName("part1")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();
            var part2 = new FinishedGoodBuilder(this.Session)
                .WithName("part2")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .WithBillToContactMechanism(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(3, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(7, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);

            order.Reject();

            this.Session.Derive();

            Assert.Equal(0, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(0, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);
        }

        [Fact]
        public void GivenConfirmedOrder_WhenOrderIsCancelled_ThenNonSerialisedInventoryQuantitiesAreReleased()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.Session)
                .WithName("part1")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();
            var part2 = new FinishedGoodBuilder(this.Session)
                .WithName("part2")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .WithBillToContactMechanism(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(3, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(7, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);

            order.Cancel();

            this.Session.Derive();

            Assert.Equal(0, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(0, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);
        }

        [Fact]
        public void GivenSalesOrder_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Provisional, order.SalesOrderState);
            Assert.Equal(order.LastSalesOrderState, order.SalesOrderState);
        }

        [Fact]
        public void GivenSalesOrder_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Null(order.PreviousSalesOrderState);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirmed_ThenCurrentOrderStatusMustBeDerived()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.Session)
                .WithName("part1")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();
            var part2 = new FinishedGoodBuilder(this.Session)
                .WithName("part2")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithOrganisation(this.InternalOrganisation)
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
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
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            new SalesOrderBuilder(this.Session).WithBillToCustomer(billToCustomer).WithTakenBy(this.InternalOrganisation).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesOrder_WhenGettingOrderNumberWithoutFormat_ThenOrderNumberShouldBeReturned()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.Session).WithName("store")
                .WithDefaultFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .Build();

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.Session.Derive();

            Assert.Equal("1", order1.OrderNumber);

            var order2 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.Session).WithName("store")
                .WithDefaultFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .WithSalesOrderNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .Build();

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.Session.Derive();

            Assert.Equal("the format is 1", order1.OrderNumber);

            var order2 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
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
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithFromDate(DateTime.UtcNow)
                .WithCustomer(customer)
                .WithSalesRepresentative(salesrep)
                .Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.Session)
                .WithName("part1")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            order.AddSalesOrderItem(new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(10).Build());

            this.Session.Derive();

            Assert.Contains(salesrep, order.SalesReps);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenTakenByContactMechanismMustExist()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            var internalOrganisation = this.InternalOrganisation;

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var orderContact = new EmailAddressBuilder(this.Session).WithElectronicAddressString("orders@acme.com").Build();

            internalOrganisation.OrderAddress = orderContact;

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(orderContact, order1.TakenByContactMechanism);
        }

        [Fact]
        public void GivenSalesOrder_WhenDeriving_ThenBillFromContactMechanismMustExist()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            var internalOrganisation = this.InternalOrganisation;

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var billingContact = new EmailAddressBuilder(this.Session).WithElectronicAddressString("orders@acme.com").Build();

            internalOrganisation.BillingAddress = billingContact;

            this.Session.Derive();

            var order1 = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(billingContact, order1.BillFromContactMechanism);
        }

        [Fact]
        public void GivenSalesOrderWithBillToCustomerWithPreferredCurrency_WhenBuild_ThenCurrencyIsFromCustomer()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var englischLocale = new Locales(this.Session).EnglishGreatBritain;
            var poundSterling = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "GBP");

            var customer = new OrganisationBuilder(this.Session).WithName("customer").WithLocale(englischLocale).WithPreferredCurrency(poundSterling).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
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
            var shipToContactMechanism = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

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
        public void GivenSalesOrder_WhenObjectStateIsProvisional_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            this.SetIdentity(Users.AdministratorUserName);

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(new SalesOrderStates(this.Session).Provisional, order.SalesOrderState);
            var acl = new AccessControlList(order, this.Session.GetUser());
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
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            this.SetIdentity(Users.AdministratorUserName);

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();
            this.Session.Commit();

            var acl = new AccessControlList(order, this.Session.GetUser());
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
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            this.SetIdentity("customer");

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Cancel();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Cancelled, order.SalesOrderState);
            var acl = new AccessControlList(order, this.Session.GetUser());
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            this.SetIdentity("customer");

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Reject();

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Rejected, order.SalesOrderState);
            var acl = new AccessControlList(order, this.Session.GetUser());
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            this.SetIdentity("customer");

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            order.SalesOrderState = new SalesOrderStates(this.Session).Finished;

            this.Session.Derive();

            Assert.Equal(new SalesOrderStates(this.Session).Finished, order.SalesOrderState);
            var acl = new AccessControlList(order, this.Session.GetUser());
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            this.SetIdentity(Users.AdministratorUserName);

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            order.Hold();

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(new SalesOrderStates(this.Session).OnHold, order.SalesOrderState);
            var acl = new AccessControlList(order, this.Session.GetUser());
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
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.Session).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
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
            Assert.Equal(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Fact]
        public void GivenSalesOrderWithShippingAndHandlingPercentage_WhenDeriving_ThenOrderTotalsMustIncludeShippingAndHandlingAmount()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.Session).WithPercentage(5).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
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
            Assert.Equal(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Fact]
        public void GivenSalesOrderWithFeeAmount_WhenDeriving_ThenOrderTotalsMustIncludeFeeAmount()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.Session).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithFee(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
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
            Assert.Equal(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Fact]
        public void GivenSalesOrderWithFeePercentage_WhenDeriving_ThenOrderTotalsMustIncludeFeeAmount()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.Session).WithPercentage(5).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithFee(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
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
            Assert.Equal(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirming_ThenInventoryItemsQuantityCommittedOutAndAvailableToPromiseMustBeUpdated()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.Session)
                .WithName("part1")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();
            var part2 = new FinishedGoodBuilder(this.Session)
                .WithName("part2")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(6, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.Equal(3, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.Equal(0, item3.ReservedFromInventoryItem.AvailableToPromise);
        }

        [Fact]
        public void GivenSalesOrder_WhenChangingItemQuantityToZero_ThenItemIsInvalid()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.Session)
                .WithName("part1")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();
            var part2 = new FinishedGoodBuilder(this.Session)
                .WithName("part2")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.Session.Derive();
            Assert.Equal(4, order.ValidOrderItems.Count);

            order.Confirm();

            this.Session.Derive();

            item4.QuantityOrdered = 0;

            this.Session.Derive();

            Assert.Equal(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Fact]
        public void GivenSalesOrder_WhenOrderItemIsWithoutBasePrice_ThenItemIsInvalid()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.Session)
                .WithName("part1")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();
            var part2 = new FinishedGoodBuilder(this.Session)
                .WithName("part2")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            order.Confirm();

            this.Session.Derive();

            item4.RemoveActualUnitPrice();

            this.Session.Derive();

            Assert.Equal(0, item4.UnitBasePrice);
            Assert.Equal(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Fact]
        public void GivenSalesOrder_WhenConfirming_ThenAllValidItemsAreInConfirmedState()
        {
            var billToCustomer = new PersonBuilder(this.Session).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.Session).WithLastName("person2").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.Session)
                .WithName("part1")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();
            var part2 = new FinishedGoodBuilder(this.Session)
                .WithName("part2")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

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
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1InventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var good2InventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
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

            var shipment = customer.ShipmentsWhereBillToParty.First;

            Assert.Equal(2, shipment.ShipmentItems.Count);
            Assert.Equal(1, item1.OrderShipmentsWhereSalesOrderItem[0].Quantity);
            Assert.Equal(2, item2.OrderShipmentsWhereSalesOrderItem[0].Quantity);
            Assert.Equal(5, item3.OrderShipmentsWhereSalesOrderItem[0].Quantity);
        }

        [Fact]
        public void GivenSalesOrderWithMultipleRecipients_WhenConfirmed_ThenShipmentIsCreatedForEachRecipientAndPickListIsCreated()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var baal = new CityBuilder(this.Session).WithName("Baal").Build();
            var baalAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(baal).WithAddress1("Haverwerf 15").Build();
            var shipToBaal = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(baalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var person1 = new PersonBuilder(this.Session).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).Build();
            var person2 = new PersonBuilder(this.Session).WithLastName("person2").WithPartyContactMechanism(shipToBaal).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(person1).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(person2).WithInternalOrganisation(this.InternalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var part = new FinishedGoodBuilder(this.Session)
                .WithName("part1")
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.Session.Derive();

            var partInventory = (NonSerialisedInventoryItem)part.InventoryItemsWherePart[0];
            partInventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Unknown).Build());

            this.Session.Derive();

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithFinishedGood(part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

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

            var item1 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.Session).WithItemType(new SalesInvoiceItemTypes(this.Session).ProductFeatureItem).WithProductFeature(colorBlack).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.Session).WithItemType(new SalesInvoiceItemTypes(this.Session).ProductFeatureItem).WithProductFeature(extraLarge).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            item1.AddOrderedWithFeature(item2);
            item1.AddOrderedWithFeature(item3);
            var item4 = new SalesOrderItemBuilder(this.Session).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item5 = new SalesOrderItemBuilder(this.Session).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).WithAssignedShipToParty(person2).Build();
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
            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(1, order.Customers.Count);
            Assert.Equal(customer, order.Customers.First);
        }

        [Fact]
        public void GivenSalesOrder_WhenShipToAndBillToAreDifferentCustomers_ThenDerivedCustomersHoldsBothCustomers()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var billToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var shipToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(billToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).WithInternalOrganisation(this.InternalOrganisation).Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToCustomer(billToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
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
                .WithParent(parentProductCategory).
                Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var customer = new OrganisationBuilder(this.Session).WithName("company").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(this.InternalOrganisation).Build();

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

            var good1 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithProductCategory(childProductCategory)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithProductCategory(parentProductCategory)
                .Build();

            var good3 = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good1)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item1);

            this.Session.Derive();

            Assert.Equal(1, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);

            var item2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good2)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item2);

            this.Session.Derive();

            Assert.Equal(2, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);
            Assert.Contains(salesrep2, order.SalesReps);

            var item3 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good3)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item3);

            this.Session.Derive();

            Assert.Equal(3, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);
            Assert.Contains(salesrep2, order.SalesReps);
            Assert.Contains(salesrep3, order.SalesReps);
        }
    }
}
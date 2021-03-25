// <copyright file="PartWeightedAverageTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Xunit;

    public class PartWeightedAverageTests : DomainTest
    {
        [Fact]
        public void GivenNonSerialisedUnifiedGood_WhenPurchased_ThenAverageCostIsCalculated()
        {
            this.InternalOrganisation.IsAutomaticallyReceived = true;
            var defaultFacility = this.InternalOrganisation.StoresWhereInternalOrganisation.Single().DefaultFacility;

            var secondFacility = new FacilityBuilder(this.Session)
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithName("second facility")
                .WithOwner(this.InternalOrganisation)
                .Build();

            var supplier = this.InternalOrganisation.ActiveSuppliers.First;
            var customer = this.InternalOrganisation.ActiveCustomers.First;

            var part = new UnifiedGoodBuilder(this.Session).WithNonSerialisedDefaults(this.InternalOrganisation).Build();
            this.Session.Derive();

            var purchaseOrder1 = new PurchaseOrderBuilder(this.Session)
                .WithTakenViaSupplier(supplier)
                .WithStoredInFacility(defaultFacility)
                .WithDeliveryDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            // Beginning inventory: 150 items at 8 euro received in 2 facilities
            var purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(100).WithAssignedUnitPrice(8M).Build();
            purchaseOrder1.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder1.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder1.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            var purchaseOrder2 = new PurchaseOrderBuilder(this.Session)
                .WithTakenViaSupplier(supplier)
                .WithStoredInFacility(secondFacility)
                .WithDeliveryDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            // Beginning inventory: 150 items at 8 euro
            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(50).WithAssignedUnitPrice(8M).Build();
            purchaseOrder2.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder2.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder2.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            Assert.Equal(150, part.QuantityOnHand);
            Assert.Equal(8, part.PartWeightedAverage.AverageCost);

            purchaseOrder1.Revise();
            this.Session.Derive();

            // Purchase: 75 items at 8.1 euro
            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(75).WithAssignedUnitPrice(8.1M).Build();
            purchaseOrder1.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder1.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder1.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            Assert.Equal(225, part.QuantityOnHand);
            Assert.Equal(8.03M, part.PartWeightedAverage.AverageCost);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(customer)
                .Build();

            this.Session.Derive();

            // Sell 50 items for 20 euro
            var salesItem1 = new SalesOrderItemBuilder(this.Session).WithProduct(part).WithQuantityOrdered(50).WithAssignedUnitPrice(20M).Build();
            salesOrder.AddSalesOrderItem(salesItem1);

            this.Session.Derive();

            salesOrder.SetReadyForPosting();
            this.Session.Derive();

            salesOrder.Post();
            this.Session.Derive();

            salesOrder.Accept();
            this.Session.Derive();

            salesOrder.Ship();
            this.Session.Derive();

            var customerShipment = salesItem1.OrderShipmentsWhereOrderItem.First.ShipmentItem.ShipmentWhereShipmentItem as CustomerShipment;

            customerShipment.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in customerShipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            customerShipment.Ship();
            this.Session.Derive();

            Assert.Equal(175, part.QuantityOnHand);
            Assert.Equal(8.03M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(401.5M, salesItem1.CostOfGoodsSold);

            // Again Sell 50 items for 20 euro
            salesOrder.Revise();
            this.Session.Derive();

            var salesItem2 = new SalesOrderItemBuilder(this.Session).WithProduct(part).WithQuantityOrdered(50).WithAssignedUnitPrice(20M).Build();
            salesOrder.AddSalesOrderItem(salesItem2);

            this.Session.Derive();

            salesOrder.SetReadyForPosting();
            this.Session.Derive();

            salesOrder.Post();
            this.Session.Derive();

            salesOrder.Accept();
            this.Session.Derive();

            salesOrder.Ship();
            this.Session.Derive();

            var customerShipment2 = salesItem2.OrderShipmentsWhereOrderItem.First.ShipmentItem.ShipmentWhereShipmentItem as CustomerShipment;

            customerShipment2.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            var package2 = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment2.AddShipmentPackage(package2);

            foreach (ShipmentItem shipmentItem in customerShipment2.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            customerShipment2.Ship();
            this.Session.Derive();

            Assert.Equal(125, part.QuantityOnHand);
            Assert.Equal(8.03M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(401.5M, salesItem1.CostOfGoodsSold);

            // Purchase: 50 items at 8.25 euro
            purchaseOrder1.Revise();
            this.Session.Derive();

            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(50).WithAssignedUnitPrice(8.25M).Build();
            purchaseOrder1.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder1.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder1.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            Assert.Equal(175, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);

            // Use 65 items in a workorder
            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(this.InternalOrganisation).Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(65)
                .Build();

            this.Session.Derive(true);

            Assert.Equal(110, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(525.85M, inventoryAssignment.CostOfGoodsSold);

            // Cancel workeffort inventory assignment
            inventoryAssignment.Delete();

            this.Session.Derive(true);

            Assert.Equal(175, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);

            // Use 35 items in a workorder
            inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(35)
                .Build();

            this.Session.Derive(true);

            this.Session.Derive(true);

            Assert.Equal(140, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(283.15M, inventoryAssignment.CostOfGoodsSold);

            // Use 30 items in a workorder form second facility
            inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First(v => v.Facility.Equals(secondFacility)))
                .WithQuantity(30)
                .Build();

            this.Session.Derive(true);

            Assert.Equal(110, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(242.7M, inventoryAssignment.CostOfGoodsSold);

            // Purchase: 90 items at 8.35 euro
            var purchaseOrder3 = new PurchaseOrderBuilder(this.Session)
                .WithTakenViaSupplier(supplier)
                .WithStoredInFacility(defaultFacility)
                .WithDeliveryDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(90).WithAssignedUnitPrice(8.35M).Build();
            purchaseOrder3.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder3.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder3.Send();
            this.Session.Derive();

            purchaseOrder3.QuickReceive();
            this.Session.Derive();

            // Purchase: 50 items at 8.45 euro
            var purchaseOrder4 = new PurchaseOrderBuilder(this.Session)
                .WithTakenViaSupplier(supplier)
                .WithStoredInFacility(defaultFacility)
                .WithDeliveryDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(50).WithAssignedUnitPrice(8.45M).Build();
            purchaseOrder4.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder4.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder4.Send();
            this.Session.Derive();

            purchaseOrder4.QuickReceive();
            this.Session.Derive();

            Assert.Equal(250, part.QuantityOnHand);
            Assert.Equal(8.26M, part.PartWeightedAverage.AverageCost);

            // Ship 10 items to customer (without sales order)
            var outgoingShipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(customer.ShippingAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive(true);

            var outgoingItem = new ShipmentItemBuilder(this.Session).WithGood(part).WithQuantity(10).Build();
            outgoingShipment.AddShipmentItem(outgoingItem);

            this.Session.Derive(true);

            outgoingShipment.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            package = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment2.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in outgoingShipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            outgoingShipment.Ship();
            this.Session.Derive();

            Assert.Equal(240, part.QuantityOnHand);
            Assert.Equal(8.26M, part.PartWeightedAverage.AverageCost);

            // Receive 10 items at 8.55 from supplier (without purchase order)
            var incomingShipment = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            var incomingItem = new ShipmentItemBuilder(this.Session).WithPart(part).WithQuantity(10).WithUnitPurchasePrice(8.55M).Build();
            incomingShipment.AddShipmentItem(incomingItem);

            this.Session.Derive();

            incomingShipment.Receive();
            this.Session.Derive();

            Assert.Equal(250, part.QuantityOnHand);
            Assert.Equal(8.27M, part.PartWeightedAverage.AverageCost);

            // Receive 100 items at 7.9 from supplier (without purchase order)
            incomingShipment = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            incomingItem = new ShipmentItemBuilder(this.Session).WithPart(part).WithQuantity(100).WithUnitPurchasePrice(7.9M).Build();
            incomingShipment.AddShipmentItem(incomingItem);

            this.Session.Derive();

            incomingShipment.Receive();
            this.Session.Derive();

            Assert.Equal(350, part.QuantityOnHand);
            Assert.Equal(8.17M, part.PartWeightedAverage.AverageCost);

            // Ship all items to customer (without sales order)
            outgoingShipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipFromFacility(part.DefaultFacility)
                .WithShipToAddress(customer.ShippingAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive(true);

            outgoingItem = new ShipmentItemBuilder(this.Session).WithGood(part).WithQuantity(330).Build();
            outgoingShipment.AddShipmentItem(outgoingItem);

            this.Session.Derive(true);

            outgoingShipment.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            package = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment2.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in outgoingShipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            outgoingShipment.Ship();
            this.Session.Derive();

            // Ship all items to customer (without sales order)
            outgoingShipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipFromFacility(secondFacility)
                .WithShipToAddress(customer.ShippingAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive(true);

            outgoingItem = new ShipmentItemBuilder(this.Session).WithGood(part).WithQuantity(20).Build();
            outgoingShipment.AddShipmentItem(outgoingItem);

            this.Session.Derive(true);

            outgoingShipment.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            package = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment2.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in outgoingShipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            outgoingShipment.Ship();
            this.Session.Derive();

            Assert.Equal(0, part.QuantityOnHand);
            Assert.Equal(8.17M, part.PartWeightedAverage.AverageCost);

            purchaseOrder1.Revise();
            this.Session.Derive();

            // Purchase 150 items at 8 euro
            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(150).WithAssignedUnitPrice(8M).Build();
            purchaseOrder1.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder1.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder1.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            Assert.Equal(150, part.QuantityOnHand);
            Assert.Equal(8, part.PartWeightedAverage.AverageCost);
        }

        [Fact]
        public void GivenNonSerialisedNonUnifiedPart_WhenPurchased_ThenAverageCostIsCalculated()
        {
            this.InternalOrganisation.IsAutomaticallyReceived = true;
            var defaultFacility = this.InternalOrganisation.StoresWhereInternalOrganisation.Single().DefaultFacility;

            var secondFacility = new FacilityBuilder(this.Session)
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithName("second facility")
                .WithOwner(this.InternalOrganisation)
                .Build();

            var supplier = this.InternalOrganisation.ActiveSuppliers.First;
            var customer = this.InternalOrganisation.ActiveCustomers.First;

            var part = new NonUnifiedPartBuilder(this.Session).WithNonSerialisedDefaults(this.InternalOrganisation).Build();
            var good = new NonUnifiedGoodBuilder(this.Session)
                .WithName(part.Name)
                .WithPart(part)
                .WithVatRegime(new VatRegimes(this.Session).ZeroRated)
                .Build();

            this.Session.Derive();

            var purchaseOrder1 = new PurchaseOrderBuilder(this.Session)
                .WithTakenViaSupplier(supplier)
                .WithDeliveryDate(this.Session.Now())
                .WithStoredInFacility(defaultFacility)
                .Build();

            this.Session.Derive();

            // Beginning inventory: 150 items at 8 euro received in 2 facilities
            var purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(100).WithAssignedUnitPrice(8M).Build();
            purchaseOrder1.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder1.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder1.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            var purchaseOrder2 = new PurchaseOrderBuilder(this.Session)
                .WithTakenViaSupplier(supplier)
                .WithStoredInFacility(secondFacility)
                .WithDeliveryDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            // Beginning inventory: 150 items at 8 euro
            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(50).WithAssignedUnitPrice(8M).Build();
            purchaseOrder2.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder2.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder2.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            Assert.Equal(150, part.QuantityOnHand);
            Assert.Equal(8, part.PartWeightedAverage.AverageCost);

            purchaseOrder1.Revise();
            this.Session.Derive();

            // Purchase: 75 items at 8.1 euro
            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(75).WithAssignedUnitPrice(8.1M).Build();
            purchaseOrder1.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder1.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder1.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            Assert.Equal(225, part.QuantityOnHand);
            Assert.Equal(8.03M, part.PartWeightedAverage.AverageCost);

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(customer)
                .Build();

            this.Session.Derive();

            // Sell 50 items for 20 euro
            var salesItem1 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(50).WithAssignedUnitPrice(20M).Build();
            salesOrder.AddSalesOrderItem(salesItem1);

            this.Session.Derive();

            salesOrder.SetReadyForPosting();
            this.Session.Derive();

            salesOrder.Post();
            this.Session.Derive();

            salesOrder.Accept();
            this.Session.Derive();

            salesOrder.Ship();
            this.Session.Derive();

            var customerShipment = salesItem1.OrderShipmentsWhereOrderItem.First.ShipmentItem.ShipmentWhereShipmentItem as CustomerShipment;

            customerShipment.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            var package = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in customerShipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            customerShipment.Ship();
            this.Session.Derive();

            Assert.Equal(175, part.QuantityOnHand);
            Assert.Equal(8.03M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(401.5M, salesItem1.CostOfGoodsSold);

            // Again Sell 50 items for 20 euro
            salesOrder.Revise();
            this.Session.Derive();

            var salesItem2 = new SalesOrderItemBuilder(this.Session).WithProduct(good).WithQuantityOrdered(50).WithAssignedUnitPrice(20M).Build();
            salesOrder.AddSalesOrderItem(salesItem2);

            this.Session.Derive();

            salesOrder.SetReadyForPosting();
            this.Session.Derive();

            salesOrder.Post();
            this.Session.Derive();

            salesOrder.Accept();
            this.Session.Derive();

            salesOrder.Ship();
            this.Session.Derive();

            var customerShipment2 = salesItem2.OrderShipmentsWhereOrderItem.First.ShipmentItem.ShipmentWhereShipmentItem as CustomerShipment;

            customerShipment2.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            var package2 = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment2.AddShipmentPackage(package2);

            foreach (ShipmentItem shipmentItem in customerShipment2.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            customerShipment2.Ship();
            this.Session.Derive();

            Assert.Equal(125, part.QuantityOnHand);
            Assert.Equal(8.03M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(401.5M, salesItem1.CostOfGoodsSold);

            // Purchase: 50 items at 8.25 euro
            purchaseOrder1.Revise();
            this.Session.Derive();

            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(50).WithAssignedUnitPrice(8.25M).Build();
            purchaseOrder1.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder1.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder1.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            Assert.Equal(175, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);

            // Use 65 items in a workorder
            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(this.InternalOrganisation).Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(65)
                .Build();

            this.Session.Derive(true);

            Assert.Equal(110, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(525.85M, inventoryAssignment.CostOfGoodsSold);

            // Cancel workeffort inventory assignment
            inventoryAssignment.Delete();

            this.Session.Derive(true);

            Assert.Equal(175, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);

            // Use 35 items in a workorder
            inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(35)
                .Build();

            this.Session.Derive(true);

            this.Session.Derive(true);

            Assert.Equal(140, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(283.15M, inventoryAssignment.CostOfGoodsSold);

            // Use 30 items in a workorder form second facility
            inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First(v => v.Facility.Equals(secondFacility)))
                .WithQuantity(30)
                .Build();

            this.Session.Derive(true);

            Assert.Equal(110, part.QuantityOnHand);
            Assert.Equal(8.09M, part.PartWeightedAverage.AverageCost);
            Assert.Equal(242.7M, inventoryAssignment.CostOfGoodsSold);

            // Purchase: 90 items at 8.35 euro
            var purchaseOrder3 = new PurchaseOrderBuilder(this.Session)
                .WithTakenViaSupplier(supplier)
                .WithStoredInFacility(defaultFacility)
                .WithDeliveryDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(90).WithAssignedUnitPrice(8.35M).Build();
            purchaseOrder3.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder3.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder3.Send();
            this.Session.Derive();

            purchaseOrder3.QuickReceive();
            this.Session.Derive();

            // Purchase: 50 items at 8.45 euro
            var purchaseOrder4 = new PurchaseOrderBuilder(this.Session)
                .WithTakenViaSupplier(supplier)
                .WithStoredInFacility(defaultFacility)
                .WithDeliveryDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(50).WithAssignedUnitPrice(8.45M).Build();
            purchaseOrder4.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder4.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder4.Send();
            this.Session.Derive();

            purchaseOrder4.QuickReceive();
            this.Session.Derive();

            Assert.Equal(250, part.QuantityOnHand);
            Assert.Equal(8.26M, part.PartWeightedAverage.AverageCost);

            // Ship 10 items to customer (without sales order)
            var outgoingShipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(customer.ShippingAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive(true);

            var outgoingItem = new ShipmentItemBuilder(this.Session).WithGood(good).WithQuantity(10).Build();
            outgoingShipment.AddShipmentItem(outgoingItem);

            this.Session.Derive(true);

            outgoingShipment.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            package = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment2.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in outgoingShipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            outgoingShipment.Ship();
            this.Session.Derive();

            Assert.Equal(240, part.QuantityOnHand);
            Assert.Equal(8.26M, part.PartWeightedAverage.AverageCost);

            // Receive 10 items at 8.55 from supplier (without purchase order)
            var incomingShipment = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            var incomingItem = new ShipmentItemBuilder(this.Session).WithPart(part).WithQuantity(10).WithUnitPurchasePrice(8.55M).Build();
            incomingShipment.AddShipmentItem(incomingItem);

            this.Session.Derive();

            incomingShipment.Receive();
            this.Session.Derive();

            Assert.Equal(250, part.QuantityOnHand);
            Assert.Equal(8.27M, part.PartWeightedAverage.AverageCost);

            // Receive 100 items at 7.9 from supplier (without purchase order)
            incomingShipment = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(supplier)
                .Build();

            this.Session.Derive();

            incomingItem = new ShipmentItemBuilder(this.Session).WithPart(part).WithQuantity(100).WithUnitPurchasePrice(7.9M).Build();
            incomingShipment.AddShipmentItem(incomingItem);

            this.Session.Derive();

            incomingShipment.Receive();
            this.Session.Derive();

            Assert.Equal(350, part.QuantityOnHand);
            Assert.Equal(8.17M, part.PartWeightedAverage.AverageCost);

            // Ship all items to customer (without sales order)
            outgoingShipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipFromFacility(part.DefaultFacility)
                .WithShipToAddress(customer.ShippingAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive(true);

            outgoingItem = new ShipmentItemBuilder(this.Session).WithGood(good).WithQuantity(330).Build();
            outgoingShipment.AddShipmentItem(outgoingItem);

            this.Session.Derive(true);

            outgoingShipment.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            package = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment2.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in outgoingShipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            outgoingShipment.Ship();
            this.Session.Derive();

            // Ship all items to customer (without sales order)
            outgoingShipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipFromFacility(secondFacility)
                .WithShipToAddress(customer.ShippingAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive(true);

            outgoingItem = new ShipmentItemBuilder(this.Session).WithGood(good).WithQuantity(20).Build();
            outgoingShipment.AddShipmentItem(outgoingItem);

            this.Session.Derive(true);

            outgoingShipment.Pick();
            this.Session.Derive();

            customer.PickListsWhereShipToParty.First(v => v.PickListState.Equals(new PickListStates(this.Session).Created)).SetPicked();

            package = new ShipmentPackageBuilder(this.Session).Build();
            customerShipment2.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in outgoingShipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            outgoingShipment.Ship();
            this.Session.Derive();

            Assert.Equal(0, part.QuantityOnHand);
            Assert.Equal(8.17M, part.PartWeightedAverage.AverageCost);

            purchaseOrder1.Revise();
            this.Session.Derive();

            // Purchase 150 items at 8 euro
            purchaseItem = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(150).WithAssignedUnitPrice(8M).Build();
            purchaseOrder1.AddPurchaseOrderItem(purchaseItem);

            this.Session.Derive();

            purchaseOrder1.SetReadyForProcessing();
            this.Session.Derive();

            purchaseOrder1.Send();
            this.Session.Derive();

            purchaseItem.QuickReceive();
            this.Session.Derive();

            Assert.Equal(150, part.QuantityOnHand);
            Assert.Equal(8, part.PartWeightedAverage.AverageCost);
        }
    }
}

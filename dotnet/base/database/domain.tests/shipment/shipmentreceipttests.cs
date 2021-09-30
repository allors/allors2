// <copyright file="ShipmentReceiptTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Xunit;

    public class ShipmentReceiptTests : DomainTest
    {
        [Fact]
        public void GivenShipmentReceiptBuilderWhenBuildThenPostBuildRelationsMustExist()
        {
            var receipt = new ShipmentReceiptBuilder(this.Session).Build();

            Assert.NotNull(receipt.ReceivedDateTime);
            Assert.Equal(0, receipt.QuantityAccepted);
            Assert.Equal(0, receipt.QuantityRejected);
        }

        [Fact]
        public void GivenShipmentReceiptWhenValidatingThenRequiredRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();

            this.Session.Derive();
            this.Session.Commit();

            var inventoryItem = good1.Part.InventoryItemsWherePart.First;
            var builder = new ShipmentReceiptBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithInventoryItem(inventoryItem);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            var shipment = new PurchaseShipmentBuilder(this.Session).WithShipmentMethod(new ShipmentMethods(this.Session).Ground).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.Session).WithGood(good1).Build();
            shipment.AddShipmentItem(shipmentItem);

            builder.WithShipmentItem(shipmentItem);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenShipmentReceiptForPartWithoutSelectedInventoryItemWhenDerivingThenInventoryItemIsFromDefaultFacility()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            var order = new PurchaseOrderBuilder(this.Session)
                .WithTakenViaSupplier(supplier)
                .WithAssignedVatRegime(new VatRegimes(this.Session).ZeroRated)
                .Build();

            var item1 = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(1).Build();
            order.AddPurchaseOrderItem(item1);

            this.Session.Derive();

            order.SetReadyForProcessing();
            this.Session.Derive();

            order.QuickReceive();
            this.Session.Derive();

            var shipment = (PurchaseShipment)item1.OrderShipmentsWhereOrderItem.First.ShipmentItem.ShipmentWhereShipmentItem;
            shipment.Receive();
            this.Session.Derive();

            var receipt = item1.ShipmentReceiptsWhereOrderItem.Single();

            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), receipt.InventoryItem.Facility);
            Assert.Equal(part.InventoryItemsWherePart[0], receipt.InventoryItem);
        }

        [Fact]
        public void GivenShipmentReceiptForGoodWithoutSelectedInventoryItemWhenDerivingThenInventoryItemIsFromDefaultFacility()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            var order = new PurchaseOrderBuilder(this.Session).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.Session).WithPart(good1.Part).WithQuantityOrdered(1).Build();
            order.AddPurchaseOrderItem(item1);

            this.Session.Derive();
            this.Session.Commit();

            order.SetReadyForProcessing();
            this.Session.Derive();

            order.QuickReceive();
            this.Session.Derive();

            var shipment = (PurchaseShipment)item1.OrderShipmentsWhereOrderItem.First.ShipmentItem.ShipmentWhereShipmentItem;
            shipment.Receive();
            this.Session.Derive();

            var receipt = item1.ShipmentReceiptsWhereOrderItem.Single();

            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), receipt.InventoryItem.Facility);
            Assert.Equal(good1.Part.InventoryItemsWherePart[0], receipt.InventoryItem);

            this.Session.Rollback();
        }

        [Fact]
        public void GivenShipmentReceiptWhenDerivingThenInventoryItemQuantityOnHandIsUpdated()
        {
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            new InventoryItemTransactionBuilder(this.Session).WithQuantity(20).WithReason(new InventoryTransactionReasons(this.Session).Unknown).WithPart(good1.Part).Build();

            this.Session.Derive();

            var inventoryItem = good1.Part.InventoryItemsWherePart.First;

            var order1 = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithDeliveryDate(this.Session.Now())
                .Build();

            var salesItem = new SalesOrderItemBuilder(this.Session).WithDescription("item1").WithProduct(good1).WithQuantityOrdered(30).WithAssignedUnitPrice(15).Build();
            order1.AddSalesOrderItem(salesItem);

            this.Session.Derive();
            this.Session.Commit();

            order1.SetReadyForPosting();

            this.Session.Derive();

            order1.Post();
            this.Session.Derive();

            order1.Accept();
            this.Session.Derive();

            var sessionInventoryItem = (NonSerialisedInventoryItem)this.Session.Instantiate(inventoryItem);
            var sessionSalesItem = (SalesOrderItem)this.Session.Instantiate(salesItem);

            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            Assert.Equal(20, sessionSalesItem.QuantityPendingShipment);
            Assert.Equal(30, sessionSalesItem.QuantityReserved);
            Assert.Equal(10, sessionSalesItem.QuantityShortFalled);

            var order = new PurchaseOrderBuilder(this.Session).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.Session).WithPart(good1.Part).WithQuantityOrdered(10).Build();
            order.AddPurchaseOrderItem(item1);

            this.Session.Derive();
            this.Session.Commit();

            order.SetReadyForProcessing();

            order.OrderedBy.IsAutomaticallyReceived = true;
            order.QuickReceive();

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(30, sessionInventoryItem.QuantityOnHand);

            Assert.Equal(30, sessionSalesItem.QuantityPendingShipment);
            Assert.Equal(30, sessionSalesItem.QuantityReserved);
            Assert.Equal(0, sessionSalesItem.QuantityShortFalled);
        }

        [Fact]
        public void GivenShipmentReceiptWhenDerivingThenOrderItemQuantityReceivedIsUpdated()
        {
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            var order = new PurchaseOrderBuilder(this.Session).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.Session).WithPart(good1.Part).WithQuantityOrdered(10).Build();
            order.AddPurchaseOrderItem(item1);

            this.Session.Derive();
            this.Session.Commit();

            order.SetReadyForProcessing();
            this.Session.Derive();

            order.QuickReceive();
            this.Session.Derive();

            var shipment = (PurchaseShipment)item1.OrderShipmentsWhereOrderItem.First.ShipmentItem.ShipmentWhereShipmentItem;
            shipment.Receive();
            this.Session.Derive();

            Assert.Equal(10, item1.QuantityReceived);

            this.Session.Rollback();
        }
    }
}

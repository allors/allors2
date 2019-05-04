//------------------------------------------------------------------------------------------------- 
// <copyright file="ShipmentReceiptTests.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using System;
    using Meta;
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
                .WithVatRegime(new VatRegimes(this.Session).Export)
                .Build();

            var item1 = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(1).Build();
            order.AddPurchaseOrderItem(item1);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            order.QuickReceive();

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

            order.Confirm();

            this.Session.Derive();

            order.QuickReceive();

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
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
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

            order1.Confirm();

            this.Session.Derive();
            this.Session.Commit();

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

            order.Confirm();

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

            order.Confirm();

            this.Session.Derive();

            order.QuickReceive();

            this.Session.Derive();

            Assert.Equal(10, item1.QuantityReceived);

            this.Session.Rollback();
        }
    }
}
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

namespace Allors.Domain
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ShipmentReceiptTests : DomainTest
    {
        [Test]
        public void GivenShipmentReceiptBuilderWhenBuildThenPostBuildRelationsMustExist()
        {
            var receipt = new ShipmentReceiptBuilder(this.DatabaseSession).Build();

            Assert.IsNotNull(receipt.ReceivedDateTime);
            Assert.AreEqual(0, receipt.QuantityAccepted);
            Assert.AreEqual(0, receipt.QuantityRejected);
        }

        [Test]
        public void GivenShipmentReceiptWhenValidatingThenRequiredRelationsMustExist()
        {
            throw new Exception("TODO");

            //var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            //var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            //new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            //var good = new GoodBuilder(this.DatabaseSession)
            //    .WithName("Good")
            //    .WithSku("10101")
            //    .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
            //    .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
            //    .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
            //    .Build();

            //var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).WithQuantityOnHand(100).Build();
            //this.DatabaseSession.Derive(true);
            //this.DatabaseSession.Commit();

            //var builder = new ShipmentReceiptBuilder(this.DatabaseSession);
            //builder.Build();

            //Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            //this.DatabaseSession.Rollback();

            //builder.WithInventoryItem(inventoryItem);
            //builder.Build();

            //Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            //this.DatabaseSession.Rollback();

            //var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipFromParty(supplier).Build();
            //var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            //shipment.AddShipmentItem(shipmentItem);

            //builder.WithShipmentItem(shipmentItem);
            //builder.Build();

            //Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenShipmentReceiptForPartWithoutSelectedInventoryItemWhenDerivingThenInventoryItemIsFromDefaultFacility()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();
            
            var part = new RawMaterialBuilder(this.DatabaseSession)
                .WithName("RawMaterial")
                .Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(1).Build();
            order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithPart(part).Build();
            shipment.AddShipmentItem(shipmentItem);

            var receipt = new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(1M)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item1)
                .Build();

            shipment.Complete();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Assert.AreEqual(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"), receipt.InventoryItem.Facility);
            Assert.AreEqual(part.InventoryItemsWherePart[0], receipt.InventoryItem);

            this.DatabaseSession.Rollback();
        }

        [Test]
        public void GivenShipmentReceiptForGoodWithoutSelectedInventoryItemWhenDerivingThenInventoryItemIsFromDefaultFacility()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(1).Build();
            order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            order.Confirm();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            var receipt = new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(1M)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item1)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            shipment.Complete();

            Assert.AreEqual(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"), receipt.InventoryItem.Facility);
            Assert.AreEqual(good.InventoryItemsWhereGood[0], receipt.InventoryItem);

            this.DatabaseSession.Rollback();
        }

        [Test]
        public void GivenShipmentReceiptWhenDerivingThenInventoryItemQuantityOnHandIsUpdated()
        {
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

            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(20).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithDeliveryDate(DateTime.UtcNow)
                .Build();

            var salesItem = new SalesOrderItemBuilder(this.DatabaseSession).WithDescription("item1").WithProduct(good).WithQuantityOrdered(30).WithActualUnitPrice(15).Build();
            order1.AddSalesOrderItem(salesItem);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            order1.Confirm();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var sessionInventoryItem = (NonSerializedInventoryItem)this.DatabaseSession.Instantiate(inventoryItem);
            var sessionSalesItem = (SalesOrderItem)this.DatabaseSession.Instantiate(salesItem);

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            Assert.AreEqual(20, sessionSalesItem.QuantityPendingShipment);
            Assert.AreEqual(30, sessionSalesItem.QuantityReserved);
            Assert.AreEqual(10, sessionSalesItem.QuantityShortFalled);

            var order = new PurchaseOrderBuilder(this.DatabaseSession).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(10).Build();
            order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            order.Confirm();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(10)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item1)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            shipment.Complete();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Assert.AreEqual(30, sessionInventoryItem.QuantityOnHand);

            Assert.AreEqual(30, sessionSalesItem.QuantityPendingShipment);
            Assert.AreEqual(30, sessionSalesItem.QuantityReserved);
            Assert.AreEqual(0, sessionSalesItem.QuantityShortFalled);
        }

        [Test]
        public void GivenShipmentReceiptWhenDerivingThenOrderItemQuantityReceivedIsUpdated()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(10).Build();
            order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            order.Confirm();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(10)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item1)
                .Build();

            shipment.Complete();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Assert.AreEqual(10, item1.QuantityReceived);

            this.DatabaseSession.Rollback();
        }
    }
}
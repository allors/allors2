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
    using Meta;
    using Xunit;

    public class ShipmentReceiptTests : DomainTest
    {
        [Fact]
        public void GivenShipmentReceiptBuilderWhenBuildThenPostBuildRelationsMustExist()
        {
            var receipt = new ShipmentReceiptBuilder(this.DatabaseSession).Build();

            Assert.NotNull(receipt.ReceivedDateTime);
            Assert.Equal(0, receipt.QuantityAccepted);
            Assert.Equal(0, receipt.QuantityRejected);
        }

        [Fact]
        public void GivenShipmentReceiptWhenValidatingThenRequiredRelationsMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            

            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Order).Build());
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var builder = new ShipmentReceiptBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithInventoryItem(inventoryItem);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            builder.WithShipmentItem(shipmentItem);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenShipmentReceiptForPartWithoutSelectedInventoryItemWhenDerivingThenInventoryItemIsFromDefaultFacility()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            

            var part = new RawMaterialBuilder(this.DatabaseSession)
                .WithName("RawMaterial")
                .Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(supplier)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(part).WithQuantityOrdered(1).Build();
            order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive();

            order.Confirm();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithPart(part).Build();
            shipment.AddShipmentItem(shipmentItem);

            var receipt = new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(1M)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item1)
                .Build();

            shipment.AppsComplete();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            Assert.Equal(new Warehouses(this.DatabaseSession).FindBy(M.Warehouse.Name, "facility"), receipt.InventoryItem.Facility);
            Assert.Equal(part.InventoryItemsWherePart[0], receipt.InventoryItem);

            this.DatabaseSession.Rollback();
        }

        [Fact]
        public void GivenShipmentReceiptForGoodWithoutSelectedInventoryItemWhenDerivingThenInventoryItemIsFromDefaultFacility()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            

            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(1).Build();
            order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            order.Confirm();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            var receipt = new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(1M)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item1)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            shipment.AppsComplete();

            Assert.Equal(new Warehouses(this.DatabaseSession).FindBy(M.Warehouse.Name, "facility"), receipt.InventoryItem.Facility);
            Assert.Equal(good.InventoryItemsWhereGood[0], receipt.InventoryItem);

            this.DatabaseSession.Rollback();
        }

        [Fact]
        public void GivenShipmentReceiptWhenDerivingThenInventoryItemQuantityOnHandIsUpdated()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            

            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var inventoryItem = new NonSerialisedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(20).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithDeliveryDate(DateTime.UtcNow)
                .Build();

            var salesItem = new SalesOrderItemBuilder(this.DatabaseSession).WithDescription("item1").WithProduct(good).WithQuantityOrdered(30).WithActualUnitPrice(15).Build();
            order1.AddSalesOrderItem(salesItem);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            order1.Confirm();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var sessionInventoryItem = (NonSerialisedInventoryItem)this.DatabaseSession.Instantiate(inventoryItem);
            var sessionSalesItem = (SalesOrderItem)this.DatabaseSession.Instantiate(salesItem);

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            

            Assert.Equal(20, sessionSalesItem.QuantityPendingShipment);
            Assert.Equal(30, sessionSalesItem.QuantityReserved);
            Assert.Equal(10, sessionSalesItem.QuantityShortFalled);

            var order = new PurchaseOrderBuilder(this.DatabaseSession).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(10).Build();
            order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            order.Confirm();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(10)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item1)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            shipment.AppsComplete();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            Assert.Equal(30, sessionInventoryItem.QuantityOnHand);

            Assert.Equal(30, sessionSalesItem.QuantityPendingShipment);
            Assert.Equal(30, sessionSalesItem.QuantityReserved);
            Assert.Equal(0, sessionSalesItem.QuantityShortFalled);
        }

        [Fact]
        public void GivenShipmentReceiptWhenDerivingThenOrderItemQuantityReceivedIsUpdated()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            

            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new PurchaseOrderBuilder(this.DatabaseSession).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(10).Build();
            order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            order.Confirm();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(10)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item1)
                .Build();

            shipment.AppsComplete();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            Assert.Equal(10, item1.QuantityReceived);

            this.DatabaseSession.Rollback();
        }
    }
}
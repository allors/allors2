//------------------------------------------------------------------------------------------------- 
// <copyright file="PackagingContentTests.cs" company="Allors bvba">
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

    
    public class PackagingContentTests : DomainTest
    {
        [Fact]
        public void GivenPackingingContent_WhenDeriving_ThenAssertQuantityPackedIsNotGreaterThanQuantityShipped()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.Session).Customer).Build();
            var internalOrganisation = this.Session.GetSingleton().InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("good1").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("good2").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var good1inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good1).Build();
            good1inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            this.Session.Derive();

            var good2inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good2).Build();
            good2inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
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

            var package = new ShipmentPackageBuilder(this.Session).Build();
            package.AddPackagingContent(new PackagingContentBuilder(this.Session)
                                            .WithShipmentItem(shipment.ShipmentItems[0])
                                            .WithQuantity(shipment.ShipmentItems[0].Quantity + 1)
                                            .Build());

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPackingingContent_WhenDerived_ThenShipmentItemsQuantityPackedIsSet()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPersonRole(new PersonRoles(this.Session).Customer).Build();
            var internalOrganisation = this.Session.GetSingleton().InternalOrganisation;
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();


            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("good1").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("good2").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good1inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good1).Build();
            good1inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            this.Session.Derive();

            var good2inventoryItem = new NonSerialisedInventoryItemBuilder(this.Session).WithGood(good2).Build();
            good2inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.Session).WithQuantity(100).WithReason(new VarianceReasons(this.Session).Ruined).Build());

            this.Session.Derive();

            var order = new SalesOrderBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
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
            var package = new ShipmentPackageBuilder(this.Session).Build();
            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                Assert.Equal(shipmentItem.QuantityShipped, shipmentItem.Quantity);
            }
        }
    }
}
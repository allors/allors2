// <copyright file="ShipmentItemTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

using System.ComponentModel;
using Allors.Domain.TestPopulation;

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;
    using Xunit;

    public class ShipmentItemTests : DomainTest
    {
        [Fact]
        public void GivenPurchaseShipmentItemForNonSerialisedNotFromPurchaseOrder_WhenDerived_ThenUnitPurchasePriceIsRequired()
        {
            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");

            User user = this.Administrator;
            this.Session.SetUser(user);

            var shipment = new PurchaseShipmentBuilder(this.Session)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithShipFromParty(this.InternalOrganisation.ActiveSuppliers.First)
                .Build();

            var shipmentItem = new ShipmentItemBuilder(this.Session).WithPart(good1.Part).WithQuantity(1).Build();
            shipment.AddShipmentItem(shipmentItem);

            var validation = this.Session.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Single(validation.Errors);
            Assert.Contains(MetaShipmentItem.Instance.UnitPurchasePrice, validation.Errors.First().RoleTypes);
        }
    }

    [Trait("Category", "Security")]
    public class ShipmentItemSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenShipmentItem_WhenProcessed_ThenDeleteIsNotAllowed()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var shipToAddress = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var good1 = new NonUnifiedGoods(this.Session).FindBy(M.Good.Name, "good1");
            new InventoryItemTransactionBuilder(this.Session).WithQuantity(100).WithReason(new InventoryTransactionReasons(this.Session).PhysicalCount).WithPart(good1.Part).Build();

            User user = this.Administrator;
            this.Session.SetUser(user);

            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();

            var shipment = new CustomerShipmentBuilder(this.Session)
                .WithShipToParty(customer)
                .WithShipToAddress(shipToAddress)
                .WithShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            var shipmentItem = new ShipmentItemBuilder(this.Session).WithGood(good1).WithQuantity(10).Build();
            shipment.AddShipmentItem(shipmentItem);

            this.Session.Derive();

            shipment.Pick();
            this.Session.Derive();

            var acl = new DatabaseAccessControlLists(this.Session.GetUser())[shipmentItem];
            Assert.Equal(new ShipmentItemStates(this.Session).Picking, shipmentItem.ShipmentItemState);
            Assert.False(acl.CanExecute(M.ShipmentItem.Delete));

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = this.OrderProcessor;

            pickList.SetPicked();
            this.Session.Derive();

            acl = new DatabaseAccessControlLists(this.Session.GetUser())[shipmentItem];
            Assert.Equal(new ShipmentItemStates(this.Session).Picked, shipmentItem.ShipmentItemState);
            Assert.False(acl.CanExecute(M.ShipmentItem.Delete));

            var package = new ShipmentPackageBuilder(this.Session).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem item in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.Session).WithShipmentItem(item).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.Session.Derive();

            shipment.SetPacked();
            this.Session.Derive();

            acl = new DatabaseAccessControlLists(this.Session.GetUser())[shipmentItem];
            Assert.Equal(new ShipmentItemStates(this.Session).Packed, shipmentItem.ShipmentItemState);
            Assert.False(acl.CanExecute(M.ShipmentItem.Delete));

            shipment.Ship();
            this.Session.Derive();

            acl = new DatabaseAccessControlLists(this.Session.GetUser())[shipmentItem];
            Assert.Equal(new ShipmentItemStates(this.Session).Shipped, shipmentItem.ShipmentItemState);
            Assert.False(acl.CanExecute(M.ShipmentItem.Delete));
        }
    }
}

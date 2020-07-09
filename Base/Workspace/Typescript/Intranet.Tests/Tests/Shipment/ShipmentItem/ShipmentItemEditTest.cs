// <copyright file="ShipmentItemCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ShipmentItemTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.customershipment.overview;
    using src.allors.material.@base.objects.shipment.list;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Shipment")]
    public class ShipmentItemEditTest : Test
    {
        private readonly ShipmentListComponent shipmentListPage;
        private CustomerShipment customerShipment;
        private Organisation internalOrganisation;

        public ShipmentItemEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            var customerShipments = new CustomerShipments(this.Session).Extent();
            customerShipments.Filter.AddEquals(M.CustomerShipment.ShipFromParty.RoleType, internalOrganisation);
            this.customerShipment = customerShipments.First;

            this.Login();
            this.shipmentListPage = this.Sidenav.NavigateToShipments();
        }

        [Fact]
        public void CreateCustomerShipmentItemForSerialisedUnifiedGood()
        {
            var before = customerShipment.ShipmentItems.ToArray();

            var goods = new UnifiedGoods(this.Session).Extent();
            goods.Filter.AddEquals(M.UnifiedGood.InventoryItemKind.RoleType, new InventoryItemKinds(this.Session).Serialised);
            var serializedGood = goods.First;

            var serialisedItem = new SerialisedItemBuilder(this.Session).WithForSaleDefaults(this.internalOrganisation).Build();
            serializedGood.AddSerialisedItem(serialisedItem);

            this.Session.Derive();
            this.Session.Commit();

            this.shipmentListPage.Table.DefaultAction(customerShipment);
            var shipmentOverview = new CustomerShipmentOverviewComponent(this.shipmentListPage.Driver);
            var shipmentItemOverview = shipmentOverview.ShipmentitemOverviewPanel.Click();

            var shipmentItemCreate = shipmentItemOverview.CreateShipmentItem();
            shipmentItemCreate
                .Good.Select(serializedGood.Name)
                .ShipmentItemSerialisedItem_1.Select(serialisedItem)
                .NextSerialisedItemAvailability.Select(new SerialisedItemAvailabilities(this.Session).Sold)
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = customerShipment.ShipmentItems.ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(serializedGood.Name, actual.Good.Name);
            Assert.Equal(serialisedItem.Name, actual.SerialisedItem.Name);
            Assert.Equal(1, actual.Quantity);
        }

        [Fact]
        public void CreateCustomerShipmentItemForNonSerialisedUnifiedGood()
        {
            var before = customerShipment.ShipmentItems.ToArray();

            var goods = new UnifiedGoods(this.Session).Extent();
            goods.Filter.AddEquals(M.UnifiedGood.InventoryItemKind.RoleType, new InventoryItemKinds(this.Session).NonSerialised);
            var nonSerializedGood = goods.First;

            this.shipmentListPage.Table.DefaultAction(customerShipment);
            var shipmentOverview = new CustomerShipmentOverviewComponent(this.shipmentListPage.Driver);
            var shipmentItemOverview = shipmentOverview.ShipmentitemOverviewPanel.Click();

            var shipmentItemCreate = shipmentItemOverview.CreateShipmentItem();
            shipmentItemCreate
                .Good.Select(nonSerializedGood.Name)
                .Quantity.Set("5")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = customerShipment.ShipmentItems.ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(nonSerializedGood.Name, actual.Good.Name);
            Assert.Equal(5, actual.Quantity);
        }

        [Fact]
        public void EditCustomerShipmentItemForSerialisedUnifiedGood()
        {
            var before = customerShipment.ShipmentItems.ToArray();

            var goods = new UnifiedGoods(this.Session).Extent();
            goods.Filter.AddEquals(M.UnifiedGood.InventoryItemKind.RoleType, new InventoryItemKinds(this.Session).Serialised);
            var serializedGood = goods.First;

            var serialisedItem = new SerialisedItemBuilder(this.Session).WithForSaleDefaults(this.internalOrganisation).Build();
            serializedGood.AddSerialisedItem(serialisedItem);

            this.Session.Derive();
            this.Session.Commit();

            this.shipmentListPage.Table.DefaultAction(customerShipment);
            var shipmentOverview = new CustomerShipmentOverviewComponent(this.shipmentListPage.Driver);
            var shipmentItemOverview = shipmentOverview.ShipmentitemOverviewPanel.Click();

            var shipmentItemCreate = shipmentItemOverview.CreateShipmentItem();
            shipmentItemCreate
                .Good.Select(serializedGood.Name)
                .ShipmentItemSerialisedItem_1.Select(serialisedItem)
                .NextSerialisedItemAvailability.Select(new SerialisedItemAvailabilities(this.Session).Sold)
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = customerShipment.ShipmentItems.ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(serializedGood.Name, actual.Good.Name);
            Assert.Equal(serialisedItem.Name, actual.SerialisedItem.Name);
            Assert.Equal(1, actual.Quantity);
        }
    }
}

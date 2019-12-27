// <copyright file="ShipmentItemCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;
using Components;
using src.allors.material.@base.objects.customershipment.overview;

namespace Tests.ShipmentItemTests
{
    using Allors.Domain;
    using Allors.Meta;
    using src.allors.material.@base.objects.purchaseshipment.overview;
    using src.allors.material.@base.objects.shipment.list;
    using Xunit;

    [Collection("Test collection")]
    public class ShipmentItemEditTest : Test
    {
        private readonly ShipmentListComponent shipmentListPage;
        private CustomerShipment customerShipment;

        public ShipmentItemEditTest(TestFixture fixture)
            : base(fixture)
        {
            var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");

            var customerShipments = new CustomerShipments(this.Session).Extent();
            customerShipments.Filter.AddEquals(M.CustomerShipment.ShipFromParty.RoleType, internalOrganisation);
            this.customerShipment = customerShipments.First;

            this.Login();
            this.shipmentListPage = this.Sidenav.NavigateToShipments();
        }

        [Fact]
        public void CreateCustomerShipmentItemForUnifiedGoodSerialisedItem()
        {
            var goods = new UnifiedGoods(this.Session).Extent();
            goods.Filter.AddEquals(M.UnifiedGood.InventoryItemKind.RoleType, new InventoryItemKinds(this.Session).Serialised);
            var serializedGood = goods.First;

            var before = customerShipment.ShipmentItems.ToArray();

            this.shipmentListPage.Table.DefaultAction(customerShipment);
            var shipmentOverview = new CustomerShipmentOverviewComponent(this.shipmentListPage.Driver);
            var shipmentItemOverview = shipmentOverview.ShipmentitemOverviewPanel.Click();

            var shipmentItemCreate = shipmentItemOverview.CreateShipmentItem();
            shipmentItemCreate
                .Good.Select(serializedGood.Name)
                .ShipmentItemSerialisedItem_1.Set(serializedGood.SerialisedItems.First.Name)
                .Quantity.Set("1")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = customerShipment.ShipmentItems.ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(serializedGood.Name, actual.Good.Name);
            Assert.Equal(serializedGood.SerialisedItems.First.Name, actual.SerialisedItem.Name);
            Assert.Equal(1, actual.Quantity);
        }

        //[Fact]
        //public void CreateMinimal()
        //{
        //    var before = new PurchaseShipments(this.Session).Extent().ToArray();

        //    var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");
        //    var expected = new PurchaseShipmentBuilder(this.MemorySession).WithDefaults(internalOrganisation).Build();

        //    this.MemorySession.Derive();

        //    var purchaseShipmentCreate = this.shipmentListPage
        //        .CreatePurchaseShipment()
        //        .Build(expected, true);

        //    purchaseShipmentCreate.AssertFull(expected);

        //    purchaseShipmentCreate.SAVE.Click();

        //    this.Driver.WaitForAngular();
        //    this.Session.Rollback();

        //    var after = new PurchaseShipments(this.Session).Extent().ToArray();

        //    Assert.Equal(after.Length, before.Length + 1);

        //    var actual = after.Except(before).First();

        //    Assert.Equal(expected.ShipFromParty.PartyName, actual.ShipFromParty.PartyName);
        //    Assert.Equal(expected.ShipToParty.PartyName, actual.ShipToParty.PartyName);
        //}
    }
}

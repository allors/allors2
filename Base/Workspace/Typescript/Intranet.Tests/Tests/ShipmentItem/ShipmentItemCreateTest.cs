// <copyright file="ShipmentItemCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors;
using Allors.Meta;
using src.allors.material.@base.objects.purchaseshipment.overview;
using src.allors.material.@base.objects.shipment.list;

namespace Tests.ShipmentItemTests
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class ShipmentItemCreateTest : Test
    {
        private readonly ShipmentListComponent shipmentListPage;

        public ShipmentItemCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.shipmentListPage = this.Sidenav.NavigateToShipments();
        }

        //[Fact]
        //public void CreateCustomerShipmentItemForSerialisedItem()
        //{
        //    var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");
        //    var expected = new CustomerShipmentBuilder(this.MemorySession).WithDefaults(internalOrganisation).Build();

        //    this.MemorySession.Derive();

        //    var customerShipments = new CustomerShipments(this.Session).Extent();
        //    customerShipments.Filter.AddEquals(M.CustomerShipment.ShipFromParty.RoleType, internalOrganisation);
        //    var customerShipment = customerShipments.First;
        //    var id = customerShipment.Id;

        //    var before = customerShipment.ShipmentItems.ToArray();

        //    this.shipmentListPage.Table.DefaultAction(customerShipment);
        //    var shipmentOverview = new PurchaseShipmentOverviewComponent(this.shipmentListPage.Driver);
        //    var shipmentItemOverview = shipmentOverview.ShipmentitemOverviewPanel.Click();

        //    var shipmentItemCreate = shipmentItemOverview.CreateShipmentItem();
        //    shipmentItemCreate.Part.Select(expected)
        //}

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

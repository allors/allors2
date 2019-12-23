// <copyright file="PurchaseShipmentEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PurchaseShipmentTests
{
    using Allors;
    using Allors.Meta;
    using src.allors.material.@base.objects.purchaseshipment.overview;
    using src.allors.material.@base.objects.shipment.list;
    using System.Linq;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class PurchaseShipmentEditTest : Test
    {
        private readonly ShipmentListComponent shipmentListPage;

        public PurchaseShipmentEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.shipmentListPage = this.Sidenav.NavigateToShipments();
        }

        [Fact]
        public void Edit()
        {
            var before = new PurchaseShipments(this.Session).Extent().ToArray();

            var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new PurchaseShipmentBuilder(this.MemorySession).WithDefaults(internalOrganisation).Build();

            this.MemorySession.Derive();

            var shipment = before.First(v => ((Organisation)v.ShipToParty).IsInternalOrganisation.Equals(true));
            var id = shipment.Id;

            this.shipmentListPage.Table.DefaultAction(shipment);
            var shipmentOverview = new PurchaseShipmentOverviewComponent(this.shipmentListPage.Driver);
            var shipmentOverviewDetail = shipmentOverview.PurchaseshipmentOverviewDetail.Click();

            shipmentOverviewDetail
                .ShipFromParty.Select(expected.ShipFromParty.PartyName)
                .ShipFromContactPerson.Set(expected.ShipFromContactPerson?.PartyName)
                .ShipToAddress.Set(expected.ShipToAddress?.DisplayName())
                .ShipToContactPerson.Set(expected.ShipToContactPerson?.PartyName)
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseShipments(this.Session).Extent().ToArray();
            shipment = (PurchaseShipment) this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expected.ShipFromParty.PartyName, shipment.ShipFromParty.PartyName);
            Assert.Equal(expected.ShipFromContactPerson.PartyName, shipment.ShipFromContactPerson.PartyName);
            Assert.Equal(expected.ShipToAddress.DisplayName(), shipment.ShipToAddress.DisplayName());
            Assert.Equal(expected.ShipToContactPerson.PartyName, shipment.ShipToContactPerson.PartyName);
        }
    }
}

// <copyright file="PurchaseShipmentEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PurchaseShipmentTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.purchaseshipment.overview;
    using src.allors.material.@base.objects.shipment.list;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Shipment")]
    public class PurchaseShipmentEditTest : Test
    {
        private readonly ShipmentListComponent shipmentListPage;
        private Organisation internalOrganisation;

        public PurchaseShipmentEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            for (var i = 0; i < 10; i++)
            {
                this.internalOrganisation.CreateSupplier(this.Session.Faker());
            }

            this.Login();
            this.shipmentListPage = this.Sidenav.NavigateToShipments();
        }

        [Fact]
        public void Edit()
        {
            var before = new PurchaseShipments(this.Session).Extent().ToArray();

            var expected = new PurchaseShipmentBuilder(this.Session).WithDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
            var expectedShipToContactPersonPartyName = expected.ShipToContactPerson.DisplayName();
            var expectedShipFromPartyPartyName = expected.ShipFromParty.DisplayName();
            var expectedShipFromContactPersonPartyName = expected.ShipFromContactPerson.DisplayName();

            var shipment = before.First(v => ((Organisation)v.ShipToParty).IsInternalOrganisation.Equals(true));
            var id = shipment.Id;

            this.shipmentListPage.Table.DefaultAction(shipment);
            var shipmentOverview = new PurchaseShipmentOverviewComponent(this.shipmentListPage.Driver);
            var shipmentOverviewDetail = shipmentOverview.PurchaseshipmentOverviewDetail.Click();

            shipmentOverviewDetail
                .ShipFromParty.Select(expected.ShipFromParty.DisplayName())
                .ShipFromContactPerson.Select(expected.ShipFromContactPerson)
                .ShipToAddress.Select(expected.ShipToAddress)
                .ShipToContactPerson.Select(expected.ShipToContactPerson);

            this.Session.Rollback();
            shipmentOverviewDetail.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseShipments(this.Session).Extent().ToArray();
            shipment = (PurchaseShipment)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedShipFromPartyPartyName, shipment.ShipFromParty.DisplayName());
            Assert.Equal(expectedShipFromContactPersonPartyName, shipment.ShipFromContactPerson.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, shipment.ShipToAddress.DisplayName());
            Assert.Equal(expectedShipToContactPersonPartyName, shipment.ShipToContactPerson.DisplayName());
        }
    }
}

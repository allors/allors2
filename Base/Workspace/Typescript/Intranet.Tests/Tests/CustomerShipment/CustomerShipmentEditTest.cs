// <copyright file="CustomerShipmentEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors;
using Allors.Meta;
using src.allors.material.@base.objects.customershipment.create;
using src.allors.material.@base.objects.customershipment.overview;
using src.allors.material.@base.objects.shipment.list;

namespace Tests.OrganisationTests
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class CustomerShipmentEditTest : Test
    {
        private readonly ShipmentListComponent shipmentListPage;

        public CustomerShipmentEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.shipmentListPage = this.Sidenav.NavigateToShipments();
        }

        [Fact]
        public void Edit()
        {
            var before = new CustomerShipments(this.Session).Extent().ToArray();

            var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new CustomerShipmentBuilder(this.MemorySession).WithDefaults(internalOrganisation).Build();

            this.MemorySession.Derive();

            var shipment = before.First(v => ((Organisation)v.ShipFromParty).IsInternalOrganisation.Equals(true));
            var id = shipment.Id;

            this.shipmentListPage.Table.DefaultAction(shipment);
            var shipmentOverview = new CustomerShipmentOverviewComponent(this.shipmentListPage.Driver);
            var shipmentOverviewDetail = shipmentOverview.CustomershipmentOverviewDetail.Click();

            shipmentOverviewDetail
                .ShipToParty.Select(expected.ShipToParty.PartyName)
                .ShipToAddress.Set(expected.ShipToAddress?.DisplayName())
                .ShipToContactPerson.Set(expected.ShipToContactPerson?.PartyName)
                .ShipFromAddress.Set(expected.ShipFromParty?.ShippingAddress.DisplayName())
                .ShipmentMethod.Set(expected.ShipmentMethod.Name)
                .ShipFromFacility.Set(((Organisation)expected.ShipFromParty).FacilitiesWhereOwner?.First.Name)
                .Carrier.Set(expected.Carrier.Name)
                .EstimatedShipDate.Set(expected.EstimatedShipDate.Value.Date)
                .EstimatedArrivalDate.Set(expected.EstimatedArrivalDate.Value.Date)
                .HandlingInstruction.Set(expected.HandlingInstruction)
                .Comment.Set(expected.Comment)
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new CustomerShipments(this.Session).Extent().ToArray();
            shipment = (CustomerShipment) this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expected.ShipToParty.PartyName, shipment.ShipToParty.PartyName);
            Assert.Equal(expected.ShipToAddress.DisplayName(), shipment.ShipToAddress.DisplayName());
            Assert.Equal(expected.ShipToContactPerson.PartyName, shipment.ShipToContactPerson.PartyName);
            Assert.Equal(expected.ShipFromAddress.DisplayName(), shipment.ShipFromAddress.DisplayName());
            Assert.Equal(expected.ShipFromFacility.Name, shipment.ShipFromFacility.Name);
            Assert.Equal(expected.ShipmentMethod.Name, shipment.ShipmentMethod.Name);
            Assert.Equal(expected.Carrier.Name, shipment.Carrier.Name);
            Assert.Equal(expected.EstimatedShipDate.Value.Date, shipment.EstimatedShipDate);
            Assert.Equal(expected.EstimatedArrivalDate.Value.Date, shipment.EstimatedArrivalDate);
            Assert.Equal(expected.HandlingInstruction, shipment.HandlingInstruction);
            Assert.Equal(expected.Comment, shipment.Comment);
        }
    }
}

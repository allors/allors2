// <copyright file="CustomerShipmentEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors;
using Allors.Meta;
using src.allors.material.@base.objects.customershipment.create;
using src.allors.material.@base.objects.customershipment.overview;
using src.allors.material.@base.objects.shipment.list;

namespace Tests.CustomerShipmentTests
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

            var internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new CustomerShipmentBuilder(this.Session).WithDefaults(internalOrganisation).Build();

            this.Session.Derive();

            var expectedShipToPartyPartyName = expected.ShipToParty.PartyName;
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
            var expectedShipToContactPersonPartyName = expected.ShipToContactPerson.PartyName;
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress.DisplayName();
            var expectedShipFromFacilityName = expected.ShipFromFacility.Name;
            var expectedShipmentMethodName = expected.ShipmentMethod.Name;
            var expectedCarrierName = expected.Carrier.Name;
            var expectedEstimatedShipDate = expected.EstimatedShipDate.Value.Date;
            var expectedEstimatedArrivalDate = expected.EstimatedArrivalDate.Value.Date;
            var expectedHandlingInstruction = expected.HandlingInstruction;
            var expectedComment = expected.Comment;

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
                .Comment.Set(expected.Comment);

            this.Session.Rollback();
            shipmentOverviewDetail.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new CustomerShipments(this.Session).Extent().ToArray();
            shipment = (CustomerShipment) this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedShipToPartyPartyName, shipment.ShipToParty.PartyName);
            Assert.Equal(expectedShipToAddressDisplayName, shipment.ShipToAddress.DisplayName());
            Assert.Equal(expectedShipToContactPersonPartyName, shipment.ShipToContactPerson.PartyName);
            Assert.Equal(expectedShipFromAddressDisplayName, shipment.ShipFromAddress.DisplayName());
            Assert.Equal(expectedShipFromFacilityName, shipment.ShipFromFacility.Name);
            Assert.Equal(expectedShipmentMethodName, shipment.ShipmentMethod.Name);
            Assert.Equal(expectedCarrierName, shipment.Carrier.Name);
            Assert.Equal(expectedEstimatedShipDate, shipment.EstimatedShipDate);
            Assert.Equal(expectedEstimatedArrivalDate, shipment.EstimatedArrivalDate);
            Assert.Equal(expectedHandlingInstruction, shipment.HandlingInstruction);
            Assert.Equal(expectedComment, shipment.Comment);
        }
    }
}

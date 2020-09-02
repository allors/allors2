// <copyright file="CustomerShipmentCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.CustomerShipmentTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using libs.angular.material.@base.src.export.objects.customershipment.create;
    using libs.angular.material.@base.src.export.objects.shipment.list;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Shipment")]
    public class CustomerShipmentCreateTest : Test
    {
        private readonly ShipmentListComponent shipmentListPage;
        private Organisation internalOrganisation;

        public CustomerShipmentCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            this.Login();
            this.shipmentListPage = this.Sidenav.NavigateToShipments();
        }

        [Fact]
        public void CreateFull()
        {
            var before = new CustomerShipments(this.Session).Extent().ToArray();

            var expected = new CustomerShipmentBuilder(this.Session).WithDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedShipToPartyPartyName = expected.ShipToParty?.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress?.DisplayName();
            var expectedShipToContactPersonPartyName = expected.ShipToContactPerson?.DisplayName();
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress?.DisplayName();
            var expectedShipFromFacilityName = expected.ShipFromFacility.Name;
            var expectedShipmentMethodName = expected.ShipmentMethod.Name;
            var expectedCarrierName = expected.Carrier.Name;
            var expectedEstimatedShipDate = expected.EstimatedShipDate.Value.Date;
            var expectedEstimatedArrivalDate = expected.EstimatedArrivalDate.Value.Date;
            var expectedHandlingInstruction = expected.HandlingInstruction;
            var expectedComment = expected.Comment;

            var customerShipmentCreate = this.shipmentListPage
                .CreateCustomerShipment()
                .Build(expected);

            customerShipmentCreate.AssertFull(expected);

            this.Session.Rollback();
            customerShipmentCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new CustomerShipments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedShipToPartyPartyName, actual.ShipToParty?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.ShipToAddress?.DisplayName());
            Assert.Equal(expectedShipToContactPersonPartyName, actual.ShipToContactPerson?.DisplayName());
            Assert.Equal(expectedShipFromAddressDisplayName, actual.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedShipFromFacilityName, actual.ShipFromFacility.Name);
            Assert.Equal(expectedShipmentMethodName, actual.ShipmentMethod.Name);
            Assert.Equal(expectedCarrierName, actual.Carrier.Name);
            Assert.Equal(expectedEstimatedShipDate, actual.EstimatedShipDate);
            Assert.Equal(expectedEstimatedArrivalDate, actual.EstimatedArrivalDate);
            Assert.Equal(expectedHandlingInstruction, actual.HandlingInstruction);
            Assert.Equal(expectedComment, actual.Comment);
        }

        [Fact]
        public void CreateMinimal()
        {
            var before = new CustomerShipments(this.Session).Extent().ToArray();

            var expected = new CustomerShipmentBuilder(this.Session).WithDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedShipToPartyPartyName = expected.ShipToParty?.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress?.DisplayName();
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress?.DisplayName();
            var expectedShipFromFacilityName = expected.ShipFromFacility.Name;

            var customerShipmentCreate = this.shipmentListPage
                .CreateCustomerShipment()
                .Build(expected, true);

            customerShipmentCreate.AssertFull(expected);

            this.Session.Rollback();
            customerShipmentCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new CustomerShipments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedShipToPartyPartyName, actual.ShipToParty?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.ShipToAddress?.DisplayName());
            Assert.Equal(expectedShipFromAddressDisplayName, actual.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedShipFromFacilityName, actual.ShipFromFacility.Name);
        }
    }
}

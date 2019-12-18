// <copyright file="CustomerShipmentCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors;
using Allors.Meta;
using src.allors.material.@base.objects.customershipment.create;
using src.allors.material.@base.objects.shipment.list;

namespace Tests.OrganisationTests
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class CustomerShipmentCreateTest : Test
    {
        private readonly ShipmentListComponent shipmentListPage;

        public CustomerShipmentCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.shipmentListPage = this.Sidenav.NavigateToShipments();
        }

        [Fact]
        public void CreateFull()
        {
            var before = new CustomerShipments(this.Session).Extent().ToArray();

            var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new CustomerShipmentBuilder(this.MemorySession).WithDefaults(internalOrganisation).Build();

            this.MemorySession.Derive();

            var customerShipmentCreate = this.shipmentListPage
                .CreateCustomerShipment()
                .Build(expected);

            customerShipmentCreate.AssertFull(expected);

            customerShipmentCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new CustomerShipments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expected.ShipToParty.PartyName, actual.ShipToParty.PartyName);
            Assert.Equal(expected.ShipToAddress.DisplayName(), actual.ShipToAddress.DisplayName());
            Assert.Equal(expected.ShipToContactPerson.PartyName, actual.ShipToContactPerson.PartyName);
            Assert.Equal(expected.ShipFromAddress.DisplayName(), actual.ShipFromAddress.DisplayName());
            Assert.Equal(expected.ShipFromFacility.Name, actual.ShipFromFacility.Name);
            Assert.Equal(expected.ShipmentMethod.Name, actual.ShipmentMethod.Name);
            Assert.Equal(expected.Carrier.Name, actual.Carrier.Name);
            Assert.Equal(expected.EstimatedShipDate.Value.Date, actual.EstimatedShipDate);
            Assert.Equal(expected.EstimatedArrivalDate.Value.Date, actual.EstimatedArrivalDate);
            Assert.Equal(expected.HandlingInstruction, actual.HandlingInstruction);
            Assert.Equal(expected.Comment, actual.Comment);
        }

        [Fact]
        public void CreateMinimal()
        {
            var before = new CustomerShipments(this.Session).Extent().ToArray();

            var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new CustomerShipmentBuilder(this.MemorySession).WithDefaults(internalOrganisation).Build();

            this.MemorySession.Derive();

            var customerShipmentCreate = this.shipmentListPage
                .CreateCustomerShipment()
                .Build(expected, true);

            customerShipmentCreate.AssertFull(expected);

            customerShipmentCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new CustomerShipments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expected.ShipToParty.PartyName, actual.ShipToParty.PartyName);
            Assert.Equal(expected.ShipToAddress.DisplayName(), actual.ShipToAddress.DisplayName());
            Assert.Equal(expected.ShipFromAddress.DisplayName(), actual.ShipFromAddress.DisplayName());
            Assert.Equal(expected.ShipFromFacility.Name, actual.ShipFromFacility.Name);
        }
    }
}

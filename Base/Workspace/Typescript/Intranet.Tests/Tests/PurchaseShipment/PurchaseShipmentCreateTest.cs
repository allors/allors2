// <copyright file="PurchaseShipmentCreateTest.cs" company="Allors bvba">
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
    using src.allors.material.@base.objects.purchaseshipment.create;
    using src.allors.material.@base.objects.shipment.list;
    using Xunit;

    [Collection("Test collection")]
    public class PurchaseShipmentCreateTest : Test
    {
        private readonly ShipmentListComponent shipmentListPage;

        public PurchaseShipmentCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.shipmentListPage = this.Sidenav.NavigateToShipments();
        }

        [Fact]
        public void CreateFull()
        {
            var before = new PurchaseShipments(this.Session).Extent().ToArray();

            var internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new PurchaseShipmentBuilder(this.Session).WithDefaults(internalOrganisation).Build();

            this.Session.Derive();

            var expectedShipToPartyPartyName = expected.ShipToParty.PartyName;
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
            var expectedShipToContactPersonPartyName = expected.ShipToContactPerson.PartyName;
            var expectedShipFromPartyPartyName = expected.ShipFromParty.PartyName;
            var expectedShipFromContactPersonPartyName = expected.ShipFromContactPerson.PartyName;

            var purchaseShipmentCreate = this.shipmentListPage
                .CreatePurchaseShipment()
                .Build(expected);

            purchaseShipmentCreate.AssertFull(expected);

            this.Session.Rollback();
            purchaseShipmentCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseShipments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedShipFromPartyPartyName, actual.ShipFromParty.PartyName);
            Assert.Equal(expectedShipFromContactPersonPartyName, actual.ShipFromContactPerson.PartyName);
            Assert.Equal(expectedShipToPartyPartyName, actual.ShipToParty.PartyName);
            Assert.Equal(expectedShipToAddressDisplayName, actual.ShipToAddress.DisplayName());
            Assert.Equal(expectedShipToContactPersonPartyName, actual.ShipToContactPerson.PartyName);
        }

        [Fact]
        public void CreateMinimal()
        {
            var before = new PurchaseShipments(this.Session).Extent().ToArray();

            var internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new PurchaseShipmentBuilder(this.Session).WithDefaults(internalOrganisation).Build();

            this.Session.Derive();

            var expectedShipToPartyPartyName = expected.ShipToParty.PartyName;
            var expectedShipFromPartyPartyName = expected.ShipFromParty.PartyName;

            var purchaseShipmentCreate = this.shipmentListPage
                .CreatePurchaseShipment()
                .Build(expected, true);

            purchaseShipmentCreate.AssertFull(expected);

            this.Session.Rollback();
            purchaseShipmentCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseShipments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedShipFromPartyPartyName, actual.ShipFromParty.PartyName);
            Assert.Equal(expectedShipToPartyPartyName, actual.ShipToParty.PartyName);
        }
    }
}

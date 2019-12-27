// <copyright file="PurchaseShipmentCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors;
using Allors.Meta;
using src.allors.material.@base.objects.purchaseshipment.create;
using src.allors.material.@base.objects.shipment.list;

namespace Tests.PurchaseShipmentTests
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
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

            var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new PurchaseShipmentBuilder(this.MemorySession).WithDefaults(internalOrganisation).Build();

            this.MemorySession.Derive();

            var purchaseShipmentCreate = this.shipmentListPage
                .CreatePurchaseShipment()
                .Build(expected);

            purchaseShipmentCreate.AssertFull(expected);

            purchaseShipmentCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseShipments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expected.ShipFromParty.PartyName, actual.ShipFromParty.PartyName);
            Assert.Equal(expected.ShipFromContactPerson.PartyName, actual.ShipFromContactPerson.PartyName);
            Assert.Equal(expected.ShipToParty.PartyName, actual.ShipToParty.PartyName);
            Assert.Equal(expected.ShipToAddress.DisplayName(), actual.ShipToAddress.DisplayName());
            Assert.Equal(expected.ShipToContactPerson.PartyName, actual.ShipToContactPerson.PartyName);
        }

        [Fact]
        public void CreateMinimal()
        {
            var before = new PurchaseShipments(this.Session).Extent().ToArray();

            var internalOrganisation = new Organisations(this.MemorySession).FindBy(M.Organisation.Name, "Allors BVBA");
            var expected = new PurchaseShipmentBuilder(this.MemorySession).WithDefaults(internalOrganisation).Build();

            this.MemorySession.Derive();

            var purchaseShipmentCreate = this.shipmentListPage
                .CreatePurchaseShipment()
                .Build(expected, true);

            purchaseShipmentCreate.AssertFull(expected);

            purchaseShipmentCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PurchaseShipments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expected.ShipFromParty.PartyName, actual.ShipFromParty.PartyName);
            Assert.Equal(expected.ShipToParty.PartyName, actual.ShipToParty.PartyName);
        }
    }
}

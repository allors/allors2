// <copyright file="SalesOrderCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.SalesOrderTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.salesorder.create;
    using src.allors.material.@base.objects.salesorder.list;
    using Xunit;

    [Collection("Test collection")]
    public class SalesOrderCreateTest : Test
    {
        private readonly SalesOrderListComponent salesOrderListPage;
        private Organisation internalOrganisation;

        public SalesOrderCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            this.Login();
            this.salesOrderListPage = this.Sidenav.NavigateToSalesOrders();
        }

        [Fact]
        public void CreateMinimal()
        {
            /*var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedShipToPartyPartyName = expected.ShipToParty?.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress?.DisplayName();
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress?.DisplayName();
            var expectedShipFromFacilityName = expected.ShipFromFacility.Name;

            var customerShipmentCreate = this.salesOrderListPage
                .CreateSalesOrder()
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
            Assert.Equal(expectedShipFromFacilityName, actual.ShipFromFacility.Name);*/
        }
    }
}

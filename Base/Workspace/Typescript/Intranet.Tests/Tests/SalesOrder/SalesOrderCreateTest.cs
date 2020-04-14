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
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedBillToEndCustomerContactMechanism = expected.BillToEndCustomerContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedBillToEndCustomer = expected.BillToEndCustomer.DisplayName();
            var expectedShipToEndCustomer = expected.ShipToEndCustomer.DisplayName();
            var expectedShipToEndCustomerAddress = expected.ShipToEndCustomerAddress;
            var expectedShipToEndCustomerContactPerson = expected.ShipToEndCustomerContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
            var expectedShipToContactPerson = expected.ShipToContactPerson;
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress.DisplayName();
            var expectedCustomerReference = expected.CustomerReference;
            var expectedDescription = expected.Description;
            var expectedInternalComment = expected.InternalComment;

            var salesOrderCreate = this.salesOrderListPage
                .CreateSalesOrder()
                .Build(expected);

            salesOrderCreate.AssertFull(expected);

            this.Session.Rollback();
            salesOrderCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedBillToCustomer, actual.BillToCustomer?.DisplayName());
            Assert.Equal(expectedBillToEndCustomerContactMechanism, actual.BillToEndCustomerContactMechanism);
            Assert.Equal(expectedBillToContactMechanism, actual.BillToContactMechanism);
            Assert.Equal(expectedBillToContactPerson, actual.BillToContactPerson);
            Assert.Equal(expectedBillToEndCustomer, actual.BillToEndCustomer?.DisplayName());
            Assert.Equal(expectedShipToEndCustomer, actual.ShipToEndCustomer?.DisplayName());
            Assert.Equal(expectedShipToEndCustomerAddress, actual.ShipToEndCustomerAddress);
            Assert.Equal(expectedShipToEndCustomerContactPerson, actual.ShipToEndCustomerContactPerson);
            Assert.Equal(expectedShipToCustomer, actual.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.ShipToAddress.DisplayName());
            Assert.Equal(expectedShipToContactPerson, actual.ShipToContactPerson);
            Assert.Equal(expectedShipFromAddressDisplayName, actual.ShipFromAddress.DisplayName());
            Assert.Equal(expectedCustomerReference, actual.CustomerReference);
            Assert.Equal(expectedDescription, actual.Description);
            Assert.Equal(expectedInternalComment, actual.InternalComment);
        }
    }
}

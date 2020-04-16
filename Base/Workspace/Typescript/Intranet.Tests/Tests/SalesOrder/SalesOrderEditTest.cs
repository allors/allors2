// <copyright file="SalesOrderEditTest.cs" company="Allors bvba">
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
    using src.allors.material.@base.objects.salesorder.list;
    using src.allors.material.@base.objects.salesorder.overview;
    using Xunit;

    [Collection("Test collection")]
    public class SalesOrderEditTest : Test
    {
        private readonly SalesOrderListComponent salesOrderListPage;
        private readonly Organisation internalOrganisation;

        public SalesOrderEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.internalOrganisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            this.Login();
            this.salesOrderListPage = this.Sidenav.NavigateToSalesOrders();
        }

        [Fact]
        public void Edit()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer?.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedBillToEndCustomerContactMechanism = expected.BillToEndCustomerContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedBillToEndCustomer = expected.BillToEndCustomer?.DisplayName();
            var expectedShipToEndCustomer = expected.ShipToEndCustomer?.DisplayName();
            var expectedShipToEndCustomerAddress = expected.ShipToEndCustomerAddress;
            var expectedShipToEndCustomerContactPerson = expected.ShipToEndCustomerContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer?.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress?.DisplayName();
            var expectedShipToContactPerson = expected.ShipToContactPerson;
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress?.DisplayName();
            var expectedCustomerReference = expected.CustomerReference;
            var expectedDescription = expected.Description;
            var expectedInternalComment = expected.InternalComment;

            var salesOrder = before.First();
            var id = salesOrder.Id;

            this.salesOrderListPage.Table.DefaultAction(salesOrder);
            var salesOrderOverview = new SalesOrderOverviewComponent(this.salesOrderListPage.Driver);
            var salesOrderOverviewDetail = salesOrderOverview.SalesorderOverviewDetail.Click();

            salesOrderOverviewDetail.BillToCustomer.Select(expected.BillToCustomer?.DisplayName());
            salesOrderOverviewDetail.BillToContactMechanism.Select(expected.BillToContactMechanism);
            salesOrderOverviewDetail.BillToEndCustomerContactMechanism.Select(expected.BillToEndCustomerContactMechanism);
            salesOrderOverviewDetail.BillToEndCustomer.Select(expected.BillToEndCustomer?.DisplayName());
            salesOrderOverviewDetail.ShipToEndCustomer.Select(expected.ShipToEndCustomer?.DisplayName());
            salesOrderOverviewDetail.ShipToEndCustomerAddress.Select(expected.ShipToEndCustomerAddress);
            salesOrderOverviewDetail.ShipToCustomer.Select(expected.ShipToCustomer?.DisplayName());
            salesOrderOverviewDetail.ShipToAddress.Select(expected.ShipToAddress);
            salesOrderOverviewDetail.ShipFromAddress.Select(expected.ShipFromAddress);
            salesOrderOverviewDetail.CustomerReference.Set(expected.CustomerReference);
            salesOrderOverviewDetail.Description.Set(expected.Description);
            salesOrderOverviewDetail.InternalComment.Set(expected.InternalComment);

            if (salesOrder.ExistBillToContactPerson)
            {
                salesOrderOverviewDetail.BillToContactPerson.Select(salesOrder.BillToContactPerson);
            }

            if (salesOrder.ExistShipToEndCustomerContactPerson)
            {
                salesOrderOverviewDetail.ShipToEndCustomerContactPerson.Select(salesOrder.ShipToEndCustomerContactPerson);
            }

            if (salesOrder.ExistShipToContactPerson)
            {
                salesOrderOverviewDetail.ShipToContactPerson.Select(salesOrder.ShipToContactPerson);
            }

            this.Session.Rollback();
            salesOrderOverviewDetail.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();
            salesOrder = (SalesOrder)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedBillToCustomer, salesOrder.BillToCustomer?.DisplayName());
            Assert.Equal(expectedBillToContactMechanism, salesOrder.BillToContactMechanism);
            Assert.Equal(expectedBillToEndCustomerContactMechanism, salesOrder.BillToEndCustomerContactMechanism);
            Assert.Equal(expectedBillToContactPerson, salesOrder.BillToContactPerson);
            Assert.Equal(expectedBillToEndCustomer, salesOrder.BillToEndCustomer?.DisplayName());
            Assert.Equal(expectedShipToEndCustomer, salesOrder.ShipToEndCustomer?.DisplayName());
            Assert.Equal(expectedShipToEndCustomerAddress, salesOrder.ShipToEndCustomerAddress);
            Assert.Equal(expectedShipToEndCustomerContactPerson, salesOrder.ShipToEndCustomerContactPerson);
            Assert.Equal(expectedShipToCustomer, salesOrder.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, salesOrder.ShipToAddress?.DisplayName());
            Assert.Equal(expectedShipToContactPerson, salesOrder.ShipToContactPerson);
            Assert.Equal(expectedShipFromAddressDisplayName, salesOrder.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedCustomerReference, salesOrder.CustomerReference);
            Assert.Equal(expectedDescription, salesOrder.Description);
            Assert.Equal(expectedInternalComment, salesOrder.InternalComment);
        }
    }
}

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
    [Trait("Category", "Order")]
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

        /**
         * MinimalWithInternalOrganisation
         **/
        [Fact]
        public void EditWithInternalOrganisation()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithOrganisationInternalDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer?.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer?.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress;
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

            this.Driver.WaitForAngular();

            salesOrderOverviewDetail.BillToContactMechanism.Select(expected.BillToContactMechanism);
            salesOrderOverviewDetail.BillToContactPerson.Select(expected.BillToContactPerson);
            salesOrderOverviewDetail.ShipFromAddress.Select(expected.ShipFromAddress);
            salesOrderOverviewDetail.ShipToCustomer.Select(expected.ShipToCustomer?.DisplayName());

            this.Driver.WaitForAngular();

            salesOrderOverviewDetail.ShipToAddress.Select(expected.ShipToAddress);
            salesOrderOverviewDetail.ShipToContactPerson.Select(expected.ShipToContactPerson);
            salesOrderOverviewDetail.CustomerReference.Set(expected.CustomerReference);
            salesOrderOverviewDetail.Description.Set(expected.Description);
            salesOrderOverviewDetail.InternalComment.Set(expected.InternalComment);

            this.Session.Rollback();
            salesOrderOverviewDetail.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();
            salesOrder = (SalesOrder)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedBillToCustomer, salesOrder.BillToCustomer?.DisplayName());
            Assert.Equal(expectedBillToContactMechanism, salesOrder.BillToContactMechanism);
            Assert.Equal(expectedBillToContactPerson, salesOrder.BillToContactPerson);
            Assert.Equal(expectedShipToAddressDisplayName, salesOrder.ShipToAddress);
            Assert.Equal(expectedShipFromAddressDisplayName, salesOrder.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedShipToCustomer, salesOrder.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToContactPerson, salesOrder.ShipToContactPerson);
            Assert.Equal(expectedCustomerReference, salesOrder.CustomerReference);
            Assert.Equal(expectedDescription, salesOrder.Description);
            Assert.Equal(expectedInternalComment, salesOrder.InternalComment);
        }

        /**
         * MinimalWithExternalOrganisation
         **/
        [Fact]
        public void EditWithExternalOrganisation()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithOrganisationExternalDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer?.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer?.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
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

            this.Driver.WaitForAngular();

            salesOrderOverviewDetail.BillToContactMechanism.Select(expected.BillToContactMechanism);
            salesOrderOverviewDetail.BillToContactPerson.Select(expected.BillToContactPerson);
            salesOrderOverviewDetail.ShipFromAddress.Select(expected.ShipFromAddress);
            salesOrderOverviewDetail.ShipToCustomer.Select(expected.ShipToCustomer?.DisplayName());

            this.Driver.WaitForAngular();

            salesOrderOverviewDetail.ShipToAddress.Select(expected.ShipToAddress);
            salesOrderOverviewDetail.ShipToContactPerson.Select(expected.ShipToContactPerson);
            salesOrderOverviewDetail.CustomerReference.Set(expected.CustomerReference);
            salesOrderOverviewDetail.Description.Set(expected.Description);
            salesOrderOverviewDetail.InternalComment.Set(expected.InternalComment);

            this.Session.Rollback();
            salesOrderOverviewDetail.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();
            salesOrder = (SalesOrder)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedBillToCustomer, salesOrder.BillToCustomer?.DisplayName());
            Assert.Equal(expectedBillToContactMechanism, salesOrder.BillToContactMechanism);
            Assert.Equal(expectedBillToContactPerson, salesOrder.BillToContactPerson);
            Assert.Equal(expectedShipToAddressDisplayName, salesOrder.ShipToAddress.DisplayName());
            Assert.Equal(expectedShipFromAddressDisplayName, salesOrder.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedShipToCustomer, salesOrder.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToContactPerson, salesOrder.ShipToContactPerson);
            Assert.Equal(expectedCustomerReference, salesOrder.CustomerReference);
            Assert.Equal(expectedDescription, salesOrder.Description);
            Assert.Equal(expectedInternalComment, salesOrder.InternalComment);
        }

        /**
         * MinimalWithInternalPerson
         **/
        [Fact]
        public void EditWithInternalPerson()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithPersonInternalDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer?.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer?.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
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

            this.Driver.WaitForAngular();

            salesOrderOverviewDetail.BillToContactMechanism.Select(expected.BillToContactMechanism);
            salesOrderOverviewDetail.BillToContactPerson.Select(expected.BillToContactPerson);
            salesOrderOverviewDetail.ShipFromAddress.Select(expected.ShipFromAddress);
            salesOrderOverviewDetail.ShipToCustomer.Select(expected.ShipToCustomer?.DisplayName());

            this.Driver.WaitForAngular();

            salesOrderOverviewDetail.ShipToAddress.Select(expected.ShipToAddress);
            salesOrderOverviewDetail.ShipToContactPerson.Select(expected.ShipToContactPerson);
            salesOrderOverviewDetail.CustomerReference.Set(expected.CustomerReference);
            salesOrderOverviewDetail.Description.Set(expected.Description);
            salesOrderOverviewDetail.InternalComment.Set(expected.InternalComment);

            this.Session.Rollback();
            salesOrderOverviewDetail.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();
            salesOrder = (SalesOrder)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedBillToCustomer, salesOrder.BillToCustomer?.DisplayName());
            Assert.Equal(expectedBillToContactMechanism, salesOrder.BillToContactMechanism);
            Assert.Equal(expectedBillToContactPerson, salesOrder.BillToContactPerson);
            Assert.Equal(expectedShipToAddressDisplayName, salesOrder.ShipToAddress.DisplayName());
            Assert.Equal(expectedShipFromAddressDisplayName, salesOrder.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedShipToCustomer, salesOrder.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToContactPerson, salesOrder.ShipToContactPerson);
            Assert.Equal(expectedCustomerReference, salesOrder.CustomerReference);
            Assert.Equal(expectedDescription, salesOrder.Description);
            Assert.Equal(expectedInternalComment, salesOrder.InternalComment);
        }

        /**
        * MinimalWithInternalPerson
        **/
        [Fact]
        public void EditWithExternalPerson()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithPersonExternalDefaults(this.internalOrganisation).Build();

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer?.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedShipToCustomer = expected.ShipToCustomer?.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
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

            this.Driver.WaitForAngular();

            salesOrderOverviewDetail.BillToContactMechanism.Select(expected.BillToContactMechanism);
            salesOrderOverviewDetail.ShipToCustomer.Select(expected.ShipToCustomer?.DisplayName());

            this.Driver.WaitForAngular();

            salesOrderOverviewDetail.ShipToAddress.Select(expected.ShipToAddress);
            salesOrderOverviewDetail.ShipFromAddress.Select(expected.ShipFromAddress);
            salesOrderOverviewDetail.CustomerReference.Set(expected.CustomerReference);
            salesOrderOverviewDetail.Description.Set(expected.Description);
            salesOrderOverviewDetail.InternalComment.Set(expected.InternalComment);

            this.Session.Rollback();
            salesOrderOverviewDetail.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();
            salesOrder = (SalesOrder)this.Session.Instantiate(id);

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(expectedBillToCustomer, salesOrder.BillToCustomer?.DisplayName());
            Assert.Equal(expectedBillToContactMechanism, salesOrder.BillToContactMechanism);
            Assert.Equal(expectedShipToCustomer, salesOrder.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, salesOrder.ShipToAddress.DisplayName());
            Assert.Equal(expectedShipFromAddressDisplayName, salesOrder.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedCustomerReference, salesOrder.CustomerReference);
            Assert.Equal(expectedDescription, salesOrder.Description);
            Assert.Equal(expectedInternalComment, salesOrder.InternalComment);
        }
    }
}

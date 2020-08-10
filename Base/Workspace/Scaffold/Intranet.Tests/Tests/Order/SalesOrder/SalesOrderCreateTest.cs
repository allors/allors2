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
    [Trait("Category", "Order")]
    public class SalesOrderCreateTest : Test
    {
        private readonly SalesOrderListComponent salesOrderListPage;
        private readonly Organisation internalOrganisation;

        public SalesOrderCreateTest(TestFixture fixture)
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
        public void CreateWithInternalOrganisation()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithOrganisationInternalDefaults(this.internalOrganisation).Build();

            Assert.True(expected.ExistShipToAddress);
            Assert.True(expected.ExistComment);
            Assert.True(expected.ExistBillToCustomer);
            Assert.True(expected.ExistBillToContactMechanism);
            Assert.True(expected.ExistBillToContactPerson);
            Assert.True(expected.ExistShipToCustomer);
            Assert.True(expected.ExistShipToAddress);
            Assert.True(expected.ExistShipToContactPerson);
            Assert.True(expected.ExistShipFromAddress);
            Assert.True(expected.ExistCustomerReference);

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
            var expectedShipToContactPerson = expected.ShipToContactPerson;
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress.DisplayName();
            var expectedCustomerReference = expected.CustomerReference;

            var salesOrderCreate = this.salesOrderListPage
                .CreateSalesOrder()
                .BuildForOrganisationInternalDefaults(expected);


            this.Session.Rollback();
            salesOrderCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedBillToCustomer, actual.BillToCustomer?.DisplayName());

            this.Driver.WaitForAngular();

            Assert.Equal(expectedBillToContactMechanism, actual.BillToContactMechanism);
            Assert.Equal(expectedBillToContactPerson, actual.BillToContactPerson);
            Assert.Equal(expectedShipToCustomer, actual.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.ShipToAddress?.DisplayName());
            Assert.Equal(expectedShipToContactPerson, actual.ShipToContactPerson);
            Assert.Equal(expectedShipFromAddressDisplayName, actual.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedCustomerReference, actual.CustomerReference);
        }

        /**
         * MinimalWithExternalOrganisation
         **/
        [Fact]
        public void CreateWithExternalOrganisation()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithOrganisationExternalDefaults(this.internalOrganisation).Build();

            Assert.True(expected.ExistBillToCustomer);
            Assert.True(expected.ExistBillToContactMechanism);
            Assert.True(expected.ExistBillToContactPerson);
            Assert.True(expected.ExistShipToCustomer);
            Assert.True(expected.ExistShipToAddress);
            Assert.True(expected.ExistShipToContactPerson);
            Assert.True(expected.ExistShipFromAddress);
            Assert.True(expected.ExistCustomerReference);

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
            var expectedShipToContactPerson = expected.ShipToContactPerson;
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress.DisplayName();
            var expectedCustomerReference = expected.CustomerReference;

            var salesOrderCreate = this.salesOrderListPage
                .CreateSalesOrder()
                .BuildForOrganisationExternalDefaults(expected);

            this.Session.Rollback();
            salesOrderCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedBillToCustomer, actual.BillToCustomer?.DisplayName());

            this.Driver.WaitForAngular();

            Assert.Equal(expectedBillToContactMechanism, actual.BillToContactMechanism);
            Assert.Equal(expectedBillToContactPerson, actual.BillToContactPerson);
            Assert.Equal(expectedShipToCustomer, actual.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.ShipToAddress?.DisplayName());
            Assert.Equal(expectedShipToContactPerson, actual.ShipToContactPerson);
            Assert.Equal(expectedShipFromAddressDisplayName, actual.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedCustomerReference, actual.CustomerReference);
        }

        /**
         * MinimalWithInternalPerson
         **/
        [Fact]
        public void CreateWithInternalPerson()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithPersonInternalDefaults(this.internalOrganisation).Build();

            Assert.True(expected.ExistBillToCustomer);
            Assert.True(expected.ExistBillToContactMechanism);
            Assert.True(expected.ExistBillToContactPerson);
            Assert.True(expected.ExistShipToCustomer);
            Assert.True(expected.ExistShipToAddress);
            Assert.True(expected.ExistShipToContactPerson);
            Assert.True(expected.ExistShipFromAddress);
            Assert.True(expected.ExistCustomerReference);

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
            var expectedShipToContactPerson = expected.ShipToContactPerson;
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress.DisplayName();
            var expectedCustomerReference = expected.CustomerReference;

            var salesOrderCreate = this.salesOrderListPage
                .CreateSalesOrder()
                .BuildForPersonInternalDefaults(expected);


            this.Session.Rollback();
            salesOrderCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedBillToCustomer, actual.BillToCustomer?.DisplayName());

            this.Driver.WaitForAngular();

            Assert.Equal(expectedBillToContactMechanism, actual.BillToContactMechanism);
            Assert.Equal(expectedBillToContactPerson, actual.BillToContactPerson);
            Assert.Equal(expectedShipToCustomer, actual.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.ShipToAddress?.DisplayName());
            Assert.Equal(expectedShipToContactPerson, actual.ShipToContactPerson);
            Assert.Equal(expectedShipFromAddressDisplayName, actual.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedCustomerReference, actual.CustomerReference);
        }

        /**
        * MinimalWithInternalPerson
        **/
        [Fact]
        public void CreateWithExternalPerson()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = new SalesOrderBuilder(this.Session).WithPersonExternalDefaults(this.internalOrganisation).Build();

            Assert.True(expected.ExistBillToCustomer);
            Assert.True(expected.ExistBillToContactMechanism);
            Assert.True(expected.ExistShipToCustomer);
            Assert.True(expected.ExistShipToAddress);
            Assert.True(expected.ExistShipFromAddress);
            Assert.True(expected.ExistCustomerReference);

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer.DisplayName();
            var expectedBillToContactMechanism = expected.BillToContactMechanism;
            var expectedShipToCustomer = expected.ShipToCustomer.DisplayName();
            var expectedShipToAddressDisplayName = expected.ShipToAddress.DisplayName();
            var expectedShipFromAddressDisplayName = expected.ShipFromAddress.DisplayName();
            var expectedCustomerReference = expected.CustomerReference;

            var salesOrderCreate = this.salesOrderListPage
                .CreateSalesOrder()
                .BuildForPersonExternalDefaults(expected);

            this.Session.Rollback();
            salesOrderCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new SalesOrders(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedBillToCustomer, actual.BillToCustomer?.DisplayName());

            this.Driver.WaitForAngular();

            Assert.Equal(expectedBillToContactMechanism, actual.BillToContactMechanism);
            Assert.Equal(expectedShipToCustomer, actual.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.ShipToAddress?.DisplayName());
            Assert.Equal(expectedShipFromAddressDisplayName, actual.ShipFromAddress?.DisplayName());
            Assert.Equal(expectedCustomerReference, actual.CustomerReference);
        }
    }
}

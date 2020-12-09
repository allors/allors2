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
    using libs.angular.material.@base.src.export.objects.salesorder.create;
    using libs.angular.material.@base.src.export.objects.salesorder.list;
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

            var expected = this.internalOrganisation.CreateInternalSalesOrder(this.Session.Faker());

            Assert.True(expected.ExistDerivedShipToAddress);
            Assert.True(expected.ExistComment);
            Assert.True(expected.ExistBillToCustomer);
            Assert.True(expected.ExistDerivedBillToContactMechanism);
            Assert.True(expected.ExistBillToContactPerson);
            Assert.True(expected.ExistShipToCustomer);
            Assert.True(expected.ExistDerivedShipToAddress);
            Assert.True(expected.ExistShipToContactPerson);
            Assert.True(expected.ExistDerivedShipFromAddress);
            Assert.True(expected.ExistCustomerReference);

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer.DisplayName();
            var expectedBillToContactMechanism = expected.DerivedBillToContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer.DisplayName();
            var expectedShipToAddressDisplayName = expected.DerivedShipToAddress.DisplayName();
            var expectedShipToContactPerson = expected.ShipToContactPerson;
            var expectedShipFromAddressDisplayName = expected.DerivedShipFromAddress.DisplayName();
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

            Assert.Equal(expectedBillToContactMechanism, actual.DerivedBillToContactMechanism);
            Assert.Equal(expectedBillToContactPerson, actual.BillToContactPerson);
            Assert.Equal(expectedShipToCustomer, actual.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.DerivedShipToAddress?.DisplayName());
            Assert.Equal(expectedShipToContactPerson, actual.ShipToContactPerson);
            Assert.Equal(expectedShipFromAddressDisplayName, actual.DerivedShipFromAddress?.DisplayName());
            Assert.Equal(expectedCustomerReference, actual.CustomerReference);
        }

        /**
         * MinimalWithExternalOrganisation
         **/
        [Fact]
        public void CreateWithExternalOrganisation()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = this.internalOrganisation.CreateB2BSalesOrder(this.Session.Faker());

            Assert.True(expected.ExistBillToCustomer);
            Assert.True(expected.ExistDerivedBillToContactMechanism);
            Assert.True(expected.ExistBillToContactPerson);
            Assert.True(expected.ExistShipToCustomer);
            Assert.True(expected.ExistDerivedShipToAddress);
            Assert.True(expected.ExistShipToContactPerson);
            Assert.True(expected.ExistDerivedShipFromAddress);
            Assert.True(expected.ExistCustomerReference);

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer.DisplayName();
            var expectedBillToContactMechanism = expected.DerivedBillToContactMechanism;
            var expectedBillToContactPerson = expected.BillToContactPerson;
            var expectedShipToCustomer = expected.ShipToCustomer.DisplayName();
            var expectedShipToAddressDisplayName = expected.DerivedShipToAddress.DisplayName();
            var expectedShipToContactPerson = expected.ShipToContactPerson;
            var expectedShipFromAddressDisplayName = expected.DerivedShipFromAddress.DisplayName();
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

            Assert.Equal(expectedBillToContactMechanism, actual.DerivedBillToContactMechanism);
            Assert.Equal(expectedBillToContactPerson, actual.BillToContactPerson);
            Assert.Equal(expectedShipToCustomer, actual.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.DerivedShipToAddress?.DisplayName());
            Assert.Equal(expectedShipToContactPerson, actual.ShipToContactPerson);
            Assert.Equal(expectedShipFromAddressDisplayName, actual.DerivedShipFromAddress?.DisplayName());
            Assert.Equal(expectedCustomerReference, actual.CustomerReference);
        }

        /**
        * MinimalWithInternalPerson
        **/
        [Fact]
        public void CreateWithExternalPerson()
        {
            var before = new SalesOrders(this.Session).Extent().ToArray();

            var expected = this.internalOrganisation.CreateB2CSalesOrder(this.Session.Faker());

            Assert.True(expected.ExistBillToCustomer);
            Assert.True(expected.ExistDerivedBillToContactMechanism);
            Assert.True(expected.ExistShipToCustomer);
            Assert.True(expected.ExistDerivedShipToAddress);
            Assert.True(expected.ExistDerivedShipFromAddress);
            Assert.True(expected.ExistCustomerReference);

            this.Session.Derive();

            var expectedBillToCustomer = expected.BillToCustomer.DisplayName();
            var expectedBillToContactMechanism = expected.DerivedBillToContactMechanism;
            var expectedShipToCustomer = expected.ShipToCustomer.DisplayName();
            var expectedShipToAddressDisplayName = expected.DerivedShipToAddress.DisplayName();
            var expectedShipFromAddressDisplayName = expected.DerivedShipFromAddress.DisplayName();
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

            Assert.Equal(expectedBillToContactMechanism, actual.DerivedBillToContactMechanism);
            Assert.Equal(expectedShipToCustomer, actual.ShipToCustomer?.DisplayName());
            Assert.Equal(expectedShipToAddressDisplayName, actual.DerivedShipToAddress?.DisplayName());
            Assert.Equal(expectedShipFromAddressDisplayName, actual.DerivedShipFromAddress?.DisplayName());
            Assert.Equal(expectedCustomerReference, actual.CustomerReference);
        }
    }
}

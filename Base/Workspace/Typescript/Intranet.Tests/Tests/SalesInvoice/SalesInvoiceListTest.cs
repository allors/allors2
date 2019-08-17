// <copyright file="SalesInvoiceListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.SalesInvoicesOverviewTest
{
    using Xunit;

    [Collection("Test collection")]
    public class SalesInvoiceListTest : Test
    {
        public SalesInvoiceListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToSalesInvoices();
        }

        [Fact]
        public void Title() => Assert.Equal("Sales Invoices", this.Driver.Title);
    }
}

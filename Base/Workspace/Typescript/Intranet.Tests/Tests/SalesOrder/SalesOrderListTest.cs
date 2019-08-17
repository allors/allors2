// <copyright file="SalesOrderListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.SalesOrderTest
{
    using Xunit;

    [Collection("Test collection")]
    public class SalesOrderListTest : Test
    {
        public SalesOrderListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToSalesOrders();
        }

        [Fact]
        public void Title() => Assert.Equal("Sales Orders", this.Driver.Title);
    }
}

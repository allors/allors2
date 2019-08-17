// <copyright file="PurchaseInvoiceListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PurchaseInvoiceTest
{
    using Xunit;

    [Collection("Test collection")]
    public class PurchaseInvoiceListTest : Test
    {
        public PurchaseInvoiceListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToPurchaseInvoices();
        }

        [Fact]
        public void Title() => Assert.Equal("Purchase Invoices", this.Driver.Title);
    }
}

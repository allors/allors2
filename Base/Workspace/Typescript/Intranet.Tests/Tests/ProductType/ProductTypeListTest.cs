// <copyright file="ProductTypeListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ProductTypeTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductTypeListTest : Test
    {
        public ProductTypeListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToProductTypes();
        }

        [Fact]
        public void Title() => Assert.Equal("Product Types", this.Driver.Title);
    }
}

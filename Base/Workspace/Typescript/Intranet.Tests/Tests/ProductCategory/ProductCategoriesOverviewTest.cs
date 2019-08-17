// <copyright file="ProductCategoriesOverviewTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ProductCategoryTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductCategoryListTest : Test
    {
        public ProductCategoryListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToProductCategories();
        }

        [Fact]
        public void Title() => Assert.Equal("Categories", this.Driver.Title);
    }
}

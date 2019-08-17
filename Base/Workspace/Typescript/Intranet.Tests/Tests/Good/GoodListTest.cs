// <copyright file="GoodListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ProductTest
{
    using Xunit;

    [Collection("Test collection")]
    public class GoodListTest : Test
    {
        public GoodListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToGoods();
        }

        [Fact]
        public void Title() => Assert.Equal("Goods", this.Driver.Title);
    }
}

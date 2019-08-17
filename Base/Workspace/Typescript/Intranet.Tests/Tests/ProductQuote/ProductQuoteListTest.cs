// <copyright file="ProductQuoteListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.ProductQuoteTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductQuoteListTest : Test
    {
        public ProductQuoteListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToProductQuotes();
        }

        [Fact]
        public void Title() => Assert.Equal("Quotes", this.Driver.Title);
    }
}

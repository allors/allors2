// <copyright file="RequestsForQuoteListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.RequestsForQuoteTest
{
    using Xunit;

    [Collection("Test collection")]
    public class RequestsForQuoteListTest : Test
    {
        public RequestsForQuoteListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToRequestsForQuote();
        }

        [Fact]
        public void Title() => Assert.Equal("Requests", this.Driver.Title);
    }
}

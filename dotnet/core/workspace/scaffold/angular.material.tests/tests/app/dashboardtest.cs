// <copyright file="DashboardTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using Xunit;

    [Collection("Test collection")]
    public class DashboardTest : Test
    {
        public DashboardTest(TestFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public void Title()
        {
            this.Login();

            Assert.Equal("Dashboard", this.Driver.Title);
        }
    }
}

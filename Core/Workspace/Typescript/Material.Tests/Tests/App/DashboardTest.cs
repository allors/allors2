// <copyright file="DashboardTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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

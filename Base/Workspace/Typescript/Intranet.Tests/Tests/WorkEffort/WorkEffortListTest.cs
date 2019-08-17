// <copyright file="WorkEffortListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.WorkEffortOverviewTests
{
    using Xunit;

    [Collection("Test collection")]
    public class WorkEffortListTest : Test
    {
        public WorkEffortListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToWorkEfforts();
        }

        [Fact]
        public void Title() => Assert.Equal("Work Orders", this.Driver.Title);
    }
}

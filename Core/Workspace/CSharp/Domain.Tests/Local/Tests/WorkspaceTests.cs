// <copyright file="WorkspaceTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Local
{
    using Allors.Protocol.Remote.Pull;

    using Xunit;

    public class WorkspaceTests : LocalTest
    {
        [Fact]
        public void Load()
        {
            this.Workspace.Sync(Fixture.loadData);

            var martien = this.Workspace.Get(3);

            Assert.Equal(3, martien.Id);
            Assert.Equal(1003, martien.Version);
            Assert.Equal("Person", martien.ObjectType.Name);
            Assert.Equal("Martien", martien.Roles["FirstName"]);
            Assert.Equal("van", martien.Roles["MiddleName"]);
            Assert.Equal("Knippenberg", martien.Roles["LastName"]);
            Assert.False(martien.Roles.ContainsKey("IsStudent"));
            Assert.False(martien.Roles.ContainsKey("BirthDate"));
        }

        [Fact]
        public void CheckVersions()
        {
            this.Workspace.Sync(Fixture.loadData);

            var required = new PullResponse
            {
                UserSecurityHash = "#",
                Objects =
                                       new[]
                                           {
                                                new[] { "1", "1001" },
                                                new[] { "2", "1002" },
                                                new[] { "3", "1004" }
                                           },
            };

            var requireLoad = this.Workspace.Diff(required);

            Assert.Equal(1, requireLoad.Objects.Length);
        }

        [Fact]
        public void CheckVersionsUserSecurityHash()
        {
            this.Workspace.Sync(Fixture.loadData);

            var required = new PullResponse
            {
                UserSecurityHash = "def",
                Objects =
                                       new[]
                                           {
                                                new[] { "1", "1001" },
                                                new[] { "2", "1002" },
                                                new[] { "3", "1004" }
                                           },
            };

            var requireLoad = this.Workspace.Diff(required);

            Assert.Equal(3, requireLoad.Objects.Length);
        }
    }
}

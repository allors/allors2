// <copyright file="WorkspaceTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Mock
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol;
    using Allors.Protocol.Remote.Pull;
    using Allors.Workspace;
    using Allors.Workspace.Meta;
    using Xunit;

    public class WorkspaceTests : MockTest
    {
        [Fact]
        public void Load()
        {
            this.Workspace.Sync(Fixture.LoadData);

            object Value(IWorkspaceObject @object, IRoleType roleType) => @object.Roles.First(v => v.RoleType == roleType).Value;

            var martien = this.Workspace.Get(3);

            Assert.Equal(3, martien.Id);
            Assert.Equal(1003, martien.Version);
            Assert.Equal("Person", martien.Class.Name);
            Assert.Equal("Martien", Value(martien, M.Person.FirstName));
            Assert.Equal("van", Value(martien, M.Person.MiddleName));
            Assert.Equal("Knippenberg", Value(martien, M.Person.LastName));
            Assert.DoesNotContain(martien.Roles, v => v.RoleType == M.Person.IsStudent);
            Assert.DoesNotContain(martien.Roles, v => v.RoleType == M.Person.BirthDate);
        }

        [Fact]
        public void Diff()
        {
            this.Workspace.Sync(Fixture.LoadData);
            var pullResponse = new PullResponse
            {
                Objects =
                       new[]
                           {
                                new[] { "1", "1001", "101" },
                                new[] { "2", "1002", "102", "103" },
                                new[] { "3", "1003" },
                           },
            };

            var requireLoad = this.Workspace.Diff(pullResponse);

            Assert.Empty(requireLoad.Objects);
        }

        [Fact]
        public void DiffVersion()
        {
            this.Workspace.Sync(Fixture.LoadData);
            var pullResponse = new PullResponse
            {
                Objects =
                    new[]
                    {
                        new[] { "1", "1001", "101" },
                        new[] { "2", "1002", "102", "103" },
                        new[] { "3", "1004" },
                    },
            };

            var requireLoad = this.Workspace.Diff(pullResponse);

            Assert.Single(requireLoad.Objects);

            Assert.Equal("3", requireLoad.Objects[0]);
        }

        [Fact]
        public void DiffAccessControl()
        {
            this.Workspace.Sync(Fixture.LoadData);
            var pullResponse = new PullResponse
            {
                Objects =
                    new[]
                    {
                        new[] { "1", "1001", "201" },
                        new[] { "2", "1002", "102", "103" },
                        new[] { "3", "1003" },
                    },
            };

            var requireLoad = this.Workspace.Diff(pullResponse);

            Assert.Single(requireLoad.Objects);

            Assert.Equal("1", requireLoad.Objects[0]);
        }

        [Fact]
        public void DiffChangeDeniedPermission()
        {
            this.Workspace.Sync(Fixture.LoadData);
            var pullResponse = new PullResponse
            {
                Objects =
                    new[]
                    {
                        new[] { "1", "1001", "101" },
                        new[] { "2", "1002", "102", "104" },
                        new[] { "3", "1003" },
                    },
            };

            var requireLoad = this.Workspace.Diff(pullResponse);

            Assert.Single(requireLoad.Objects);

            Assert.Equal("2", requireLoad.Objects[0]);
        }

        [Fact]
        public void DiffAddDeniedPermission()
        {
            this.Workspace.Sync(Fixture.LoadData);
            var pullResponse = new PullResponse
            {
                Objects =
                    new[]
                    {
                        new[] { "1", "1001", "101", "104" },
                        new[] { "2", "1002", "102", "103" },
                        new[] { "3", "1003" },
                    },
            };

            var requireLoad = this.Workspace.Diff(pullResponse);

            Assert.Single(requireLoad.Objects);

            Assert.Equal("1", requireLoad.Objects[0]);
        }

        [Fact]
        public void DiffRemoveDeniedPermission()
        {
            this.Workspace.Sync(Fixture.LoadData);
            var pullResponse = new PullResponse
            {
                Objects =
                    new[]
                    {
                        //new[] { "1", "1001", "101" },
                        new[] { "2", "1002", "102" },
                        //new[] { "3", "1003" },
                    },
            };

            var requireLoad = this.Workspace.Diff(pullResponse);

            Assert.Single(requireLoad.Objects);

            Assert.Equal("2", requireLoad.Objects[0]);
        }
    }
}

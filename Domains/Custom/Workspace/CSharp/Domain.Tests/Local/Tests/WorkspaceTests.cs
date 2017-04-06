namespace Tests.Local
{
    using Allors.Workspace.Data;
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
                                   userSecurityHash = "#",
                                   objects =
                                       new[]
                                           {
                                                new[] { "1", "1001" },
                                                new[] { "2", "1002" },
                                                new[] { "3", "1004" }
                                           }
                               };

            var requireLoad = this.Workspace.Diff(required);

            Assert.Equal(1, requireLoad.objects.Length);
        }

        [Fact]
        public void CheckVersionsUserSecurityHash()
        {
            this.Workspace.Sync(Fixture.loadData);

            var required = new PullResponse
                               {
                                   userSecurityHash = "def",
                                   objects =
                                       new[]
                                           {
                                                new[] { "1", "1001" },
                                                new[] { "2", "1002" },
                                                new[] { "3", "1004" }
                                           }
                               };

            var requireLoad = this.Workspace.Diff(required);

            Assert.Equal(3, requireLoad.objects.Length);
        }
    }
}
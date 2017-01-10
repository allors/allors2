namespace Tests.Local
{
    using Allors.Workspace.Data;
    using Allors.Workspace;
    using NUnit.Framework;

    [TestFixture]
    public class WorkspaceTests
    {
        [Test]
        public void Load()
        {
            var workspace = new Workspace(Config.ObjectFactory);
            workspace.Sync(Fixture.loadData);

            var martien = workspace.Get(3);

            Assert.AreEqual(3, martien.Id);
            Assert.AreEqual(1003, martien.Version);
            Assert.AreEqual("Person", martien.ObjectType.Name);
            Assert.AreEqual("Martien", martien.Roles["FirstName"]);
            Assert.AreEqual("van", martien.Roles["MiddleName"]);
            Assert.AreEqual("Knippenberg", martien.Roles["LastName"]);
            Assert.IsFalse(martien.Roles.ContainsKey("IsStudent"));
            Assert.IsFalse(martien.Roles.ContainsKey("BirthDate"));
        }

        [Test]
        public void CheckVersions()
        {
            var workspace = new Workspace(Config.ObjectFactory);
            workspace.Sync(Fixture.loadData);

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

            var requireLoad = workspace.Diff(required);

            Assert.AreEqual(1, requireLoad.objects.Length);
        }

        [Test]
        public void CheckVersionsUserSecurityHash()
        {
            var workspace = new Workspace(Config.ObjectFactory);
            workspace.Sync(Fixture.loadData);

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

            var requireLoad = workspace.Diff(required);

            Assert.AreEqual(3, requireLoad.objects.Length);
        }
    }
}
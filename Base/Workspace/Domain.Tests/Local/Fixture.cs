namespace Tests.Local
{
    using Allors.Workspace.Data;

    public class Fixture
    {
        public static SyncResponse loadData = new SyncResponse
        {
            userSecurityHash = "#",
            objects = new[]
            {
                    new SyncResponseObject
                    {
                        i = "1",
                        v = "1001",
                        t = "Person",
                        roles = new[]
                                   {
                                        new object[] { "FirstName", "rw", "Koen" },
                                        new object[] { "LastName", "rw", "Van Exem" },
                                        new object[] { "BirthDate", "rw", "1973-03-27T18:00:00Z" },
                                        new object[] { "IsStudent", "rw", true }
                                    }
                    },
                    new SyncResponseObject
                    {
                        i = "2",
                        v = "1002",
                        t = "Person",
                        roles = new[]
                                {
                                    new object[] { "FirstName", "rw", "Patrick" },
                                    new object[] { "LastName", "rw", "De Boeck" },
                                    new object[] { "IsStudent", "rw", false }
                                }
                    },
                    new SyncResponseObject
                    {
                        i = "3",
                        v = "1003",
                        t = "Person",
                        roles = new[] 
                        {
                            new object[] { "FirstName", "rw", "Martien" },
                            new object[] { "MiddleName", "rw", "van" },
                            new object[] { "LastName", "rw", "Knippenberg" },
                        }
                    },
                    new SyncResponseObject
                    {
                        i = "101",
                        v = "1101",
                        t = "Organisation",
                        roles = new[] 
                        {
                            new object[] { "Name", "rw", "Acme" },
                            new object[] { "Owner", "rw", "1" },
                            new object[] { "Employees", "rw", new[] { "1", "2", "3" } }
                        },
                        methods = new[]
                        {
                            new[] { "JustDoIt", "x" }
                        }
                    },
                    new SyncResponseObject
                    {
                        i = "102",
                        v = "1102",
                        t = "Organisation",
                        roles = new[]
                        {
                            new object[] { "Name", "rw", "Ocme" },
                            new object[] { "Owner", "rw", "2" },
                            new object[] { "Employees", "rw", new[] { "1" } }
                        },
                        methods = new[]
                        {
                            new[] { "JustDoIt", string.Empty }
                        }
                    },
                    new SyncResponseObject
                    {
                        i = "103",
                        v = "1103",
                        t = "Organisation",
                        roles = new[]
                        {
                            new object[] { "Name", "rw", "icme" },
                            new object[] { "Owner", "rw", "3" }
                        },
                        methods = new[]
                        {
                            new[] { "JustDoIt", "" }
                        }
                    }
                }
        };
    }
}


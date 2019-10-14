// <copyright file="Fixture.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Mock
{
    using Allors.Protocol.Remote.Sync;
    using Allors.Workspace.Meta;

    public class Fixture
    {
        public static SyncResponse LoadData =>
            new SyncResponse
            {
                Objects = new[]
                {
                    new SyncResponseObject
                    {
                        I = "1",
                        V = "1001",
                        T = M.Person.ObjectType.IdAsString,
                        R = new[]
                        {
                            new SyncResponseRole { T = M.Person.FirstName.IdAsString, V = "Koen"},
                            new SyncResponseRole { T = M.Person.LastName.IdAsString, V = "Van Exem"},
                            new SyncResponseRole { T = M.Person.BirthDate.IdAsString, V = "1973-03-27T18:00:00Z"},
                            new SyncResponseRole { T = M.Person.IsStudent.IdAsString, V = "1"},
                        },
                        A = "101",
                    },
                    new SyncResponseObject
                    {
                        I = "2",
                        V = "1002",
                        T = M.Person.ObjectType.IdAsString,
                        R = new[]
                        {
                            new SyncResponseRole { T = M.Person.FirstName.IdAsString, V = "Patrick"},
                            new SyncResponseRole { T = M.Person.LastName.IdAsString, V = "De Boeck"},
                            new SyncResponseRole { T = M.Person.IsStudent.IdAsString, V = "0"},
                        },
                        A = "102",
                        D = "103",
                    },
                    new SyncResponseObject
                    {
                        I = "3",
                        V = "1003",
                        T = M.Person.ObjectType.IdAsString,
                        R = new[]
                        {
                            new SyncResponseRole { T = M.Person.FirstName.IdAsString, V = "Martien"},
                            new SyncResponseRole { T = M.Person.MiddleName.IdAsString, V = "van"},
                            new SyncResponseRole { T = M.Person.LastName.IdAsString, V = "Knippenberg"},
                        },
                    },
                    new SyncResponseObject
                    {
                        I = "101",
                        V = "1101",
                        T = M.Organisation.ObjectType.IdAsString,
                        R = new[]
                        {
                            new SyncResponseRole { T = M.Organisation.Name.IdAsString, V = "Acme" },
                            new SyncResponseRole { T = M.Organisation.Owner.IdAsString, V = "1" },
                            new SyncResponseRole { T = M.Organisation.Employees.IdAsString, V = "1,2,3" },
                            new SyncResponseRole { T = M.Organisation.Manager.IdAsString},
                        },
                    },
                    new SyncResponseObject
                    {
                        I = "102",
                        V = "1102",
                        T = M.Organisation.ObjectType.IdAsString,
                        R = new[]
                        {
                            new SyncResponseRole { T = M.Organisation.Name.IdAsString, V = "Ocme" },
                            new SyncResponseRole { T = M.Organisation.Owner.IdAsString, V = "2" },
                            new SyncResponseRole { T = M.Organisation.Employees.IdAsString, V = "1" },
                        },
                    },
                    new SyncResponseObject
                    {
                        I = "103",
                        V = "1103",
                        T = M.Organisation.ObjectType.IdAsString,
                        R = new[]
                        {
                            new SyncResponseRole { T = M.Organisation.Name.IdAsString, V = "icme" },
                            new SyncResponseRole { T = M.Organisation.Owner.IdAsString, V = "3" },
                        },
                    },
                },
            };
    }
}

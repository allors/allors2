// <copyright file="Fixture.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
                objects = new[]
                {
                    new SyncResponseObject
                    {
                        i = "1",
                        v = "1001",
                        t = M.Person.ObjectType.IdAsString,
                        r = new[]
                        {
                            new SyncResponseRole { t = M.Person.FirstName.IdAsString, v = "Koen"},
                            new SyncResponseRole { t = M.Person.LastName.IdAsString, v = "Van Exem"},
                            new SyncResponseRole { t = M.Person.BirthDate.IdAsString, v = "1973-03-27T18:00:00Z"},
                            new SyncResponseRole { t = M.Person.IsStudent.IdAsString, v = "1"},
                        },
                        a = "101",
                    },
                    new SyncResponseObject
                    {
                        i = "2",
                        v = "1002",
                        t = M.Person.ObjectType.IdAsString,
                        r = new[]
                        {
                            new SyncResponseRole { t = M.Person.FirstName.IdAsString, v = "Patrick"},
                            new SyncResponseRole { t = M.Person.LastName.IdAsString, v = "De Boeck"},
                            new SyncResponseRole { t = M.Person.IsStudent.IdAsString, v = "0"},
                        },
                        a = "102",
                        d = "103",
                    },
                    new SyncResponseObject
                    {
                        i = "3",
                        v = "1003",
                        t = M.Person.ObjectType.IdAsString,
                        r = new[]
                        {
                            new SyncResponseRole { t = M.Person.FirstName.IdAsString, v = "Martien"},
                            new SyncResponseRole { t = M.Person.MiddleName.IdAsString, v = "van"},
                            new SyncResponseRole { t = M.Person.LastName.IdAsString, v = "Knippenberg"},
                        },
                    },
                    new SyncResponseObject
                    {
                        i = "101",
                        v = "1101",
                        t = M.Organisation.ObjectType.IdAsString,
                        r = new[]
                        {
                            new SyncResponseRole { t = M.Organisation.Name.IdAsString, v = "Acme" },
                            new SyncResponseRole { t = M.Organisation.Owner.IdAsString, v = "1" },
                            new SyncResponseRole { t = M.Organisation.Employees.IdAsString, v = "1|2|3" },
                            new SyncResponseRole { t = M.Organisation.Manager.IdAsString},
                        },
                    },
                    new SyncResponseObject
                    {
                        i = "102",
                        v = "1102",
                        t = M.Organisation.ObjectType.IdAsString,
                        r = new[]
                        {
                            new SyncResponseRole { t = M.Organisation.Name.IdAsString, v = "Ocme" },
                            new SyncResponseRole { t = M.Organisation.Owner.IdAsString, v = "2" },
                            new SyncResponseRole { t = M.Organisation.Employees.IdAsString, v = "1" },
                        },
                    },
                    new SyncResponseObject
                    {
                        i = "103",
                        v = "1103",
                        t = M.Organisation.ObjectType.IdAsString,
                        r = new[]
                        {
                            new SyncResponseRole { t = M.Organisation.Name.IdAsString, v = "icme" },
                            new SyncResponseRole { t = M.Organisation.Owner.IdAsString, v = "3" },
                        },
                    },
                },
            };
    }
}

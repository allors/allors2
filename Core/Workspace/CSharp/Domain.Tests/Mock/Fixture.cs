// <copyright file="Fixture.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Mock
{
    using System.Collections.Generic;
    using Allors.Protocol;
    using Allors.Protocol.Remote.Sync;
    using Allors.Server;
    using Allors.Workspace;
    using Allors.Workspace.Meta;

    public class Fixture
    {
        public static SyncResponse LoadData
        {
            get
            {
                var c = new Compressor();
                var mc = new MetaObjectCompressor(c);

                return new SyncResponse
                {
                    Objects = new[]
                    {
                        new SyncResponseObject
                        {
                            I = "1",
                            V = "1001",
                            T = mc.Write(M.Person.ObjectType),
                            R = new[]
                            {
                                new SyncResponseRole { T = mc.Write(M.Person.FirstName), V = "Koen"},
                                new SyncResponseRole { T = mc.Write(M.Person.LastName), V = "Van Exem"},
                                new SyncResponseRole { T = mc.Write(M.Person.BirthDate), V = "1973-03-27T18:00:00Z"},
                                new SyncResponseRole { T = mc.Write(M.Person.IsStudent), V = "1"},
                            },
                            A = c.Write("101"),
                        },
                        new SyncResponseObject
                        {
                            I = "2",
                            V = "1002",
                            T = mc.Write(M.Person.ObjectType),
                            R = new[]
                            {
                                new SyncResponseRole { T = mc.Write(M.Person.FirstName), V = "Patrick"},
                                new SyncResponseRole { T = mc.Write(M.Person.LastName), V = "De Boeck"},
                                new SyncResponseRole { T = mc.Write(M.Person.IsStudent), V = "0"},
                            },
                            A = c.Write("102"),
                            D = c.Write("103"),
                        },
                        new SyncResponseObject
                        {
                            I = "3",
                            V = "1003",
                            T = mc.Write(M.Person.ObjectType),
                            R = new[]
                            {
                                new SyncResponseRole { T = mc.Write(M.Person.FirstName), V = "Martien"},
                                new SyncResponseRole { T = mc.Write(M.Person.MiddleName), V = "van"},
                                new SyncResponseRole { T = mc.Write(M.Person.LastName), V = "Knippenberg"},
                            },
                        },
                        new SyncResponseObject
                        {
                            I = "101",
                            V = "1101",
                            T = mc.Write(M.Organisation.ObjectType),
                            R = new[]
                            {
                                new SyncResponseRole { T = mc.Write(M.Organisation.Name), V = "Acme" },
                                new SyncResponseRole { T = mc.Write(M.Organisation.Owner), V = "1" },
                                new SyncResponseRole { T = mc.Write(M.Organisation.Employees), V = "1,2,3" },
                                new SyncResponseRole { T = mc.Write(M.Organisation.Manager)},
                            },
                        },
                        new SyncResponseObject
                        {
                            I = "102",
                            V = "1102",
                            T = mc.Write(M.Organisation.ObjectType),
                            R = new[]
                            {
                                new SyncResponseRole { T = mc.Write(M.Organisation.Name), V = "Ocme" },
                                new SyncResponseRole { T = mc.Write(M.Organisation.Owner), V = "2" },
                                new SyncResponseRole { T = mc.Write(M.Organisation.Employees), V = "1" },
                            },
                        },
                        new SyncResponseObject
                        {
                            I = "103",
                            V = "1103",
                            T = mc.Write(M.Organisation.ObjectType),
                            R = new[]
                            {
                                new SyncResponseRole { T = mc.Write(M.Organisation.Name), V = "icme" },
                                new SyncResponseRole { T = mc.Write(M.Organisation.Owner), V = "3" },
                            },
                        },
                    },
                };
            }
        }
    }
}

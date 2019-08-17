// <copyright file="Fixture.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Local
{
    using Allors.Protocol.Remote.Sync;

    public class Fixture
    {
        public static SyncResponse loadData = new SyncResponse
        {
            UserSecurityHash = "#",
            Objects = new[]
            {
                    new SyncResponseObject
                    {
                        I = "1",
                        V = "1001",
                        T = "Person",
                        Roles = new[]
                                   {
                                        new object[] { "FirstName", "rw", "Koen" },
                                        new object[] { "LastName", "rw", "Van Exem" },
                                        new object[] { "BirthDate", "rw", "1973-03-27T18:00:00Z" },
                                        new object[] { "IsStudent", "rw", true }
                                    },
                    },
                    new SyncResponseObject
                    {
                        I = "2",
                        V = "1002",
                        T = "Person",
                        Roles = new[]
                                {
                                    new object[] { "FirstName", "rw", "Patrick" },
                                    new object[] { "LastName", "rw", "De Boeck" },
                                    new object[] { "IsStudent", "rw", false }
                                },
                    },
                    new SyncResponseObject
                    {
                        I = "3",
                        V = "1003",
                        T = "Person",
                        Roles = new[]
                        {
                            new object[] { "FirstName", "rw", "Martien" },
                            new object[] { "MiddleName", "rw", "van" },
                            new object[] { "LastName", "rw", "Knippenberg" },
                        },
                    },
                    new SyncResponseObject
                    {
                        I = "101",
                        V = "1101",
                        T = "Organisation",
                        Roles = new[]
                        {
                            new object[] { "Name", "rw", "Acme" },
                            new object[] { "Owner", "rw", "1" },
                            new object[] { "Employees", "rw", new[] { "1", "2", "3" } },
                        },
                        Methods = new[]
                        {
                            new[] { "JustDoIt", "x" }
                        },
                    },
                    new SyncResponseObject
                    {
                        I = "102",
                        V = "1102",
                        T = "Organisation",
                        Roles = new[]
                        {
                            new object[] { "Name", "rw", "Ocme" },
                            new object[] { "Owner", "rw", "2" },
                            new object[] { "Employees", "rw", new[] { "1" } },
                        },
                        Methods = new[]
                        {
                            new[] { "JustDoIt", string.Empty }
                        },
                    },
                    new SyncResponseObject
                    {
                        I = "103",
                        V = "1103",
                        T = "Organisation",
                        Roles = new[]
                        {
                            new object[] { "Name", "rw", "icme" },
                            new object[] { "Owner", "rw", "3" }
                        },
                        Methods = new[]
                        {
                            new[] { "JustDoIt", "" }
                        }
                    },
                },
        };
    }
}


// <copyright file="Fixture.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Mock
{
    using System.Collections.Generic;
    using Allors.Protocol.Remote.Sync;
    using Allors.Workspace.Meta;

    public class Fixture
    {
        public static SyncResponse LoadData
        {
            get
            {
                var keyByMetaObject = new Dictionary<IMetaObject, string>();
                var index = 0;

                string encode(IMetaObject metaObject)
                {
                    if (keyByMetaObject.TryGetValue(metaObject, out var key))
                    {
                        return key;
                    }

                    key = (++index).ToString();
                    keyByMetaObject.Add(metaObject, key);
                    return $":{key}:{metaObject.Id.ToString("D").ToLower()}";
                }

                return new SyncResponse
                {
                    UserSecurityHash = "#",
                    Objects = new[]
                    {
                        new SyncResponseObject
                        {
                            I = "1",
                            V = "1001",
                            T = encode(M.Person.ObjectType),
                            W = new[]
                            {
                                new SyncResponseRole { T = encode(M.Person.FirstName), V = "Koen"},
                                new SyncResponseRole { T = encode(M.Person.LastName), V = "Van Exem"},
                                new SyncResponseRole { T = encode(M.Person.BirthDate), V = "1973-03-27T18:00:00Z"},
                                new SyncResponseRole { T = encode(M.Person.IsStudent), V = "0"},
                            },
                        },
                        new SyncResponseObject
                        {
                            I = "2",
                            V = "1002",
                            T = encode(M.Person.ObjectType),
                            W = new[]
                            {
                                new SyncResponseRole { T = encode(M.Person.FirstName), V = "Patrick"},
                                new SyncResponseRole { T = encode(M.Person.LastName), V = "De Boeck"},
                                new SyncResponseRole { T = encode(M.Person.IsStudent), V = "1"},
                            },
                        },
                        new SyncResponseObject
                        {
                            I = "3",
                            V = "1003",
                            T = encode(M.Person.ObjectType),
                            W = new[]
                            {
                                new SyncResponseRole { T = encode(M.Person.FirstName), V = "Martien"},
                                new SyncResponseRole { T = encode(M.Person.MiddleName), V = "van"},
                                new SyncResponseRole { T = encode(M.Person.LastName), V = "Knippenberg"},
                            },
                        },
                        new SyncResponseObject
                        {
                            I = "101",
                            V = "1101",
                            T = encode(M.Organisation.ObjectType),
                            W = new[]
                            {
                                new SyncResponseRole { T = encode(M.Organisation.Name), V = "Acme" },
                                new SyncResponseRole { T = encode(M.Organisation.Owner), V = "1" },
                                new SyncResponseRole { T = encode(M.Organisation.Employees), V = "1,2,3" },
                                new SyncResponseRole { T = encode(M.Organisation.Manager)},
                            },
                            X = new[] { encode(M.Organisation.JustDoIt) },
                        },
                        new SyncResponseObject
                        {
                            I = "102",
                            V = "1102",
                            T = encode(M.Organisation.ObjectType),
                            W = new[]
                            {
                                new SyncResponseRole { T = encode(M.Organisation.Name), V = "Ocme" },
                                new SyncResponseRole { T = encode(M.Organisation.Owner), V = "2" },
                                new SyncResponseRole { T = encode(M.Organisation.Employees), V = "1" },
                            },
                            X = new[] { encode(M.Organisation.JustDoIt) },
                        },
                        new SyncResponseObject
                        {
                            I = "103",
                            V = "1103",
                            T = encode(M.Organisation.ObjectType),
                            W = new[]
                            {
                                new SyncResponseRole { T = encode(M.Organisation.Name), V = "icme" },
                                new SyncResponseRole { T = encode(M.Organisation.Owner), V = "3" },
                            },
                            X = new[] { encode(M.Organisation.JustDoIt) },
                        },
                    },
                };
            }
        }
    }
}


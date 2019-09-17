// <copyright file="ObjectTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Remote
{
    using System.Linq;
    using Allors.Workspace;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Nito.AsyncEx;
    using Xunit;
    using Result = Allors.Workspace.Data.Result;

    public class AssociationTests : RemoteTest
    {
        [Fact]
        public void Get() =>
            AsyncContext.Run(
                async () =>
                {
                    var context = new Context(this.Database, this.Workspace);

                    var pull = new Pull[]
                    {
                        new Pull
                        {
                            Extent = new Filter(M.C1.ObjectType),
                            Results = new[]
                            {
                                new Result
                                {
                                    Fetch = new Fetch
                                    {
                                        Include = new[]
                                            {
                                                new Node(M.C1.C1C1One2One),
                                                new Node(M.C1.C1C1One2Manies),
                                                new Node(M.C1.C1C1Many2One),
                                                new Node(M.C1.C1C1Many2Manies),
                                                new Node(M.C1.C1C2One2One),
                                                new Node(M.C1.C1C2One2Manies),
                                                new Node(M.C1.C1C2Many2One),
                                                new Node(M.C1.C1C2Many2Manies),
                                            },
                                    },
                                },
                            },
                        },
                        new Pull
                        {
                            Extent = new Filter(M.C2.ObjectType),
                        },
                    };

                    var result = await context.Load(pull);

                    var c1s = result.GetCollection<C1>("C1s");
                    var c2s = result.GetCollection<C2>("C2s");

                    var c1A = c1s.First((v) => v.Name == "c1A");
                    var c1B = c1s.First((v) => v.Name == "c1B");
                    var c1C = c1s.First((v) => v.Name == "c1C");
                    var c1D = c1s.First((v) => v.Name == "c1D");

                    var c2A = c2s.First((v) => v.Name == "c2A");
                    var c2B = c2s.First((v) => v.Name == "c2B");
                    var c2C = c2s.First((v) => v.Name == "c2C");
                    var c2D = c2s.First((v) => v.Name == "c2D");

                    // One to One
                    Assert.Null(c2A.C1WhereC1C2One2One);
                    Assert.Equal(c2B.C1WhereC1C2One2One, c1B);
                    Assert.Equal(c2C.C1WhereC1C2One2One, c1C);
                    Assert.Equal(c2D.C1WhereC1C2One2One, c1D);

                    // Many to One
                    Assert.Empty(c2A.C1sWhereC1C2Many2One);
                    Assert.Single(c2B.C1sWhereC1C2Many2One);
                    Assert.Contains(c1B, c2B.C1sWhereC1C2Many2One);
                    Assert.Equal(2, c2C.C1sWhereC1C2Many2One.Length);
                    Assert.Contains(c1C, c2C.C1sWhereC1C2Many2One);
                    Assert.Contains(c1D, c2C.C1sWhereC1C2Many2One);
                    Assert.Empty(c2D.C1sWhereC1C2Many2One);
                });
    }
}

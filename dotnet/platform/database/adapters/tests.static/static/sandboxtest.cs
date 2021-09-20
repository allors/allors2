// <copyright file="SandboxTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using System;
    using System.Collections.Generic;
    using Allors;
    using Allors.Data;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Data;
    using Xunit;
    using Extent = Data.Extent;

    public abstract class SandboxTest : IDisposable
    {
        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        protected abstract IProfile Profile { get; }

        protected IDatabase Population => this.Profile.Database;

        public abstract void Dispose();

        [Fact]
        public void SqlReservedWords()
        {
            foreach (var init in this.Inits)
            {
                init();

                var user = User.Create(this.Session);
                user.From = "Nowhere";
                Assert.Equal("Nowhere", user.From);

                this.Session.Commit();

                user = (User)this.Session.Instantiate(user.Id);
                Assert.Equal("Nowhere", user.From);
            }
        }

        [Fact]
        public void MultipleInits()
        {
            foreach (var init in this.Inits)
            {
                init();

                if (this.Population is IDatabase database)
                {
                    for (var i = 0; i < 100; i++)
                    {
                        database.Init();
                    }
                }
            }
        }

        [Fact]
        public void DeleteAssociations()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1A = C1.Create(this.Session);
                var c1B = C1.Create(this.Session);
                c1A.C1C1many2one = c1B;

                foreach (C1 c in c1B.C1sWhereC1C1many2one)
                {
                    c.Strategy.Delete();
                }

                Assert.False(c1B.ExistC1sWhereC1C1many2one);
            }
        }

        [Fact]
        public void EqualsWithParameter()
        {
            foreach (var init in this.Inits)
            {
                init();
                var population = new TestPopulation(this.Session);

                var extent = new Extent(M.C1.ObjectType)
                {
                    Predicate = new Equals(M.C1.C1AllorsString) { Parameter = "pString" },
                };

                var objects = this.Session.Resolve<C1>(extent, new Dictionary<string, string> { { "pString", "ᴀbra" } });

                Assert.Single(objects);
            }
        }

        [Fact]
        public void EqualsMissingParameter()
        {
            foreach (var init in this.Inits)
            {
                init();
                var population = new TestPopulation(this.Session);

                var extent = new Extent(M.C1.ObjectType)
                {
                    Predicate = new Equals(M.C1.C1AllorsString) { Parameter = "pString" },
                };

                var objects = this.Session.Resolve<C1>(extent);

                Assert.Equal(4, objects.Length);
            }
        }

        [Fact]
        public void LoadExtent()
        {
            foreach (var init in this.Inits)
            {
                init();
                var population = new TestPopulation(this.Session);

                var schemaExtent = new Protocol.Data.Extent
                {
                    kind = Protocol.Data.ExtentKind.Extent,
                    objectType = M.C1.ObjectType.Id,
                    predicate = new Predicate
                    {
                        kind = Protocol.Data.PredicateKind.Equals,
                        propertyType = M.C1.C1AllorsString.Id,
                        value = "ᴀbra",
                    },
                };

                var extent = schemaExtent.Load(this.Session);

                var objects = this.Session.Resolve<C1>(extent);

                Assert.Single(objects);
            }
        }

        [Fact]
        public void SaveExtent()
        {
            foreach (var init in this.Inits)
            {
                init();
                var population = new TestPopulation(this.Session);

                var extent = new Extent(M.C1.ObjectType)
                {
                    Predicate = new Equals(M.C1.C1AllorsString) { Parameter = "pString" },
                };

                var schemaExtent = extent.Save();

                Assert.NotNull(schemaExtent);

                Assert.Equal(ExtentKind.Extent, schemaExtent.kind);

                var predicate = schemaExtent.predicate;

                Assert.NotNull(predicate);
                Assert.Equal(PredicateKind.Equals, predicate.kind);
                Assert.Equal("pString", predicate.parameter);
            }
        }

        [Fact]
        public void ScratchPad()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1 = this.Session.Create<C1>();
                var c2a = this.Session.Create<C2>();
                var c2b = this.Session.Create<C2>();

                c1.I1I12one2one = c2a;

                this.Session.Commit();

                c1.I1I12one2one = c2b;

                this.Session.Commit();

                Assert.Null(c2a.I1WhereI1I12one2one);
                Assert.Equal(c1, c2b.I1WhereI1I12one2one);
            }
        }
    }
}

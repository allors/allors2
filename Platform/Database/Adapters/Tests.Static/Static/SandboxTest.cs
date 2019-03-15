// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SandboxTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters
{
    using System;

    using Allors;
    using Allors.Data;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public abstract class SandboxTest : IDisposable
    {
        protected abstract IProfile Profile { get; }

        protected IDatabase Population => this.Profile.Database;

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

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
                var database = this.Population as IDatabase;

                if (database != null)
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

                var extent = new Filter(M.C1.ObjectType)
                {
                    Predicate = new Equals(M.C1.C1AllorsString) { Parameter = "pString" }
                };

                var objects = this.Session.Resolve<C1>(extent, new { pString = "ᴀbra" });

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

                var extent = new Filter(M.C1.ObjectType)
                {
                    Predicate = new Equals(M.C1.C1AllorsString) { Parameter = "pString" }
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
                
                var schemaExtent = new Data.Protocol.Extent
                {
                    Kind = Data.Protocol.ExtentKind.Filter,
                    ObjectType = M.C1.ObjectType.Id,
                    Predicate = new Data.Protocol.Predicate
                    {
                        Kind = Data.Protocol.PredicateKind.Equals,
                        PropertyType = M.C1.C1AllorsString.Id,
                        Value = "ᴀbra"
                    }
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

                var extent = new Filter(M.C1.ObjectType)
                                 {
                                     Predicate = new Equals(M.C1.C1AllorsString) { Parameter = "pString" }
                                 };


                var schemaExtent = extent.Save();

                Assert.NotNull(schemaExtent);

                Assert.Equal(Data.Protocol.ExtentKind.Filter, schemaExtent.Kind);

                var predicate = schemaExtent.Predicate;

                Assert.NotNull(predicate);
                Assert.Equal(Data.Protocol.PredicateKind.Equals, predicate.Kind);
                Assert.Equal("pString", predicate.Parameter);
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

                c1.C1C2one2manies = new[] {c2a, c2b};
                
                this.Session.Commit();

                Assert.Equal(2, c1.C1C2one2manies.Count);

                c1.C1C2one2manies = new[] { c2a };

                this.Session.Commit();

                Assert.Single(c1.C1C2one2manies);           
            }
        }

    }
}
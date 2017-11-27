// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrefetchTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.SqlClient
{
    using System;
    using System.Linq;
    using System.Text;

    using Adapters;

    using Allors;
    using Allors.Adapters.Object.SqlClient.Caching.Debugging;
    using Allors.Adapters.Object.SqlClient.Debug;
    using Allors.Domain;

    using Xunit;
    
    public abstract class DebugTests
    {
        #region Population
        protected C1 c1A;
        protected C1 c1B;
        protected C1 c1C;
        protected C1 c1D;
        protected C2 c2A;
        protected C2 c2B;
        protected C2 c2C;
        protected C2 c2D;
        protected C3 c3A;
        protected C3 c3B;
        protected C3 c3C;
        protected C3 c3D;
        protected C4 c4A;
        protected C4 c4B;
        protected C4 c4C;
        protected C4 c4D;
        #endregion

        protected abstract IProfile Profile { get; }

        protected Database Database => (Database)this.Profile.Database;

        protected DebugConnectionFactory ConnectionFactory => (DebugConnectionFactory)this.Database.ConnectionFactory;

        protected DebugCache Cache => ((DebugCacheFactory)this.Database.CacheFactory).Cache;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        [Fact]
        public void SessionCreation()
        {
            foreach (var init in this.Inits)
            {
                init();

                using (var session = this.Database.CreateSession())
                {
                    var connectionFactory = (DebugConnectionFactory)this.Database.ConnectionFactory;

                    var connection = connectionFactory.Connections.Last();

                    Assert.Equal(connection.Commands.Count, 0);
                }
            }
        }

        [Fact]
        public void Extent()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    connection.Commands.Clear();

                    var extent = session.Extent<C1>();

                    foreach (C1 c1 in extent)
                    {
                        Assert.Equal(c1.Strategy.Class, C1.Meta.ObjectType);
                    }

                    Assert.Equal(connection.Commands.Count, 2);
                    Assert.Equal(connection.Executions.Count(), 2);

                    session.Rollback();
                    connection.Commands.Clear();
                    this.Cache.Invalidate();
                }

                connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();

                    foreach (C1 c1 in extent)
                    {
                        Assert.Equal(c1.Strategy.Class, C1.Meta.ObjectType);
                    }

                    Assert.Equal(connection.Commands.Count, 2);
                    Assert.Equal(connection.Executions.Count(), 2);
                }
            }
        }

        [Fact]
        public void GenericExtent()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                this.Cache.Invalidate();

                var connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    C1[] extent = session.Extent<C1>();

                    foreach (C1 c1 in extent)
                    {
                        Assert.Equal(c1.Strategy.Class, C1.Meta.ObjectType);
                    }

                    Assert.Equal(connection.Commands.Count, 2);
                    Assert.Equal(connection.Executions.Count(), 2);
                }
                
                this.Cache.Invalidate();

                connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();

                    foreach (C1 c1 in extent)
                    {
                        Assert.Equal(c1.Strategy.Class, C1.Meta.ObjectType);
                    }

                    Assert.Equal(connection.Commands.Count, 2);
                    Assert.Equal(connection.Executions.Count(), 2);
                }
            }
        }
        
        [Fact]
        public void ExtentRole()
        {
            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();

                    var stringBuilder = new StringBuilder();
                    foreach (C1 c1 in extent)
                    {
                        stringBuilder.Append(c1.C1AllorsString);
                    }

                    Assert.Equal(connection.Commands.Count, 3);
                    Assert.Equal(connection.Executions.Count(), 6);
                }

                this.Cache.Invalidate();

                connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();

                    var stringBuilder = new StringBuilder();
                    foreach (C1 c1 in extent)
                    {
                        stringBuilder.Append(c1.C1AllorsString);
                    }

                    Assert.Equal(connection.Commands.Count, 3);
                    Assert.Equal(connection.Executions.Count(), 6);
                }
            }
        }

        [Fact]
        public void Prefetch()
        {
            var c1Prefetcher = new PrefetchPolicyBuilder().WithRule(C1.Meta.C1AllorsString).Build();

            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();
                    session.Prefetch(c1Prefetcher, extent);

                    Assert.Equal(connection.Commands.Count, 3);
                    Assert.Equal(connection.Executions.Count(), 3);

                    var stringBuilder = new StringBuilder();
                    foreach (C1 c1 in extent)
                    {
                        stringBuilder.Append(c1.C1AllorsString);
                    }

                    Assert.Equal(connection.Commands.Count, 3);
                    Assert.Equal(connection.Executions.Count(), 3);
                }

                this.Cache.Invalidate();

                connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();
                    session.Prefetch(c1Prefetcher, extent);

                    var stringBuilder = new StringBuilder();
                    foreach (C1 c1 in extent)
                    {
                        stringBuilder.Append(c1.C1AllorsString);
                    }

                    Assert.Equal(connection.Commands.Count, 3);
                    Assert.Equal(connection.Executions.Count(), 3);
                }
            }
        }
        
        [Fact]
        public void PrefetchOneClass()
        {
            var c2Prefetcher = new PrefetchPolicyBuilder().WithRule(C1.Meta.C1AllorsString).Build();
            var c1Prefetcher =
                new PrefetchPolicyBuilder().WithRule(C1.Meta.C1C2one2one, c2Prefetcher)
                    .WithRule(C1.Meta.C1AllorsString)
                    .Build();

            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();
                    session.Prefetch(c1Prefetcher, extent);

                    Assert.Equal(connection.Commands.Count, 5);
                    Assert.Equal(connection.Executions.Count(), 6);

                    var stringBuilder = new StringBuilder();
                    foreach (C1 c1 in extent)
                    {
                        stringBuilder.Append(c1.C1AllorsString);

                        var c2 = c1.C1C2one2one;
                        stringBuilder.Append(c2?.C2AllorsString);
                    }

                    Assert.Equal(connection.Commands.Count, 5);
                    Assert.Equal(connection.Executions.Count(), 6);
                }

                this.Cache.Invalidate();

                connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();
                    session.Prefetch(c1Prefetcher, extent);

                    var stringBuilder = new StringBuilder();
                    foreach (C1 c1 in extent)
                    {
                        stringBuilder.Append(c1.C1AllorsString);

                        var c2 = c1.C1C2one2one;
                        stringBuilder.Append(c2?.C2AllorsString);
                    }

                    Assert.Equal(connection.Commands.Count, 5);
                    Assert.Equal(connection.Executions.Count(), 6);
                }
            }
        }

        [Fact]
        public void PrefetchManyInterface()
        {
            var i12Prefetcher = new PrefetchPolicyBuilder().WithRule(C1.Meta.I12AllorsString.RoleType).Build();

            var c1Prefetcher =
                new PrefetchPolicyBuilder().WithRule(C1.Meta.C1I12one2manies, i12Prefetcher)
                    .WithRule(C1.Meta.C1AllorsString)
                    .Build();

            foreach (var init in this.Inits)
            {
                init();
                this.Populate();

                var connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();
                    session.Prefetch(c1Prefetcher, extent);

                    Assert.Equal(connection.Commands.Count, 5);
                    Assert.Equal(connection.Executions.Count(), 6);

                    var stringBuilder = new StringBuilder();
                    foreach (C1 c1 in extent)
                    {
                        stringBuilder.Append(c1.C1AllorsString);

                        foreach (I12 i12 in c1.C1I12one2manies)
                        {
                            stringBuilder.Append(i12?.I12AllorsString);
                        }
                    }

                    Assert.Equal(connection.Commands.Count, 5);
                    Assert.Equal(connection.Executions.Count(), 6);

                    session.Prefetch(c1Prefetcher, extent);

                    stringBuilder = new StringBuilder();
                    foreach (C1 c1 in extent)
                    {
                        stringBuilder.Append(c1.C1AllorsString);

                        foreach (I12 i12 in c1.C1I12one2manies)
                        {
                            stringBuilder.Append(i12?.I12AllorsString);
                        }
                    }

                    Assert.Equal(connection.Commands.Count, 5);
                    Assert.Equal(connection.Executions.Count(), 6);
                }

                this.Cache.Invalidate();
                
                connection = (DebugConnection)this.ConnectionFactory.Create(this.Database);
                using (var session = this.Database.CreateSession(connection))
                {
                    var extent = session.Extent<C1>();
                    session.Prefetch(c1Prefetcher, extent);

                    var stringBuilder = new StringBuilder();
                    foreach (C1 c1 in extent)
                    {
                        stringBuilder.Append(c1.C1AllorsString);
                        foreach (I12 i12 in c1.C1I12one2manies)
                        {
                            stringBuilder.Append(i12?.I12AllorsString);
                        }
                    }

                    Assert.Equal(connection.Commands.Count, 5);
                    Assert.Equal(connection.Executions.Count(), 6);
                }
            }
        }
        
        protected void Populate()
        {
            using (var session = this.Database.CreateSession())
            {
                var population = new TestPopulation(session);

                this.c1A = population.C1A;
                this.c1B = population.C1B;
                this.c1C = population.C1C;
                this.c1D = population.C1D;

                this.c2A = population.C2A;
                this.c2B = population.C2B;
                this.c2C = population.C2C;
                this.c2D = population.C2D;

                this.c3A = population.C3A;
                this.c3B = population.C3B;
                this.c3C = population.C3C;
                this.c3D = population.C3D;

                this.c4A = population.C4A;
                this.c4B = population.C4B;
                this.c4C = population.C4C;
                this.c4D = population.C4D;

                session.Commit();
            }
        }

        protected ISession CreateSession()
        {
            return this.Profile.Database.CreateSession();
        }
    }
}
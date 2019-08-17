// <copyright file="CacheTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors;
    using Allors.Meta;

    using Allors.Domain;

    using Xunit;

    using IDatabase = Allors.IDatabase;

    public abstract class CacheTest : IDisposable
    {
        public abstract void Dispose();

        [Fact(Skip = "Cache invalidation")]
        public void InitDifferentDatabase()
        {
            var database = this.CreateDatabase();
            database.Init();

            using (var session = database.CreateSession())
            {
                var c1 = C1.Create(session);
                c1.C1AllorsString = "a";
                session.Commit();
            }

            using (var session = database.CreateSession())
            {
                var c1 = session.Extent<C1>().First;
                Assert.Equal("a", c1.C1AllorsString);
            }

            database.Init();

            var database2 = this.CreateDatabase();

            using (var session = database.CreateSession())
            {
                var c1 = C1.Create(session);
                c1.C1AllorsString = "b";
                session.Commit();
            }

            using (var session = database2.CreateSession())
            {
                var c1 = session.Extent<C1>().First;
                c1.C1AllorsString = "c";
            }

            using (var session = database.CreateSession())
            {
                var c1 = session.Extent<C1>().First;
                Assert.Equal("c", c1.C1AllorsString);
            }
        }

        [Fact]
        public void FlushCacheOnInit()
        {
            var database = this.CreateDatabase();
            database.Init();

            using (var session = database.CreateSession())
            {
                var c1a = C1.Create(session);
                var c2a = C2.Create(session);
                c1a.C1C2one2one = c2a;
                session.Commit();

                // load cache
                c2a = c1a.C1C2one2one;
            }

            database.Init();

            using (var session = database.CreateSession())
            {
                var c1a = C1.Create(session);
                var c1b = C1.Create(session);

                session.Commit();

                c1a = C1.Instantiate(session, c1a.Id);

                Assert.Null(c1a.C1C2one2one);
            }
        }

        [Fact]
        public void CacheUnitRole()
        {
            var database = this.CreateDatabase();
            database.Init();

            using (var session = database.CreateSession())
            {
                var c1 = C1.Create(session);
                c1.C1AllorsString = "Test";

                session.Commit();

            }
        }

        [Fact]
        public void FailedCommit()
        {
            var database = this.CreateDatabase();
            database.Init();

            long c1Id = 0;
            long c2Id = 0;

            using (var session = database.CreateSession())
            {
                var c1 = C1.Create(session);
                var c2 = C2.Create(session);

                c1Id = c1.Id;
                c2Id = c2.Id;

                c1.C1C2one2one = c2;

                session.Commit();

                c1.C1AllorsString = "Session 1";

                using (var session2 = database.CreateSession())
                {
                    var session2C1 = (C1)session2.Instantiate(c1);
                    session2C1.C1AllorsString = "Session 2";

                    session2C1.C1C2one2one = null;

                    session2.Commit();

                    var session2C2 = (C2)session2.Instantiate(c2);
                    session2C2.Strategy.Delete();

                    session2.Commit();
                }

                var triggerCache = c1.C1C2one2one;

                var exceptionThrown = false;
                try
                {
                    session.Commit();
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);
            }

            using (var session = database.CreateSession())
            {
                var c1 = (C1)session.Instantiate(c1Id);
                var c2 = session.Instantiate(c2Id);

                Assert.Null(c1.C1C2one2one);
            }
        }

        [Fact]
        public void PrefetchCompositeRole()
        {
            var database = this.CreateDatabase();
            database.Init();

            using (var session = database.CreateSession())
            {
                var c1a = C1.Create(session);
                var c1b = C1.Create(session);
                var c2a = C2.Create(session);
                var c2b = C2.Create(session);

                session.Commit();

                c1a.C1C2many2one = c2a;

                var extent = session.Extent<C1>();
                var array = extent.ToArray();

                var nestedPrefetchPolicyBuilder = new PrefetchPolicyBuilder();
                nestedPrefetchPolicyBuilder.WithRule(M.C2.C2C2one2manies);
                var nestedPrefetchPolicy = nestedPrefetchPolicyBuilder.Build();

                var prefetchPolicyBuilder = new PrefetchPolicyBuilder();
                prefetchPolicyBuilder.WithRule(M.C1.C1C2many2one, nestedPrefetchPolicy);
                var prefetchPolicy = prefetchPolicyBuilder.Build();
                session.Prefetch(prefetchPolicy, new[] { c1a, c1b });

                var result = c1a.C1C2many2one;

                session.Rollback();

                Assert.False(c1a.ExistC1C2many2one);
            }
        }

        [Fact]
        public void PrefetchCompositesRole()
        {
            var database = this.CreateDatabase();
            database.Init();

            using (var session = database.CreateSession())
            {
                var c1a = C1.Create(session);
                var c1b = C1.Create(session);
                var c2a = C2.Create(session);
                var c2b = C2.Create(session);
                var c2c = C2.Create(session);

                c1a.AddC1C2one2many(c2a);
                c1a.AddC1C2one2many(c2b);

                session.Commit();

                c1a.RemoveC1C1one2manies();
                c1a.AddC1C2one2many(c2c);

                var extent = session.Extent<C1>();
                var array = extent.ToArray();

                var nestedPrefetchPolicyBuilder = new PrefetchPolicyBuilder();
                nestedPrefetchPolicyBuilder.WithRule(M.C2.C2C2one2manies);
                var nestedPrefetchPolicy = nestedPrefetchPolicyBuilder.Build();

                var prefetchPolicyBuilder = new PrefetchPolicyBuilder();
                prefetchPolicyBuilder.WithRule(M.C1.C1C2one2manies, nestedPrefetchPolicy);
                var prefetchPolicy = prefetchPolicyBuilder.Build();
                session.Prefetch(prefetchPolicy, new[] { c1a, c1b });

                var result = c1a.C1C2one2manies;

                session.Rollback();

                Assert.Equal(2, c1a.C1C2one2manies.Count);
                Assert.Contains(c2a, c1a.C1C2one2manies.ToArray());
                Assert.Contains(c2b, c1a.C1C2one2manies.ToArray());
            }
        }

        protected abstract IDatabase CreateDatabase();
    }
}

// <copyright file="LifeCycleTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public abstract class LifeCycleTest : IDisposable
    {
        protected static readonly bool[] TrueFalse = { true, false };

        protected abstract IProfile Profile { get; }

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

        public abstract void Dispose();

        [Fact]
        public void NextId()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1 = this.Session.Create<C1>();
                var strategy = c1.Strategy;
                var id = long.Parse(strategy.ObjectId.ToString());

                var nextId = long.Parse(this.Session.Create<C1>().Strategy.ObjectId.ToString());

                Assert.Equal(id + 1, nextId);
            }
        }

        [Fact]
        public void CreateMany()
        {
            foreach (var init in this.Inits)
            {
                init();

                int[] runs = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };

                var total = 0;
                foreach (var run in runs)
                {
                    var allorsObjects = this.Session.Create(MetaC1.Instance.ObjectType, run);

                    Assert.Equal(run, allorsObjects.Length);

                    total += run;

                    Assert.Equal(total, this.GetExtent(MetaC1.Instance.ObjectType).Length);

                    var ids = new ArrayList();
                    foreach (C1 allorsObject in allorsObjects)
                    {
                        Assert.Equal(MetaC1.Instance.ObjectType, allorsObject.Strategy.Class);
                        ids.Add(allorsObject.Strategy.ObjectId);
                        allorsObject.C1AllorsString = "CreateMany";
                    }

                    Assert.Equal(run, ids.Count);

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate((long[])ids.ToArray(typeof(long)));
                    foreach (C1 allorsObject in allorsObjects)
                    {
                        Assert.Equal(MetaC1.Instance.ObjectType, allorsObject.Strategy.Class);
                        allorsObject.C1AllorsString = "CreateMany";
                    }

                    var c2s = (C2[])this.Session.Create(MetaC2.Instance.ObjectType, run);
                    Assert.Equal(run, c2s.Length);
                }
            }
        }

        [Fact]
        public void UnitRole()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1A = C1.Create(this.Session);

                this.Session.Commit();

                c1A.C1AllorsString = "1";

                this.Session.Commit();

                c1A.C1AllorsString = "2";

                this.Session.Rollback();

                Assert.Equal("1", c1A.C1AllorsString);
            }
        }

        [Fact]
        public void CompositeRole()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1A = C1.Create(this.Session);

                var c2A = C2.Create(this.Session);
                var c2B = C2.Create(this.Session);

                this.Session.Commit();

                c1A.C1C2one2one = c2A;

                this.Session.Commit();

                c1A.C1C2one2one = c2B;

                this.Session.Rollback();

                Assert.Equal(c2A, c1A.C1C2one2one);
            }
        }

        [Fact]
        public void CompositeRoles()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1A = C1.Create(this.Session);

                var c2A = C2.Create(this.Session);
                var c2B = C2.Create(this.Session);

                this.Session.Commit();

                c1A.AddC1C2one2many(c2A);

                this.Session.Commit();

                c1A.AddC1C2one2many(c2B);

                this.Session.Rollback();

                Assert.Single(c1A.C1C2one2manies);
                Assert.Equal(c2A, c1A.C1C2one2manies[0]);
            }
        }

        [Fact]
        public void Delete()
        {
            foreach (var init in this.Inits)
            {
                init();

                // Object
                var anObject = C1.Create(this.Session);

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);
                anObject = (C1)this.Session.Instantiate(anObject.Strategy.ObjectId);
                Assert.Null(anObject);

                //// Commit & Rollback

                anObject = C1.Create(this.Session);
                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                var id = anObject.Strategy.ObjectId;
                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                //// Commit + Commit + Commit

                anObject = C1.Create(this.Session);

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                //// Nothing + Commit + Rollback

                anObject = C1.Create(this.Session);

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                //// Commit + Commit + Rollback

                anObject = C1.Create(this.Session);

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                //// Nothing + Rollback + Rollback

                anObject = C1.Create(this.Session);
                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                //// Commit + Rollback + Rollback

                anObject = C1.Create(this.Session);

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.False(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.False(anObject.Strategy.IsDeleted);

                //// Nothing + Rollback + Commit

                anObject = C1.Create(this.Session);

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.Null(anObject);

                //// Commit + Rollback + Commit

                anObject = C1.Create(this.Session);

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.False(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.False(anObject.Strategy.IsDeleted);

                // Clean up
                this.Session.Commit();
                foreach (C1 removeObject in this.GetExtent(MetaC1.Instance.ObjectType))
                {
                    removeObject.Strategy.Delete();
                }

                this.Session.Commit();

                // Strategy
                if (this.Session is ISession databaseSession)
                {
                    var aStrategy = C1.Create(this.Session).Strategy;

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);
                    aStrategy = databaseSession.InstantiateStrategy(aStrategy.ObjectId);
                    Assert.Null(aStrategy);

                    //// Commit & Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;
                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.True(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;
                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    //// Commit + Commit + Commit

                    aStrategy = C1.Create(databaseSession).Strategy;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.True(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    //// Nothing + Commit + Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.True(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    //// Commit + Commit + Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.True(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    //// Nothing + Rollback + Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;
                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.True(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    //// Commit + Rollback + Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.False(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.False(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.False(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.False(aStrategy.IsDeleted);

                    //// Nothing + Rollback + Commit

                    aStrategy = C1.Create(databaseSession).Strategy;

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.True(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.Null(aStrategy);

                    //// Commit + Rollback + Commit

                    aStrategy = C1.Create(databaseSession).Strategy;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.False(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.False(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.True(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.False(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.False(aStrategy.IsDeleted);

                    // Clean up
                    databaseSession.Commit();
                    foreach (C1 removeObject in this.GetExtent(MetaC1.Instance.ObjectType))
                    {
                        removeObject.Strategy.Delete();
                    }

                    databaseSession.Commit();
                }

                //// Units

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";
                anObject.Strategy.Delete();

                StrategyAssert.RoleExistHasException(anObject, MetaC1.Instance.C1AllorsString);

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";
                anObject.Strategy.Delete();

                StrategyAssert.RoleGetHasException(anObject, MetaC1.Instance.C1AllorsString);

                var secondObject = C1.Create(this.Session);
                secondObject.C1AllorsString = "b";
                var thirdObject = C1.Create(this.Session);
                thirdObject.C1AllorsString = "c";

                Assert.Equal(2, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                thirdObject.Strategy.Delete();

                Assert.Single(this.GetExtent(MetaC1.Instance.ObjectType));
                Assert.Equal("b", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                secondObject.Strategy.Delete();

                //// Cached

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";

                AllorsTestUtils.ForceRoleCaching(anObject);

                anObject.Strategy.Delete();

                StrategyAssert.RoleExistHasException(anObject, MetaC1.Instance.C1AllorsString);

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";

                AllorsTestUtils.ForceRoleCaching(anObject);

                anObject.Strategy.Delete();

                StrategyAssert.RoleGetHasException(anObject, MetaC1.Instance.C1AllorsString);

                secondObject = C1.Create(this.Session);
                secondObject.C1AllorsString = "b";
                thirdObject = C1.Create(this.Session);
                thirdObject.C1AllorsString = "c";

                Assert.Equal(2, this.GetExtent(MetaC1.Instance.ObjectType).Length);

                AllorsTestUtils.ForceRoleCaching(secondObject);
                AllorsTestUtils.ForceRoleCaching(thirdObject);

                thirdObject.Strategy.Delete();

                Assert.Single(this.GetExtent(MetaC1.Instance.ObjectType));
                Assert.Equal("b", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                secondObject.Strategy.Delete();

                //// Commit

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";
                anObject.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.RoleExistHasException(anObject, MetaC1.Instance.C1AllorsString);

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";
                anObject.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.RoleGetHasException(anObject, MetaC1.Instance.C1AllorsString);

                secondObject = C1.Create(this.Session);
                secondObject.C1AllorsString = "b";
                thirdObject = C1.Create(this.Session);
                thirdObject.C1AllorsString = "c";

                Assert.Equal(2, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                thirdObject.Strategy.Delete();

                this.Session.Commit();

                Assert.Single(this.GetExtent(MetaC1.Instance.ObjectType));
                Assert.Equal("b", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                secondObject.Strategy.Delete();

                this.Session.Commit();

                //// Cached

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";

                AllorsTestUtils.ForceRoleCaching(anObject);

                anObject.Strategy.Delete();
                this.Session.Commit();

                StrategyAssert.RoleExistHasException(anObject, MetaC1.Instance.C1AllorsString);

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";

                AllorsTestUtils.ForceRoleCaching(anObject);

                anObject.Strategy.Delete();
                this.Session.Commit();

                StrategyAssert.RoleGetHasException(anObject, MetaC1.Instance.C1AllorsString);

                secondObject = C1.Create(this.Session);
                secondObject.C1AllorsString = "b";
                thirdObject = C1.Create(this.Session);
                thirdObject.C1AllorsString = "c";

                Assert.Equal(2, this.GetExtent(MetaC1.Instance.ObjectType).Length);

                AllorsTestUtils.ForceRoleCaching(secondObject);
                AllorsTestUtils.ForceRoleCaching(thirdObject);

                thirdObject.Strategy.Delete();
                this.Session.Commit();

                Assert.Single(this.GetExtent(MetaC1.Instance.ObjectType));
                Assert.Equal("b", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                secondObject.Strategy.Delete();
                this.Session.Commit();

                //// Rollback

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";
                this.Session.Commit();

                anObject.Strategy.Delete();

                this.Session.Rollback();

                Assert.Single(this.GetExtent(MetaC1.Instance.ObjectType));
                Assert.Equal("a", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                secondObject = C1.Create(this.Session);
                secondObject.C1AllorsString = "b";
                thirdObject = C1.Create(this.Session);
                thirdObject.C1AllorsString = "c";

                Assert.Equal(3, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                thirdObject.Strategy.Delete();

                this.Session.Rollback();

                Assert.Single(this.GetExtent(MetaC1.Instance.ObjectType));
                Assert.Equal("a", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                anObject.Strategy.Delete();

                this.Session.Commit();

                //// Cached

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";
                this.Session.Commit();

                AllorsTestUtils.ForceRoleCaching(anObject);

                anObject.Strategy.Delete();
                this.Session.Rollback();

                Assert.Single(this.GetExtent(MetaC1.Instance.ObjectType));
                Assert.Equal("a", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                secondObject = C1.Create(this.Session);
                secondObject.C1AllorsString = "b";
                thirdObject = C1.Create(this.Session);
                thirdObject.C1AllorsString = "c";

                Assert.Equal(3, this.GetExtent(MetaC1.Instance.ObjectType).Length);

                AllorsTestUtils.ForceRoleCaching(secondObject);
                AllorsTestUtils.ForceRoleCaching(thirdObject);

                thirdObject.Strategy.Delete();
                this.Session.Rollback();

                Assert.Single(this.GetExtent(MetaC1.Instance.ObjectType));
                Assert.Equal("a", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                anObject.Strategy.Delete();
                this.Session.Commit();

                //// IComposite

                //// Role

                var fromC1a = C1.Create(this.Session);
                var fromC1b = C1.Create(this.Session);
                var fromC1c = C1.Create(this.Session);
                var fromC1d = C1.Create(this.Session);

                var toC1a = C1.Create(this.Session);
                var toC1b = C1.Create(this.Session);
                var toC1c = C1.Create(this.Session);
                var toC1d = C1.Create(this.Session);

                var toC2a = C2.Create(this.Session);
                var toC2b = C2.Create(this.Session);
                var toC2c = C2.Create(this.Session);
                var toC2d = C2.Create(this.Session);

                //// C1 <-> C1

                fromC1a.C1C1one2one = toC1a;
                fromC1b.C1C1one2one = toC1c;

                fromC1a.C1C1many2one = toC1a;
                fromC1b.C1C1many2one = toC1c;

                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);

                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                toC1a.Strategy.Delete();
                toC1b.Strategy.Delete();

                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2many);

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.True(toC1a.Strategy.IsDeleted);
                Assert.True(toC1b.Strategy.IsDeleted);
                Assert.Null(fromC1a.C1C1one2one);
                Assert.Empty(fromC1a.C1C1one2manies);
                Assert.Null(fromC1a.C1C1many2one);
                Assert.Empty(fromC1a.C1C1many2manies);

                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(toC1c.Strategy.IsDeleted);
                Assert.False(toC1d.Strategy.IsDeleted);
                Assert.Equal(toC1c, fromC1b.C1C1one2one);
                Assert.Equal(2, fromC1b.C1C1one2manies.Count);
                Assert.Equal(toC1c, fromC1b.C1C1many2one);
                Assert.Equal(2, fromC1b.C1C1many2manies.Count);

                this.Session.Commit();

                //// C1 <-> C2

                fromC1a.C1C2one2one = toC2a;
                fromC1b.C1C2one2one = toC2c;

                fromC1a.C1C2many2one = toC2a;
                fromC1b.C1C2many2one = toC2c;

                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);

                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                toC2a.Strategy.Delete();
                toC2b.Strategy.Delete();

                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2many);

                StrategyAssert.AssociationExistHasException(toC1a, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC2.Instance.C1sWhereC1C2many2many);

                StrategyAssert.AssociationExistHasException(toC1b, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC2.Instance.C1sWhereC1C2many2many);

                StrategyAssert.AssociationGetHasException(toC1b, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC2.Instance.C1sWhereC1C2many2many);

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.True(toC2a.Strategy.IsDeleted);
                Assert.True(toC2b.Strategy.IsDeleted);
                Assert.Null(fromC1a.C1C2one2one);
                Assert.Empty(fromC1a.C1C2one2manies);
                Assert.Null(fromC1a.C1C2many2one);
                Assert.Empty(fromC1a.C1C2many2manies);

                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(toC2c.Strategy.IsDeleted);
                Assert.False(toC2d.Strategy.IsDeleted);
                Assert.Equal(toC2c, fromC1b.C1C2one2one);
                Assert.Equal(2, fromC1b.C1C2one2manies.Count);
                Assert.Equal(toC2c, fromC1b.C1C2many2one);
                Assert.Equal(2, fromC1b.C1C2many2manies.Count);

                this.Session.Commit();

                //// Commit

                //// C1 <-> C1

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1b.C1C1one2one = toC1c;

                fromC1a.C1C1many2one = toC1a;
                fromC1b.C1C1many2one = toC1c;

                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);

                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                toC1a.Strategy.Delete();
                toC1b.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2many);

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.True(toC1a.Strategy.IsDeleted);
                Assert.True(toC1b.Strategy.IsDeleted);
                Assert.Null(fromC1a.C1C1one2one);
                Assert.Empty(fromC1a.C1C1one2manies);
                Assert.Null(fromC1a.C1C1many2one);
                Assert.Empty(fromC1a.C1C1many2manies);

                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(toC1c.Strategy.IsDeleted);
                Assert.False(toC1d.Strategy.IsDeleted);
                Assert.Equal(toC1c, fromC1b.C1C1one2one);
                Assert.Equal(2, fromC1b.C1C1one2manies.Count);
                Assert.Equal(toC1c, fromC1b.C1C1many2one);
                Assert.Equal(2, fromC1b.C1C1many2manies.Count);

                //// C1 <-> C2

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1b.C1C2one2one = toC2c;

                fromC1a.C1C2many2one = toC2a;
                fromC1b.C1C2many2one = toC2c;

                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);

                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                toC2a.Strategy.Delete();
                toC2b.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2many);

                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2many);

                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1sWhereC1C2many2many);

                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1sWhereC1C2many2many);

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.True(toC2a.Strategy.IsDeleted);
                Assert.True(toC2b.Strategy.IsDeleted);
                Assert.Null(fromC1a.C1C2one2one);
                Assert.Empty(fromC1a.C1C2one2manies);
                Assert.Null(fromC1a.C1C2many2one);
                Assert.Empty(fromC1a.C1C2many2manies);

                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(toC2c.Strategy.IsDeleted);
                Assert.False(toC2d.Strategy.IsDeleted);
                Assert.Equal(toC2c, fromC1b.C1C2one2one);
                Assert.Equal(2, fromC1b.C1C2one2manies.Count);
                Assert.Equal(toC2c, fromC1b.C1C2many2one);
                Assert.Equal(2, fromC1b.C1C2many2manies.Count);

                //// Rollback

                //// C1 <-> C1

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1b.C1C1one2one = toC1c;

                fromC1a.C1C1many2one = toC1a;
                fromC1b.C1C1many2one = toC1c;

                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);

                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                toC1a.Strategy.Delete();
                toC1b.Strategy.Delete();

                this.Session.Rollback();

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.True(fromC1c.Strategy.IsDeleted);
                Assert.True(fromC1d.Strategy.IsDeleted);

                Assert.True(toC1a.Strategy.IsDeleted);
                Assert.True(toC1b.Strategy.IsDeleted);
                Assert.True(toC1c.Strategy.IsDeleted);
                Assert.True(toC1d.Strategy.IsDeleted);

                // Commit + Rollback
                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1b.C1C1one2one = toC1c;

                fromC1a.C1C1many2one = toC1a;
                fromC1b.C1C1many2one = toC1c;

                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);

                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                this.Session.Commit();

                toC1a.Strategy.Delete();
                toC1b.Strategy.Delete();

                this.Session.Rollback();

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(fromC1c.Strategy.IsDeleted);
                Assert.False(fromC1d.Strategy.IsDeleted);

                Assert.False(toC1a.Strategy.IsDeleted);
                Assert.False(toC1b.Strategy.IsDeleted);
                Assert.False(toC1c.Strategy.IsDeleted);
                Assert.False(toC1d.Strategy.IsDeleted);

                Assert.Equal(fromC1a, toC1a.C1WhereC1C1one2one);
                Assert.Equal(fromC1a, toC1b.C1WhereC1C1one2many);
                Assert.Single(toC1a.C1sWhereC1C1many2one);
                Assert.Single(toC1a.C1sWhereC1C1many2many);
                Assert.Single(toC1b.C1sWhereC1C1many2many);

                //// C1 <-> C2

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1b.C1C2one2one = toC2c;

                fromC1a.C1C2many2one = toC2a;
                fromC1b.C1C2many2one = toC2c;

                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);

                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                toC2a.Strategy.Delete();
                toC2b.Strategy.Delete();

                this.Session.Rollback();

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.True(fromC1c.Strategy.IsDeleted);
                Assert.True(fromC1d.Strategy.IsDeleted);

                Assert.True(toC2a.Strategy.IsDeleted);
                Assert.True(toC2b.Strategy.IsDeleted);
                Assert.True(toC2c.Strategy.IsDeleted);
                Assert.True(toC2d.Strategy.IsDeleted);

                // Commit + Rollback
                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1b.C1C2one2one = toC2c;

                fromC1a.C1C2many2one = toC2a;
                fromC1b.C1C2many2one = toC2c;

                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);

                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                this.Session.Commit();

                toC2a.Strategy.Delete();
                toC2b.Strategy.Delete();

                this.Session.Rollback();

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(fromC1c.Strategy.IsDeleted);
                Assert.False(fromC1d.Strategy.IsDeleted);

                Assert.False(toC2a.Strategy.IsDeleted);
                Assert.False(toC2b.Strategy.IsDeleted);
                Assert.False(toC2c.Strategy.IsDeleted);
                Assert.False(toC2d.Strategy.IsDeleted);

                Assert.Equal(fromC1a, toC2a.C1WhereC1C2one2one);
                Assert.Equal(fromC1a, toC2b.C1WhereC1C2one2many);
                Assert.Single(toC2a.C1sWhereC1C2many2one);
                Assert.Single(toC2a.C1sWhereC1C2many2many);
                Assert.Single(toC2b.C1sWhereC1C2many2many);

                //// Cached

                //// C1 <-> C1

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1b.C1C1one2one = toC1c;

                fromC1a.C1C1many2one = toC1a;
                fromC1b.C1C1many2one = toC1c;

                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);

                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                toC1a.Strategy.Delete();
                toC1b.Strategy.Delete();

                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2many);

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.True(toC1a.Strategy.IsDeleted);
                Assert.True(toC1b.Strategy.IsDeleted);
                Assert.Null(fromC1a.C1C1one2one);
                Assert.Empty(fromC1a.C1C1one2manies);
                Assert.Null(fromC1a.C1C1many2one);
                Assert.Empty(fromC1a.C1C1many2manies);

                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(toC1c.Strategy.IsDeleted);
                Assert.False(toC1d.Strategy.IsDeleted);
                Assert.Equal(toC1c, fromC1b.C1C1one2one);
                Assert.Equal(2, fromC1b.C1C1one2manies.Count);
                Assert.Equal(toC1c, fromC1b.C1C1many2one);
                Assert.Equal(2, fromC1b.C1C1many2manies.Count);

                this.Session.Commit();

                //// Commit

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1b.C1C1one2one = toC1c;

                fromC1a.C1C1many2one = toC1a;
                fromC1b.C1C1many2one = toC1c;

                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);

                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                toC1a.Strategy.Delete();
                toC1b.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationGetHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationExistHasException(toC1a, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationExistHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2many);

                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1WhereC1C1one2many);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2one);
                StrategyAssert.AssociationGetHasException(toC1b, MetaC1.Instance.C1sWhereC1C1many2many);

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.True(toC1a.Strategy.IsDeleted);
                Assert.True(toC1b.Strategy.IsDeleted);
                Assert.Null(fromC1a.C1C1one2one);
                Assert.Empty(fromC1a.C1C1one2manies);
                Assert.Null(fromC1a.C1C1many2one);
                Assert.Empty(fromC1a.C1C1many2manies);

                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(toC1c.Strategy.IsDeleted);
                Assert.False(toC1d.Strategy.IsDeleted);
                Assert.Equal(toC1c, fromC1b.C1C1one2one);
                Assert.Equal(2, fromC1b.C1C1one2manies.Count);
                Assert.Equal(toC1c, fromC1b.C1C1many2one);
                Assert.Equal(2, fromC1b.C1C1many2manies.Count);

                this.Session.Commit();

                //// Rollback

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1b.C1C1one2one = toC1c;

                fromC1a.C1C1many2one = toC1a;
                fromC1b.C1C1many2one = toC1c;

                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);

                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                toC1a.Strategy.Delete();
                toC1b.Strategy.Delete();

                this.Session.Rollback();

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.True(fromC1c.Strategy.IsDeleted);
                Assert.True(fromC1d.Strategy.IsDeleted);

                Assert.True(toC1a.Strategy.IsDeleted);
                Assert.True(toC1b.Strategy.IsDeleted);
                Assert.True(toC1c.Strategy.IsDeleted);
                Assert.True(toC1d.Strategy.IsDeleted);

                //// Commit + Rollback

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1b.C1C1one2one = toC1c;

                fromC1a.C1C1many2one = toC1a;
                fromC1b.C1C1many2one = toC1c;

                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);

                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                this.Session.Commit();

                toC1a.Strategy.Delete();
                toC1b.Strategy.Delete();

                this.Session.Rollback();

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(fromC1c.Strategy.IsDeleted);
                Assert.False(fromC1d.Strategy.IsDeleted);

                Assert.False(toC1a.Strategy.IsDeleted);
                Assert.False(toC1b.Strategy.IsDeleted);
                Assert.False(toC1c.Strategy.IsDeleted);
                Assert.False(toC1d.Strategy.IsDeleted);

                Assert.Equal(fromC1a, toC1a.C1WhereC1C1one2one);
                Assert.Equal(fromC1a, toC1b.C1WhereC1C1one2many);
                Assert.Single(toC1a.C1sWhereC1C1many2one);
                Assert.Single(toC1a.C1sWhereC1C1many2many);
                Assert.Single(toC1b.C1sWhereC1C1many2many);

                //// C1 <-> C2

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1b.C1C2one2one = toC2c;

                fromC1a.C1C2many2one = toC2a;
                fromC1b.C1C2many2one = toC2c;

                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);

                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                toC2a.Strategy.Delete();
                toC2b.Strategy.Delete();

                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);

                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);

                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);

                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.True(toC2a.Strategy.IsDeleted);
                Assert.True(toC2b.Strategy.IsDeleted);
                Assert.Null(fromC1a.C1C2one2one);
                Assert.Empty(fromC1a.C1C2one2manies);
                Assert.Null(fromC1a.C1C2many2one);
                Assert.Empty(fromC1a.C1C2many2manies);

                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(toC2c.Strategy.IsDeleted);
                Assert.False(toC2d.Strategy.IsDeleted);
                Assert.Equal(toC2c, fromC1b.C1C2one2one);
                Assert.Equal(2, fromC1b.C1C2one2manies.Count);
                Assert.Equal(toC2c, fromC1b.C1C2many2one);
                Assert.Equal(2, fromC1b.C1C2many2manies.Count);

                this.Session.Commit();

                //// Commit

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1b.C1C2one2one = toC2c;

                fromC1a.C1C2many2one = toC2a;
                fromC1b.C1C2many2one = toC2c;

                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);

                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                toC2a.Strategy.Delete();
                toC2b.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationGetHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);

                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationExistHasException(toC2a, MetaC2.Instance.C1WhereC1C2one2many);

                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationExistHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);

                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2one);
                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);
                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1sWhereC1C2many2one);
                StrategyAssert.AssociationGetHasException(toC2b, MetaC2.Instance.C1WhereC1C2one2many);

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.True(toC2a.Strategy.IsDeleted);
                Assert.True(toC2b.Strategy.IsDeleted);
                Assert.Null(fromC1a.C1C2one2one);
                Assert.Empty(fromC1a.C1C2one2manies);
                Assert.Null(fromC1a.C1C2many2one);
                Assert.Empty(fromC1a.C1C2many2manies);

                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(toC2c.Strategy.IsDeleted);
                Assert.False(toC2d.Strategy.IsDeleted);
                Assert.Equal(toC2c, fromC1b.C1C2one2one);
                Assert.Equal(2, fromC1b.C1C2one2manies.Count);
                Assert.Equal(toC2c, fromC1b.C1C2many2one);
                Assert.Equal(2, fromC1b.C1C2many2manies.Count);

                this.Session.Commit();

                //// Rollback

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1b.C1C2one2one = toC2c;

                fromC1a.C1C2many2one = toC2a;
                fromC1b.C1C2many2one = toC2c;

                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);

                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                toC2a.Strategy.Delete();
                toC2b.Strategy.Delete();

                this.Session.Rollback();

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.True(fromC1c.Strategy.IsDeleted);
                Assert.True(fromC1d.Strategy.IsDeleted);

                Assert.True(toC2a.Strategy.IsDeleted);
                Assert.True(toC2b.Strategy.IsDeleted);
                Assert.True(toC2c.Strategy.IsDeleted);
                Assert.True(toC2d.Strategy.IsDeleted);

                // Commit + Rollback
                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1b.C1C2one2one = toC2c;

                fromC1a.C1C2many2one = toC2a;
                fromC1b.C1C2many2one = toC2c;

                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);

                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                this.Session.Commit();

                toC2a.Strategy.Delete();
                toC2b.Strategy.Delete();

                this.Session.Rollback();

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(fromC1c.Strategy.IsDeleted);
                Assert.False(fromC1d.Strategy.IsDeleted);

                Assert.False(toC2a.Strategy.IsDeleted);
                Assert.False(toC2b.Strategy.IsDeleted);
                Assert.False(toC2c.Strategy.IsDeleted);
                Assert.False(toC2d.Strategy.IsDeleted);

                Assert.Equal(fromC1a, toC2a.C1WhereC1C2one2one);
                Assert.Equal(fromC1a, toC2b.C1WhereC1C2one2many);
                Assert.Single(toC2a.C1sWhereC1C2many2one);
                Assert.Single(toC2a.C1sWhereC1C2many2many);
                Assert.Single(toC2b.C1sWhereC1C2many2many);

                //// Association

                //// C1 <-> C1

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1a.C1C1many2one = toC1a;
                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);

                fromC1b.C1C1one2one = toC1b;
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);
                fromC1b.C1C1many2one = toC1b;
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                fromC1a.Strategy.Delete();
                fromC1b.Strategy.Delete();

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.False(toC1a.Strategy.IsDeleted);
                Assert.False(toC1b.Strategy.IsDeleted);
                Assert.Null(toC1a.C1WhereC1C1one2one);
                Assert.Null(toC1b.C1WhereC1C1one2many);
                Assert.Empty(toC1a.C1sWhereC1C1many2one);
                Assert.Empty(toC1a.C1sWhereC1C1many2many);
                Assert.Empty(toC1b.C1sWhereC1C1many2many);

                //// C1 <-> C2

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1a.C1C2many2one = toC2a;
                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);

                fromC1b.C1C2one2one = toC2b;
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);
                fromC1b.C1C2many2one = toC2b;
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                fromC1a.Strategy.Delete();
                fromC1b.Strategy.Delete();

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2many2manies);

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2many2manies);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.False(toC2a.Strategy.IsDeleted);
                Assert.False(toC2b.Strategy.IsDeleted);
                Assert.Null(toC2a.C1WhereC1C2one2one);
                Assert.Null(toC2b.C1WhereC1C2one2many);
                Assert.Empty(toC2a.C1sWhereC1C2many2one);
                Assert.Empty(toC2a.C1sWhereC1C2many2many);
                Assert.Empty(toC2b.C1sWhereC1C2many2many);

                //// Commit

                //// C1 <-> C1

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1a.C1C1many2one = toC1a;
                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);

                fromC1b.C1C1one2one = toC1b;
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);
                fromC1b.C1C1many2one = toC1b;
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                fromC1a.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2one);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.False(toC1a.Strategy.IsDeleted);
                Assert.False(toC1b.Strategy.IsDeleted);
                Assert.Null(toC1a.C1WhereC1C1one2one);
                Assert.Null(toC1b.C1WhereC1C1one2many);
                Assert.Empty(toC1a.C1sWhereC1C1many2one);
                Assert.Empty(toC1a.C1sWhereC1C1many2many);
                Assert.Empty(toC1b.C1sWhereC1C1many2many);

                //// C1 <-> C2

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1a.C1C2many2one = toC2a;
                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);

                fromC1b.C1C2one2one = toC2b;
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);
                fromC1b.C1C2many2one = toC2b;
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                fromC1a.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2one);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.False(toC2a.Strategy.IsDeleted);
                Assert.False(toC2b.Strategy.IsDeleted);
                Assert.Null(toC2a.C1WhereC1C2one2one);
                Assert.Null(toC2b.C1WhereC1C2one2many);
                Assert.Empty(toC2a.C1sWhereC1C2many2one);
                Assert.Empty(toC2a.C1sWhereC1C2many2many);
                Assert.Empty(toC2b.C1sWhereC1C2many2many);

                //// Rollback

                //// C1 <-> C1

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1a.C1C1many2one = toC1a;
                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);

                fromC1b.C1C1one2one = toC1b;
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);
                fromC1b.C1C1many2one = toC1b;
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                fromC1a.Strategy.Delete();

                this.Session.Rollback();

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C1many2manies);

                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C1many2manies);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.True(fromC1c.Strategy.IsDeleted);
                Assert.True(fromC1d.Strategy.IsDeleted);
                Assert.True(toC1a.Strategy.IsDeleted);
                Assert.True(toC1b.Strategy.IsDeleted);
                Assert.True(toC1c.Strategy.IsDeleted);
                Assert.True(toC1d.Strategy.IsDeleted);

                // Commit + Rollback
                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1a.C1C1many2one = toC1a;
                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);

                fromC1b.C1C1one2one = toC1b;
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);
                fromC1b.C1C1many2one = toC1b;
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                this.Session.Commit();

                fromC1a.Strategy.Delete();

                this.Session.Rollback();

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(fromC1c.Strategy.IsDeleted);
                Assert.False(fromC1d.Strategy.IsDeleted);
                Assert.False(toC1a.Strategy.IsDeleted);
                Assert.False(toC1b.Strategy.IsDeleted);
                Assert.False(toC1c.Strategy.IsDeleted);
                Assert.False(toC1d.Strategy.IsDeleted);

                Assert.Equal(fromC1a, toC1a.C1WhereC1C1one2one);
                Assert.Equal(fromC1a, toC1b.C1WhereC1C1one2many);
                Assert.Single(toC1a.C1sWhereC1C1many2one);
                Assert.Single(toC1a.C1sWhereC1C1many2many);
                Assert.Single(toC1b.C1sWhereC1C1many2many);

                //// C1 <-> C2

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1a.C1C2many2one = toC2a;
                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);

                fromC1b.C1C2one2one = toC2b;
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);
                fromC1b.C1C2many2one = toC2b;
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                fromC1a.Strategy.Delete();

                this.Session.Rollback();

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2manies);

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2manies);

                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C2one2manies);

                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C2one2manies);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.True(fromC1c.Strategy.IsDeleted);
                Assert.True(fromC1d.Strategy.IsDeleted);
                Assert.True(toC2a.Strategy.IsDeleted);
                Assert.True(toC2b.Strategy.IsDeleted);
                Assert.True(toC2c.Strategy.IsDeleted);
                Assert.True(toC2d.Strategy.IsDeleted);

                // Commit + Rollback
                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1a.C1C2many2one = toC2a;
                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);

                fromC1b.C1C2one2one = toC2b;
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);
                fromC1b.C1C2many2one = toC2b;
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                this.Session.Commit();

                fromC1a.Strategy.Delete();

                this.Session.Rollback();

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(fromC1c.Strategy.IsDeleted);
                Assert.False(fromC1d.Strategy.IsDeleted);
                Assert.False(toC2a.Strategy.IsDeleted);
                Assert.False(toC2b.Strategy.IsDeleted);
                Assert.False(toC2c.Strategy.IsDeleted);
                Assert.False(toC2d.Strategy.IsDeleted);

                Assert.Equal(fromC1a, toC2a.C1WhereC1C2one2one);
                Assert.Equal(fromC1a, toC2b.C1WhereC1C2one2many);
                Assert.Single(toC2a.C1sWhereC1C2many2one);
                Assert.Single(toC2a.C1sWhereC1C2many2many);
                Assert.Single(toC2b.C1sWhereC1C2many2many);

                //// Cached

                //// C1 <-> C1

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1a.C1C1many2one = toC1a;
                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);

                fromC1b.C1C1one2one = toC1b;
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);
                fromC1b.C1C1many2one = toC1b;
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                AllorsTestUtils.ForceAssociationCaching(toC1a);
                AllorsTestUtils.ForceAssociationCaching(toC1a);

                fromC1a.Strategy.Delete();
                fromC1b.Strategy.Delete();

                Assert.Empty(toC1a.C1sWhereC1C1many2one);

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.False(toC1a.Strategy.IsDeleted);
                Assert.False(toC1b.Strategy.IsDeleted);
                Assert.Null(toC1a.C1WhereC1C1one2one);
                Assert.Null(toC1b.C1WhereC1C1one2many);
                Assert.Empty(toC1a.C1sWhereC1C1many2one);
                Assert.Empty(toC1a.C1sWhereC1C1many2many);
                Assert.Empty(toC1b.C1sWhereC1C1many2many);

                //// Commit

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1a.C1C1many2one = toC1a;
                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);

                fromC1b.C1C1one2one = toC1b;
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);
                fromC1b.C1C1many2one = toC1b;
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                fromC1a.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2one);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.False(toC1a.Strategy.IsDeleted);
                Assert.False(toC1b.Strategy.IsDeleted);
                Assert.Null(toC1a.C1WhereC1C1one2one);
                Assert.Null(toC1b.C1WhereC1C1one2many);
                Assert.Empty(toC1a.C1sWhereC1C1many2one);
                Assert.Empty(toC1a.C1sWhereC1C1many2many);
                Assert.Empty(toC1b.C1sWhereC1C1many2many);

                //// Rollback

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1a.C1C1many2one = toC1a;
                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);

                fromC1b.C1C1one2one = toC1b;
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);
                fromC1b.C1C1many2one = toC1b;
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                fromC1a.Strategy.Delete();

                this.Session.Rollback();

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C1many2manies);

                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C1many2manies);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.True(fromC1c.Strategy.IsDeleted);
                Assert.True(fromC1d.Strategy.IsDeleted);
                Assert.True(toC1a.Strategy.IsDeleted);
                Assert.True(toC1b.Strategy.IsDeleted);
                Assert.True(toC1c.Strategy.IsDeleted);
                Assert.True(toC1d.Strategy.IsDeleted);

                //// Commit + Rollback

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC1a = C1.Create(this.Session);
                toC1b = C1.Create(this.Session);
                toC1c = C1.Create(this.Session);
                toC1d = C1.Create(this.Session);

                fromC1a.C1C1one2one = toC1a;
                fromC1a.AddC1C1one2many(toC1a);
                fromC1a.AddC1C1one2many(toC1b);
                fromC1a.C1C1many2one = toC1a;
                fromC1a.AddC1C1many2many(toC1a);
                fromC1a.AddC1C1many2many(toC1b);

                fromC1b.C1C1one2one = toC1b;
                fromC1b.AddC1C1one2many(toC1c);
                fromC1b.AddC1C1one2many(toC1d);
                fromC1b.C1C1many2one = toC1b;
                fromC1b.AddC1C1many2many(toC1c);
                fromC1b.AddC1C1many2many(toC1d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                this.Session.Commit();

                fromC1a.Strategy.Delete();

                this.Session.Rollback();

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(fromC1c.Strategy.IsDeleted);
                Assert.False(fromC1d.Strategy.IsDeleted);
                Assert.False(toC1a.Strategy.IsDeleted);
                Assert.False(toC1b.Strategy.IsDeleted);
                Assert.False(toC1c.Strategy.IsDeleted);
                Assert.False(toC1d.Strategy.IsDeleted);

                Assert.Equal(fromC1a, toC1a.C1WhereC1C1one2one);
                Assert.Equal(fromC1a, toC1b.C1WhereC1C1one2many);
                Assert.Single(toC1a.C1sWhereC1C1many2one);
                Assert.Single(toC1a.C1sWhereC1C1many2many);
                Assert.Single(toC1b.C1sWhereC1C1many2many);

                //// C1 <-> C2

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1a.C1C2many2one = toC2a;
                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);

                fromC1b.C1C2one2one = toC2b;
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);
                fromC1b.C1C2many2one = toC2b;
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                AllorsTestUtils.ForceAssociationCaching(toC2a);
                AllorsTestUtils.ForceAssociationCaching(toC2a);

                fromC1a.Strategy.Delete();
                fromC1b.Strategy.Delete();

                Assert.Empty(toC2a.C1sWhereC1C2many2one);

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2many2manies);

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2many2manies);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.False(toC2a.Strategy.IsDeleted);
                Assert.False(toC2b.Strategy.IsDeleted);
                Assert.Null(toC2a.C1WhereC1C2one2one);
                Assert.Null(toC2b.C1WhereC1C2one2many);
                Assert.Empty(toC2a.C1sWhereC1C2many2one);
                Assert.Empty(toC2a.C1sWhereC1C2many2many);
                Assert.Empty(toC2b.C1sWhereC1C2many2many);

                //// Commit

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1a.C1C2many2one = toC2a;
                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);

                fromC1b.C1C2one2one = toC2b;
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);
                fromC1b.C1C2many2one = toC2b;
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                fromC1a.Strategy.Delete();

                this.Session.Commit();

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2one);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.False(toC2a.Strategy.IsDeleted);
                Assert.False(toC2b.Strategy.IsDeleted);
                Assert.Null(toC2a.C1WhereC1C2one2one);
                Assert.Null(toC2b.C1WhereC1C2one2many);
                Assert.Empty(toC2a.C1sWhereC1C2many2one);
                Assert.Empty(toC2a.C1sWhereC1C2many2many);
                Assert.Empty(toC2b.C1sWhereC1C2many2many);

                //// Rollback

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1a.C1C2many2one = toC2a;
                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);

                fromC1b.C1C2one2one = toC2b;
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);
                fromC1b.C1C2many2one = toC2b;
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                fromC1a.Strategy.Delete();

                this.Session.Rollback();

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2many2manies);

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2many2manies);

                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleExistHasException(fromC1b, MetaC1.Instance.C1C2many2manies);

                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleGetHasException(fromC1b, MetaC1.Instance.C1C2many2manies);

                Assert.True(fromC1a.Strategy.IsDeleted);
                Assert.True(fromC1b.Strategy.IsDeleted);
                Assert.True(fromC1c.Strategy.IsDeleted);
                Assert.True(fromC1d.Strategy.IsDeleted);
                Assert.True(toC2a.Strategy.IsDeleted);
                Assert.True(toC2b.Strategy.IsDeleted);
                Assert.True(toC2c.Strategy.IsDeleted);
                Assert.True(toC2d.Strategy.IsDeleted);

                //// Commit + Rollback

                fromC1a = C1.Create(this.Session);
                fromC1b = C1.Create(this.Session);
                fromC1c = C1.Create(this.Session);
                fromC1d = C1.Create(this.Session);

                toC2a = C2.Create(this.Session);
                toC2b = C2.Create(this.Session);
                toC2c = C2.Create(this.Session);
                toC2d = C2.Create(this.Session);

                fromC1a.C1C2one2one = toC2a;
                fromC1a.AddC1C2one2many(toC2a);
                fromC1a.AddC1C2one2many(toC2b);
                fromC1a.C1C2many2one = toC2a;
                fromC1a.AddC1C2many2many(toC2a);
                fromC1a.AddC1C2many2many(toC2b);

                fromC1b.C1C2one2one = toC2b;
                fromC1b.AddC1C2one2many(toC2c);
                fromC1b.AddC1C2one2many(toC2d);
                fromC1b.C1C2many2one = toC2b;
                fromC1b.AddC1C2many2many(toC2c);
                fromC1b.AddC1C2many2many(toC2d);

                AllorsTestUtils.ForceRoleCaching(fromC1a);
                AllorsTestUtils.ForceRoleCaching(fromC1b);

                this.Session.Commit();

                fromC1a.Strategy.Delete();

                this.Session.Rollback();

                Assert.False(fromC1a.Strategy.IsDeleted);
                Assert.False(fromC1b.Strategy.IsDeleted);
                Assert.False(fromC1c.Strategy.IsDeleted);
                Assert.False(fromC1d.Strategy.IsDeleted);
                Assert.False(toC2a.Strategy.IsDeleted);
                Assert.False(toC2b.Strategy.IsDeleted);
                Assert.False(toC2c.Strategy.IsDeleted);
                Assert.False(toC2d.Strategy.IsDeleted);

                Assert.Equal(fromC1a, toC2a.C1WhereC1C2one2one);
                Assert.Equal(fromC1a, toC2b.C1WhereC1C2one2many);
                Assert.Single(toC2a.C1sWhereC1C2many2one);
                Assert.Single(toC2a.C1sWhereC1C2many2many);
                Assert.Single(toC2b.C1sWhereC1C2many2many);

                //// Assignment

                anObject = C1.Create(this.Session);
                var c1Removed = C1.Create(this.Session);
                c1Removed.Strategy.Delete();
                C1[] c1RemovedArray = { c1Removed };

                var error = false;
                try
                {
                    anObject.C1C1one2one = c1Removed;
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);

                error = false;
                try
                {
                    anObject.AddC1C1one2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);
                Assert.Empty(anObject.C1C1one2manies);

                error = false;
                try
                {
                    anObject.C1C1one2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);
                Assert.Empty(anObject.C1C1one2manies);

                error = false;
                try
                {
                    anObject.AddC1C1many2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);
                Assert.Empty(anObject.C1C1many2manies);

                error = false;
                try
                {
                    anObject.C1C1many2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);
                Assert.Empty(anObject.C1C1many2manies);

                //// Commit

                anObject = C1.Create(this.Session);
                c1Removed = C1.Create(this.Session);
                c1Removed.Strategy.Delete();
                c1RemovedArray = new C1[1];
                c1RemovedArray[0] = c1Removed;

                this.Session.Commit();

                error = false;
                try
                {
                    anObject.C1C1one2one = c1Removed;
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);

                error = false;
                try
                {
                    anObject.AddC1C1one2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);
                Assert.Empty(anObject.C1C1one2manies);

                error = false;
                try
                {
                    anObject.C1C1one2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);
                Assert.Empty(anObject.C1C1one2manies);

                error = false;
                try
                {
                    anObject.AddC1C1many2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);
                Assert.Empty(anObject.C1C1many2manies);

                error = false;
                try
                {
                    anObject.C1C1many2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);
                Assert.Empty(anObject.C1C1many2manies);

                //// Rollback

                anObject = C1.Create(this.Session);
                c1Removed = C1.Create(this.Session);
                c1RemovedArray = new C1[1];
                c1RemovedArray[0] = c1Removed;

                c1Removed.Strategy.Delete();

                this.Session.Rollback();

                error = false;
                try
                {
                    anObject.C1C1one2one = c1Removed;
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);

                error = false;
                try
                {
                    anObject.AddC1C1one2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);

                error = false;
                try
                {
                    anObject.C1C1one2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);

                error = false;
                try
                {
                    anObject.AddC1C1many2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);

                error = false;
                try
                {
                    anObject.C1C1many2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.True(error);

                // Commit + Rollback
                anObject = C1.Create(this.Session);
                c1Removed = C1.Create(this.Session);
                c1RemovedArray = new C1[1];
                c1RemovedArray[0] = c1Removed;

                this.Session.Commit();

                c1Removed.Strategy.Delete();

                this.Session.Rollback();

                anObject.C1C1one2one = c1Removed;
                Assert.Equal(c1Removed, anObject.C1C1one2one);

                anObject.AddC1C1one2many(c1Removed);
                Assert.Single(anObject.C1C1one2manies);

                anObject.C1C1one2manies = c1RemovedArray;
                Assert.Single(anObject.C1C1one2manies);

                anObject.AddC1C1many2many(c1Removed);
                Assert.Single(anObject.C1C1many2manies);

                anObject.C1C1many2manies = c1RemovedArray;
                Assert.Single(anObject.C1C1many2manies);

                //// Proxy

                var proxy = C1.Create(this.Session);
                id = proxy.Strategy.ObjectId;
                this.Session.Commit();

                var subject = C1.Instantiate(this.Session, id);
                subject.Strategy.Delete();
                StrategyAssert.RoleExistHasException(proxy, MetaC1.Instance.C1AllorsString);

                this.Session.Commit();

                proxy = C1.Create(this.Session);
                id = proxy.Strategy.ObjectId;
                this.Session.Commit();

                subject = C1.Instantiate(this.Session, id);
                subject.Strategy.Delete();
                StrategyAssert.RoleGetHasException(proxy, MetaC1.Instance.C1AllorsString);

                this.Session.Commit();

                //// Commit

                proxy = C1.Create(this.Session);
                id = proxy.Strategy.ObjectId;
                this.Session.Commit();

                subject = C1.Instantiate(this.Session, id);
                subject.Strategy.Delete();
                this.Session.Commit();

                subject = C1.Instantiate(this.Session, id);
                StrategyAssert.RoleExistHasException(proxy, MetaC1.Instance.C1AllorsString);

                this.Session.Commit();

                proxy = C1.Create(this.Session);
                id = proxy.Strategy.ObjectId;
                this.Session.Commit();

                subject = C1.Instantiate(this.Session, id);
                subject.Strategy.Delete();
                this.Session.Commit();

                subject = C1.Instantiate(this.Session, id);
                StrategyAssert.RoleGetHasException(proxy, MetaC1.Instance.C1AllorsString);

                this.Session.Commit();

                //// Rollback

                proxy = C1.Create(this.Session);
                id = proxy.Strategy.ObjectId;
                this.Session.Commit();

                subject = C1.Instantiate(this.Session, id);
                subject.Strategy.Delete();
                this.Session.Rollback();

                subject = C1.Instantiate(this.Session, id);
                Assert.False(proxy.Strategy.IsDeleted);

                this.Session.Commit();

                //// unit roles

                anObject = C1.Create(this.Session);
                var anotherObject = C1.Create(this.Session);
                anotherObject.C1AllorsString = "value";
                anObject.Strategy.Delete();
                Assert.Equal("value", anotherObject.C1AllorsString);

                this.Session.Commit();

                Assert.Equal("value", anotherObject.C1AllorsString);

                anObject = C1.Create(this.Session);
                anotherObject = C1.Create(this.Session);
                anotherObject.C1AllorsString = "value";
                anObject.Strategy.Delete();

                this.Session.Commit();

                Assert.Equal("value", anotherObject.C1AllorsString);
            }
        }

        [Fact]
        public virtual void DifferentSessions()
        {
            foreach (var init in this.Inits)
            {
                init();

                var secondSession = this.CreateSession();

                try
                {
                    var c1a = C1.Create(this.Session);
                    var c1b = C1.Create(this.Session);

                    var c2a = C2.Create(secondSession);
                    var c2b = C2.Create(secondSession);
                    C2[] c2Array = { c2a, c2b };

                    this.Session.Commit();
                    secondSession.Commit();

                    var exceptionThrown = false;
                    try
                    {
                        c1a.C1C2one2one = c2a;
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.C1C2many2one = c2a;
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.AddC1C2one2many(c2a);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.C1C2one2manies = c2Array;
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.AddC1C2many2many(c2a);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.C1C2many2manies = c2Array;
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.True(exceptionThrown);
                }
                finally
                {
                    secondSession.Commit();
                }
            }
        }

        [Fact]
        public void Identity()
        {
            foreach (var init in this.Inits)
            {
                init();

                var anObject = C1.Create(this.Session);
                var id = anObject.Strategy.ObjectId;
                var proxy = C1.Instantiate(this.Session, id);

                var anotherObject = C1.Create(this.Session);
                var anotherId = anotherObject.Strategy.ObjectId;
                var anotherProxy = C1.Instantiate(this.Session, anotherId);

                Assert.Equal(anObject, proxy);
                Assert.Equal(anotherObject, anotherProxy);
                Assert.NotEqual(anObject, anotherObject);
                Assert.NotEqual(anObject, anotherProxy);
                Assert.NotEqual(proxy, anotherObject);
                Assert.NotEqual(proxy, anotherProxy);

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                anotherObject = C1.Create(this.Session);
                anotherId = anotherObject.Strategy.ObjectId;
                anotherProxy = C1.Instantiate(this.Session, anotherId);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                anotherObject = C1.Instantiate(this.Session, anotherId);

                Assert.Equal(anObject, proxy);
                Assert.Equal(anotherObject, anotherProxy);
                Assert.NotEqual(anObject, anotherObject);
                Assert.NotEqual(anObject, anotherProxy);
                Assert.NotEqual(proxy, anotherObject);
                Assert.NotEqual(proxy, anotherProxy);

                //// Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                anotherObject = C1.Create(this.Session);
                anotherId = anotherObject.Strategy.ObjectId;
                anotherProxy = C1.Instantiate(this.Session, anotherId);

                this.Session.Rollback();

                Assert.Equal(anObject, proxy);
                Assert.Equal(anotherObject, anotherProxy);
                Assert.NotEqual(anObject, anotherObject);
                Assert.NotEqual(anObject, anotherProxy);
                Assert.NotEqual(proxy, anotherObject);
                Assert.NotEqual(proxy, anotherProxy);
            }
        }

        [Fact]
        public void Instantiate()
        {
            foreach (var init in this.Inits)
            {
                init();

                var anObject = C1.Create(this.Session);
                var id = anObject.Strategy.ObjectId;
                var sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                //// Proxy

                //// Unit

                var subject = C1.Create(this.Session);
                id = subject.Strategy.ObjectId;
                this.Session.Commit();

                subject.C1AllorsString = "a";
                var proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                Assert.Equal("b", subject.C1AllorsString);
                Assert.Equal("b", proxy.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                Assert.Equal("d", proxy.C1AllorsString);
                Assert.Equal("d", subject.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                this.Session.Commit();
                Assert.Equal("b", subject.C1AllorsString);
                Assert.Equal("b", proxy.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                this.Session.Commit();
                Assert.Equal("d", proxy.C1AllorsString);
                Assert.Equal("d", subject.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                Assert.Equal("b", subject.C1AllorsString);
                this.Session.Commit();
                Assert.Equal("b", proxy.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                Assert.Equal("d", proxy.C1AllorsString);
                this.Session.Commit();
                Assert.Equal("d", subject.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                this.Session.Commit();
                Assert.Equal("b", subject.C1AllorsString);
                this.Session.Commit();
                Assert.Equal("b", proxy.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                this.Session.Commit();
                Assert.Equal("d", proxy.C1AllorsString);
                this.Session.Commit();
                Assert.Equal("d", subject.C1AllorsString);
                this.Session.Commit();

                //// IComposite

                var fromProxy = C1.Create(this.Session);
                var toProxy = C1.Create(this.Session);
                var fromId = fromProxy.Strategy.ObjectId;
                var toId = toProxy.Strategy.ObjectId;
                this.Session.Commit();

                var from = C1.Instantiate(this.Session, fromId);
                var to = C1.Instantiate(this.Session, toId);
                from.C1AllorsString = "a";
                from.C1C1one2one = to;
                from.C1C1many2one = to;
                from.AddC1C1one2many(to);
                from.AddC1C1many2many(to);

                StrategyAssert.RolesExistExclusive(
                    fromProxy,
                    MetaC1.Instance.C1AllorsString,
                    MetaC1.Instance.C1C1one2one,
                    MetaC1.Instance.C1C1many2one,
                    MetaC1.Instance.C1C1one2manies,
                    MetaC1.Instance.C1C1many2manies);

                StrategyAssert.AssociationsExistExclusive(
                    toProxy,
                    MetaC1.Instance.C1WhereC1C1one2one,
                    MetaC1.Instance.C1sWhereC1C1many2one,
                    MetaC1.Instance.C1WhereC1C1one2many,
                    MetaC1.Instance.C1sWhereC1C1many2many);

                Assert.Equal("a", fromProxy.C1AllorsString);
                Assert.Equal(toProxy, fromProxy.C1C1one2one);
                Assert.Equal(toProxy, fromProxy.C1C1many2one);
                Assert.Contains(toProxy, (C1[])fromProxy.C1C1one2manies);
                Assert.Contains(toProxy, (C1[])fromProxy.C1C1many2manies);

                Assert.Equal(fromProxy, toProxy.C1WhereC1C1one2one);
                Assert.Contains(fromProxy, (C1[])toProxy.C1sWhereC1C1many2one);
                Assert.Equal(fromProxy, toProxy.C1WhereC1C1one2many);
                Assert.Contains(fromProxy, (C1[])toProxy.C1sWhereC1C1many2many);

                from.C1AllorsString = null;
                from.C1C1one2one = null;
                from.C1C1many2one = null;
                from.C1C1one2manies = null;
                from.C1C1many2manies = null;

                this.Session.Commit();

                Assert.Null(from.C1AllorsString);
                Assert.Null(from.C1C1one2one);
                Assert.Null(from.C1C1many2one);
                Assert.Empty((C1[])from.C1C1one2manies);
                Assert.Empty((C1[])from.C1C1many2manies);

                StrategyAssert.RolesExistExclusive(from);
                StrategyAssert.AssociationsExistExclusive(to);

                Assert.Null(fromProxy.C1AllorsString);
                Assert.Null(fromProxy.C1C1one2one);
                Assert.Null(fromProxy.C1C1many2one);
                Assert.Empty(fromProxy.C1C1one2manies);
                Assert.Empty(fromProxy.C1C1many2manies);

                StrategyAssert.RolesExistExclusive(fromProxy);
                StrategyAssert.AssociationsExistExclusive(toProxy);

                this.Session.Commit();

                //// Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));
                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Rollback();

                sameObject = (C1)this.Session.Instantiate(id);
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Rollback();

                sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Rollback();

                sameObject = (C1)this.Session.Instantiate(id);
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.True(anObject.Equals(sameObject));
                Assert.True(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                //// Proxy

                //// Unit

                subject = C1.Create(this.Session);
                id = subject.Strategy.ObjectId;
                this.Session.Commit();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                Assert.Equal("b", subject.C1AllorsString);
                Assert.Equal("b", proxy.C1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                Assert.Equal("d", proxy.C1AllorsString);
                Assert.Equal("d", subject.C1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                this.Session.Rollback();
                Assert.False(subject.ExistC1AllorsString);
                Assert.False(proxy.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                this.Session.Rollback();
                Assert.False(proxy.ExistC1AllorsString);
                Assert.False(subject.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                Assert.Equal("b", subject.C1AllorsString);
                this.Session.Rollback();
                Assert.False(proxy.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                Assert.Equal("d", proxy.C1AllorsString);
                this.Session.Rollback();
                Assert.False(subject.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                this.Session.Rollback();
                Assert.False(subject.ExistC1AllorsString);
                this.Session.Rollback();
                Assert.False(proxy.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                this.Session.Rollback();
                Assert.False(proxy.ExistC1AllorsString);
                this.Session.Rollback();
                Assert.False(subject.ExistC1AllorsString);
                this.Session.Rollback();

                //// IComposite

                from = C1.Instantiate(this.Session, fromId);
                to = C1.Instantiate(this.Session, toId);
                from.C1AllorsString = "a";
                from.C1C1one2one = to;
                from.C1C1many2one = to;
                from.AddC1C1one2many(to);
                from.AddC1C1many2many(to);

                StrategyAssert.RolesExistExclusive(
                    fromProxy,
                    MetaC1.Instance.C1AllorsString,
                    MetaC1.Instance.C1C1one2one,
                    MetaC1.Instance.C1C1many2one,
                    MetaC1.Instance.C1C1one2manies,
                    MetaC1.Instance.C1C1many2manies);

                StrategyAssert.AssociationsExistExclusive(
                    toProxy,
                    MetaC1.Instance.C1WhereC1C1one2one,
                    MetaC1.Instance.C1sWhereC1C1many2one,
                    MetaC1.Instance.C1WhereC1C1one2many,
                    MetaC1.Instance.C1sWhereC1C1many2many);

                Assert.Equal("a", fromProxy.C1AllorsString);
                Assert.Equal(toProxy, fromProxy.C1C1one2one);
                Assert.Equal(toProxy, fromProxy.C1C1many2one);
                Assert.Contains(toProxy, (C1[])fromProxy.C1C1one2manies);
                Assert.Contains(toProxy, (C1[])fromProxy.C1C1many2manies);

                Assert.Equal(fromProxy, toProxy.C1WhereC1C1one2one);
                Assert.Contains(fromProxy, (C1[])toProxy.C1sWhereC1C1many2one);
                Assert.Equal(fromProxy, toProxy.C1WhereC1C1one2many);
                Assert.Contains(fromProxy, (C1[])toProxy.C1sWhereC1C1many2many);

                this.Session.Rollback();

                Assert.Null(from.C1AllorsString);
                Assert.Null(from.C1C1one2one);
                Assert.Null(from.C1C1many2one);
                Assert.Empty((C1[])from.C1C1one2manies);
                Assert.Empty((C1[])from.C1C1many2manies);

                StrategyAssert.RolesExistExclusive(from);
                StrategyAssert.AssociationsExistExclusive(to);

                Assert.Null(fromProxy.C1AllorsString);
                Assert.Null(fromProxy.C1C1one2one);
                Assert.Null(fromProxy.C1C1many2one);
                Assert.Empty(fromProxy.C1C1one2manies);
                Assert.Empty(fromProxy.C1C1many2manies);

                StrategyAssert.RolesExistExclusive(fromProxy);
                StrategyAssert.AssociationsExistExclusive(toProxy);

                this.Session.Rollback();

                var unexistingObject = (C1)this.Session.Instantiate("1000000");
                Assert.Null(unexistingObject);
            }
        }

        [Fact]
        public void InstantiateMany()
        {
            foreach (var init in this.Inits)
            {
                init();

                int[] runs = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };

                foreach (var run in runs)
                {
                    // Empty arrays
                    long[] nullObjectIdArray = null;
                    var allorsObjects = this.Session.Instantiate(nullObjectIdArray);
                    Assert.Empty(allorsObjects);

                    string[] nullStringArray = null;
                    allorsObjects = this.Session.Instantiate(nullStringArray);
                    Assert.Empty(allorsObjects);

                    IObject[] nullObjectArray = null;
                    allorsObjects = this.Session.Instantiate(nullObjectArray);
                    Assert.Empty(allorsObjects);

                    var objects = new IObject[run];
                    var idStrings = new string[run];
                    var ids = new long[run];
                    for (var i = 0; i < run; i++)
                    {
                        var anObject = C1.Create(this.Session);
                        objects[i] = anObject;
                        idStrings[i] = anObject.Strategy.ObjectId.ToString();
                        ids[i] = anObject.Strategy.ObjectId;
                    }

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate(objects);

                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Equal(allorsObjects[i].Id, ids[i]);
                    }

                    allorsObjects = this.Session.Instantiate(idStrings);

                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Equal(allorsObjects[i].Id, ids[i]);
                    }

                    allorsObjects = this.Session.Instantiate(ids);

                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Equal(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate(objects);

                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Equal(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate(idStrings);

                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Equal(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate(ids);

                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Equal(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Rollback();

                    allorsObjects = this.Session.Instantiate(objects);

                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Equal(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Rollback();

                    allorsObjects = this.Session.Instantiate(idStrings);

                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Equal(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Rollback();

                    allorsObjects = this.Session.Instantiate(ids);

                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Equal(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Commit();

                    // Caching in Sql
                    this.SwitchDatabase();
                    var minusOne = new List<long>(ids);
                    minusOne.RemoveAt(0);
                    allorsObjects = this.Session.Instantiate(minusOne.ToArray());

                    allorsObjects = this.Session.Instantiate(ids);
                    Assert.Equal(run, allorsObjects.Length);
                    for (var i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.Contains(allorsObjects[i].Id, ids);
                    }

                    this.Session.Commit();

                    Assert.Empty(this.Session.Instantiate(new IObject[0]));

                    this.Session.Commit();

                    Assert.Empty(this.Session.Instantiate(new string[0]));

                    this.Session.Commit();

                    Assert.Empty(this.Session.Instantiate(new long[0]));

                    this.Session.Commit();

                    var doesntExistIds = new[] { (1000 * 1000 * 1000).ToString() };

                    Assert.Empty(this.Session.Instantiate(doesntExistIds));

                    // Preserve order
                    var c1A = C1.Create(this.Session);
                    var c1B = C1.Create(this.Session);
                    var c1C = C1.Create(this.Session);
                    var c1D = C1.Create(this.Session);

                    var objectIds = new[] { c1A.Id, c1B.Id, c1C.Id, c1D.Id };

                    var instantiatedObjects = this.Session.Instantiate(objectIds);

                    Assert.Equal(4, instantiatedObjects.Length);
                    Assert.Equal(c1A, instantiatedObjects[0]);
                    Assert.Equal(c1B, instantiatedObjects[1]);
                    Assert.Equal(c1C, instantiatedObjects[2]);
                    Assert.Equal(c1D, instantiatedObjects[3]);

                    this.Session.Commit();

                    using (var session2 = this.CreateSession())
                    {
                        c1C = (C1)session2.Instantiate(objectIds[2]);

                        instantiatedObjects = session2.Instantiate(objectIds);

                        Assert.Equal(4, instantiatedObjects.Length);
                        Assert.Equal(c1A, instantiatedObjects[0]);
                        Assert.Equal(c1B, instantiatedObjects[1]);
                        Assert.Equal(c1C, instantiatedObjects[2]);
                        Assert.Equal(c1D, instantiatedObjects[3]);
                    }
                }
            }
        }

        [Fact]
        public void IsDeleted()
        {
            foreach (var init in this.Inits)
            {
                init();

                //// Commit + Commit

                var anObject = C1.Create(this.Session);
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.False(anObject.Strategy.IsDeleted);

                //// Commit + Rollback

                anObject = C1.Create(this.Session);
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.False(anObject.Strategy.IsDeleted);

                //// Rollback + Commit

                anObject = C1.Create(this.Session);
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                //// Rollback + Rollback

                anObject = C1.Create(this.Session);
                Assert.False(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                //// With Delete

                //// Commit + Commit

                anObject = C1.Create(this.Session);
                Assert.False(anObject.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                //// Commit + Rollback

                anObject = C1.Create(this.Session);
                Assert.False(anObject.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                //// Rollback + Commit

                anObject = C1.Create(this.Session);
                Assert.False(anObject.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.True(anObject.Strategy.IsDeleted);

                //// Rollback + Rollback

                anObject = C1.Create(this.Session);
                Assert.False(anObject.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(anObject.Strategy.IsDeleted);

                //// Proxy

                //// Without Delete

                //// Commit + Commit

                anObject = C1.Create(this.Session);
                var id = anObject.Strategy.ObjectId;
                var proxy = C1.Instantiate(this.Session, id);
                Assert.False(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.False(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.False(proxy.Strategy.IsDeleted);

                //// Commit + Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);
                Assert.False(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.False(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.False(proxy.Strategy.IsDeleted);

                //// Rollback + Commit

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);
                Assert.False(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                //// Rollback + Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);
                Assert.False(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                //// With Delete

                //// Commit + Commit

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                Assert.False(proxy.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                //// Commit + Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                Assert.False(proxy.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.True(proxy.Strategy.IsDeleted);

                //// Rollback + Commit

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                Assert.False(proxy.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                //// Rollback + Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                Assert.False(proxy.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.True(proxy.Strategy.IsDeleted);
            }
        }

        [Fact]
        public void OnChange()
        {
            // TODO:
        }

        [Fact]
        public void Rollback()
        {
            foreach (var init in this.Inits)
            {
                init();

                const int ObjectCount = 10;
                var allorsObjects = this.Session.Create(MetaCompany.Instance.ObjectType, ObjectCount);
                var ids = new string[ObjectCount];
                for (var i = 0; i < ObjectCount; i++)
                {
                    var allorsObject = allorsObjects[i];
                    ids[i] = allorsObject.Strategy.ObjectId.ToString();
                }

                Assert.Equal(ObjectCount, allorsObjects.Length);

                this.Session.Rollback();

                allorsObjects = this.Session.Instantiate(ids);

                Assert.Empty(allorsObjects);
            }
        }

        [Fact]
        public void Versioning()
        {
            foreach (var init in this.Inits)
            {
                init();

                var obj = this.Session.Create<C1>();

                Assert.Equal(0, obj.Strategy.ObjectVersion);

                this.Session.Commit();

                using (var session2 = this.CreateSession())
                {
                    Assert.Equal(0, session2.Instantiate(obj).Strategy.ObjectVersion);
                }

                Assert.Equal(0, obj.Strategy.ObjectVersion);

                obj.C1AllorsString = "Changed";

                Assert.Equal(0, obj.Strategy.ObjectVersion);

                this.Session.Commit();

                using (var session2 = this.CreateSession())
                {
                    var session2Object = (C1)session2.Instantiate(obj);
                    Assert.Equal(1, session2Object.Strategy.ObjectVersion);
                    session2Object.C1AllorsString = "Session 2 changed";
                    session2.Commit();

                    Assert.Equal(2, session2Object.Strategy.ObjectVersion);
                }

                this.Session.Rollback();

                Assert.Equal(2, obj.Strategy.ObjectVersion);

                obj.C1AllorsString = "Changed again.";

                Assert.Equal(2, obj.Strategy.ObjectVersion);

                this.Session.Commit();

                using (var session2 = this.CreateSession())
                {
                    Assert.Equal(3, session2.Instantiate(obj).Strategy.ObjectVersion);
                }

                Assert.Equal(3, obj.Strategy.ObjectVersion);

                obj.RemoveC1AllorsString();

                Assert.Equal(3, obj.Strategy.ObjectVersion);

                this.Session.Commit();

                using (var session2 = this.CreateSession())
                {
                    Assert.Equal(4, session2.Instantiate(obj).Strategy.ObjectVersion);
                }

                Assert.Equal(4, obj.Strategy.ObjectVersion);
            }
        }

        [Fact]
        public void WihtoutRoles()
        {
            foreach (var init in this.Inits)
            {
                init();

                // TODO: Move to other tests
                var withoutValueRoles = ClassWithoutUnitRoles.Create(this.Session);
                var withoutValueRolesClone = (ClassWithoutUnitRoles)this.GetExtent(MetaClassWithoutUnitRoles.Instance.ObjectType)[0];

                Assert.Equal(withoutValueRoles, withoutValueRolesClone);

                var withoutRoles = ClassWithoutRoles.Create(this.Session);
                var withoutRolesClone = (ClassWithoutRoles)this.GetExtent(MetaClassWithoutRoles.Instance.ObjectType)[0];

                Assert.Equal(withoutRoles, withoutRolesClone);
            }
        }

        [Fact]
        public void SwitchPopulation()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1A = C1.Create(this.Session);
                var c2A = C2.Create(this.Session);
                var c2B = C2.Create(this.Session);
                var c2C = C2.Create(this.Session);

                var c1aId = c1A.Id.ToString();

                c1A.C1I12one2one = c2A;
                c1A.AddC1I12one2many(c2B);
                c1A.AddC1I12one2many(c2C);

                this.Session.Commit();

                this.SwitchDatabase();

                var switchC1A = (C1)this.Session.Instantiate(c1A.Id.ToString());

                Assert.Equal(MetaC1.Instance.ObjectType, switchC1A.Strategy.Class);

                var switchC2A = switchC1A.C1I12one2one;

                Assert.Equal(MetaC2.Instance.ObjectType, switchC2A.Strategy.Class);

                var switchC2BC = switchC1A.C1I12one2manies;

                Assert.Equal(MetaC2.Instance.ObjectType, switchC2BC[0].Strategy.Class);
                Assert.Equal(MetaC2.Instance.ObjectType, switchC2BC[1].Strategy.Class);

                this.Session.Commit();

                this.SwitchDatabase();

                long[] objectIds = { c1A.Id, c2A.Id };
                var switchC1aC2a = this.Session.Instantiate(objectIds);

                Assert.Equal(2, switchC1aC2a.Length);
                Assert.Contains(this.Session.Instantiate(c1A.Id), switchC1aC2a);
                Assert.Contains(this.Session.Instantiate(c2A.Id), switchC1aC2a);
            }
        }

        [Fact]
        public void SwitchSession()
        {
            foreach (var init in this.Inits)
            {
                init();

                if (this.Session is ISession)
                {
                    var c1A = C1.Create(this.Session);
                    var c2A = C2.Create(this.Session);

                    var c1AObjectId = c1A.Id;
                    var c2AObjectId = c2A.Id;

                    c1A.C1C2one2one = c2A;
                    c1A.C1C2many2one = c2A;
                    c1A.AddC1C2one2many(c2A);
                    c1A.AddC1C2many2many(c2A);

                    this.Session.Commit();

                    using (var session2 = this.CreateSession())
                    {
                        c2A = (C2)session2.Instantiate(c2AObjectId);

                        Assert.True(c2A.ExistC1WhereC1C2one2one);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c2A = (C2)session2.Instantiate(c2AObjectId);

                        Assert.True(c2A.ExistC1sWhereC1C2many2one);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c2A = (C2)session2.Instantiate(c2AObjectId);

                        Assert.True(c2A.ExistC1WhereC1C2one2many);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c2A = (C2)session2.Instantiate(c2AObjectId);

                        Assert.True(c2A.ExistC1sWhereC1C2many2many);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c1A = (C1)session2.Instantiate(c1AObjectId);

                        Assert.True(c1A.ExistC1C2one2one);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c1A = (C1)session2.Instantiate(c1AObjectId);

                        Assert.True(c1A.ExistC1C2many2one);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c1A = (C1)session2.Instantiate(c1AObjectId);

                        Assert.True(c1A.ExistC1C2one2manies);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c1A = (C1)session2.Instantiate(c1AObjectId);

                        Assert.True(c1A.ExistC1C2many2manies);
                    }
                }
            }
        }

        [Fact]
        public virtual void CreateManyPopulations()
        {
            foreach (var init in this.Inits)
            {
                init();

                // don't garbage collect populations
                var populations = new List<IDatabase>();

                for (var i = 0; i < 100; i++)
                {
                    var population1 = this.CreatePopulation();
                    populations.Add(population1);
                    var session1 = population1.CreateSession();

                    var c1 = session1.Create<C1>();

                    var population2 = this.CreatePopulation();
                    populations.Add(population2);
                    var session2 = population2.CreateSession();

                    var c2 = session2.Create<C2>();

                    session1.Commit();
                    session2.Commit();
                }
            }
        }

        [Fact]
        public void ObjectType()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1a = this.Session.Create(MetaC1.Instance.ObjectType);
                Assert.Equal(MetaC1.Instance.ObjectType, c1a.Strategy.Class);

                this.Session.Commit();

                Assert.Equal(MetaC1.Instance.ObjectType, c1a.Strategy.Class);

                var c1b = this.Session.Create(MetaC1.Instance.ObjectType);

                this.Session.Rollback();

                var exceptionThrown = false;

                try
                {
                    var objectType = c1b.Strategy.Class;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.True(exceptionThrown);

                var c2a = this.Session.Create(MetaC2.Instance.ObjectType);
                Assert.Equal(MetaC2.Instance.ObjectType, c2a.Strategy.Class);

                this.Session.Commit();

                Assert.Equal(MetaC2.Instance.ObjectType, c2a.Strategy.Class);
            }
        }

        [Fact]
        public void PrefetchEmptyPolicy()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);
                    c1A.C1AllorsString = "1";

                    var prefetchPolicy = new PrefetchPolicyBuilder().Build();

                    this.Session.Prefetch(prefetchPolicy, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    }
                }
            }
        }

        [Fact]
        public void PrefetchUnitRole()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);
                    c1A.C1AllorsString = "1";

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Equal("1", c1A.C1AllorsString);

                    this.Session.Commit();

                    Assert.Equal("1", c1A.C1AllorsString);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Equal("1", c1A.C1AllorsString);

                    this.Session.Commit();

                    c1A.C1AllorsString = "2";

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Equal("2", c1A.C1AllorsString);

                    this.Session.Rollback();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Equal("1", c1A.C1AllorsString);

                    this.Session.Rollback();

                    Assert.Equal("1", c1A.C1AllorsString);
                }
            }
        }

        [Fact]
        public void PrefetchCompositeRoleOne2One()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);
                    var c2A = C2.Create(this.Session);
                    var c2b = C2.Create(this.Session);

                    c1A.C1C2one2one = c2A;

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Equal(c2A, c1A.C1C2one2one);

                    this.Session.Commit();

                    Assert.Equal(c2A, c1A.C1C2one2one);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Equal(c2A, c1A.C1C2one2one);

                    this.Session.Commit();

                    c1A.C1C2one2one = c2b;

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Equal(c2b, c1A.C1C2one2one);

                    this.Session.Rollback();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Equal(c2A, c1A.C1C2one2one);

                    this.Session.Rollback();

                    Assert.Equal(c2A, c1A.C1C2one2one);
                }
            }
        }

        [Fact]
        public void PrefetchCompositeRoleOne2OneAndUnit()
        {
            var prefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(C1.Meta.C1C2one2one, new IPropertyType[] { C2.Meta.C2AllorsString })
                .Build();

            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1a = C1.Create(this.Session);
                    var c2a = C2.Create(this.Session);
                    var c2b = C2.Create(this.Session);

                    c2a.C2AllorsString = "c2a";
                    c2b.C2AllorsString = "c2b";

                    c1a.C1C2one2one = c2a;

                    this.Session.Prefetch(prefetchPolicy, c1a);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1a);
                    }

                    Assert.Equal(c2a, c1a.C1C2one2one);

                    this.Session.Commit();

                    Assert.Equal(c2a, c1a.C1C2one2one);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c1a);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1a);
                    }

                    Assert.Equal(c2a, c1a.C1C2one2one);

                    this.Session.Commit();

                    c1a.C1C2one2one = c2b;

                    this.Session.Prefetch(prefetchPolicy, c1a);

                    Assert.Equal(c2b, c1a.C1C2one2one);

                    this.Session.Rollback();

                    this.Session.Prefetch(prefetchPolicy, c1a);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1a);
                    }

                    Assert.Equal(c2a, c1a.C1C2one2one);

                    this.Session.Rollback();

                    Assert.Equal(c2a, c1a.C1C2one2one);
                }
            }
        }

        [Fact]
        public void PrefetchCompositeRoleOne2OneEmptyAndUnit()
        {
            var prefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(C1.Meta.C1C2one2one, new IPropertyType[] { C2.Meta.C2AllorsString })
                .Build();

            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1A);
                    }

                    Assert.Empty(c1A.C1C2one2manies);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1A);
                    }

                    Assert.Empty(c1A.C1C2one2manies);
                }
            }
        }

        [Fact]
        public void PrefetchCompositesRolesOne2Many()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);
                    var c2A = C2.Create(this.Session);
                    var c2b = C2.Create(this.Session);
                    c1A.AddC1C2one2many(c2A);

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, new[] { c1A.Strategy.ObjectId });

                    Assert.Contains(c2A, c1A.C1C2one2manies);

                    this.Session.Commit();

                    Assert.Contains(c2A, c1A.C1C2one2manies);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Contains(c2A, c1A.C1C2one2manies);

                    this.Session.Commit();

                    c1A.RemoveC1C2one2many(c2A);
                    c1A.AddC1C2one2many(c2b);

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Contains(c2b, c1A.C1C2one2manies);

                    this.Session.Rollback();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Contains(c2A, c1A.C1C2one2manies);

                    this.Session.Rollback();

                    Assert.Contains(c2A, c1A.C1C2one2manies);
                }
            }
        }

        [Fact]
        public void PrefetchCompositesRoleOne2ManyEmpty()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, c1A);
                    }

                    Assert.Empty(c1A.C1C2one2manies);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, c1A);
                    }

                    Assert.Empty(c1A.C1C2one2manies);
                }
            }
        }

        [Fact]
        public void PrefetchCompositesRoleOne2ManyEmptyAndUnit()
        {
            var prefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(C1.Meta.C1C2one2manies, new IPropertyType[] { C2.Meta.C2AllorsString })
                .Build();

            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1A);
                    }

                    Assert.Empty(c1A.C1C2one2manies);

                    this.Session.Prefetch(prefetchPolicy, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1A);
                    }

                    Assert.Empty(c1A.C1C2one2manies);
                }
            }
        }

        [Fact]
        public void PrefetchCompositesRoleMany2Many()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);
                    var c2A = C2.Create(this.Session);
                    var c2b = C2.Create(this.Session);
                    c1A.AddC1C2many2many(c2A);

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Contains(c2A, c1A.C1C2many2manies);

                    this.Session.Commit();

                    Assert.Contains(c2A, c1A.C1C2many2manies);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Contains(c2A, c1A.C1C2many2manies);

                    this.Session.Commit();

                    c1A.RemoveC1C2many2many(c2A);
                    c1A.AddC1C2many2many(c2b);

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Contains(c2b, c1A.C1C2many2manies);

                    this.Session.Rollback();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.Contains(c2A, c1A.C1C2many2manies);

                    this.Session.Rollback();

                    Assert.Contains(c2A, c1A.C1C2many2manies);
                }
            }
        }

        [Fact]
        public void PrefetchCompositesRoleMany2ManyEmpty()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, c1A);
                    }

                    Assert.Empty(c1A.C1C2many2manies);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, c1A);
                    }

                    Assert.Empty(c1A.C1C2many2manies);
                }
            }
        }

        [Fact]
        public void PrefetchCompositesRoleMany2ManyEmptyAndUnit()
        {
            var prefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(C1.Meta.C1C2many2manies, new IPropertyType[] { C2.Meta.C2AllorsString })
                .Build();

            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1A = C1.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1A);
                    }

                    Assert.Empty(c1A.C1C2many2manies);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1A);
                    }

                    Assert.Empty(c1A.C1C2many2manies);
                }
            }
        }

        [Fact]
        public void PrefetchAssociationOne2One()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1a = C1.Create(this.Session);
                    var c1b = C1.Create(this.Session);
                    var c2a = C2.Create(this.Session);
                    c1a.C1C2one2one = c2a;

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });

                    Assert.Equal(c1a, c2a.C1WhereC1C2one2one);

                    this.Session.Commit();

                    Assert.Equal(c1a, c2a.C1WhereC1C2one2one);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    }

                    Assert.Equal(c1a, c2a.C1WhereC1C2one2one);

                    this.Session.Commit();

                    c1b.C1C2one2one = c2a;

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    }

                    Assert.Equal(c1b, c2a.C1WhereC1C2one2one);

                    this.Session.Rollback();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    }

                    Assert.Equal(c1a, c2a.C1WhereC1C2one2one);

                    this.Session.Rollback();

                    Assert.Equal(c1a, c2a.C1WhereC1C2one2one);
                }
            }
        }

        [Fact]
        public void PrefetchAssociationOne2OneEmpty()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c2A = C2.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2many);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2many);
                }
            }
        }

        [Fact]
        public void PrefetchAssociationOne2OneEmptyAndUnit()
        {
            var prefetchPolicy = new PrefetchPolicyBuilder().WithRule(C2.Meta.C1WhereC1C2one2one, new IPropertyType[] { C1.Meta.C1AllorsString }).Build();
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c2A = C2.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2many);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2many);
                }
            }
        }

        [Fact]
        public void PrefetchAssociationMany2Many()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c1a = C1.Create(this.Session);
                    var c1b = C1.Create(this.Session);
                    var c2a = C2.Create(this.Session);
                    c1a.AddC1C2many2many(c2a);

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, new[] { c2a.Strategy.ObjectId });
                    }

                    Assert.Contains(c1a, c2a.C1sWhereC1C2many2many);

                    this.Session.Commit();

                    Assert.Contains(c1a, c2a.C1sWhereC1C2many2many);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, new[] { c2a.Strategy.ObjectId });
                    }

                    Assert.Contains(c1a, c2a.C1sWhereC1C2many2many);

                    this.Session.Commit();

                    c1a.RemoveC1C2many2many(c2a);
                    c1b.AddC1C2many2many(c2a);

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, new[] { c2a.Strategy.ObjectId });
                    }

                    var b = c1b.Strategy.ObjectId;
                    var assoc = c2a.C1sWhereC1C2many2many[0].Strategy.ObjectId;

                    Assert.Contains(c1b, c2a.C1sWhereC1C2many2many);

                    this.Session.Rollback();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, new[] { c2a.Strategy.ObjectId });
                    }

                    Assert.Contains(c1a, c2a.C1sWhereC1C2many2many);

                    this.Session.Rollback();

                    Assert.Contains(c1a, c2a.C1sWhereC1C2many2many);
                }
            }
        }

        [Fact]
        public void PrefetchAssociationMany2ManyEmpty()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c2A = C2.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2many);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2many);
                }
            }
        }

        [Fact]
        public void PrefetchAssociationMany2ManyEmptyAndUnit()
        {
            var prefetchPolicy = new PrefetchPolicyBuilder().WithRule(C2.Meta.C1sWhereC1C2many2many, new IPropertyType[] { C1.Meta.C1AllorsString }).Build();
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c2A = C2.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2many);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2many);
                }
            }
        }

        [Fact]
        public void PrefetchAssociationMany2OneEmpty()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c2A = C2.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2one }, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2one }, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2one);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2one }, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2one }, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2one);
                }
            }
        }

        [Fact]
        public void PrefetchAssociationMany2OneEmptyAndUnit()
        {
            var prefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(C2.Meta.C1sWhereC1C2many2one, new IPropertyType[] { C1.Meta.C1AllorsString })
                .Build();

            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c2A = C2.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2one);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c2A);
                    }

                    Assert.Empty(c2A.C1sWhereC1C2many2one);
                }
            }
        }

        protected abstract void SwitchDatabase();

        protected abstract IDatabase CreatePopulation();

        protected abstract ISession CreateSession();

        private IObject[] GetExtent(IComposite objectType) => this.Session.Extent(objectType);
    }
}

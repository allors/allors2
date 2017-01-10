// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LifeCycleTest.cs" company="Allors bvba">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Adapters;

    using NUnit.Framework;

    public abstract class LifeCycleTest
    {
        protected static readonly bool[] TrueFalse = { true, false };

        protected abstract IProfile Profile { get; }

        protected ISession Session
        {
            get
            {
                return this.Profile.Session;
            }
        }

        protected Action[] Markers
        {
            get
            {
                return this.Profile.Markers;
            }
        }

        protected Action[] Inits
        {
            get
            {
                return this.Profile.Inits;
            }
        }

        [Test]
        public void NextId()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1 = this.Session.Create<C1>();
                var strategy = c1.Strategy;
                var id = long.Parse(strategy.ObjectId.ToString());

                long nextId = long.Parse(this.Session.Create<C1>().Strategy.ObjectId.ToString());

                Assert.AreEqual(id + 1, nextId);
            }
        }

        [Test]
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

                    Assert.AreEqual(run, allorsObjects.Length);

                    total += run;

                    Assert.AreEqual(total, this.GetExtent(MetaC1.Instance.ObjectType).Length);

                    var ids = new ArrayList();
                    foreach (C1 allorsObject in allorsObjects)
                    {
                        Assert.AreEqual(MetaC1.Instance.ObjectType, allorsObject.Strategy.Class);
                        ids.Add(allorsObject.Strategy.ObjectId);
                        allorsObject.C1AllorsString = "CreateMany";
                    }

                    Assert.AreEqual(run, ids.Count);

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate((long[])ids.ToArray(typeof(long)));
                    foreach (C1 allorsObject in allorsObjects)
                    {
                        Assert.AreEqual(MetaC1.Instance.ObjectType, allorsObject.Strategy.Class);
                        allorsObject.C1AllorsString = "CreateMany";
                    }

                    var c2s = (C2[])this.Session.Create(MetaC2.Instance.ObjectType, run);
                    Assert.AreEqual(run, c2s.Length);
                }
            }
        }

        [Test]
        public void UnitRole()
        {
            foreach (var init in this.Inits)
            {
                init();

                C1 c1A = C1.Create(this.Session);

                this.Session.Commit();

                c1A.C1AllorsString = "1";

                this.Session.Commit();

                c1A.C1AllorsString = "2";

                this.Session.Rollback();

                Assert.AreEqual("1", c1A.C1AllorsString);
            }
        }

        [Test]
        public void CompositeRole()
        {
            foreach (var init in this.Inits)
            {
                init();

                C1 c1A = C1.Create(this.Session);

                C2 c2A = C2.Create(this.Session);
                C2 c2B = C2.Create(this.Session);

                this.Session.Commit();

                c1A.C1C2one2one = c2A;

                this.Session.Commit();

                c1A.C1C2one2one = c2B;

                this.Session.Rollback();

                Assert.AreEqual(c2A, c1A.C1C2one2one);
            }
        }

        [Test]
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

                Assert.AreEqual(1, c1A.C1C2one2manies.Count);
                Assert.AreEqual(c2A, c1A.C1C2one2manies[0]);
            }
        }

        [Test]
        public void Delete()
        {
            foreach (var init in this.Inits)
            {
                init();

                // Object
                C1 anObject = C1.Create(this.Session);

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);
                anObject = (C1)this.Session.Instantiate(anObject.Strategy.ObjectId);
                Assert.IsNull(anObject);

                //// Commit & Rollback

                anObject = C1.Create(this.Session);
                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                var id = anObject.Strategy.ObjectId;
                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                //// Commit + Commit + Commit

                anObject = C1.Create(this.Session);

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                //// Nothing + Commit + Rollback

                anObject = C1.Create(this.Session);

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                //// Commit + Commit + Rollback

                anObject = C1.Create(this.Session);

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                //// Nothing + Rollback + Rollback

                anObject = C1.Create(this.Session);
                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                //// Commit + Rollback + Rollback

                anObject = C1.Create(this.Session);

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                //// Nothing + Rollback + Commit

                anObject = C1.Create(this.Session);

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsNull(anObject);

                //// Commit + Rollback + Commit

                anObject = C1.Create(this.Session);

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                // instantiate
                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;

                this.Session.Commit();

                anObject.Strategy.Delete();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                // Clean up
                this.Session.Commit();
                foreach (C1 removeObject in this.GetExtent(MetaC1.Instance.ObjectType))
                {
                    removeObject.Strategy.Delete();
                }

                this.Session.Commit();

                // Strategy
                var databaseSession = this.Session as ISession;
                if (databaseSession != null)
                {
                    IStrategy aStrategy = C1.Create(this.Session).Strategy;

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);
                    aStrategy = databaseSession.InstantiateStrategy(aStrategy.ObjectId);
                    Assert.IsNull(aStrategy);

                    //// Commit & Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;
                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;
                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    //// Commit + Commit + Commit

                    aStrategy = C1.Create(databaseSession).Strategy;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    //// Nothing + Commit + Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    //// Commit + Commit + Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    //// Nothing + Rollback + Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;
                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    //// Commit + Rollback + Rollback

                    aStrategy = C1.Create(databaseSession).Strategy;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.IsFalse(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.IsFalse(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsFalse(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsFalse(aStrategy.IsDeleted);

                    //// Nothing + Rollback + Commit

                    aStrategy = C1.Create(databaseSession).Strategy;

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsNull(aStrategy);

                    //// Commit + Rollback + Commit

                    aStrategy = C1.Create(databaseSession).Strategy;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    Assert.IsFalse(aStrategy.IsDeleted);

                    databaseSession.Commit();
                    Assert.IsFalse(aStrategy.IsDeleted);

                    // instantiate
                    aStrategy = C1.Create(databaseSession).Strategy;
                    id = aStrategy.ObjectId;

                    databaseSession.Commit();

                    aStrategy.Delete();
                    Assert.IsTrue(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsFalse(aStrategy.IsDeleted);

                    databaseSession.Rollback();
                    aStrategy = databaseSession.InstantiateStrategy(id);
                    Assert.IsFalse(aStrategy.IsDeleted);

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

                C1 secondObject = C1.Create(this.Session);
                secondObject.C1AllorsString = "b";
                C1 thirdObject = C1.Create(this.Session);
                thirdObject.C1AllorsString = "c";

                Assert.AreEqual(2, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                thirdObject.Strategy.Delete();

                Assert.AreEqual(1, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                Assert.AreEqual("b", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

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

                Assert.AreEqual(2, this.GetExtent(MetaC1.Instance.ObjectType).Length);

                AllorsTestUtils.ForceRoleCaching(secondObject);
                AllorsTestUtils.ForceRoleCaching(thirdObject);

                thirdObject.Strategy.Delete();

                Assert.AreEqual(1, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                Assert.AreEqual("b", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

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

                Assert.AreEqual(2, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                thirdObject.Strategy.Delete();

                this.Session.Commit();

                Assert.AreEqual(1, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                Assert.AreEqual("b", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

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

                Assert.AreEqual(2, this.GetExtent(MetaC1.Instance.ObjectType).Length);

                AllorsTestUtils.ForceRoleCaching(secondObject);
                AllorsTestUtils.ForceRoleCaching(thirdObject);

                thirdObject.Strategy.Delete();
                this.Session.Commit();

                Assert.AreEqual(1, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                Assert.AreEqual("b", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                secondObject.Strategy.Delete();
                this.Session.Commit();

                //// Rollback

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";
                this.Session.Commit();

                anObject.Strategy.Delete();

                this.Session.Rollback();

                Assert.AreEqual(1, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                Assert.AreEqual("a", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                secondObject = C1.Create(this.Session);
                secondObject.C1AllorsString = "b";
                thirdObject = C1.Create(this.Session);
                thirdObject.C1AllorsString = "c";

                Assert.AreEqual(3, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                thirdObject.Strategy.Delete();

                this.Session.Rollback();

                Assert.AreEqual(1, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                Assert.AreEqual("a", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                anObject.Strategy.Delete();

                this.Session.Commit();

                //// Cached

                anObject = C1.Create(this.Session);
                anObject.C1AllorsString = "a";
                this.Session.Commit();

                AllorsTestUtils.ForceRoleCaching(anObject);

                anObject.Strategy.Delete();
                this.Session.Rollback();

                Assert.AreEqual(1, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                Assert.AreEqual("a", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                secondObject = C1.Create(this.Session);
                secondObject.C1AllorsString = "b";
                thirdObject = C1.Create(this.Session);
                thirdObject.C1AllorsString = "c";

                Assert.AreEqual(3, this.GetExtent(MetaC1.Instance.ObjectType).Length);

                AllorsTestUtils.ForceRoleCaching(secondObject);
                AllorsTestUtils.ForceRoleCaching(thirdObject);

                thirdObject.Strategy.Delete();
                this.Session.Rollback();

                Assert.AreEqual(1, this.GetExtent(MetaC1.Instance.ObjectType).Length);
                Assert.AreEqual("a", ((C1)this.GetExtent(MetaC1.Instance.ObjectType)[0]).C1AllorsString);

                anObject.Strategy.Delete();
                this.Session.Commit();

                //// IComposite

                //// Role

                C1 fromC1a = C1.Create(this.Session);
                C1 fromC1b = C1.Create(this.Session);
                C1 fromC1c = C1.Create(this.Session);
                C1 fromC1d = C1.Create(this.Session);

                C1 toC1a = C1.Create(this.Session);
                C1 toC1b = C1.Create(this.Session);
                C1 toC1c = C1.Create(this.Session);
                C1 toC1d = C1.Create(this.Session);

                C2 toC2a = C2.Create(this.Session);
                C2 toC2b = C2.Create(this.Session);
                C2 toC2c = C2.Create(this.Session);
                C2 toC2d = C2.Create(this.Session);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1b.Strategy.IsDeleted);
                Assert.AreEqual(null, fromC1a.C1C1one2one);
                Assert.AreEqual(0, fromC1a.C1C1one2manies.Count);
                Assert.AreEqual(null, fromC1a.C1C1many2one);
                Assert.AreEqual(0, fromC1a.C1C1many2manies.Count);

                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1c.Strategy.IsDeleted);
                Assert.IsFalse(toC1d.Strategy.IsDeleted);
                Assert.AreEqual(toC1c, fromC1b.C1C1one2one);
                Assert.AreEqual(2, fromC1b.C1C1one2manies.Count);
                Assert.AreEqual(toC1c, fromC1b.C1C1many2one);
                Assert.AreEqual(2, fromC1b.C1C1many2manies.Count);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC2a.Strategy.IsDeleted);
                Assert.IsTrue(toC2b.Strategy.IsDeleted);
                Assert.AreEqual(null, fromC1a.C1C2one2one);
                Assert.AreEqual(0, fromC1a.C1C2one2manies.Count);
                Assert.AreEqual(null, fromC1a.C1C2many2one);
                Assert.AreEqual(0, fromC1a.C1C2many2manies.Count);

                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC2c.Strategy.IsDeleted);
                Assert.IsFalse(toC2d.Strategy.IsDeleted);
                Assert.AreEqual(toC2c, fromC1b.C1C2one2one);
                Assert.AreEqual(2, fromC1b.C1C2one2manies.Count);
                Assert.AreEqual(toC2c, fromC1b.C1C2many2one);
                Assert.AreEqual(2, fromC1b.C1C2many2manies.Count);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1b.Strategy.IsDeleted);
                Assert.AreEqual(null, fromC1a.C1C1one2one);
                Assert.AreEqual(0, fromC1a.C1C1one2manies.Count);
                Assert.AreEqual(null, fromC1a.C1C1many2one);
                Assert.AreEqual(0, fromC1a.C1C1many2manies.Count);

                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1c.Strategy.IsDeleted);
                Assert.IsFalse(toC1d.Strategy.IsDeleted);
                Assert.AreEqual(toC1c, fromC1b.C1C1one2one);
                Assert.AreEqual(2, fromC1b.C1C1one2manies.Count);
                Assert.AreEqual(toC1c, fromC1b.C1C1many2one);
                Assert.AreEqual(2, fromC1b.C1C1many2manies.Count);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC2a.Strategy.IsDeleted);
                Assert.IsTrue(toC2b.Strategy.IsDeleted);
                Assert.AreEqual(null, fromC1a.C1C2one2one);
                Assert.AreEqual(0, fromC1a.C1C2one2manies.Count);
                Assert.AreEqual(null, fromC1a.C1C2many2one);
                Assert.AreEqual(0, fromC1a.C1C2many2manies.Count);

                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC2c.Strategy.IsDeleted);
                Assert.IsFalse(toC2d.Strategy.IsDeleted);
                Assert.AreEqual(toC2c, fromC1b.C1C2one2one);
                Assert.AreEqual(2, fromC1b.C1C2one2manies.Count);
                Assert.AreEqual(toC2c, fromC1b.C1C2many2one);
                Assert.AreEqual(2, fromC1b.C1C2many2manies.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsTrue(fromC1c.Strategy.IsDeleted);
                Assert.IsTrue(fromC1d.Strategy.IsDeleted);

                Assert.IsTrue(toC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1b.Strategy.IsDeleted);
                Assert.IsTrue(toC1c.Strategy.IsDeleted);
                Assert.IsTrue(toC1d.Strategy.IsDeleted);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(fromC1c.Strategy.IsDeleted);
                Assert.IsFalse(fromC1d.Strategy.IsDeleted);

                Assert.IsFalse(toC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1c.Strategy.IsDeleted);
                Assert.IsFalse(toC1d.Strategy.IsDeleted);

                Assert.AreEqual(fromC1a, toC1a.C1WhereC1C1one2one);
                Assert.AreEqual(fromC1a, toC1b.C1WhereC1C1one2many);
                Assert.AreEqual(1, toC1a.C1sWhereC1C1many2one.Count);
                Assert.AreEqual(1, toC1a.C1sWhereC1C1many2many.Count);
                Assert.AreEqual(1, toC1b.C1sWhereC1C1many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsTrue(fromC1c.Strategy.IsDeleted);
                Assert.IsTrue(fromC1d.Strategy.IsDeleted);

                Assert.IsTrue(toC2a.Strategy.IsDeleted);
                Assert.IsTrue(toC2b.Strategy.IsDeleted);
                Assert.IsTrue(toC2c.Strategy.IsDeleted);
                Assert.IsTrue(toC2d.Strategy.IsDeleted);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(fromC1c.Strategy.IsDeleted);
                Assert.IsFalse(fromC1d.Strategy.IsDeleted);

                Assert.IsFalse(toC2a.Strategy.IsDeleted);
                Assert.IsFalse(toC2b.Strategy.IsDeleted);
                Assert.IsFalse(toC2c.Strategy.IsDeleted);
                Assert.IsFalse(toC2d.Strategy.IsDeleted);

                Assert.AreEqual(fromC1a, toC2a.C1WhereC1C2one2one);
                Assert.AreEqual(fromC1a, toC2b.C1WhereC1C2one2many);
                Assert.AreEqual(1, toC2a.C1sWhereC1C2many2one.Count);
                Assert.AreEqual(1, toC2a.C1sWhereC1C2many2many.Count);
                Assert.AreEqual(1, toC2b.C1sWhereC1C2many2many.Count);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1b.Strategy.IsDeleted);
                Assert.AreEqual(null, fromC1a.C1C1one2one);
                Assert.AreEqual(0, fromC1a.C1C1one2manies.Count);
                Assert.AreEqual(null, fromC1a.C1C1many2one);
                Assert.AreEqual(0, fromC1a.C1C1many2manies.Count);

                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1c.Strategy.IsDeleted);
                Assert.IsFalse(toC1d.Strategy.IsDeleted);
                Assert.AreEqual(toC1c, fromC1b.C1C1one2one);
                Assert.AreEqual(2, fromC1b.C1C1one2manies.Count);
                Assert.AreEqual(toC1c, fromC1b.C1C1many2one);
                Assert.AreEqual(2, fromC1b.C1C1many2manies.Count);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1b.Strategy.IsDeleted);
                Assert.AreEqual(null, fromC1a.C1C1one2one);
                Assert.AreEqual(0, fromC1a.C1C1one2manies.Count);
                Assert.AreEqual(null, fromC1a.C1C1many2one);
                Assert.AreEqual(0, fromC1a.C1C1many2manies.Count);

                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1c.Strategy.IsDeleted);
                Assert.IsFalse(toC1d.Strategy.IsDeleted);
                Assert.AreEqual(toC1c, fromC1b.C1C1one2one);
                Assert.AreEqual(2, fromC1b.C1C1one2manies.Count);
                Assert.AreEqual(toC1c, fromC1b.C1C1many2one);
                Assert.AreEqual(2, fromC1b.C1C1many2manies.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsTrue(fromC1c.Strategy.IsDeleted);
                Assert.IsTrue(fromC1d.Strategy.IsDeleted);

                Assert.IsTrue(toC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1b.Strategy.IsDeleted);
                Assert.IsTrue(toC1c.Strategy.IsDeleted);
                Assert.IsTrue(toC1d.Strategy.IsDeleted);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(fromC1c.Strategy.IsDeleted);
                Assert.IsFalse(fromC1d.Strategy.IsDeleted);

                Assert.IsFalse(toC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1c.Strategy.IsDeleted);
                Assert.IsFalse(toC1d.Strategy.IsDeleted);

                Assert.AreEqual(fromC1a, toC1a.C1WhereC1C1one2one);
                Assert.AreEqual(fromC1a, toC1b.C1WhereC1C1one2many);
                Assert.AreEqual(1, toC1a.C1sWhereC1C1many2one.Count);
                Assert.AreEqual(1, toC1a.C1sWhereC1C1many2many.Count);
                Assert.AreEqual(1, toC1b.C1sWhereC1C1many2many.Count);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC2a.Strategy.IsDeleted);
                Assert.IsTrue(toC2b.Strategy.IsDeleted);
                Assert.AreEqual(null, fromC1a.C1C2one2one);
                Assert.AreEqual(0, fromC1a.C1C2one2manies.Count);
                Assert.AreEqual(null, fromC1a.C1C2many2one);
                Assert.AreEqual(0, fromC1a.C1C2many2manies.Count);

                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC2c.Strategy.IsDeleted);
                Assert.IsFalse(toC2d.Strategy.IsDeleted);
                Assert.AreEqual(toC2c, fromC1b.C1C2one2one);
                Assert.AreEqual(2, fromC1b.C1C2one2manies.Count);
                Assert.AreEqual(toC2c, fromC1b.C1C2many2one);
                Assert.AreEqual(2, fromC1b.C1C2many2manies.Count);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC2a.Strategy.IsDeleted);
                Assert.IsTrue(toC2b.Strategy.IsDeleted);
                Assert.AreEqual(null, fromC1a.C1C2one2one);
                Assert.AreEqual(0, fromC1a.C1C2one2manies.Count);
                Assert.AreEqual(null, fromC1a.C1C2many2one);
                Assert.AreEqual(0, fromC1a.C1C2many2manies.Count);

                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC2c.Strategy.IsDeleted);
                Assert.IsFalse(toC2d.Strategy.IsDeleted);
                Assert.AreEqual(toC2c, fromC1b.C1C2one2one);
                Assert.AreEqual(2, fromC1b.C1C2one2manies.Count);
                Assert.AreEqual(toC2c, fromC1b.C1C2many2one);
                Assert.AreEqual(2, fromC1b.C1C2many2manies.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsTrue(fromC1c.Strategy.IsDeleted);
                Assert.IsTrue(fromC1d.Strategy.IsDeleted);

                Assert.IsTrue(toC2a.Strategy.IsDeleted);
                Assert.IsTrue(toC2b.Strategy.IsDeleted);
                Assert.IsTrue(toC2c.Strategy.IsDeleted);
                Assert.IsTrue(toC2d.Strategy.IsDeleted);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(fromC1c.Strategy.IsDeleted);
                Assert.IsFalse(fromC1d.Strategy.IsDeleted);

                Assert.IsFalse(toC2a.Strategy.IsDeleted);
                Assert.IsFalse(toC2b.Strategy.IsDeleted);
                Assert.IsFalse(toC2c.Strategy.IsDeleted);
                Assert.IsFalse(toC2d.Strategy.IsDeleted);

                Assert.AreEqual(fromC1a, toC2a.C1WhereC1C2one2one);
                Assert.AreEqual(fromC1a, toC2b.C1WhereC1C2one2many);
                Assert.AreEqual(1, toC2a.C1sWhereC1C2many2one.Count);
                Assert.AreEqual(1, toC2a.C1sWhereC1C2many2many.Count);
                Assert.AreEqual(1, toC2b.C1sWhereC1C2many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1b.Strategy.IsDeleted);
                Assert.AreEqual(null, toC1a.C1WhereC1C1one2one);
                Assert.AreEqual(null, toC1b.C1WhereC1C1one2many);
                Assert.AreEqual(0, toC1a.C1sWhereC1C1many2one.Count);
                Assert.AreEqual(0, toC1a.C1sWhereC1C1many2many.Count);
                Assert.AreEqual(0, toC1b.C1sWhereC1C1many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC2a.Strategy.IsDeleted);
                Assert.IsFalse(toC2b.Strategy.IsDeleted);
                Assert.AreEqual(null, toC2a.C1WhereC1C2one2one);
                Assert.AreEqual(null, toC2b.C1WhereC1C2one2many);
                Assert.AreEqual(0, toC2a.C1sWhereC1C2many2one.Count);
                Assert.AreEqual(0, toC2a.C1sWhereC1C2many2many.Count);
                Assert.AreEqual(0, toC2b.C1sWhereC1C2many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1b.Strategy.IsDeleted);
                Assert.AreEqual(null, toC1a.C1WhereC1C1one2one);
                Assert.AreEqual(null, toC1b.C1WhereC1C1one2many);
                Assert.AreEqual(0, toC1a.C1sWhereC1C1many2one.Count);
                Assert.AreEqual(0, toC1a.C1sWhereC1C1many2many.Count);
                Assert.AreEqual(0, toC1b.C1sWhereC1C1many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC2a.Strategy.IsDeleted);
                Assert.IsFalse(toC2b.Strategy.IsDeleted);
                Assert.AreEqual(null, toC2a.C1WhereC1C2one2one);
                Assert.AreEqual(null, toC2b.C1WhereC1C2one2many);
                Assert.AreEqual(0, toC2a.C1sWhereC1C2many2one.Count);
                Assert.AreEqual(0, toC2a.C1sWhereC1C2many2many.Count);
                Assert.AreEqual(0, toC2b.C1sWhereC1C2many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsTrue(fromC1c.Strategy.IsDeleted);
                Assert.IsTrue(fromC1d.Strategy.IsDeleted);
                Assert.IsTrue(toC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1b.Strategy.IsDeleted);
                Assert.IsTrue(toC1c.Strategy.IsDeleted);
                Assert.IsTrue(toC1d.Strategy.IsDeleted);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(fromC1c.Strategy.IsDeleted);
                Assert.IsFalse(fromC1d.Strategy.IsDeleted);
                Assert.IsFalse(toC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1c.Strategy.IsDeleted);
                Assert.IsFalse(toC1d.Strategy.IsDeleted);

                Assert.AreEqual(fromC1a, toC1a.C1WhereC1C1one2one);
                Assert.AreEqual(fromC1a, toC1b.C1WhereC1C1one2many);
                Assert.AreEqual(1, toC1a.C1sWhereC1C1many2one.Count);
                Assert.AreEqual(1, toC1a.C1sWhereC1C1many2many.Count);
                Assert.AreEqual(1, toC1b.C1sWhereC1C1many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsTrue(fromC1c.Strategy.IsDeleted);
                Assert.IsTrue(fromC1d.Strategy.IsDeleted);
                Assert.IsTrue(toC2a.Strategy.IsDeleted);
                Assert.IsTrue(toC2b.Strategy.IsDeleted);
                Assert.IsTrue(toC2c.Strategy.IsDeleted);
                Assert.IsTrue(toC2d.Strategy.IsDeleted);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(fromC1c.Strategy.IsDeleted);
                Assert.IsFalse(fromC1d.Strategy.IsDeleted);
                Assert.IsFalse(toC2a.Strategy.IsDeleted);
                Assert.IsFalse(toC2b.Strategy.IsDeleted);
                Assert.IsFalse(toC2c.Strategy.IsDeleted);
                Assert.IsFalse(toC2d.Strategy.IsDeleted);

                Assert.AreEqual(fromC1a, toC2a.C1WhereC1C2one2one);
                Assert.AreEqual(fromC1a, toC2b.C1WhereC1C2one2many);
                Assert.AreEqual(1, toC2a.C1sWhereC1C2many2one.Count);
                Assert.AreEqual(1, toC2a.C1sWhereC1C2many2many.Count);
                Assert.AreEqual(1, toC2b.C1sWhereC1C2many2many.Count);

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

                Assert.AreEqual(0, toC1a.C1sWhereC1C1many2one.Count);

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C1many2manies);

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1b.Strategy.IsDeleted);
                Assert.AreEqual(null, toC1a.C1WhereC1C1one2one);
                Assert.AreEqual(null, toC1b.C1WhereC1C1one2many);
                Assert.AreEqual(0, toC1a.C1sWhereC1C1many2one.Count);
                Assert.AreEqual(0, toC1a.C1sWhereC1C1many2many.Count);
                Assert.AreEqual(0, toC1b.C1sWhereC1C1many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1b.Strategy.IsDeleted);
                Assert.AreEqual(null, toC1a.C1WhereC1C1one2one);
                Assert.AreEqual(null, toC1b.C1WhereC1C1one2many);
                Assert.AreEqual(0, toC1a.C1sWhereC1C1many2one.Count);
                Assert.AreEqual(0, toC1a.C1sWhereC1C1many2many.Count);
                Assert.AreEqual(0, toC1b.C1sWhereC1C1many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsTrue(fromC1c.Strategy.IsDeleted);
                Assert.IsTrue(fromC1d.Strategy.IsDeleted);
                Assert.IsTrue(toC1a.Strategy.IsDeleted);
                Assert.IsTrue(toC1b.Strategy.IsDeleted);
                Assert.IsTrue(toC1c.Strategy.IsDeleted);
                Assert.IsTrue(toC1d.Strategy.IsDeleted);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(fromC1c.Strategy.IsDeleted);
                Assert.IsFalse(fromC1d.Strategy.IsDeleted);
                Assert.IsFalse(toC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC1c.Strategy.IsDeleted);
                Assert.IsFalse(toC1d.Strategy.IsDeleted);

                Assert.AreEqual(fromC1a, toC1a.C1WhereC1C1one2one);
                Assert.AreEqual(fromC1a, toC1b.C1WhereC1C1one2many);
                Assert.AreEqual(1, toC1a.C1sWhereC1C1many2one.Count);
                Assert.AreEqual(1, toC1a.C1sWhereC1C1many2many.Count);
                Assert.AreEqual(1, toC1b.C1sWhereC1C1many2many.Count);

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

                Assert.AreEqual(0, toC2a.C1sWhereC1C2many2one.Count);

                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleExistHasException(fromC1a, MetaC1.Instance.C1C2many2manies);

                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2one2manies);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2many2one);
                StrategyAssert.RoleGetHasException(fromC1a, MetaC1.Instance.C1C2many2manies);

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(toC2a.Strategy.IsDeleted);
                Assert.IsFalse(toC2b.Strategy.IsDeleted);
                Assert.AreEqual(null, toC2a.C1WhereC1C2one2one);
                Assert.AreEqual(null, toC2b.C1WhereC1C2one2many);
                Assert.AreEqual(0, toC2a.C1sWhereC1C2many2one.Count);
                Assert.AreEqual(0, toC2a.C1sWhereC1C2many2many.Count);
                Assert.AreEqual(0, toC2b.C1sWhereC1C2many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(toC2a.Strategy.IsDeleted);
                Assert.IsFalse(toC2b.Strategy.IsDeleted);
                Assert.AreEqual(null, toC2a.C1WhereC1C2one2one);
                Assert.AreEqual(null, toC2b.C1WhereC1C2one2many);
                Assert.AreEqual(0, toC2a.C1sWhereC1C2many2one.Count);
                Assert.AreEqual(0, toC2a.C1sWhereC1C2many2many.Count);
                Assert.AreEqual(0, toC2b.C1sWhereC1C2many2many.Count);

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

                Assert.IsTrue(fromC1a.Strategy.IsDeleted);
                Assert.IsTrue(fromC1b.Strategy.IsDeleted);
                Assert.IsTrue(fromC1c.Strategy.IsDeleted);
                Assert.IsTrue(fromC1d.Strategy.IsDeleted);
                Assert.IsTrue(toC2a.Strategy.IsDeleted);
                Assert.IsTrue(toC2b.Strategy.IsDeleted);
                Assert.IsTrue(toC2c.Strategy.IsDeleted);
                Assert.IsTrue(toC2d.Strategy.IsDeleted);

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

                Assert.IsFalse(fromC1a.Strategy.IsDeleted);
                Assert.IsFalse(fromC1b.Strategy.IsDeleted);
                Assert.IsFalse(fromC1c.Strategy.IsDeleted);
                Assert.IsFalse(fromC1d.Strategy.IsDeleted);
                Assert.IsFalse(toC2a.Strategy.IsDeleted);
                Assert.IsFalse(toC2b.Strategy.IsDeleted);
                Assert.IsFalse(toC2c.Strategy.IsDeleted);
                Assert.IsFalse(toC2d.Strategy.IsDeleted);

                Assert.AreEqual(fromC1a, toC2a.C1WhereC1C2one2one);
                Assert.AreEqual(fromC1a, toC2b.C1WhereC1C2one2many);
                Assert.AreEqual(1, toC2a.C1sWhereC1C2many2one.Count);
                Assert.AreEqual(1, toC2a.C1sWhereC1C2many2many.Count);
                Assert.AreEqual(1, toC2b.C1sWhereC1C2many2many.Count);

                //// Assignment

                anObject = C1.Create(this.Session);
                C1 c1Removed = C1.Create(this.Session);
                c1Removed.Strategy.Delete();
                C1[] c1RemovedArray = { c1Removed };

                bool error = false;
                try
                {
                    anObject.C1C1one2one = c1Removed;
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);

                error = false;
                try
                {
                    anObject.AddC1C1one2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);
                Assert.AreEqual(0, anObject.C1C1one2manies.Count);

                error = false;
                try
                {
                    anObject.C1C1one2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);
                Assert.AreEqual(0, anObject.C1C1one2manies.Count);

                error = false;
                try
                {
                    anObject.AddC1C1many2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);
                Assert.AreEqual(0, anObject.C1C1many2manies.Count);

                error = false;
                try
                {
                    anObject.C1C1many2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);
                Assert.AreEqual(0, anObject.C1C1many2manies.Count);

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

                Assert.IsTrue(error);

                error = false;
                try
                {
                    anObject.AddC1C1one2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);
                Assert.AreEqual(0, anObject.C1C1one2manies.Count);

                error = false;
                try
                {
                    anObject.C1C1one2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);
                Assert.AreEqual(0, anObject.C1C1one2manies.Count);

                error = false;
                try
                {
                    anObject.AddC1C1many2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);
                Assert.AreEqual(0, anObject.C1C1many2manies.Count);

                error = false;
                try
                {
                    anObject.C1C1many2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);
                Assert.AreEqual(0, anObject.C1C1many2manies.Count);

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

                Assert.IsTrue(error);

                error = false;
                try
                {
                    anObject.AddC1C1one2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);

                error = false;
                try
                {
                    anObject.C1C1one2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);

                error = false;
                try
                {
                    anObject.AddC1C1many2many(c1Removed);
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);

                error = false;
                try
                {
                    anObject.C1C1many2manies = c1RemovedArray;
                }
                catch
                {
                    error = true;
                }

                Assert.IsTrue(error);

                // Commit + Rollback
                anObject = C1.Create(this.Session);
                c1Removed = C1.Create(this.Session);
                c1RemovedArray = new C1[1];
                c1RemovedArray[0] = c1Removed;

                this.Session.Commit();

                c1Removed.Strategy.Delete();

                this.Session.Rollback();

                anObject.C1C1one2one = c1Removed;
                Assert.AreEqual(c1Removed, anObject.C1C1one2one);

                anObject.AddC1C1one2many(c1Removed);
                Assert.AreEqual(1, anObject.C1C1one2manies.Count);

                anObject.C1C1one2manies = c1RemovedArray;
                Assert.AreEqual(1, anObject.C1C1one2manies.Count);

                anObject.AddC1C1many2many(c1Removed);
                Assert.AreEqual(1, anObject.C1C1many2manies.Count);

                anObject.C1C1many2manies = c1RemovedArray;
                Assert.AreEqual(1, anObject.C1C1many2manies.Count);

                //// Proxy

                C1 proxy = C1.Create(this.Session);
                id = proxy.Strategy.ObjectId;
                this.Session.Commit();

                C1 subject = C1.Instantiate(this.Session, id);
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
                Assert.IsFalse(proxy.Strategy.IsDeleted);

                this.Session.Commit();

                //// unit roles

                anObject = C1.Create(this.Session);
                C1 anotherObject = C1.Create(this.Session);
                anotherObject.C1AllorsString = "value";
                anObject.Strategy.Delete();
                Assert.AreEqual("value", anotherObject.C1AllorsString);

                this.Session.Commit();

                Assert.AreEqual("value", anotherObject.C1AllorsString);

                anObject = C1.Create(this.Session);
                anotherObject = C1.Create(this.Session);
                anotherObject.C1AllorsString = "value";
                anObject.Strategy.Delete();

                this.Session.Commit();

                Assert.AreEqual("value", anotherObject.C1AllorsString);
            }
        }

        [Test]
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

                    Assert.IsTrue(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.C1C2many2one = c2a;
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.IsTrue(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.AddC1C2one2many(c2a);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.IsTrue(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.C1C2one2manies = c2Array;
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.IsTrue(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.AddC1C2many2many(c2a);
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.IsTrue(exceptionThrown);

                    exceptionThrown = false;
                    try
                    {
                        c1a.C1C2many2manies = c2Array;
                    }
                    catch
                    {
                        exceptionThrown = true;
                    }

                    Assert.IsTrue(exceptionThrown);
                }
                finally
                {
                    secondSession.Commit();
                }
            }
        }

        [Test]
        public void Identity()
        {
            foreach (var init in this.Inits)
            {
                init();

                C1 anObject = C1.Create(this.Session);
                var id = anObject.Strategy.ObjectId;
                C1 proxy = C1.Instantiate(this.Session, id);

                C1 anotherObject = C1.Create(this.Session);
                long anotherId = anotherObject.Strategy.ObjectId;
                C1 anotherProxy = C1.Instantiate(this.Session, anotherId);

                Assert.AreEqual(anObject, proxy);
                Assert.AreEqual(anotherObject, anotherProxy);
                Assert.AreNotEqual(anObject, anotherObject);
                Assert.AreNotEqual(anObject, anotherProxy);
                Assert.AreNotEqual(proxy, anotherObject);
                Assert.AreNotEqual(proxy, anotherProxy);

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                anotherObject = C1.Create(this.Session);
                anotherId = anotherObject.Strategy.ObjectId;
                anotherProxy = C1.Instantiate(this.Session, anotherId);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                anotherObject = C1.Instantiate(this.Session, anotherId);

                Assert.AreEqual(anObject, proxy);
                Assert.AreEqual(anotherObject, anotherProxy);
                Assert.AreNotEqual(anObject, anotherObject);
                Assert.AreNotEqual(anObject, anotherProxy);
                Assert.AreNotEqual(proxy, anotherObject);
                Assert.AreNotEqual(proxy, anotherProxy);

                //// Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                anotherObject = C1.Create(this.Session);
                anotherId = anotherObject.Strategy.ObjectId;
                anotherProxy = C1.Instantiate(this.Session, anotherId);

                this.Session.Rollback();

                Assert.AreEqual(anObject, proxy);
                Assert.AreEqual(anotherObject, anotherProxy);
                Assert.AreNotEqual(anObject, anotherObject);
                Assert.AreNotEqual(anObject, anotherProxy);
                Assert.AreNotEqual(proxy, anotherObject);
                Assert.AreNotEqual(proxy, anotherProxy);
            }
        }

        [Test]
        public void Insert()
        {
            foreach (var init in this.Inits)
            {
                init();

                var session = this.Session as ISession;
                if (session != null)
                {
                    C1 a = C1.Create(this.Session);
                    C1 b;

                    long aLongId = long.Parse(a.Strategy.ObjectId.ToString());
                    if (a.Strategy.IsNewInSession)
                    {
                        b = (C1)session.Insert(MetaC1.Instance.ObjectType, (aLongId - 1).ToString());
                    }
                    else
                    {
                        b = (C1)session.Insert(MetaC1.Instance.ObjectType, (aLongId + 1).ToString());
                    }

                    C1 c = C1.Create(this.Session);

                    Assert.AreNotEqual(a.Strategy.ObjectId, b.Strategy.ObjectId);
                    Assert.AreNotEqual(a.Strategy.ObjectId, c.Strategy.ObjectId);
                    Assert.AreNotEqual(c.Strategy.ObjectId, b.Strategy.ObjectId);

                    long[] ids = { a.Strategy.ObjectId, b.Strategy.ObjectId, c.Strategy.ObjectId };
                    foreach (long id in ids)
                    {
                        var exceptionThrown = false;
                        try
                        {
                            session.Insert(MetaC1.Instance.ObjectType, id);
                        }
                        catch
                        {
                            exceptionThrown = true;
                        }

                        Assert.IsTrue(exceptionThrown);
                    }

                    Assert.AreNotEqual(a.Strategy.ObjectId, b.Strategy.ObjectId);
                    Assert.AreNotEqual(a.Strategy.ObjectId, c.Strategy.ObjectId);
                    Assert.AreNotEqual(c.Strategy.ObjectId, b.Strategy.ObjectId);
                }
            }
        }

        [Test]
        public void Instantiate()
        {
            foreach (var init in this.Inits)
            {
                init();

                C1 anObject = C1.Create(this.Session);
                var id = anObject.Strategy.ObjectId;
                var sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                //// Proxy

                //// Unit

                C1 subject = C1.Create(this.Session);
                id = subject.Strategy.ObjectId;
                this.Session.Commit();

                subject.C1AllorsString = "a";
                C1 proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                Assert.AreEqual("b", subject.C1AllorsString);
                Assert.AreEqual("b", proxy.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                Assert.AreEqual("d", proxy.C1AllorsString);
                Assert.AreEqual("d", subject.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                this.Session.Commit();
                Assert.AreEqual("b", subject.C1AllorsString);
                Assert.AreEqual("b", proxy.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                this.Session.Commit();
                Assert.AreEqual("d", proxy.C1AllorsString);
                Assert.AreEqual("d", subject.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                Assert.AreEqual("b", subject.C1AllorsString);
                this.Session.Commit();
                Assert.AreEqual("b", proxy.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                Assert.AreEqual("d", proxy.C1AllorsString);
                this.Session.Commit();
                Assert.AreEqual("d", subject.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                this.Session.Commit();
                Assert.AreEqual("b", subject.C1AllorsString);
                this.Session.Commit();
                Assert.AreEqual("b", proxy.C1AllorsString);
                this.Session.Commit();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                this.Session.Commit();
                Assert.AreEqual("d", proxy.C1AllorsString);
                this.Session.Commit();
                Assert.AreEqual("d", subject.C1AllorsString);
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

                Assert.AreEqual("a", fromProxy.C1AllorsString);
                Assert.AreEqual(toProxy, fromProxy.C1C1one2one);
                Assert.AreEqual(toProxy, fromProxy.C1C1many2one);
                Assert.Contains(toProxy, (C1[])fromProxy.C1C1one2manies);
                Assert.Contains(toProxy, (C1[])fromProxy.C1C1many2manies);

                Assert.AreEqual(fromProxy, toProxy.C1WhereC1C1one2one);
                Assert.Contains(fromProxy, (C1[])toProxy.C1sWhereC1C1many2one);
                Assert.AreEqual(fromProxy, toProxy.C1WhereC1C1one2many);
                Assert.Contains(fromProxy, (C1[])toProxy.C1sWhereC1C1many2many);

                from.C1AllorsString = null;
                from.C1C1one2one = null;
                from.C1C1many2one = null;
                from.C1C1one2manies = null;
                from.C1C1many2manies = null;

                this.Session.Commit();

                Assert.AreEqual(null, from.C1AllorsString);
                Assert.AreEqual(null, from.C1C1one2one);
                Assert.AreEqual(null, from.C1C1many2one);
                Assert.IsEmpty((C1[])from.C1C1one2manies);
                Assert.IsEmpty((C1[])from.C1C1many2manies);

                StrategyAssert.RolesExistExclusive(from);
                StrategyAssert.AssociationsExistExclusive(to);

                Assert.AreEqual(null, fromProxy.C1AllorsString);
                Assert.AreEqual(null, fromProxy.C1C1one2one);
                Assert.AreEqual(null, fromProxy.C1C1many2one);
                Assert.AreEqual(0, fromProxy.C1C1one2manies.Count);
                Assert.AreEqual(0, fromProxy.C1C1many2manies.Count);

                StrategyAssert.RolesExistExclusive(fromProxy);
                StrategyAssert.AssociationsExistExclusive(toProxy);

                this.Session.Commit();

                //// Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));
                this.Session.Commit();

                sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Rollback();

                sameObject = (C1)this.Session.Instantiate(id);
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Rollback();

                sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                this.Session.Rollback();

                sameObject = (C1)this.Session.Instantiate(id);
                sameObject = (C1)this.Session.Instantiate(id);

                Assert.IsTrue(anObject.Equals(sameObject));
                Assert.IsTrue(anObject.Strategy.ObjectId.Equals(sameObject.Strategy.ObjectId));

                //// Proxy

                //// Unit

                subject = C1.Create(this.Session);
                id = subject.Strategy.ObjectId;
                this.Session.Commit();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                Assert.AreEqual("b", subject.C1AllorsString);
                Assert.AreEqual("b", proxy.C1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                Assert.AreEqual("d", proxy.C1AllorsString);
                Assert.AreEqual("d", subject.C1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                this.Session.Rollback();
                Assert.IsFalse(subject.ExistC1AllorsString);
                Assert.IsFalse(proxy.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                this.Session.Rollback();
                Assert.IsFalse(proxy.ExistC1AllorsString);
                Assert.IsFalse(subject.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                Assert.AreEqual("b", subject.C1AllorsString);
                this.Session.Rollback();
                Assert.IsFalse(proxy.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                Assert.AreEqual("d", proxy.C1AllorsString);
                this.Session.Rollback();
                Assert.IsFalse(subject.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "a";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "b";
                this.Session.Rollback();
                Assert.IsFalse(subject.ExistC1AllorsString);
                this.Session.Rollback();
                Assert.IsFalse(proxy.ExistC1AllorsString);
                this.Session.Rollback();

                subject.C1AllorsString = "c";
                proxy = C1.Instantiate(this.Session, id);
                proxy.C1AllorsString = "d";
                this.Session.Rollback();
                Assert.IsFalse(proxy.ExistC1AllorsString);
                this.Session.Rollback();
                Assert.IsFalse(subject.ExistC1AllorsString);
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

                Assert.AreEqual("a", fromProxy.C1AllorsString);
                Assert.AreEqual(toProxy, fromProxy.C1C1one2one);
                Assert.AreEqual(toProxy, fromProxy.C1C1many2one);
                Assert.Contains(toProxy, (C1[])fromProxy.C1C1one2manies);
                Assert.Contains(toProxy, (C1[])fromProxy.C1C1many2manies);

                Assert.AreEqual(fromProxy, toProxy.C1WhereC1C1one2one);
                Assert.Contains(fromProxy, (C1[])toProxy.C1sWhereC1C1many2one);
                Assert.AreEqual(fromProxy, toProxy.C1WhereC1C1one2many);
                Assert.Contains(fromProxy, (C1[])toProxy.C1sWhereC1C1many2many);

                this.Session.Rollback();

                Assert.AreEqual(null, from.C1AllorsString);
                Assert.AreEqual(null, from.C1C1one2one);
                Assert.AreEqual(null, from.C1C1many2one);
                Assert.IsEmpty((C1[])from.C1C1one2manies);
                Assert.IsEmpty((C1[])from.C1C1many2manies);

                StrategyAssert.RolesExistExclusive(from);
                StrategyAssert.AssociationsExistExclusive(to);

                Assert.AreEqual(null, fromProxy.C1AllorsString);
                Assert.AreEqual(null, fromProxy.C1C1one2one);
                Assert.AreEqual(null, fromProxy.C1C1many2one);
                Assert.AreEqual(0, fromProxy.C1C1one2manies.Count);
                Assert.AreEqual(0, fromProxy.C1C1many2manies.Count);

                StrategyAssert.RolesExistExclusive(fromProxy);
                StrategyAssert.AssociationsExistExclusive(toProxy);

                this.Session.Rollback();

                C1 unexistingObject = (C1)this.Session.Instantiate("1000000");
                Assert.IsNull(unexistingObject);
            }
        }

        [Test]
        public void InstantiateMany()
        {
            foreach (var init in this.Inits)
            {
                init();

int[] runs = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };

                foreach (int run in runs)
                {
                    // Empty arrays
                    long[] nullObjectIdArray = null;
                    var allorsObjects = this.Session.Instantiate(nullObjectIdArray);
                    Assert.AreEqual(0, allorsObjects.Length);

                    string[] nullStringArray = null;
                    allorsObjects = this.Session.Instantiate(nullStringArray);
                    Assert.AreEqual(0, allorsObjects.Length);

                    IObject[] nullObjectArray = null;
                    allorsObjects = this.Session.Instantiate(nullObjectArray);
                    Assert.AreEqual(0, allorsObjects.Length);

                    var objects = new IObject[run];
                    var idStrings = new string[run];
                    var ids = new long[run];
                    for (int i = 0; i < run; i++)
                    {
                        C1 anObject = C1.Create(this.Session);
                        objects[i] = anObject;
                        idStrings[i] = anObject.Strategy.ObjectId.ToString();
                        ids[i] = anObject.Strategy.ObjectId;
                    }

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate(objects);

                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.AreEqual(allorsObjects[i].Id, ids[i]);
                    }

                    allorsObjects = this.Session.Instantiate(idStrings);

                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.AreEqual(allorsObjects[i].Id, ids[i]);
                    }

                    allorsObjects = this.Session.Instantiate(ids);

                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.AreEqual(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate(objects);

                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.AreEqual(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate(idStrings);

                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.AreEqual(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Commit();

                    allorsObjects = this.Session.Instantiate(ids);

                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.AreEqual(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Rollback();

                    allorsObjects = this.Session.Instantiate(objects);

                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.AreEqual(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Rollback();

                    allorsObjects = this.Session.Instantiate(idStrings);

                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.AreEqual(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Rollback();

                    allorsObjects = this.Session.Instantiate(ids);

                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.AreEqual(allorsObjects[i].Id, ids[i]);
                    }

                    this.Session.Commit();

                    // Caching in Sql
                    this.SwitchDatabase();
                    var minusOne = new List<long>(ids);
                    minusOne.RemoveAt(0);
                    allorsObjects = this.Session.Instantiate(minusOne.ToArray());

                    allorsObjects = this.Session.Instantiate(ids);
                    Assert.AreEqual(run, allorsObjects.Length);
                    for (int i = 0; i < allorsObjects.Length; i++)
                    {
                        Assert.IsTrue(ids.Contains(allorsObjects[i].Id));
                    }

                    this.Session.Commit();

                    Assert.IsEmpty(this.Session.Instantiate(new IObject[0]));

                    this.Session.Commit();

                    Assert.IsEmpty(this.Session.Instantiate(new string[0]));

                    this.Session.Commit();

                    Assert.IsEmpty(this.Session.Instantiate(new long[0]));

                    this.Session.Commit();

                    string[] doesntExistIds = new[] { (1000 * 1000 * 1000).ToString() };

                    Assert.AreEqual(0, this.Session.Instantiate(doesntExistIds).Length);
                }
            }
        }

        [Test]
        public void IsDeleted()
        {
            foreach (var init in this.Inits)
            {
                init();

                //// Commit + Commit

                C1 anObject = C1.Create(this.Session);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                //// Commit + Rollback

                anObject = C1.Create(this.Session);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                //// Rollback + Commit

                anObject = C1.Create(this.Session);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                //// Rollback + Rollback

                anObject = C1.Create(this.Session);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                //// With Delete

                //// Commit + Commit

                anObject = C1.Create(this.Session);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                //// Commit + Rollback

                anObject = C1.Create(this.Session);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                //// Rollback + Commit

                anObject = C1.Create(this.Session);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Commit();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                //// Rollback + Rollback

                anObject = C1.Create(this.Session);
                Assert.IsFalse(anObject.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(anObject.Strategy.IsDeleted);

                //// Proxy

                //// Without Delete

                //// Commit + Commit

                anObject = C1.Create(this.Session);
                var id = anObject.Strategy.ObjectId;
                C1 proxy = C1.Instantiate(this.Session, id);
                Assert.IsFalse(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsFalse(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsFalse(proxy.Strategy.IsDeleted);

                //// Commit + Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);
                Assert.IsFalse(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsFalse(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsFalse(proxy.Strategy.IsDeleted);

                //// Rollback + Commit

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);
                Assert.IsFalse(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                //// Rollback + Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);
                Assert.IsFalse(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                //// With Delete

                //// Commit + Commit

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                Assert.IsFalse(proxy.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                //// Commit + Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                Assert.IsFalse(proxy.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                //// Rollback + Commit

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                Assert.IsFalse(proxy.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Commit();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                //// Rollback + Rollback

                anObject = C1.Create(this.Session);
                id = anObject.Strategy.ObjectId;
                proxy = C1.Instantiate(this.Session, id);

                Assert.IsFalse(proxy.Strategy.IsDeleted);

                anObject.Strategy.Delete();

                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);

                this.Session.Rollback();
                anObject = C1.Instantiate(this.Session, id);
                Assert.IsTrue(proxy.Strategy.IsDeleted);
            }
        }

        [Test]
        public void OnChange()
        {
            // TODO:
        }

        [Test]
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

                Assert.AreEqual(ObjectCount, allorsObjects.Length);

                this.Session.Rollback();

                allorsObjects = this.Session.Instantiate(ids);

                Assert.AreEqual(0, allorsObjects.Length);
            }
        }

        [Test]
        public void Versioning()
        {
            foreach (var init in this.Inits)
            {
                init();

                var obj = this.Session.Create<C1>();
                
                Assert.AreEqual("0", obj.Strategy.ObjectVersion.ToString());

                this.Session.Commit();

                Assert.AreEqual("0", obj.Strategy.ObjectVersion.ToString());

                obj.C1AllorsString = "Changed";

                Assert.AreEqual("0", obj.Strategy.ObjectVersion.ToString());

                this.Session.Commit();

                Assert.AreEqual("1", obj.Strategy.ObjectVersion.ToString());

                obj.C1AllorsString = "Changed again.";

                Assert.AreEqual("1", obj.Strategy.ObjectVersion.ToString());

                this.Session.Commit();

                Assert.AreEqual("2", obj.Strategy.ObjectVersion.ToString());

                obj.RemoveC1AllorsString();

                Assert.AreEqual("2", obj.Strategy.ObjectVersion.ToString());

                this.Session.Commit();

                Assert.AreEqual("3", obj.Strategy.ObjectVersion.ToString());
            }
        }

        [Test]
        public void WihtoutRoles()
        {
            foreach (var init in this.Inits)
            {
                init();

                // TODO: Move to other tests
                var withoutValueRoles = ClassWithoutUnitRoles.Create(this.Session);
                var withoutValueRolesClone = (ClassWithoutUnitRoles)this.GetExtent(MetaClassWithoutUnitRoles.Instance.ObjectType)[0];

                Assert.AreEqual(withoutValueRoles, withoutValueRolesClone);

                var withoutRoles = ClassWithoutRoles.Create(this.Session);
                var withoutRolesClone = (ClassWithoutRoles)this.GetExtent(MetaClassWithoutRoles.Instance.ObjectType)[0];

                Assert.AreEqual(withoutRoles, withoutRolesClone);
            }
        }

        [Test]
        public void SwitchPopulation()
        {
            foreach (var init in this.Inits)
            {
                init();

                C1 c1A = C1.Create(this.Session);
                C2 c2A = C2.Create(this.Session);
                C2 c2B = C2.Create(this.Session);
                C2 c2C = C2.Create(this.Session);

                var c1aId = c1A.Id.ToString();

                c1A.C1I12one2one = c2A;
                c1A.AddC1I12one2many(c2B);
                c1A.AddC1I12one2many(c2C);

                this.Session.Commit();

                this.SwitchDatabase();

                var switchC1A = (C1)this.Session.Instantiate(c1A.Id.ToString());

                Assert.AreEqual(MetaC1.Instance.ObjectType, switchC1A.Strategy.Class);

                var switchC2A = switchC1A.C1I12one2one;

                Assert.AreEqual(MetaC2.Instance.ObjectType, switchC2A.Strategy.Class);

                var switchC2BC = switchC1A.C1I12one2manies;

                Assert.AreEqual(MetaC2.Instance.ObjectType, switchC2BC[0].Strategy.Class);
                Assert.AreEqual(MetaC2.Instance.ObjectType, switchC2BC[1].Strategy.Class);

                this.Session.Commit();

                this.SwitchDatabase();

                long[] objectIds = { c1A.Id, c2A.Id };
                var switchC1aC2a = this.Session.Instantiate(objectIds);

                Assert.AreEqual(2, switchC1aC2a.Length);
                Assert.Contains(this.Session.Instantiate(c1A.Id), switchC1aC2a);
                Assert.Contains(this.Session.Instantiate(c2A.Id), switchC1aC2a);
            }
        }

        [Test]
        public void SwitchSession()
        {
            foreach (var init in this.Inits)
            {
                init();

                if (this.Session is ISession)
                {
                    C1 c1A = C1.Create(this.Session);
                    C2 c2A = C2.Create(this.Session);

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

                        Assert.IsTrue(c2A.ExistC1WhereC1C2one2one);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c2A = (C2)session2.Instantiate(c2AObjectId);

                        Assert.IsTrue(c2A.ExistC1sWhereC1C2many2one);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c2A = (C2)session2.Instantiate(c2AObjectId);

                        Assert.IsTrue(c2A.ExistC1WhereC1C2one2many);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c2A = (C2)session2.Instantiate(c2AObjectId);

                        Assert.IsTrue(c2A.ExistC1sWhereC1C2many2many);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c1A = (C1)session2.Instantiate(c1AObjectId);

                        Assert.IsTrue(c1A.ExistC1C2one2one);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c1A = (C1)session2.Instantiate(c1AObjectId);

                        Assert.IsTrue(c1A.ExistC1C2many2one);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c1A = (C1)session2.Instantiate(c1AObjectId);

                        Assert.IsTrue(c1A.ExistC1C2one2manies);
                    }

                    using (var session2 = this.CreateSession())
                    {
                        c1A = (C1)session2.Instantiate(c1AObjectId);

                        Assert.IsTrue(c1A.ExistC1C2many2manies);
                    }
                }
            }
        }
        
        [Test]
        public virtual void CreateManyPopulations()
        {
            foreach (var init in this.Inits)
            {
                init();

                // don't garbage collect populations
                var populations = new List<IDatabase>();

                for (int i = 0; i < 100; i++)
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

        [Test]
        public void ObjectType()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1a = this.Session.Create(MetaC1.Instance.ObjectType);
                Assert.AreEqual(MetaC1.Instance.ObjectType, c1a.Strategy.Class);

                this.Session.Commit();

                Assert.AreEqual(MetaC1.Instance.ObjectType, c1a.Strategy.Class);

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

                Assert.IsTrue(exceptionThrown);

                var c2a = this.Session.Create(MetaC2.Instance.ObjectType);
                Assert.AreEqual(MetaC2.Instance.ObjectType, c2a.Strategy.Class);

                this.Session.Commit();

                Assert.AreEqual(MetaC2.Instance.ObjectType, c2a.Strategy.Class);
            }
        }

        [Test]
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

        [Test]
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

                    Assert.AreEqual("1", c1A.C1AllorsString);

                    this.Session.Commit();

                    Assert.AreEqual("1", c1A.C1AllorsString);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.AreEqual("1", c1A.C1AllorsString);

                    this.Session.Commit();

                    c1A.C1AllorsString = "2";

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.AreEqual("2", c1A.C1AllorsString);

                    this.Session.Rollback();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1AllorsString }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.AreEqual("1", c1A.C1AllorsString);

                    this.Session.Rollback();

                    Assert.AreEqual("1", c1A.C1AllorsString);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(c2A, c1A.C1C2one2one);

                    this.Session.Commit();

                    Assert.AreEqual(c2A, c1A.C1C2one2one);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.AreEqual(c2A, c1A.C1C2one2one);

                    this.Session.Commit();

                    c1A.C1C2one2one = c2b;

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.AreEqual(c2b, c1A.C1C2one2one);

                    this.Session.Rollback();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2one }, new[] { c1A.Strategy.ObjectId });
                    }

                    Assert.AreEqual(c2A, c1A.C1C2one2one);

                    this.Session.Rollback();

                    Assert.AreEqual(c2A, c1A.C1C2one2one);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(c2a, c1a.C1C2one2one);

                    this.Session.Commit();

                    Assert.AreEqual(c2a, c1a.C1C2one2one);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c1a);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1a);
                    }

                    Assert.AreEqual(c2a, c1a.C1C2one2one);

                    this.Session.Commit();

                    c1a.C1C2one2one = c2b;

                    this.Session.Prefetch(prefetchPolicy, c1a);

                    Assert.AreEqual(c2b, c1a.C1C2one2one);

                    this.Session.Rollback();

                    this.Session.Prefetch(prefetchPolicy, c1a);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1a);
                    }

                    Assert.AreEqual(c2a, c1a.C1C2one2one);

                    this.Session.Rollback();

                    Assert.AreEqual(c2a, c1a.C1C2one2one);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(0, c1A.C1C2one2manies.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1A);
                    }

                    Assert.AreEqual(0, c1A.C1C2one2manies.Count);
                }
            }
        }

        [Test]
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

        [Test]
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

                    Assert.AreEqual(0, c1A.C1C2one2manies.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2one2manies }, c1A);
                    }

                    Assert.AreEqual(0, c1A.C1C2one2manies.Count);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(0, c1A.C1C2one2manies.Count);
                    
                    this.Session.Prefetch(prefetchPolicy, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1A);
                    }

                    Assert.AreEqual(0, c1A.C1C2one2manies.Count);
                }
            }
        }

        [Test]
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

        [Test]
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

                    Assert.AreEqual(0, c1A.C1C2many2manies.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C1.Meta.C1C2many2manies }, c1A);
                    }

                    Assert.AreEqual(0, c1A.C1C2many2manies.Count);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(0, c1A.C1C2many2manies.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c1A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c1A);
                    }

                    Assert.AreEqual(0, c1A.C1C2many2manies.Count);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(c1a, c2a.C1WhereC1C2one2one);

                    this.Session.Commit();

                    Assert.AreEqual(c1a, c2a.C1WhereC1C2one2one);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    }

                    Assert.AreEqual(c1a, c2a.C1WhereC1C2one2one);

                    this.Session.Commit();

                    c1b.C1C2one2one = c2a;

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    }

                    Assert.AreEqual(c1b, c2a.C1WhereC1C2one2one);

                    this.Session.Rollback();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, new[] { c2a.Strategy.ObjectId });
                    }

                    Assert.AreEqual(c1a, c2a.C1WhereC1C2one2one);

                    this.Session.Rollback();

                    Assert.AreEqual(c1a, c2a.C1WhereC1C2one2one);
                }
            }
        }

        [Test]
        public void PrefetchAssociationOne2OneEmpty()
        {
            foreach (var twice in TrueFalse)
            {
                foreach (var init in this.Inits)
                {
                    init();

                    var c2A = C2.Create(this.Session);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one}, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, c2A);
                    }

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2many.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1WhereC1C2one2one }, c2A);
                    }

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2many.Count);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2many.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c2A);
                    }

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2many.Count);
                }
            }
        }

        [Test]
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

        [Test]
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

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2many.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2many }, c2A);
                    }

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2many.Count);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2many.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c2A);
                    }

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2many.Count);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2one.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2one }, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(new IPropertyType[] { C2.Meta.C1sWhereC1C2many2one }, c2A);
                    }

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2one.Count);
                }
            }
        }

        [Test]
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

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2one.Count);

                    this.Session.Commit();

                    this.Session.Prefetch(prefetchPolicy, c2A);
                    if (twice)
                    {
                        this.Session.Prefetch(prefetchPolicy, c2A);
                    }

                    Assert.AreEqual(0, c2A.C1sWhereC1C2many2one.Count);
                }
            }
        }

        protected abstract void SwitchDatabase();

        protected abstract IDatabase CreatePopulation();

        protected abstract ISession CreateSession();

        private IObject[] GetExtent(IComposite objectType)
        {
            return this.Session.Extent(objectType);
        }
    }
}
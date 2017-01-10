// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheTest.cs" company="Allors bvba">
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
    using Allors;

    using Domain;

    using NUnit.Framework;

    using IDatabase = IDatabase;

    [TestFixture]
    public abstract class CacheTest
    {
        [Test]
        [Ignore("Cache invalidation")]
        public void InitDifferentDatabase()
        {
            var database = this.CreateDatabase();
            database.Init();

            using (ISession session = database.CreateSession())
            {
                var c1 = C1.Create(session);
                c1.C1AllorsString = "a";
                session.Commit();
            }

            using (ISession session = database.CreateSession())
            {
                var c1 = session.Extent<C1>().First;
                Assert.AreEqual("a", c1.C1AllorsString);
            }

            database.Init();

            var database2 = this.CreateDatabase();

            using (ISession session = database.CreateSession())
            {
                var c1 = C1.Create(session);
                c1.C1AllorsString = "b";
                session.Commit();
            }

            using (ISession session = database2.CreateSession())
            {
                var c1 = session.Extent<C1>().First;
                c1.C1AllorsString = "c";
            }

            using (ISession session = database.CreateSession())
            {
                var c1 = session.Extent<C1>().First;
                Assert.AreEqual("c", c1.C1AllorsString);
            }
        }
        
        [Test]
        public void FlushCacheOnInit()
        {
            var database = this.CreateDatabase();
            database.Init();

            using (ISession session = database.CreateSession())
            {
                var c1a = C1.Create(session);
                var c2a = C2.Create(session);
                c1a.C1C2one2one = c2a;
                session.Commit();

                // load cache
                c2a = c1a.C1C2one2one;
            }
            
            database.Init();

            using (ISession session = database.CreateSession())
            {
                var c1a = C1.Create(session);
                var c1b = C1.Create(session);

                session.Commit();

                c1a = C1.Instantiate(session, c1a.Id);

                Assert.IsNull(c1a.C1C2one2one);
            }
        }

        [Test]
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


        protected abstract IDatabase CreateDatabase();
    }
}
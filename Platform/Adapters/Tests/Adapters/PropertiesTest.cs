//------------------------------------------------------------------------------------------------- 
// <copyright file="PropertiesTest.cs" company="Allors bvba">
// Copyright 2002-2012 Allors bvba.
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
// <summary>Defines the Default type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.Adapters
{
    using System;

    using Allors;

    using Allors.Domain;
    using Adapters;

    using NUnit.Framework;

    public abstract class PropertiesTest
    {
        private int count;

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
        public void SessionEvent()
        {
            foreach (var init in this.Inits)
            {
                init();

                var c1 = C1.Create(this.Session);
                var subscriber = new Subscriber();
                this.Session["subscriber"] = subscriber;
                c1.Broadcast("Hello Subscriber");

                Assert.AreEqual("Hello Subscriber", subscriber.Message);
            }
        }

        [Test]
        public void InitDatabase()
        {
            foreach (var init in this.Inits)
            {
                init();

                var database = this.Session.Database;

                Assert.IsNull(database["key"]);

                database["key"] = "value";

                database.Init();

                Assert.IsNull(database["key"]);
            }
        }
        

        [Test]
        public void InitSession()
        {
            foreach (var init in this.Inits)
            {
                init();

                var database = this.Session.Database;

                Assert.IsNull(this.Session["key"]);

                this.Session["key"] = "value";

                database.Init();

                using (var newSession = database.CreateSession())
                {
                    Assert.IsNull(newSession["key"]);
                }
            }
        }
    }
}
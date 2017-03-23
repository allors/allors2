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

    using Allors.Domain;
    using Adapters;

    using Allors.Meta;

    using Xunit;

    public abstract class SandboxTest
    {
        protected abstract IProfile Profile { get; }

        protected IDatabase Population => this.Profile.Database;

        protected ISession Session => this.Profile.Session;

        protected Action[] Markers => this.Profile.Markers;

        protected Action[] Inits => this.Profile.Inits;

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
    }
}
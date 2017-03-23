// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlClientTest.cs" company="Allors bvba">
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

using Allors.Domain;

namespace Allors.Adapters.Relation.SqlClient
{
    using System;

    using Allors;
    using Allors.Meta;
    using Adapters;

    using Domain;

    using Xunit;

    
    public abstract class SqlClientTest
    {
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

        [Fact]
        public void Bulk()
        {
            foreach (var init in this.Inits)
            {
                init();

                var count = Settings.LargeArraySize;

                using (var session = this.CreateSession())
                {
                    var c1s = (Extent<C1>)session.Create(MetaC1.Instance.ObjectType, count);
                    var c2s = (Extent<C2>)session.Create(MetaC2.Instance.ObjectType, count);

                    for (var i = 0; i < count; i++)
                    {
                        var c1 = c1s[i];

                        c1.C1C2many2one = c2s[i];

                        for (var j = 0; j < 10; j++)
                        {
                            var c2 = c2s[j];
                            c1.AddC1C2many2many(c2);
                        }
                    }

                    session.Commit();
                }
            }
        }

        protected ISession CreateSession()
        {
            return this.Profile.Database.CreateSession();
        }
    }
}
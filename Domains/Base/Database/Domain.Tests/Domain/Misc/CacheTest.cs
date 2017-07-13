// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheTest.cs" company="Allors bvba">
//   Copyright 2002-2011 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using Allors;

    using Xunit;

    using global::Allors.Domain;

    
    public class CacheTest : DomainTest
    {
        [Fact]
        public void Default()
        {
            var existingOrganisation = new OrganisationBuilder(this.Session).WithName("existing organisation").Build();

            this.Session.Derive(true);
            this.Session.Commit();

            var sessions = new ISession[] { this.Session };
            foreach (var session in sessions)
            {
                session.Commit();

                var cachedOrganisation = new Organisations(session).Cache.Get(existingOrganisation.UniqueId);
                Assert.Equal(existingOrganisation.UniqueId, cachedOrganisation.UniqueId);
                Assert.Same(session, cachedOrganisation.Strategy.Session);

                var newOrganisation = new OrganisationBuilder(session).WithName("new organisation").Build();
                cachedOrganisation = new Organisations(session).Cache.Get(newOrganisation.UniqueId);
                Assert.Equal(newOrganisation.UniqueId, cachedOrganisation.UniqueId);
                Assert.Same(session, cachedOrganisation.Strategy.Session);

                session.Rollback();
            }
        }
    }
}


// <copyright file="CacheTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using Allors;

    using Xunit;

    using Allors.Domain;

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

                var cachedOrganisation = new Organisations(session).Sticky[existingOrganisation.UniqueId];
                Assert.Equal(existingOrganisation.UniqueId, cachedOrganisation.UniqueId);
                Assert.Same(session, cachedOrganisation.Strategy.Session);

                var newOrganisation = new OrganisationBuilder(session).WithName("new organisation").Build();
                cachedOrganisation = new Organisations(session).Sticky[newOrganisation.UniqueId];
                Assert.Equal(newOrganisation.UniqueId, cachedOrganisation.UniqueId);
                Assert.Same(session, cachedOrganisation.Strategy.Session);

                session.Rollback();
            }
        }
    }
}

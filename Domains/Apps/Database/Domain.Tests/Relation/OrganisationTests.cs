// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using Meta;
    using Xunit;

    
    public class OrganisationTests : DomainTest
    {
        [Fact]
        public void GivenOrganisation_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrganisationBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("Organisation");
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrganisation_WhenActiveContactRelationship_ThenOrganisationCurrentOrganisationContactRelationshipsContainsOrganisation()
        {
            var contact = new PersonBuilder(this.DatabaseSession).WithLastName("organisationContact").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            var organisation = new OrganisationBuilder(this.DatabaseSession).WithName("organisation").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

            

            new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithContact(contact)
                .WithOrganisation(organisation)
                .WithFromDate(DateTime.UtcNow.Date)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(contact.CurrentOrganisationContactRelationships[0].Organisation, organisation);
            Assert.Equal(0, contact.InactiveOrganisationContactRelationships.Count);
        }

        [Fact]
        public void GivenOrganisation_WhenInActiveContactRelationship_ThenOrganisationnactiveOrganisationContactRelationshipsContainsOrganisation()
        {
            var contact = new PersonBuilder(this.DatabaseSession).WithLastName("organisationContact").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            var organisation = new OrganisationBuilder(this.DatabaseSession).WithName("organisation").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

            

            new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithContact(contact)
                .WithOrganisation(organisation)
                .WithFromDate(DateTime.UtcNow.Date.AddDays(-1))
                .WithThroughDate(DateTime.UtcNow.Date.AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(contact.InactiveOrganisationContactRelationships[0].Organisation, organisation);
            Assert.Equal(0, contact.CurrentOrganisationContactRelationships.Count);
        }

    }
}

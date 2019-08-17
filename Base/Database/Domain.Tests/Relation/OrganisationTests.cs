// <copyright file="OrganisationTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Allors.Domain
{
    using Xunit;

    public class OrganisationTests : DomainTest
    {
        [Fact]
        public void GivenOrganisation_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrganisationBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("Organisation");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrganisation_WhenActiveContactRelationship_ThenOrganisationCurrentOrganisationContactRelationshipsContainsOrganisation()
        {
            var contact = new PersonBuilder(this.Session).WithLastName("organisationContact").Build();
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(organisation)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            new OrganisationContactRelationshipBuilder(this.Session)
                .WithContact(contact)
                .WithOrganisation(organisation)
                .WithFromDate(this.Session.Now().Date)
                .Build();

            this.Session.Derive();

            Assert.Equal(contact.CurrentOrganisationContactRelationships[0].Organisation, organisation);
            Assert.Equal(0, contact.InactiveOrganisationContactRelationships.Count);
        }

        [Fact]
        public void GivenOrganisation_WhenInActiveContactRelationship_ThenOrganisationnactiveOrganisationContactRelationshipsContainsOrganisation()
        {
            var contact = new PersonBuilder(this.Session).WithLastName("organisationContact").Build();
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(organisation)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            new OrganisationContactRelationshipBuilder(this.Session)
                .WithContact(contact)
                .WithOrganisation(organisation)
                .WithFromDate(this.Session.Now().Date.AddDays(-1))
                .WithThroughDate(this.Session.Now().Date.AddDays(-1))
                .Build();

            this.Session.Derive();

            Assert.Equal(contact.InactiveOrganisationContactRelationships[0].Organisation, organisation);
            Assert.Equal(0, contact.CurrentOrganisationContactRelationships.Count);
        }
    }
}

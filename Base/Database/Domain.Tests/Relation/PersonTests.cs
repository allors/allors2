// <copyright file="PersonTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Xunit;

    public class PersonTests : DomainTest
    {
        [Fact]
        public void GivenPerson_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new PersonBuilder(this.Session);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPerson_WhenEmployed_ThenIsEmployeeEqualsTrue()
        {
            var employment = new EmploymentBuilder(this.Session)
                .WithEmployee(this.Purchaser)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.True(this.Purchaser.BaseIsActiveEmployee(this.Session.Now()));
        }

        [Fact]
        public void GivenPerson_WhenActiveContactRelationship_ThenPersonCurrentOrganisationContactRelationshipsContainsPerson()
        {
            var contact = new PersonBuilder(this.Session).WithLastName("organisationContact").Build();
            var organisation = new OrganisationBuilder(this.Session).WithName("organisation").Build();

            new OrganisationContactRelationshipBuilder(this.Session)
                .WithContact(contact)
                .WithOrganisation(organisation)
                .WithFromDate(this.Session.Now().Date)
                .Build();

            this.Session.Derive();

            Assert.Equal(contact, contact.CurrentOrganisationContactRelationships[0].Contact);
            Assert.Empty(contact.InactiveOrganisationContactRelationships);
        }

        [Fact]
        public void GivenPerson_WhenInActiveContactRelationship_ThenPersonInactiveOrganisationContactRelationshipsContainsPerson()
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

            Assert.Equal(contact, contact.InactiveOrganisationContactRelationships[0].Contact);
            Assert.Empty(contact.CurrentOrganisationContactRelationships);
        }

        [Fact]
        public void GivenPerson_WhenEmployed_ThenTimeSheetSynced()
        {
            var person = new PersonBuilder(this.Session).WithFirstName("Good").WithLastName("Employee").Build();
            var employer = new InternalOrganisations(this.Session).Extent().First;

            var employment = new EmploymentBuilder(this.Session)
                .WithEmployee(person)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.NotNull(person.TimeSheetWhereWorker);
        }

        [Fact]
        public void GivenPerson_WhenSubContractor_ThenTimeSheetSynced()
        {
            var subContractor = this.InternalOrganisation.CreateSubContractor(this.Session.Faker());

            this.Session.Derive();
            this.Session.Commit();

            var organisationContactRelationship = subContractor.OrganisationContactRelationshipsWhereOrganisation.First();
            var contact = organisationContactRelationship.Contact;

            Assert.NotNull(contact.TimeSheetWhereWorker);
        }

        [Fact]
        public void GivenPerson_WhenInContactRelationship_ThenCurrentOrganisationContactMechanismsIsDerived()
        {
            var contact = new PersonBuilder(this.Session).WithLastName("organisationContact").Build();
            var organisation1 = new OrganisationBuilder(this.Session).WithName("organisation1").Build();
            var organisation2 = new OrganisationBuilder(this.Session).WithName("organisation2").Build();

            // Even when relationship is inactive CurrentOrganisationContactMechanisms is maintained
            new OrganisationContactRelationshipBuilder(this.Session)
                .WithContact(contact)
                .WithOrganisation(organisation1)
                .WithFromDate(this.Session.Now().Date.AddDays(-1))
                .WithThroughDate(this.Session.Now().Date.AddDays(-1))
                .Build();

            var contactMechanism1 = new TelecommunicationsNumberBuilder(this.Session).WithAreaCode("111").WithContactNumber("222").Build();
            var partyContactMechanism1 = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(contactMechanism1).Build();
            organisation1.AddPartyContactMechanism(partyContactMechanism1);

            this.Session.Derive();

            Assert.Single(contact.CurrentOrganisationContactMechanisms);
            Assert.Contains(contactMechanism1, contact.CurrentOrganisationContactMechanisms);

            partyContactMechanism1.ThroughDate = partyContactMechanism1.FromDate;

            this.Session.Derive();

            Assert.Empty(contact.CurrentOrganisationContactMechanisms);

            partyContactMechanism1.RemoveThroughDate();

            new OrganisationContactRelationshipBuilder(this.Session)
                .WithContact(contact)
                .WithOrganisation(organisation2)
                .WithFromDate(this.Session.Now().Date.AddDays(-1))
                .Build();

            var contactMechanism2 = new TelecommunicationsNumberBuilder(this.Session).WithAreaCode("222").WithContactNumber("333").Build();
            var partyContactMechanism2 = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(contactMechanism2).Build();
            organisation2.AddPartyContactMechanism(partyContactMechanism2);

            this.Session.Derive();

            Assert.Equal(2, contact.CurrentOrganisationContactMechanisms.Count);
            Assert.Contains(contactMechanism1, contact.CurrentOrganisationContactMechanisms);
            Assert.Contains(contactMechanism2, contact.CurrentOrganisationContactMechanisms);
        }
    }

    [Trait("Category", "Security")]
    public class PersonSecurityTests : DomainTest
    {
        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenLoggedUserIsAdministrator_WhenAccessingSingleton_ThenLoggedInUserIsGrantedAccess()
        {
            var existingAdministrator = this.Administrator;
            var secondAdministrator = new PersonBuilder(this.Session).WithLastName("second admin").Build();
            Assert.False(secondAdministrator.IsAdministrator());

            var internalOrganisation = this.InternalOrganisation;

            this.Session.Derive();

            User user = this.Administrator;
            this.Session.SetUser(user);

            var acl = new DatabaseAccessControlLists(existingAdministrator)[internalOrganisation];
            Assert.True(acl.CanRead(M.Organisation.Name));

            acl = new DatabaseAccessControlLists(existingAdministrator)[internalOrganisation];
            Assert.True(acl.CanWrite(M.Organisation.Name));

            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(secondAdministrator);

            this.Session.Derive();

            Assert.True(secondAdministrator.IsAdministrator());

            acl = new DatabaseAccessControlLists(existingAdministrator)[internalOrganisation];
            Assert.True(acl.CanRead(M.Organisation.Name));

            acl = new DatabaseAccessControlLists(existingAdministrator)[internalOrganisation];
            Assert.True(acl.CanWrite(M.Organisation.Name));
        }
    }
}

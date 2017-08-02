//------------------------------------------------------------------------------------------------- 
// <copyright file="OrganisationContactRelationshipTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using Meta;
    using Xunit;

    
    public class OrganisationContactRelationshipTests : DomainTest
    {
        private Person contact;
        private OrganisationContactRelationship organisationContactRelationship;
        
        public OrganisationContactRelationshipTests()
        {
            this.contact = new PersonBuilder(this.DatabaseSession).WithLastName("organisationContact").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            this.organisationContactRelationship = new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithContact(this.contact)
                .WithOrganisation(new Organisations(this.DatabaseSession).FindBy(M.Organisation.Name, "customer"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
        }

        [Fact]
        public void GivenorganisationContactRelationship_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var contact = new PersonBuilder(this.DatabaseSession).WithLastName("organisationContact").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var builder = new OrganisationContactRelationshipBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithContact(contact);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithOrganisation(new OrganisationBuilder(this.DatabaseSession).WithName("organisation").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build());
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPerson_WhenFirstContactForOrganisationIsCreated_ThenContactUserGroupIsCreated()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var usergroup = this.organisationContactRelationship.Organisation.ContactsUserGroup;
            Assert.True(usergroup.Members.Contains(this.organisationContactRelationship.Contact));
        }

        [Fact]
        public void GivenNextPerson_WhenContactForOrganisationIsCreated_ThenContactIsAddedToUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var usergroup = this.organisationContactRelationship.Organisation.ContactsUserGroup;
            Assert.Equal(1, usergroup.Members.Count);
            Assert.True(usergroup.Members.Contains(this.organisationContactRelationship.Contact));

            var secondRelationship = new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithContact(new PersonBuilder(this.DatabaseSession).WithLastName("contact 2").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build())
                .WithOrganisation(new Organisations(this.DatabaseSession).FindBy(M.Organisation.Name, "customer"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(2, usergroup.Members.Count);
            Assert.True(usergroup.Members.Contains(secondRelationship.Contact));
        }

        [Fact]
        public void GivenOrganisationContactRelationship_WhenRelationshipPeriodIsNotValid_ThenContactIsNotInCustomerContactUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var usergroup = this.organisationContactRelationship.Organisation.ContactsUserGroup;

            Assert.Equal(1, usergroup.Members.Count);
            Assert.True(usergroup.Members.Contains(this.contact));

            this.organisationContactRelationship.FromDate = DateTime.UtcNow.AddDays(+1);
            this.organisationContactRelationship.RemoveThroughDate();

            this.DatabaseSession.Derive();

            Assert.Equal(0, usergroup.Members.Count);

            this.organisationContactRelationship.FromDate = DateTime.UtcNow;
            this.organisationContactRelationship.RemoveThroughDate();

            this.DatabaseSession.Derive();

            Assert.Equal(1, usergroup.Members.Count);
            Assert.True(usergroup.Members.Contains(this.contact));

            this.organisationContactRelationship.FromDate = DateTime.UtcNow.AddDays(-2);
            this.organisationContactRelationship.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive();

            Assert.Equal(0, usergroup.Members.Count);
        }

        private void InstantiateObjects(ISession session)
        {
            this.contact = (Person)session.Instantiate(this.contact);
            this.organisationContactRelationship = (OrganisationContactRelationship)session.Instantiate(this.organisationContactRelationship);
        }
    }
}

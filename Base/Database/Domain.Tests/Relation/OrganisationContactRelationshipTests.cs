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
            this.contact = new PersonBuilder(this.Session).WithLastName("organisationContact").Build();

            this.organisationContactRelationship = new OrganisationContactRelationshipBuilder(this.Session)
                .WithContact(this.contact)
                .WithOrganisation(new Organisations(this.Session).FindBy(M.Organisation.Name, "customer"))
                .WithFromDate(this.Session.Now().AddYears(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void GivenorganisationContactRelationship_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var contact = new PersonBuilder(this.Session).WithLastName("organisationContact").Build();
            this.Session.Derive();
            this.Session.Commit();

            this.InstantiateObjects(this.Session);

            var builder = new OrganisationContactRelationshipBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithContact(contact);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithOrganisation(new OrganisationBuilder(this.Session).WithName("organisation").WithLocale(this.Session.GetSingleton().DefaultLocale).Build());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPerson_WhenFirstContactForOrganisationIsCreated_ThenContactUserGroupIsCreated()
        {
            this.InstantiateObjects(this.Session);

            var usergroup = this.organisationContactRelationship.Organisation.ContactsUserGroup;
            Assert.True(usergroup.Members.Contains(this.organisationContactRelationship.Contact));
        }

        [Fact]
        public void GivenNextPerson_WhenContactForOrganisationIsCreated_ThenContactIsAddedToUserGroup()
        {
            this.InstantiateObjects(this.Session);

            var usergroup = this.organisationContactRelationship.Organisation.ContactsUserGroup;
            Assert.Single(usergroup.Members);
            Assert.True(usergroup.Members.Contains(this.organisationContactRelationship.Contact));

            var secondRelationship = new OrganisationContactRelationshipBuilder(this.Session)
                .WithContact(new PersonBuilder(this.Session).WithLastName("contact 2").Build())
                .WithOrganisation(new Organisations(this.Session).FindBy(M.Organisation.Name, "customer"))
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(2, usergroup.Members.Count);
            Assert.True(usergroup.Members.Contains(secondRelationship.Contact));
        }

        [Fact]
        public void GivenOrganisationContactRelationship_WhenRelationshipPeriodIsNotValid_ThenContactIsNotInCustomerContactUserGroup()
        {
            this.InstantiateObjects(this.Session);

            var usergroup = this.organisationContactRelationship.Organisation.ContactsUserGroup;

            Assert.Single(usergroup.Members);
            Assert.True(usergroup.Members.Contains(this.contact));

            this.organisationContactRelationship.FromDate = this.Session.Now().AddDays(+1);
            this.organisationContactRelationship.RemoveThroughDate();

            this.Session.Derive();

            Assert.Equal(0, usergroup.Members.Count);

            this.organisationContactRelationship.FromDate = this.Session.Now();
            this.organisationContactRelationship.RemoveThroughDate();

            this.Session.Derive();

            Assert.Single(usergroup.Members);
            Assert.True(usergroup.Members.Contains(this.contact));

            this.organisationContactRelationship.FromDate = this.Session.Now().AddDays(-2);
            this.organisationContactRelationship.ThroughDate = this.Session.Now().AddDays(-1);

            this.Session.Derive();

            Assert.Equal(0, usergroup.Members.Count);
        }

        private void InstantiateObjects(ISession session)
        {
            this.contact = (Person)session.Instantiate(this.contact);
            this.organisationContactRelationship = (OrganisationContactRelationship)session.Instantiate(this.organisationContactRelationship);
        }
    }
}

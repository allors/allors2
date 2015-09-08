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

    
    

    using NUnit.Framework;

    [TestFixture]
    public class OrganisationContactRelationshipTests : DomainTest
    {
        private Person contact;
        private OrganisationContactRelationship organisationContactRelationship;

        [SetUp]
        public override void Init()
        {
            base.Init();

            this.contact = new PersonBuilder(this.DatabaseSession).WithLastName("organisationContact").Build();

            this.organisationContactRelationship = new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithContact(this.contact)
                .WithOrganisation(new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
        }

        [Test]
        public void GivenorganisationContactRelationship_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var contact = new PersonBuilder(this.DatabaseSession).WithLastName("organisationContact").Build();
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var builder = new OrganisationContactRelationshipBuilder(this.DatabaseSession);
            var relationship = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithContact(contact);
            relationship = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithOrganisation(new OrganisationBuilder(this.DatabaseSession).WithName("organisation").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build());
            relationship = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenPerson_WhenFirstContactForOrganisationIsCreated_ThenContactUserGroupIsCreated()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var usergroup = this.organisationContactRelationship.Organisation.CustomerContactUserGroup;
            Assert.IsTrue(usergroup.Members.Contains(this.organisationContactRelationship.Contact));
        }

        [Test]
        public void GivenNextPerson_WhenContactForOrganisationIsCreated_ThenContactIsAddedToUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var usergroup = this.organisationContactRelationship.Organisation.CustomerContactUserGroup;
            Assert.AreEqual(1, usergroup.Members.Count);
            Assert.IsTrue(usergroup.Members.Contains(this.organisationContactRelationship.Contact));

            var secondContact = new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithContact(new PersonBuilder(this.DatabaseSession).WithLastName("contact 2").Build())
                .WithOrganisation(new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, usergroup.Members.Count);
            Assert.IsTrue(usergroup.Members.Contains(secondContact.Contact));
        }

        [Test]
        public void GivenOrganisationContactRelationship_WhenRelationshipPeriodIsNotValid_ThenContactIsNotInCustomerContactUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var usergroup = this.organisationContactRelationship.Organisation.CustomerContactUserGroup;

            Assert.AreEqual(1, usergroup.Members.Count);
            Assert.IsTrue(usergroup.Members.Contains(this.contact));

            this.organisationContactRelationship.FromDate = DateTime.UtcNow.AddDays(+1);
            this.organisationContactRelationship.RemoveThroughDate();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, usergroup.Members.Count);

            this.organisationContactRelationship.FromDate = DateTime.UtcNow;
            this.organisationContactRelationship.RemoveThroughDate();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, usergroup.Members.Count);
            Assert.IsTrue(usergroup.Members.Contains(this.contact));

            this.organisationContactRelationship.FromDate = DateTime.UtcNow.AddDays(-2);
            this.organisationContactRelationship.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, usergroup.Members.Count);
        }

        private void InstantiateObjects(ISession session)
        {
            this.contact = (Person)session.Instantiate(this.contact);
            this.organisationContactRelationship = (OrganisationContactRelationship)session.Instantiate(this.organisationContactRelationship);
        }
    }
}

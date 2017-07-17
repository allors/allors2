// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonTests.cs" company="Allors bvba">
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

    
    public class PersonTests : DomainTest
    {
        [Fact]
        public void GivenPerson_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new PersonBuilder(this.DatabaseSession).WithPersonRole(new PersonRoles(this.DatabaseSession).Customer);
            builder.Build();
                
            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPerson_WhenEmployed_ThenCurrentEmploymentIsDerived()
        {
            var salesRep = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();

            var employment = new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(salesRep)
                .WithEmployer(new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(employment, salesRep.CurrentEmployment);
        }

        [Fact]
        public void GivenLoggedUserIsAdministrator_WhenAccessingInternalOrganisation_ThenLoggedInUserIsGrantedAccess()
        {
            var existingAdministrator = new People(this.DatabaseSession).FindBy(M.Person.UserName, Users.AdministratorUserName);
            var secondAdministrator = new PersonBuilder(this.DatabaseSession).WithLastName("second admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            Assert.False(secondAdministrator.IsAdministrator);

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");

            this.DatabaseSession.Derive(true);

            this.SetIdentity(Users.AdministratorUserName);

            var acl = new AccessControlList(internalOrganisation, existingAdministrator);
            Assert.True(acl.CanWrite(M.InternalOrganisation.Name));
            
            acl = new AccessControlList(internalOrganisation, secondAdministrator);
            Assert.False(acl.CanRead(M.InternalOrganisation.Name));

            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(secondAdministrator);

            this.DatabaseSession.Derive(true);

            Assert.True(secondAdministrator.IsAdministrator);

            acl = new AccessControlList(internalOrganisation, secondAdministrator);
            Assert.True(acl.CanWrite(M.InternalOrganisation.Name));
        }

        [Fact]
        public void GivenPerson_WhenActiveContactRelationship_ThenPersonCurrentOrganisationContactRelationshipsContainsPerson()
        {
            var contact = new PersonBuilder(this.DatabaseSession).WithLastName("organisationContact").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            var organisation = new OrganisationBuilder(this.DatabaseSession).WithName("organisation").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation"))
                .WithCustomer(organisation)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithContact(contact)
                .WithOrganisation(organisation)
                .WithFromDate(DateTime.UtcNow.Date)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(contact.CurrentOrganisationContactRelationships[0].Contact, contact);
            Assert.Equal(0, contact.InactiveOrganisationContactRelationships.Count);
        }

        [Fact]
        public void GivenPerson_WhenInActiveContactRelationship_ThenPersonInactiveOrganisationContactRelationshipsContainsPerson()
        {
            var contact = new PersonBuilder(this.DatabaseSession).WithLastName("organisationContact").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            var organisation = new OrganisationBuilder(this.DatabaseSession).WithName("organisation").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation"))
                .WithCustomer(organisation)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithContact(contact)
                .WithOrganisation(organisation)
                .WithFromDate(DateTime.UtcNow.Date.AddDays(-1))
                .WithThroughDate(DateTime.UtcNow.Date.AddDays(-1))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(contact.InactiveOrganisationContactRelationships[0].Contact, contact);
            Assert.Equal(0, contact.CurrentOrganisationContactRelationships.Count);
        }
    }
}

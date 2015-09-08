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

    using NUnit.Framework;

    [TestFixture]
    public class OrganisationTests : DomainTest
    {
        [Test]
        public void GivenOrganisation_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrganisationBuilder(this.DatabaseSession);
            var organisation = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("Organisation");
            organisation = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenOrganisation_WhenCurrentUserIsContactForOrganisation_ThenCustomerPermissionsAreGranted()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            var organisation = new OrganisationBuilder(this.DatabaseSession).WithName("organisation").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("Customer").WithUserName("customer").Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(organisation).WithInternalOrganisation(internalOrganisation).WithFromDate(DateTime.UtcNow).Build();
            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customer).WithOrganisation(organisation).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);
            var acl = new AccessControlList(organisation, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanRead(Organisations.Meta.Name));
            Assert.IsTrue(acl.CanWrite(Organisations.Meta.Name));
            Assert.IsTrue(acl.CanRead(Organisations.Meta.LegalForm));
            Assert.IsTrue(acl.CanWrite(Organisations.Meta.LegalForm));
            Assert.IsTrue(acl.CanRead(Organisations.Meta.LogoImage));
            Assert.IsTrue(acl.CanWrite(Organisations.Meta.LogoImage));
            Assert.IsTrue(acl.CanRead(Organisations.Meta.Locale));
            Assert.IsTrue(acl.CanWrite(Organisations.Meta.Locale));

            Assert.IsFalse(acl.CanRead(Organisations.Meta.OwnerSecurityToken));
            Assert.IsFalse(acl.CanWrite(Organisations.Meta.OwnerSecurityToken));
        }
    }
}

//------------------------------------------------------------------------------------------------- 
// <copyright file="SupplierRelationshipTests.cs" company="Allors bvba">
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

    public class SupplierRelationshipTests : DomainTest
    {
        private Person contact;
        private Organisation supplier;
        private Singleton internalOrganisation;
        
        public SupplierRelationshipTests()
        {
            this.contact = new PersonBuilder(this.DatabaseSession).WithLastName("contact").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            this.supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier)
                .Build();

            this.DatabaseSession.Derive();

            this.internalOrganisation = Singleton.Instance(this.DatabaseSession);

            new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithOrganisation(this.supplier)
                .WithContact(this.contact)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
        }

        [Fact]
        public void GivenSupplierOrganisation_WhenOrganisationContactRelationshipIsCreated_ThenPersonIsAddedToUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            Assert.Equal(1, this.supplier.ContactsUserGroup.Members.Count);
            Assert.True(this.supplier.ContactsUserGroup.Members.Contains(this.contact));
        }

        [Fact]
        public void GivenSupplierContactRelationship_WhenContactForOrganisationEnds_ThenContactIsRemovedfromContactsUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var contact2 = new PersonBuilder(this.DatabaseSession).WithLastName("contact2").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            var contactRelationship2 = new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithOrganisation(this.supplier)
                .WithContact(contact2)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(2, this.supplier.ContactsUserGroup.Members.Count);
            Assert.True(this.supplier.ContactsUserGroup.Members.Contains(this.contact));

            contactRelationship2.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive();

            Assert.Equal(1, this.supplier.ContactsUserGroup.Members.Count);
            Assert.True(this.supplier.ContactsUserGroup.Members.Contains(this.contact));
            Assert.False(this.supplier.ContactsUserGroup.Members.Contains(contact2));
        }

        private void InstantiateObjects(ISession session)
        {
            this.contact = (Person)session.Instantiate(this.contact);
            this.supplier = (Organisation)session.Instantiate(this.supplier);
            this.internalOrganisation = (Singleton)session.Instantiate(this.internalOrganisation);
        }
    }
}

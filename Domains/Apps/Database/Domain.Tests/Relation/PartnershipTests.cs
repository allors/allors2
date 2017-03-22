//------------------------------------------------------------------------------------------------- 
// <copyright file="PartnershipTests.cs" company="Allors bvba">
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
    
    public class PartnershipTests : DomainTest
    {
        private Person contact;
        private Organisation partner;
        private InternalOrganisation internalOrganisation;
        private OrganisationContactRelationship contactRelationship;
        private Partnership partnership;
        
        public PartnershipTests()
        {
            this.contact = new PersonBuilder(this.DatabaseSession).WithLastName("contact").Build();
            this.partner = new OrganisationBuilder(this.DatabaseSession).WithName("partner").WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain).Build();
            this.internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");

            this.partnership = new PartnershipBuilder(this.DatabaseSession)
                .WithPartner(this.partner)
                .WithInternalOrganisation(this.internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.contactRelationship = new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithOrganisation(this.partner)
                .WithContact(this.contact)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
        }

        [Fact]
        public void GivenPartnershipIsBuild_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var builder = new PartnershipBuilder(this.DatabaseSession);
            var relationship = builder.Build();

            this.DatabaseSession.Derive();
            Assert.True(relationship.Strategy.IsDeleted);

            this.DatabaseSession.Rollback();

            builder.WithPartner(this.partner);
            relationship = builder.Build();

            this.DatabaseSession.Derive();
            Assert.True(relationship.Strategy.IsDeleted);

            this.DatabaseSession.Rollback();
            
            builder.WithInternalOrganisation(this.internalOrganisation);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);
        }

        [Fact]
        public void GivenSupplierOrganisation_WhenPartnershipIsCreatedIsCreated_ThenPersonIsAddedToUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            Assert.Equal(1, this.partnership.Partner.ContactsUserGroup.Members.Count);
            Assert.True(this.partnership.Partner.ContactsUserGroup.Members.Contains(this.contact));
        }

        [Fact]
        public void GivenSupplierContactRelationship_WhenRelationshipPeriodIsNotValid_ThenContactIsNotInContactsUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            Assert.Equal(1, this.partnership.Partner.ContactsUserGroup.Members.Count);
            Assert.True(this.partnership.Partner.ContactsUserGroup.Members.Contains(this.contact));

            this.partnership.FromDate = DateTime.UtcNow.AddDays(+1);
            this.partnership.RemoveThroughDate();

            this.DatabaseSession.Derive(true);

            Assert.Equal(0, this.partnership.Partner.ContactsUserGroup.Members.Count);

            this.partnership.FromDate = DateTime.UtcNow;
            this.partnership.RemoveThroughDate();

            this.DatabaseSession.Derive(true);

            Assert.Equal(1, this.partnership.Partner.ContactsUserGroup.Members.Count);
            Assert.True(this.partnership.Partner.ContactsUserGroup.Members.Contains(this.contact));

            this.partnership.FromDate = DateTime.UtcNow.AddDays(-2);
            this.partnership.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive(true);

            Assert.Equal(0, this.partnership.Partner.ContactsUserGroup.Members.Count);
        }

        [Fact]
        public void GivenSupplierContactRelationship_WhenContactForOrganisationEnds_ThenContactIsRemovedfromContactsUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var contact2 = new PersonBuilder(this.DatabaseSession).WithLastName("contact2").Build();
            var contactRelationship2 = new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithOrganisation(this.partner)
                .WithContact(contact2)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(2, this.partnership.Partner.ContactsUserGroup.Members.Count);
            Assert.True(this.partnership.Partner.ContactsUserGroup.Members.Contains(this.contact));

            contactRelationship2.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive(true);

            Assert.Equal(1, this.partnership.Partner.ContactsUserGroup.Members.Count);
            Assert.True(this.partnership.Partner.ContactsUserGroup.Members.Contains(this.contact));
            Assert.False(this.partnership.Partner.ContactsUserGroup.Members.Contains(contact2));
        }

        private void InstantiateObjects(ISession session)
        {
            this.contact = (Person)session.Instantiate(this.contact);
            this.partner = (Organisation)session.Instantiate(this.partner);
            this.internalOrganisation = (InternalOrganisation)session.Instantiate(this.internalOrganisation);
            this.contactRelationship = (OrganisationContactRelationship)session.Instantiate(this.contactRelationship);
            this.partnership = (Partnership)session.Instantiate(this.partnership);
        }
    }
}
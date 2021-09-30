// <copyright file="SubContractorRelationshipTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Domain.TestPopulation;
    using System.Linq;
    using Xunit;

    public class SubContractorRelationshipTests : DomainTest
    {
        private Person contact;
        private Organisation subcontractor;
        private SubContractorRelationship subContractorRelationship;
        private OrganisationContactRelationship organisationContactRelationship;

        public SubContractorRelationshipTests()
        {
            this.subcontractor = this.InternalOrganisation.CreateSubContractor(this.Session.Faker());

            this.Session.Derive();
            this.Session.Commit();

            this.subContractorRelationship = this.subcontractor.SubContractorRelationshipsWhereSubContractor.First();
            this.organisationContactRelationship = this.subcontractor.OrganisationContactRelationshipsWhereOrganisation.First();
            this.contact = this.organisationContactRelationship.Contact;
        }

        [Fact]
        public void GivenSubContractorRelationshipBuilder_WhenBuild_ThenSubAccountNumerIsValidElevenTestNumber()
        {
            this.InternalOrganisation.SubAccountCounter.Value = 1000;

            this.Session.Commit();

            var subcontractor1 = new OrganisationBuilder(this.Session).WithDefaults().Build();
            new SubContractorRelationshipBuilder(this.Session).WithSubContractor(subcontractor1).Build();

            this.Session.Derive();

            var subContractor1Financial = subcontractor1.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, this.InternalOrganisation));

            this.Session.Derive();

            Assert.Equal(1007, subContractor1Financial.SubAccountNumber);

            var subcontractor2 = new OrganisationBuilder(this.Session).WithDefaults().Build();
            new SubContractorRelationshipBuilder(this.Session).WithSubContractor(subcontractor2).Build();

            this.Session.Derive();

            var subContractor2Financial = subcontractor2.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, this.InternalOrganisation));

            this.Session.Derive();

            Assert.Equal(1015, subContractor2Financial.SubAccountNumber);

            var subcontractor3 = new OrganisationBuilder(this.Session).WithDefaults().Build();
            new SubContractorRelationshipBuilder(this.Session).WithSubContractor(subcontractor3).Build();

            this.Session.Derive();

            var subContractor3Financial = subcontractor3.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, this.InternalOrganisation));

            this.Session.Derive();

            Assert.Equal(1023, subContractor3Financial.SubAccountNumber);
        }

        [Fact]
        public void GivenSubContractorRelationship_WhenDeriving_ThenSubAccountNumberMustBeUniqueWithinInternalOrganisation()
        {
            var belgium = new Countries(this.Session).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var internalOrganisation2 = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("internalOrganisation2")
                .WithDefaultCollectionMethod(ownBankAccount)
                .Build();

            this.Session.Derive();

            var subcontractor2 = internalOrganisation2.CreateSubContractor(this.Session.Faker());

            this.Session.Derive();

            var subContractorRelationship2 = subcontractor2.SubContractorRelationshipsWhereSubContractor.First();

            this.Session.Derive();

            var partyFinancial2 = subcontractor2.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, internalOrganisation2));

            partyFinancial2.SubAccountNumber = 19;

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSubContractorRelationship_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new SubContractorRelationshipBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithSubContractor(this.subcontractor);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenContractorOrganisation_WhenOrganisationContactRelationshipIsCreated_ThenPersonIsAddedToUserGroup()
        {
            this.InstantiateObjects(this.Session);

            Assert.Single(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members);
            Assert.True(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Contains(this.contact));
        }

        [Fact]
        public void GivenSubContractorRelationship_WhenRelationshipPeriodIsNotValid_ThenContactIsNotInContactsUserGroup()
        {
            this.InstantiateObjects(this.Session);

            Assert.Single(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members);
            Assert.True(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Contains(this.contact));

            this.organisationContactRelationship.FromDate = this.Session.Now().AddDays(+1);
            this.organisationContactRelationship.RemoveThroughDate();

            this.Session.Derive();

            Assert.Equal(0, this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Count);

            this.organisationContactRelationship.FromDate = this.Session.Now().AddSeconds(-1);
            this.organisationContactRelationship.RemoveThroughDate();

            this.Session.Derive();

            Assert.Single(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members);
            Assert.True(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Contains(this.contact));

            this.organisationContactRelationship.FromDate = this.Session.Now().AddDays(-2);
            this.organisationContactRelationship.ThroughDate = this.Session.Now().AddDays(-1);

            this.Session.Derive();

            Assert.Equal(0, this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Count);
        }

        [Fact]
        public void GivenSubContractorRelationship_WhenContactForOrganisationEnds_ThenContactIsRemovedfromContactsUserGroup()
        {
            this.InstantiateObjects(this.Session);

            var contact2 = new PersonBuilder(this.Session).WithLastName("contact2").Build();
            var contactRelationship2 = new OrganisationContactRelationshipBuilder(this.Session)
                .WithOrganisation(this.subcontractor)
                .WithContact(contact2)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            Assert.Equal(2, this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Count);
            Assert.True(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Contains(this.contact));
            Assert.True(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Contains(contact2));

            contactRelationship2.ThroughDate = this.Session.Now().AddDays(-1);

            this.Session.Derive();

            Assert.Single(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members);
            Assert.True(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Contains(this.contact));
            Assert.False(this.subContractorRelationship.SubContractor.ContactsUserGroup.Members.Contains(contact2));
        }

        [Fact]
        public void GivenActiveSubContractorRelationship_WhenDeriving_ThenInternalOrganisationSuppliersContainsSupplier()
        {
            Assert.Contains(this.subcontractor, this.InternalOrganisation.ActiveSubContractors);
        }

        [Fact]
        public void GivenSubContractorRelationshipToCome_WhenDeriving_ThenInternalOrganisationSuppliersDosNotContainSupplier()
        {
            this.subContractorRelationship.FromDate = this.Session.Now().AddDays(1);
            this.Session.Derive();

            Assert.False(this.InternalOrganisation.ActiveSubContractors.Contains(this.subcontractor));
        }

        [Fact]
        public void GivenSubContractorRelationshipThatHasEnded_WhenDeriving_ThenInternalOrganisationSuppliersDosNotContainSupplier()
        {
            this.subContractorRelationship.FromDate = this.Session.Now().AddDays(-10);
            this.subContractorRelationship.ThroughDate = this.Session.Now().AddDays(-1);

            this.Session.Derive();

            Assert.False(this.InternalOrganisation.ActiveSubContractors.Contains(this.subcontractor));
        }

        private void InstantiateObjects(ISession session)
        {
            this.contact = (Person)session.Instantiate(this.contact);
            this.subcontractor = (Organisation)session.Instantiate(this.subcontractor);
            this.subContractorRelationship = (SubContractorRelationship)session.Instantiate(this.subContractorRelationship);
            this.organisationContactRelationship = (OrganisationContactRelationship)session.Instantiate(this.organisationContactRelationship);
        }
    }
}

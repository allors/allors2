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
        private InternalOrganisation internalOrganisation;
        private SupplierRelationship supplierRelationship;

        
        public SupplierRelationshipTests()
        {
            this.contact = new PersonBuilder(this.DatabaseSession).WithLastName("contact").WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();
            this.supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier)
                .Build();

            this.DatabaseSession.Derive();

            this.internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");

            new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithOrganisation(this.supplier)
                .WithContact(this.contact)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(this.supplier)
                .WithInternalOrganisation(this.internalOrganisation)
                .WithFromDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
        }

        [Fact]
        public void GivenSupplierRelationshipBuilder_WhenBuild_ThenSubAccountNumerIsValidElevenTestNumber()
        {
            this.internalOrganisation.SubAccountCounter.Value = 1000;

            this.DatabaseSession.Commit();

            var supplier1 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier1").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var supplierRelationship1 = new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier1).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1007, supplierRelationship1.SubAccountNumber);

            var supplier2 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier2").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var supplierRelationship2 = new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier2).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1015, supplierRelationship2.SubAccountNumber);

            var supplier3 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier3").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var supplierRelationship3 = new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier3).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1023, supplierRelationship3.SubAccountNumber);
        }

        [Fact]
        public void GivenSupplierRelationship_WhenDeriving_ThenSubAccountNumberMustBeUniqueWithinInternalOrganisation()
        {
            var supplier2 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();

            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(new WebAddressBuilder(this.DatabaseSession).WithElectronicAddressString("billfrom").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var internalOrganisation2 = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("internalOrganisation2")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithPartyContactMechanism(billingAddress)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithPreferredCurrency(euro)
                .Build();

            var supplierRelationship2 = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier2)
                .WithInternalOrganisation(internalOrganisation2)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            supplierRelationship2.SubAccountNumber = 19;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSupplierRelationship_WhenDeriving_ThenSameSubAccountNumberIsAllowedAtInternalOrganisation()
        {
            var supplier1 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();

            var supplierRelationship1 = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier1)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            this.DatabaseSession.Derive();

            supplierRelationship1.SubAccountNumber = this.supplier.SupplierRelationshipsWhereSupplier.First.SubAccountNumber;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSupplierRelationship_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var builder = new SupplierRelationshipBuilder(this.DatabaseSession);
            var supplier = builder.Build();

            this.DatabaseSession.Derive();
            Assert.True(supplier.Strategy.IsDeleted);

            this.DatabaseSession.Rollback();

            builder.WithSupplier(this.supplier);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSupplierOrganisation_WhenOrganisationContactRelationshipIsCreated_ThenPersonIsAddedToUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            Assert.Equal(1, this.supplierRelationship.Supplier.ContactsUserGroup.Members.Count);
            Assert.True(this.supplierRelationship.Supplier.ContactsUserGroup.Members.Contains(this.contact));
        }

        [Fact]
        public void GivenSupplierContactRelationship_WhenRelationshipPeriodIsNotValid_ThenContactIsNotInContactsUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            Assert.Equal(1, this.supplierRelationship.Supplier.ContactsUserGroup.Members.Count);
            Assert.True(this.supplierRelationship.Supplier.ContactsUserGroup.Members.Contains(this.contact));

            this.supplierRelationship.FromDate = DateTime.UtcNow.AddDays(+1);
            this.supplierRelationship.RemoveThroughDate();

            this.DatabaseSession.Derive();

            Assert.Equal(0, this.supplierRelationship.Supplier.ContactsUserGroup.Members.Count);

            this.supplierRelationship.FromDate = DateTime.UtcNow.AddSeconds(-1);
            this.supplierRelationship.RemoveThroughDate();

            this.DatabaseSession.Derive();

            Assert.Equal(1, this.supplierRelationship.Supplier.ContactsUserGroup.Members.Count);
            Assert.True(this.supplierRelationship.Supplier.ContactsUserGroup.Members.Contains(this.contact));

            this.supplierRelationship.FromDate = DateTime.UtcNow.AddDays(-2);
            this.supplierRelationship.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive();

            Assert.Equal(0, this.supplierRelationship.Supplier.ContactsUserGroup.Members.Count);
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

            Assert.Equal(2, this.supplierRelationship.Supplier.ContactsUserGroup.Members.Count);
            Assert.True(this.supplierRelationship.Supplier.ContactsUserGroup.Members.Contains(this.contact));

            contactRelationship2.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive();

            Assert.Equal(1, this.supplierRelationship.Supplier.ContactsUserGroup.Members.Count);
            Assert.True(this.supplierRelationship.Supplier.ContactsUserGroup.Members.Contains(this.contact));
            Assert.False(this.supplierRelationship.Supplier.ContactsUserGroup.Members.Contains(contact2));
        }

        [Fact]
        public void GivenActiveSupplierRelationship_WhenDeriving_ThenInternalOrganisationSuppliersContainsSupplier()
        {
            Assert.Contains(this.supplier, this.internalOrganisation.Suppliers);
        }

        [Fact]
        public void GivenSupplierRelationshipToCome_WhenDeriving_ThenInternalOrganisationSuppliersDosNotContainSupplier()
        {
            this.supplierRelationship.FromDate = DateTime.UtcNow.AddDays(1);
            this.DatabaseSession.Derive();

            Assert.False(internalOrganisation.Suppliers.Contains(supplier));
        }

        [Fact]
        public void GivenSupplierRelationshipThatHasEnded_WhenDeriving_ThenInternalOrganisationSuppliersDosNotContainSupplier()
        {
            this.supplierRelationship.FromDate = DateTime.UtcNow.AddDays(-10);
            this.supplierRelationship.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive();

            Assert.False(internalOrganisation.Suppliers.Contains(supplier));
        }

        private void InstantiateObjects(ISession session)
        {
            this.contact = (Person)session.Instantiate(this.contact);
            this.supplier = (Organisation)session.Instantiate(this.supplier);
            this.internalOrganisation = (InternalOrganisation)session.Instantiate(this.internalOrganisation);
            this.supplierRelationship = (SupplierRelationship)session.Instantiate(this.supplierRelationship);
        }
    }
}

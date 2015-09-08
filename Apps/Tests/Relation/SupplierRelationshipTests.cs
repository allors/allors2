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
    using NUnit.Framework;

    [TestFixture]
    public class SupplierRelationshipTests : DomainTest
    {
        private Person contact;
        private Organisation supplier;
        private InternalOrganisation internalOrganisation;
        private SupplierRelationship supplierRelationship;

        [SetUp]
        public override void Init()
        {
            base.Init();

            this.contact = new PersonBuilder(this.DatabaseSession).WithLastName("contact").Build();
            this.supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .Build();
            this.internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithOrganisation(this.supplier)
                .WithContact(this.contact)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(this.supplier)
                .WithInternalOrganisation(this.internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
        }

        [Test]
        public void GivenSupplierRelationshipBuilder_WhenBuild_ThenSubAccountNumerIsValidElevenTestNumber()
        {
            this.internalOrganisation.SubAccountCounter.Value = 1007;

            this.DatabaseSession.Commit();

            var supplier1 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier1").Build();
            var supplierRelationship1 = new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier1).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1007, supplierRelationship1.SubAccountNumber);

            var supplier2 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier2").Build();
            var supplierRelationship2 = new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier2).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1015, supplierRelationship2.SubAccountNumber);

            var supplier3 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier3").Build();
            var supplierRelationship3 = new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier3).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1023, supplierRelationship3.SubAccountNumber);
        }

        [Test]
        public void GivenSupplierRelationship_WhenDeriving_ThenSubAccountNumberMustBeUniqueWithinInternalOrganisation()
        {
            var supplier2 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var internalOrganisation2 = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("internalOrganisation2")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithPreferredCurrency(euro)
                .Build();

            var supplierRelationship2 = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier2)
                .WithInternalOrganisation(internalOrganisation2)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            supplierRelationship2.SubAccountNumber = 19;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSupplierRelationship_WhenDeriving_ThenSameSubAccountNumberIsAllowedAtInternalOrganisation()
        {
            var supplier1 = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            var supplierRelationship1 = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier1)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            this.DatabaseSession.Derive(true);

            supplierRelationship1.SubAccountNumber = this.supplier.SupplierRelationshipsWhereSupplier.First.SubAccountNumber;

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSupplierRelationship_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var builder = new SupplierRelationshipBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSupplier(this.supplier);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSupplierOrganisation_WhenOrganisationContactRelationshipIsCreated_ThenPersonIsAddedToUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            Assert.AreEqual(1, this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Count);
            Assert.IsTrue(this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Contains(this.contact));
        }

        [Test]
        public void GivenSupplierContactRelationship_WhenRelationshipPeriodIsNotValid_ThenContactIsNotInSupplierContactsUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            Assert.AreEqual(1, this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Count);
            Assert.IsTrue(this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Contains(this.contact));

            this.supplierRelationship.FromDate = DateTime.UtcNow.AddDays(+1);
            this.supplierRelationship.RemoveThroughDate();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Count);

            this.supplierRelationship.FromDate = DateTime.UtcNow;
            this.supplierRelationship.RemoveThroughDate();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Count);
            Assert.IsTrue(this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Contains(this.contact));

            this.supplierRelationship.FromDate = DateTime.UtcNow.AddDays(-2);
            this.supplierRelationship.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Count);
        }

        [Test]
        public void GivenSupplierContactRelationship_WhenContactForOrganisationEnds_ThenContactIsRemovedfromSupplierContactsUserGroup()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var contact2 = new PersonBuilder(this.DatabaseSession).WithLastName("contact2").Build();
            var contactRelationship2 = new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithOrganisation(this.supplier)
                .WithContact(contact2)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Count);
            Assert.IsTrue(this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Contains(this.contact));

            contactRelationship2.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Count);
            Assert.IsTrue(this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Contains(this.contact));
            Assert.IsFalse(this.supplierRelationship.Supplier.SupplierContactUserGroup.Members.Contains(contact2));
        }

        [Test]
        public void GivenActiveSupplierRelationship_WhenDeriving_ThenInternalOrganisationSuppliersContainsSupplier()
        {
            Assert.Contains(this.supplier, this.internalOrganisation.Suppliers);
        }

        [Test]
        public void GivenSupplierRelationshipToCome_WhenDeriving_ThenInternalOrganisationSuppliersDosNotContainSupplier()
        {
            this.supplierRelationship.FromDate = DateTime.UtcNow.AddDays(1);
            this.DatabaseSession.Derive(true);

            Assert.IsFalse(internalOrganisation.Suppliers.Contains(supplier));
        }

        [Test]
        public void GivenSupplierRelationshipThatHasEnded_WhenDeriving_ThenInternalOrganisationSuppliersDosNotContainSupplier()
        {
            this.supplierRelationship.FromDate = DateTime.UtcNow.AddDays(-10);
            this.supplierRelationship.ThroughDate = DateTime.UtcNow.AddDays(-1);

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(internalOrganisation.Suppliers.Contains(supplier));
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

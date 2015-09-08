//------------------------------------------------------------------------------------------------- 
// <copyright file="OwnBankAccountTests.cs" company="Allors bvba">
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
    public class OwnBankAccountTests : DomainTest
    {
        [Test]
        public void GivenOwnBankAccount_WhenDeriving_ThenBankAccountRelationMustExist()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            var bankAccount = new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build();

            this.DatabaseSession.Commit();

            var builder = new OwnBankAccountBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBankAccount(bankAccount);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenOwnBankAccount_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            var bankAccount = new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build();

            var paymentMethod = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(bankAccount)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsTrue(paymentMethod.IsActive);
            Assert.IsTrue(paymentMethod.ExistDescription);
        }

        [Test]
        public void GivenOwnBankAccount_WhenDeriving_ThenBankAccountMustBeValidated()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var builder = new BankAccountBuilder(this.DatabaseSession).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien");
            var bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(bankAccount).Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            builder.WithBank(bank);
            bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(bankAccount).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenOwnBankAccountForInternalOrganisationThatDoesAccounting_WhenDeriving_ThenCreditorIsRequired()
        {
            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.DoAccounting = false;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.DoAccounting = true;

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenOwnBankAccount_WhenDeriving_ThenGeneralLedgerAccountAndJournalCannotExistBoth()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession).WithDescription("journal").Build();

            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            var bankAccount = new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build();

            var paymentMethod = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(bankAccount)
                .WithGeneralLedgerAccount(internalOrganisationGlAccount)
                .WithCreditor(supplierRelationship)
                .Build();

            this.DatabaseSession.Commit();

            internalOrganisation.RemovePaymentMethods();
            internalOrganisation.AddPaymentMethod(paymentMethod);
            internalOrganisation.DoAccounting = true;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            paymentMethod.Journal = journal;

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            paymentMethod.RemoveGeneralLedgerAccount();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenOwnBankAccountForInternalOrganisationThatDoesAccounting_WhenDeriving_ThenEitherGeneralLedgerAccountOrJournalMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession).WithDescription("journal").Build();

            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            var bankAccount = new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build();

            var paymentMethod = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(bankAccount)
                .WithCreditor(supplierRelationship)
                .Build();

            this.DatabaseSession.Commit();

            internalOrganisation.RemovePaymentMethods();
            internalOrganisation.AddPaymentMethod(paymentMethod);
            internalOrganisation.DoAccounting = true;

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            paymentMethod.Journal = journal;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            paymentMethod.RemoveJournal();
            paymentMethod.GeneralLedgerAccount = internalOrganisationGlAccount;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
    }
}

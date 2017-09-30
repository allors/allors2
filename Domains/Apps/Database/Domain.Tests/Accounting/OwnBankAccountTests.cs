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
    using Meta;
    using Xunit;

    
    public class OwnBankAccountTests : DomainTest
    {
        [Fact]
        public void GivenOwnBankAccount_WhenDeriving_ThenBankAccountRelationMustExist()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            var bankAccount = new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build();

            this.DatabaseSession.Commit();

            var builder = new OwnBankAccountBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBankAccount(bankAccount);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
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

            this.DatabaseSession.Derive();

            Assert.True(paymentMethod.IsActive);
            Assert.True(paymentMethod.ExistDescription);
        }

        [Fact]
        public void GivenOwnBankAccount_WhenDeriving_ThenBankAccountMustBeValidated()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var builder = new BankAccountBuilder(this.DatabaseSession).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien");
            var bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(bankAccount).Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            builder.WithBank(bank);
            bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("own account")
                .WithBankAccount(bankAccount).Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnBankAccountForSingletonThatDoesAccounting_WhenDeriving_ThenCreditorIsRequired()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            internalOrganisation.DoAccounting = false;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            internalOrganisation.DoAccounting = true;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnBankAccount_WhenDeriving_ThenGeneralLedgerAccountAndJournalCannotExistBoth()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier)
                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
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
                .WithCreditor(supplier)
                .Build();

            this.DatabaseSession.Commit();

            internalOrganisation.DoAccounting = true;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            paymentMethod.Journal = journal;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            paymentMethod.RemoveGeneralLedgerAccount();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnBankAccountForSingletonThatDoesAccounting_WhenDeriving_ThenEitherGeneralLedgerAccountOrJournalMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier)
                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
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
                .WithCreditor(supplier)
                .Build();

            this.DatabaseSession.Commit();

            internalOrganisation.DoAccounting = true;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            paymentMethod.Journal = journal;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            paymentMethod.RemoveJournal();
            paymentMethod.GeneralLedgerAccount = internalOrganisationGlAccount;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }
    }
}

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
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            var bankAccount = new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build();

            this.Session.Commit();

            var builder = new OwnBankAccountBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithBankAccount(bankAccount);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnBankAccount_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            var bankAccount = new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build();

            var paymentMethod = new OwnBankAccountBuilder(this.Session)
                .WithDescription("own account")
                .WithBankAccount(bankAccount)
                .Build();

            this.Session.Derive();

            Assert.True(paymentMethod.IsActive);
            Assert.True(paymentMethod.ExistDescription);
        }

        [Fact]
        public void GivenOwnBankAccount_WhenDeriving_ThenBankAccountMustBeValidated()
        {
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var builder = new BankAccountBuilder(this.Session).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien");
            var bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.Session)
                .WithDescription("own account")
                .WithBankAccount(bankAccount).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            builder.WithBank(bank);
            bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.Session)
                .WithDescription("own account")
                .WithBankAccount(bankAccount).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnBankAccount_WhenDeriving_ThenGeneralLedgerAccountAndJournalAtMostOne()
        {
            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.Session).WithDescription("journal").Build();

            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            var bankAccount = new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build();

            var paymentMethod = new OwnBankAccountBuilder(this.Session)
                .WithDescription("own account")
                .WithBankAccount(bankAccount)
                .WithGeneralLedgerAccount(internalOrganisationGlAccount)
                .Build();

            this.Session.Commit();

            var internalOrganisation = this.InternalOrganisation;
            internalOrganisation.DoAccounting = true;
            internalOrganisation.DefaultCollectionMethod = paymentMethod;

            Assert.False(this.Session.Derive(false).HasErrors);

            paymentMethod.Journal = journal;

            Assert.True(this.Session.Derive(false).HasErrors);

            paymentMethod.RemoveGeneralLedgerAccount();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnBankAccountForSingletonThatDoesAccounting_WhenDeriving_ThenEitherGeneralLedgerAccountOrJournalMustExist()
        {
            var internalOrganisation = this.InternalOrganisation;

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.Session).WithDescription("journal").Build();

            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            var bankAccount = new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build();

            var collectionMethod = new OwnBankAccountBuilder(this.Session)
                .WithDescription("own account")
                .WithBankAccount(bankAccount)
                .Build();

            this.Session.Commit();

            internalOrganisation.DoAccounting = true;
            internalOrganisation.AddActiveCollectionMethod(collectionMethod);

            Assert.True(this.Session.Derive(false).HasErrors);

            collectionMethod.Journal = journal;

            Assert.False(this.Session.Derive(false).HasErrors);

            collectionMethod.RemoveJournal();
            collectionMethod.GeneralLedgerAccount = internalOrganisationGlAccount;

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}

//------------------------------------------------------------------------------------------------- 
// <copyright file="JournalTests.cs" company="Allors bvba">
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

    using Xunit;

    public class JournalTests : DomainTest
    {
        [Fact]
        public void GivenJournal_WhenDeriving_ThenDescriptionMustExist()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var glAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            this.DatabaseSession.Commit();

            var builder = new JournalBuilder(this.DatabaseSession);
            builder.WithJournalType(new JournalTypes(this.DatabaseSession).Bank);
            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenSingletonMustExist()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            var glAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            this.DatabaseSession.Commit();

            var builder = new JournalBuilder(this.DatabaseSession);
            builder.WithDescription("description");
            builder.WithJournalType(new JournalTypes(this.DatabaseSession).Bank);
            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenJournalTypeMustExist()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            var glAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            this.DatabaseSession.Commit();

            var builder = new JournalBuilder(this.DatabaseSession);
            builder.WithDescription("description");
            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithJournalType(new JournalTypes(this.DatabaseSession).Bank);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenContraAccountMustExist()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var builder = new JournalBuilder(this.DatabaseSession);
            builder.WithDescription("description");
            builder.WithJournalType(new JournalTypes(this.DatabaseSession).Bank);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            var glAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenJournal_WhenBuildWithout_ThenBlockUnpaidTransactionsIsFalse()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .WithContraAccount(internalOrganisationGlAccount)
                .WithDescription("journal")
                .Build();

            this.DatabaseSession.Derive(false);

            Assert.False(journal.BlockUnpaidTransactions);
        }

        [Fact]
        public void GivenJournal_WhenBuildWithout_ThenCloseWhenInBalanceIsFalse()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .WithContraAccount(internalOrganisationGlAccount)
                .WithDescription("journal")
                .Build();

            this.DatabaseSession.Derive(false);

            Assert.False(journal.CloseWhenInBalance);
        }

        [Fact]
        public void GivenJournal_WhenBuildWithout_ThenUseAsDefaultIsFalse()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .WithContraAccount(internalOrganisationGlAccount)
                .WithDescription("journal")
                .Build();

            this.DatabaseSession.Derive(false);

            Assert.False(journal.UseAsDefault);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenContraAccountCanBeChangedWhenNotUsedYet()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var generalLedgerAccount1 = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("bankAccount 1")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount1 = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var generalLedgerAccount2 = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0002")
                .WithName("bankAccount 2")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount2 = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(generalLedgerAccount2)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount1)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.ContraAccount = internalOrganisationGlAccount2;

            this.DatabaseSession.Derive();

            Assert.Equal(generalLedgerAccount2, journal.PreviousContraAccount.GeneralLedgerAccount);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenContraAccountCanNotBeChangedWhenJournalEntriesArePresent()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var generalLedgerAccount1 = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("bankAccount 1")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount1 = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var generalLedgerAccount2 = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0002")
                .WithName("bankAccount 2")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount2 = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(generalLedgerAccount2)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount1)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.AddJournalEntry(new JournalEntryBuilder(this.DatabaseSession)
                                        .WithJournalEntryDetail(new JournalEntryDetailBuilder(this.DatabaseSession)
                                                                    .WithAmount(1)
                                                                    .WithDebit(true)
                                                                    .WithGeneralLedgerAccount(internalOrganisationGlAccount1)
                                                                    .Build())
                                        .Build());

            journal.ContraAccount = internalOrganisationGlAccount2;

            Assert.Equal("Journal.ContraAccount, Journal.PreviousContraAccount are not equal", this.DatabaseSession.Derive(false).Errors[0].Message);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenJournalTypeCanBeChangedWhenJournalIsNotUsedYet()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var generalLedgerAccount1 = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("bankAccount 1")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.JournalType = new JournalTypes(this.DatabaseSession).Cash;

            this.DatabaseSession.Derive();

            Assert.Equal(new JournalTypes(this.DatabaseSession).Cash, journal.PreviousJournalType);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenJournalTypeCanNotBeChangedWhenJournalEntriesArePresent()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var generalLedgerAccount1 = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("bankAccount 1")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.AddJournalEntry(new JournalEntryBuilder(this.DatabaseSession)
                                        .WithJournalEntryDetail(new JournalEntryDetailBuilder(this.DatabaseSession)
                                                                    .WithAmount(1)
                                                                    .WithDebit(true)
                                                                    .WithGeneralLedgerAccount(internalOrganisationGlAccount)
                                                                    .Build())
                                        .Build());

            journal.JournalType = new JournalTypes(this.DatabaseSession).Cash;

            Assert.Equal("Journal.JournalType, Journal.PreviousJournalType are not equal", this.DatabaseSession.Derive(false).Errors[0].Message);
        }
    }
}

// <copyright file="JournalTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class JournalTests : DomainTest
    {
        [Fact]
        public void GivenJournal_WhenDeriving_ThenDescriptionMustExist()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var glAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            this.Session.Commit();

            var builder = new JournalBuilder(this.Session);
            builder.WithJournalType(new JournalTypes(this.Session).Bank);
            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenSingletonMustExist()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var glAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            this.Session.Commit();

            var builder = new JournalBuilder(this.Session);
            builder.WithDescription("description");
            builder.WithJournalType(new JournalTypes(this.Session).Bank);
            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenJournalTypeMustExist()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var glAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            this.Session.Commit();

            var builder = new JournalBuilder(this.Session);
            builder.WithDescription("description");
            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithJournalType(new JournalTypes(this.Session).Bank);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenContraAccountMustExist()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var builder = new JournalBuilder(this.Session);
            builder.WithDescription("description");
            builder.WithJournalType(new JournalTypes(this.Session).Bank);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            var glAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenJournal_WhenBuildWithout_ThenBlockUnpaidTransactionsIsFalse()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.Session)
                .WithJournalType(new JournalTypes(this.Session).Bank)
                .WithContraAccount(internalOrganisationGlAccount)
                .WithDescription("journal")
                .Build();

            this.Session.Derive(false);

            Assert.False(journal.BlockUnpaidTransactions);
        }

        [Fact]
        public void GivenJournal_WhenBuildWithout_ThenCloseWhenInBalanceIsFalse()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.Session)
                .WithJournalType(new JournalTypes(this.Session).Bank)
                .WithContraAccount(internalOrganisationGlAccount)
                .WithDescription("journal")
                .Build();

            this.Session.Derive(false);

            Assert.False(journal.CloseWhenInBalance);
        }

        [Fact]
        public void GivenJournal_WhenBuildWithout_ThenUseAsDefaultIsFalse()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.Session)
                .WithJournalType(new JournalTypes(this.Session).Bank)
                .WithContraAccount(internalOrganisationGlAccount)
                .WithDescription("journal")
                .Build();

            this.Session.Derive(false);

            Assert.False(journal.UseAsDefault);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenContraAccountCanBeChangedWhenNotUsedYet()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var generalLedgerAccount1 = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("bankAccount 1")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount1 = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var generalLedgerAccount2 = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0002")
                .WithName("bankAccount 2")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount2 = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(generalLedgerAccount2)
                .Build();

            var journal = new JournalBuilder(this.Session)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount1)
                .WithJournalType(new JournalTypes(this.Session).Bank)
                .Build();

            this.Session.Derive();

            Assert.Equal(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.ContraAccount = internalOrganisationGlAccount2;

            this.Session.Derive();

            Assert.Equal(generalLedgerAccount2, journal.PreviousContraAccount.GeneralLedgerAccount);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenContraAccountCanNotBeChangedWhenJournalEntriesArePresent()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var generalLedgerAccount1 = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("bankAccount 1")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount1 = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var generalLedgerAccount2 = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0002")
                .WithName("bankAccount 2")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount2 = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(generalLedgerAccount2)
                .Build();

            var journal = new JournalBuilder(this.Session)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount1)
                .WithJournalType(new JournalTypes(this.Session).Bank)
                .Build();

            this.Session.Derive();

            Assert.Equal(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.AddJournalEntry(new JournalEntryBuilder(this.Session)
                                        .WithJournalEntryDetail(new JournalEntryDetailBuilder(this.Session)
                                                                    .WithAmount(1)
                                                                    .WithDebit(true)
                                                                    .WithGeneralLedgerAccount(internalOrganisationGlAccount1)
                                                                    .Build())
                                        .Build());

            journal.ContraAccount = internalOrganisationGlAccount2;

            Assert.Equal("Journal.ContraAccount, Journal.PreviousContraAccount are not equal", this.Session.Derive(false).Errors[0].Message);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenJournalTypeCanBeChangedWhenJournalIsNotUsedYet()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var generalLedgerAccount1 = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("bankAccount 1")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var journal = new JournalBuilder(this.Session)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount)
                .WithJournalType(new JournalTypes(this.Session).Bank)
                .Build();

            this.Session.Derive();

            Assert.Equal(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.JournalType = new JournalTypes(this.Session).Cash;

            this.Session.Derive();

            Assert.Equal(new JournalTypes(this.Session).Cash, journal.PreviousJournalType);
        }

        [Fact]
        public void GivenJournal_WhenDeriving_ThenJournalTypeCanNotBeChangedWhenJournalEntriesArePresent()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var generalLedgerAccount1 = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("bankAccount 1")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var journal = new JournalBuilder(this.Session)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount)
                .WithJournalType(new JournalTypes(this.Session).Bank)
                .Build();

            this.Session.Derive();

            Assert.Equal(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.AddJournalEntry(new JournalEntryBuilder(this.Session)
                                        .WithJournalEntryDetail(new JournalEntryDetailBuilder(this.Session)
                                                                    .WithAmount(1)
                                                                    .WithDebit(true)
                                                                    .WithGeneralLedgerAccount(internalOrganisationGlAccount)
                                                                    .Build())
                                        .Build());

            journal.JournalType = new JournalTypes(this.Session).Cash;

            Assert.Equal("Journal.JournalType, Journal.PreviousJournalType are not equal", this.Session.Derive(false).Errors[0].Message);
        }
    }
}

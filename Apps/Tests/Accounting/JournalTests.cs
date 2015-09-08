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

using System;

namespace Allors.Domain
{
    using NUnit.Framework;

    [TestFixture]
    public class JournalTests : DomainTest
    {
        [Test]
        public void GivenJournal_WhenDeriving_ThenDescriptionMustExist()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

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
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            this.DatabaseSession.Commit();

            var builder = new JournalBuilder(this.DatabaseSession);
            builder.WithJournalType(new JournalTypes(this.DatabaseSession).Bank);
            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("description");
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenJournal_WhenDeriving_ThenInternalOrganisationMustExist()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
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
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            this.DatabaseSession.Commit();

            var builder = new JournalBuilder(this.DatabaseSession);
            builder.WithDescription("description");
            builder.WithJournalType(new JournalTypes(this.DatabaseSession).Bank);
            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenJournal_WhenDeriving_ThenJournalTypeMustExist()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
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
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            this.DatabaseSession.Commit();

            var builder = new JournalBuilder(this.DatabaseSession);
            builder.WithDescription("description");
            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithJournalType(new JournalTypes(this.DatabaseSession).Bank);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenJournal_WhenDeriving_ThenContraAccountMustExist()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var builder = new JournalBuilder(this.DatabaseSession);
            builder.WithDescription("description");
            builder.WithJournalType(new JournalTypes(this.DatabaseSession).Bank);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

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
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(glAccount)
                .Build();

            builder.WithContraAccount(internalOrganisationGlAccount);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenJournal_WhenBuildWithout_ThenBlockUnpaidTransactionsIsFalse()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .WithContraAccount(internalOrganisationGlAccount)
                .WithDescription("journal")
                .Build();

            this.DatabaseSession.Derive();

            Assert.IsFalse(journal.BlockUnpaidTransactions);
        }

        [Test]
        public void GivenJournal_WhenBuildWithout_ThenCloseWhenInBalanceIsFalse()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .WithContraAccount(internalOrganisationGlAccount)
                .WithDescription("journal")
                .Build();

            this.DatabaseSession.Derive();

            Assert.IsFalse(journal.CloseWhenInBalance);
        }

        [Test]
        public void GivenJournal_WhenBuildWithout_ThenUseAsDefaultIsFalse()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .WithContraAccount(internalOrganisationGlAccount)
                .WithDescription("journal")
                .Build();

            this.DatabaseSession.Derive();

            Assert.IsFalse(journal.UseAsDefault);
        }

        [Test]
        public void GivenJournal_WhenDeriving_ThenContraAccountCanBeChangedWhenNotUsedYet()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

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
                .WithInternalOrganisation(internalOrganisation)
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
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount2)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount1)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.ContraAccount = internalOrganisationGlAccount2;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(generalLedgerAccount2, journal.PreviousContraAccount.GeneralLedgerAccount);
        }

        [Test]
        public void GivenJournal_WhenDeriving_ThenContraAccountCanNotBeChangedWhenJournalEntriesArePresent()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

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
                .WithInternalOrganisation(internalOrganisation)
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
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount2)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount1)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.AddJournalEntry(new JournalEntryBuilder(this.DatabaseSession)
                                        .WithJournalEntryDetail(new JournalEntryDetailBuilder(this.DatabaseSession)
                                                                    .WithAmount(1)
                                                                    .WithDebit(true)
                                                                    .WithGeneralLedgerAccount(internalOrganisationGlAccount1)
                                                                    .Build())
                                        .Build());

            journal.ContraAccount = internalOrganisationGlAccount2;

            Assert.AreEqual("Journal.ContraAccount, Journal.PreviousContraAccount are not equal", this.DatabaseSession.Derive().Errors[0].Message);
        }

        [Test]
        public void GivenJournal_WhenDeriving_ThenJournalTypeCanBeChangedWhenJournalIsNotUsedYet()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

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
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.JournalType = new JournalTypes(this.DatabaseSession).Cash;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new JournalTypes(this.DatabaseSession).Cash, journal.PreviousJournalType);
        }

        [Test]
        public void GivenJournal_WhenDeriving_ThenJournalTypeCanNotBeChangedWhenJournalEntriesArePresent()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

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
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount1)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession)
                .WithDescription("description")
                .WithContraAccount(internalOrganisationGlAccount)
                .WithJournalType(new JournalTypes(this.DatabaseSession).Bank)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(generalLedgerAccount1, journal.PreviousContraAccount.GeneralLedgerAccount);

            journal.AddJournalEntry(new JournalEntryBuilder(this.DatabaseSession)
                                        .WithJournalEntryDetail(new JournalEntryDetailBuilder(this.DatabaseSession)
                                                                    .WithAmount(1)
                                                                    .WithDebit(true)
                                                                    .WithGeneralLedgerAccount(internalOrganisationGlAccount)
                                                                    .Build())
                                        .Build());

            journal.JournalType = new JournalTypes(this.DatabaseSession).Cash;

            Assert.AreEqual("Journal.JournalType, Journal.PreviousJournalType are not equal", this.DatabaseSession.Derive().Errors[0].Message);
        }
    }
}

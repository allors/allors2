// <copyright file="OrganisationGlAccountTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class OrganisationGlAccountTests : DomainTest
    {
        [Fact]
        public void GivenOrganisationGlAccount_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrganisationGlAccountBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithGeneralLedgerAccount(new GeneralLedgerAccountBuilder(this.Session)
                                                .WithAccountNumber("0001")
                                                .WithName("GeneralLedgerAccount")
                                                .WithBalanceSheetAccount(true)
                                                .WithSide(new DebitCreditConstants(this.Session).Debit)
                                                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                                                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                                                .Build());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrganisationGlAccount_WhenBuild_ThenHasBankStatementTransactionsIsAlwaysFalse()
        {
            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var organisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            this.Session.Derive();

            Assert.False(organisationGlAccount.HasBankStatementTransactions);
        }

        [Fact]
        public void GivenOrganisationGlAccount_WhenNotReferenced_ThenAccountIsNeutral()
        {
            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var organisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            Assert.True(organisationGlAccount.IsNeutralAccount());
            Assert.False(organisationGlAccount.IsBankAccount());
            Assert.False(organisationGlAccount.IsCashAccount());
            Assert.False(organisationGlAccount.IsCostAccount());
            Assert.False(organisationGlAccount.IsCreditorAccount());
            Assert.False(organisationGlAccount.IsDebtorAccount());
            Assert.False(organisationGlAccount.IsInventoryAccount());
            Assert.False(organisationGlAccount.IsTurnOverAccount());
        }
    }
}

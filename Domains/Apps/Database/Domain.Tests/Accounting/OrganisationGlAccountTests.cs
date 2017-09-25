//------------------------------------------------------------------------------------------------- 
// <copyright file="OrganisationGlAccountTests.cs" company="Allors bvba">
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

    
    public class OrganisationGlAccountTests : DomainTest
    {
        [Fact]
        public void GivenOrganisationGlAccount_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrganisationGlAccountBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithGeneralLedgerAccount(new GeneralLedgerAccountBuilder(this.DatabaseSession)
                                                .WithAccountNumber("0001")
                                                .WithName("GeneralLedgerAccount")
                                                .WithBalanceSheetAccount(true)
                                                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                                                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                                                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                                                .Build());
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrganisationGlAccount_WhenBuild_ThenHasBankStatementTransactionsIsAlwaysFalse()
        {
            var internalOrganisation = InternalOrganisation.Instance(this.DatabaseSession);

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var organisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            this.DatabaseSession.Derive();

            Assert.False(organisationGlAccount.HasBankStatementTransactions);
        }

        [Fact]
        public void GivenOrganisationGlAccount_WhenNotReferenced_ThenAccountIsNeutral()
        {
            var internalOrganisation = InternalOrganisation.Instance(this.DatabaseSession);

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var organisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
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
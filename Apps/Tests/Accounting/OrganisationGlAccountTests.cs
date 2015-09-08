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
    using NUnit.Framework;

    [TestFixture]
    public class OrganisationGlAccountTests : DomainTest
    {
        [Test]
        public void GivenOrganisationGlAccount_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrganisationGlAccountBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"));
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

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

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenOrganisationGlAccount_WhenBuild_ThenHasBankStatementTransactionsIsAlwaysFalse()
        {
            throw new Exception("Review");
            //var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            //var generalLedgerAccount1 = new GeneralLedgerAccountBuilder(this.DatabaseSession)
            //    .WithAccountNumber("0001")
            //    .WithName("GeneralLedgerAccount")
            //    .WithBalanceSheetAccount(true)
            //    .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
            //    .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
            //    .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
            //    .Build();

            //var organisationGlAccount1 = new OrganisationGlAccountBuilder(this.DatabaseSession)
            //    .WithInternalOrganisation(internalOrganisation)
            //    .WithGeneralLedgerAccount(generalLedgerAccount1)
            //    .Build();

            //var generalLedgerAccount2 = new GeneralLedgerAccountBuilder(this.DatabaseSession)
            //    .WithAccountNumber("0002")
            //    .WithName("GeneralLedgerAccount")
            //    .WithBalanceSheetAccount(true)
            //    .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
            //    .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
            //    .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
            //    .Build();

            //var organisationGlAccount2 = new OrganisationGlAccountBuilder(this.DatabaseSession)
            //    .WithInternalOrganisation(internalOrganisation)
            //    .WithGeneralLedgerAccount(generalLedgerAccount2)
            //    .WithHasBankStatementTransactions(true)
            //    .Build();

            //this.DatabaseSession.Derive();

            //Assert.IsFalse(organisationGlAccount1.HasBankStatementTransactions);
            //Assert.IsFalse(organisationGlAccount2.HasBankStatementTransactions);
        }

        [Test]
        public void GivenOrganisationGlAccount_WhenNotReferenced_ThenAccountIsNeutral()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var organisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            Assert.IsTrue(organisationGlAccount.IsNeutralAccount());
            Assert.IsFalse(organisationGlAccount.IsBankAccount());
            Assert.IsFalse(organisationGlAccount.IsCashAccount());
            Assert.IsFalse(organisationGlAccount.IsCostAccount());
            Assert.IsFalse(organisationGlAccount.IsCreditorAccount());
            Assert.IsFalse(organisationGlAccount.IsDebtorAccount());
            Assert.IsFalse(organisationGlAccount.IsInventoryAccount());
            Assert.IsFalse(organisationGlAccount.IsTurnOverAccount());
        }
    }
}
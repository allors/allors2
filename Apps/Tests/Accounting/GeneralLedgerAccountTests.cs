//------------------------------------------------------------------------------------------------- 
// <copyright file="GeneralLedgerAccountTests.cs" company="Allors bvba">
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
    using NUnit.Framework;

    using Resources;

    [TestFixture]
    public class GeneralLedgerAccountTests : DomainTest
    {
        [Test]
        public void GivenGeneralLedgerAccount_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var accountGroup = new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build();
            var accountType = new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build();

            this.DatabaseSession.Commit();

            var builder = new GeneralLedgerAccountBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithAccountNumber("0001");
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors); 

            this.DatabaseSession.Rollback();

            builder.WithName("GeneralLedgerAccount");
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBalanceSheetAccount(true);
            builder.Build();

            this.DatabaseSession.Rollback();

            builder.WithSide(new DebitCreditConstants(this.DatabaseSession).Debit);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithGeneralLedgerAccountGroup(accountGroup);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithGeneralLedgerAccountType(accountType);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenGeneralLedgerAccount_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            Assert.IsTrue(generalLedgerAccount.ExistUniqueId);
            Assert.AreEqual(generalLedgerAccount.CashAccount, false);
            Assert.AreEqual(generalLedgerAccount.CostCenterAccount, false);
            Assert.AreEqual(generalLedgerAccount.CostCenterRequired, false);
            Assert.AreEqual(generalLedgerAccount.CostUnitAccount, false);
            Assert.AreEqual(generalLedgerAccount.CostUnitRequired, false);
            Assert.AreEqual(generalLedgerAccount.Protected, false);
            Assert.AreEqual(generalLedgerAccount.ReconciliationAccount, false);
        }

        [Test]
        public void GivenGeneralLedgerAccount_WhenAddedToChartOfAccounts_ThenAccountNumberMustBeUnique()
        {
            var glAccount0001 = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var glAccount0001Dup = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount duplicate number")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var chart = new ChartOfAccountsBuilder(this.DatabaseSession).WithName("name").WithGeneralLedgerAccount(glAccount0001).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            chart.AddGeneralLedgerAccount(glAccount0001Dup);

            var derivationLog = this.DatabaseSession.Derive();
            var expectedMessage = ErrorMessages.AccountNumberUniqueWithinChartOfAccounts;

            Assert.AreEqual(derivationLog.Errors[0].Message, expectedMessage);

            new ChartOfAccountsBuilder(this.DatabaseSession).WithName("another Chart").WithGeneralLedgerAccount(glAccount0001Dup).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenGeneralLedgerAccount_WhenSettingCostCenterRequired_ThenAccountMustBeMarkedAsCostCenterAccount()
        {
            new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithCostCenterRequired(true)
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var derivationLog = this.DatabaseSession.Derive();
            
            var expectedMessage = ErrorMessages.NotACostCenterAccount;
            
            Assert.AreEqual(derivationLog.Errors[0].Message, expectedMessage);
        }

        [Test]
        public void GivenGeneralLedgerAccount_WhenSettingCostUnitRequired_ThenAccountMustBeMarkedAsCostUnitAccount()
        {
            new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithCostUnitRequired(true)
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var derivationLog = this.DatabaseSession.Derive();
            var expectedMessage = ErrorMessages.NotACostUnitAccount;

            Assert.AreEqual(derivationLog.Errors[0].Message, expectedMessage);
        }

        [Test]
        public void GivenGeneralLedgerAccount_WhenSettingDefaultCostCenter_ThenDefaultCostCenterMustBeInListOfAllowedCostCenters()
        {
            var costCenter = new CostCenterBuilder(this.DatabaseSession).WithName("costCenter").Build();

            var glAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithCostCenterAccount(true)
                .WithCostCenterRequired(true)
                .WithDefaultCostCenter(costCenter)
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var derivationLog = this.DatabaseSession.Derive();

            var expectedMessage = ErrorMessages.CostCenterNotAllowed;

            Assert.AreEqual(derivationLog.Errors[0].Message, expectedMessage);

            glAccount.AddCostCenterAllowed(costCenter);

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenGeneralLedgerAccount_WhenSettingDefaultCostUnit_ThenDefaultCostUnitMustBeInListOfAllowedCostUnits()
        {
            var costUnit = new GoodBuilder(this.DatabaseSession)
                .WithName("Good")
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var glAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithCostUnitAccount(true)
                .WithCostUnitRequired(true)
                .WithDefaultCostUnit(costUnit)
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.DatabaseSession).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.DatabaseSession).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.DatabaseSession).WithDescription("accountGroup").Build())
                .Build();

            var derivationLog = this.DatabaseSession.Derive();
            var expectedMessage = ErrorMessages.CostUnitNotAllowed;

            Assert.AreEqual(derivationLog.Errors[0].Message, expectedMessage);

            glAccount.AddCostUnitAllowed(costUnit);

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
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
    using Meta;
    using Xunit;
    using Resources;

    
    public class GeneralLedgerAccountTests : DomainTest
    {
        [Fact]
        public void GivenGeneralLedgerAccount_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var accountGroup = new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build();
            var accountType = new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build();

            this.Session.Commit();

            var builder = new GeneralLedgerAccountBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithAccountNumber("0001");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors); 

            this.Session.Rollback();

            builder.WithName("GeneralLedgerAccount");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithBalanceSheetAccount(true);
            builder.Build();

            this.Session.Rollback();

            builder.WithSide(new DebitCreditConstants(this.Session).Debit);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithGeneralLedgerAccountGroup(accountGroup);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithGeneralLedgerAccountType(accountType);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenGeneralLedgerAccount_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            Assert.True(generalLedgerAccount.ExistUniqueId);
            Assert.Equal(generalLedgerAccount.CashAccount, false);
            Assert.Equal(generalLedgerAccount.CostCenterAccount, false);
            Assert.Equal(generalLedgerAccount.CostCenterRequired, false);
            Assert.Equal(generalLedgerAccount.CostUnitAccount, false);
            Assert.Equal(generalLedgerAccount.CostUnitRequired, false);
            Assert.Equal(generalLedgerAccount.Protected, false);
            Assert.Equal(generalLedgerAccount.ReconciliationAccount, false);
        }

        [Fact]
        public void GivenGeneralLedgerAccount_WhenAddedToChartOfAccounts_ThenAccountNumberMustBeUnique()
        {
            var glAccount0001 = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var glAccount0001Dup = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount duplicate number")
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var chart = new ChartOfAccountsBuilder(this.Session).WithName("name").WithGeneralLedgerAccount(glAccount0001).Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            chart.AddGeneralLedgerAccount(glAccount0001Dup);

            var derivationLog = this.Session.Derive(false);
            var expectedMessage = ErrorMessages.AccountNumberUniqueWithinChartOfAccounts;

            Assert.Equal(derivationLog.Errors[0].Message, expectedMessage);

            new ChartOfAccountsBuilder(this.Session).WithName("another Chart").WithGeneralLedgerAccount(glAccount0001Dup).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenGeneralLedgerAccount_WhenSettingCostCenterRequired_ThenAccountMustBeMarkedAsCostCenterAccount()
        {
            new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithCostCenterRequired(true)
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var derivationLog = this.Session.Derive(false);
            
            var expectedMessage = ErrorMessages.NotACostCenterAccount;
            
            Assert.Equal(derivationLog.Errors[0].Message, expectedMessage);
        }

        [Fact]
        public void GivenGeneralLedgerAccount_WhenSettingCostUnitRequired_ThenAccountMustBeMarkedAsCostUnitAccount()
        {
            new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithCostUnitRequired(true)
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var derivationLog = this.Session.Derive(false);
            var expectedMessage = ErrorMessages.NotACostUnitAccount;

            Assert.Equal(derivationLog.Errors[0].Message, expectedMessage);
        }

        [Fact]
        public void GivenGeneralLedgerAccount_WhenSettingDefaultCostCenter_ThenDefaultCostCenterMustBeInListOfAllowedCostCenters()
        {
            var costCenter = new CostCenterBuilder(this.Session).WithName("costCenter").Build();

            var glAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithCostCenterAccount(true)
                .WithCostCenterRequired(true)
                .WithDefaultCostCenter(costCenter)
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var derivationLog = this.Session.Derive(false);

            var expectedMessage = ErrorMessages.CostCenterNotAllowed;

            Assert.Equal(derivationLog.Errors[0].Message, expectedMessage);

            glAccount.AddCostCentersAllowed(costCenter);

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenGeneralLedgerAccount_WhenSettingDefaultCostUnit_ThenDefaultCostUnitMustBeInListOfAllowedCostUnits()
        {
            var costUnit = new GoodBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("good").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var glAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithCostUnitAccount(true)
                .WithCostUnitRequired(true)
                .WithDefaultCostUnit(costUnit)
                .WithBalanceSheetAccount(true)
                .WithSide(new DebitCreditConstants(this.Session).Debit)
                .WithGeneralLedgerAccountType(new GeneralLedgerAccountTypeBuilder(this.Session).WithDescription("accountType").Build())
                .WithGeneralLedgerAccountGroup(new GeneralLedgerAccountGroupBuilder(this.Session).WithDescription("accountGroup").Build())
                .Build();

            var derivationLog = this.Session.Derive(false);
            var expectedMessage = ErrorMessages.CostUnitNotAllowed;

            Assert.Equal(derivationLog.Errors[0].Message, expectedMessage);

            glAccount.AddCostUnitsAllowed(costUnit);

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
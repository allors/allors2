// <copyright file="ChartOfAccountsImportTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    public class ChartOfAccountsImportTests : DomainTest
    {
        // TODO: Import
        // [Fact]
        // public void GivenGeneralLedgerAccountXml_WhenImported_ThenGeneralLedgerAccountIsCreated()
        // {
        //    var filepath = string.Format("domain\\import\\minimaal genormaliseerd rekeningstelsel 1.xml");

        // new ChartOfAccountsImport(this.DatabaseSession).ImportChartOfAccounts(filepath);

        // Assert.Single(new GeneralLedgerAccounts(this.DatabaseSession).Extent().Count);
        //    Assert.Single(this.DatabaseSession.Extent<GeneralLedgerAccount>().Count);
        //    Assert.Single(this.DatabaseSession.Extent<ChartOfAccounts>().Count);
        //    Assert.Single(this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().Count);
        //    Assert.Single(this.DatabaseSession.Extent<GeneralLedgerAccountType>().Count);
        //    Assert.Equal(0, this.DatabaseSession.Extent<CostCenter>().Count);

        // var chartOfAccounts = (ChartOfAccounts)this.DatabaseSession.Extent<ChartOfAccounts>().First;
        //    Assert.Equal("Minimum Algemeen Rekeningenstelsel", chartOfAccounts.Name);
        //    Assert.Single(chartOfAccounts.GeneralLedgerAccounts.Count);

        // var generalLedgerAccountGroup = (GeneralLedgerAccountGroup)this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().First;
        //    Assert.Equal("Kapitaal", generalLedgerAccountGroup.Description);
        //    Assert.False(generalLedgerAccountGroup.ExistParent);

        // var generalLedgerAccountType = (GeneralLedgerAccountType)this.DatabaseSession.Extent<GeneralLedgerAccountType>().First;
        //    Assert.Equal("Eigen vermogen, voorzieningen voor risico's en kosten en schulden op langer dan een jaar", generalLedgerAccountType.Description);

        // var generalLedgerAccount = (GeneralLedgerAccount)this.DatabaseSession.Extent<GeneralLedgerAccount>().First;
        //    Assert.Equal("100000", generalLedgerAccount.AccountNumber);
        //    Assert.True(generalLedgerAccount.BalanceSheetAccount);
        //    Assert.False(generalLedgerAccount.CashAccount);
        //    Assert.Equal(chartOfAccounts, generalLedgerAccount.ChartOfAccountsWhereGeneralLedgerAccount);
        //    Assert.False(generalLedgerAccount.CostCenterAccount);
        //    Assert.False(generalLedgerAccount.CostCenterRequired);
        //    Assert.False(generalLedgerAccount.ExistCostCentersAllowed);
        //    Assert.False(generalLedgerAccount.CostUnitAccount);
        //    Assert.False(generalLedgerAccount.CostUnitRequired);
        //    Assert.False(generalLedgerAccount.ExistCostUnitsAllowed);
        //    Assert.Equal(new DebitCreditConstants(this.DatabaseSession).Credit, generalLedgerAccount.Side);
        //    Assert.False(generalLedgerAccount.ExistDefaultCostCenter);
        //    Assert.False(generalLedgerAccount.ExistDefaultCostUnit);
        //    Assert.IsNullOrEmpty(generalLedgerAccount.Description);
        //    Assert.Equal(generalLedgerAccountGroup, generalLedgerAccount.GeneralLedgerAccountGroup);
        //    Assert.Equal(generalLedgerAccountType, generalLedgerAccount.GeneralLedgerAccountType);
        //    Assert.Equal("Geplaats kapitaal", generalLedgerAccount.Name);
        //    Assert.False(generalLedgerAccount.Protected);
        //    Assert.False(generalLedgerAccount.ReconciliationAccount);
        // }

        // [Fact]
        // public void GivenGeneralLedgerAccountXmlWithGroupHierarchy_WhenImported_ThenGeneralLedgerAccountGroupsAreCreated()
        // {
        //    var filepath = string.Format("domain\\import\\minimaal genormaliseerd rekeningstelsel 2.xml");

        // new ChartOfAccountsImport(this.DatabaseSession).ImportChartOfAccounts(filepath);

        // Assert.Single(new GeneralLedgerAccounts(this.DatabaseSession).Extent().Count);
        //    Assert.Single(this.DatabaseSession.Extent<GeneralLedgerAccount>().Count);
        //    Assert.Single(this.DatabaseSession.Extent<ChartOfAccounts>().Count);
        //    Assert.Equal(3, this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().Count);
        //    Assert.Single(this.DatabaseSession.Extent<GeneralLedgerAccountType>().Count);
        //    Assert.Equal(0, this.DatabaseSession.Extent<CostCenter>().Count);

        // var chartOfAccounts = (ChartOfAccounts)this.DatabaseSession.Extent<ChartOfAccounts>().First;
        //    Assert.Equal("Minimum Algemeen Rekeningenstelsel", chartOfAccounts.Name);
        //    Assert.Single(chartOfAccounts.GeneralLedgerAccounts.Count);

        // var generalLedgerAccountGroups = this.DatabaseSession.Extent<GeneralLedgerAccountGroup>();
        //    generalLedgerAccountGroups.Filter.AddEquals(GeneralLedgerAccountGroups.Meta.Description, "Verworpen uitgaven");

        // var generalLedgerAccountGroup = (GeneralLedgerAccountGroup)generalLedgerAccountGroups.First;
        //    Assert.True(generalLedgerAccountGroup.ExistParent);

        // var parent = generalLedgerAccountGroup.Parent;
        //    Assert.Equal("Overige kosten", parent.Description);
        //    Assert.True(parent.ExistParent);

        // var grandParent = parent.Parent;
        //    Assert.Equal("Diensten en diverse goederen", grandParent.Description);
        //    Assert.False(grandParent.ExistParent);

        // var generalLedgerAccountType = (GeneralLedgerAccountType)this.DatabaseSession.Extent<GeneralLedgerAccountType>().First;
        //    Assert.Equal("Kosten", generalLedgerAccountType.Description);

        // var generalLedgerAccount = (GeneralLedgerAccount)this.DatabaseSession.Extent<GeneralLedgerAccount>().First;
        //    Assert.Equal("617910", generalLedgerAccount.AccountNumber);
        //    Assert.False(generalLedgerAccount.BalanceSheetAccount);
        //    Assert.False(generalLedgerAccount.CashAccount);
        //    Assert.Equal(chartOfAccounts, generalLedgerAccount.ChartOfAccountsWhereGeneralLedgerAccount);
        //    Assert.False(generalLedgerAccount.CostCenterAccount);
        //    Assert.False(generalLedgerAccount.CostCenterRequired);
        //    Assert.False(generalLedgerAccount.ExistCostCentersAllowed);
        //    Assert.False(generalLedgerAccount.CostUnitAccount);
        //    Assert.False(generalLedgerAccount.CostUnitRequired);
        //    Assert.False(generalLedgerAccount.ExistCostUnitsAllowed);
        //    Assert.Equal(new DebitCreditConstants(this.DatabaseSession).Debit, generalLedgerAccount.Side);
        //    Assert.False(generalLedgerAccount.ExistDefaultCostCenter);
        //    Assert.False(generalLedgerAccount.ExistDefaultCostUnit);
        //    Assert.IsNullOrEmpty(generalLedgerAccount.Description);
        //    Assert.Equal(generalLedgerAccountGroup, generalLedgerAccount.GeneralLedgerAccountGroup);
        //    Assert.Equal(generalLedgerAccountType, generalLedgerAccount.GeneralLedgerAccountType);
        //    Assert.Equal("Verworpen uitgaven niet-aftrekbare belastingen", generalLedgerAccount.Name);
        //    Assert.False(generalLedgerAccount.Protected);
        //    Assert.False(generalLedgerAccount.ReconciliationAccount);
        // }

        // [Fact]
        // public void GivenGeneralLedgerAccountXmlWithCostCenter_WhenImported_ThenCostCenterIsCreated()
        // {
        //    var filepath = string.Format("domain\\import\\minimaal genormaliseerd rekeningstelsel 3.xml");

        // new ChartOfAccountsImport(this.DatabaseSession).ImportChartOfAccounts(filepath);

        // Assert.Single(new GeneralLedgerAccounts(this.DatabaseSession).Extent().Count);
        //    Assert.Single(this.DatabaseSession.Extent<GeneralLedgerAccount>().Count);
        //    Assert.Single(this.DatabaseSession.Extent<ChartOfAccounts>().Count);
        //    Assert.Single(this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().Count);
        //    Assert.Single(this.DatabaseSession.Extent<GeneralLedgerAccountType>().Count);
        //    Assert.Single(this.DatabaseSession.Extent<CostCenter>().Count);

        // var chartOfAccounts = (ChartOfAccounts)this.DatabaseSession.Extent<ChartOfAccounts>().First;
        //    Assert.Equal("Minimum Algemeen Rekeningenstelsel", chartOfAccounts.Name);
        //    Assert.Single(chartOfAccounts.GeneralLedgerAccounts.Count);

        // var generalLedgerAccountGroup = (GeneralLedgerAccountGroup)this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().First;
        //    Assert.Equal("Kapitaal", generalLedgerAccountGroup.Description);
        //    Assert.False(generalLedgerAccountGroup.ExistParent);

        // var generalLedgerAccountType = (GeneralLedgerAccountType)this.DatabaseSession.Extent<GeneralLedgerAccountType>().First;
        //    Assert.Equal("Eigen vermogen, voorzieningen voor risico's en kosten en schulden op langer dan een jaar", generalLedgerAccountType.Description);

        // var costCenter = (CostCenter)this.DatabaseSession.Extent<CostCenter>().First;
        //    Assert.Equal("Misc", costCenter.Name);

        // var generalLedgerAccount = (GeneralLedgerAccount)this.DatabaseSession.Extent<GeneralLedgerAccount>().First;
        //    Assert.Equal("100000", generalLedgerAccount.AccountNumber);
        //    Assert.True(generalLedgerAccount.BalanceSheetAccount);
        //    Assert.False(generalLedgerAccount.CashAccount);
        //    Assert.Equal(chartOfAccounts, generalLedgerAccount.ChartOfAccountsWhereGeneralLedgerAccount);
        //    Assert.True(generalLedgerAccount.CostCenterAccount);
        //    Assert.False(generalLedgerAccount.CostCenterRequired);
        //    Assert.Equal(costCenter, generalLedgerAccount.CostCentersAllowed.First);
        //    Assert.False(generalLedgerAccount.CostUnitAccount);
        //    Assert.False(generalLedgerAccount.CostUnitRequired);
        //    Assert.False(generalLedgerAccount.ExistCostUnitsAllowed);
        //    Assert.Equal(new DebitCreditConstants(this.DatabaseSession).Credit, generalLedgerAccount.Side);
        //    Assert.Equal(costCenter, generalLedgerAccount.DefaultCostCenter);
        //    Assert.False(generalLedgerAccount.ExistDefaultCostUnit);
        //    Assert.IsNullOrEmpty(generalLedgerAccount.Description);
        //    Assert.Equal(generalLedgerAccountGroup, generalLedgerAccount.GeneralLedgerAccountGroup);
        //    Assert.Equal(generalLedgerAccountType, generalLedgerAccount.GeneralLedgerAccountType);
        //    Assert.Equal("Geplaats kapitaal", generalLedgerAccount.Name);
        //    Assert.False(generalLedgerAccount.Protected);
        //    Assert.False(generalLedgerAccount.ReconciliationAccount);
        // }
    }
}

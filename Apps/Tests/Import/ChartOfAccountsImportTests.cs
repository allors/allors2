//------------------------------------------------------------------------------------------------- 
// <copyright file="ChartOfAccountsImportTests.cs" company="Allors bvba">
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

    [TestFixture]
    public class ChartOfAccountsImportTests : DomainTest
    {
        //TODO: Import
        //[Test]
        //public void GivenGeneralLedgerAccountXml_WhenImported_ThenGeneralLedgerAccountIsCreated()
        //{
        //    var filepath = string.Format("domain\\import\\minimaal genormaliseerd rekeningstelsel 1.xml");

        //    new ChartOfAccountsImport(this.DatabaseSession).ImportChartOfAccounts(filepath);

        //    Assert.AreEqual(1, new GeneralLedgerAccounts(this.DatabaseSession).Extent().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<GeneralLedgerAccount>().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<ChartOfAccounts>().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<GeneralLedgerAccountType>().Count);
        //    Assert.AreEqual(0, this.DatabaseSession.Extent<CostCenter>().Count);

        //    var chartOfAccounts = (ChartOfAccounts)this.DatabaseSession.Extent<ChartOfAccounts>().First;
        //    Assert.AreEqual("Minimum Algemeen Rekeningenstelsel", chartOfAccounts.Name);
        //    Assert.AreEqual(1, chartOfAccounts.GeneralLedgerAccounts.Count);

        //    var generalLedgerAccountGroup = (GeneralLedgerAccountGroup)this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().First;
        //    Assert.AreEqual("Kapitaal", generalLedgerAccountGroup.Description);
        //    Assert.IsFalse(generalLedgerAccountGroup.ExistParent);

        //    var generalLedgerAccountType = (GeneralLedgerAccountType)this.DatabaseSession.Extent<GeneralLedgerAccountType>().First;
        //    Assert.AreEqual("Eigen vermogen, voorzieningen voor risico's en kosten en schulden op langer dan een jaar", generalLedgerAccountType.Description);

        //    var generalLedgerAccount = (GeneralLedgerAccount)this.DatabaseSession.Extent<GeneralLedgerAccount>().First;
        //    Assert.AreEqual("100000", generalLedgerAccount.AccountNumber);
        //    Assert.IsTrue(generalLedgerAccount.BalanceSheetAccount);
        //    Assert.IsFalse(generalLedgerAccount.CashAccount);
        //    Assert.AreEqual(chartOfAccounts, generalLedgerAccount.ChartOfAccountsWhereGeneralLedgerAccount);
        //    Assert.IsFalse(generalLedgerAccount.CostCenterAccount);
        //    Assert.IsFalse(generalLedgerAccount.CostCenterRequired);
        //    Assert.IsFalse(generalLedgerAccount.ExistCostCentersAllowed);
        //    Assert.IsFalse(generalLedgerAccount.CostUnitAccount);
        //    Assert.IsFalse(generalLedgerAccount.CostUnitRequired);
        //    Assert.IsFalse(generalLedgerAccount.ExistCostUnitsAllowed);
        //    Assert.AreEqual(new DebitCreditConstants(this.DatabaseSession).Credit, generalLedgerAccount.Side);
        //    Assert.IsFalse(generalLedgerAccount.ExistDefaultCostCenter);
        //    Assert.IsFalse(generalLedgerAccount.ExistDefaultCostUnit);
        //    Assert.IsNullOrEmpty(generalLedgerAccount.Description);
        //    Assert.AreEqual(generalLedgerAccountGroup, generalLedgerAccount.GeneralLedgerAccountGroup);
        //    Assert.AreEqual(generalLedgerAccountType, generalLedgerAccount.GeneralLedgerAccountType);
        //    Assert.AreEqual("Geplaats kapitaal", generalLedgerAccount.Name);
        //    Assert.IsFalse(generalLedgerAccount.Protected);
        //    Assert.IsFalse(generalLedgerAccount.ReconciliationAccount);
        //}

        //[Test]
        //public void GivenGeneralLedgerAccountXmlWithGroupHierarchy_WhenImported_ThenGeneralLedgerAccountGroupsAreCreated()
        //{
        //    var filepath = string.Format("domain\\import\\minimaal genormaliseerd rekeningstelsel 2.xml");

        //    new ChartOfAccountsImport(this.DatabaseSession).ImportChartOfAccounts(filepath);

        //    Assert.AreEqual(1, new GeneralLedgerAccounts(this.DatabaseSession).Extent().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<GeneralLedgerAccount>().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<ChartOfAccounts>().Count);
        //    Assert.AreEqual(3, this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<GeneralLedgerAccountType>().Count);
        //    Assert.AreEqual(0, this.DatabaseSession.Extent<CostCenter>().Count);

        //    var chartOfAccounts = (ChartOfAccounts)this.DatabaseSession.Extent<ChartOfAccounts>().First;
        //    Assert.AreEqual("Minimum Algemeen Rekeningenstelsel", chartOfAccounts.Name);
        //    Assert.AreEqual(1, chartOfAccounts.GeneralLedgerAccounts.Count);

        //    var generalLedgerAccountGroups = this.DatabaseSession.Extent<GeneralLedgerAccountGroup>();
        //    generalLedgerAccountGroups.Filter.AddEquals(GeneralLedgerAccountGroups.Meta.Description, "Verworpen uitgaven");

        //    var generalLedgerAccountGroup = (GeneralLedgerAccountGroup)generalLedgerAccountGroups.First; 
        //    Assert.IsTrue(generalLedgerAccountGroup.ExistParent);

        //    var parent = generalLedgerAccountGroup.Parent;
        //    Assert.AreEqual("Overige kosten", parent.Description);
        //    Assert.IsTrue(parent.ExistParent);

        //    var grandParent = parent.Parent;
        //    Assert.AreEqual("Diensten en diverse goederen", grandParent.Description);
        //    Assert.IsFalse(grandParent.ExistParent);

        //    var generalLedgerAccountType = (GeneralLedgerAccountType)this.DatabaseSession.Extent<GeneralLedgerAccountType>().First;
        //    Assert.AreEqual("Kosten", generalLedgerAccountType.Description);

        //    var generalLedgerAccount = (GeneralLedgerAccount)this.DatabaseSession.Extent<GeneralLedgerAccount>().First;
        //    Assert.AreEqual("617910", generalLedgerAccount.AccountNumber);
        //    Assert.IsFalse(generalLedgerAccount.BalanceSheetAccount);
        //    Assert.IsFalse(generalLedgerAccount.CashAccount);
        //    Assert.AreEqual(chartOfAccounts, generalLedgerAccount.ChartOfAccountsWhereGeneralLedgerAccount);
        //    Assert.IsFalse(generalLedgerAccount.CostCenterAccount);
        //    Assert.IsFalse(generalLedgerAccount.CostCenterRequired);
        //    Assert.IsFalse(generalLedgerAccount.ExistCostCentersAllowed);
        //    Assert.IsFalse(generalLedgerAccount.CostUnitAccount);
        //    Assert.IsFalse(generalLedgerAccount.CostUnitRequired);
        //    Assert.IsFalse(generalLedgerAccount.ExistCostUnitsAllowed);
        //    Assert.AreEqual(new DebitCreditConstants(this.DatabaseSession).Debit, generalLedgerAccount.Side);
        //    Assert.IsFalse(generalLedgerAccount.ExistDefaultCostCenter);
        //    Assert.IsFalse(generalLedgerAccount.ExistDefaultCostUnit);
        //    Assert.IsNullOrEmpty(generalLedgerAccount.Description);
        //    Assert.AreEqual(generalLedgerAccountGroup, generalLedgerAccount.GeneralLedgerAccountGroup);
        //    Assert.AreEqual(generalLedgerAccountType, generalLedgerAccount.GeneralLedgerAccountType);
        //    Assert.AreEqual("Verworpen uitgaven niet-aftrekbare belastingen", generalLedgerAccount.Name);
        //    Assert.IsFalse(generalLedgerAccount.Protected);
        //    Assert.IsFalse(generalLedgerAccount.ReconciliationAccount);
        //}

        //[Test]
        //public void GivenGeneralLedgerAccountXmlWithCostCenter_WhenImported_ThenCostCenterIsCreated()
        //{
        //    var filepath = string.Format("domain\\import\\minimaal genormaliseerd rekeningstelsel 3.xml");

        //    new ChartOfAccountsImport(this.DatabaseSession).ImportChartOfAccounts(filepath);

        //    Assert.AreEqual(1, new GeneralLedgerAccounts(this.DatabaseSession).Extent().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<GeneralLedgerAccount>().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<ChartOfAccounts>().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<GeneralLedgerAccountType>().Count);
        //    Assert.AreEqual(1, this.DatabaseSession.Extent<CostCenter>().Count);

        //    var chartOfAccounts = (ChartOfAccounts)this.DatabaseSession.Extent<ChartOfAccounts>().First;
        //    Assert.AreEqual("Minimum Algemeen Rekeningenstelsel", chartOfAccounts.Name);
        //    Assert.AreEqual(1, chartOfAccounts.GeneralLedgerAccounts.Count);

        //    var generalLedgerAccountGroup = (GeneralLedgerAccountGroup)this.DatabaseSession.Extent<GeneralLedgerAccountGroup>().First;
        //    Assert.AreEqual("Kapitaal", generalLedgerAccountGroup.Description);
        //    Assert.IsFalse(generalLedgerAccountGroup.ExistParent);

        //    var generalLedgerAccountType = (GeneralLedgerAccountType)this.DatabaseSession.Extent<GeneralLedgerAccountType>().First;
        //    Assert.AreEqual("Eigen vermogen, voorzieningen voor risico's en kosten en schulden op langer dan een jaar", generalLedgerAccountType.Description);

        //    var costCenter = (CostCenter)this.DatabaseSession.Extent<CostCenter>().First;
        //    Assert.AreEqual("Misc", costCenter.Name);

        //    var generalLedgerAccount = (GeneralLedgerAccount)this.DatabaseSession.Extent<GeneralLedgerAccount>().First;
        //    Assert.AreEqual("100000", generalLedgerAccount.AccountNumber);
        //    Assert.IsTrue(generalLedgerAccount.BalanceSheetAccount);
        //    Assert.IsFalse(generalLedgerAccount.CashAccount);
        //    Assert.AreEqual(chartOfAccounts, generalLedgerAccount.ChartOfAccountsWhereGeneralLedgerAccount);
        //    Assert.IsTrue(generalLedgerAccount.CostCenterAccount);
        //    Assert.IsFalse(generalLedgerAccount.CostCenterRequired);
        //    Assert.AreEqual(costCenter, generalLedgerAccount.CostCentersAllowed.First);
        //    Assert.IsFalse(generalLedgerAccount.CostUnitAccount);
        //    Assert.IsFalse(generalLedgerAccount.CostUnitRequired);
        //    Assert.IsFalse(generalLedgerAccount.ExistCostUnitsAllowed);
        //    Assert.AreEqual(new DebitCreditConstants(this.DatabaseSession).Credit, generalLedgerAccount.Side);
        //    Assert.AreEqual(costCenter, generalLedgerAccount.DefaultCostCenter);
        //    Assert.IsFalse(generalLedgerAccount.ExistDefaultCostUnit);
        //    Assert.IsNullOrEmpty(generalLedgerAccount.Description);
        //    Assert.AreEqual(generalLedgerAccountGroup, generalLedgerAccount.GeneralLedgerAccountGroup);
        //    Assert.AreEqual(generalLedgerAccountType, generalLedgerAccount.GeneralLedgerAccountType);
        //    Assert.AreEqual("Geplaats kapitaal", generalLedgerAccount.Name);
        //    Assert.IsFalse(generalLedgerAccount.Protected);
        //    Assert.IsFalse(generalLedgerAccount.ReconciliationAccount);
        //}
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralLedgerAccount.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Resources;

    public partial class GeneralLedgerAccount
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            

            if (!this.ExistCashAccount)
            {
                this.CashAccount = false;
            }

            if (!this.ExistCostCenterAccount)
            {
                this.CostCenterAccount = false;
            }

            if (!this.ExistCostCenterRequired)
            {
                this.CostCenterRequired = false;
            }

            if (!this.ExistCostUnitAccount)
            {
                this.CostUnitAccount = false;
            }

            if (!this.ExistCostUnitRequired)
            {
                this.CostUnitRequired = false;
            }

            if (!this.ExistReconciliationAccount)
            {
                this.ReconciliationAccount = false;
            }

            if (!this.ExistProtected)
            {
                this.Protected = false;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistChartOfAccountsWhereGeneralLedgerAccount)
            {
                var extent = this.Strategy.Session.Extent<GeneralLedgerAccount>();
                extent.Filter.AddEquals(GeneralLedgerAccounts.Meta.ChartOfAccountsWhereGeneralLedgerAccount, this.ChartOfAccountsWhereGeneralLedgerAccount);
                extent.Filter.AddEquals(GeneralLedgerAccounts.Meta.AccountNumber, this.AccountNumber);

                if (extent.Count > 1)
                {
                    derivation.Log.AddError(this, GeneralLedgerAccounts.Meta.AccountNumber, ErrorMessages.AccountNumberUniqueWithinChartOfAccounts);
                }
            }

            if (!this.CostCenterAccount && this.CostCenterRequired)
            {
                derivation.Log.AddError(this, GeneralLedgerAccounts.Meta.CostCenterRequired, ErrorMessages.NotACostCenterAccount);
            }

            if (this.CostCenterAccount && this.ExistDefaultCostCenter)
            {
                if (!this.CostCentersAllowed.Contains(this.DefaultCostCenter))
                {
                    derivation.Log.AddError(this, GeneralLedgerAccounts.Meta.DefaultCostCenter, ErrorMessages.CostCenterNotAllowed);
                }
            }

            if (!this.CostUnitAccount && this.CostUnitRequired)
            {
                derivation.Log.AddError(this, GeneralLedgerAccounts.Meta.CostCenterRequired, ErrorMessages.NotACostUnitAccount);
            }

            if (this.CostUnitAccount && this.ExistDefaultCostUnit)
            {
                if (!this.CostUnitsAllowed.Contains(this.DefaultCostUnit))
                {
                    derivation.Log.AddError(this, GeneralLedgerAccounts.Meta.DefaultCostUnit, ErrorMessages.CostUnitNotAllowed);
                }
            }
        }
    }
}
// <copyright file="GeneralLedgerAccount.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Resources;

    public partial class GeneralLedgerAccount
    {
        public void BaseOnBuild(ObjectOnBuild method)
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

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistChartOfAccountsWhereGeneralLedgerAccount)
            {
                var extent = this.Strategy.Session.Extent<GeneralLedgerAccount>();
                extent.Filter.AddEquals(this.Meta.ChartOfAccountsWhereGeneralLedgerAccount, this.ChartOfAccountsWhereGeneralLedgerAccount);
                extent.Filter.AddEquals(this.Meta.AccountNumber, this.AccountNumber);

                if (extent.Count > 1)
                {
                    derivation.Validation.AddError(this, this.Meta.AccountNumber, ErrorMessages.AccountNumberUniqueWithinChartOfAccounts);
                }
            }

            if (!this.CostCenterAccount && this.CostCenterRequired)
            {
                derivation.Validation.AddError(this, this.Meta.CostCenterRequired, ErrorMessages.NotACostCenterAccount);
            }

            if (this.CostCenterAccount && this.ExistDefaultCostCenter)
            {
                if (!this.CostCentersAllowed.Contains(this.DefaultCostCenter))
                {
                    derivation.Validation.AddError(this, this.Meta.DefaultCostCenter, ErrorMessages.CostCenterNotAllowed);
                }
            }

            if (!this.CostUnitAccount && this.CostUnitRequired)
            {
                derivation.Validation.AddError(this, this.Meta.CostCenterRequired, ErrorMessages.NotACostUnitAccount);
            }

            if (this.CostUnitAccount && this.ExistDefaultCostUnit)
            {
                if (!this.CostUnitsAllowed.Contains(this.DefaultCostUnit))
                {
                    derivation.Validation.AddError(this, this.Meta.DefaultCostUnit, ErrorMessages.CostUnitNotAllowed);
                }
            }
        }
    }
}

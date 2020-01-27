// <copyright file="OrganisationGlAccount.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class OrganisationGlAccount
    {
        public bool IsNeutralAccount() =>
            !this.IsBankAccount() && !this.IsCashAccount() && !this.IsCostAccount() && !this.IsCostAccount()
            && !this.IsCreditorAccount() && !this.IsDebtorAccount() && !this.IsInventoryAccount();

        public bool IsBankAccount()
        {
            if (this.ExistJournalWhereContraAccount &&
                this.JournalWhereContraAccount.JournalType.Equals(new JournalTypes(this.Strategy.Session).Bank))
            {
                return true;
            }

            if (this.HasBankStatementTransactions)
            {
                return true;
            }

            foreach (OrganisationGlAccountBalance organisationGlAccountBalance in this.OrganisationGlAccountBalancesWhereOrganisationGlAccount)
            {
                foreach (AccountingTransactionDetail accountingTransactionDetail in organisationGlAccountBalance.AccountingTransactionDetailsWhereOrganisationGlAccountBalance)
                {
                    if (accountingTransactionDetail.AccountingTransactionWhereAccountingTransactionDetail.AccountingTransactionNumber.AccountingTransactionType.Equals(new AccountingTransactionTypes(this.Strategy.Session).BankStatement))
                    {
                        this.HasBankStatementTransactions = true;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsDebtorAccount() => false;

        public bool IsCreditorAccount() => false;

        public bool IsInventoryAccount() => false;

        public bool IsTurnOverAccount() => false;

        public bool IsCostAccount() => false;

        public bool IsCashAccount() => false;

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistFromDate)
            {
                this.FromDate = this.Session().Now();
            }

            this.HasBankStatementTransactions = false;
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistInternalOrganisation && internalOrganisations.Count() == 1)
            {
                this.InternalOrganisation = internalOrganisations.First();
            }
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationGlAccount.cs" company="Allors bvba">
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
    public partial class OrganisationGlAccount
    {
        public bool IsNeutralAccount()
        {
            return !this.IsBankAccount() && !this.IsCashAccount() && !this.IsCostAccount() && !this.IsCostAccount()
                   && !this.IsCreditorAccount() && !this.IsDebtorAccount() && !this.IsInventoryAccount();
        }

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

        public bool IsDebtorAccount()
        {
            return false;
        }

        public bool IsCreditorAccount()
        {
            return false;
        }

        public bool IsInventoryAccount()
        {
            return false;
        }

        public bool IsTurnOverAccount()
        {
            return false;
        }

        public bool IsCostAccount()
        {
            return false;
        }

        public bool IsCashAccount()
        {
            return false;
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            this.HasBankStatementTransactions = false;
        }
    }
}
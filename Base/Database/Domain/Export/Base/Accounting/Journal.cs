// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Journal.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using Resources;

    public partial class Journal
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistBlockUnpaidTransactions)
            {
                this.BlockUnpaidTransactions = false;
            }

            if (!this.ExistCloseWhenInBalance)
            {
                this.CloseWhenInBalance = false;
            }

            if (!this.ExistUseAsDefault)
            {
                this.UseAsDefault = false;
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.BaseOnDeriveContraAccount(derivation);
            this.BaseOnDerivePreviousJournalType(derivation);
        }

        public void BaseOnDeriveContraAccount(IDerivation derivation)
        {
            if (this.ExistPreviousContraAccount)
            {
                if (this.PreviousContraAccount.ExistJournalEntryDetailsWhereGeneralLedgerAccount)
                {
                    derivation.Validation.AssertAreEqual(this, this.Meta.ContraAccount, this.Meta.PreviousContraAccount);
                }
                else
                {
                    this.PreviousContraAccount = this.ContraAccount;
                }
            }
            else
            {
                if (this.ExistJournalType && this.JournalType.Equals(new JournalTypes(this.Strategy.Session).Bank))
                {
                    // initial derivation of ContraAccount, PreviousContraAccount does not exist yet.
                    if (this.ExistContraAccount)
                    {
                        var savedContraAccount = this.ContraAccount;
                        this.RemoveContraAccount();
                        if (!savedContraAccount.IsNeutralAccount())
                        {
                            derivation.Validation.AddError(this, this.Meta.ContraAccount, ErrorMessages.GeneralLedgerAccountNotNeutral);
                        }

                        if (!savedContraAccount.GeneralLedgerAccount.BalanceSheetAccount)
                        {
                            derivation.Validation.AddError(this, this.Meta.ContraAccount, ErrorMessages.GeneralLedgerAccountNotBalanceAccount);
                        }

                        this.ContraAccount = savedContraAccount;
                    }
                }

                if (!derivation.Validation.HasErrors)
                {
                    this.PreviousContraAccount = this.ContraAccount;
                }
            }
        }

        public void BaseOnDerivePreviousJournalType(IDerivation derivation)
        {
            if (this.ExistPreviousJournalType)
            {
                if (this.ExistPreviousContraAccount && this.PreviousContraAccount.ExistJournalEntryDetailsWhereGeneralLedgerAccount)
                {
                    derivation.Validation.AssertAreEqual(this, this.Meta.JournalType, this.Meta.PreviousJournalType);
                }
                else
                {
                    this.PreviousJournalType = this.JournalType;
                }
            }
            else
            {
                this.PreviousJournalType = this.JournalType;
            }
        }
    }
}
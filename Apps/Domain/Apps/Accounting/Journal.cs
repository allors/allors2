// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Journal.cs" company="Allors bvba">
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

    public partial class Journal
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {

            if (!this.ExistInternalOrganisation)
            {
                this.InternalOrganisation = Domain.Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }

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

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.DeriveContraAccount(derivation);
            this.DerivePreviousJournalType(derivation);
        }

        public void AppsOnDeriveContraAccount(IDerivation derivation)
        {
            if (this.ExistPreviousContraAccount)
            {
                if (this.PreviousContraAccount.ExistJournalEntryDetailsWhereGeneralLedgerAccount)
                {
                    derivation.Log.AssertAreEqual(this, Journals.Meta.ContraAccount, Journals.Meta.PreviousContraAccount);
                }
                else
                {
                    this.PreviousContraAccount = this.ContraAccount;
                }
            }
            else
            {
                if (this.ExistJournalType &&this.JournalType.Equals(new JournalTypes(this.Strategy.Session).Bank))
                {
                    // initial derivation of ContraAccount, PreviousContraAccount does not exist yet.
                    if (this.ExistContraAccount)
                    {
                        var savedContraAccount = this.ContraAccount;
                        this.RemoveContraAccount();
                        if (!savedContraAccount.IsNeutralAccount())
                        {
                            derivation.Log.AddError(this, Journals.Meta.ContraAccount, ErrorMessages.GeneralLedgerAccountNotNeutral);
                        }

                        if (!savedContraAccount.GeneralLedgerAccount.BalanceSheetAccount)
                        {
                            derivation.Log.AddError(this, Journals.Meta.ContraAccount, ErrorMessages.GeneralLedgerAccountNotBalanceAccount);
                        }

                        this.ContraAccount = savedContraAccount;
                    }
                }

                if (!derivation.Log.HasErrors)
                {
                    this.PreviousContraAccount = this.ContraAccount;
                }
            }
        }

        public void AppsOnDerivePreviousJournalType(IDerivation derivation)
        {
            if (this.ExistPreviousJournalType)
            {
                if (this.ExistPreviousContraAccount && this.PreviousContraAccount.ExistJournalEntryDetailsWhereGeneralLedgerAccount)
                {
                    derivation.Log.AssertAreEqual(this, Journals.Meta.JournalType, Journals.Meta.PreviousJournalType);
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
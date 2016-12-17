// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnCreditCard.cs" company="Allors bvba">
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
    using System;
    using Meta;

    public partial class OwnCreditCard
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistIsActive)
            {
                this.IsActive = true;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistInternalOrganisationWherePaymentMethod && this.InternalOrganisationWherePaymentMethod.DoAccounting)
            { 
                derivation.Validation.AssertExists(this, M.OwnCreditCard.Creditor);
                derivation.Validation.AssertAtLeastOne(this, M.Cash.GeneralLedgerAccount, M.Cash.Journal);
            }

            if (this.ExistCreditCard)
            {
                if (this.CreditCard.ExpirationYear <= DateTime.UtcNow.Year && this.CreditCard.ExpirationMonth <= DateTime.UtcNow.Month)
                {
                    this.IsActive = false;
                }
            }

            derivation.Validation.AssertExistsAtMostOne(this, M.Cash.GeneralLedgerAccount, M.Cash.Journal);
        }
    }
}
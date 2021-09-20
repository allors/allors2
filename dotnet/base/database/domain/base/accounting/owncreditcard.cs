// <copyright file="OwnCreditCard.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class OwnCreditCard
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistIsActive)
            {
                this.IsActive = true;
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistInternalOrganisationWherePaymentMethod && this.InternalOrganisationWherePaymentMethod.DoAccounting)
            {
                derivation.Validation.AssertAtLeastOne(this, M.Cash.GeneralLedgerAccount, M.Cash.Journal);
            }

            if (this.ExistCreditCard)
            {
                if (this.CreditCard.ExpirationYear <= this.Session().Now().Year && this.CreditCard.ExpirationMonth <= this.Session().Now().Month)
                {
                    this.IsActive = false;
                }
            }

            derivation.Validation.AssertExistsAtMostOne(this, M.Cash.GeneralLedgerAccount, M.Cash.Journal);
        }
    }
}

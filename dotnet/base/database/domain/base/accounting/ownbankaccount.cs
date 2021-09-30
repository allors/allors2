// <copyright file="OwnBankAccount.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class OwnBankAccount
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistIsActive)
            {
                this.IsActive = true;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRole(this, this.Meta.BankAccount))
            {
                if (this.ExistBankAccount)
                {
                    iteration.Mark(this.BankAccount);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistInternalOrganisationWhereActiveCollectionMethod && this.InternalOrganisationWhereActiveCollectionMethod.DoAccounting)
            {
                derivation.Validation.AssertAtLeastOne(this, M.Cash.GeneralLedgerAccount, M.Cash.Journal);
            }

            derivation.Validation.AssertExistsAtMostOne(this, M.Cash.GeneralLedgerAccount, M.Cash.Journal);
        }
    }
}

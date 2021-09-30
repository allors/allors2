// <copyright file="Cash.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Cash
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

            if (this.InternalOrganisationWhereActiveCollectionMethod?.DoAccounting ?? false)
            {
                derivation.Validation.AssertAtLeastOne(this, this.Meta.GeneralLedgerAccount, this.Meta.Journal);
            }

            derivation.Validation.AssertExistsAtMostOne(this, this.Meta.GeneralLedgerAccount, this.Meta.Journal);
        }
    }
}

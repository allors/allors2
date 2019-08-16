
// <copyright file="OrganisationGlAccountBalance.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class OrganisationGlAccountBalance
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistAccountingPeriod)
            {
                derivation.AddDependency(this, this.AccountingPeriod);
            }
        }
    }
}

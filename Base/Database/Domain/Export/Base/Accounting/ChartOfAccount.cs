
// <copyright file="ChartOfAccount.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ChartOfAccounts
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (GeneralLedgerAccount generalLedgerAccount in this.GeneralLedgerAccounts)
            {
                derivation.AddDependency(this, generalLedgerAccount);
            }
        }
    }
}

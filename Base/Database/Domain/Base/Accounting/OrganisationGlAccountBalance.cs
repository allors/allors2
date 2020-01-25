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
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistAccountingPeriod)
                {
                    iteration.AddDependency(this, this.AccountingPeriod);
                    iteration.Mark(this.AccountingPeriod);
                }
            }
        }
    }
}

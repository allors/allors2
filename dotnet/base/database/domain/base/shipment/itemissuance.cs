// <copyright file="ItemIssuance.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ItemIssuance
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                iteration.AddDependency(this.ShipmentItem, this);
                iteration.Mark(this.ShipmentItem);
            }
        }

        public void BaseOnPostBuild(ObjectOnPostBuild method)
        {
            if (!this.ExistIssuanceDateTime)
            {
                this.IssuanceDateTime = this.Strategy.Session.Now();
            }
        }
    }
}

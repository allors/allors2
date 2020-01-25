// <copyright file="SyncDepthC1.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SyncDepthC1
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (!changeSet.IsCreated(this) && changeSet.HasChangedRoles(this, RelationKind.Regular))
            {
                iteration.AddDependency(this, this.SyncRootWhereSyncDepth1);
            }

            if (iteration.IsMarked(this))
            {
                iteration.Mark(this, this.SyncRootWhereSyncDepth1);
            }
        }

        public void Sync()
        {
            if (!this.ExistSyncDepth2)
            {
                this.SyncDepth2 = new SyncDepth2Builder(this.strategy.Session).Build();
            }
        }
    }
}

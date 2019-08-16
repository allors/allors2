// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncDepthC1.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class SyncDepthC1
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (!derivation.IsCreated(this) && derivation.IsModified(this, RelationKind.Regular))
            {
                derivation.AddDependency(this, this.SyncRootWhereSyncDepth1);
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

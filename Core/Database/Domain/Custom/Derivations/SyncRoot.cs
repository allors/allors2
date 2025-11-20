// <copyright file="SyncRoot.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SyncRoot
    {
        public void CustomOnDerive(ObjectOnDerive method) => this.Sync();

        private void Sync()
        {
            if (!this.ExistSyncDepth1)
            {
                this.SyncDepth1 = new SyncDepthC1Builder(this.strategy.Session).Build();
            }

            this.SyncDepth1.Sync();
        }
    }
}

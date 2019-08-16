// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncDepth2.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class SyncDepth2
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (!derivation.IsCreated(this) && derivation.IsModified(this, RelationKind.Regular))
            {
                derivation.AddDependency(this, this.SyncDepthI1WhereSyncDepth2);
            }
        }
    }
}

// <copyright file="SyncDepth2.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Derivations;

    public partial class SyncDepth2
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (!changeSet.IsCreated(this) && changeSet.HasChangedRoles(this, RelationKind.Regular))
            {
                iteration.AddDependency(this, this.SyncDepthI1WhereSyncDepth2);
                iteration.Mark(this, this.SyncDepthI1WhereSyncDepth2);
            }
        }
    }
}

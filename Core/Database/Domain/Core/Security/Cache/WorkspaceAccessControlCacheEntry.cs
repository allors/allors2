// <copyright file="AccessControlCacheEntry.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class WorkspaceAccessControlCacheEntry
    {
        internal WorkspaceAccessControlCacheEntry(AccessControl accessControl)
        {
            this.CacheId = accessControl.CacheId;

            this.EffectiveWorkspacePermissionIds = new HashSet<long>(accessControl.EffectiveWorkspacePermissions.Select(v => v.Id));
        }

        public Guid CacheId { get; }

        public HashSet<long> EffectiveWorkspacePermissionIds { get; }
    }
}

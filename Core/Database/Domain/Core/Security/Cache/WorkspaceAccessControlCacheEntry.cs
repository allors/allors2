// <copyright file="AccessControlCacheEntry.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
            if (!string.IsNullOrWhiteSpace(accessControl.EffectiveWorkspacePermissionIds))
            {
                this.EffectiveWorkspacePermissionIds = new HashSet<long>(accessControl.EffectiveWorkspacePermissionIds.Split(',').Select(long.Parse));
                
            }
            else
            {
                this.EffectiveWorkspacePermissionIds = new HashSet<long>();
            }
        }

        public Guid CacheId { get; }

        public HashSet<long> EffectiveWorkspacePermissionIds { get; }
    }
}

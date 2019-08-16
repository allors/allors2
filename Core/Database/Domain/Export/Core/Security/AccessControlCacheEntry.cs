
// <copyright file="AccessControlCacheEntry.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AccessControlCacheEntry
    {
        internal AccessControlCacheEntry(AccessControl accessControl)
        {
            this.CacheId = accessControl.CacheId;

            this.EffectiveUserIds = new HashSet<long>(accessControl.EffectiveUsers.Select(v => v.Id));
            this.EffectivePermissionIds = new HashSet<long>(accessControl.EffectivePermissions.Select(v => v.Id));
        }

        public Guid CacheId { get; }

        public HashSet<long> EffectiveUserIds { get; }

        public HashSet<long> EffectivePermissionIds { get; }
    }
}

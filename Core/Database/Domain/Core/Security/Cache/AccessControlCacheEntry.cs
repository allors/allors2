// <copyright file="AccessControlCacheEntry.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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

            this.EffectivePermissionIds = new HashSet<long>(accessControl.EffectivePermissions.Select(v => v.Id));
        }

        public Guid CacheId { get; }

        public HashSet<long> EffectivePermissionIds { get; }
    }
}

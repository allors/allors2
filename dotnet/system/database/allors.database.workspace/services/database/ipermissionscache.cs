// <copyright file="ICacheService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Services
{
    using System;
    using System.Linq;
    using Database.Security;

    public interface IPermissionsCache
    {
        IPermissionsCacheEntry Create(IGrouping<Guid, IPermission> permissions);

        IPermissionsCacheEntry Get(Guid classId);

        void Clear();
    }
}

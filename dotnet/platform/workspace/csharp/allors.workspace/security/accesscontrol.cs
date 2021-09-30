// <copyright file="Context.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;

    public class AccessControl
    {
        public AccessControl(long id, long version, ISet<long> permissionIds)
        {
            this.Id = id;
            this.Version = version;
            this.PermissionIds = permissionIds;
        }

        public long Id { get; }

        public long Version { get; }

        public ISet<long> PermissionIds { get; }
    }
}

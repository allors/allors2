// <copyright file="SessionObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol.Remote;

    public class ResponseContext
    {
        private readonly Dictionary<long, AccessControl> accessControlById;
        private readonly Dictionary<long, Permission> permissionById;

        public ResponseContext(Workspace workspace, Dictionary<long, AccessControl> accessControlById, Dictionary<long, Permission> permissionById)
        {
            this.Workspace = workspace;
            this.accessControlById = accessControlById;
            this.permissionById = permissionById;

            this.MissingAccessControlIds = new HashSet<long>();
            this.MissingPermissionIds = new HashSet<long>();
        }

        internal HashSet<long> MissingAccessControlIds { get; }

        internal HashSet<long> MissingPermissionIds { get; }

        internal Workspace Workspace { get; }

        internal string ReadSortedAccessControlIds(string value)
        {
            foreach (var accessControlId in value
                .Split(Encoding.Separator)
                .Select(v => long.Parse(v))
                .Where(v => !this.accessControlById.ContainsKey(v)))
            {
                this.MissingAccessControlIds.Add(accessControlId);
            }

            return value;
        }

        internal string ReadSortedDeniedPermissionIds(string compressed)
        {
            var value = compressed;

            foreach (var permissionId in value
                .Split(Encoding.Separator)
                .Select(v => long.Parse(v))
                .Where(v => !this.permissionById.ContainsKey(v)))
            {
                this.MissingPermissionIds.Add(permissionId);
            }

            return value;
        }
    }
}

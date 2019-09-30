// <copyright file="SessionObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;

    public class PullResponseContext
    {
        private readonly Dictionary<long, AccessControl> accessControlById;
        private readonly Dictionary<long, Permission> permissionById;
        private readonly Decompressor decompressor;

        public PullResponseContext(Dictionary<long, AccessControl> accessControlById, Dictionary<long, Permission> permissionById)
        {
            this.accessControlById = accessControlById;
            this.permissionById = permissionById;
            this.decompressor = new Decompressor();

            this.MissingAccessControlIds = new HashSet<long>();
            this.MissingPermissionIds = new HashSet<long>();
        }

        internal HashSet<long> MissingAccessControlIds { get; }

        internal HashSet<long> MissingPermissionIds { get; }

        internal string ReadSortedAccessControlIds(string compressed)
        {
            var value = this.decompressor.Read(compressed, out var first);

            if (first)
            {
                foreach (var accessControlId in value
                    .Split(Compressor.ItemSeparator)
                    .Select(v => long.Parse(v))
                    .Where(v => !this.accessControlById.ContainsKey(v)))
                {
                    this.MissingAccessControlIds.Add(accessControlId);
                }
            }

            return value;
        }

        internal string ReadSortedDeniedPermissionIds(string compressed)
        {
            var value = this.decompressor.Read(compressed, out var first);

            if (first)
            {
                foreach (var permissionId in value
                    .Split(Compressor.ItemSeparator)
                    .Select(v => long.Parse(v))
                    .Where(v => !this.permissionById.ContainsKey(v)))
                {
                    this.MissingPermissionIds.Add(permissionId);
                }
            }

            return value;
        }
    }
}

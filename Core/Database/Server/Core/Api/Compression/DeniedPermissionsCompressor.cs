// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    internal class DeniedPermissionsCompressor
    {
        private readonly AccessControlLists acls;
        private readonly Dictionary<string, string> indexBySortedDeniedPermissionIds;

        private int counter;

        internal DeniedPermissionsCompressor(AccessControlLists acls)
        {
            this.acls = acls;
            this.indexBySortedDeniedPermissionIds = new Dictionary<string, string>();
            this.counter = 0;
        }

        public string Write(IObject @object)
        {
            var deniedPermissionIds = this.acls[@object].DeniedPermissionIds;
            if (deniedPermissionIds == null)
            {
                return null;
            }

            var sortedDeniedPermissionIds = string.Join(Compression.ItemSeparator, deniedPermissionIds.OrderBy(v => v));
            if (this.indexBySortedDeniedPermissionIds.TryGetValue(sortedDeniedPermissionIds, out var index))
            {
                return index;
            }

            index = (++this.counter).ToString();
            this.indexBySortedDeniedPermissionIds.Add(sortedDeniedPermissionIds, index);
            return $"{Compression.IndexMarker}{index}{Compression.IndexMarker}{sortedDeniedPermissionIds}";
        }
    }
}

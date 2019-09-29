// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Org.BouncyCastle.Asn1.X509;

    internal class DeniedPermissionsCompression
    {
        private readonly Dictionary<string, string> indexBySortedDeniedPermissionIds;

        private int counter;

        internal DeniedPermissionsCompression()
        {
            this.indexBySortedDeniedPermissionIds = new Dictionary<string, string>();
            this.counter = 0;
        }

        public string Write(IAccessControlList acl)
        {
            if (acl.DeniedPermissionIds == null)
            {
                return null;
            }

            var sortedDeniedPermissionIds = string.Join(',', acl.DeniedPermissionIds.OrderBy(v => v));
            if (this.indexBySortedDeniedPermissionIds.TryGetValue(sortedDeniedPermissionIds, out var index))
            {
                return index;
            }

            index = (++this.counter).ToString();
            this.indexBySortedDeniedPermissionIds.Add(sortedDeniedPermissionIds, index);
            return $":{index}:{sortedDeniedPermissionIds}";
        }
    }
}

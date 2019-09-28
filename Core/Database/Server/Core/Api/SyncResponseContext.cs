// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using Domain;
    using Meta;

    internal class SyncResponseContext
    {
        private readonly Dictionary<IMetaObject, string> keyByMetaObject;
        private readonly Dictionary<string, string> keyBySortedPermissionIds;

        private int indexForMetaObject;
        private int indexForSortedPermissionIds;

        internal SyncResponseContext()
        {
            this.keyByMetaObject = new Dictionary<IMetaObject, string>();
            this.indexForMetaObject = 0;

            this.keyBySortedPermissionIds = new Dictionary<string, string>();
            this.indexForSortedPermissionIds = 0;
        }

        public string Write(IMetaObject metaObject)
        {
            if (this.keyByMetaObject.TryGetValue(metaObject, out var key))
            {
                return key;
            }

            key = (++this.indexForMetaObject).ToString();
            return $":{key}:{metaObject.Id.ToString("D").ToLower()}";
        }

        public string Write(IAccessControlList acl)
        {
            var sortedPermissionIds = acl.SortedWorkspacePermissionIds;
            if (this.keyBySortedPermissionIds.TryGetValue(sortedPermissionIds, out var key))
            {
                return key;
            }

            key = (++this.indexForSortedPermissionIds).ToString();
            return $":{key}:{sortedPermissionIds}";
        }
    }
}

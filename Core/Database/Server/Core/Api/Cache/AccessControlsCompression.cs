// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    internal class AccessControlsCompression
    {
        private readonly Dictionary<string, string> indexBySortedAccessControlIds;

        private int counter;

        internal AccessControlsCompression()
        {
            this.indexBySortedAccessControlIds = new Dictionary<string, string>();
            this.counter = 0;
        }

        public string Write(IAccessControlList acl)
        {
            if (acl.AccessControls == null)
            {
                return null;
            }

            var sortedAccessControlIds = string.Join(',', acl.AccessControls.OrderBy(v => v.Id).Select(v => v.Id));
            if (this.indexBySortedAccessControlIds.TryGetValue(sortedAccessControlIds, out var index))
            {
                return index;
            }

            index = (++this.counter).ToString();
            this.indexBySortedAccessControlIds.Add(sortedAccessControlIds, index);
            return $":{index}:{sortedAccessControlIds}";
        }
    }
}

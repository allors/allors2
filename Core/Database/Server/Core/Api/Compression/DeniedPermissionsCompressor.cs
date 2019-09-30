// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Linq;
    using Domain;
    using Protocol;

    internal class DeniedPermissionsCompressor
    {
        private readonly Compressor compressor;
        private readonly AccessControlLists acls;

        internal DeniedPermissionsCompressor(Compressor compressor, AccessControlLists acls)
        {
            this.compressor = compressor;
            this.acls = acls;
        }

        public string Write(IObject @object)
        {
            var deniedPermissionIds = this.acls[@object].DeniedPermissionIds;
            if (deniedPermissionIds == null)
            {
                return null;
            }

            var sortedPermissionIds = string.Join(Compressor.ItemSeparator, deniedPermissionIds.OrderBy(v => v));
            return this.compressor.Write(sortedPermissionIds);
        }
    }
}

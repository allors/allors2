// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Linq;
    using Domain;
    using Protocol;
    using Protocol.Remote;

    internal class AccessControlsWriter
    {
        private readonly IAccessControlLists acls;

        internal AccessControlsWriter(IAccessControlLists acls)
        {
            this.acls = acls;
        }

        public string Write(IObject @object)
        {
            var accessControls = this.acls[@object].AccessControls;
            if (accessControls == null || accessControls.Length == 0)
            {
                return null;
            }

            var sortedAccessControlIds = string.Join(Encoding.Separator, accessControls.OrderBy(v => v.Id).Select(v => v.Id));
            return sortedAccessControlIds;
        }
    }
}

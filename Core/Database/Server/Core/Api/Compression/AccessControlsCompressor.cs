// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Linq;
    using Domain;
    using Protocol;

    internal class AccessControlsCompressor
    {
        private readonly Compressor compressor;
        private readonly IAccessControlLists acls;

        internal AccessControlsCompressor(Compressor compressor, IAccessControlLists acls)
        {
            this.compressor = compressor;
            this.acls = acls;
        }

        public string Write(IObject @object)
        {
            var accessControls = this.acls[@object].AccessControls;
            if (accessControls == null)
            {
                return null;
            }

            var sortedAccessControlIds = string.Join(Compressor.ItemSeparator, accessControls.OrderBy(v => v.Id).Select(v => v.Id));
            return this.compressor.Write(sortedAccessControlIds);
        }
    }
}

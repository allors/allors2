// <copyright file="SessionObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Server;

    public class SyncResponseContext
    {
        public SyncResponseContext(IWorkspace workspace)
        {
            this.Workspace = workspace;
            this.MetaObjectDecompressor = new MetaObjectDecompressor(this.Workspace);
            this.AccessControlsDecompressor = new AccessControlsDecompressor(this.Workspace);
            this.DeniedPermissionsDecompressor = new DeniedPermissionsDecompressor(this.Workspace);
        }

        public IWorkspace Workspace { get; }

        internal AccessControlsDecompressor AccessControlsDecompressor { get; set; }

        internal DeniedPermissionsDecompressor DeniedPermissionsDecompressor { get; set; }

        internal MetaObjectDecompressor MetaObjectDecompressor { get; }
    }
}

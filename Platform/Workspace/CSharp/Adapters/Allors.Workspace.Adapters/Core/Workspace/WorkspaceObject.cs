// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Allors.Protocol.Remote.Sync;
    using Allors.Workspace.Meta;
    using System.Linq;
    using Security;
    using Server;

    public class WorkspaceObject : IWorkspaceObject
    {
        internal WorkspaceObject(SyncResponseContext ctx, SyncResponseObject syncResponseObject)
        {
            this.Workspace = ctx.Workspace;
            this.Id = long.Parse(syncResponseObject.I);
            this.Version = !string.IsNullOrEmpty(syncResponseObject.V) ? long.Parse(syncResponseObject.V) : 0;
            this.Class = (IClass)ctx.MetaObjectDecompressor.Read(syncResponseObject.T);
            this.Roles = syncResponseObject.R?.Select(v => new WorkspaceRole(ctx.MetaObjectDecompressor, v)).Cast<IWorkspaceRole>().ToArray();
        }

        public IAccessControlList AccessControlList { get; }

        public IClass Class { get; }

        public long Id { get; }

        public IWorkspaceRole[] Roles { get; }

        public long Version { get; }

        public IWorkspace Workspace { get; }
    }
}

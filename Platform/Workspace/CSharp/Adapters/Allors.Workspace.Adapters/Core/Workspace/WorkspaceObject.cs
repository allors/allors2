// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Allors.Protocol.Remote.Sync;
    using Allors.Workspace.Meta;
    using System.Linq;

    public class WorkspaceObject : IWorkspaceObject
    {
        internal WorkspaceObject(SyncResponseContext ctx, SyncResponseObject syncResponseObject)
        {
            this.Workspace = ctx.Workspace;
            this.UserSecurityHash = ctx.UserSecurityHash;
            this.Id = long.Parse(syncResponseObject.I);
            this.Version = !string.IsNullOrEmpty(syncResponseObject.V) ? long.Parse(syncResponseObject.V) : 0;
            this.Class = ctx.ReadClass(syncResponseObject);
            this.Roles = syncResponseObject.R?.Select(v => new WorkspaceRole(ctx, v)).Cast<IWorkspaceRole>().ToArray();
        }

        public IWorkspace Workspace { get; }

        public long Id { get; }

        public long Version { get; }

        public string UserSecurityHash { get; internal set; }

        public IClass Class { get; }

        public IWorkspaceRole[] Roles { get; }
    }
}

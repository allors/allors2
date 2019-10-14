// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using Allors.Protocol.Remote.Sync;
    using Allors.Workspace.Meta;
    using System.Linq;
    using Domain;
    using Protocol;
    using Protocol.Remote;

    public class WorkspaceObject : IWorkspaceObject
    {
        private AccessControl[] accessControls;

        private Permission[] deniedPermissions;

        internal WorkspaceObject(Workspace workspace, long objectId, IClass @class)
        {
            this.Workspace = workspace;
            this.Id = objectId;
            this.Class = @class;
            this.Version = 0;
        }

        internal WorkspaceObject(ResponseContext ctx, SyncResponseObject syncResponseObject)
        {
            this.Workspace = ctx.Workspace;
            this.Id = long.Parse(syncResponseObject.I);
            this.Class = (IClass)this.Workspace.ObjectFactory.MetaPopulation.Find(Guid.Parse(syncResponseObject.T));
            this.Version = !string.IsNullOrEmpty(syncResponseObject.V) ? long.Parse(syncResponseObject.V) : 0;
            this.Roles = syncResponseObject.R?.Select(v => new WorkspaceRole(this.Workspace.ObjectFactory.MetaPopulation, v)).Cast<IWorkspaceRole>().ToArray();
            this.SortedAccessControlIds = ctx.ReadSortedAccessControlIds(syncResponseObject.A);
            this.SortedDeniedPermissionIds = ctx.ReadSortedDeniedPermissionIds(syncResponseObject.D);
        }

        public IClass Class { get; }

        public long Id { get; }

        public IWorkspaceRole[] Roles { get; }

        public string SortedAccessControlIds { get; set; }

        public string SortedDeniedPermissionIds { get; set; }

        public long Version { get; private set; }

        IWorkspace IWorkspaceObject.Workspace => this.Workspace;

        public Workspace Workspace { get; }

        public bool IsPermitted(Permission permission)
        {
            if (permission == null)
            {
                return false;
            }

            if (this.accessControls == null && this.SortedAccessControlIds != null)
            {
                this.accessControls = this.SortedAccessControlIds.Split(Encoding.Separator).Select(v => this.Workspace.AccessControlById[long.Parse(v)]).ToArray();
                if (this.deniedPermissions != null)
                {
                    this.deniedPermissions = this.SortedDeniedPermissionIds.Split(Encoding.Separator).Select(v => this.Workspace.PermissionById[long.Parse(v)]).ToArray();
                }
            }

            if (this.deniedPermissions != null && this.deniedPermissions.Contains(permission))
            {
                return false;
            }

            if (this.accessControls != null && this.accessControls.Length > 0)
            {
                return this.accessControls.Any(v => v.PermissionIds.Any(w => w == permission.Id));
            }

            return false;
        }
    }
}

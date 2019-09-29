// <copyright file="Workspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Protocol.Remote.Pull;
    using Allors.Protocol.Remote.Sync;
    using Allors.Workspace.Meta;

    public class Workspace : IWorkspace
    {
        private readonly Dictionary<long, WorkspaceObject> workspaceObjectById = new Dictionary<long, WorkspaceObject>();

        public Workspace(ObjectFactory objectFactory) => this.ObjectFactory = objectFactory;

        IObjectFactory IWorkspace.ObjectFactory => this.ObjectFactory;

        public ObjectFactory ObjectFactory { get; }

        public SyncRequest Diff(PullResponse response)
        {
            var syncRequest = new SyncRequest
            {
                Objects = response.Objects.Where(v =>
                        {
                            var id = long.Parse(v[0]);
                            var version = long.Parse(v[1]);
                            this.workspaceObjectById.TryGetValue(id, out var workspaceObject);
                            return workspaceObject == null || !workspaceObject.Version.Equals(version);
                        }).Select(v => v[0]).ToArray(),
            };

            return syncRequest;
        }

        public void Sync(SyncResponse syncResponse)
        {
            var syncResponseContext = new SyncResponseContext(this, syncResponse.UserSecurityHash);
            foreach (var syncResponseObject in syncResponse.Objects)
            {
                var workspaceObject = new WorkspaceObject(syncResponseContext, syncResponseObject);
                this.workspaceObjectById[workspaceObject.Id] = workspaceObject;
            }
        }

        public IWorkspaceObject Get(long id)
        {
            var workspaceObject = this.workspaceObjectById[id];
            if (workspaceObject == null)
            {
                throw new Exception($"Object with id {id} is not present.");
            }

            return workspaceObject;
        }

        /// <summary>
        /// Invalidates the object in order to force a sync on next pull.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        internal void Invalidate(long objectId)
        {
            if (this.workspaceObjectById.TryGetValue(objectId, out var workspaceObject))
            {
                workspaceObject.UserSecurityHash = "#";
            }
        }

        internal IEnumerable<IWorkspaceObject> Get(IComposite objectType)
        {
            var classes = new HashSet<IClass>(objectType.Classes);
            return this.workspaceObjectById.Where(v => classes.Contains(v.Value.Class)).Select(v => v.Value);
        }
    }
}

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
    using Domain;
    using Protocol.Remote.Security;

    public class Workspace : IWorkspace
    {
        private readonly Dictionary<IClass, Dictionary<IOperandType, Permission>> executePermissionByOperandTypeByClass;
        private readonly Dictionary<IClass, Dictionary<IOperandType, Permission>> readPermissionByOperandTypeByClass;
        private readonly Dictionary<long, WorkspaceObject> workspaceObjectById;
        private readonly Dictionary<IClass, Dictionary<IOperandType, Permission>> writePermissionByOperandTypeByClass;

        public Workspace(ObjectFactory objectFactory)
        {
            this.ObjectFactory = objectFactory;
            this.AccessControlById = new Dictionary<long, AccessControl>();
            this.PermissionById = new Dictionary<long, Permission>();

            this.workspaceObjectById = new Dictionary<long, WorkspaceObject>();

            this.readPermissionByOperandTypeByClass = new Dictionary<IClass, Dictionary<IOperandType, Permission>>();
            this.writePermissionByOperandTypeByClass = new Dictionary<IClass, Dictionary<IOperandType, Permission>>();
            this.executePermissionByOperandTypeByClass = new Dictionary<IClass, Dictionary<IOperandType, Permission>>();
        }

        IObjectFactory IWorkspace.ObjectFactory => this.ObjectFactory;

        public ObjectFactory ObjectFactory { get; }

        internal Dictionary<long, AccessControl> AccessControlById { get; }

        internal Dictionary<long, Permission> PermissionById { get; }

        public SyncRequest Diff(PullResponse response)
        {
            var ctx = new PullResponseContext(this.AccessControlById, this.PermissionById);

            var syncRequest = new SyncRequest
            {
                Objects = response.Objects
                    .Where(v =>
                    {
                        var id = long.Parse(v[0]);
                        this.workspaceObjectById.TryGetValue(id, out var workspaceObject);
                        var sortedAccessControlIds = v.Length > 2 ? ctx.ReadSortedAccessControlIds(v[2]) : null;
                        var sortedDeniedPermissionIds = v.Length > 3 ? ctx.ReadSortedDeniedPermissionIds(v[3]) : null;

                        if (workspaceObject == null)
                        {
                            return true;
                        }

                        var version = long.Parse(v[1]);
                        if (!workspaceObject.Version.Equals(version))
                        {
                            return true;
                        }

                        if (v.Length == 2)
                        {
                            return false;
                        }

                        if (v.Length == 3)
                        {
                            if (workspaceObject.SortedDeniedPermissionIds != null)
                            {
                                return true;
                            }

                            return !Equals(workspaceObject.SortedAccessControlIds, sortedAccessControlIds);
                        }

                        return !Equals(workspaceObject.SortedAccessControlIds, sortedAccessControlIds) ||
                               !Equals(workspaceObject.SortedDeniedPermissionIds, sortedDeniedPermissionIds);
                    })
                    .Select(v => v[0]).ToArray(),
            };

            return syncRequest;
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

        public void Security(SecurityResponse securityResponse)
        {
            var ctx = new SecurityResponseContext(this);

            if (securityResponse.Permissions != null)
            {
                foreach (var syncResponsePermission in securityResponse.Permissions)
                {
                    var id = long.Parse(syncResponsePermission[0]);
                    var @class = (IClass)ctx.MetaObjectDecompressor.Read(syncResponsePermission[1]);
                    var operandType = (IOperandType)ctx.MetaObjectDecompressor.Read(syncResponsePermission[2]);
                    Enum.TryParse(syncResponsePermission[3], out Operations operation);

                    var permission = new Permission(id, @class, operandType, operation);
                    this.PermissionById[id] = permission;

                    switch (operation)
                    {
                        case Operations.Read:
                            if (!this.readPermissionByOperandTypeByClass.TryGetValue(@class,
                                out var readPermissionByOperandType))
                            {
                                readPermissionByOperandType = new Dictionary<IOperandType, Permission>();
                                this.readPermissionByOperandTypeByClass[@class] = readPermissionByOperandType;
                            }

                            readPermissionByOperandType[operandType] = permission;

                            break;

                        case Operations.Write:
                            if (!this.writePermissionByOperandTypeByClass.TryGetValue(@class,
                                out var writePermissionByOperandType))
                            {
                                writePermissionByOperandType = new Dictionary<IOperandType, Permission>();
                                this.writePermissionByOperandTypeByClass[@class] = writePermissionByOperandType;
                            }

                            writePermissionByOperandType[operandType] = permission;

                            break;

                        case Operations.Execute:
                            if (!this.executePermissionByOperandTypeByClass.TryGetValue(@class,
                                out var executePermissionByOperandType))
                            {
                                executePermissionByOperandType = new Dictionary<IOperandType, Permission>();
                                this.executePermissionByOperandTypeByClass[@class] = executePermissionByOperandType;
                            }

                            executePermissionByOperandType[operandType] = permission;

                            break;
                    }
                }
            }

            if (securityResponse.AccessControls != null)
            {
                foreach (var syncResponseAccessControl in securityResponse.AccessControls)
                {
                    var id = long.Parse(syncResponseAccessControl.I);
                    var version = long.Parse(syncResponseAccessControl.V);
                    var permissions = syncResponseAccessControl.P.Select(v => this.PermissionById[long.Parse(v)]);
                    var permissionSet = new HashSet<Permission>(permissions);

                    var accessControl = new AccessControl(id, version, permissionSet);
                    this.AccessControlById[id] = accessControl;
                }
            }
        }

        public SecurityRequest Sync(SyncResponse syncResponse)
        {
            var ctx = new SyncResponseContext(this, this.AccessControlById, this.PermissionById);
            foreach (var syncResponseObject in syncResponse.Objects)
            {
                var workspaceObject = new WorkspaceObject(ctx, syncResponseObject);
                this.workspaceObjectById[workspaceObject.Id] = workspaceObject;
            }

            if (ctx.MissingAccessControlIds.Count > 0 || ctx.MissingPermissionIds.Count > 0)
            {
                return new SecurityRequest
                {
                    AccessControls = ctx.MissingAccessControlIds.Select(v => v.ToString()).ToArray(),
                    Permissions = ctx.MissingPermissionIds.Select(v => v.ToString()).ToArray(),
                };
            }

            return null;
        }

        internal IEnumerable<IWorkspaceObject> Get(IComposite objectType)
        {
            var classes = new HashSet<IClass>(objectType.Classes);
            return this.workspaceObjectById.Where(v => classes.Contains(v.Value.Class)).Select(v => v.Value);
        }

        public Permission GetPermission(IClass @class, IOperandType roleType, Operations operation)
        {
            switch (operation)
            {
                case Operations.Read:
                    if (this.readPermissionByOperandTypeByClass.TryGetValue(@class, out var readPermissionByOperandType))
                    {
                        if (readPermissionByOperandType.TryGetValue(roleType, out var readPermission))
                        {
                            return readPermission;
                        }
                    }

                    return null;

                case Operations.Write:
                    if (this.writePermissionByOperandTypeByClass.TryGetValue(@class, out var writePermissionByOperandType))
                    {
                        if (writePermissionByOperandType.TryGetValue(roleType, out var writePermission))
                        {
                            return writePermission;
                        }
                    }

                    return null;

                default:
                    if (this.executePermissionByOperandTypeByClass.TryGetValue(@class, out var executePermissionByOperandType))
                    {
                        if (executePermissionByOperandType.TryGetValue(roleType, out var executePermission))
                        {
                            return executePermission;
                        }
                    }

                    return null;
            }
        }

        /// <summary>
        /// Invalidates the object in order to force a sync on next pull.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="class"></param>
        internal void Invalidate(long objectId, IClass @class) => this.workspaceObjectById[objectId] = new WorkspaceObject(this, objectId, @class);
    }
}

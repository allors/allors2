// <copyright file="SyncResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Remote.Sync;
    using Protocol;
    using Protocol.Data;

    public class SyncResponseBuilder
    {
        private readonly AccessControlsCompressor accessControlsCompressor;
        private readonly AccessControlLists acls;
        private readonly DeniedPermissionsCompressor deniedPermissionsCompressor;
        private readonly MetaObjectCompressor metaObjectCompressor;
        private readonly ISession session;
        private readonly SyncRequest syncRequest;
        private readonly User user;

        public SyncResponseBuilder(ISession session, User user, SyncRequest syncRequest)
        {
            this.session = session;
            this.user = user;
            this.syncRequest = syncRequest;
            this.acls = new AccessControlLists(this.user);

            var compressor = new Compressor();
            this.metaObjectCompressor = new MetaObjectCompressor(compressor);
            this.accessControlsCompressor = new AccessControlsCompressor(compressor, this.acls);
            this.deniedPermissionsCompressor = new DeniedPermissionsCompressor(compressor, this.acls);
        }

        public SyncResponse Build()
        {
            var objects = this.session.Instantiate(this.syncRequest.Objects);

            // Prefetch
            var objectByClass = objects.GroupBy(v => v.Strategy.Class, v => v);
            foreach (var groupBy in objectByClass)
            {
                var prefetchClass = (Class)groupBy.Key;
                var prefetchObjects = groupBy.ToArray();

                var prefetchPolicyBuilder = new PrefetchPolicyBuilder();
                prefetchPolicyBuilder.WithWorkspaceRules(prefetchClass);
                prefetchPolicyBuilder.WithSecurityRules(prefetchClass);
                var prefetcher = prefetchPolicyBuilder.Build();

                this.session.Prefetch(prefetcher, prefetchObjects);
            }

            SyncResponseRole CreateSyncResponseRole(IObject @object, IRoleType roleType)
            {
                var syncResponseRole = new SyncResponseRole { T = this.metaObjectCompressor.Write(roleType) };

                if (roleType.ObjectType.IsUnit)
                {
                    syncResponseRole.V = UnitConvert.ToString(@object.Strategy.GetUnitRole(roleType.RelationType));
                }
                else if (roleType.IsOne)
                {
                    syncResponseRole.V = @object.Strategy.GetCompositeRole(roleType.RelationType)?.Id.ToString();
                }
                else
                {
                    syncResponseRole.V = string.Join(
                        separator: ',',
                        values: @object.Strategy.GetCompositeRoles(roleType.RelationType)
                            .Cast<IObject>()
                            .Select(roleObject => roleObject.Id.ToString()));
                }

                return syncResponseRole;
            }

            var syncResponse = new SyncResponse
            {
                Objects = objects.Select(v =>
                {
                    var @class = (Class)v.Strategy.Class;
                    var acl = this.acls[v];

                    return new SyncResponseObject
                    {
                        I = v.Id.ToString(),
                        V = v.Strategy.ObjectVersion.ToString(),
                        T = v.Strategy.Class.Name,
                        R = @class.WorkspaceRoleTypes
                            .Where(w => acl.CanRead(w))
                            .Select(w => CreateSyncResponseRole(v, w))
                            .ToArray(),
                        A = this.accessControlsCompressor.Write(v),
                        D = this.deniedPermissionsCompressor.Write(v),
                    };
                }).ToArray(),
            };

            HashSet<Permission> permissions = null;
            if (this.syncRequest.Permissions != null)
            {
                var permissionIds = this.syncRequest.Permissions;
                permissions = new HashSet<Permission>(this.session.Instantiate(permissionIds).Cast<Permission>());
            }

            AccessControl[] accessControls = null;
            if (this.syncRequest.AccessControls != null)
            {
                if (permissions == null)
                {
                    permissions = new HashSet<Permission>();
                }

                var accessControlIds = this.syncRequest.AccessControls;
                accessControls = this.session.Instantiate(accessControlIds).Cast<AccessControl>().ToArray();
                syncResponse.AccessControls = accessControls
                    .Select(v =>
                    {
                        var effectiveWorkspacePermissions = v.EffectivePermissions.Where(w => w.OperandType.Workspace).ToArray();
                        permissions.UnionWith(effectiveWorkspacePermissions);
                        return new SyncResponseAccessControl
                        {
                            I = v.Strategy.ObjectId.ToString(),
                            V = v.Strategy.ObjectVersion.ToString(),
                            P = effectiveWorkspacePermissions.Select(w => w.Id.ToString()).ToArray(),
                        };
                    }).ToArray();
            }

            if (permissions != null)
            {
                syncResponse.Permissions = permissions.Select(v =>
                    new[]
                    {
                        v.Strategy.ObjectId.ToString(),
                        this.metaObjectCompressor.Write(v.ConcreteClass),
                        this.metaObjectCompressor.Write(v.OperandType),
                        v.OperationEnum.ToString(),
                    }).ToArray();
            }

            return syncResponse;
        }
    }
}

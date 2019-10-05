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
        private readonly IAccessControlLists acls;
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
            this.acls = new WorkspaceAccessControlLists(this.user);

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
                    var roles = @object.Strategy.GetCompositeRoles(roleType.RelationType);
                    if (roles.Count > 0)
                    {
                        syncResponseRole.V = string.Join(
                            separator: Compressor.ItemSeparator,
                            values: roles
                                .Cast<IObject>()
                                .Select(roleObject => roleObject.Id.ToString()));
                    }
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
                        T = this.metaObjectCompressor.Write(v.Strategy.Class),
                        R = @class.WorkspaceRoleTypes
                            .Where(w => acl.CanRead(w) && v.Strategy.ExistRole(w.RelationType))
                            .Select(w => CreateSyncResponseRole(v, w))
                            .ToArray(),
                        A = this.accessControlsCompressor.Write(v),
                        D = this.deniedPermissionsCompressor.Write(v),
                    };
                }).ToArray(),
            };

            syncResponse.AccessControls = this.acls.EffectivePermissionIdsByAccessControl.Keys
                .Select(v => new[]
                {
                    v.Strategy.ObjectId.ToString(),
                    v.Strategy.ObjectVersion.ToString(),
                })
                .ToArray();

            return syncResponse;
        }
    }
}

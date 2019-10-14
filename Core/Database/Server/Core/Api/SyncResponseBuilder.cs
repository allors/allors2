// <copyright file="SyncResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Remote.Sync;
    using Protocol;
    using Protocol.Data;
    using Protocol.Remote;

    public class SyncResponseBuilder
    {
        private readonly AccessControlsWriter accessControlsWriter;
        private readonly IAccessControlLists acls;
        private readonly PermissionsWriter permissionsWriter;

        private readonly ISession session;
        private readonly SyncRequest syncRequest;

        public SyncResponseBuilder(ISession session, SyncRequest syncRequest, IAccessControlLists acls)
        {
            this.session = session;
            this.syncRequest = syncRequest;
            this.acls = acls;

            this.accessControlsWriter = new AccessControlsWriter(this.acls);
            this.permissionsWriter = new PermissionsWriter(this.acls);
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
                var syncResponseRole = new SyncResponseRole { T = roleType.IdAsString };

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
                            separator: Encoding.Separator,
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
                        T = v.Strategy.Class.IdAsString,
                        R = @class.WorkspaceRoleTypes
                            .Where(w => acl.CanRead(w) && v.Strategy.ExistRole(w.RelationType))
                            .Select(w => CreateSyncResponseRole(v, w))
                            .ToArray(),
                        A = this.accessControlsWriter.Write(v),
                        D = this.permissionsWriter.Write(v),
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

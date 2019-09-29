// <copyright file="SyncResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Remote.Sync;
    using Protocol.Data;

    public class SyncResponseBuilder
    {
        private static readonly object[][] EmptyRoles = { };

        private readonly ISession session;
        private readonly SyncRequest syncRequest;
        private readonly User user;
        private readonly AccessControlListFactory aclFactory;

        private readonly MetaObjectCompression metaObjectCompression;
        private readonly AccessControlsCompression accessControlsCompression;
        private readonly DeniedPermissionsCompression deniedPermissionsCompression;

        public SyncResponseBuilder(ISession session, User user, SyncRequest syncRequest)
        {
            this.session = session;
            this.user = user;
            this.syncRequest = syncRequest;
            this.aclFactory = new AccessControlListFactory(this.user);

            this.metaObjectCompression = new MetaObjectCompression();
            this.accessControlsCompression = new AccessControlsCompression();
            this.deniedPermissionsCompression = new DeniedPermissionsCompression();
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
                var syncResponseRole = new SyncResponseRole { T = this.metaObjectCompression.Write(roleType) };

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
                        ',',
                        @object.Strategy.GetCompositeRoles(roleType.RelationType)
                            .Cast<IObject>()
                            .Select(roleObject => roleObject.Id.ToString()));
                }

                return syncResponseRole;
            }

            return new SyncResponse
            {
                UserSecurityHash = this.user.SecurityHash(),
                Objects = objects.Select(v =>
                {
                    var @class = (Class)v.Strategy.Class;
                    var acl = this.aclFactory.Create(v);

                    return new SyncResponseObject
                    {
                        I = v.Id.ToString(),
                        V = v.Strategy.ObjectVersion.ToString(),
                        T = v.Strategy.Class.Name,
                        R = @class.WorkspaceRoleTypes
                            .Where(w => acl.CanRead(w))
                            .Select(w => CreateSyncResponseRole(v, w))
                            .ToArray(),
                        A = this.accessControlsCompression.Write(acl),
                        D = this.deniedPermissionsCompression.Write(acl),
                    };
                }).ToArray(),
            };
        }
    }
}

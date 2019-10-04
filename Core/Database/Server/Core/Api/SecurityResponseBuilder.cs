// <copyright file="SecurityResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Domain;
    using Allors.Protocol.Remote.Security;
    using Meta;
    using Protocol;

    public class SecurityResponseBuilder
    {
        private readonly AccessControlsCompressor accessControlsCompressor;
        private readonly AccessControlLists acls;
        private readonly DeniedPermissionsCompressor deniedPermissionsCompressor;
        private readonly MetaObjectCompressor metaObjectCompressor;
        private readonly SecurityRequest securityRequest;
        private readonly ISession session;
        private readonly User user;

        public SecurityResponseBuilder(ISession session, User user, SecurityRequest securityRequest)
        {
            this.session = session;
            this.user = user;
            this.securityRequest = securityRequest;
            this.acls = new AccessControlLists(this.user);

            var compressor = new Compressor();
            this.metaObjectCompressor = new MetaObjectCompressor(compressor);
            this.accessControlsCompressor = new AccessControlsCompressor(compressor, this.acls);
            this.deniedPermissionsCompressor = new DeniedPermissionsCompressor(compressor, this.acls);
        }

        public SecurityResponse Build()
        {
            var securityResponse = new SecurityResponse();

            // TODO: Prefetch

            var permissionIds = this.securityRequest.Permissions;
            var permissions = new HashSet<Permission>(this.session.Instantiate(permissionIds).Cast<Permission>());

            if (this.securityRequest.AccessControls != null)
            {
                var accessControlIds = this.securityRequest.AccessControls;
                var accessControls = this.session.Instantiate(accessControlIds).Cast<AccessControl>().ToArray();
                securityResponse.AccessControls = accessControls
                    .Select(v =>
                    {
                        var effectiveWorkspacePermissions = v.EffectivePermissions.Where(w => w.OperandType.Workspace).ToArray();
                        permissions.UnionWith(effectiveWorkspacePermissions);
                        return new SecurityResponseAccessControl
                        {
                            I = v.Strategy.ObjectId.ToString(),
                            V = v.Strategy.ObjectVersion.ToString(),
                            P = effectiveWorkspacePermissions.Select(w => w.Id.ToString()).ToArray(),
                        };
                    }).ToArray();
            }

            if (permissions.Count > 0)
            {
                securityResponse.Permissions = permissions.Select(v =>
                    new[]
                    {
                        v.Strategy.ObjectId.ToString(),
                        this.metaObjectCompressor.Write(v.ConcreteClass),
                        this.metaObjectCompressor.Write(v.OperandType),
                        v.OperationEnum.ToString(),
                    }).ToArray();
            }

            return securityResponse;
        }
    }
}

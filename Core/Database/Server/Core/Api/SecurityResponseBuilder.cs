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
        private static readonly PrefetchPolicy AccessControlPrefetchPolicy;

        private readonly AccessControlsCompressor accessControlsCompressor;
        private readonly AccessControlLists acls;
        private readonly DeniedPermissionsCompressor deniedPermissionsCompressor;
        private readonly Compressor compressor;
        private readonly SecurityRequest securityRequest;
        private readonly ISession session;
        private readonly User user;

        static SecurityResponseBuilder() =>
            AccessControlPrefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(M.AccessControl.EffectivePermissions)
                .Build();

        public SecurityResponseBuilder(ISession session, User user, SecurityRequest securityRequest)
        {
            this.session = session;
            this.user = user;
            this.securityRequest = securityRequest;
            this.acls = new AccessControlLists(this.user);

            this.compressor = new Compressor();
            this.accessControlsCompressor = new AccessControlsCompressor(compressor, this.acls);
            this.deniedPermissionsCompressor = new DeniedPermissionsCompressor(compressor, this.acls);
        }

        public SecurityResponse Build()
        {
            var securityResponse = new SecurityResponse();

            if (this.securityRequest.AccessControls != null)
            {
                var accessControlIds = this.securityRequest.AccessControls;
                var accessControls = this.session.Instantiate(accessControlIds).Cast<AccessControl>().ToArray();

                this.session.Prefetch(AccessControlPrefetchPolicy, accessControls);

                securityResponse.AccessControls = accessControls
                    .Select(v => new SecurityResponseAccessControl
                    {
                        I = v.Strategy.ObjectId.ToString(),
                        V = v.Strategy.ObjectVersion.ToString(),
                        P = v.EffectivePermissions.Where(w => w.OperandType.Workspace).Select(w => w.Id.ToString()).ToArray(),
                    }).ToArray();
            }

            if (this.securityRequest.Permissions.Length > 0)
            {
                var permissionIds = this.securityRequest.Permissions;
                var permissions = this.session.Instantiate(permissionIds).Cast<Permission>();

                securityResponse.Permissions = permissions.Select(v =>
                    new[]
                    {
                        v.Strategy.ObjectId.ToString(),
                        this.compressor.Write(v.ConcreteClassPointer.ToString("D")),
                        this.compressor.Write(v.OperandTypePointer.ToString("D")),
                        v.OperationEnum.ToString(),
                    }).ToArray();
            }

            return securityResponse;
        }
    }
}

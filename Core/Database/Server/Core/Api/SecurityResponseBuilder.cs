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
        private static readonly PrefetchPolicy PermissionPrefetchPolicy;

        private readonly ISession session;
        private readonly SecurityRequest securityRequest;
        private readonly IAccessControlLists acls;

        static SecurityResponseBuilder()
        {
            AccessControlPrefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(M.AccessControl.EffectiveWorkspacePermissions)
                .Build();

            PermissionPrefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(M.Permission.ConcreteClassPointer)
                .WithRule(M.Permission.OperandTypePointer)
                .WithRule(M.Permission.OperationEnum)
                .Build();
        }

        public SecurityResponseBuilder(ISession session, SecurityRequest securityRequest, IAccessControlLists acls)
        {
            this.session = session;
            this.securityRequest = securityRequest;
            this.acls = acls;
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
                        P = v.EffectiveWorkspacePermissions.Select(w => w.Id.ToString()).ToArray(),
                    }).ToArray();
            }

            if (this.securityRequest.Permissions.Length > 0)
            {
                var permissionIds = this.securityRequest.Permissions;
                var permissions = this.session.Instantiate(permissionIds)
                    .Cast<Permission>()
                    .Where(v => v.OperandType.Workspace);

                securityResponse.Permissions = permissions.Select(v =>
                    new[]
                    {
                        v.Strategy.ObjectId.ToString(),
                        v.ConcreteClassPointer.ToString("D"),
                        v.OperandTypePointer.ToString("D"),
                        v.OperationEnum.ToString(),
                    }).ToArray();
            }

            return securityResponse;
        }
    }
}

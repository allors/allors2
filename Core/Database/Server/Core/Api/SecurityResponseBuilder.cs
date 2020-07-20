// <copyright file="SecurityResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Linq;
    using Allors.Domain;
    using Allors.Protocol.Remote.Security;

    public class SecurityResponseBuilder
    {
        private readonly ISession session;
        private readonly SecurityRequest securityRequest;

        public SecurityResponseBuilder(ISession session, SecurityRequest securityRequest)
        {
            this.session = session;
            this.securityRequest = securityRequest;
        }

        public SecurityResponse Build()
        {
            var securityResponse = new SecurityResponse();

            if (this.securityRequest.AccessControls != null)
            {
                var accessControlIds = this.securityRequest.AccessControls;
                var accessControls = this.session.Instantiate(accessControlIds).Cast<AccessControl>().ToArray();

                securityResponse.AccessControls = accessControls
                    .Select(v => new SecurityResponseAccessControl
                    {
                        I = v.Strategy.ObjectId.ToString(),
                        V = v.Strategy.ObjectVersion.ToString(),
                        P = v.EffectiveWorkspacePermissionIds,
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

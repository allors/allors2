// <copyright file="SecurityResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System.Linq;
    using Allors.Protocol.Json.Api.Security;
    using Security;

    public class AccessResponseBuilder
    {
        private readonly ITransaction transaction;

        public AccessResponseBuilder(ITransaction transaction, IAccessControl accessControl)
        {
            this.transaction = transaction;
            this.AccessControl = accessControl;
        }

        public IAccessControl AccessControl { get; }

        public AccessResponse Build(AccessRequest accessRequest)
        {
            var accessResponse = new AccessResponse();

            if (accessRequest.g?.Length > 0)
            {
                var ids = accessRequest.g;
                var grants = this.transaction.Instantiate(ids).Cast<IGrant>().ToArray();

                accessResponse.g = grants
                    .Select(v =>
                    {
                        var response = new AccessResponseGrant
                        {
                            i = v.Strategy.ObjectId,
                            v = v.Strategy.ObjectVersion,
                        };

                        response.p = this.AccessControl.GrantedPermissionIds(v).Save();

                        return response;
                    }).ToArray();
            }

            if (accessRequest.r?.Length > 0)
            {
                var revocationIds = accessRequest.r;
                var revocations = this.transaction.Instantiate(revocationIds).Cast<IRevocation>().ToArray();

                accessResponse.r = revocations
                    .Select(v =>
                    {
                        var response = new AccessResponseRevocation
                        {
                            i = v.Strategy.ObjectId,
                            v = v.Strategy.ObjectVersion,
                        };

                        response.p = this.AccessControl.RevokedPermissionIds(v).Save();

                        return response;
                    }).ToArray();
            }

            return accessResponse;
        }
    }
}

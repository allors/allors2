// <copyright file="SecurityResponseBuilder.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api.Security;
    using Meta;
    using Security;

    public class PermissionResponseBuilder
    {
        private readonly ITransaction transaction;
        private readonly ISet<IClass> allowedClasses;

        public PermissionResponseBuilder(ITransaction transaction, ISet<IClass> allowedClasses)
        {
            this.transaction = transaction;
            this.allowedClasses = allowedClasses;
        }

        public PermissionResponse Build(PermissionRequest permissionRequest)
        {
            var securityResponse = new PermissionResponse();

            if (permissionRequest.p?.Length > 0)
            {
                var permissionIds = permissionRequest.p;
                var permissions = this.transaction.Instantiate(permissionIds)
                    .Cast<IPermission>()
                    .Where(v => this.allowedClasses?.Contains(v.Class) == true);

                securityResponse.p = permissions.Select(v => v switch
                {
                    IReadPermission permission => new PermissionResponsePermission
                    {
                        i = permission.Strategy.ObjectId,
                        c = permission.Class.Tag,
                        t = permission.RelationType.Tag,
                        o = (long)Operations.Read
                    },
                    IWritePermission permission => new PermissionResponsePermission
                    {
                        i = permission.Strategy.ObjectId,
                        c = permission.Class.Tag,
                        t = permission.RelationType.Tag,
                        o = (long)Operations.Write
                    },
                    IExecutePermission permission => new PermissionResponsePermission
                    {
                        i = permission.Strategy.ObjectId,
                        c = permission.Class.Tag,
                        t = permission.MethodType.Tag,
                        o = (long)Operations.Execute
                    },
                    _ => throw new Exception(),
                }).ToArray();
            }

            return securityResponse;
        }
    }
}

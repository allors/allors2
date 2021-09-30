// <copyright file="AccessControlListFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Security
{
    using Ranges;

    public interface IAccessControl
    {
        IRange<long> GrantedPermissionIds(IGrant grant);

        IRange<long> RevokedPermissionIds(IRevocation revocation);

        IAccessControlList this[IObject @object]
        {
            get;
        }
    }
}

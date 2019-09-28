// <copyright file="AccessControlListFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;

    public class AccessControlListFactory : IAccessControlListFactory
    {
        public AccessControlListFactory(User user)
        {
            this.User = user;
            this.EffectivePermissionIdsByAccessControl = user.EffectivePermissionsByAccessControl();
        }

        public User User { get; }

        public IReadOnlyDictionary<AccessControl, HashSet<long>> EffectivePermissionIdsByAccessControl { get; set; }

        public IAccessControlList Create(IObject @object) => new AccessControlList(this, @object);
    }
}

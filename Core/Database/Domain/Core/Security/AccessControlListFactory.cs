// <copyright file="AccessControlListFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Concurrent;
    using Allors;

    /// <summary>
    /// A factory for AccessControlLists.
    /// </summary>
    public class AccessControlListFactory : IAccessControlListFactory
    {
        private readonly User user;
        private readonly ConcurrentDictionary<IObject, IAccessControlList> accessControlListByObject;

        private readonly Func<IObject, User, IAccessControlList> factory = (allorsObject, user) => new AccessControlList(allorsObject, user);

        public AccessControlListFactory(User user)
        {
            this.user = user;
            this.accessControlListByObject = new ConcurrentDictionary<IObject, IAccessControlList>();
        }

        public AccessControlListFactory(User user, Func<IObject, User, IAccessControlList> factory)
            : this(user) =>
            this.factory = factory;

        public IAccessControlList Create(IObject allorsObject)
        {
            if (!this.accessControlListByObject.TryGetValue(allorsObject, out var acl))
            {
                acl = this.factory(allorsObject, this.user);
                this.accessControlListByObject[allorsObject] = acl;
            }

            return acl;
        }
    }
}

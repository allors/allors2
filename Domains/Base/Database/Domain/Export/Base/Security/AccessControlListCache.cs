// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControlListCache.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Collections.Concurrent;
    using Allors;

    /// <summary>
    /// A factory for AccessControlLists.
    /// </summary>
    public class AccessControlListCache : IAccessControlListFactory
    {
        private readonly User user;
        private readonly Func<IObject, User, IAccessControlList> factory = (allorsObject, user) => new AccessControlList(allorsObject, user);
        private readonly ConcurrentDictionary<IObject, IAccessControlList> accessControlListByObject;

        public AccessControlListCache(User user)
        {
            this.user = user;
            this.accessControlListByObject = new ConcurrentDictionary<IObject, IAccessControlList>();
        }

        public AccessControlListCache(User user, Func<IObject, User, IAccessControlList> factory)
            : this(user)
        {
            this.factory = factory;
        }

        public IAccessControlList Create(IObject allorsObject)
        {
            IAccessControlList acl;
            if (!this.accessControlListByObject.TryGetValue(allorsObject, out acl))
            {
                acl = this.factory(allorsObject, this.user);
                this.accessControlListByObject[allorsObject] = acl;
            }

            return acl;
        }
    }
}
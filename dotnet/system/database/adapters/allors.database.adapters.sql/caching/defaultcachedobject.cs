// <copyright file="DefaultCachedObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Caching
{
    using System.Collections.Concurrent;

    using Meta;

    public sealed class DefaultCachedObject : ICachedObject
    {
        private readonly ConcurrentDictionary<IRoleType, object> roleByRoleType;

        internal DefaultCachedObject(long version)
        {
            this.Version = version;
            this.roleByRoleType = new ConcurrentDictionary<IRoleType, object>();
        }

        public long Version { get; }

        public bool Contains(IRoleType roleType) => this.roleByRoleType.ContainsKey(roleType);

        public bool TryGetValue(IRoleType roleType, out object value) => this.roleByRoleType.TryGetValue(roleType, out value);

        public void SetValue(IRoleType roleType, object value) => this.roleByRoleType[roleType] = value;
    }
}

// <copyright file="DefaultCachedObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient.Caching
{
    using System.Collections.Concurrent;

    using Allors.Meta;

    public sealed class DefaultCachedObject : ICachedObject
    {
        private readonly ConcurrentDictionary<IRoleType, object> roleByRoleType;

        internal DefaultCachedObject(long version)
        {
            this.Version = version;
            this.roleByRoleType = new ConcurrentDictionary<IRoleType, object>();
        }

        public long Version { get; }

        public bool TryGetValue(IRoleType roleType, out object value) => this.roleByRoleType.TryGetValue(roleType, out value);

        public void SetValue(IRoleType roleType, object value) => this.roleByRoleType[roleType] = value;
    }
}

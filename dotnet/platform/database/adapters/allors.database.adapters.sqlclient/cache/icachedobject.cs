// <copyright file="ICachedObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient.Caching
{
    using Allors.Meta;

    public interface ICachedObject
    {
        long Version { get; }

        bool TryGetValue(IRoleType roleType, out object value);

        void SetValue(IRoleType roleType, object value);
    }
}

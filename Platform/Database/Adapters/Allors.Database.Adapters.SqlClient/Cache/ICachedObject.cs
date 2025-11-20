// <copyright file="ICachedObject.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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

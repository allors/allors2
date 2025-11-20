// <copyright file="ICacheFactory.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient.Caching
{
    /// <summary>
    /// The cache factory creates a new Cache.
    /// </summary>
    public interface ICacheFactory
    {
        ICache CreateCache();
    }
}

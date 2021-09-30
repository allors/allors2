// <copyright file="ICacheFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Caching
{
    /// <summary>
    /// The cache factory creates a new Cache.
    /// </summary>
    public interface ICacheFactory
    {
        ICache CreateCache();
    }
}

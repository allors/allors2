// <copyright file="DefaultCacheFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Caching
{
    using Meta;

    /// <summary>
    /// Factory for default cache.
    /// </summary>
    public sealed class DefaultCacheFactory : ICacheFactory
    {
        public IClass[] ExcludedClasses { get; set; }

        public ICache CreateCache() => new DefaultCache(this.ExcludedClasses);
    }
}

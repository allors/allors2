// <copyright file="SessionExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Allors.Services;

    using Antlr.Runtime.Misc;

    using Microsoft.Extensions.DependencyInjection;

    public static partial class SessionExtension
    {
        public static ConcurrentDictionary<long, T> GetCache<T>(this ISession @this)
        {
            var caches = @this.ServiceProvider.GetRequiredService<ICacheService>();
            var cache = caches.Get<T, ConcurrentDictionary<long, T>>();
            if (cache == null)
            {
                cache = new ConcurrentDictionary<long, T>();
                caches.Set<T, ConcurrentDictionary<long, T>>(cache);
            }

            return cache;
        }

        public static TValue GetCache<TKey, TValue>(this ISession @this, Func<TValue> factory)
        {
            var caches = @this.ServiceProvider.GetRequiredService<ICacheService>();
            var cache = caches.Get<TKey, TValue>();
            if (cache == null)
            {
                cache = factory();
                caches.Set<TKey, TValue>(cache);
            }

            return cache;
        }

        public static void ClearCache<T>(this ISession @this)
        {
            var caches = @this.ServiceProvider.GetRequiredService<ICacheService>();
            caches.Clear<T>();
        }
    }
}

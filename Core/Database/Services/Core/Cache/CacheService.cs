// <copyright file="CacheService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using System;
    using System.Collections.Concurrent;

    public class CacheService : ICacheService
    {
        private ConcurrentDictionary<Type, object> caches;

        public CacheService(IStateService stateService)
        {
            this.Clear();
            stateService.Register(this);
        }

        public void Clear() => this.caches = new ConcurrentDictionary<Type, object>();

        public TValue Get<TKey, TValue>()
        {
            if (this.caches.TryGetValue(typeof(TKey), out var cache))
            {
                return (TValue)cache;
            }

            return default(TValue);
        }

        public void Set<TKey, TValue>(TValue value) => this.caches[typeof(TKey)] = value;

        public void Clear<TKey>() => this.caches.TryRemove(typeof(TKey), out var value);
    }
}

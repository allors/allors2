// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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

            return default;
        }

        public void Set<TKey, TValue>(TValue value) => this.caches[typeof(TKey)] = value;

        public void Clear<TKey>() => this.caches.TryRemove(typeof(TKey), out var value);
    }
}

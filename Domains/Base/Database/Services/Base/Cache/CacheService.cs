// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheService.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
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

        public TValue Get<TKey, TValue>()
        {
            if (this.caches.TryGetValue(typeof(TKey), out var cache))
            {
                return (TValue)cache;
            }

            return default(TValue);
        }

        public void Set<TKey, TValue>(TValue value)
        {
            this.caches[typeof(TKey)] = value;
        }

        public void Clear()
        {
            this.caches = new ConcurrentDictionary<Type, object>();
        }
    }
}
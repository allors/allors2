// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionExtension.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System.Collections.Generic;

    using Allors.Services;

    using Antlr.Runtime.Misc;

    using Microsoft.Extensions.DependencyInjection;

    public static partial class SessionExtension
    {
        public static Dictionary<long, T> GetCache<T>(this ISession @this)
        {
            var caches = @this.ServiceProvider.GetRequiredService<ICacheService>();
            var cache = caches.Get<T, Dictionary<long, T>>();
            if (cache == null)
            {
                cache = new Dictionary<long, T>();
                caches.Set<T, Dictionary<long, T>>(cache);
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

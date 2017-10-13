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
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public static partial class SessionExtension
    {
        public static IDictionary<T, long> GetSticky<T>(this ISession @this, Type type, RoleType roleType)
        {
            var key = $"{type}.{roleType}";

            var stickies = @this.ServiceProvider.GetRequiredService<IStickyService>();
            var cache = stickies.Get<T>(key);
            if (cache == null)
            {
                cache = new ConcurrentDictionary<T, long>();
                stickies.Set<T>(key, cache);
            }

            return cache;
        }
    }
}
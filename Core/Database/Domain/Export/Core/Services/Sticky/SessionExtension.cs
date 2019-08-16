// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
                stickies.Set(key, cache);
            }

            return cache;
        }
    }
}

// <copyright file="SessionExtension.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public static partial class SessionExtension
    {
        public static Singleton GetSingleton(this ISession @this)
        {
            var singletonService = @this.ServiceProvider.GetRequiredService<ISingletonService>();
            var instance = @this.Instantiate(singletonService.Id) as Singleton ?? @this.Extent<Singleton>().First;
            singletonService.Id = instance?.Id ?? 0;
            return instance;
        }
    }
}

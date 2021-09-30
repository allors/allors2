// <copyright file="SessionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using Bogus;
    using Microsoft.Extensions.DependencyInjection;

    public static class SessionExtensions
    {
        public static Faker Faker(this ISession @this) => @this.ServiceProvider.GetRequiredService<Faker>();
    }
}

// <copyright file="IdentityBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Security
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddAllorsStores(this IdentityBuilder builder)
            => builder
                .AddAllorsUserStore()
                .AddAllorsRoleStore();

        private static IdentityBuilder AddAllorsUserStore(this IdentityBuilder builder)
        {
            var userStoreType = typeof(AllorsUserStore);
            builder.Services.AddScoped(typeof(IUserStore<>).MakeGenericType(builder.UserType), userStoreType);
            return builder;
        }

        private static IdentityBuilder AddAllorsRoleStore(this IdentityBuilder builder)
        {
            if (builder.RoleType != null)
            {
                var roleStoreType = typeof(AllorsRoleStore);
                builder.Services.AddScoped(typeof(IRoleStore<>).MakeGenericType(builder.RoleType), roleStoreType);
            }

            return builder;
        }
    }
}

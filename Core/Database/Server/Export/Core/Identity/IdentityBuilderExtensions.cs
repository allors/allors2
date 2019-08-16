// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityBuilderExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Identity
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder UseAllors(this IdentityBuilder builder)
            => builder
                .AddAllorsUserStore()
                .AddAllorsRoleStore();

        private static IdentityBuilder AddAllorsUserStore(this IdentityBuilder builder)
        {
            var userStoreType = typeof(AllorsUserStore<>).MakeGenericType(builder.UserType);
            builder.Services.AddScoped(typeof(IUserStore<>).MakeGenericType(builder.UserType), userStoreType);
            return builder;
        }

        private static IdentityBuilder AddAllorsRoleStore(this IdentityBuilder builder)
        {
            var roleStoreType = typeof(AllorsRoleStore<>).MakeGenericType(builder.RoleType);
            builder.Services.AddScoped(typeof(IRoleStore<>).MakeGenericType(builder.RoleType), roleStoreType);
            return builder;
        }
    }
}

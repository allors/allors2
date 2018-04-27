// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityBuilderExtensions.cs" company="Allors bvba">
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

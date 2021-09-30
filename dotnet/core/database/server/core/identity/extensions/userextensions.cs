// <copyright file="AllorsUserStore.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Security
{
    using Allors.Domain;
    using Microsoft.AspNetCore.Identity;

    public static class UserExtensions
    {
        public static IdentityUser AsIdentityUser(this User @this) =>
            new IdentityUser
            {
                Id = @this.Id.ToString(),
                UserName = @this.UserName,
                NormalizedUserName = @this.NormalizedUserName,
                PasswordHash = @this.UserPasswordHash,
                Email = @this.UserEmail,
                NormalizedEmail = @this.NormalizedUserEmail,
                EmailConfirmed = @this.UserEmailConfirmed,
                SecurityStamp = @this.UserSecurityStamp,
                PhoneNumber = @this.UserPhoneNumber,
                PhoneNumberConfirmed = @this.UserPhoneNumberConfirmed,
                TwoFactorEnabled = @this.UserTwoFactorEnabled,
                LockoutEnd = @this.UserLockoutEnd,
                LockoutEnabled = @this.UserLockoutEnabled,
                AccessFailedCount = @this.UserAccessFailedCount,
            };
    }
}

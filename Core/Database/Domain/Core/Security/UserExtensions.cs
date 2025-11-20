// <copyright file="UserExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static partial class UserExtensions
    {
        public static bool IsAdministrator(this User @this)
        {
            var administrators = new UserGroups(@this.Session()).Administrators;
            return administrators.Members.Contains(@this);
        }

        public static T SetPassword<T>(this T @this, string clearTextPassword)
            where T : User
        {
            var securityService = @this.Session().ServiceProvider.GetRequiredService<IPasswordService>();
            var passwordHash = securityService.HashPassword(@this.UserName, clearTextPassword);
            @this.UserPasswordHash = passwordHash;
            return @this;
        }

        public static bool VerifyPassword(this User @this, string clearTextPassword)
        {
            if (string.IsNullOrWhiteSpace(clearTextPassword))
            {
                return false;
            }

            var securityService = @this.Session().ServiceProvider.GetRequiredService<IPasswordService>();
            return securityService.VerifyHashedPassword(@this.UserName, @this.UserPasswordHash, clearTextPassword);
        }

        public static void CoreOnPostBuild(this User @this, ObjectOnPostBuild method)
        {
            if (!@this.ExistNotificationList)
            {
                @this.NotificationList = new NotificationListBuilder(@this.Strategy.Session).Build();
            }

            if (!@this.ExistOwnerAccessControl)
            {
                var ownerRole = new Roles(@this.Strategy.Session).Owner;
                @this.DerivedRoles.OwnerAccessControl = new AccessControlBuilder(@this.Strategy.Session)
                    .WithRole(ownerRole)
                    .WithSubject(@this)
                    .Build();
            }

            if (!@this.ExistOwnerSecurityToken)
            {
                @this.DerivedRoles.OwnerSecurityToken = new SecurityTokenBuilder(@this.Strategy.Session)
                    .WithAccessControl(@this.OwnerAccessControl)
                    .Build();
            }

            if (!@this.ExistUserSecurityStamp)
            {
                @this.UserSecurityStamp = Guid.NewGuid().ToString();
            }
        }

        public static void CoreOnDerive(this User @this, ObjectOnDerive method)
        {
            @this.DerivedRoles.NormalizedUserName = Users.Normalize(@this.UserName);
            @this.DerivedRoles.NormalizedUserEmail = Users.Normalize(@this.UserEmail);

            if (@this.ExistInUserPassword)
            {
                var passwordService = @this.Session().ServiceProvider.GetRequiredService<IPasswordService>();
                @this.UserPasswordHash = passwordService.HashPassword(@this.UserName, @this.InUserPassword);
                @this.RemoveInUserPassword();
            }
        }

        public static void CoreDelete(this User @this, DeletableDelete method)
        {
            @this.OwnerAccessControl?.Delete();
            @this.OwnerSecurityToken?.Delete();
            foreach (Login login in @this.Logins)
            {
                login.Delete();
            }

            @this.NotificationList?.Delete();
        }
    }
}

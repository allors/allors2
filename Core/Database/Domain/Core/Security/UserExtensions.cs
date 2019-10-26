// <copyright file="UserExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static partial class UserExtensions
    {
        public static bool IsAdministrator(this User @this)
        {
            var administrators = new UserGroups(@this.Session()).Administrators;
            return administrators.Members.Contains(@this);
        }

        public static User SetPassword(this User @this, string clearTextPassword)
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
                @this.OwnerAccessControl = new AccessControlBuilder(@this.Strategy.Session)
                    .WithRole(ownerRole)
                    .WithSubject(@this)
                    .Build();
            }

            if (!@this.ExistOwnerSecurityToken)
            {
                @this.OwnerSecurityToken = new SecurityTokenBuilder(@this.Strategy.Session)
                    .WithAccessControl(@this.OwnerAccessControl)
                    .Build();
            }
        }

        public static void CoreOnDerive(this User @this, ObjectOnDerive method)
        {
            @this.NormalizedUserName = Users.Normalize(@this.UserName);
            @this.NormalizedUserEmail = Users.Normalize(@this.UserEmail);
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

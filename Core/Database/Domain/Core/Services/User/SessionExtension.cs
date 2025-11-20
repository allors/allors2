// <copyright file="SessionExtension.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Services;
    using Meta;
    using Microsoft.Extensions.DependencyInjection;

    public static partial class SessionExtension
    {
        public static User GetUser(this ISession @this)
        {
            var userService = @this.ServiceProvider.GetRequiredService<IUserService>();
            var userId = userService.UserId;
            return (User) (userId.HasValue ? @this.Instantiate(userId.Value) : new AutomatedAgents(@this).Guest);
        }

        public static void SetUser(this ISession @this, User user)
        {
            var userService = @this.ServiceProvider.GetRequiredService<IUserService>();
            userService.UserId = user.Id;
        }
    }
}

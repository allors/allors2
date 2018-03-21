// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionExtension.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public static partial class SessionExtension
    {
        public static User GetUser(this ISession @this)
        {
            var userService = @this.ServiceProvider.GetRequiredService<IUserService>();
            return @this.Instantiate(userService.Id) as User;
        }

        public static void SetUser(this ISession @this, User user)
        {
            var userService = @this.ServiceProvider.GetRequiredService<IUserService>();
            userService.Id = user?.Id ?? 0;
        }

    }
}
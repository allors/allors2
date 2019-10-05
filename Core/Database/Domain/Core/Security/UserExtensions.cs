// <copyright file="UserExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using Allors.Services;
    using Meta;
    using Microsoft.Extensions.DependencyInjection;

    public static partial class UserExtensions
    {
        public static void CoreOnPostBuild(this User @this, ObjectOnPostBuild method)
        {
            if (!@this.ExistNormalizedUserName)
            {
                @this.NormalizedUserName = @this.UserName?.ToUpperInvariant();
            }
        }

        public static void SetPassword(this User @this, string clearTextPassword)
        {
            var securityService = @this.Strategy.Session.ServiceProvider.GetRequiredService<IPasswordService>();
            var passwordHash = securityService.HashPassword(@this.UserName, clearTextPassword);
            @this.UserPasswordHash = passwordHash;
        }

        public static bool VerifyPassword(this User @this, string clearTextPassword)
        {
            if (string.IsNullOrWhiteSpace(clearTextPassword))
            {
                return false;
            }

            var securityService = @this.Strategy.Session.ServiceProvider.GetRequiredService<IPasswordService>();
            return securityService.VerifyHashedPassword(@this.UserName, @this.UserPasswordHash, clearTextPassword);
        }

        public static string SecurityHash(this User @this)
        {
            var accessControls = @this.AccessControlsWhereEffectiveUser;

            // TODO: Append a Salt
            var idsWithVersion = string.Join(":", accessControls.OrderBy(v => v.Id).Select(v => v.Id + v.Strategy.ObjectVersion));

            var crypt = SHA256.Create();
            var hash = new StringBuilder();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(idsWithVersion), 0, Encoding.UTF8.GetByteCount(idsWithVersion));
            foreach (var theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}

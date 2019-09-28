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
        private static PrefetchPolicy prefetchPolicy;

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

        public static IReadOnlyDictionary<AccessControl, HashSet<long>> EffectivePermissionsByAccessControl(this User user)
        {
            var session = user.Session();

            var effectivePermissionsByAccessControl = new Dictionary<AccessControl, HashSet<long>>();

            var caches = session.GetCache<AccessControlCacheEntry>();
            List<AccessControl> misses = null;
            foreach (AccessControl accessControl in user.AccessControlsWhereEffectiveUser)
            {
                caches.TryGetValue(accessControl.Id, out var cache);
                if (cache == null || !accessControl.CacheId.Equals(cache.CacheId))
                {
                    if (misses == null)
                    {
                        misses = new List<AccessControl>();
                    }

                    misses.Add(accessControl);
                }
                else
                {
                    effectivePermissionsByAccessControl.Add(accessControl, cache.EffectivePermissionIds);
                }
            }

            if (misses != null)
            {
                if (misses.Count > 1)
                {
                    if (prefetchPolicy == null)
                    {
                        prefetchPolicy = new PrefetchPolicyBuilder()
                            .WithRule(M.AccessControl.EffectivePermissions)
                            .Build();
                    }

                    session.Prefetch(prefetchPolicy, misses);
                }

                foreach (var accessControl in misses)
                {
                    var cache = new AccessControlCacheEntry(accessControl);
                    caches[accessControl.Id] = cache;
                    effectivePermissionsByAccessControl.Add(accessControl, cache.EffectivePermissionIds);
                }
            }

            return effectivePermissionsByAccessControl;
        }
    }
}

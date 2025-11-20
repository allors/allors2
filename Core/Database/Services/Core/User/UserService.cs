// <copyright file="UserService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private long? userId;

        public UserService(IHttpContextAccessor httpContextAccessor = null) => this.httpContextAccessor = httpContextAccessor;

        public long? UserId
        {
            get
            {
                if (!this.userId.HasValue)
                {
                    var nameIdentifier =
                        this.httpContextAccessor?.HttpContext.User.Claims
                            .FirstOrDefault(v => v.Type == ClaimTypes.NameIdentifier)
                            ?.Value;

                    if (long.TryParse(nameIdentifier, out var userId))
                    {
                        this.userId = userId;
                    }
                }

                return this.userId;
            }
            set => this.userId = value;
        }
    }
}

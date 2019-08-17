// <copyright file="UserService.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using Microsoft.AspNetCore.Http;

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private string userName;

        public UserService(IHttpContextAccessor httpContextAccessor = null) => this.httpContextAccessor = httpContextAccessor;

        public string UserName
        {
            get => this.userName ?? this.httpContextAccessor?.HttpContext.User.Identity.Name;
            set => this.userName = value;
        }
    }
}

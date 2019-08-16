// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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

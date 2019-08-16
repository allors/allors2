// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Allors bvba">
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

namespace Allors.Services
{
    using Microsoft.AspNetCore.Http;

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private string userName;

        public UserService(IHttpContextAccessor httpContextAccessor = null)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string UserName
        {
            get => this.userName ?? this.httpContextAccessor?.HttpContext.User.Identity.Name;
            set => this.userName = value;
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationController.cs" company="Allors bvba">
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

namespace Allors.Server
{
    using System.Threading.Tasks;

    using Allors.Services;

    using Identity.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class AuthenticationController : Controller
    {
        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AuthenticationController> logger, IConfiguration config, ISessionService sessionService)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.Logger = logger;
            this.Configuration = config;
            this.SessionService = sessionService;
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public SignInManager<ApplicationUser> SignInManager { get; }

        public ILogger Logger { get; }

        public IConfiguration Configuration { get; }

        public ISessionService SessionService { get; }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody]AuthenticationTokenRequest request)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserManager.FindByNameAsync(request.UserName);

                if (user != null)
                {
                    var result = await this.SignInManager.CheckPasswordSignInAsync(user, request.Password, false);
                    if (result.Succeeded)
                    {

                        var token = user.CreateToken(this.Configuration);
                        var response = new AuthenticationTokenResponse
                        {
                            Authenticated = true,
                            UserId = user.Id,
                            Token = token
                        };
                        return this.Ok(response);
                    }
                }
            }

            return this.Ok(new { Authenticated = false });
        }

    }
}

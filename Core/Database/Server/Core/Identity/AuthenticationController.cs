// <copyright file="AuthenticationController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Api
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
        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<AuthenticationController> logger, IConfiguration config, ISessionService sessionService)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.Logger = logger;
            this.Configuration = config;
            this.SessionService = sessionService;
        }

        public UserManager<IdentityUser> UserManager { get; }

        public SignInManager<IdentityUser> SignInManager { get; }

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
                            Token = token,
                        };
                        return this.Ok(response);
                    }
                }
            }

            return this.Ok(new { Authenticated = false });
        }
    }
}

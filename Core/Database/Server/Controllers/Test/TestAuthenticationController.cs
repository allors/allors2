namespace Allors.Server
{
    using System.Threading.Tasks;

    using Identity.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class TestAuthenticationController : Controller
    {
        public TestAuthenticationController(UserManager<ApplicationUser> userManager, ILogger<AuthenticationController> logger, IConfiguration config)
        {
            this.UserManager = userManager;
            this.Logger = logger;
            this.Configuration = config;
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public ILogger Logger { get; }

        public IConfiguration Configuration { get; }

        [HttpPost]
        public async Task<IActionResult> Token([FromBody]AuthenticationTokenRequest request)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserManager.FindByNameAsync(request.UserName);

                if (user != null)
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

            return this.Ok(new { Authenticated = false });
        }
    }
}


namespace Allors.Server
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Identity.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

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
        public async Task<IActionResult> SignIn([FromBody]SignInRequest signInRequest)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserManager.FindByNameAsync(signInRequest.UserName);

                if (user != null)
                {
                    var claims = new[]
                                     {
                                         new Claim(ClaimTypes.Name, user.UserName), // Required for User.Identity.Name
                                         new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                     };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        this.Configuration["Tokens:Issuer"],
                        this.Configuration["Tokens:Issuer"],
                        claims,
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: credentials);

                    var result = new { Authenticated = true, Token = new JwtSecurityTokenHandler().WriteToken(token) };
                    return this.Ok(result);
                }
            }

            return this.Ok(new { Authenticated = false });
        }
    }
}


namespace Allors.Server
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;

    using Microsoft.AspNetCore.Http.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AuthenticationController : Controller
    {
        public const string Scheme = "Allors";

        public AuthenticationController(IAllorsContext allorsContext)
        {
            this.AllorsSession = allorsContext.Session;
        }

        public ISession AllorsSession { get; }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]SignInRequest signInRequest)
        {
            var user = new Users(this.AllorsSession).FindBy(M.User.UserName, signInRequest.UserName);
            if (user != null)
            {
                var passwordHasher = new PasswordHasher<string>();
                if (user.ExistUserPasswordHash && passwordHasher.VerifyHashedPassword(user.UserName, user.UserPasswordHash, signInRequest.Password) != PasswordVerificationResult.Failed)
                {
                    var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };
                    var identity = new ClaimsIdentity(claims, Scheme);
                    var principal = new ClaimsPrincipal(identity);
                    var authenticationProperties = new AuthenticationProperties { IsPersistent = signInRequest.IsPersistent };
                    await this.HttpContext.Authentication.SignInAsync(Scheme, principal, authenticationProperties);
                    return this.Ok();
                }
            }

            return this.Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await this.HttpContext.Authentication.SignOutAsync(Scheme);
            return this.Ok();
        }
    }
}
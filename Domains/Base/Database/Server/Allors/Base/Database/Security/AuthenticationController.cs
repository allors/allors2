namespace Allors.Server
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Principal;

    using Allors.Domain;
    using Allors.Meta;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;

    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationContext authenticationContext;

        public AuthenticationController(IAllorsContext allorsContext, IAuthenticationContext authenticationContext)
        {
            this.authenticationContext = authenticationContext;
            this.AllorsSession = allorsContext.Session;
        }

        public ISession AllorsSession { get; }

        [HttpPost]
        public IActionResult SignIn([FromBody]SignInRequest signInRequest)
        {
            var user = new Users(this.AllorsSession).FindBy(M.User.UserName, signInRequest.UserName);

            if (user != null && user.VerifyPassword(signInRequest.Password))
            {
                var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };
                var identity = new GenericIdentity(user.UserName, "TokenAuth");
                var claimsIdentity = new ClaimsIdentity(identity, claims);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                                                            {
                                                                Issuer = this.authenticationContext.Issuer,
                                                                Audience = this.authenticationContext.Audience,
                                                                SigningCredentials = new SigningCredentials(this.authenticationContext.Key, SecurityAlgorithms.RsaSha256Signature),
                                                                Subject = claimsIdentity,
                                                                Expires = this.authenticationContext.Expires()
                                                            });
                var token = handler.WriteToken(securityToken);

                return this.Ok(
                    new
                        {
                            Authenticated = true,
                            Token = token
                    });
            }

            return this.Ok(new { Authenticated = false });
        }
    }
}


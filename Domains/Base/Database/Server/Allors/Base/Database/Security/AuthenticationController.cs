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
        public AuthenticationController(IAllorsContext allorsContext, IAuthenticationContext authenticationContext)
        {
            this.AllorsSession = allorsContext.Session;
            this.Key = authenticationContext.Key;
            this.Issuer = authenticationContext.Issuer;
            this.Audience = authenticationContext.Audience;
        }

        public ISession AllorsSession { get; }

        public RsaSecurityKey Key { get; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

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
                                                                Issuer = this.Issuer,
                                                                Audience = this.Audience,
                                                                SigningCredentials = new SigningCredentials(this.Key, SecurityAlgorithms.RsaSha256Signature),
                                                                Subject = claimsIdentity,
                                                                Expires = DateTime.Now.AddDays(1)
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


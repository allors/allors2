// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationUser.cs" company="Allors bvba">
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

namespace Identity.Models
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class ApplicationUser : IdentityUser
    {
        private const string TokensKeyKey = "Tokens:Key";

        private const string TokensIssuerKey = "Tokens:Issuer";

        private const string TokensAudienceKey = "Tokens:Audience";

        private const string TokensExpirationKey = "Tokens:Expiration";

        private static readonly TimeSpan DefaultExpiration = new TimeSpan(30, 0, 0, 0);

        public string Id { get; internal set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string CreateToken(IConfiguration configuration)
        {
            var claims = new[]
                             {
                                 new Claim(ClaimTypes.Name, this.UserName), // Required for User.Identity.Name
                                 new Claim(JwtRegisteredClaimNames.Sub, this.UserName),
                                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[TokensKeyKey]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (!TimeSpan.TryParse(configuration[TokensExpirationKey], out var expiration))
            {
                expiration = DefaultExpiration;
            }

            var token = new JwtSecurityToken(
                configuration[TokensIssuerKey],
                configuration[TokensAudienceKey] ?? configuration[TokensIssuerKey],
                claims,
                expires: DateTime.Now.Add(expiration),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

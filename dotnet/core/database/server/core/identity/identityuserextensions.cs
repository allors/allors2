// <copyright file="IdentityUserExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Security
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Allors;
    using Allors.Domain;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public static class IdentityUserExtensions
    {
        private const string TokensKeyKey = "JwtToken:Key";

        private const string TokensIssuerKey = "JwtToken:Issuer";

        private const string TokensAudienceKey = "JwtToken:Audience";

        private const string TokensExpirationKey = "JwtToken:Expiration";

        private static readonly TimeSpan DefaultExpiration = new TimeSpan(30, 0, 0, 0);

        public static string CreateToken(this IdentityUser @this, IConfiguration configuration)
        {
            var claims = new[]
                             {
                                 new Claim(ClaimTypes.Name, @this.UserName), // Required for User.Identity.Name
                                 new Claim(ClaimTypes.NameIdentifier, @this.Id), // Required for UserService
                                 new Claim(JwtRegisteredClaimNames.Sub, @this.UserName),
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

        public static User User(this IdentityUser @this, ISession session) => (User)session.Instantiate(@this.Id);
    }
}

// <copyright file="IdentityUserExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Identity.Models
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public static class IdentityUserExtensions
    {
        private const string TokensKeyKey = "Tokens:Key";

        private const string TokensIssuerKey = "Tokens:Issuer";

        private const string TokensAudienceKey = "Tokens:Audience";

        private const string TokensExpirationKey = "Tokens:Expiration";

        private static readonly TimeSpan DefaultExpiration = new TimeSpan(30, 0, 0, 0);

        public static string CreateToken(this IdentityUser @this, IConfiguration configuration)
        {
            var claims = new[]
                             {
                                 new Claim(ClaimTypes.Name, @this.UserName), // Required for User.Identity.Name
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
    }
}

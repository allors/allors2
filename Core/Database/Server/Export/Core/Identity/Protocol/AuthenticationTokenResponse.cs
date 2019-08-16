// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationTokenResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    public class AuthenticationTokenResponse
    {
        public bool Authenticated { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }
    }
}

// <copyright file="AuthenticationTokenResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Auth
{
    public class AuthenticationTokenResponse
    {
        /// <summary>
        /// Authenticated
        /// </summary>
        public bool a { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public string u { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string t { get; set; }
    }
}


// <copyright file="AuthenticationTokenResponse.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    public class AuthenticationTokenResponse
    {
        public bool Authenticated { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }
    }
}

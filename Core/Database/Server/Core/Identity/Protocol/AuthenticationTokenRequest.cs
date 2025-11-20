// <copyright file="AuthenticationTokenRequest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    public class AuthenticationTokenRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

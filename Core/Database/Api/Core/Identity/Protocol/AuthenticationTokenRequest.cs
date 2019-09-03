// <copyright file="AuthenticationTokenRequest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Api
{
    public class AuthenticationTokenRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

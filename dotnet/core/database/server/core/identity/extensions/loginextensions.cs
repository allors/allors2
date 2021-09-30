// <copyright file="AllorsUserStore.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Security
{
    using Allors.Domain;
    using Microsoft.AspNetCore.Identity;

    public static class LoginExtensions
    {
        public static UserLoginInfo AsUserLoginInfo(this Login @this) => new UserLoginInfo(@this.Provider, @this.Key, @this.DisplayName);
    }
}

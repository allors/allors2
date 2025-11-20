// <copyright file="AllorsUserStore.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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

// <copyright file="AllorsUserStore.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Security
{
    using System.Security.Claims;
    using Allors.Domain;

    public static class IdenityClaimExtensions
    {
        public static Claim AsClaim(this IdentityClaim @this) =>
            new Claim(@this.Type, @this.Value, @this.ValueType, @this.Issuer);
    }
}

// <copyright file="DelegatedAccessControlledObjectDelegateAccess.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public abstract partial class DelegatedAccessControlledObjectDelegateAccess
    {
        public SecurityToken[] SecurityTokens { get; set; }

        public Permission[] DeniedPermissions { get; set; }
    }
}

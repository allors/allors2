// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegatedAccessControlledObjectDelegateAccess.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public abstract partial class DelegatedAccessControlledObjectDelegateAccess
    {
        public SecurityToken[] SecurityTokens { get; set; }

        public Permission[] DeniedPermissions { get; set; }
    }
}

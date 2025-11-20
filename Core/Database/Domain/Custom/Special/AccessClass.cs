// <copyright file="AccessClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class AccessClass
    {
        public void CustomDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (this.Block)
            {
                return;
            }

            var defaultSecurityToken = new SecurityTokens(this.Session()).DefaultSecurityToken;
            var initialSecurityToken = new SecurityTokens(this.Session()).InitialSecurityToken;

            method.SecurityTokens = new[] { defaultSecurityToken, initialSecurityToken };
        }
    }
}

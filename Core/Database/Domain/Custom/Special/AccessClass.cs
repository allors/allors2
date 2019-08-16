
// <copyright file="AccessClass.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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

            var singleton = this.strategy.Session.GetSingleton();
            method.SecurityTokens = new[] { singleton.DefaultSecurityToken, singleton.InitialSecurityToken };
        }
    }
}

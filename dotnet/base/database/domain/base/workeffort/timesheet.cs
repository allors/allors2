// <copyright file="TimeSheet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public partial class TimeSheet
    {
        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                var securityTokens = new[] { new SecurityTokens(this.Session()).DefaultSecurityToken, this.Worker.OwnerSecurityToken };
                method.SecurityTokens = securityTokens;
            }
        }

        public void BaseDelete(DeletableDelete method)
        {
            if (this.ExistTimeEntries)
            {
                throw new Exception("Cannot delete TimeSheet due to associated TimeEntry details");
            }
        }
    }
}

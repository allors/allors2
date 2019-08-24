// <copyright file="OrganisationGlAccounts.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class OrganisationGlAccounts
    {
        protected override void BaseSecure(Security config)
        {
            var write = Operations.Write;
            var closed = new BudgetStates(this.Session).Closed;

            config.Deny(this.ObjectType, closed, write);
        }
    }
}

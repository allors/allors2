// <copyright file="Budgets.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Budgets
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.BudgetState);

        protected override void BaseSecure(Security config)
        {
            var closed = new BudgetStates(this.Session).Closed;
            var opened = new BudgetStates(this.Session).Opened;

            config.Deny(this.ObjectType, closed, Operations.Write);

            config.Deny(this.ObjectType, closed, this.Meta.Close);
            config.Deny(this.ObjectType, opened, this.Meta.Reopen);
        }
    }
}

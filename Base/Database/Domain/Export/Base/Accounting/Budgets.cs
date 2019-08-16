// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Budgets.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using Meta;

    public partial class Budgets
    {
        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, M.BudgetState);
        }

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

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Budget.cs" company="Allors bvba">
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
    public static partial class BudgetExtensions
    {
        public static void AppsOnBuild(this Budget @this, ObjectOnBuild method)
        {
            if (!@this.ExistBudgetState)
            {
                @this.BudgetState = new BudgetStates(@this.Strategy.Session).Opened;
            }
        }

        public static void AppsClose(this Budget @this, BudgetClose method)
        {
            @this.BudgetState = new BudgetStates(@this.Strategy.Session).Closed;
        }

        public static void AppsReopen(this Budget @this, BudgetReopen method)
        {
            @this.BudgetState = new BudgetStates(@this.Strategy.Session).Opened;
        }
    }
}

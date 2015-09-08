// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Budget.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public static partial class BudgetExtensions
    {
        public static void AppsOnDerive(this Budget @this, ObjectOnDerive method)
        {
            if (@this.ExistCurrentObjectState && !@this.CurrentObjectState.Equals(@this.PreviousObjectState))
            {
                var currentStatus = new BudgetStatusBuilder(@this.Strategy.Session).WithBudgetObjectState(@this.CurrentObjectState).Build();
                @this.AddBudgetStatus(currentStatus);
                @this.CurrentBudgetStatus = currentStatus;
            }

            if (@this.ExistCurrentObjectState)
            {
                @this.CurrentObjectState.Process(@this);
            }
        }

        public static void AppsClose(this Budget @this, BudgetClose method)
        {
            @this.CurrentObjectState = new BudgetObjectStates(@this.Strategy.Session).Closed;
        }

        public static void AppsReopen(this Budget @this, BudgetReopen method)
        {
            @this.CurrentObjectState = new BudgetObjectStates(@this.Strategy.Session).Opened;
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeEntryByBillingRateModel.cs" company="Allors bvba">
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

namespace Allors.Domain.Print.WorkTaskModel
{
    using System;
    using System.Linq;

    public class TimeEntryByBillingRateModel
    {
        public TimeEntryByBillingRateModel(IGrouping<decimal?, TimeEntry> @group)
        {
            var timeEntries = @group.ToArray();

            this.BillingRate = @group.Key ?? 0.0m;
            this.AmountOfTime = timeEntries.Sum(v => v.BillableAmountOfTime ?? v.AmountOfTime ?? 0.0m);
            this.BillingAmount = timeEntries.Sum(v => v.BillingAmount);
            this.Cost = timeEntries.Sum(v => v.Cost);

            // Round
            this.BillingRate = Math.Round(this.BillingRate, 2);
            this.AmountOfTime = Math.Round(this.AmountOfTime, 2);
            this.BillingAmount = Math.Round(this.BillingAmount, 2);
            this.Cost = Math.Round(this.Cost, 2);
        }

        public decimal AmountOfTime { get; }

        public decimal BillingRate { get; }

        public decimal BillingAmount { get; }

        public decimal Cost { get; }
    }
}

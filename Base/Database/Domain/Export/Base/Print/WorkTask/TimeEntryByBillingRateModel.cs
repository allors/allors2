// <copyright file="TimeEntryByBillingRateModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

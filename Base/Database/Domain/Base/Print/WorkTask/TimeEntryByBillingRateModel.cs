// <copyright file="TimeEntryByBillingRateModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.WorkTaskModel
{
    using System;
    using System.Globalization;
    using System.Linq;

    public class TimeEntryByBillingRateModel
    {
        public TimeEntryByBillingRateModel(IGrouping<decimal?, TimeEntry> @group)
        {
            var timeEntries = @group.ToArray();

            var billingRate = @group.Key ?? 0.0m;
            var amountOfTime = timeEntries.Where(v => v.IsBillable).Sum(v => v.BillableAmountOfTime ?? v.AmountOfTime ?? 0.0m);
            var billingAmount = timeEntries.Where(v => v.IsBillable).Sum(v => v.BillingAmount);
            var cost = timeEntries.Sum(v => v.Cost);

            // Round
            this.BillingRate = Math.Round(billingRate, 2).ToString("N2", new CultureInfo("nl-BE"));
            this.AmountOfTime = Math.Round(amountOfTime, 2).ToString("N2", new CultureInfo("nl-BE"));
            this.BillingAmount = Math.Round(billingAmount, 2).ToString("N2", new CultureInfo("nl-BE"));
            this.Cost = Math.Round(cost, 2).ToString("N2", new CultureInfo("nl-BE"));
        }

        public string AmountOfTime { get; }

        public string BillingRate { get; }

        public string BillingAmount { get; }

        public string Cost { get; }
    }
}

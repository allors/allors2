
// <copyright file="TimeEntryModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.WorkTaskModel
{
    using System;

    public class TimeEntryModel
    {
        public TimeEntryModel(TimeEntry timeEntry)
        {
            var frequency = timeEntry.TimeFrequency?.Abbreviation ?? timeEntry.TimeFrequency?.Name;

            this.AmountOfTime = Math.Round(timeEntry.BillableAmountOfTime ?? timeEntry.AmountOfTime ?? 0.0m, 2);
            this.BillingRate = Math.Round(timeEntry.BillingRate ?? 0.0m, 2);
            this.BillingAmount = Math.Round(timeEntry.BillingAmount, 2);
            this.Cost = Math.Round(timeEntry.Cost, 2);
            this.TimeFrequency = frequency?.ToUpperInvariant();
            this.WorkerName = timeEntry.TimeSheetWhereTimeEntry?.Worker?.PartyName;
            this.WorkerId = timeEntry.TimeSheetWhereTimeEntry?.Worker?.FirstName;
            this.Description = timeEntry.Description;
            this.IsBillable = timeEntry.IsBillable == true;
            this.FromDate = timeEntry.FromDate.ToString("yyyy-MM-dd");
            this.FromTime = timeEntry.FromDate.ToString("hh:mm:ss");
            this.ThroughDate = timeEntry.ThroughDate?.ToString("yyyy-MM-dd");
            this.ThroughTime = timeEntry.ThroughDate?.ToString("hh:mm:ss");
        }

        public decimal AmountOfTime { get; }

        public decimal BillingRate { get; }

        public decimal BillingAmount { get; }

        public decimal Cost { get; }

        public string TimeFrequency { get; }

        public string WorkerName { get; }

        public string WorkerId { get; }

        public string Description { get; }

        public bool IsBillable { get; }

        public string FromDate { get; }

        public string FromTime { get; }

        public string ThroughDate { get; }

        public string ThroughTime { get; }
    }
}

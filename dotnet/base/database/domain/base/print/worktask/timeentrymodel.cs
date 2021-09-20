// <copyright file="TimeEntryModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.WorkTaskModel
{
    using System;
    using System.Globalization;

    public class TimeEntryModel
    {
        public TimeEntryModel(TimeEntry timeEntry)
        {
            var frequency = timeEntry.TimeFrequency?.Abbreviation ?? timeEntry.TimeFrequency?.Name;

            this.AmountOfTime = Rounder.RoundDecimal(timeEntry.BillableAmountOfTime ?? timeEntry.AmountOfTime ?? 0.0m, 2);
            this.BillingRate = Rounder.RoundDecimal(timeEntry.BillingRate ?? 0.0m, 2).ToString("N2", new CultureInfo("nl-BE"));
            this.BillingAmount = Rounder.RoundDecimal(timeEntry.BillingAmount, 2).ToString("N2", new CultureInfo("nl-BE"));
            this.Cost = Rounder.RoundDecimal(timeEntry.Cost, 2).ToString("N2", new CultureInfo("nl-BE"));
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

        public string BillingRate { get; }

        public string BillingAmount { get; }

        public string Cost { get; }

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

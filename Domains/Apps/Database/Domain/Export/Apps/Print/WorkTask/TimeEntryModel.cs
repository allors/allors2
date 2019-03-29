// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceModel.cs" company="Allors bvba">
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

    public class TimeEntryModel
    {
        public TimeEntryModel(TimeEntry timeEntry)
        {
            if (timeEntry != null)
            {
                var frequency = timeEntry.TimeFrequency?.Abbreviation ?? timeEntry.TimeFrequency.Name;

                this.AmountOfTime = Math.Round(timeEntry.AmountOfTime ?? 0.0m, 2);
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

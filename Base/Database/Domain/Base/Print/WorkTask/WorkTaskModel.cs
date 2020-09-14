// <copyright file="WorkTaskModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.WorkTaskModel
{
    using System;
    using System.Globalization;
    using System.Linq;

    public class WorkTaskModel
    {
        public WorkTaskModel(WorkTask workTask)
        {
            this.Number = workTask.WorkEffortNumber;
            this.Name = workTask.Name;
            this.Description = workTask.Description?.Split('\n');
            this.WorkDone = workTask.WorkDone?.Split('\n');
            
            this.Date = (workTask.ThroughDate() ?? workTask.Strategy.Session.Now()).ToString("yyyy-MM-dd");
            this.Purpose = string.Join(", ", workTask.WorkEffortPurposes.Select(v => v.Name));
            this.Facility = workTask.Facility?.Name;
            this.ContactName = workTask.ContactPerson?.PartyName;
            this.ContactTelephone = workTask.ContactPerson?.CellPhoneNumber?.Description ?? workTask.ContactPerson?.GeneralPhoneNumber?.Description;

            var totalLabour = Math.Round(workTask.ServiceEntriesWhereWorkEffort.OfType<TimeEntry>()
                .Where(v => (v.IsBillable &&
                            !v.BillableAmountOfTime.HasValue && v.AmountOfTime.HasValue) || v.BillableAmountOfTime.HasValue)
                .Sum(v => v.BillingAmount), 2);

            var totalParts = Math.Round(workTask.WorkEffortInventoryAssignmentsWhereAssignment
                .Sum(v => v.Quantity * v.UnitSellingPrice), 2);

            var totalOther = Math.Round(workTask.WorkEffortPurchaseOrderItemAssignmentsWhereAssignment
                .Sum(v => v.Quantity * v.UnitSellingPrice), 2);

            var total = this.TotalLabour + this.TotalParts + this.TotalOther;

            this.TotalLabour = totalLabour.ToString("N2", new CultureInfo("nl-BE"));

            this.TotalParts = totalParts.ToString("N2", new CultureInfo("nl-BE"));

            this.TotalOther = totalOther.ToString("N2", new CultureInfo("nl-BE"));

            this.Total = (totalLabour + totalParts + totalOther).ToString("N2", new CultureInfo("nl-BE"));
        }

        public string Number { get; }

        public string Name { get; }

        public string[] Description { get; }

        public string[] WorkDone { get; }

        public string Purpose { get; }

        public string Date { get; }

        public string PurchaseOrder { get; }

        public string ContactName { get; }

        public string ContactTelephone { get; }

        public string Facility { get; }

        public string TotalLabour { get; }

        public string TotalParts { get; }

        public string TotalOther { get; }

        public string Total { get; }
    }
}

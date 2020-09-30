// <copyright file="Model.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.WorkTaskModel
{
    using System.Linq;

    public class Model
    {
        public Model(WorkTask workTask)
        {
            this.WorkTask = new WorkTaskModel(workTask);
            this.Customer = new CustomerModel(workTask.Customer);
            this.FixedAssetAssignments = workTask.WorkEffortFixedAssetAssignmentsWhereAssignment.Select(v => new FixedAssetAssignmentModel(v)).ToArray();

            this.PurchaseOrderItems = workTask.WorkEffortPurchaseOrderItemAssignmentsWhereAssignment.Select(v => new PurchaseOrderItemAssignmentModel(v)).ToArray();
            this.InventoryAssignments = workTask.WorkEffortInventoryAssignmentsWhereAssignment.Where(v => v.DerivedBillableQuantity > 0).Select(v => new InventoryAssignmentModel(v)).ToArray();
            this.TimeEntries = workTask.ServiceEntriesWhereWorkEffort.OfType<TimeEntry>().Where(v => v.IsBillable).Select(v => new TimeEntryModel(v)).ToArray();
            this.TimeEntriesByBillingRate = workTask.ServiceEntriesWhereWorkEffort.OfType<TimeEntry>()
                .Where(v => v.IsBillable)
                .GroupBy(v => v.BillingRate)
                .Select(v => new TimeEntryByBillingRateModel(v))
                .ToArray();

            this.Barcode = BarcodeName(workTask);
        }

        public static string BarcodeName(WorkTask workTask) => $"Barcode{workTask.Id}";

        public string Logo => "Logo";

        public string Barcode { get; }

        public WorkTaskModel WorkTask { get; }

        public CustomerModel Customer { get; }

        public FixedAssetAssignmentModel[] FixedAssetAssignments { get; }

        public PurchaseOrderItemAssignmentModel[] PurchaseOrderItems { get; }

        public InventoryAssignmentModel[] InventoryAssignments { get; }

        public TimeEntryModel[] TimeEntries { get; }

        public TimeEntryByBillingRateModel[] TimeEntriesByBillingRate { get; }
    }
}

// <copyright file="WorkTaskModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.WorkTaskModel
{
    using System;
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

            this.TotalLabour = Math.Round(workTask.ServiceEntriesWhereWorkEffort.OfType<TimeEntry>()
                .Where(v => (v.IsBillable &&
                            !v.BillableAmountOfTime.HasValue && v.AmountOfTime.HasValue) || v.BillableAmountOfTime.HasValue)
                .Sum(v => v.BillingAmount), 2);

            this.TotalParts = Math.Round(workTask.WorkEffortInventoryAssignmentsWhereAssignment
                .Sum(v => v.Quantity * v.UnitSellingPrice), 2);

            this.TotalOther = Math.Round(workTask.WorkEffortPurchaseOrderItemAssignmentsWhereAssignment
                .Sum(v => v.Quantity * v.UnitSellingPrice), 2);

            this.Total = this.TotalLabour + this.TotalParts + this.TotalOther;

            if (workTask.ExistOrderItemFulfillment)
            {
                if (workTask.OrderItemFulfillment is SalesOrderItem salesOrderItem)
                {
                    var salesOrder = salesOrderItem.SalesOrderWhereSalesOrderItem;
                    this.PurchaseOrder = salesOrder?.OrderNumber;

                    if (salesOrderItem.ExistSalesTerms)
                    {
                        this.PaymentTerms = string.Join(", ", salesOrderItem.SalesTerms.Select(v => v.Description));
                    }
                    else if (salesOrder?.ExistSalesTerms == true)
                    {
                        this.PaymentTerms = string.Join(", ", salesOrder.SalesTerms.Select(v => v.Description));
                    }
                    else if (workTask.Customer?.PaymentNetDays() != null)
                    {
                        this.PaymentTerms = workTask.Customer.PaymentNetDays().ToString();
                    }
                }
            }
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

        public string PaymentTerms { get; }

        public string Facility { get; }

        public decimal TotalLabour { get; }

        public decimal TotalParts { get; }

        public decimal TotalOther { get; }

        public decimal Total { get; }
    }
}

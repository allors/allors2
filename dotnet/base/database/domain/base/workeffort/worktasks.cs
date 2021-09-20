// <copyright file="WorkTasks.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    public partial class WorkTasks
    {
        public static void BaseMonthly(ISession session)
        {
            var customers = new Parties(session).Extent();
            customers.Filter.AddEquals(M.Party.CollectiveWorkEffortInvoice, true);

            var workTasks = new WorkTasks(session).Extent();
            workTasks.Filter.AddEquals(M.WorkEffort.WorkEffortState, new WorkEffortStates(session).Completed);
            workTasks.Filter.AddContainedIn(M.WorkEffort.Customer, (Extent)customers);

            var workTasksByCustomer = workTasks.Select(v => v.Customer).Distinct()
                .ToDictionary(v => v, v => v.WorkEffortsWhereCustomer.Where(w => w.WorkEffortState.Equals(new WorkEffortStates(session).Completed)).ToArray());

            SalesInvoice salesInvoice = null;

            foreach (var customerWorkTasks in workTasksByCustomer)
            {
                var customer = customerWorkTasks.Key;

                var customerWorkTasksByInternalOrganisation = customerWorkTasks.Value
                    .GroupBy(v => v.TakenBy)
                    .Select(v => v)
                    .ToArray();

                if (customerWorkTasks.Value.Any(v => v.CanInvoice))
                {
                    foreach (var group in customerWorkTasksByInternalOrganisation)
                    {
                        if (group.Any(v => v.CanInvoice))
                        {
                            salesInvoice = new SalesInvoiceBuilder(session)
                                .WithBilledFrom(group.Key)
                                .WithBillToCustomer(customer)
                                .WithInvoiceDate(session.Now())
                                .WithSalesInvoiceType(new SalesInvoiceTypes(session).SalesInvoice)
                                .Build();
                        }

                        var timeEntriesByBillingRate = group.SelectMany(v => v.ServiceEntriesWhereWorkEffort.OfType<TimeEntry>())
                            .Where(v => (v.IsBillable && !v.BillableAmountOfTime.HasValue && v.AmountOfTime.HasValue) || v.BillableAmountOfTime.HasValue)
                            .GroupBy(v => v.BillingRate)
                            .Select(v => v)
                            .ToArray();

                        foreach (var rateGroup in timeEntriesByBillingRate)
                        {
                            var timeEntries = rateGroup.ToArray();

                            var invoiceItem = new SalesInvoiceItemBuilder(session)
                                .WithInvoiceItemType(new InvoiceItemTypes(session).Service)
                                .WithAssignedUnitPrice(rateGroup.Key)
                                .WithQuantity(timeEntries.Sum(v => v.BillableAmountOfTime ?? v.AmountOfTime ?? 0.0m))
                                .Build();

                            salesInvoice.AddSalesInvoiceItem(invoiceItem);

                            foreach (var billableEntry in timeEntries)
                            {
                                new TimeEntryBillingBuilder(session)
                                    .WithTimeEntry(billableEntry)
                                    .WithInvoiceItem(invoiceItem)
                                    .Build();
                            }
                        }

                        foreach (var workEffort in group)
                        {
                            if (workEffort.CanInvoice)
                            {
                                if (string.IsNullOrEmpty(salesInvoice.CustomerReference))
                                {
                                    salesInvoice.CustomerReference = $"WorkOrder(s): {workEffort.WorkEffortNumber}";
                                }
                                else
                                {
                                    salesInvoice.CustomerReference += $", {workEffort.WorkEffortNumber}";
                                }

                                foreach (WorkEffortInventoryAssignment inventoryAssignment in workEffort.WorkEffortInventoryAssignmentsWhereAssignment)
                                {
                                    var part = inventoryAssignment.InventoryItem.Part;

                                    var invoiceItem = new SalesInvoiceItemBuilder(session)
                                        .WithInvoiceItemType(new InvoiceItemTypes(session).PartItem)
                                        .WithPart(part)
                                        .WithAssignedUnitPrice(inventoryAssignment.UnitSellingPrice)
                                        .WithQuantity(inventoryAssignment.DerivedBillableQuantity)
                                        .Build();

                                    salesInvoice.AddSalesInvoiceItem(invoiceItem);

                                    new WorkEffortBillingBuilder(session)
                                        .WithWorkEffort(workEffort)
                                        .WithInvoiceItem(invoiceItem)
                                        .Build();
                                }

                                foreach (WorkEffortPurchaseOrderItemAssignment purchaseOrderItemAssignment in workEffort.WorkEffortPurchaseOrderItemAssignmentsWhereAssignment)
                                {
                                    var invoiceItem = new SalesInvoiceItemBuilder(session)
                                        .WithInvoiceItemType(new InvoiceItemTypes(session).Service)
                                        .WithAssignedUnitPrice(purchaseOrderItemAssignment.UnitSellingPrice)
                                        .WithQuantity(purchaseOrderItemAssignment.Quantity)
                                        .Build();

                                    salesInvoice.AddSalesInvoiceItem(invoiceItem);

                                    new WorkEffortBillingBuilder(session)
                                        .WithWorkEffort(workEffort)
                                        .WithInvoiceItem(invoiceItem)
                                        .Build();
                                }

                                workEffort.WorkEffortState = new WorkEffortStates(session).Finished;
                            }
                        }
                    }
                }
            }
        }

        protected override void BaseSecure(Security config)
        {
            var created = new WorkEffortStates(this.Session).Created;
            var inProgress = new WorkEffortStates(this.Session).InProgress;
            var cancelled = new WorkEffortStates(this.Session).Cancelled;
            var completed = new WorkEffortStates(this.Session).Completed;
            var finished = new WorkEffortStates(this.Session).Finished;

            var cancel = this.Meta.Cancel;
            var reopen = this.Meta.Reopen;
            var complete = this.Meta.Complete;
            var invoice = this.Meta.Invoice;
            var revise = this.Meta.Revise;

            config.Deny(this.ObjectType, created, reopen, complete, invoice, revise);
            config.Deny(this.ObjectType, inProgress, cancel, reopen, revise);
            config.Deny(this.ObjectType, cancelled, cancel, invoice, complete, revise);
            config.Deny(this.ObjectType, completed, cancel, reopen, complete);
            config.Deny(this.ObjectType, finished, cancel, reopen, complete, invoice, revise);

            var except = new HashSet<IOperandType>
            {
                this.Meta.ElectronicDocuments.RoleType,
                this.Meta.Print,
            };

            config.DenyExcept(this.ObjectType, cancelled, except, Operations.Write);
            config.DenyExcept(this.ObjectType, completed, except, Operations.Write);
            config.DenyExcept(this.ObjectType, finished, except, Operations.Write);

            config.Deny(M.TimeEntry, cancelled, Operations.Write);
            config.Deny(M.TimeEntry, finished, Operations.Write);
            config.Deny(M.TimeEntry, completed, Operations.Write);
            config.Deny(M.WorkEffortAssignmentRate, cancelled, Operations.Write);
            config.Deny(M.WorkEffortAssignmentRate, finished, Operations.Write);
            config.Deny(M.WorkEffortAssignmentRate, completed, Operations.Write);
            config.Deny(M.WorkEffortInventoryAssignment, cancelled, Operations.Write);
            config.Deny(M.WorkEffortInventoryAssignment, finished, Operations.Write);
            config.Deny(M.WorkEffortInventoryAssignment, completed, Operations.Write);
            config.Deny(M.WorkEffortPartyAssignment, cancelled, Operations.Write);
            config.Deny(M.WorkEffortPartyAssignment, finished, Operations.Write);
            config.Deny(M.WorkEffortPartyAssignment, completed, Operations.Write);
            config.Deny(M.WorkEffortPurchaseOrderItemAssignment, cancelled, Operations.Write);
            config.Deny(M.WorkEffortPurchaseOrderItemAssignment, finished, Operations.Write);
            config.Deny(M.WorkEffortPurchaseOrderItemAssignment, completed, Operations.Write);
            config.Deny(M.WorkEffortFixedAssetAssignment, cancelled, Operations.Write);
            config.Deny(M.WorkEffortFixedAssetAssignment, finished, Operations.Write);
            config.Deny(M.WorkEffortFixedAssetAssignment, completed, Operations.Write);
        }
    }
}

// <copyright file="WorkEffortExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;

    public static partial class WorkEffortExtensions
    {
        public static DateTime? FromDate(this WorkEffort @this) => @this.ActualStart ?? @this.ScheduledStart;

        public static DateTime? ThroughDate(this WorkEffort @this) => @this.ActualCompletion ?? @this.ScheduledCompletion;

        public static void BaseOnBuild(this WorkEffort @this, ObjectOnBuild method)
        {
            if (!@this.ExistWorkEffortState)
            {
                @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Created;
            }

            if (!@this.ExistOwner && @this.Strategy.Session.GetUser() is Person owner)
            {
                @this.Owner = owner;
            }
        }

        public static void BaseOnPreDerive(this WorkEffort @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.ChangeSet.Associations.Contains(@this.Id))
            {
                foreach (WorkEffortInventoryAssignment inventoryAssignment in @this.WorkEffortInventoryAssignmentsWhereAssignment)
                {
                    iteration.AddDependency(inventoryAssignment, @this);
                    iteration.Mark(inventoryAssignment);
                }
            }
        }

        public static void BaseOnDerive(this WorkEffort @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var derivedRoles = (WorkEffortDerivedRoles)@this;

            var internalOrganisations = new Organisations(@this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!@this.ExistTakenBy && internalOrganisations.Count() == 1)
            {
                @this.TakenBy = internalOrganisations.First();
            }

            if (!@this.ExistWorkEffortNumber && @this.ExistTakenBy)
            {
                derivedRoles.WorkEffortNumber = @this.TakenBy.NextWorkEffortNumber();
            }

            if (!@this.ExistExecutedBy && @this.ExistTakenBy)
            {
                @this.ExecutedBy = @this.TakenBy;
            }

            @this.VerifyWorkEffortPartyAssignments(derivation);
            @this.DeriveActualHoursAndDates();
            @this.DeriveCanInvoice();

            if (@this.ExistActualStart && @this.WorkEffortState.IsCreated)
            {
                @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).InProgress;
            }
        }

        public static void BaseOnPostDerive(this WorkEffort @this, ObjectOnPostDerive method)
        {
            if (!@this.CanInvoice)
            {
                @this.AddDeniedPermission(new Permissions(@this.Strategy.Session).Get((Class)@this.Strategy.Class, MetaWorkEffort.Instance.Invoice, Operations.Execute));
            }
            else
            {
                @this.RemoveDeniedPermission(new Permissions(@this.Strategy.Session).Get((Class)@this.Strategy.Class, MetaWorkEffort.Instance.Invoice, Operations.Execute));
            }
        }

        public static void BaseComplete(this WorkEffort @this, WorkEffortComplete method) => @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Completed;

        public static void BaseCancel(this WorkEffort @this, WorkEffortCancel cancel) => @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Cancelled;

        public static void BaseReopen(this WorkEffort @this, WorkEffortReopen reopen)
        {
            if (@this.ExistActualStart)
            {
                @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).InProgress;
            }
            else
            {
                @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Created;
            }
        }

        public static void BaseInvoice(this WorkEffort @this, WorkEffortInvoice method)
        {
            if (!method.Result.HasValue)
            {
                if (@this.CanInvoice)
                {
                    @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Finished;
                    @this.InvoiceThis();
                }

                method.Result = true;
            }
        }

        private static void VerifyWorkEffortPartyAssignments(this WorkEffort @this, IDerivation derivation)
        {
            var existingAssignmentRequired = @this.TakenBy?.RequireExistingWorkEffortPartyAssignment == true;
            var existingAssignments = @this.WorkEffortPartyAssignmentsWhereAssignment.ToArray();

            foreach (ServiceEntry serviceEntry in @this.ServiceEntriesWhereWorkEffort)
            {
                if (serviceEntry is TimeEntry timeEntry)
                {
                    var from = timeEntry.FromDate;
                    var through = timeEntry.ThroughDate;
                    var worker = timeEntry.TimeSheetWhereTimeEntry?.Worker;
                    var facility = timeEntry.WorkEffort.Facility;

                    var matchingAssignment = existingAssignments.FirstOrDefault
                        (a => a.Assignment.Equals(@this)
                        && a.Party.Equals(worker)
                        && ((a.ExistFacility && a.Facility.Equals(facility)) || (!a.ExistFacility && facility == null))
                        && (!a.ExistFromDate || (a.ExistFromDate && (a.FromDate <= from)))
                        && (!a.ExistThroughDate || (a.ExistThroughDate && (a.ThroughDate >= through))));

                    if (matchingAssignment == null)
                    {
                        if (existingAssignmentRequired)
                        {
                            var message = $"No Work Effort Party Assignment matches Worker: {worker}, Facility: {facility}" +
                                $", Work Effort: {@this}, From: {from}, Through {through}";
                            derivation.Validation.AddError(@this, M.WorkEffort.WorkEffortPartyAssignmentsWhereAssignment, message);
                        }
                        else if (worker != null) // Sync a new WorkEffortPartyAssignment
                        {
                            new WorkEffortPartyAssignmentBuilder(@this.Strategy.Session)
                                .WithAssignment(@this)
                                .WithParty(worker)
                                .WithFacility(facility)
                                .Build();
                        }
                    }
                }
            }
        }

        private static void DeriveActualHoursAndDates(this WorkEffort @this)
        {
            var derivedRoles = (WorkEffortDerivedRoles)@this;

            derivedRoles.ActualHours = 0M;

            foreach (ServiceEntry serviceEntry in @this.ServiceEntriesWhereWorkEffort)
            {
                if (serviceEntry is TimeEntry timeEntry)
                {
                    derivedRoles.ActualHours += timeEntry.ActualHours;

                    if (!@this.ExistActualStart)
                    {
                        derivedRoles.ActualStart = timeEntry.FromDate;
                    }
                    else if (timeEntry.FromDate < @this.ActualStart)
                    {
                        derivedRoles.ActualStart = timeEntry.FromDate;
                    }

                    if (!@this.ExistActualCompletion)
                    {
                        derivedRoles.ActualCompletion = timeEntry.ThroughDate;
                    }
                    else if (timeEntry.ThroughDate > @this.ActualCompletion)
                    {
                        derivedRoles.ActualCompletion = timeEntry.ThroughDate;
                    }
                }
            }
        }

        private static SalesInvoice InvoiceThis(this WorkEffort @this)
        {
            var salesInvoice = new SalesInvoiceBuilder(@this.Strategy.Session)
                .WithBilledFrom(@this.TakenBy)
                .WithBillToCustomer(@this.Customer)
                .WithBillToContactMechanism(@this.Customer.ExistBillingAddress ? @this.Customer.BillingAddress : @this.Customer.GeneralCorrespondence)
                .WithBillToContactPerson(@this.ContactPerson)
                .WithInvoiceDate(@this.Strategy.Session.Now())
                .WithSalesInvoiceType(new SalesInvoiceTypes(@this.Strategy.Session).SalesInvoice)
                .Build();

            CreateInvoiceItems(@this, salesInvoice);
            foreach (WorkEffort childWorkEffort in @this.Children)
            {
                CreateInvoiceItems(childWorkEffort, salesInvoice);
            }

            return salesInvoice;
        }

        private static void CreateInvoiceItems(this WorkEffort @this, SalesInvoice salesInvoice)
        {
            var session = @this.Strategy.Session;

            var timeEntriesByBillingRate = @this.ServiceEntriesWhereWorkEffort.OfType<TimeEntry>()
                .Where(v => (v.IsBillable && !v.BillableAmountOfTime.HasValue && v.AmountOfTime.HasValue) || v.BillableAmountOfTime.HasValue)
                .GroupBy(v => v.BillingRate)
                .Select(v => v)
                .ToArray();

            foreach (var rateGroup in timeEntriesByBillingRate)
            {
                var timeEntries = rateGroup.ToArray();

                var invoiceItem = new SalesInvoiceItemBuilder(session)
                    .WithInvoiceItemType(new InvoiceItemTypes(session).Time)
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

            foreach (WorkEffortInventoryAssignment workEffortInventoryAssignment in @this.WorkEffortInventoryAssignmentsWhereAssignment)
            {
                var part = workEffortInventoryAssignment.InventoryItem.Part;

                var invoiceItem = new SalesInvoiceItemBuilder(session)
                    .WithInvoiceItemType(new InvoiceItemTypes(session).PartItem)
                    .WithPart(part)
                    .WithAssignedUnitPrice(workEffortInventoryAssignment.UnitSellingPrice)
                    .WithQuantity(workEffortInventoryAssignment.BillableQuantity ?? workEffortInventoryAssignment.Quantity)
                    .Build();

                salesInvoice.AddSalesInvoiceItem(invoiceItem);

                new WorkEffortBillingBuilder(session)
                    .WithWorkEffort(@this)
                    .WithInvoiceItem(invoiceItem)
                    .Build();
            }
        }

        private static void DeriveCanInvoice(this WorkEffort @this)
        {
            var derivedRoles = (WorkEffortDerivedRoles)@this;

            if (@this.WorkEffortState.Equals(new WorkEffortStates(@this.Strategy.Session).Completed))
            {
                derivedRoles.CanInvoice = true;

                if (@this.ExistWorkEffortWhereChild)
                {
                    derivedRoles.CanInvoice = false;
                }

                if (@this.CanInvoice)
                {
                    foreach (WorkEffort child in @this.Children)
                    {
                        if (!@this.WorkEffortState.Equals(new WorkEffortStates(@this.Strategy.Session).Completed))
                        {
                            derivedRoles.CanInvoice = false;
                            break;
                        }
                    }
                }

                if (@this.CanInvoice)
                {
                    foreach (TimeEntry timeEntry in @this.ServiceEntriesWhereWorkEffort)
                    {
                        if (!timeEntry.ExistThroughDate)
                        {
                            derivedRoles.CanInvoice = false;
                            break;
                        }
                    }
                }

                if (@this.ExistWorkEffortAssignmentRatesWhereWorkEffort && !@this.ExistWorkEffortAssignmentRatesWhereWorkEffort)
                {
                    derivedRoles.CanInvoice = false;
                }
            }
            else
            {
                derivedRoles.CanInvoice = false;
            }
        }
    }
}

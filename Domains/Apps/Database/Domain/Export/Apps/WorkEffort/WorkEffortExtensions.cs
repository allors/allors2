// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkEffortExtensions.cs" company="Allors bvba">
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

using Allors.Meta;
using System;
using System.Linq;

namespace Allors.Domain
{
    public static partial class WorkEffortExtensions
    {
        public static DateTime? FromDate(this WorkEffort @this) => @this.ActualStart ?? @this.ScheduledStart;

        public static DateTime? ThroughDate(this WorkEffort @this) => @this.ActualCompletion ?? @this.ScheduledCompletion;

        public static void AppsOnPreDerive(this WorkEffort @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.ChangeSet.Associations.Contains(@this.Id))
            {
                foreach (WorkEffortPartyAssignment partyAssignment in @this.WorkEffortPartyAssignmentsWhereAssignment)
                {
                    derivation.AddDependency(partyAssignment, @this);
                }

                foreach (WorkEffortInventoryAssignment inventoryAssignment in @this.WorkEffortInventoryAssignmentsWhereAssignment)
                {
                    derivation.AddDependency(inventoryAssignment, @this);
                }
            }
        }

        public static void AppsOnDerive(this WorkEffort @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!@this.ExistOwner)
            {
                if (!(@this.Strategy.Session.GetUser() is Person owner))
                {
                    owner = @this.Strategy.Session.GetSingleton().Guest as Person;
                }

                @this.Owner = owner;
            }

            @this.DeriveOwnerSecurity();
            @this.VerifyWorkEffortPartyAssignments(derivation);
            @this.DeriveActualHoursAndDates();
        }

        public static void AppsOnBuild(this WorkEffort @this, ObjectOnBuild method)
        {
            if (!@this.ExistWorkEffortState)
            {
                @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).NeedsAction;
            }
        }

        public static void AppsConfirm(this WorkEffort @this, WorkEffortConfirm method)
        {
            @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Confirmed;
        }

        public static void AppsComplete(this WorkEffort @this, WorkEffortComplete method)
        {
            @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Completed;
        }

        public static void AppsCancel(this WorkEffort @this, WorkEffortCancel cancel)
        {
            @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Cancelled;
        }

        public static void AppsReopen(this WorkEffort @this, WorkEffortReopen reopen)
        {
            @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).NeedsAction;
        }

        private static void DeriveOwnerSecurity(this WorkEffort @this)
        {
            if (!@this.ExistOwnerAccessControl)
            {
                var ownerRole = new Roles(@this.Strategy.Session).Owner;
                @this.OwnerAccessControl = new AccessControlBuilder(@this.Strategy.Session)
                    .WithRole(ownerRole)
                    .WithSubject(@this.Owner)
                    .Build();
            }

            if (!@this.ExistOwnerSecurityToken)
            {
                @this.OwnerSecurityToken = new SecurityTokenBuilder(@this.Strategy.Session)
                    .WithAccessControl(@this.OwnerAccessControl)
                    .Build();
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
                        && (a.Party.Equals(worker))
                        && (a.ExistFacility && a.Facility.Equals(facility))
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
                        else if (worker != null)  // Sync a new WorkEffortPartyAssignment
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
            @this.ActualHours = 0M;

            foreach (ServiceEntry serviceEntry in @this.ServiceEntriesWhereWorkEffort)
            {
                if (serviceEntry is TimeEntry timeEntry)
                {
                    @this.ActualHours += timeEntry.ActualHours;

                    if (!@this.ExistActualStart)
                    {
                        @this.ActualStart = timeEntry.FromDate;
                    }
                    else if (timeEntry.FromDate < @this.ActualStart)
                    {
                        @this.ActualStart = timeEntry.FromDate;
                    }

                    if (!@this.ExistActualCompletion)
                    {
                        @this.ActualCompletion = timeEntry.ThroughDate;
                    }
                    else if (timeEntry.ThroughDate > @this.ActualCompletion)
                    {
                        @this.ActualCompletion = timeEntry.ThroughDate;
                    }
                }
            }
        }
    }
}

// <copyright file="CommunicationEventExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;
    using Resources;

    public static partial class CommunicationEventExtensions
    {
        public static void BaseOnDerive(this CommunicationEvent @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!@this.ExistOwner && @this.Strategy.Session.GetUser() is Person owner)
            {
                @this.Owner = owner;
            }

            if (@this.ExistScheduledStart && @this.ExistScheduledEnd && @this.ScheduledEnd < @this.ScheduledStart)
            {
                derivation.Validation.AddError(@this, M.CommunicationEvent.ScheduledEnd, ErrorMessages.EndDateBeforeStartDate);
            }

            if (@this.ExistActualStart && @this.ExistActualEnd && @this.ActualEnd < @this.ActualStart)
            {
                derivation.Validation.AddError(@this, M.CommunicationEvent.ActualEnd, ErrorMessages.EndDateBeforeStartDate);
            }

            if (!@this.ExistCommunicationEventState)
            {
                if (!@this.ExistActualStart || (@this.ExistActualStart && @this.ActualStart > @this.Strategy.Session.Now()))
                {
                    @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Scheduled;
                }

                if (@this.ExistActualStart && @this.ActualStart <= @this.Strategy.Session.Now() &&
                    ((@this.ExistActualEnd && @this.ActualEnd > @this.Strategy.Session.Now()) || !@this.ExistActualEnd))
                {
                    @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).InProgress;
                }

                if (@this.ExistActualEnd && @this.ActualEnd <= @this.Strategy.Session.Now())
                {
                    @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Completed;
                }
            }

            if (!@this.ExistInitialScheduledStart && @this.ExistScheduledStart)
            {
                @this.InitialScheduledStart = @this.ScheduledStart;
            }

            if (!@this.ExistInitialScheduledEnd && @this.ExistScheduledEnd)
            {
                @this.InitialScheduledEnd = @this.ScheduledEnd;
            }

            var openCommunicationTasks = @this.TasksWhereWorkItem
                .OfType<CommunicationTask>()
                .Where(v => !v.ExistDateClosed)
                .ToArray();

            if (@this.ExistActualEnd)
            {
                if (openCommunicationTasks.Length > 0)
                {
                    openCommunicationTasks.First().DateClosed = @this.Strategy.Session.Now();
                }
            }
            else
            {
                if (openCommunicationTasks.Length == 0)
                {
                    new CommunicationTaskBuilder(@this.Strategy.Session).WithCommunicationEvent(@this).Build();
                }
            }

            @this.DeriveInvolvedParties();
        }

        public static void BaseOnPostDerive(this CommunicationEvent @this, ObjectOnPostDerive method)
        {
            var session = @this.Strategy.Session;
            @this.AddSecurityToken(new SecurityTokens(session).DefaultSecurityToken);
            @this.AddSecurityToken(@this.Owner?.OwnerSecurityToken);
        }

        public static void BaseDelete(this CommunicationEvent @this, DeletableDelete method)
        {
            foreach (Task task in @this.TasksWhereWorkItem)
            {
                task.Delete();
            }
        }

        public static void BaseClose(this CommunicationEvent @this, CommunicationEventClose method) => @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Completed;

        public static void BaseReopen(this CommunicationEvent @this, CommunicationEventReopen method) => @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Scheduled;

        public static void BaseCancel(this CommunicationEvent @this, CommunicationEventCancel method) => @this.CommunicationEventState = new CommunicationEventStates(@this.Strategy.Session).Cancelled;

        private static void DeriveInvolvedParties(this CommunicationEvent @this)
        {
            var now = @this.Strategy.Session.Now();

            var parties = new[] { @this.FromParty, @this.ToParty, @this.Owner }.Distinct().ToArray();

            var organisation = parties.OfType<Person>()
                .SelectMany(v => v.OrganisationContactRelationshipsWhereContact)
                .Where(v => v.FromDate <= now && (!v.ExistThroughDate || v.ThroughDate >= now))
                .Select(v => v.Organisation);

            @this.DerivedRoles.InvolvedParties = parties.Union(organisation).ToArray();
        }
    }
}

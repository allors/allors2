// <copyright file="ContactMechanism.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class CommunicationTask
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.WorkItem = this.CommunicationEvent;

            this.Title = this.CommunicationEvent.WorkItemDescription;

            // Lifecycle
            if (!this.ExistDateClosed && this.CommunicationEvent.ExistActualEnd)
            {
                this.DateClosed = this.Session().Now();
            }

            this.DeriveParticipants();
        }

        public void BaseDeriveParticipants(TaskDeriveParticipants method)
        {
            if (!method.Result.HasValue)
            {
                var participants = this.ExistDateClosed ? Array.Empty<User>() : new[] { this.CommunicationEvent.FromParty as User };
                this.AssignParticipants(participants);

                method.Result = true;
            }
        }

        public void ManageNotification(TaskAssignment taskAssignment)
        {
            if (!taskAssignment.ExistNotification && this.CommunicationEvent.SendNotification == true && this.CommunicationEvent.RemindAt < this.Strategy.Session.Now())
            {
                var notification = new NotificationBuilder(this.Strategy.Session)
                    .WithTitle("CommunicationEvent: " + this.WorkItem.WorkItemDescription)
                    .WithDescription("CommunicationEvent: " + this.WorkItem.WorkItemDescription)
                    .WithTarget(this)
                    .Build();

                taskAssignment.Notification = notification;
                taskAssignment.User.NotificationList.AddNotification(notification);
            }
        }
    }
}

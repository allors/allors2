// <copyright file="PurchaseInvoiceApproval.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class PurchaseInvoiceApproval
    {
        public void BaseApprove(PurchaseInvoiceApprovalApprove method)
        {
            this.AssignPerformer();

            this.PurchaseInvoice.Approve();

            if (!this.ExistApprovalNotification && this.PurchaseInvoice.ExistCreatedBy)
            {
                var now = this.Strategy.Session.Now();
                var workItemDescription = this.WorkItem.WorkItemDescription;
                var performerName = this.Performer.UserName;
                var comment = this.Comment ?? "N/A";

                var description = $"<h2>Approved...</h2>" +
                                  $"<p>On {now:D} {workItemDescription} was approved by {performerName}</p>" +
                                  $"<h3>Comment</h3>" +
                                  $"<p>{comment}</p>";

                this.ApprovalNotification = new NotificationBuilder(this.Session())
                    .WithTitle("PurchaseInvoice approved")
                    .WithDescription(description)
                    .Build();

                this.PurchaseInvoice.CreatedBy.NotificationList.AddNotification(this.ApprovalNotification);
            }
        }

        public void BaseReject(PurchaseInvoiceApprovalReject method)
        {
            this.AssignPerformer();

            this.PurchaseInvoice.Reject();

            if (!this.ExistRejectionNotification && this.PurchaseInvoice.ExistCreatedBy)
            {
                var now = this.Strategy.Session.Now();
                var workItemDescription = this.WorkItem.WorkItemDescription;
                var performerName = this.Performer.UserName;
                var comment = this.Comment ?? "N/A";

                var description = $"<h2>Approval Rejected...</h2>" +
                                  $"<p>On {now:D} {workItemDescription} was rejected by {performerName}</p>" +
                                  $"<h3>Comment</h3>" +
                                  $"<p>{comment}</p>";

                this.RejectionNotification = new NotificationBuilder(this.Session())
                    .WithTitle("Approval Rejected")
                    .WithDescription(description)
                    .Build();

                this.PurchaseInvoice.CreatedBy.NotificationList.AddNotification(this.RejectionNotification);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.Title = "Approval of " + this.PurchaseInvoice.WorkItemDescription;

            this.WorkItem = this.PurchaseInvoice;

            // Lifecycle
            if (!this.ExistDateClosed && !this.PurchaseInvoice.PurchaseInvoiceState.IsAwaitingApproval)
            {
                this.DateClosed = this.Session().Now();
            }

            this.DeriveParticipants();
        }

        public void BaseDeriveParticipants(TaskDeriveParticipants method)
        {
            if (!method.Result.HasValue)
            {
                var participants = this.ExistDateClosed
                    ? (IEnumerable<Person>)Array.Empty<Person>()
                    : new UserGroups(this.Strategy.Session).Administrators.Members.Select(v => (Person)v).ToArray();
                this.AssignParticipants(participants);

                method.Result = true;
            }
        }

        public void ManageNotification(TaskAssignment taskAssignment)
        {
            if (!taskAssignment.ExistNotification)
            {
                var notification = new NotificationBuilder(this.Strategy.Session).WithTitle(
                        "Approval: " + this.WorkItem.WorkItemDescription)
                        .WithDescription("Approval: " + this.WorkItem.WorkItemDescription)
                        .WithTarget(this)
                        .Build();

                taskAssignment.Notification = notification;
                taskAssignment.User.NotificationList.AddNotification(notification);
            }
        }
    }
}

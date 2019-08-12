namespace Allors.Domain
{
    using System.Linq;

    public partial class PurchaseOrderApprovalLevel1
    {
        public void BaseApprove(PurchaseOrderApprovalLevel1Approve method)
        {
            this.AssignPerformer();

            this.PurchaseOrder.Approve();
        }

        public void BaseReject(PurchaseOrderApprovalLevel1Reject method)
        {
            this.AssignPerformer();

            this.PurchaseOrder.Reject();

            if (!this.ExistRejectionNotification && this.PurchaseOrder.ExistCreatedBy)
            {
                var now = this.Strategy.Session.Now();
                var workItemDescription = this.WorkItem.WorkItemDescription;
                var performerName = this.Performer.LastName + " " + this.Performer.FirstName;
                var comment = this.Comment ?? "N/A";

                var description = $"<h2>Approval Rejected...</h2>" +
                                  $"<p>On {now:D} {workItemDescription} was rejected by {performerName}</p>" +
                                  $"<h3>Comment</h3>" +
                                  $"<p>{comment}</p>";

                this.RejectionNotification = new NotificationBuilder(this.strategy.Session)
                    .WithTitle("Approval Rejected")
                    .WithDescription(description)
                    .Build();

                this.PurchaseOrder.CreatedBy.NotificationList.AddNotification(this.RejectionNotification);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.Title = "Approval of " + this.PurchaseOrder.WorkItemDescription;

            this.WorkItem = this.PurchaseOrder;

            // Lifecycle
            if (!this.ExistDateClosed && !this.PurchaseOrder.PurchaseOrderState.IsAwaitingApprovalLevel1)
            {
                this.DateClosed = this.strategy.Session.Now();
            }

            // Assignments
            var participants = this.ExistDateClosed
                                   ? People.EmptyList
                                   : this.PurchaseOrder.PurchaseOrderState.IsAwaitingApprovalLevel1 ? this.PurchaseOrder.OrderedBy.PurchaseOrderApproversLevel1 : this.PurchaseOrder.OrderedBy.PurchaseOrderApproversLevel2;
            this.AssignParticipants(participants);
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

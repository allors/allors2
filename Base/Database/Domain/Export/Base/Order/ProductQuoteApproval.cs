namespace Allors.Domain
{
    using System.Linq;

    public partial class ProductQuoteApproval
    {
        public void BaseApprove(ProductQuoteApprovalApprove method)
        {
            this.AssignPerformer();

            this.ProductQuote.Approve();
        }

        public void BaseReject(ProductQuoteApprovalReject method)
        {
            this.AssignPerformer();

            this.ProductQuote.Reject();

            if (!this.ExistRejectionNotification && this.ProductQuote.ExistCreatedBy)
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

                this.ProductQuote.CreatedBy.NotificationList.AddNotification(this.RejectionNotification);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            this.Title = "Approval of " + this.ProductQuote.WorkItemDescription;

            this.WorkItem = this.ProductQuote;

            // Lifecycle
            if (!this.ExistDateClosed && !this.ProductQuote.QuoteState.IsCreated)
            {
                this.DateClosed = this.strategy.Session.Now();
            }

            // Assignments
            var participants = this.ExistDateClosed
                                   ? People.EmptyList
                                   : this.ProductQuote.Issuer.ProductQuoteApprovers;
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

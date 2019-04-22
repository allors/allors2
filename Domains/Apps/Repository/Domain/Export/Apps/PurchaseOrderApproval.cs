namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("1D7EE206-1EF9-479D-8458-B44C2CB131AA")]
    #endregion
    public partial class PurchaseOrderApproval : ApproveTask
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public WorkItem WorkItem { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateClosed { get; set; }

        public Person[] Participants { get; set; }

        public Person Performer { get; set; }

        public string Comment { get; set; }

        public Notification ApprovalNotification { get; set; }

        public Notification RejectionNotification { get; set; }


        #endregion

        #region Allors
        [Id("230DE3B0-637D-4311-AF2B-A909134F710E")]
        [AssociationId("D28D41C7-1BA5-48CF-B9C6-DE73EF3F13D2")]
        [RoleId("4B899ECF-F36B-480E-B35C-1593AE8D162B")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        [Workspace]
        [Required]
        public PurchaseOrder PurchaseOrder { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete(){}


        public void Approve() { }

        public void Reject() { }


        #endregion
    }
}
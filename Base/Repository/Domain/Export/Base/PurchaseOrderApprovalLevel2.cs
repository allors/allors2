namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("AC2A9DE8-9368-4BD5-9114-7F14DE98027B")]
    #endregion
    [Plural("PurchaseOrderApprovalsLevel2")]
    public partial class PurchaseOrderApprovalLevel2 : ApproveTask
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
        [Id("FC25DD98-D812-4FF0-8C5A-83CF77E6C3E7")]
        [AssociationId("C6C8B22E-E882-429B-B856-91D0F74852B9")]
        [RoleId("57A368CE-1670-4142-8604-23550040F6E8")]
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

        public void Delete() { }

        public void Approve() { }

        public void Reject() { }

        #endregion
    }
}

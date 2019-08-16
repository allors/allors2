namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("38A75CAC-6669-4118-B72F-057C86693D95")]
    #endregion
    public partial class PurchaseInvoiceApproval : ApproveTask
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
        [Id("EEAB1D40-6E68-43B9-A236-AB5F84880E19")]
        [AssociationId("A2EEB714-EC08-467C-928B-489080101030")]
        [RoleId("C062AE16-F464-4659-B5AF-CAED48BC2558")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        [Workspace]
        [Required]
        public PurchaseInvoice PurchaseInvoice { get; set; }

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

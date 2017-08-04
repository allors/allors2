namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("b2cf9a3d-f156-4da7-87bf-ecdeaa13e326")]
    #endregion
    public partial class WorkTask : WorkEffort 
    {
        #region inherited properties
        public WorkEffortStatus CurrentWorkEffortStatus { get; set; }

        public WorkEffort[] Precendencies { get; set; }

        public Facility Facility { get; set; }

        public Deliverable[] DeliverablesProduced { get; set; }

        public DateTime ActualStart { get; set; }

        public WorkEffortInventoryAssignment[] InventoryItemsNeeded { get; set; }

        public WorkEffort[] Children { get; set; }

        public DateTime ActualCompletion { get; set; }

        public OrderItem OrderItemFulfillment { get; set; }

        public WorkEffortStatus[] WorkEffortStatuses { get; set; }

        public WorkEffortType WorkEffortType { get; set; }

        public InventoryItem[] InventoryItemsProduced { get; set; }

        public WorkEffortPurpose[] WorkEffortPurposes { get; set; }

        public Priority Priority { get; set; }

        public string Name { get; set; }

        public Requirement[] RequirementFulfillments { get; set; }

        public string SpecialTerms { get; set; }

        public WorkEffort[] Concurrencies { get; set; }

        public DateTime ScheduledStart { get; set; }

        public decimal ActualHours { get; set; }

        public string Description { get; set; }

        public WorkEffortObjectState CurrentObjectState { get; set; }

        public DateTime ScheduledCompletion { get; set; }

        public decimal EstimatedHours { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public ObjectState PreviousObjectState { get; set; }

        public ObjectState LastObjectState { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Allors
        [Id("A1070CB5-3492-408C-959A-1C0785C774A0")]
        [AssociationId("16CCFA38-D34E-47E8-B73C-4E57FEF7A0BC")]
        [RoleId("4FD10D7B-1006-4D40-A53F-404B4DA2C379")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public bool SendNotification { get; set; }

        #region Allors
        [Id("55229180-203E-4743-B41B-DA4B4FC1B079")]
        [AssociationId("3375EB99-EA3A-4F91-BA7F-ABE1D63B847C")]
        [RoleId("A5752F2D-5B5A-42A8-9CB8-C11AF8C48880")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public bool SendReminder { get; set; }

        #region Allors
        [Id("413541ED-963E-4036-9347-047456F211E6")]
        [AssociationId("DF6788A3-4D2A-44C6-9213-B14B4FCD708F")]
        [RoleId("8186D56F-CEF3-4D94-A34B-A111454F2BB5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public DateTime RemindAt { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Confirm(){}

        public void Finish(){}

        public void Cancel(){}

        public void Reopen(){}





        public void Delete(){}
        #endregion
    }
}
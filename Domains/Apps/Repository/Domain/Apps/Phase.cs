namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("90a8fa64-c9c7-4a7a-a543-d500668619eb")]
    #endregion
    public partial class Phase : WorkEffort, Versioned
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }
        public AccessControl OwnerAccessControl { get; set; }
        public Person Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkEffortObjectState CurrentObjectState { get; set; }
        public Priority Priority { get; set; }
        public WorkEffortPurpose[] WorkEffortPurposes { get; set; }
        public DateTime ActualCompletion { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledCompletion { get; set; }
        public decimal ActualHours { get; set; }
        public decimal EstimatedHours { get; set; }
        public WorkEffort[] Precendencies { get; set; }
        public Facility Facility { get; set; }
        public Deliverable[] DeliverablesProduced { get; set; }
        public DateTime ActualStart { get; set; }
        public WorkEffortInventoryAssignment[] InventoryItemsNeeded { get; set; }
        public WorkEffort[] Children { get; set; }
        public OrderItem OrderItemFulfillment { get; set; }
        public WorkEffortType WorkEffortType { get; set; }
        public InventoryItem[] InventoryItemsProduced { get; set; }
        public Requirement[] RequirementFulfillments { get; set; }
        public string SpecialTerms { get; set; }
        public WorkEffort[] Concurrencies { get; set; }
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

        #region Versioning
        #region Allors
        [Id("93A2B577-77BB-4327-9370-FAD3F72F6A70")]
        [AssociationId("D1085D7B-B443-43C9-BFE3-A19FF7A8DC62")]
        [RoleId("CBA16F62-1947-42B7-8458-9E29CEFE16A6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PhaseVersion CurrentVersion { get; set; }

        #region Allors
        [Id("5DABEB83-7451-41B9-BBD5-05DE00DCE832")]
        [AssociationId("144DEDED-0340-4A02-98B6-F47D51275920")]
        [RoleId("897C25AA-3569-4DFF-939C-7760342FEC2D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PhaseVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods

        public void OnBuild() { }
        public void OnPostBuild() { }
        public void OnPreDerive() { }
        public void OnDerive() { }
        public void OnPostDerive() { }
        public void Confirm() { }
        public void Finish() { }
        public void Cancel() { }
        public void Reopen() { }
        public void Delete() { }

        #endregion
    }
}
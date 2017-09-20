namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("339a58af-4939-4eee-8028-0fd18119ec34")]
    #endregion
    public partial class Activity : WorkEffort 
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }
        public AccessControl OwnerAccessControl { get; set; }
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
        [Id("4058FCBA-9323-47C5-B165-A3EED8DE70B6")]
        [AssociationId("7FD58473-6579-4269-A4A1-D1BFAE6B3542")]
        [RoleId("DAB0E0A8-712B-4278-B635-92D367F4D41A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ActivityVersion CurrentVersion { get; set; }

        #region Allors
        [Id("DF0E52D4-07B3-45AC-9F36-2C0DE9802C2F")]
        [AssociationId("08A55411-57F6-4015-858D-BE9177328319")]
        [RoleId("BF309243-98E3-457D-A396-3E6BCB06DE6A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ActivityVersion[] AllVersions { get; set; }
        #endregion

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
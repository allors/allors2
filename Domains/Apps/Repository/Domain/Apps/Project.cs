namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("2e2c567e-4c1f-4729-97a1-5ae203be936c")]
    #endregion
    public partial class Project : WorkEffort, IProject 
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
        public IInventoryItem[] InventoryItemsProduced { get; set; }
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

        #region Allors
        [Id("DBE55BB7-113F-4142-AEB7-D59F9DDE5364")]
        [AssociationId("8C9E473B-38D7-48EA-BB8B-07872181B26A")]
        [RoleId("2DB4B219-BDBA-423E-A12C-EAF6FB772482")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProjectVersion CurrentVersion { get; set; }

        #region Allors
        [Id("D8166C38-8C41-4EBD-BABE-1563CB36BF57")]
        [AssociationId("E9E9DD1A-355C-4F80-AD4E-62C9E207EF2F")]
        [RoleId("2492412F-0F2A-4671-9ED5-8E323C0A38CB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProjectVersion PreviousVersion { get; set; }

        #region Allors
        [Id("3193381F-07F1-48C2-B1CB-CA5100C4450E")]
        [AssociationId("DC0A5AEF-17B4-4FFA-8A46-615A022DBE6E")]
        [RoleId("A27282DE-53E6-41B3-9C45-5960B55AC692")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProjectVersion[] AllVersions { get; set; }

        #region Allors
        [Id("C9B64100-8A7E-44F6-906D-FDCE015DBC61")]
        [AssociationId("02F1ABEE-F073-4799-9764-BA24F336FFF9")]
        [RoleId("D0D32C23-01C1-4EBD-85B5-789A0049AFE6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProjectVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("0954FCD3-8479-463F-B412-A666A43F88E8")]
        [AssociationId("AEEBB9D0-F418-46B6-9731-6FEFDE80D5C9")]
        [RoleId("DC4C5226-90BA-4228-A60D-FAA1AE701E87")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProjectVersion[] AllStateVersions { get; set; }

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
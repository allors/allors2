namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("d1d8f99e-430d-4104-a2db-777a0f6292e3")]
    #endregion
    public partial class Research : WorkEffort, IResearch 
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
        [Id("6C3D4B62-710F-4FE9-AC70-A2A617072090")]
        [AssociationId("962CFAEA-6C67-4F13-80F3-CA309E1424F0")]
        [RoleId("47489540-81F6-47B9-A0C8-F4E60D691BB6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ResearchVersion CurrentVersion { get; set; }

        #region Allors
        [Id("B8651BF1-6699-4274-9CBD-507EF9A348AD")]
        [AssociationId("58F0FD84-211B-437B-9A99-C46A1A6B8DF6")]
        [RoleId("FCAD6645-4C37-46CD-A6BC-6C5C7606C387")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ResearchVersion PreviousVersion { get; set; }

        #region Allors
        [Id("2D11618F-F90C-4FC5-83AC-6C781369C95C")]
        [AssociationId("18D2E255-395D-462E-8891-41D9F2514556")]
        [RoleId("F29D7966-05E5-4ED4-9D0E-4A800B6BB4F4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ResearchVersion[] AllVersions { get; set; }

        #region Allors
        [Id("F0B5C54B-5A84-4A0F-A91B-84F7DE93C627")]
        [AssociationId("666F02B9-35EB-4BEA-9C5C-CD41004C79D9")]
        [RoleId("9D59FCF1-7349-48D0-94F7-8F38E075EA14")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ResearchVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("E3993584-1052-4B85-9A5B-B13E7D69DE0A")]
        [AssociationId("1F38B601-37A3-4A78-A075-C94C8D5A7508")]
        [RoleId("D8D4FF46-11B0-4854-88C1-4FC258810238")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ResearchVersion[] AllStateVersions { get; set; }

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
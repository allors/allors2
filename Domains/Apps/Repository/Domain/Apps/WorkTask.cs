namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("76911215-A288-4B0D-BECE-83E7A617B847")]
    #endregion
    public partial class WorkTask : WorkEffort, IWorkTask
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
        public bool SendNotification { get; set; }
        public bool SendReminder { get; set; }
        public DateTime RemindAt { get; set; }

        #endregion

        #region Allors
        [Id("D5808960-5987-435A-B7B6-4D75B8DE8FE2")]
        [AssociationId("75EE66FE-82E8-448E-92F6-0991688077DC")]
        [RoleId("8F5DFD56-0D21-4396-8E3C-30FB4ADBA2CE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WorkTaskVersion CurrentVersion { get; set; }

        #region Allors
        [Id("A99961A2-D91C-4F43-8DCA-D7C4D18E8C8E")]
        [AssociationId("9679524D-55AC-4EC4-A823-FCED61065C04")]
        [RoleId("3C5E7DA7-4772-4253-8D62-3C5630FAD67F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WorkTaskVersion PreviousVersion { get; set; }

        #region Allors
        [Id("78A2865E-C4FF-4EDD-AA05-AC35EF3C59AE")]
        [AssociationId("A213D663-ECB3-481A-B597-6A915982E549")]
        [RoleId("9BDBB56E-7DD4-47CC-B2D0-C76EE4A9A37E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WorkTaskVersion[] AllVersions { get; set; }

        #region Allors
        [Id("4941D8D1-B292-4E23-BB95-3B89FA2B96C1")]
        [AssociationId("B4DA4A6A-D2BE-4B73-82BB-6009CA18C354")]
        [RoleId("839047FF-4EB3-420C-BABE-C6824B072EB8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WorkTaskVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("969B5681-A260-41D3-8A65-A2519935A186")]
        [AssociationId("F93C7703-DB57-4C65-927B-3F485E628E76")]
        [RoleId("56143C67-8F1E-49B1-A582-DD0F450EB9CD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WorkTaskVersion[] AllStateVersions { get; set; }

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
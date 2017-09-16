namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("dfe47c36-58b5-4438-b674-cc2e861922d6")]
    #endregion
    public partial class Program : WorkEffort, IProgram 
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
        [Id("FCD8882C-6D6D-4477-B622-1446776CD812")]
        [AssociationId("01962E0E-290F-42C2-B0AE-84EE8C8E0212")]
        [RoleId("F3602944-5BCF-49DC-990C-CBB377C8AB8C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProgramVersion CurrentVersion { get; set; }

        #region Allors
        [Id("29F40789-1EDD-4D90-ADDF-9D96CFACDEDE")]
        [AssociationId("95A22A58-554E-44DF-9BB3-1AA1DCBDF3EC")]
        [RoleId("CA567D89-9124-42B4-B603-9210C0256A09")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProgramVersion PreviousVersion { get; set; }

        #region Allors
        [Id("FC1F233F-57E7-4337-A868-B3CF06E1F5AA")]
        [AssociationId("606DB65D-D89D-4276-B3C9-52C77D4A4DE5")]
        [RoleId("747AE1F7-C8D0-438A-A613-3FBD69614256")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProgramVersion[] AllVersions { get; set; }

        #region Allors
        [Id("C6589026-12DA-4938-B31A-4218603A4397")]
        [AssociationId("6451BCB2-0FB6-45F2-83B2-67EED83A9FE0")]
        [RoleId("62909C2D-282A-4C28-9630-C95284678EBF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProgramVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("B92184D7-E62E-47FB-8E64-970B225449BB")]
        [AssociationId("2A9321C8-7A0A-4373-87B7-8FF3458D87E0")]
        [RoleId("68558F36-F7C1-42DF-9745-8CB01358F8B5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProgramVersion[] AllStateVersions { get; set; }

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
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
        [Id("242F0627-CABD-414E-ACF1-4256334B5019")]
        [AssociationId("78053FBF-7123-4508-B6BF-AC3CDFA18B5C")]
        [RoleId("01372295-B330-45CE-B378-C36E5D3DE71C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ActivityVersion PreviousVersion { get; set; }

        #region Allors
        [Id("DF0E52D4-07B3-45AC-9F36-2C0DE9802C2F")]
        [AssociationId("08A55411-57F6-4015-858D-BE9177328319")]
        [RoleId("BF309243-98E3-457D-A396-3E6BCB06DE6A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ActivityVersion[] AllVersions { get; set; }

        #region Allors
        [Id("65C11EF5-15A4-4A0C-B0C5-E27A1523244F")]
        [AssociationId("32E373FF-F492-4ECB-B8F3-8966A20BFD45")]
        [RoleId("911ED218-9B72-46B3-8782-CA2E55F1485D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ActivityVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("D94F2C09-2FF4-4803-B43D-5F1A7B67B39B")]
        [AssociationId("7895849E-C39E-45DA-A56D-E614F308ADD4")]
        [RoleId("8289BDBB-06A8-4818-8D7D-E8A5563CA0FE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ActivityVersion[] AllStateVersions { get; set; }

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
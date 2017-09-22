namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("37de59b2-ca6c-4fa9-86a2-299fd6f14812")]
    #endregion
    public partial class ProductionRun : WorkEffort, Versioned
    {
        #region inherited properties

        public SecurityToken OwnerSecurityToken { get; set; }
        public AccessControl OwnerAccessControl { get; set; }
        public Person Owner { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
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
        public Guid UniqueId { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Allors
        [Id("108bb811-ece8-42b4-89e2-7a394f848f4d")]
        [AssociationId("8eeef339-d38c-45c4-8300-61bbb33cb205")]
        [RoleId("abbbac9a-74f3-4e7d-a56e-f5ba0c967530")]
        #endregion
        public int QuantityProduced { get; set; }

        #region Allors
        [Id("407b8671-79ea-4998-b5ed-188dd4a9f43c")]
        [AssociationId("7358c0de-4918-4998-afb8-ecd122e04e3a")]
        [RoleId("56da6402-ddd9-4bbb-83be-3c368de22d09")]
        #endregion
        public int QuantityRejected { get; set; }

        #region Allors
        [Id("558dfd44-26a5-4d64-9317-a121fabaecf1")]
        [AssociationId("69036ddb-1a7b-4bce-8ee2-2610715e47c0")]
        [RoleId("a708d61a-08d1-45db-877b-3eb4514a9069")]
        #endregion
        public int QuantityToProduce { get; set; }

        #region Versioning
        #region Allors
        [Id("350FCFA7-E7A4-4E22-A89B-ECB986C3F656")]
        [AssociationId("7C579167-54F5-4AA0-90C7-14A943452A2E")]
        [RoleId("95D92081-5243-4E6D-B98F-32DB8DA275C1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ProductionRunVersion CurrentVersion { get; set; }

        #region Allors
        [Id("080CA05B-6BB2-4365-ABC1-448C143123B0")]
        [AssociationId("EBFCE320-05B9-473B-9B38-4CCA740B1B53")]
        [RoleId("BF606013-A904-4666-84D1-E0D7703EFD5A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ProductionRunVersion[] AllVersions { get; set; }
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
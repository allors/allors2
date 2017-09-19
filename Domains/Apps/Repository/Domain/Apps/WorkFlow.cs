namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("9549E504-C7BA-41C1-8CE9-8F4425D3C769")]
    #endregion
    public partial class WorkFlow : WorkEffort
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
        [Id("8371E900-C6BB-4C33-A1BA-DA2FEC437FAD")]
        [AssociationId("769BDD32-4EAB-4118-A905-815AC171CE39")]
        [RoleId("754D83F0-E75A-49E1-9E23-5DFB70B0755B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WorkFlowVersion CurrentVersion { get; set; }

        #region Allors
        [Id("57D4BEFA-9669-41A1-8920-183FCDD4AF96")]
        [AssociationId("7F892A90-6B76-44B3-BF1D-6972D4D88EC0")]
        [RoleId("867B5C70-96A6-4187-862C-9ABB5A36949C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WorkFlowVersion PreviousVersion { get; set; }

        #region Allors
        [Id("8070D3C4-5594-4415-972C-7B8F50F7AA68")]
        [AssociationId("68C5AF33-C7CA-408C-A9E1-7AF4386B2194")]
        [RoleId("C1DFFC53-B40C-4167-B065-9ED09A39401D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WorkFlowVersion[] AllVersions { get; set; }

        #region Allors
        [Id("2BDDEB83-8284-471B-9A58-A1342BCE13F2")]
        [AssociationId("F859E0A5-FC78-4514-BA3C-6751FCC149D8")]
        [RoleId("249CA062-2174-4BC2-8191-FD709B815F99")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WorkFlowVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("B0F8FAC5-ECE3-4DC6-90A7-EA47C3EE1BA6")]
        [AssociationId("D882C5C5-8AF7-41DE-B964-1474A9FB2AA9")]
        [RoleId("A6EF21E9-A9AB-4F6E-998D-66FFE9C6710A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WorkFlowVersion[] AllStateVersions { get; set; }

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
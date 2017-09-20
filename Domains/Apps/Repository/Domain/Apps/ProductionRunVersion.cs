namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("C36F15B1-B7BC-4DC2-B3B4-6106B319CB98")]
    #endregion
    public partial class ProductionRunVersion : WorkEffortVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
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

        public Guid VersionId { get; set; }

        public DateTime VersionTimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("4257749A-7767-412B-B742-8B3E88F39D62")]
        [AssociationId("2FB96520-CD81-4567-93BC-A280A6991601")]
        [RoleId("BC8FDC94-5BCF-43C4-9AD1-347B9ADCFB2F")]
        #endregion
        public int QuantityProduced { get; set; }

        #region Allors
        [Id("744354A8-D467-46BC-9F44-A78484EB3162")]
        [AssociationId("6CFB0C3C-4E61-44A0-8AF1-EFB83C06F478")]
        [RoleId("129A660E-B879-4ADD-A628-D5BA2966406E")]
        #endregion
        public int QuantityRejected { get; set; }

        #region Allors
        [Id("EB3B310F-20B1-4FAC-87EC-D546899849E3")]
        [AssociationId("4536DA40-898D-4C22-855A-4C3D4FA9A290")]
        [RoleId("FBE1C888-A46F-4156-85B4-1CE3AB089E75")]
        #endregion
        public int QuantityToProduce { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
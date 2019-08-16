namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("448FA98B-5683-4F0E-9745-AAA1093F5614")]
    #endregion
    public partial class WorkTaskVersion : WorkEffortVersion
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public WorkEffortState WorkEffortState { get; set; }

        public SecurityToken OwnerSecurityToken { get; set; }

        public AccessControl OwnerAccessControl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

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

        public WorkEffort[] Children { get; set; }

        public OrderItem OrderItemFulfillment { get; set; }

        public WorkEffortType WorkEffortType { get; set; }

        public Requirement[] RequirementFulfillments { get; set; }

        public string SpecialTerms { get; set; }

        public WorkEffort[] Concurrencies { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("2C32FA14-D400-407E-B701-0B02BBB30404")]
        [AssociationId("E4D5527E-8E0B-47B3-901B-E52AC27C8A63")]
        [RoleId("ECA9CB64-A3E2-484C-B117-1AFC93A08DCE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public bool SendNotification { get; set; }

        #region Allors
        [Id("D15B652A-76B9-46C0-A948-ABC7F78E3AA9")]
        [AssociationId("FA7C5B7B-C88B-4586-8B2D-D2026A31D896")]
        [RoleId("7A4850C9-1AE6-425F-9720-5A9A09A6740B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public bool SendReminder { get; set; }

        #region Allors
        [Id("5119D560-AA72-4D16-B770-6155D21D0321")]
        [AssociationId("B60E8C6E-C9B4-4CE4-9231-1B083BD1F7D1")]
        [RoleId("32BDE3BE-CFAD-4FB9-AD08-ED14F14C7ED2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public DateTime RemindAt { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}

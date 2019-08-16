namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("0819F7F9-4B4F-4197-AFD6-3DAEABF60AAF")]
    #endregion
    public partial class WorkEffortInventoryProducedVersion : Version
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the WorkEffort under which this Assignment exists.
        /// </summary>
        #region Allors
        [Id("F54C515C-0927-451F-92FD-45796A981C13")]
        [AssociationId("C5D0DE56-3DD0-4405-BD46-003AC98D69B8")]
        [RoleId("BA9655A4-24B4-4B99-B580-F0E51A73FB63")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public WorkEffort Assignment { get; set; }

        /// <summary>
        /// Gets or sets the Part which describes this WorkEffortInventoryProduced.
        /// </summary>
        #region Allors
        [Id("A2238689-EC36-47DA-B6E3-495DB5304D6B")]
        [AssociationId("0887CBD8-6CEC-44B0-A61A-02CB94E0A245")]
        [RoleId("3497802F-563B-4A9D-A038-D9507DBFA02B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Part Part { get; set; }

        /// <summary>
        /// Gets or sets the Quantity of the Part for this WorkEffortInventoryProduced.
        /// </summary>
        #region Allors
        [Id("12DFE479-59FF-449C-AD10-B003DD38B37D")]
        [AssociationId("D33613CD-5F5A-47C9-9060-B291326B6F39")]
        [RoleId("FBC86C3D-9CEF-4C6A-B9E3-4379B8F45C00")]
        #endregion
        [Required]
        [Workspace]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the InventoryItemTransactions create by this WorkEffortInventoryProduced (derived).
        /// </summary>
        #region Allors
        [Id("C1D81C5C-2BCF-4971-AFB7-146A2B4D7DEB")]
        [AssociationId("6AE4D8F9-ADED-47C0-8BD0-4180E550070F")]
        [RoleId("1ACCB4E9-7EFF-49D8-8151-D4B139B44138")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Derived]
        [Workspace]
        public InventoryItemTransaction[] InventoryItemTransactions { get; set; }

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
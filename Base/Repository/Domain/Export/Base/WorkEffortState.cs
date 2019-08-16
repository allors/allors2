namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("f7d24734-88d3-47e7-b718-8815914c9ad4")]
    #endregion
    public partial class WorkEffortState : ObjectState
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public Permission[] ObjectDeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        /// <summary>
        /// Gets or Sets the InventoryTransactionReasons to Create (if they do not exist) for this WorkEffortState.
        /// </summary>
        #region Allors
        [Id("3DBEB0A7-DB3B-4B37-9BCC-5D46842AAF44")]
        [AssociationId("D81D3F31-E72F-4034-B4A3-9C3D004123EE")]
        [RoleId("7BFECC43-BEE3-4BE7-A94C-F1B5EB6946CC")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public InventoryTransactionReason[] InventoryTransactionReasonsToCreate { get; set; }

        /// <summary>
        /// Gets or Sets the InventoryTransactionReasons to Cancel (if they exist) for this WorkEffortState.
        /// </summary>
        #region Allors
        [Id("4F283FBF-1A14-42DC-B277-A567AFE40111")]
        [AssociationId("4FAED165-8EFE-410D-891A-AA0D8E4BC7B4")]
        [RoleId("25B3312B-7DC3-4B12-ACF9-9A29B05D09ED")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public InventoryTransactionReason[] InventoryTransactionReasonsToCancel { get; set; }

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

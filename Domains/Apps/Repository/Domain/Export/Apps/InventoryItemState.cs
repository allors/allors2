namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("d042eeae-5c17-4936-861b-aaa9dfaed254")]
    #endregion
    public partial class InventoryItemState : ObjectState 
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        /// <summary>
        /// Is this InventoryItemState valid for SerialisedInventoryItem objects?
        /// </summary>
        #region Allors
        [Id("A38DDADC-22A1-4606-9059-2D803F3F4EF1")]
        [AssociationId("DEF7CEF2-A688-4B0C-A91C-922C665485D0")]
        [RoleId("EF144C5E-C273-4D23-B863-21F17E7996C3")]
        #endregion
        [Indexed]
        [Workspace]
        public bool IsSerialisedState { get; set; }

        /// <summary>
        /// Is this InventoryItemState valid for NonSerialisedInventoryItem objects?
        /// </summary>
        #region Allors
        [Id("F9FDDE1C-7989-461F-85E0-A602439054F1")]
        [AssociationId("E0C8C47D-D000-4D23-AF68-01EDB2ECB208")]
        [RoleId("EC6B1540-3F06-400A-B2BB-A24D0ED8DBC6")]
        #endregion
        [Indexed]
        [Workspace]
        public bool IsNonSerialisedState { get; set; }

        #region inherited methods

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}
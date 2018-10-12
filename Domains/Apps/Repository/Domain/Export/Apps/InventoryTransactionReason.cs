namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("8ff46109-8ae7-4da5-a1f9-f19d4cf4e27e")]
    #endregion
    public partial class InventoryTransactionReason : Enumeration 
    {
        #region inherited properties
        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        /// <summary>
        /// Gets or Sets a flag to indicate if Manual Entry Is Allowed for this Transaction Reason
        /// </summary>
        #region Allors
        [Id("2CC54ADD-BB3C-4AE9-8970-917D84EC368F")]
        [AssociationId("045C1105-939C-4633-9B1B-B83E41F3C8C8")]
        [RoleId("C0C0ADFC-301C-4F68-95FF-30F523817B05")]
        #endregion
        [Indexed]
        [Required]
        [Workspace]
        public bool IsManualEntryAllowed { get; set; }

        /// <summary>
        /// Gets or Sets the Default NonSerialisedInventoryItemState which corresponds to this InventoryTransactionReason
        /// </summary>
        #region Allors
        [Id("AE9F412A-EF95-4389-BEC2-919809BB5576")]
        [AssociationId("F24B0AA6-08D0-4730-BF78-D92601AFB61C")]
        [RoleId("AC479451-6B6C-492E-A033-A5144E05F299")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public NonSerialisedInventoryItemState DefaultNonSerialisedInventoryItemState { get; set; }

        /// <summary>
        /// Gets or Sets the Default SerialisedInventoryItemState which corresponds to this InventoryTransactionReason
        /// </summary>
        #region Allors
        [Id("D9B698C9-E5EC-4E1E-88AB-C8E3672835FF")]
        [AssociationId("3BDB80E5-D25A-4712-A4E3-9A695F7A63B7")]
        [RoleId("CE6BBC0A-6651-4D53-B8C7-BAFE2EC4ADBF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public SerialisedInventoryItemState DefaultSerialisedInventoryItemState { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

        #region Allors
        [Id("DC221B1A-893D-40A0-9088-2D8422593F11")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}
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
        /// Gets or Sets a flag to indicate if this TransactionReason Affects the QuantityOnHand for InventoryItem objects.
        /// </summary>
        #region Allors
        [Id("62899C64-9139-4CDA-961E-0F2DA52D6EB9")]
        [AssociationId("64E3418C-8561-480A-BE28-D893561D2E3B")]
        [RoleId("9BEC0DAE-8560-4E72-9774-D6501EC05DBF")]
        #endregion
        [Indexed]
        [Required]
        [Workspace]
        public bool AffectsQuantityOnHand { get; set; }

        /// <summary>
        /// Gets or Sets a flag to indicate if this TransactionReason Affects the QuantityCommittedOut for InventoryItem objects.
        /// </summary>
        #region Allors
        [Id("1A13F812-685E-428E-A255-3E8C6F76A58E")]
        [AssociationId("2353C89D-DA1E-4388-9878-FF27F7439DB5")]
        [RoleId("7F414807-275F-4C69-AA83-3BD8AEB44DFC")]
        #endregion
        [Indexed]
        [Required]
        [Workspace]
        public bool AffectsQuantityCommittedOut { get; set; }

        /// <summary>
        /// Gets or Sets a flag to indicate if this TransactionReason Affects the QuantityExpectedIn for InventoryItem objects.
        /// </summary>
        #region Allors
        [Id("EE2FE83F-F223-4B6B-B777-ABD8DE074491")]
        [AssociationId("48B5A5DA-27D1-4C8B-8272-FFFB6A44C33F")]
        [RoleId("5909E526-61F2-4EEF-8DD1-D98FDECE8706")]
        #endregion
        [Indexed]
        [Required]
        [Workspace]
        public bool AffectsQuantityExpectedIn { get; set; }

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
// <copyright file="InventoryTransactionReason.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

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
        /// Gets or Sets a flag to indicate if Manual Entry Is Allowed for this Transaction Reason.
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
        /// Gets or Sets a flag to indicate if this InventoryTransactionReason IncreasesQuantityCommittedOut for InventoryItem objects.
        /// True values increase inventory, False values decrease inventory, and null values do not affect inventory.
        /// </summary>
        #region Allors
        [Id("8F42A67D-7951-4450-8D31-7A4CBE864656")]
        [AssociationId("99877B3E-C716-41C9-A888-128104F68351")]
        [RoleId("D2908EE3-C7AE-4395-BC88-36161F50F92D")]
        #endregion
        [Indexed]
        [Workspace]
        public bool IncreasesQuantityCommittedOut { get; set; }

        /// <summary>
        /// Gets or Sets a flag to indicate if this InventoryTransactionReason IncreasesQuantityExpectedIn for InventoryItem objects.
        /// True values increase inventory, False values decrease inventory, and null values do not affect inventory.
        /// </summary>
        #region Allors
        [Id("15D50828-0A4B-4589-914F-85EE9D7D13A3")]
        [AssociationId("91F8C21E-3A64-424A-9166-1FD47A1D287E")]
        [RoleId("9CF55733-CDDE-43E8-A7D7-5BBEB1140052")]
        #endregion
        [Indexed]
        [Workspace]
        public bool IncreasesQuantityExpectedIn { get; set; }

        /// <summary>
        /// Gets or Sets a flag to indicate if this InventoryTransactionReason IncreasesQuantityOnHand for InventoryItem objects.
        /// True values increase inventory, False values decrease inventory, and null values do not affect inventory.
        /// </summary>
        #region Allors
        [Id("C7AD0CE1-D5D4-4E2A-9E36-006BBB4E82AA")]
        [AssociationId("43EC0DAC-30DA-46A8-A3A5-F2A0449FCA01")]
        [RoleId("308D8701-D143-4D61-94B1-2BEE8344C8F9")]
        #endregion
        [Indexed]
        [Workspace]
        public bool IncreasesQuantityOnHand { get; set; }

        /// <summary>
        /// Gets or Sets the Default NonSerialisedInventoryItemState which corresponds to this InventoryTransactionReason.
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
        /// Gets or Sets the Default SerialisedInventoryItemState which corresponds to this InventoryTransactionReason.
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

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

        #region Allors
        [Id("DC221B1A-893D-40A0-9088-2D8422593F11")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}

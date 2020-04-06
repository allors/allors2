// <copyright file="InventoryItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("61af6d19-e8e4-4b5b-97e8-3610fbc82605")]
    #endregion
    public partial interface InventoryItem : UniquelyIdentifiable, Transitional, Deletable, Versioned, Searchable
    {
        /// <summary>
        /// Gets or sets the Part for which this InventoryItem tracks inventory information.
        /// </summary>
        #region Allors
        [Id("BCC41DF1-D526-4C78-8F68-B32AB104AD12")]
        [AssociationId("B3E13E3F-3976-4920-A602-8D371210B35F")]
        [RoleId("851F9536-0B23-4536-8060-A547CEF802D5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Indexed]
        [Workspace]
        Part Part { get; set; }

        /// <summary>
        /// Gets or sets the Facility at which this InventoryItem tracks inventory information.
        /// </summary>
        #region Allors
        [Id("BC234CEA-DC2E-4BDC-B911-5A12D1D6F354")]
        [AssociationId("DCA4388A-D549-4CEA-931B-074244DE8E18")]
        [RoleId("94231D6C-7699-4428-AFFE-A459C8208394")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Indexed]
        [Workspace]
        Facility Facility { get; set; }

        /// <summary>
        /// Gets or sets the UnitOfMeasure which describes the inventory tracked by this Inventory Item.
        /// </summary>
        #region Allors
        [Id("D276D126-34D3-4820-884C-EC9944B5E10B")]
        [AssociationId("8AD230B1-A664-4A4D-A58C-FAFB98C11762")]
        [RoleId("730949B3-3CDE-46F7-816B-331AFFF7AEF5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("EB6EFE43-6584-4460-ACA8-63153FCAECFF")]
        [AssociationId("0F891CC1-146A-4C7D-9F10-3D23A684C0E7")]
        [RoleId("14272F0F-1A1A-4086-B3F0-278A58370DAB")]
        #endregion
        [Derived]
        [Size(256)]
        [Workspace]
        string Name { get; set; }

        #region Allors
        [Id("2678441b-342c-4b94-a5c7-d8c9e07de6b4")]
        [AssociationId("31b14e4c-c3bf-4e50-83ca-3bcb190ffda1")]
        [RoleId("175b37b3-94a5-41a0-9cc1-f3a9d18d9f39")]
        #endregion
        [Derived]
        [Size(256)]
        [Workspace]
        string PartDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the (optional) Lot in which this InventoryItem tracks inventory information.
        /// </summary>
        #region Allors
        [Id("8573F543-0EB9-4A5E-A68F-CC69CD5CF8F9")]
        [AssociationId("D4523FD7-ADE5-44A6-B982-9738212BD809")]
        [RoleId("2EF751E3-6250-4DDF-BBFF-B8E468A8B7D4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        Lot Lot { get; set; }
    }
}

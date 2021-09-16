// <copyright file="WorkEffortInventoryProduced.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("0E74304B-1B18-41E7-A20B-3DC1E46A8504")]
    #endregion
    public partial class WorkEffortInventoryProduced : Versioned, Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the WorkEffort under which this Assignment exists.
        /// </summary>
        #region Allors
        [Id("B978B71D-4FC4-4820-B535-2CE8E7A1E60D")]
        [AssociationId("4148A9F1-7EA5-4781-A60B-F96CBD2AECBE")]
        [RoleId("E40CB541-BD19-42B3-933B-ABB70C97D6A3")]
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
        [Id("7A5B793B-866E-40F2-BBD1-A8FB65AF9EDA")]
        [AssociationId("6F450541-CC96-4135-971F-F9B95CE967C3")]
        [RoleId("2350FCF9-60C5-4CF2-8A93-285D446665CC")]
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
        [Id("1B1F154E-5C33-4E9C-9318-4BACC7827A9E")]
        [AssociationId("2B275C54-ADCD-4D7A-8B3A-38D63E0B73CD")]
        [RoleId("A89F1001-C3A7-43D2-AF1D-4F8B4FD5830D")]
        #endregion
        [Required]
        [Workspace]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the InventoryItemTransactions create by this WorkEffortInventoryProduced (derived).
        /// </summary>
        #region Allors
        [Id("FE7B73C6-CF83-4A89-8087-E469A1C8819A")]
        [AssociationId("D9F138C0-11AE-42DC-9F7B-E780BFA1D952")]
        [RoleId("3DFCC7CE-6EAD-4F47-8B85-F4820978282F")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Derived]
        [Workspace]
        public InventoryItemTransaction[] InventoryItemTransactions { get; set; }

        #region Versioning
        #region Allors
        [Id("9D6E6FCF-AE5D-42EB-942E-E1B5AC9AE8C8")]
        [AssociationId("DC5FB29F-040E-4597-88BD-81BF556D0F39")]
        [RoleId("C6A3C426-2025-4025-9972-6434C53E66A1")]
        #endregion
        [Indexed]
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public WorkEffortInventoryProducedVersion CurrentVersion { get; set; }

        #region Allors
        [Id("8CAE04DC-EC29-4366-A322-64602710A287")]
        [AssociationId("145C05B0-D756-4363-9C9F-072272444913")]
        [RoleId("E1ACC109-AA8C-47A9-B245-AAE31BFEF825")]
        #endregion
        [Indexed]
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public WorkEffortInventoryProducedVersion[] AllVersions { get; set; }
        #endregion

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

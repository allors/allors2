// <copyright file="SalesOrderItemInventoryAssignmentVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("7CF1B3F8-46B1-4DDE-92F9-455D6E30D37F")]
    #endregion
    public partial class SalesOrderItemInventoryAssignmentVersion : Version
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        #endregion

        #region Allors
        [Id("5CE3E9F4-BBD1-4134-A187-6570D1D7E52A")]
        [AssociationId("F88FC440-5AEC-4207-98C7-2C499BE4F43A")]
        [RoleId("F4FB6672-162B-47F1-B0BF-C14EA2D1423E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InventoryItem InventoryItem { get; set; }

        #region Allors
        [Id("47239001-6F4D-4EDA-9DE4-3805629017F1")]
        [AssociationId("5CD4ACF5-A79B-4454-97C9-BF5D14E864C0")]
        [RoleId("CFF28DE0-9B4C-42D2-B444-E1895C14ACBD")]
        #endregion
        [Required]
        [Workspace]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("B05C8F82-ABA9-464A-852B-885F6C89AEEB")]
        [AssociationId("3FB372FC-122C-4C8C-A41B-B185364A6A06")]
        [RoleId("5116D63A-4070-4BF4-9742-F1AF9038E18F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
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

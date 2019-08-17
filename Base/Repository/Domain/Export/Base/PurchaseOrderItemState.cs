// <copyright file="PurchaseOrderItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("ad76acee-eccc-42ce-9897-8c3f0252caf4")]
    #endregion
    public partial class PurchaseOrderItemState : ObjectState
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public Permission[] ObjectDeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        /// <summary>
        /// Gets or Sets the InventoryTransactionReasons to Create (if they do not exist) for this PurchaseOrderItem.
        /// </summary>
        #region Allors
        [Id("E71A4607-F116-4E6F-B50F-A22A6F9972B8")]
        [AssociationId("509BC447-0A54-45BE-96CD-A74805709C67")]
        [RoleId("5C1A5EA0-DC9A-4728-9B32-BC16717F3331")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public InventoryTransactionReason[] InventoryTransactionReasonsToCreate { get; set; }

        /// <summary>
        /// Gets or Sets the InventoryTransactionReasons to Cancel (if they exist) for this PurchaseOrderItem.
        /// </summary>
        #region Allors
        [Id("A329C0E7-407E-4544-9D88-93BCB219E7E2")]
        [AssociationId("BBD2D530-6901-4C39-A55B-DDDF1DD8F2D3")]
        [RoleId("660FDF48-F77B-439C-BFF3-65F61B81902F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
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

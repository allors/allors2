// <copyright file="SalesOrderItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("21f09e4c-7b3f-4152-8822-8c485011759c")]
    #endregion
    public partial class SalesOrderItemState : ObjectState
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Permission[] ObjectDeniedPermissions { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        /// <summary>
        /// Gets or Sets the InventoryTransactionReasons to Create (if they do not exist) for this SalesOrderItem.
        /// </summary>
        #region Allors
        [Id("ADB361F0-E328-4FB1-9735-D4D9CDC40BFA")]
        [AssociationId("7E23E7A3-3BEC-4751-A986-4D9AFF9E3270")]
        [RoleId("AA6E3BA9-D2BA-4FF9-926A-0B88A85C51C4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        public InventoryTransactionReason[] InventoryTransactionReasonsToCreate { get; set; }

        /// <summary>
        /// Gets or Sets the InventoryTransactionReasons to Cancel (if they exist) for this SalesOrderItem.
        /// </summary>
        #region Allors
        [Id("9E60A60B-C036-4E37-B71F-B02FD1B029E4")]
        [AssociationId("FF4A9D11-D678-4FD3-B09B-00B0E62E224E")]
        [RoleId("BAC9F954-1871-4DFC-AACF-215223F99D3F")]
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

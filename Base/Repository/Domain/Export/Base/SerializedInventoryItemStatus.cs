// <copyright file="SerializedInventoryItemStatus.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("1da3e549-47cb-4896-94ec-3f8a263bb559")]
    #endregion
    public partial class SerialisedInventoryItemStatus : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("aabb931a-38ee-4568-af8c-5f8ed98ed7b9")]
        [AssociationId("85598163-c71c-4bdc-942b-5ad461943e01")]
        [RoleId("87e945cc-864b-42b6-ad9b-c3d447d96073")]
        #endregion
        [Required]

        public DateTime StartDateTime { get; set; }
        #region Allors
        [Id("d2c2fff8-73ec-4748-9c8f-29304abbdb0d")]
        [AssociationId("ee25cfd7-7389-4db7-9bb2-ee388e57f6d1")]
        [RoleId("584017a5-99b5-414a-b32e-c64f7f2a0d4e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public SerialisedInventoryItemObjectState SerialisedInventoryItemObjectState { get; set; }

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

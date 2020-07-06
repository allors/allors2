// <copyright file="MiscellaneousCharge.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("0be4eedc-a33a-4ded-9c30-5798851b3a3f")]
    #endregion
    public partial class MiscellaneousCharge : OrderAdjustment
    {
        #region inherited properties
        public decimal Amount { get; set; }

        public decimal Percentage { get; set; }

        public string Description { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Versioning
        #region Allors
        [Id("276bfe6d-ac6c-4036-9849-51f50ddb8d55")]
        [AssociationId("c42f941b-a6e2-49f2-b5d5-ae86b3d52493")]
        [RoleId("70b54993-cf19-4f33-ba59-bbe8c6ccaa6a")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public MiscellaneousChargeVersion CurrentVersion { get; set; }

        #region Allors
        [Id("c948d6ae-6a64-41de-b86d-3d194cfccebf")]
        [AssociationId("8fb40421-a4de-48c9-ad2f-fd87113868c8")]
        [RoleId("f12ab47e-143e-448b-b9f1-16ca4b786c6f")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public MiscellaneousChargeVersion[] AllVersions { get; set; }
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

        public void Delete()
        {
        }

        #endregion
    }
}

// <copyright file="ShippingAndHandlingCharge.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("e7625d17-2485-4894-ba1a-c565b8c6052c")]
    #endregion
    public partial class ShippingAndHandlingCharge : OrderAdjustment
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
        [Id("ec66e7f7-6792-43d7-bd8e-f0bf10c48b4e")]
        [AssociationId("7869f6dc-8d2a-4e27-bf1d-f520896ec052")]
        [RoleId("0e358a11-dc67-4f30-814f-5d7d26b076fe")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public ShippingAndHandlingChargeVersion CurrentVersion { get; set; }

        #region Allors
        [Id("c09581be-2da0-4268-bb34-e025393bda1e")]
        [AssociationId("36dc7849-2602-4643-ada3-00dc4781368e")]
        [RoleId("c84b1293-d484-4db7-aa32-481d42496483")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public ShippingAndHandlingChargeVersion[] AllVersions { get; set; }
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

// <copyright file="DiscountAdjustment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("0346a1e2-03c7-4f1e-94ae-35fdf64143a9")]
    #endregion
    public partial class DiscountAdjustment : OrderAdjustment, Versioned
    {
        #region inherited properties
        public decimal Amount { get; set; }

        public VatRate VatRate { get; set; }

        public decimal Percentage { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Versioning
        #region Allors
        [Id("69911C68-D349-4C37-8819-08700AE61681")]
        [AssociationId("556775B9-5841-4C9D-A2BF-A7B02ECA57BE")]
        [RoleId("2A829C06-54B7-41A2-98AD-DBCAE9FB666B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public DiscountAdjustmentVersion CurrentVersion { get; set; }

        #region Allors
        [Id("D5901ABB-AFBC-4983-82AD-C143BE40B05F")]
        [AssociationId("63AB0E87-039E-4B81-8D5B-CD775262D6C1")]
        [RoleId("CFA3A77A-F4DD-4403-BB24-5F9AE7CAA770")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public DiscountAdjustmentVersion[] AllVersions { get; set; }
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

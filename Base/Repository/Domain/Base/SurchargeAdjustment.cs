// <copyright file="SurchargeAdjustment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("70468d86-b8a0-4aff-881e-fca2386f64da")]
    #endregion
    public partial class SurchargeAdjustment : OrderAdjustment
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
        [Id("1c3a332f-2893-4389-83b5-49b11b545cb4")]
        [AssociationId("dd138f6d-a04a-4dc0-9068-533e5e1a13f8")]
        [RoleId("0ea8ffdd-de36-42fb-ab3c-7d91f9bd9229")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SurchargeAdjustmentVersion CurrentVersion { get; set; }

        #region Allors
        [Id("9875d102-b52a-4a73-bf02-6c8a8a7cccb2")]
        [AssociationId("0758924f-c359-436a-8cb8-e2c59497e06a")]
        [RoleId("e8bd6fd2-71e7-4ad6-8a80-d473c2d27dd4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SurchargeAdjustmentVersion[] AllVersions { get; set; }
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

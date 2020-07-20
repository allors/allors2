// <copyright file="Fee.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("fb3dd618-eeb5-4ef6-87ca-abfe91dc603f")]
    #endregion
    public partial class Fee : OrderAdjustment
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
        [Id("dcd863f5-cb25-4f75-be85-2f381d929d08")]
        [AssociationId("52315212-1d03-4a46-b470-08282faae681")]
        [RoleId("1685c861-6977-4a1e-b60f-2e275c2fd102")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public FeeVersion CurrentVersion { get; set; }

        #region Allors
        [Id("a4aeb379-8bb3-4eeb-ab47-feff9db8196d")]
        [AssociationId("4412a680-c264-459a-a092-c7e070fdd65b")]
        [RoleId("5c9b6db4-b869-4e18-b042-7728e0456c41")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public FeeVersion[] AllVersions { get; set; }
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

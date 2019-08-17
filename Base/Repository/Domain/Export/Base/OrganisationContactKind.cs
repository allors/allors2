// <copyright file="OrganisationContactKind.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("9570d60a-8baa-439c-99f4-472d10952165")]
    #endregion
    public partial class OrganisationContactKind : UniquelyIdentifiable, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("5d3446a3-ab54-4c49-89bb-928b082bb4b7")]
        [AssociationId("a1b7eec7-d13f-47da-b028-4db580da07a4")]
        [RoleId("291d3e15-301a-4865-9097-5407dadd65ff")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        public string Description { get; set; }

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

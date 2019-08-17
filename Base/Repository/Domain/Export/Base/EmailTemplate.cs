// <copyright file="EmailTemplate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c78a49b1-9918-4f15-95f3-c537c82f59fd")]
    #endregion
    public partial class EmailTemplate : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("21bbeaa8-f4cf-4b09-9fcd-af72a70e6f15")]
        [AssociationId("18d3ed19-fcac-4010-9bcb-2c0f6f41acc1")]
        [RoleId("27ade42e-f19f-444a-9134-db74add756b3")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("8bb431b6-a6ea-48d0-ad78-975ec26b470f")]
        [AssociationId("15e1b022-709b-4443-a85c-c1b2956c14e9")]
        [RoleId("8ce6a6a6-2387-4dd7-8bea-dec068aec152")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string BodyTemplate { get; set; }

        #region Allors
        [Id("f05fc608-5dcd-4d7d-b472-5b84c2a195a4")]
        [AssociationId("c00233a0-c9a2-4c01-88fc-9ea5eb7fd564")]
        [RoleId("c39a94b3-455b-4602-8d55-abb2fca560ed")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string SubjectTemplate { get; set; }

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

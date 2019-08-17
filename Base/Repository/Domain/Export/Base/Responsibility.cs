// <copyright file="Responsibility.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("3aa7bf17-bd02-4587-9006-177845ae69df")]
    #endregion
    public partial class Responsibility : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("a570dd47-5bb6-4a37-b73e-3a9f7b3f37ee")]
        [AssociationId("0f98ce04-447c-497c-b63b-f943eb818c84")]
        [RoleId("9ccfe2ef-4980-43d8-9c5b-247c93c902b7")]
        #endregion
        [Required]
        [Size(-1)]

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

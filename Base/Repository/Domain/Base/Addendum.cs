// <copyright file="Addendum.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("7baa7594-6890-4e1e-8c06-fc49b3ea262d")]
    #endregion
    public partial class Addendum : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("2aaa6623-6f1a-4b40-91f0-4014108549d6")]
        [AssociationId("071735c4-bfbe-4f30-87a4-fbb4accc540c")]
        [RoleId("d9dea2e1-6582-4ce4-863f-4819d2cffe96")]
        #endregion
        [Size(-1)]

        public string Text { get; set; }

        #region Allors
        [Id("30b99ed6-cb44-4401-b5bd-76c0099153d4")]
        [AssociationId("002ba83d-d60f-4365-90e0-4df952697ae7")]
        [RoleId("cfa04c20-ecc5-4942-b898-2966bf5052aa")]
        #endregion

        public DateTime EffictiveDate { get; set; }

        #region Allors
        [Id("45a9d28e-f131-44a8-aea5-1a9776be709e")]
        [AssociationId("4b41aff4-1882-4771-a85b-358cabdb6e3c")]
        [RoleId("8b37c47b-3dec-46e6-b669-6497cfdaf14b")]
        #endregion
        [Required]
        [Size(-1)]

        public string Description { get; set; }

        #region Allors
        [Id("f14af73d-8d7d-4c5b-bc6a-957830fd0a80")]
        [AssociationId("5430d382-14ff-4af1-8e1b-3b11142612e4")]
        [RoleId("51fc58ba-e9fb-4919-94e8-c8594f6e4ea5")]
        #endregion
        [Derived]
        [Required]

        public DateTime CreationDate { get; set; }

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

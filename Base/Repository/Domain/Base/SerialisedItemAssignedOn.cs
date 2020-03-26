// <copyright file="SerialisedItemAssignedOn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("d9a4d99d-0c06-4b7c-b69d-ab8b2833a017")]
    #endregion
    public partial class SerialisedItemAssignedOn: UniquelyIdentifiable
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("dc6f2c40-25ab-4e06-906c-62cb1d118e7f")]
        [AssociationId("4c41c694-11c0-4ead-a475-8a90b18c6a07")]
        [RoleId("249e9c5f-98a3-445f-a52b-21a23691218e")]
        #endregion
        [Workspace]
        [Indexed]
        [Size(256)]
        public string Name { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}

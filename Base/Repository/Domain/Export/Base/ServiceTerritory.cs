// <copyright file="ServiceTerritory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("987f8328-2bfa-47cd-9521-8b7bda78f90a")]
    #endregion
    public partial class ServiceTerritory : GeographicBoundaryComposite, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public GeographicBoundary[] Associations { get; set; }

        public string Abbreviation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region Allors
        [Id("a268313d-db1e-44e1-9fb1-7135d1157083")]
        [AssociationId("348c497e-7907-4409-b7b1-d77ebfd46258")]
        [RoleId("a23c1a3d-2a76-46b3-a26c-d5c5a66ebe0a")]
        #endregion
        [Required]
        [Size(256)]

        public string Name { get; set; }

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

// <copyright file="SalesTerritory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("62ea5285-b9d8-4a41-9c14-79c712fd3bf4")]
    #endregion
    public partial class SalesTerritory : GeographicBoundaryComposite, Object
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
        [Id("d904af24-887c-40b0-a5d0-7dce40ec4db3")]
        [AssociationId("0e172e31-8896-42c9-b1f2-2ff8bc1065c1")]
        [RoleId("bcf4d240-258b-43f3-ac94-4314685019ea")]
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

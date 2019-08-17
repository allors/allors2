// <copyright file="Territory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("7118e029-a8b3-415b-b9e9-d48ba4ea2823")]
    #endregion
    public partial class Territory : CityBound, GeographicBoundary, CountryBound, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public City[] Cities { get; set; }

        public string Abbreviation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid UniqueId { get; set; }

        public Country Country { get; set; }

        #endregion

        #region Allors
        [Id("9e3780d3-887f-458c-937c-379b22205e2f")]
        [AssociationId("241f1107-e802-4c2f-b0e5-80f42b3f916b")]
        [RoleId("3b19bc32-8d8e-404c-80c5-9671408a630e")]
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

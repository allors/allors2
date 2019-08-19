// <copyright file="State.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c37f7876-51af-4748-b083-4a6e42e99597")]
    #endregion
    public partial class State : CityBound, GeographicBoundary, CountryBound, Object
    {
        #region inherited properties
        public City[] Cities { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Abbreviation { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Guid UniqueId { get; set; }

        public Country Country { get; set; }

        #endregion

        #region Allors
        [Id("35ee6ba1-e75f-43f4-b33e-593748b5e359")]
        [AssociationId("040f516a-f173-44ba-b12c-a768e3216aec")]
        [RoleId("250129ac-caf9-486a-ae89-f47634738376")]
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

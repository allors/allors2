// <copyright file="EstimatedMaterialCost.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("cb6a8e8a-04a6-437b-b952-f502cca2a2db")]
    #endregion
    public partial class EstimatedMaterialCost : EstimatedProductCost
    {
        #region inherited properties
        public decimal Cost { get; set; }

        public Currency Currency { get; set; }

        public Organisation Organisation { get; set; }

        public string Description { get; set; }

        public GeographicBoundary GeographicBoundary { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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

        public void Delete() { }

        #endregion
    }
}

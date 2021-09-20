// <copyright file="EstimatedLaborCost.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("2a84fcce-91f6-4d8b-9840-2ddd5f4b3dac")]
    #endregion
    public partial class EstimatedLaborCost : EstimatedProductCost
    {
        #region inherited properties
        public decimal Cost { get; set; }

        public Currency Currency { get; set; }

        public Organisation Organisation { get; set; }

        public string Description { get; set; }

        public GeographicBoundary GeographicBoundary { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Restriction[] Restrictions { get; set; }

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

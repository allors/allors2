// <copyright file="SoftwareFeature.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("34047b37-545d-420f-ae79-2e05123cd623")]
    #endregion
    public partial class SoftwareFeature : ProductFeature, Enumeration
    {
        #region inherited properties
        public EstimatedProductCost[] EstimatedProductCosts { get; set; }

        public PriceComponent[] BasePrices { get; set; }

        public string Description { get; set; }

        public ProductFeature[] DependentFeatures { get; set; }

        public ProductFeature[] IncompatibleFeatures { get; set; }

        public VatRegime VatRegime { get; set; }

        public Guid UniqueId { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

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

        #endregion

        #region Allors
        [Id("34B67C9D-232D-4711-944C-50FFC7353816")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}

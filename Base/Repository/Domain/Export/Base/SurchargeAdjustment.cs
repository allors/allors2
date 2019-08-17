// <copyright file="SurchargeAdjustment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{

    using Allors.Repository.Attributes;

    #region Allors
    [Id("70468d86-b8a0-4aff-881e-fca2386f64da")]
    #endregion
    public partial class SurchargeAdjustment : OrderAdjustment
    {
        #region inherited properties
        public decimal Amount { get; set; }

        public VatRate VatRate { get; set; }

        public decimal Percentage { get; set; }

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

        public void Delete()
        {
        }

        #endregion
    }
}

// <copyright file="InvoiceVatRateItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("6e380347-21e3-4a00-819f-ed11e6882d03")]
    #endregion
    public partial class InvoiceVatRateItem : Deletable, Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("55659771-7862-4fb1-b30c-92a867a6c051")]
        [AssociationId("de919633-8651-479c-b3dc-5f510a6d2c4a")]
        [RoleId("d000219c-6ff5-42d2-a65f-da0a7897a00a")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal BaseAmount { get; set; }

        #region Allors
        [Id("d3300a5d-8e35-4106-9df9-d1bb25bb0352")]
        [AssociationId("14cc26ba-1c75-42dd-8a72-1f20f8692cb7")]
        [RoleId("f83c9130-b540-4b25-b7cf-699333395a9d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public VatRate[] VatRates { get; set; }

        #region Allors
        [Id("d7f6ed3a-cd81-418c-81c9-2bba827fe956")]
        [AssociationId("644237c5-167a-49f8-887f-5d10a725fd80")]
        [RoleId("d5cd524d-b5ba-4d32-b9ab-03aa08e202b9")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal VatAmount { get; set; }

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

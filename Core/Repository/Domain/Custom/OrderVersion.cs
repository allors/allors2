// <copyright file="OrderVersion.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region
    [Id("6A3A9167-9A77-491E-A1C8-CCFE4572AFB4")]
    #endregion
    public partial class OrderVersion : Version
    {
        #region inherited properties

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("88BE9AFA-122A-469B-BD47-388ECC835EAB")]
        [AssociationId("D8E59DF6-DC0C-4CAE-B0F2-402B2D927C5F")]
        [RoleId("8CD4939F-285F-4983-9C31-2AEB2B89D732")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public OrderState OrderState { get; set; }

        #region Allors
        [Id("F144557C-B63C-49F7-B713-F2493BCA1E55")]
        [AssociationId("E3CE3FA0-A40B-424D-907C-95907563EBD2")]
        [RoleId("CAC9EEFE-8E17-4D9A-BF50-298B610D514C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        public OrderLine[] OrderLines { get; set; }

        #region Allors
        [Id("49D672D8-3B3D-473A-B050-411251AE5365")]
        [AssociationId("723738EA-C2A9-4312-B9CF-A855FE71B836")]
        [RoleId("6D0FF30F-FD3E-4AB5-B6D2-C7C6E1D4346E")]
        #endregion
        [Derived]
        public decimal Amount { get; set; }

        #region inherited methods

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnInit()
        {
        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        #endregion
    }
}

// <copyright file="Bank.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    [Id("51d4dbfb-98ef-4f38-836a-5948701c4cce")]
    [Synced]
    public partial class ExchangeRate : Object, Deletable
    {
        #region inherited properties

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("d7beea67-7239-4ad8-a31d-c0850ed00b00")]
        [AssociationId("f1e6f80c-9cb6-4002-b8cb-3912aec92734")]
        [RoleId("a5d644b0-c94a-40a8-83e5-83bfceb2963d")]
        #endregion
        [Required]
        [Workspace]
        public DateTime ValidFrom { get; set; }

        #region Allors
        [Id("c73191fb-5814-4d14-8725-65be2ff90f77")]
        [AssociationId("5e2ccb12-dbe2-4a4e-be6b-0e5ae60cff2c")]
        [RoleId("a07b0ee3-9a1b-4855-8dd6-d2c039593cb6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public Currency FromCurrency { get; set; }

        #region Allors
        [Id("5d5d8bca-891e-4630-ac4d-748bfa323319")]
        [AssociationId("a353df10-519f-4b91-808f-be865c3a105f")]
        [RoleId("c4a01930-6082-4c23-b90a-9acb98590eff")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public Currency ToCurrency { get; set; }

        #region Allors
        [Id("dad55248-a724-4be0-891a-51aec803f2d8")]
        [AssociationId("bfd72241-4b97-425a-8fad-17cd7594771c")]
        [RoleId("d96cf8e0-9878-42de-89c1-253e81e408f9")]
        #endregion
        [Required]
        [Workspace]
        [Precision(28)]
        [Scale(10)]
        public decimal Rate { get; set; }

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

        public void Delete()
        {
        }

        #endregion
    }
}

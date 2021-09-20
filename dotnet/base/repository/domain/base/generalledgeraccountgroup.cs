// <copyright file="GeneralLedgerAccountGroup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("4a600c96-b813-46fc-8674-06bd3f85eae4")]
    #endregion
    public partial class GeneralLedgerAccountGroup : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("3ab2ad60-3560-4817-9862-7f60c55bbc32")]
        [AssociationId("5ab6a428-e5e3-4265-8263-0e4ead0cb5f5")]
        [RoleId("b8f88fa3-9f8e-4e2c-be79-df02a37cfa40")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccountGroup Parent { get; set; }

        #region Allors
        [Id("a48c3601-3d4c-43af-9502-d6beda764118")]
        [AssociationId("04b08f63-a2ac-43c2-889d-dbc8ebe86483")]
        [RoleId("7bd5e5e8-8605-46b2-b174-f345feb60f31")]
        #endregion
        [Required]
        [Size(-1)]
        [Workspace]
        public string Description { get; set; }

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

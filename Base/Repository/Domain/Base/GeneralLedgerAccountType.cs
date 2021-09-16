// <copyright file="GeneralLedgerAccountType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("ce5c78ee-f892-4ced-9b21-51d84c77127f")]
    #endregion
    public partial class GeneralLedgerAccountType : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("e01a0752-531b-4ee3-a58e-711f377247e1")]
        [AssociationId("dcfb5761-0d99-4a8f-afc9-2c0e64cd1c68")]
        [RoleId("7d579eae-a239-4f55-9719-02f39dbc42d8")]
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

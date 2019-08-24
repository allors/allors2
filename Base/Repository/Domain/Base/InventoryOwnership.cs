// <copyright file="InventoryOwnership.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("5CDD205C-86AE-4E31-A5AB-15356664B6F2")]
    #endregion
    public partial class InventoryOwnership : PartyRelationship
    {
        #region inherited properties

        public Party[] Parties { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("487E1274-E506-4542-B702-601A3E5A06D1")]
        [AssociationId("2D138357-3B35-42DD-98A8-1CEF98C03989")]
        [RoleId("FFE4B4AD-A1AA-493D-BA04-D86AB209B9B1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InventoryItem InventoryItem { get; set; }

        #region Allors
        [Id("38281DF0-2B5C-4AC9-91F4-18D12103CD19")]
        [AssociationId("701E1D19-BE14-47FA-8C4B-73C39C68AFF4")]
        [RoleId("CED69B79-2020-4D04-8F20-304C1C5B8F53")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public Party Owner { get; set; }

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

// <copyright file="SyncRoot.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("2A863DCF-C6FE-4838-8D3A-1212A2076D70")]
    #endregion
    public partial class SyncRoot : Object, DerivationCounted
    {
        #region inherited properties
        public int DerivationCount { get; set; }

        #endregion

        #region inherited methods

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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
        [Id("615C6C58-513A-456F-A0CE-E472D173DCB0")]
        [AssociationId("089D3E5D-6A3C-4B94-9162-65DEE526AA1F")]
        [RoleId("D1C3F53B-9E51-4574-A180-B3A927E41A0B")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        #endregion
        [Synced]
        public SyncDepthI1 SyncDepth1 { get; set; }

        #region Allors
        [Id("4061BB19-494D-4CD4-AE7F-798FC62942AB")]
        [AssociationId("ACA012C8-7EC4-4D9C-8131-A62232D275DC")]
        [RoleId("9C5A7B7A-7E0E-4D84-8791-160E9DFFF7E6")]
        #endregion
        [Required]
        public int Value { get; set; }
    }
}

// <copyright file="Login.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("F4FD1CB2-9E98-4F58-AAD4-2388916C2E56")]
    #endregion
    public partial class IdentityClaim : Deletable
    {
        #region inherited properties
        #endregion

        #region Allors
        [Id("7B1AD6D4-EF6F-40FE-90BD-89DE8A997388")]
        [AssociationId("8C60B251-5911-4962-8D2B-C0D843CDDADF")]
        [RoleId("7C4250A9-630C-41C6-AE9D-1AEE80EF6A71")]
        #endregion
        [Indexed]
        [Size(256)]
        public string Type { get; set; }

        #region Allors
        [Id("34793275-CD8F-4A4E-9E79-B82F3869ACCF")]
        [AssociationId("2795D949-9BD6-4C20-AD1F-F9845829F102")]
        [RoleId("FC2455AC-F776-49C6-B614-BC171F0D59D7")]
        #endregion
        [Indexed]
        [Size(256)]
        public string Value { get; set; }

        #region Allors
        [Id("96792928-C286-4F47-B4BE-2E92C5E3E993")]
        [AssociationId("D8256032-AE24-4F88-A37A-A12A13A45A63")]
        [RoleId("DD349DFB-B103-40A9-906E-61BE3D8F922E")]
        #endregion
        [Indexed]
        [Size(256)]
        public string ValueType { get; set; }

        #region Allors
        [Id("505088BD-A9A9-433A-9585-2FB08EBA212B")]
        [AssociationId("714D4CFB-C263-4B39-A5C7-819C891A2EF8")]
        [RoleId("F7D5741D-40CB-4E4A-82D9-71645D3E0E5C")]
        #endregion
        [Indexed]
        [Size(256)]
        public string Issuer { get; set; }

        #region Allors
        [Id("94E59DDC-62DD-4EBB-97C0-D5D441C28CE0")]
        [AssociationId("0CC942BC-D061-4358-9A30-A9ACB72E7F93")]
        [RoleId("84388FFF-2CCC-4A05-BD4F-B0477FBEB7E4")]
        #endregion
        [Indexed]
        [Size(256)]
        public string OriginalIssuer { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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

//-------------------------------------------------------------------------------------------------
// <copyright file="Media.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("da5b86a3-4f33-4c0d-965d-f4fbc1179374")]
    #endregion
    public partial class Media : UniquelyIdentifiable, Deletable, Object
    {
        #region inherited properties
        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("B74C2159-739A-4F1C-ADA7-C2DCC3CDCF83")]
        [AssociationId("96B21673-F124-4C30-A2F0-DF56D29E03F5")]
        [RoleId("DE0FE224-C40D-469C-BDC5-849A7412EFEC")]
        #endregion
        [Indexed]
        [Derived]
        [Workspace]
        public Guid Revision { get; set; }

        #region Allors
        [Id("67082a51-1502-490b-b8db-537799e550bd")]
        [AssociationId("e8537dcf-1bd7-46c4-a37c-077bee6a78a1")]
        [RoleId("02fe1ce8-c019-4a40-bd6f-b38d2f47a288")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public MediaContent MediaContent { get; set; }

        #region Allors
        [Id("18236718-1835-430C-A936-7EC461EEE2CF")]
        [AssociationId("8A79E6C5-4BAE-468D-B57C-C7788D3E21E3")]
        [RoleId("877ABDC8-8915-4640-8871-8CEF7EF69072")]
        #endregion
        [Size(-1)]
        [Workspace]
        public byte[] InData { get; set; }

        #region Allors
        [Id("79B04065-F13B-43B3-B86E-F3ADBBAAF0C4")]
        [AssociationId("287B7291-39F0-43E5-8770-811940E81BAE")]
        [RoleId("CE17BFC7-5A4E-415A-9AE0-FAE429CEE69C")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string InDataUri { get; set; }

        #region Allors
        [Id("DDD6C005-0104-44CA-A19C-1150B8BEB4A3")]
        [AssociationId("4F43B520-404E-436D-A514-71E4AEC55EC8")]
        [RoleId("4C4EC21C-A3C0-4720-92E0-CF6532000265")]
        #endregion
        [Indexed]
        [Size(256)]
        [Workspace]
        public string FileName { get; set; }

        #region Allors
        [Id("29541613-0B16-49AD-8F40-3309A7C7D7B8")]
        [AssociationId("EFB76140-4A2A-4E7F-B51D-C95BCA774664")]
        [RoleId("7CFC8B40-5199-4457-BBBD-27A786721465")]
        #endregion
        [Indexed]
        [Size(1024)]
        [Workspace]
        [Derived]
        public string Type { get; set; }

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

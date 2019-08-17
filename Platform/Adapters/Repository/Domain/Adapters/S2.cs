// <copyright file="S2.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("feeb7027-7c6c-4cb5-8718-93e6e8a4afd8")]
    #endregion
    public partial interface S2 : Object
    {
        #region Allors
        [Id("1c758737-140a-49f0-badc-29658b4bc55f")]
        [AssociationId("66f34737-9d65-4c20-a3d3-85a6b8c00891")]
        [RoleId("8fedfe1b-10d0-4be8-ae87-177b77b8d36f")]
        [Size(256)]
        #endregion
        string S2AllorsString { get; set; }

        #region Allors
        [Id("1f5a6afe-f458-43db-bea0-8c90074b5abf")]
        [AssociationId("3370eb80-a77b-448b-9653-d9de382481c3")]
        [RoleId("a4da55d6-e7ac-4889-8479-0bd1a41c6817")]
        #endregion
        int S2AllorsInteger { get; set; }

        #region Allors
        [Id("74dd2b7b-e647-4967-9838-46c701baf3a7")]
        [AssociationId("23e975f9-7eff-4dd0-8ecf-317fb59b6c6a")]
        [RoleId("9d0f4a5c-a8c7-41a7-9d92-0a9b4ff788ce")]
        #endregion
        double S2AllorsDouble { get; set; }

        #region Allors
        [Id("9a191c76-bd05-498f-91da-33184c72fe90")]
        [AssociationId("6dadca9c-e6b0-42be-b29d-ba40b19f4e4a")]
        [RoleId("bd759535-1681-455f-b140-2dcfea268b0a")]
        #endregion
        bool S2AllorsBoolean { get; set; }

        #region Allors
        [Id("9d70a5f5-ed72-4ba3-98ac-e50752f8fb79")]
        [AssociationId("329528a6-1712-4112-9797-761338248769")]
        [RoleId("f155d4f3-bcda-47a2-8be2-ba24ba6648e5")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        decimal S2AllorsDecimal { get; set; }

        #region Allors
        [Id("a305d91a-5fe1-467d-9f24-6cce5dd30b1d")]
        [AssociationId("a50b4bfa-3a0e-411c-b9f5-2af203b58668")]
        [RoleId("51f99d5f-bccf-4252-9475-dcf724e775d9")]
        #endregion
        DateTime S2AllorsDateTime { get; set; }
    }
}

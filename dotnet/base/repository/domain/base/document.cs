// <copyright file="Document.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("1d21adf0-6008-459d-9f6a-3a026e7640bc")]
    #endregion
    public partial interface Document : Printable, Commentable, Object
    {
        #region Allors
        [Id("484d082e-b3e4-4915-a355-43315f466e6d")]
        [AssociationId("47509fbb-ba47-4ca7-8689-51b9c5b46746")]
        [RoleId("b8a938cf-302c-4aa0-8e1c-e23752dee601")]
        #endregion
        [Required]
        [Size(256)]

        string Name { get; set; }

        #region Allors
        [Id("6f6ef875-2b0b-4a03-8b2e-bf242b48c843")]
        [AssociationId("de6d7bb7-0e38-4ed0-b881-8c1cf99dc101")]
        [RoleId("0d58ab92-ffed-4c8b-942e-f9b1780d150f")]
        #endregion
        [Size(-1)]

        string Description { get; set; }

        #region Allors
        [Id("d579e6e7-6791-4b9b-bf20-43ab1a701866")]
        [AssociationId("771bee4a-2e75-4826-8b15-43bfa140830b")]
        [RoleId("feeb5f20-6b25-40df-b2e0-d6c21753ea8a")]
        #endregion
        [Size(-1)]

        string Text { get; set; }

        #region Allors
        [Id("e97710fe-8def-44a8-8516-18d4eae8433b")]
        [AssociationId("e73dc13b-9f24-4f09-8039-52fbddb54664")]
        [RoleId("9388eba9-d3b7-4c8a-8e03-d8d2d31b476b")]
        #endregion
        [Size(256)]

        string DocumentLocation { get; set; }
    }
}

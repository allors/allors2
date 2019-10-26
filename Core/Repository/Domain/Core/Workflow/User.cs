// <copyright file="User.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("a0309c3b-6f80-4777-983e-6e69800df5be")]
    #endregion
    public partial interface User : UniquelyIdentifiable, SecurityTokenOwner
    {
        #region Allors
        [Id("5e8ab257-1a1c-4448-aacc-71dbaaba525b")]
        [AssociationId("eca7ef36-8928-4116-bfce-1896a685fe8c")]
        [RoleId("3b7d40a0-18ea-4018-b797-6417723e1890")]
        #endregion
        [Size(256)]
        [Required]
        [Workspace]
        string UserName { get; set; }

        #region Allors
        [Id("7397B596-D8FA-4E3C-8E0E-EA24790FE2E4")]
        [AssociationId("19CAD82C-6538-4C46-AA3F-75C082CC8204")]
        [RoleId("FAF89920-880F-4600-BAF1-A27A5268444A")]
        #endregion
        [Size(256)]
        [Derived]
        [Unique]
        string NormalizedUserName { get; set; }

        #region Allors
        [Id("ea0c7596-c0b8-4984-bc25-cb4b4857954e")]
        [AssociationId("8537ddb5-8ce2-4f35-a16f-207f2519ba9c")]
        [RoleId("75ee3ec2-02bb-4666-a6f0-bac84c844dfa")]
        #endregion
        [Size(256)]
        string UserPasswordHash { get; set; }

        #region Allors
        [Id("c1ae3652-5854-4b68-9890-a954067767fc")]
        [AssociationId("111104a2-1181-4958-92f6-6528cef79af7")]
        [RoleId("58e35754-91a9-4956-aa66-ca48d05c7042")]
        #endregion
        [Size(256)]
        [Workspace]
        string UserEmail { get; set; }

        #region Allors
        [Id("24977764-63D7-4B2B-9FD7-19D0A6F8CCDB")]
        [AssociationId("D55D24DF-E19A-4856-B239-F8CAA6586727")]
        [RoleId("F13047E9-4558-4EE4-9D38-28052FAA5DD4")]
        #endregion
        [Size(256)]
        [Derived]
        [Unique]
        string NormalizedUserEmail { get; set; }

        #region Allors
        [Id("0b3b650b-fcd4-4475-b5c4-e2ee4f39b0be")]
        [AssociationId("c89a8e3f-6f76-41ac-b4dc-839f9080d917")]
        [RoleId("1b1409b8-add7-494c-a895-002fc969ac7b")]
        #endregion
        [Required]
        bool UserEmailConfirmed { get; set; }

        #region Allors
        [Id("7792EAEE-6DC3-4E47-B5CB-0B6A08ED451F")]
        [AssociationId("54FEB59C-2FA1-411F-91F0-E7149B91D1A8")]
        [RoleId("4A0170A0-25CC-4F51-95E2-9F7359EFAD54")]
        #endregion
        [Size(256)]
        string SecurityStamp { get; set; }

        #region Allors
        [Id("1C53F0D3-FA27-4018-ACBE-88A1E5F5C386")]
        [AssociationId("926A543B-077D-439B-A9A8-485DC75B81D6")]
        [RoleId("8FCE5B0D-6EC5-48E3-8319-831B5A1C7B45")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        IdentityClaim[] IdentityClaims { get; set; }

        #region Allors
        [Id("4C9FDD8A-D7D4-4F6C-9584-77C6E1FC90FD")]
        [AssociationId("2941C736-A180-4A7C-9222-37C5037DE52E")]
        [RoleId("DC449724-23EE-4594-A1D0-9ABFF752058D")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        Login[] Logins { get; set; }

        #region Allors
        [Id("bed34563-4ed8-4c6b-88d2-b4199e521d74")]
        [AssociationId("e678c2f8-5c66-4886-ad21-2be98101f759")]
        [RoleId("79e9a907-9237-4aab-9340-277d593748f5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        NotificationList NotificationList { get; set; }
    }
}

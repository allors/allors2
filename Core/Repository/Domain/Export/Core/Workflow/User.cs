//------------------------------------------------------------------------------------------------- 
// <copyright file="User.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the Extent type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("a0309c3b-6f80-4777-983e-6e69800df5be")]
    #endregion
    public partial interface User : SecurityTokenOwner, Object
    {
        #region Allors
        [Id("5e8ab257-1a1c-4448-aacc-71dbaaba525b")]
        [AssociationId("eca7ef36-8928-4116-bfce-1896a685fe8c")]
        [RoleId("3b7d40a0-18ea-4018-b797-6417723e1890")]
        [Size(256)]
        #endregion
        [Workspace]
        string UserName { get; set; }

        #region Allors
        [Id("7397B596-D8FA-4E3C-8E0E-EA24790FE2E4")]
        [AssociationId("19CAD82C-6538-4C46-AA3F-75C082CC8204")]
        [RoleId("FAF89920-880F-4600-BAF1-A27A5268444A")]
        [Size(256)]
        #endregion
        [Workspace]
        string NormalizedUserName { get; set; }

        #region Allors
        [Id("ea0c7596-c0b8-4984-bc25-cb4b4857954e")]
        [AssociationId("8537ddb5-8ce2-4f35-a16f-207f2519ba9c")]
        [RoleId("75ee3ec2-02bb-4666-a6f0-bac84c844dfa")]
        [Size(256)]
        #endregion
        string UserPasswordHash { get; set; }

        #region Allors
        [Id("c1ae3652-5854-4b68-9890-a954067767fc")]
        [AssociationId("111104a2-1181-4958-92f6-6528cef79af7")]
        [RoleId("58e35754-91a9-4956-aa66-ca48d05c7042")]
        [Size(256)]
        #endregion
        [Workspace]
        string UserEmail { get; set; }

        #region Allors
        [Id("0b3b650b-fcd4-4475-b5c4-e2ee4f39b0be")]
        [AssociationId("c89a8e3f-6f76-41ac-b4dc-839f9080d917")]
        [RoleId("1b1409b8-add7-494c-a895-002fc969ac7b")]
        #endregion
        [Workspace]
        bool UserEmailConfirmed { get; set; }

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
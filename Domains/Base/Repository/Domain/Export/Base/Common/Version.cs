//------------------------------------------------------------------------------------------------- 
// <copyright file="Version.cs" company="Allors bvba">
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
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("A6A3C79E-150B-4586-96EA-5AC0E2E638C6")]
    #endregion
    public partial interface Version : AccessControlledObject
    {
        #region Allors
        [Id("9FAEB940-A3A0-4E7A-B889-BCFD92F6A882")]
        [AssociationId("4C4BD3D4-6642-48AA-8C29-46C02DCDC749")]
        [RoleId("FD06C364-1033-423C-B297-DC6EDF15F4FD")]
        #endregion
        Guid DerivationId { get; set; }

        #region Allors
        [Id("ADF611C3-047A-4BAE-95E3-776022D5CE7B")]
        [AssociationId("7145B062-AEE9-4B30-ADB8-C691969C6874")]
        [RoleId("B38C700C-7AD9-4962-9F53-35B8AEF22E09")]
        #endregion
        [Workspace]
        DateTime DerivationTimeStamp { get; set; }

        #region Allors
        [Id("561C7A91-5232-453F-BA26-9B84D871ECC9")]
        [AssociationId("DD8E700B-1E1D-4A69-8262-F38C971B730D")]
        [RoleId("F19CFCEA-5D55-4F22-9F46-820C2F63A9B4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        User LastModifiedBy { get; set; }
    }
}
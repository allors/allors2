// <copyright file="Version.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("A6A3C79E-150B-4586-96EA-5AC0E2E638C6")]
    #endregion
    public partial interface Version : Object
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
    }
}

//-------------------------------------------------------------------------------------------------
// <copyright file="Version.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    public partial interface Version
    {
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

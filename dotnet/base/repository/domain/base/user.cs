// <copyright file="User.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Attributes;

    public partial interface User : Localised
    {
        #region Allors
        [Id("372F197C-3B6E-4C12-9BD9-D50A42B99C80")]
        [AssociationId("2E7767FB-518C-4500-9D99-DC77227147F3")]
        [RoleId("41F1DDB5-6CE5-47C8-8321-16D9529BAE4F")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        UserProfile UserProfile { get; set; }
    }
}

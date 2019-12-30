// <copyright file="Searchable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("B34B917C-EB2D-49FE-B45E-C7C6F7FE5A6A")]
    #endregion
    public partial interface Searchable
    {
        #region Allors
        [Id("CD884022-9A53-4E59-A466-FB76946CF3C6")]
        [AssociationId("CD8AEEE3-1723-4339-8F6B-1FD6A1B83F82")]
        [RoleId("7DEAC0DA-A10D-4B3A-8B28-E37FE3A105C3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Size(-1)]
        [Workspace]
        string SearchOptions{ get; set; }
    }
}

// <copyright file="UniquelyIdentifiable.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("122ccfe1-f902-44c1-9d6c-6f6a0afa9469")]
    #endregion
    public partial interface UniquelyIdentifiable : Object
    {
        #region Allors
        [Id("e1842d87-8157-40e7-b06e-4375f311f2c3")]
        [AssociationId("fe413e96-cfcf-4e8d-9f23-0fa4f457fdf1")]
        [RoleId("d73fd9a4-13ee-4fa9-8925-d93eca328bf6")]
        #endregion
        [Workspace]
        [Indexed]
        [Required]
        Guid UniqueId { get; set; }
    }
}

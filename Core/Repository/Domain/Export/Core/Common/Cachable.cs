// <copyright file="Cachable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("B17AFC19-9E91-4631-B6D8-43B32A65E0A0")]
    #endregion
    public partial interface Cachable : Object
    {
        #region Allors
        [Id("EF6F1F4C-5B62-49DC-9D05-0F02973ACCB3")]
        [AssociationId("1137FDD3-07E6-432E-8C42-273EF24863D5")]
        [RoleId("D6A473F7-4EFF-4D3D-BDB2-59F5EE8B0E52")]
        #endregion
        [Required]
        Guid CacheId { get; set; }
    }
}

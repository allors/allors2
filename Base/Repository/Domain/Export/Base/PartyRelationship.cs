// <copyright file="PartyRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("084abb92-31fd-46e6-ab85-9a7a88c9d72b")]
    #endregion
    public partial interface PartyRelationship : Period, Deletable, Object
    {
        #region Allors
        [Id("8472a037-3a42-4d1c-a7cb-f8866141f65d")]
        [AssociationId("48ebe634-9189-4d7c-b796-b6db43d33063")]
        [RoleId("9377f1f2-581a-44ae-94c6-e2f8dbccffd0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] Parties { get; set; }
    }
}

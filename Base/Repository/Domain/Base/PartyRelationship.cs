// <copyright file="PartyRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

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

        #region Allors
        [Id("6dfbbbe2-31a1-4182-8329-0ba7989f5a71")]
        [AssociationId("9dd7c324-9566-4ab5-bdf3-a617010a8fb8")]
        [RoleId("94e12c41-318d-4b17-90c3-2577dfc0081a")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        Agreement[] Agreements { get; set; }
    }
}

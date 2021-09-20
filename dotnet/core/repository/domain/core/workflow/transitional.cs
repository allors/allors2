// <copyright file="Transitional.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("ab2179ad-9eac-4b61-8d84-81cd777c4926")]
    #endregion
    public partial interface Transitional : Object
    {
        #region Allors
        [Id("D9D86241-5FC7-4EDB-9FAA-FF5CA291F16C")]
        [AssociationId("C6D64EB2-4921-4AD9-9DC3-12BDCB8E7D97")]
        [RoleId("292A7D78-3DA8-401C-A4D1-513F61114615")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        ObjectState[] PreviousObjectStates { get; set; }

        #region Allors
        [Id("2BC8AFDF-92BE-4088-9E35-C1C942CFE74B")]
        [AssociationId("549BC4A5-42B5-46D8-B487-9D1255BC1B8E")]
        [RoleId("CA573AAD-72CC-4315-971D-43526D1A964B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        ObjectState[] LastObjectStates { get; set; }

        #region Allors
        [Id("52962C45-8A3E-4136-A968-C333CBE12685")]
        [AssociationId("B49A45EE-302E-4893-BEAD-88764D0774FF")]
        [RoleId("08BBEF2B-47A4-48B0-86E2-522F3B584426")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        ObjectState[] ObjectStates { get; set; }
    }
}

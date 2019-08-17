// <copyright file="Localised.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("7979a17c-0829-46df-a0d4-1b01775cfaac")]
    #endregion
    public partial interface Localised : Object
    {
        #region Allors
        [Id("8c005a4e-5ffe-45fd-b279-778e274f4d83")]
        [AssociationId("6684d98b-cd43-4612-bf9d-afefe02a0d43")]
        [RoleId("d43b92ac-9e6f-4238-9625-1e889be054cf")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Locale Locale { get; set; }
    }
}

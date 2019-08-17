// <copyright file="CountryBound.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{

    using Allors.Repository.Attributes;

    #region Allors
    [Id("eaebcfe7-0d65-43ab-857c-b171086a1982")]
    #endregion
    public partial interface CountryBound : Object
    {

        #region Allors
        [Id("095460a7-fffa-4c94-8b51-a4fd9fb80a2e")]
        [AssociationId("f5aa22da-64f3-447a-864c-4db5b77d221b")]
        [RoleId("799ab886-ce30-4270-8293-6c302d17e3e3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        Country Country { get; set; }

    }
}

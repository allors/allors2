// <copyright file="ExternalAccountingTransaction.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6bfa631c-80f4-495f-bb9a-0d3351390d64")]
    #endregion
    public partial interface ExternalAccountingTransaction : AccountingTransaction
    {

        #region Allors
        [Id("327fc2cb-9589-4e9d-b9e6-7429cbe14746")]
        [AssociationId("5fdf05a4-933c-42d9-897c-b68c6671f785")]
        [RoleId("df92000b-768e-41db-addc-1e2ca5c8baee")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        Party FromParty { get; set; }

        #region Allors
        [Id("681312d3-63cd-45a2-883c-4a907d379f52")]
        [AssociationId("bfe64bcc-8832-4d02-92cb-7f4b0681fc81")]
        [RoleId("2359aaec-7150-4a84-82af-7dc4ef677c9b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        Party ToParty { get; set; }

    }
}

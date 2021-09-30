// <copyright file="Country.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    public partial class Country : GeographicBoundary, CityBound
    {
        #region inherited properties

        public string Abbreviation { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Guid UniqueId { get; set; }

        public City[] Cities { get; set; }

        #endregion

        #region Allors
        [Id("2ecb8cfb-011d-4c31-a9cd-ed5a13ae23a4")]
        [AssociationId("ebdfd8e3-9d24-4721-b72b-5a5e4327d62b")]
        [RoleId("45aa4f50-a23b-4ce6-872f-d72b648e4e90")]
        #endregion
        [Workspace]
        public int IbanLength { get; set; }

        #region Allors
        [Id("6553ee71-66dd-45f2-9de9-5656b011d2fc")]
        [AssociationId("0a5662c3-1f60-41d5-a703-638480cb3c15")]
        [RoleId("a14f5154-bcf2-44f4-a49e-3c17aca71247")]
        #endregion
        [Workspace]
        public bool EuMemberState { get; set; }

        #region Allors
        [Id("7f0adb03-db73-44f2-a4a2-ece00f4908a2")]
        [AssociationId("081e6909-c744-4795-b587-82bbf938b5fe")]
        [RoleId("38546e92-a238-4d72-a731-a3f91dbcc61f")]
        #endregion
        [Size(256)]
        [Workspace]
        public string TelephoneCode { get; set; }

        #region Allors
        [Id("a2aa65d7-e0ef-4f6f-a194-9aeb49a1d898")]
        [AssociationId("86d7d9a6-77fd-491b-b563-86b8d0c76ee4")]
        [RoleId("4f6f041b-a1ea-47bc-92e4-650bddaa46ed")]
        #endregion
        [Size(256)]
        [Workspace]
        public string IbanRegex { get; set; }

        #region Allors
        [Id("b829da1c-2eb7-495b-a4a9-98e335cd87f9")]
        [AssociationId("a0377434-67ae-4ab4-90b3-99fb6bc2bf90")]
        [RoleId("8a306049-a4b9-4489-a2b8-d627fa6444c3")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public VatForm VatForm { get; set; }

        #region Allors
        [Id("bc0bf6fb-d7d0-410e-ab14-5bc0e33f491d")]
        [AssociationId("390fd915-4b64-4768-b8d1-93d3a16a09b5")]
        [RoleId("59cb3e5b-0c74-423d-87c9-8b7532c5af04")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Derived]
        [Workspace]
        public VatRegime[] DerivedVatRegimes{ get; set; }

        #region Allors
        [Id("c231ce68-bf03-4122-8699-c3c6473ab90a")]
        [AssociationId("153203db-be9a-4722-aab3-7163de779a2a")]
        [RoleId("e72228ee-ae28-406c-b7ee-a9be1a4d3286")]
        #endregion
        [Size(256)]
        [Workspace]
        public string UriExtension { get; set; }

        #region inherited methods

        #endregion
    }
}

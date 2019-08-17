// <copyright file="ShippingAndHandlingComponent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("1a174f59-c8cd-49ad-b0f4-a561cdcdcfb2")]
    #endregion
    public partial class ShippingAndHandlingComponent : Period, Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("0021e1ff-bfc3-4d0b-8168-a8f5789121f7")]
        [AssociationId("f1c6cb2b-7c7a-4ca5-b594-152238131cb2")]
        [RoleId("09d4c34a-b5b8-490c-85e3-00470bb8270e")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal Cost { get; set; }

        #region Allors
        [Id("4dfb4bda-1add-45d5-92c7-6393186301f0")]
        [AssociationId("44088ee8-b84a-494c-a59c-3164c511176c")]
        [RoleId("eac922da-3beb-41b2-a3ca-f91120f927bd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public ShipmentMethod ShipmentMethod { get; set; }

        #region Allors
        [Id("a029fb4c-4f80-4216-8fc9-9d9b44997816")]
        [AssociationId("9e7b4c12-5168-4fe3-adaf-f8d14f7be01f")]
        [RoleId("b0e26bbb-aef7-40ca-9a64-d78bc02affb9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Carrier Carrier { get; set; }

        #region Allors
        [Id("ab4377d4-69c6-4b0c-b9d4-e3a01c1a6a94")]
        [AssociationId("2ad57105-a5c4-4e7f-a6df-79af9cddf9ca")]
        [RoleId("742dcf46-5fa5-44b4-bf02-582681b0f6aa")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public ShipmentValue ShipmentValue { get; set; }

        #region Allors
        [Id("df4727ab-29a8-448c-97b4-c16033e03dcf")]
        [AssociationId("a57b1bd3-a060-41c1-91bd-ecc428dd9b55")]
        [RoleId("a554ced2-84e0-41bb-97d9-d0b04ef56679")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Currency Currency { get; set; }

        #region Allors
        [Id("f2bfd9d5-01b2-4bec-8dc2-018cc2187037")]
        [AssociationId("cf282301-2e6c-43ed-8447-cc09edcb9810")]
        [RoleId("a3ee85b7-be6a-4e2b-a15d-57872bb57783")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public GeographicBoundary GeographicBoundary { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

    }
}

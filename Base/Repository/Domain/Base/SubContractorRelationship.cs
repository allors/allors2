// <copyright file="SubContractorRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("d60cc44a-6491-4982-9b2d-99891e382a21")]
    #endregion
    public partial class SubContractorRelationship : PartyRelationship
    {
        #region inherited properties

        public Party[] Parties { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Agreement[] Agreements { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("567a8c58-2584-4dc7-96c8-13fea5b51cf9")]
        [AssociationId("f12711a8-11ce-4f9c-a75b-02594b476a9e")]
        [RoleId("8f21a29f-a0b0-412c-b7dd-e2fcdee561d6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InternalOrganisation Contractor { get; set; }

        #region Allors
        [Id("d95ecb34-dfe4-42df-bc9f-1ad4af72abaa")]
        [AssociationId("597810f4-da06-4d63-837e-6cd0419f3d4b")]
        [RoleId("6aca0d56-be58-4876-bfef-918430a119a7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Organisation SubContractor { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }
        #endregion

    }
}

// <copyright file="ProfessionalServicesRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a6f772e6-8f2c-4180-bbf9-2e5ab0f0efc8")]
    #endregion
    public partial class ProfessionalServicesRelationship : PartyRelationship
    {
        #region inherited properties
        public DateTime FromDate { get; set; }
        public DateTime ThroughDate { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public Party[] Parties { get; set; }
        #endregion

        #region Allors
        [Id("62edaaeb-bcef-4c3c-955a-30d708bc4a3c")]
        [AssociationId("af3829d6-137c-4453-b705-60b7dfa8c045")]
        [RoleId("29b1fec5-de9c-4fe2-bdfc-fc9d33ca90b5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Person Professional { get; set; }

        #region Allors
        [Id("a587695e-a9b3-4b5b-b211-a19096b88815")]
        [AssociationId("d3fc269c-debf-4ada-b1be-b2f48d2ae027")]
        [RoleId("c6b955f2-20ed-4164-8f11-2c5d24fa0443")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Organisation ProfessionalServicesProvider { get; set; }

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

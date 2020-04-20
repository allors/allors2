// <copyright file="PartSubstitute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("c0ea51d6-e9f1-4cb3-80ea-36d8ac4f8a15")]
    #endregion
    public partial class PartSubstitute : Commentable, Deletable
    {
        #region inherited properties
        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("23f8fda9-9109-4826-988f-74e115a430f4")]
        [AssociationId("9f43aa6a-68d0-44f9-aecb-977b6f0d66ea")]
        [RoleId("1a0b6784-dcbe-4f86-acb1-1a0408f00465")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Part SubstitutionPart { get; set; }

        #region Allors
        [Id("510f8f4c-ff09-4d32-8c1c-e905dbbd684b")]
        [AssociationId("25d0c7ec-767e-4509-9164-67dbec0d66f4")]
        [RoleId("d2c04285-03ab-4391-9338-d158630793b0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Ordinal Preference { get; set; }

        #region Allors
        [Id("9cd198eb-2c25-425e-a23b-c321938f2512")]
        [AssociationId("8f8c0254-8bb0-4e61-83b5-38b0b80d0b97")]
        [RoleId("6939df10-1c96-4a64-aae4-201392e9fd59")]
        #endregion

        public DateTime FromDate { get; set; }

        #region Allors
        [Id("ccb0a290-b3f4-4e55-b52c-67ca70d67439")]
        [AssociationId("0d0e7982-f7cb-4c6b-bff7-e59f81296d6b")]
        [RoleId("ab2549fe-dd9a-45dd-a9b0-7e3ff1f6a68f")]
        #endregion
        [Required]

        public int Quantity { get; set; }

        #region Allors
        [Id("e7d4ae25-175a-4e2a-88c2-9d8d5a468d1a")]
        [AssociationId("4986253b-2d85-45d1-8809-dcaab09e22f4")]
        [RoleId("b85ed877-cd74-40cd-b2cf-aee6b24c3eeb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Part Part { get; set; }

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

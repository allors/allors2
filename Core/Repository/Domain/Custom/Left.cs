// <copyright file="Left.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region

    [Id("DBAF849D-E8B0-4CEA-85E0-DFB934A06F96")]

    #endregion
    public partial class Left : DerivationCounted
    {
        #region inherited properties
        public int DerivationCount { get; set; }

        #endregion

        #region Allors
        [Id("C86BBB90-F678-4627-B651-657F86B2D2EB")]
        [AssociationId("8A14211A-5FAC-45DE-A628-31A7C1C024E1")]
        [RoleId("3C33B8D7-2BE6-4F96-A7BE-BB9294B00A1F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        public Middle Middle { get; set; }

        #region Allors
        [Id("92CF5496-063D-428E-9A24-F36321A10675")]
        [AssociationId("3D7965D5-E712-49F7-8176-C2CEAA8680FD")]
        [RoleId("AA375202-F5E3-444A-BC83-443D5D7C35A2")]
        #endregion
        [Required]
        public int Counter { get; set; }

        #region Allors
        [Id("8C454674-AE11-4305-A055-55A915139F16")]
        [AssociationId("A41CAE5F-4FAC-4B1C-A1C1-C992B82D1AC7")]
        [RoleId("B253A149-3FF5-43E7-AACB-3421FCBD1231")]
        #endregion
        [Required]
        public bool CreateMiddle { get; set; }

        #region inherited methods

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public void OnBuild()
        {
        }

        public void OnPostBuild()
        {
        }

        public void OnInit()
        {

        }

        public void OnPreDerive()
        {
        }

        public void OnDerive()
        {
        }

        public void OnPostDerive()
        {
        }

        #endregion
    }
}

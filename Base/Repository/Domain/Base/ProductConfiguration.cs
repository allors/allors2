// <copyright file="ProductConfiguration.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("23503dae-02ff-4dae-950e-d699dcb12a3c")]
    #endregion
    public partial class ProductConfiguration : ProductAssociation
    {
        #region inherited properties
        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("463f9523-62e0-4f33-a0fd-29b42f4af046")]
        [AssociationId("4c878c18-a3d4-4928-a675-4c0940f05c41")]
        [RoleId("acf16223-8095-4133-89e9-841e11447a63")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        public Product[] ProductsUsedIn { get; set; }

        #region Allors
        [Id("528afcdf-09c2-4b3a-89b0-4da8fd732e83")]
        [AssociationId("774872d1-6b99-4e5e-8f00-f791a11ea337")]
        [RoleId("b2fe3235-8886-4ed1-b8df-edb36e9c8e17")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Product Product { get; set; }

        #region Allors
        [Id("9e6d3782-2f32-4155-a1bf-62c02e8cbe82")]
        [AssociationId("aba19375-c6c7-4955-b764-bf731822b4f8")]
        [RoleId("53546606-dc80-4224-a6ce-45acff613fb9")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityUsed { get; set; }

        #region Allors
        [Id("caabfae5-6cff-41df-a267-9f4bde0b4808")]
        [AssociationId("d76092e1-40e4-45ab-ada5-cbe9206dcf84")]
        [RoleId("3afc48ad-7897-4741-9d32-78f857b414fb")]
        #endregion
        [Required]
        [Size(-1)]

        public string Description { get; set; }

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

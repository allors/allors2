// <copyright file="WorkEffortPartStandard.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("a12e5d28-e431-48d3-bbb1-8a2f5e3c4991")]
    #endregion
    public partial class WorkEffortPartStandard : Period,  Deletable
    {
        #region inherited properties
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("4d4913e2-649d-4589-86ee-93cfa6c426a7")]
        [AssociationId("9228803e-089c-4ee6-9a42-18503d12f663")]
        [RoleId("abb46361-be39-4668-8bbb-26de268a654c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Part Part { get; set; }

        #region Allors
        [Id("68d4af49-a55f-416c-8097-d93da90e1132")]
        [AssociationId("f7423733-f8ec-41f6-85a5-fd528d9291fc")]
        [RoleId("0748dd9e-6ea8-4eea-87f8-c40605e06d0c")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal EstimatedCost { get; set; }

        #region Allors
        [Id("ec3e9aee-c39b-46a1-9968-af914f9057f3")]
        [AssociationId("5e99179e-4abd-409b-b091-263037554a6a")]
        [RoleId("c63106ff-fe33-40fb-acb6-e7fb9907eb18")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Required]
        [Workspace]
        public decimal Quantity { get; set; }

        #region Allors
        [Id("8fc60462-287d-47df-a6d2-6ac857f2afbb")]
        [AssociationId("75ec4e48-d691-4673-814f-37b3cb74dd3d")]
        [RoleId("522b903b-9e48-4aa2-beef-da868b717448")]
        #endregion
        [Derived]
        [Size(-1)]
        [Workspace]
        public string PartDisplayName { get; set; }

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

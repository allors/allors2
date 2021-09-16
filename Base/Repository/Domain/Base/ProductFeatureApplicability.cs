// <copyright file="ProductFeatureApplicability.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("003433eb-a0c6-454d-8517-0c03e9be3e96")]
    #endregion
    public partial class ProductFeatureApplicability : Period, Deletable
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        #endregion

        #region Allors
        [Id("3198ade4-8080-4584-9b67-b00af681c5cf")]
        [AssociationId("d0f5e3af-01ea-44fc-8921-a7eec052ed22")]
        [RoleId("73ff3323-7903-42c7-8278-b5f36f547463")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Product AvailableFor { get; set; }

        #region Allors
        [Id("c17d3bde-ebbc-463c-b9cb-b0a5a700c6a1")]
        [AssociationId("323a85e8-ee5c-4967-9f3d-64e8e5b04d7c")]
        [RoleId("22a7598e-6862-4627-b380-06804e263871")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("A1AE46BD-FB2B-4454-8A4B-9D4C7025A577")]
        [AssociationId("A8488C04-1961-4ADF-B66D-8932A2750F8E")]
        [RoleId("6BAFBF65-D366-4177-B33B-B99F7C4B7F37")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public ProductFeatureApplicabilityKind ProductFeatureApplicabilityKind { get; set; }

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

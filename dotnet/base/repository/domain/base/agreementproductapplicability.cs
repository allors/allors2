// <copyright file="AgreementProductApplicability.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("3021de37-fd9a-4ba1-b7e9-2ba56d4cd03e")]
    #endregion
    public partial class AgreementProductApplicability: Period
    {
        #region inherited properties

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("5ec038e4-581a-400e-bf33-4a6b2543db86")]
        [AssociationId("6ac3b2c8-f60e-4bcd-8622-1de70b51ca40")]
        [RoleId("069d1dc7-263b-41a7-b3c1-9418abed4e78")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Agreement Agreement { get; set; }

        #region Allors
        [Id("8cd6c155-30a4-4bb6-b8b4-15333c4a0b2f")]
        [AssociationId("375e5b6e-b23c-45c2-82e3-da0896601c78")]
        [RoleId("f113d59f-d7a7-45ee-9f72-6d4e6497d5a0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public AgreementItem AgreementItem { get; set; }

        #region Allors
        [Id("96f730a5-5212-4751-81ad-752046e9eadd")]
        [AssociationId("26c3a0cf-e9b9-411a-ba0e-e0c0482814f4")]
        [RoleId("4e8de6c5-f378-4e3f-abc7-f6aed2753e69")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Product Product{ get; set; }

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

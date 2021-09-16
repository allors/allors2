// <copyright file="PartBillOfMaterialSubstitute.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("5906f4cd-3950-43ee-a3ba-84124c4180f6")]
    #endregion
    public partial class PartBillOfMaterialSubstitute : Period, Commentable, Object
    {
        #region inherited properties
        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        #endregion

        #region Allors
        [Id("3d84d60f-c8b7-4e33-847a-9720d6570dd1")]
        [AssociationId("6124b5f7-ad97-44d6-8b7d-98694e385792")]
        [RoleId("e2423696-2855-4ac1-8a90-aba5e8413acc")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PartBillOfMaterial SubstitutionPartBillOfMaterial { get; set; }

        #region Allors
        [Id("9bff7f7d-c35c-426d-95f3-6a681d283914")]
        [AssociationId("c9fd9c9c-f57d-413a-ac69-7983f5d51dd6")]
        [RoleId("c7596ec0-5e1d-4d8e-8707-f276c01d1e5f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Ordinal Preference { get; set; }

        #region Allors
        [Id("a5273118-61c9-43de-9754-22555332cc27")]
        [AssociationId("3de9b9ee-a96c-43b7-984a-86f6d0d20a52")]
        [RoleId("0aff8adf-0487-4beb-b5f9-2062fa37ec9f")]
        #endregion

        public int Quantity { get; set; }

        #region Allors
        [Id("ef45301b-415a-417f-a952-fd71704a05e5")]
        [AssociationId("589cd7f5-a89e-48d2-adbe-8c6307ab3585")]
        [RoleId("aa0e3719-cbc3-4cb0-b83a-3ff5489771f3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public PartBillOfMaterial PartBillOfMaterial { get; set; }

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

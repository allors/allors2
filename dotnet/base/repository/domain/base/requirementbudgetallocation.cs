// <copyright file="RequirementBudgetAllocation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("5990c1d7-02d5-4e0d-8073-657b0dbfc5e1")]
    #endregion
    public partial class RequirementBudgetAllocation : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("4d5cfc89-068f-4cf5-ae51-0b3efe426499")]
        [AssociationId("cf5c7a91-3579-458b-8337-ed0ad9474fc4")]
        [RoleId("26f1d7ab-cda3-4b47-a422-293aa6c1f57f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public BudgetItem BudgetItem { get; set; }

        #region Allors
        [Id("9b0f81f1-8df5-4c42-aa10-a05caf777d57")]
        [AssociationId("9c7ea874-5d6d-4076-a4a4-7b6596f1ebd4")]
        [RoleId("06e6d247-68b5-42d4-a474-0c1ace36f21c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Requirement Requirement { get; set; }

        #region Allors
        [Id("f4f64ec3-5e56-45d8-8112-9a32c4f8d6da")]
        [AssociationId("95b80fdf-9e3f-424d-b03a-ede9268eb545")]
        [RoleId("82960011-47b7-43c5-ace3-d3830cd93d39")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal Amount { get; set; }

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

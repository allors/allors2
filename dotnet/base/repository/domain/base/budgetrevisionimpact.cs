// <copyright file="BudgetRevisionImpact.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("ebae3ca2-5dca-486d-bbc0-30550313f153")]
    #endregion
    public partial class BudgetRevisionImpact : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("16b0c91f-5746-4ebe-a071-7c42887cccb1")]
        [AssociationId("d8f69482-e661-4447-b055-7f3806cace95")]
        [RoleId("ca98b59f-ac83-457f-a5fd-24b35347ea14")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public BudgetItem BudgetItem { get; set; }

        #region Allors
        [Id("55e9b1e3-0545-471e-97b0-07d8968629c2")]
        [AssociationId("87269928-a93d-43b3-82d5-4d26d771b113")]
        [RoleId("972f8cb7-72bf-42bd-bf7f-405cbe9f8497")]
        #endregion
        [Required]
        [Size(-1)]

        public string Reason { get; set; }

        #region Allors
        [Id("6b3a80c1-eff1-478c-a54e-4912bc4a1242")]
        [AssociationId("c8f87804-9940-491d-a6aa-3b4dd888a016")]
        [RoleId("d187ff95-ee86-4d13-90b2-64adebc19be7")]
        #endregion

        public bool Deleted { get; set; }

        #region Allors
        [Id("7d0ad499-1e3d-41cd-bc6c-79aac1a7fa57")]
        [AssociationId("d409452f-bd4f-4c71-b71b-8512068d3ce8")]
        [RoleId("ba16d574-8bea-45b8-9da0-f9f14f21ca5f")]
        #endregion

        public bool Added { get; set; }

        #region Allors
        [Id("80106b6d-8e1d-4db1-a4eb-71a56e9a4c94")]
        [AssociationId("81e2607d-d1fc-475d-8a19-b60c34fae7f9")]
        [RoleId("80d4b2a2-f67e-4e76-b527-5b2d067a0499")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal RevisedAmount { get; set; }

        #region Allors
        [Id("b93df76d-439a-45cf-885d-4887afe5fd6f")]
        [AssociationId("ed0a9f21-20e3-4f26-a020-5b0afc8ec335")]
        [RoleId("dd1b9041-cff6-4d03-9215-d963a1c2a992")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public BudgetRevision BudgetRevision { get; set; }

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

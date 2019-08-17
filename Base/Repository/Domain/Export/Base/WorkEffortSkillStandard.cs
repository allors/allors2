// <copyright file="WorkEffortSkillStandard.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{

    using Allors.Repository.Attributes;

    #region Allors
    [Id("da16f5ee-0e04-41a7-afd7-b12e20414135")]
    #endregion
    public partial class WorkEffortSkillStandard : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("13a68eeb-7ca1-4ecd-a82b-ecbd75da99b6")]
        [AssociationId("fe6ffe1f-a4eb-4478-922f-4c786e40709c")]
        [RoleId("e0a9c761-26d1-48cd-bf13-bd6f66d863ba")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Skill Skill { get; set; }

        #region Allors
        [Id("20623472-f4f3-40fc-bd7c-cd3da44fe224")]
        [AssociationId("5bc7090b-cf0e-4d08-8c8a-12db14e42ec3")]
        [RoleId("57be9ba9-bb08-4b38-80eb-596f550f7963")]
        #endregion

        public int EstimatedNumberOfPeople { get; set; }

        #region Allors
        [Id("e05c673f-6c4b-492d-bf68-b4af15310aea")]
        [AssociationId("4cd6b8fb-6713-4ba2-8cf8-7fa80e824a0e")]
        [RoleId("72b3e6fe-1f43-4964-90e0-a718635d985d")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal EstimatedDuration { get; set; }

        #region Allors
        [Id("ed6a55d4-def6-49e0-8b1a-9ee99d8b3c3d")]
        [AssociationId("d5289442-6578-4928-873e-7e64cafadf66")]
        [RoleId("206a7548-e6bc-4886-a53c-c11afcd83ede")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal EstimatedCost { get; set; }

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

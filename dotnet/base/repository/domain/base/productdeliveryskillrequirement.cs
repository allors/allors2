// <copyright file="ProductDeliverySkillRequirement.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("fd342cb7-53d3-4377-acd8-ee586b924678")]
    #endregion
    public partial class ProductDeliverySkillRequirement : Object
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("12c6abaf-a080-45f3-820d-b462978d2539")]
        [AssociationId("4a6bd8f2-ea2a-4e07-a018-4b4b37b45a96")]
        [RoleId("3e12bb69-b0bb-40ba-a987-89f5cc40c436")]
        #endregion

        public DateTime StartedUsingDate { get; set; }

        #region Allors
        [Id("5a52b67e-23e4-45ac-a1d4-cb083bf897cc")]
        [AssociationId("7de9d895-a524-4fc2-a5f4-7b9e78921d6c")]
        [RoleId("b44bf4b9-3aa8-4abb-b145-11d888bf55c5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Service Service { get; set; }

        #region Allors
        [Id("6d4ec793-41a7-4044-9744-42d1bd44bbd4")]
        [AssociationId("fe73df0c-f46c-42e6-8274-a5de09de72d5")]
        [RoleId("2c2f1476-48a5-45f4-86df-03a86a965af4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public Skill Skill { get; set; }

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

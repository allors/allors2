// <copyright file="NeededSkill.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("5e31a968-5f7d-4109-9821-b94459f13382")]
    #endregion
    public partial class NeededSkill : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("079ef934-26e1-4dba-a69a-73fcc22d380e")]
        [AssociationId("f2afa9e5-239d-46c8-94c7-57dd23cb645a")]
        [RoleId("90f27ec7-03b8-491b-862c-3c18a37d4dbc")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public SkillLevel SkillLevel { get; set; }

        #region Allors
        [Id("21207c09-22b0-469a-84a7-6edd300c73f7")]
        [AssociationId("a2c931e4-8200-4cdd-9d26-bedbaf529c29")]
        [RoleId("1984780a-81fa-4391-af4e-20f707550a3d")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal YearsExperience { get; set; }

        #region Allors
        [Id("590d749a-52d4-448a-8f95-8412c0115825")]
        [AssociationId("3e6cc798-dae0-4381-abfd-bcba0b449d03")]
        [RoleId("09e6d6b8-8a89-46af-99fa-f332fea7ab6c")]
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

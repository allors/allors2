// <copyright file="WorkEffortAssignmentRate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("ac18c87b-683c-4529-9171-d23e73c583d4")]
    #endregion
    public partial class WorkEffortAssignmentRate : Deletable, DelegatedAccessControlledObject
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("BFFC696B-84AE-4803-8C80-FF20E99BE46D")]
        [AssociationId("FE4E09BF-FC44-4773-8827-C7D4D0BEB952")]
        [RoleId("8FDFA483-EA9F-4EC7-BBBA-0479F5493E7F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public WorkEffort WorkEffort { get; set; }

        #region Allors
        [Id("98BF1DDB-F0B7-48B6-9CFA-FD1832B4C0AC")]
        [AssociationId("9E36B3C6-39DE-4584-8CB6-634491BB332C")]
        [RoleId("66D022BB-F7C1-43F9-806D-365BDCF529E1")]
        [Indexed]
        #endregion
        [Workspace]
        public DateTime FromDate { get; set; }

        #region Allors
        [Id("651B674C-44AA-41D4-979D-958EBC3FEC5D")]
        [AssociationId("1C7B9EEC-2D35-4B71-AEB1-83B419DCA1A2")]
        [RoleId("49DF17D7-7793-4584-A3B6-EE99F385653F")]
        [Indexed]
        #endregion
        [Workspace]
        public DateTime ThroughDate { get; set; }

        #region Allors
        [Id("738EFE42-075D-46B6-974C-CD57FFAC7401")]
        [AssociationId("F43638F0-B550-456C-8FA3-1A1C6EC55A09")]
        [RoleId("728ECA9A-D473-4454-A85E-0B0C452E489B")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Rate { get; set; }

        #region Allors
        [Id("74A2B45F-4873-434F-900F-D1663B170172")]
        [AssociationId("DA2A0123-D5CC-49DE-9FA5-166710F1F1AC")]
        [RoleId("C4A299E5-8BF8-4155-8BB8-B3C11AABF017")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public RateType RateType { get; set; }

        #region Allors
        [Id("A140FB3E-76E8-411E-835A-FE9EB8E84F19")]
        [AssociationId("325BC434-6E19-46B1-A854-52D313E357FA")]
        [RoleId("A47F37E0-15F7-439B-8FAF-423A270D9927")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal Cost { get; set; }

        #region Allors
        [Id("953BAA4D-5455-47C8-B8B0-D3673EFF358D")]
        [AssociationId("45E3A81D-B08B-4172-850E-B87A0A7648D2")]
        [RoleId("F2D37E12-5450-4EE2-82AF-14A1D5E14B48")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public TimeFrequency Frequency { get; set; }

        #region Allors
        [Id("627da684-d501-4221-97c2-81329e2b5e8c")]
        [AssociationId("4b9c1fd3-acf0-4e5b-8cb5-d32f94bff10b")]
        [RoleId("e6409680-f8e1-4c61-8bd3-b9ec42435741")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public WorkEffortPartyAssignment WorkEffortPartyAssignment { get; set; }

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

        public void DelegateAccess() { }

        #endregion
    }
}

// <copyright file="WorkEffortType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{

    using Allors.Repository.Attributes;

    #region Allors
    [Id("7d2d9452-f250-47c3-81e0-4e1c0655cc86")]
    #endregion
    public partial class WorkEffortType : Object
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("5ce1a600-62a9-4d2c-bfb5-bfe374b2099f")]
        [AssociationId("4d892148-3583-46a2-b68d-895274b9ea7a")]
        [RoleId("fa8657ae-5132-4f37-aba0-4f95c4b1df1e")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public WorkEffortFixedAssetStandard[] WorkEffortFixedAssetStandards { get; set; }

        #region Allors
        [Id("776839ee-f6cb-4334-a017-4ffdfddd152a")]
        [AssociationId("db3c9fba-5ca8-4296-b1a2-d306ad42dbcc")]
        [RoleId("764e51c4-8a6f-403d-849a-1bf3a1a64911")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public WorkEffortGoodStandard[] WorkEffortGoodStandards { get; set; }

        #region Allors
        [Id("89eef4e3-eda7-4336-91cb-ce7a7e96521f")]
        [AssociationId("4b61f74f-db7c-4733-b6f5-d485e432a16e")]
        [RoleId("8b9f019b-e79e-4282-9ee1-9bc652fd6817")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        public WorkEffortType[] Children { get; set; }

        #region Allors
        [Id("8d9f51b5-2c8d-4a25-a45e-c79542a09434")]
        [AssociationId("687e9909-0efa-4a04-b705-96d93547458a")]
        [RoleId("da4648c1-b495-4555-8fa3-c4ba8141e67d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public FixedAsset FixedAssetToRepair { get; set; }

        #region Allors
        [Id("93cfed3d-ae24-4a07-becf-34cdc3cdef3e")]
        [AssociationId("958d3cdc-0dbe-4ce6-81c7-492825727ada")]
        [RoleId("d0fadb8a-4891-4caf-a010-6197676cfd54")]
        #endregion
        [Required]
        [Size(-1)]

        public string Description { get; set; }

        #region Allors
        [Id("b6d68eff-8a3a-473f-bb4e-9bc46808bde0")]
        [AssociationId("1996225b-a372-44ad-b00a-b257a355d756")]
        [RoleId("2c704b66-922a-4c5c-81fb-973545230501")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]

        public WorkEffortType[] Dependencies { get; set; }

        #region Allors
        [Id("ccf22455-c42a-4f9c-8975-813431bcdd8b")]
        [AssociationId("f70821d5-2a7f-4fdd-ae45-b8c7966710fc")]
        [RoleId("abe54968-9a36-43c5-a57c-b8d1cde032ea")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]

        public WorkEffortTypeKind WorkEffortTypeKind { get; set; }

        #region Allors
        [Id("d51d620e-250e-4492-8926-c8535fad19ec")]
        [AssociationId("e26db451-eb86-44b1-b3cb-eb29d4311157")]
        [RoleId("2a4de99b-9544-4c67-b936-431622654f09")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public WorkEffortPartStandard[] WorkEffortPartStandards { get; set; }

        #region Allors
        [Id("df104ec4-6247-4199-bce1-635978fa8ad4")]
        [AssociationId("0826559c-11ef-4075-ad8e-28c7ed693f1c")]
        [RoleId("073f09b7-1502-4bac-b344-76f4cb6f3907")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]

        public WorkEffortSkillStandard[] WorkEffortSkillStandards { get; set; }

        #region Allors
        [Id("df1fa89e-25e2-4b72-a928-67fa2c95ad70")]
        [AssociationId("361d313b-8313-43bd-8a98-9b2516ca25f7")]
        [RoleId("3d4d3fd1-28dd-4349-bb57-b869687f5f82")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        public decimal StandardWorkHours { get; set; }

        #region Allors
        [Id("ee521062-a2bf-4a7f-80e4-8da6f63439fe")]
        [AssociationId("fc63a85e-7bc4-49ec-89f5-66fef934f11a")]
        [RoleId("5d9c847c-f2a5-4353-8558-880f60e75925")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Product ProductToProduce { get; set; }

        #region Allors
        [Id("f8766ab1-b0ed-42fa-806c-c40a2e68d72a")]
        [AssociationId("7e7a9632-76a8-48c3-ada3-fcc3aa06a511")]
        [RoleId("9a3b5dd0-a399-43ef-9607-32136bb5f3cd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]

        public Deliverable DeliverableToProduce { get; set; }

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

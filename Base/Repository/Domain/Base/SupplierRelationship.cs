// <copyright file="SupplierRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("2b162153-f74d-4f97-b97c-48f04749b216")]
    #endregion
    public partial class SupplierRelationship : PartyRelationship, Period, Deletable, Object
    {
        #region inherited properties

        public Party[] Parties { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ThroughDate { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region Allors
        [Id("1546c9f0-84ce-4795-bcea-634d6a78e867")]
        [AssociationId("56c5ff64-f67b-4830-a1e4-11661b0ff898")]
        [RoleId("a0e757a2-d780-43a1-8b21-ab2fc4d75e7e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Organisation Supplier { get; set; }

        #region Allors
        [Id("70914837-D4C3-472B-A6DA-E8EE42D36E99")]
        [AssociationId("24423780-B007-4188-8019-6D74C511D639")]
        [RoleId("5BA1B394-A12F-4D0B-BA8A-AB7C701C5B03")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public InternalOrganisation InternalOrganisation { get; set; }

        #region Allors
        [Id("7C5FDA1C-CE16-45C5-80DB-D89E2E9FB273")]
        [AssociationId("3CCD3C96-2348-4A5C-8C1A-8B08C67D580C")]
        [RoleId("44F54BE6-E873-436D-8B46-6924F361DCA9")]
        #endregion
        [Required]
        [Workspace]
        public bool NeedsApproval { get; set; }

        #region Allors
        [Id("EC58B25E-D84A-402D-873B-A48E1E59365D")]
        [AssociationId("CD76AE94-2B41-44B0-8085-995AC7A2A2EB")]
        [RoleId("1B9652BB-C074-46AB-AA3E-C2645CC1BAFF")]
        #endregion
        [Required]
        [Workspace]
        public decimal ApprovalThresholdLevel1 { get; set; }

        #region Allors
        [Id("3ABDE14F-EEC1-4B45-9846-7896ABC27FBB")]
        [AssociationId("B673D04F-3ECE-4452-84D6-27E3DC1BC772")]
        [RoleId("D73494F3-20FC-40A5-94BF-D45004731C5A")]
        #endregion
        [Required]
        [Workspace]
        public decimal ApprovalThresholdLevel2 { get; set; }

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

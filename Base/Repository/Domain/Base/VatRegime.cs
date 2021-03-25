// <copyright file="VatRegime.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("69db99bc-97f7-4e2e-903c-74afb55992af")]
    #endregion
    public partial class VatRegime : Enumeration, Versioned
    {
        #region inherited properties
        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Versioning
        #region Allors
        [Id("9c67ee80-4119-4af7-a997-1ed14b4e340b")]
        [AssociationId("6e336794-7407-49c5-a9eb-e5c178d5ee8d")]
        [RoleId("7beeec69-549d-491c-8d3e-7c1352fbf11e")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public VatRegimeVersion CurrentVersion { get; set; }

        #region Allors
        [Id("ec4eb2ac-ed0d-4649-aa8c-72d51bc9a032")]
        [AssociationId("fe57575d-ba66-4050-ba9a-164a0efb2314")]
        [RoleId("c1d5a396-76df-44bc-aa3e-07172d30722a")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public VatRegimeVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("71cb2a90-e65e-4a98-a2dd-cb806d7ed0e7")]
        [AssociationId("379b9fcc-3793-46c9-b1cc-bc5c896e52e9")]
        [RoleId("2abe1aa7-3efb-4199-b94e-7b643483c69a")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Country Country { get; set; }

        #region Allors
        [Id("2071cc28-c8bf-43dc-a5e5-ec5735756dfa")]
        [AssociationId("fca4a435-bd82-496b-ab1d-c2b6cb10494f")]
        [RoleId("baf416cf-3222-4c93-8fb7-f4257b2b9ef9")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public VatRate ObsoleteVatRate { get; set; }

        #region Allors
        [Id("8c66f441-be9a-468f-86f7-19fb2cebb51b")]
        [AssociationId("8e0de894-435b-40f9-b700-fa06efe73de5")]
        [RoleId("e47d68cd-c54e-4320-8868-7d0c5927fff6")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        public VatRate[] VatRates { get; set; }

        #region Allors
        [Id("00A91056-1F2D-462F-8A81-6DA277AD86E1")]
        [AssociationId("0E3D88B3-ECF4-4681-B16F-91B55CD5308B")]
        [RoleId("6BD52A1A-E7A0-479F-95EC-452BB23A3167")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public VatClause VatClause { get; set; }

        #region Allors
        [Id("a037f9f0-1aff-4ad0-8ee9-36ae4609d398")]
        [AssociationId("25db54a8-873d-4736-8408-f1d9e65c49e4")]
        [RoleId("238996a2-ec4f-47f4-8336-8fee91383649")]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public OrganisationGlAccount GeneralLedgerAccount { get; set; }

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

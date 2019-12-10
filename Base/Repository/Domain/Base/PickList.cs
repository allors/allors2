// <copyright file="PickList.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("27b6630a-35d0-4352-9223-b5b6c8d7496b")]
    #endregion
    public partial class PickList : Deletable, Printable, Transitional, Versioned
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public PrintDocument PrintDocument { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region ObjectStates
        #region PickListState
        #region Allors
        [Id("87B1275D-A60B-46B7-8510-CA42EBAAEF97")]
        [AssociationId("76F149C9-7E8D-4A79-9D9D-25E8637A605F")]
        [RoleId("9047B405-4015-4DA3-A164-04316471B1C8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PickListState PreviousPickListState { get; set; }

        #region Allors
        [Id("86EEF807-1C0B-4053-82EF-D90CD379A6D8")]
        [AssociationId("26F8DC28-38E4-43DA-8003-E1E64EAF9FB4")]
        [RoleId("211588E8-3618-4A36-964E-D3638BEB4B69")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PickListState LastPickListState { get; set; }

        #region Allors
        [Id("FDFC9DF1-D4A6-4F4F-BA5E-4D523DA7D00A")]
        [AssociationId("D0B14193-7C4E-4B98-BE1E-3C2677E693F8")]
        [RoleId("9659F469-250E-4531-AE54-11FB008ED957")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PickListState PickListState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("7BF3DC9C-258D-4744-8EAC-8DBD3702C178")]
        [AssociationId("DE640A48-A9B0-4DBB-8C9A-7122B66B15FD")]
        [RoleId("DA6E6B5F-D945-4B62-91C9-8E67F604BBB3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PickListVersion CurrentVersion { get; set; }

        #region Allors
        [Id("2C188CDB-CCE2-43D3-B1A3-D2F33716B02C")]
        [AssociationId("373A7FEF-6669-4B89-95F0-6BCC9C5E6EAC")]
        [RoleId("9958F7AF-0E25-4E4E-83AC-B673C9224E04")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PickListVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("3bb68c85-4e2b-42b8-b5fb-18a66c58c283")]
        [AssociationId("11fddfe2-9b04-4b53-a4ff-6f571e73c32a")]
        [RoleId("a139b102-f8a9-43f1-9b14-d3c76f7be294")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public PickListItem[] PickListItems { get; set; }

        #region Allors
        [Id("6572f862-31b2-4be9-b7dc-7fff5d21f620")]
        [AssociationId("2a502d47-1319-45a4-ad52-70dd41435732")]
        [RoleId("76ddffff-4968-4b4b-8b52-58a1a05a774d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person Picker { get; set; }

        #region Allors
        [Id("ae75482e-2c41-46d4-ab73-f3aac368fd50")]
        [AssociationId("6b8acd68-6aba-4092-8c87-cdc62d9a4c82")]
        [RoleId("61785577-8ab7-457c-870f-69ecb7c41f8b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party ShipToParty { get; set; }

        #region Allors
        [Id("e334e938-35e7-4217-91fa-efb231f71a37")]
        [AssociationId("0706d8f1-764d-4ab9-b63a-1b0213cc9dbd")]
        [RoleId("4c3d2de1-6735-40fc-bfe9-65a64aaf966c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Store Store { get; set; }

        #region Allors
        [Id("46CB3076-14AE-48C1-8C9F-F4EFB4B060EB")]
        #endregion
        [Workspace]
        public void Hold() { }

        #region Allors
        [Id("F3D35303-BA28-4CF0-B393-7E7D76F5B86D")]
        #endregion
        [Workspace]
        public void Continue() { }

        #region Allors
        [Id("8753A40E-FAA1-44E7-86B1-6CA6712989B5")]
        #endregion
        [Workspace]
        public void Cancel() { }

        #region Allors
        [Id("CA2ADD8E-C43E-4C95-A8A4-B279FEE4CB0A")]
        #endregion
        [Workspace]
        public void SetPicked() { }

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

        public void Print() { }

        #endregion
    }
}

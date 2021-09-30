// <copyright file="FixedAsset.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("4a3efb9c-1556-4e57-bb59-f09d297e607e")]
    #endregion
    public partial interface FixedAsset : Commentable, Searchable
    {
        #region Allors
        [Id("354107ce-4eb6-4b9a-83c8-5cfe5e3adb22")]
        [AssociationId("e0f80027-f068-4ff8-a351-b3199f92735f")]
        [RoleId("6806756e-a152-42a9-b32b-b14269e712e2")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string Name { get; set; }

        #region Allors
        [Id("54EC72A4-B9AC-44A0-A29F-10DAC2F58DEC")]
        [AssociationId("31962D0B-3A42-49DF-B660-304206F2F1E5")]
        [RoleId("0DDBA6CC-868E-45CB-BAA3-6FE971F06021")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        LocalisedText[] LocalisedNames { get; set; }

        #region Allors
        [Id("51133e4d-5135-4991-9f2f-8df9762fac78")]
        [AssociationId("fc2144b7-4a88-412d-9792-ba6f6c93c637")]
        [RoleId("1cc0737e-a810-48d3-b048-7e3077d3db5c")]
        #endregion
        [Workspace]
        DateTime LastServiceDate { get; set; }

        #region Allors
        [Id("54cf9225-9204-43ee-9984-7fd8b2cbf8bc")]
        [AssociationId("efb718b5-7d70-4696-81c8-961582ed01f2")]
        [RoleId("99c0a722-af34-4008-b7f5-dc4315c7fa1a")]
        #endregion
        [Workspace]
        DateTime AcquiredDate { get; set; }

        #region Allors
        [Id("725c6b7d-68ed-4576-8b17-eac4e9f4db83")]
        [AssociationId("ce93a11b-7c87-4d9c-9d79-a9703a9fd86d")]
        [RoleId("96524022-ff94-482a-a17c-6c3c96f79127")]
        #endregion
        [Size(-1)]
        [Workspace]
        [MediaType("text/markdown")]
        string Description { get; set; }

        #region Allors
        [Id("16B8D296-15ED-47F3-8278-C59ED863E0F8")]
        [AssociationId("E3B33F6F-C51C-4EC7-88C5-10DF34F7D48B")]
        [RoleId("94998B53-4AFE-4E91-894F-3D60E1E17D0B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        LocalisedText[] LocalisedDescriptions { get; set; }

        #region Allors
        [Id("913cc338-f844-49ae-886a-2e32db190b78")]
        [AssociationId("276b6fca-d2bb-4e43-af51-378c599c80f6")]
        [RoleId("f409664f-5c7e-4f3b-809c-acd43c36b3bc")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        decimal ProductionCapacity { get; set; }

        #region Allors
        [Id("ead0e86a-dfc7-45b0-9865-b973175c4567")]
        [AssociationId("6be614a2-0511-4ca0-9b1c-c8a3d0b0a998")]
        [RoleId("47d9d93c-8ba3-4f28-a8a5-6a4cb02853e6")]
        #endregion
        [Workspace]
        DateTime NextServiceDate { get; set; }

        #region Allors
        [Id("822F98D5-D9B2-44DA-B097-911244609066")]
        [AssociationId("4352BA8D-EE74-4E1D-805A-648E73E3FEC3")]
        [RoleId("4210FC74-0F8B-4980-A600-98BE6DAB78DC")]
        #endregion
        [Workspace]
        [Size(-1)]
        string Keywords { get; set; }

        #region Allors
        [Id("EAC7725D-7B6A-48E1-95D1-3D9B17F5FDA8")]
        [AssociationId("DFD5AB33-E2D4-4860-9AC2-77E281ADF941")]
        [RoleId("79F6FB5F-8F4F-483A-8E79-1EF50FDCE82C")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        LocalisedText[] LocalisedKeywords { get; set; }

        #region Allors
        [Id("31c78290-83ff-469b-bbc5-8b9c2c7cbdf3")]
        [AssociationId("272a8aa8-eef1-4471-b40a-52c1753afa81")]
        [RoleId("4b359c7b-2252-45b9-a9d2-9d140dd44b75")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        Media[] PublicElectronicDocuments { get; set; }

        #region Allors
        [Id("681b22d3-5338-4971-89e1-7f812883b728")]
        [AssociationId("a87cb9be-d7a9-4434-b00d-2474c37dd66e")]
        [RoleId("abb8e44c-cc95-48b2-a1e3-4afe0ca2b06f")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        LocalisedMedia[] PublicLocalisedElectronicDocuments { get; set; }

        #region Allors
        [Id("6835b1ee-427b-4fb0-acc4-e5572be2a855")]
        [AssociationId("c2b2f5e6-5cb3-48d5-b07e-2d89dd0547bf")]
        [RoleId("45595510-9337-4d71-8616-5cf8d24ec4e6")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        Media[] PrivateElectronicDocuments { get; set; }

        #region Allors
        [Id("cfd10526-8ba6-4194-ad9d-12bbaedcc1ce")]
        [AssociationId("a3f7ef2f-c7cd-4085-81c7-f630009345aa")]
        [RoleId("8449b3ec-6f93-4c69-97d1-d2da667d4ade")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        LocalisedMedia[] PrivateLocalisedElectronicDocuments { get; set; }
    }
}

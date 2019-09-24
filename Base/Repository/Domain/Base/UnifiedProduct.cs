// <copyright file="UnifiedProduct.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes;

    #region Allors
    [Id("823D3C34-3441-40CF-8ED8-C44694933DC6")]
    #endregion
    public partial interface UnifiedProduct : Commentable, UniquelyIdentifiable, Deletable, Object
    {
        #region Allors
        [Id("1A5619BE-43D0-47CF-B906-0A15277B86A6")]
        [AssociationId("3E3E2DEE-FABB-41C3-A0DD-6A457A829A8C")]
        [RoleId("DE123A63-F0AC-41ED-9CF8-76ED588EA9E2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        ProductIdentification[] ProductIdentifications { get; set; }

        #region Allors
        [Id("7423a3e3-3619-4afa-ab67-e605b2a62e02")]
        [AssociationId("153ce3b0-0969-40d7-a766-1320ecaef8ac")]
        [RoleId("62228e49-a697-4f1f-8a85-6f1976afd7bb")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        string Name { get; set; }

        #region Allors
        [Id("EBF60298-05C0-4885-81F9-64E0BE4ACE40")]
        [AssociationId("17884E44-F24C-4DFB-A75F-86916799D2BC")]
        [RoleId("3F2A6134-FFF5-45A1-AE66-C0947CC9F28F")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        LocalisedText[] LocalisedNames { get; set; }

        #region Allors
        [Id("0cbb9d37-20cf-4e0c-9099-07f1fcb88590")]
        [AssociationId("6ed33681-defd-4003-85e4-79b5ddce888f")]
        [RoleId("cf55a72e-6ca5-4315-af71-ad45ab17fdf3")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Description { get; set; }

        #region Allors
        [Id("C8B9AF4A-9385-4225-A458-77EC3526A7B2")]
        [AssociationId("4DA0E61C-341C-42A6-AF5D-A3DD98535C55")]
        [RoleId("5358F7F2-D1CE-42B1-9CF5-A592D01466F9")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        LocalisedText[] LocalisedDescriptions { get; set; }

        #region Allors
        [Id("D88189C8-735E-4A5A-B46F-AEFF4F1F0501")]
        [AssociationId("93F47711-09D1-45FE-9FCD-20C0AF122484")]
        [RoleId("97F9DCEE-ED7F-458A-BE81-D14579FD11C1")]
        #endregion
        [Workspace]
        [Size(-1)]
        string InternalComment { get; set; }

        #region Allors
        [Id("7c41deee-b270-4810-abaa-6d00e6507b9b")]
        [AssociationId("72d6f463-2335-44bc-830f-816ee635101b")]
        [RoleId("8926c093-d513-44dd-9324-3accc051cb06")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Document[] Documents { get; set; }

        #region Allors
        [Id("944F8DF7-1E19-46EA-8DFA-031093DD387E")]
        [AssociationId("CB7DD964-E46C-4589-B04E-FB23FE8946F8")]
        [RoleId("57F654B9-C50E-4B09-B51A-D6977D5895B7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        Media[] ElectronicDocuments { get; set; }

        #region Allors
        [Id("AFBF7196-1EDC-4F3A-845B-E583E6EFA6B8")]
        [AssociationId("19C9EA89-108A-4F96-B3AD-6CD6C8C46BCF")]
        [RoleId("44D97AF0-CC68-4ED8-AA41-EAA22369E3A8")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        LocalisedMedia[] LocalisedElectronicDocuments { get; set; }

        #region Allors
        [Id("9b66342e-48ac-4761-b375-b9b60d94b005")]
        [AssociationId("fcb1a5ad-544f-4613-a160-077d9130732f")]
        [RoleId("76542b1d-9085-451c-9110-85bfac863016")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("38010003-F586-49E5-8A6C-11D490738B9A")]
        [AssociationId("B79D41A7-F98E-4308-8F2B-19A2AB70D68E")]
        [RoleId("0C4A4802-2753-4D10-BBB5-6AAB23212D37")]
        #endregion
        [Workspace]
        [Size(-1)]
        string Keywords { get; set; }

        #region Allors
        [Id("990A3EFD-891E-4BB9-8FEE-8DE538E9EC04")]
        [AssociationId("266F8DEA-8AA2-4A9B-AD1B-3D229C4CE559")]
        [RoleId("D5A7BD3C-82AB-47CC-9974-46BCC4AB8A6F")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        LocalisedText[] LocalisedKeywords { get; set; }

        #region Allors
        [Id("f52c0b7e-dbc4-4082-a2b9-9b1a05ce7179")]
        [AssociationId("50478ca9-3eb4-487b-8c8a-6ff48d9155b5")]
        [RoleId("802b6cdb-873a-4455-9fa7-7f2267407f0f")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        Media PrimaryPhoto { get; set; }

        #region Allors
        [Id("C7FB85EB-CF47-4FE1-BD67-E2832E5893B9")]
        [AssociationId("1DE2FF68-A4CB-4244-8C34-6E9D08A6DFBF")]
        [RoleId("2A1DB194-1B06-498D-BA0D-C2FDA629A45D")]
        [Indexed]
        #endregion
        [Workspace]
        [Multiplicity(Multiplicity.ManyToMany)]
        Media[] Photos { get; set; }
    }
}

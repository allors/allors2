// <copyright file="RequestItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("daf83fcc-832e-4d5e-ba71-5a08f42355db")]
    #endregion
    public partial class RequestItem : DelegatedAccessControlledObject, Commentable, Transitional, Versioned, Deletable
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region ObjectStates
        #region RequestItemState
        #region Allors
        [Id("0F6064F5-C92E-44C7-A050-F9599F1BE78D")]
        [AssociationId("6CCA300C-0613-4821-8AD3-CFE1AD4FBA53")]
        [RoleId("6CB80F78-C852-445C-8308-ABA1E9FE5365")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public RequestItemState PreviousRequestItemState { get; set; }

        #region Allors
        [Id("B8D32D2C-5E6E-40A5-8C5D-EB5E5A6F32A9")]
        [AssociationId("07A67BD6-4907-4FF1-B9CA-1B96307409B4")]
        [RoleId("45AE6577-AEBF-41E2-9C68-6C1345498221")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public RequestItemState LastRequestItemState { get; set; }

        #region Allors
        [Id("D90E2400-498A-436C-B456-5B64E0E6A3E2")]
        [AssociationId("B46C8F61-3E56-426E-A2F3-4F35F29B9664")]
        [RoleId("D9FFE31A-FCAF-45EF-BC49-386E78EA0F4B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public RequestItemState RequestItemState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("AE96F162-EE98-47BA-940F-45C81412702D")]
        [AssociationId("E303A1A5-FF15-4DA3-9281-AAC482EBD3A8")]
        [RoleId("D1DDDFA7-2C05-4527-BADC-253187B3FE74")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("0E0D556B-BB62-4E88-A127-C6CD1C9CB4AB")]
        [AssociationId("9D051007-86F0-4C25-A932-B9C6F365D20D")]
        [RoleId("18D6FA02-5504-4934-A66D-C89594BE6511")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("1A82EB63-123E-4055-BA4C-8DAA62648565")]
        [AssociationId("DB3E0711-6A15-4D96-9D49-36E6C8C19567")]
        [RoleId("55F3B5F5-D36B-4414-875D-2A458CABA931")]
        #endregion
        [Workspace]
        [Size(-1)]
        public string InternalComment { get; set; }

        #region Allors
        [Id("542f3de9-e808-443b-b6e6-baf2db1ec2b1")]
        [AssociationId("30b2b652-b7a8-42ec-bd10-dd606a1be951")]
        [RoleId("c176a9ff-7656-4c22-bf00-e19cdcd16566")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("5c0f0069-b7f9-47f1-8346-c30f14afbc0c")]
        [AssociationId("0f924664-8b58-45f4-b6f3-d8201610de8f")]
        [RoleId("3560f38b-1945-4eb1-9b9a-c3e84d267647")]
        #endregion
        [Required]
        [Workspace]
        public int Quantity { get; set; }

        #region Allors
        [Id("B48D0207-26CD-4A63-922F-69EC62704200")]
        [AssociationId("0ED387D1-A458-428B-A422-4CAC0D140841")]
        [RoleId("A4B9AD09-CB3D-460D-B0F2-232C0E124170")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        #region Allors
        [Id("6544faeb-a4cf-447c-a696-b6561c45086e")]
        [AssociationId("3d03cbae-7618-458e-b705-94112c8f66db")]
        [RoleId("0204dc28-cec2-4d6b-b525-c7e4c65f958b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Requirement[] Requirements { get; set; }

        #region Allors
        [Id("a5d1bef9-3086-4c32-9a6d-ce33c4f09539")]
        [AssociationId("1d3eedcb-dc13-46ad-ac43-e6979995e00b")]
        [RoleId("474a6350-abba-4c53-ba26-0320c60aa8a8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Deliverable Deliverable { get; set; }

        #region Allors
        [Id("bf40cb6b-e561-4df1-9ac4-e5a72933c7db")]
        [AssociationId("2eddcab2-e293-4699-8392-c198018a8ce4")]
        [RoleId("90b8c610-e703-4109-92c7-bad2f5e1501b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("d02d15ae-2938-4753-95f1-686ea8b02f47")]
        [AssociationId("0abe5f12-ae64-4a8e-b5cc-175d7d2ea1d7")]
        [RoleId("91d62ba3-943e-4aa5-b4fc-6a1f62fcd63f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public NeededSkill NeededSkill { get; set; }

        #region Allors
        [Id("f03c07b5-44f2-4e61-ad23-7c373851dafc")]
        [AssociationId("d4cce9f6-ebe0-4b72-86f6-d41c8cdf072e")]
        [RoleId("bd7d900b-d5c6-46dc-8843-e4041429858b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("A3286D1A-0064-404E-B1A2-A3C9B52D7D7A")]
        [AssociationId("6DAC8E0F-810C-424B-86B4-36040509776B")]
        [RoleId("5B0C025F-5691-45DB-A6A4-C6B715123CA2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

        #region Allors
        [Id("fa33c3e6-53c4-428a-bd9c-feba1dd9ed45")]
        [AssociationId("1aa99128-9989-4933-8204-9acefc7b040d")]
        [RoleId("99251c00-d729-4363-8ce8-403ace61725e")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaximumAllowedPrice { get; set; }

        #region Allors
        [Id("ff41a43c-997d-4158-984e-e669eb935148")]
        [AssociationId("b46ffa62-adcb-4928-bdb0-79d0eef9e676")]
        [RoleId("7c4353a9-efd5-437e-8789-fae92a0be1ed")]
        #endregion
        [Workspace]
        public DateTime RequiredByDate { get; set; }

        #region Allors
        [Id("A6AF9B26-D056-4CD4-BAFA-357EE754AB9A")]
        [AssociationId("F7F54E9D-EBED-4003-A3E4-329F108C159B")]
        [RoleId("AD7B1789-7BF2-404B-990A-D715559610F8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Synced]
        public Request SyncedRequest { get; set; }

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

        #region Allors
        [Id("67541211-256A-4CC8-BCAB-EEA2CCAEBE5F")]
        #endregion
        [Workspace]
        public void Cancel() { }

        #region Allors
        [Id("C3342A3D-A82C-4AD7-815C-921E7A19B5E3")]
        #endregion
        [Workspace]
        public void Submit() { }

        #region Allors
        [Id("{7B95A518-9656-4E18-B10B-CB3C59F2229A}")]
        #endregion
        [Workspace]
        public void Hold() { }
    }
}

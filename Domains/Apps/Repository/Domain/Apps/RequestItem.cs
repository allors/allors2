namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("daf83fcc-832e-4d5e-ba71-5a08f42355db")]
    #endregion
    public partial class RequestItem : AccessControlledObject, Commentable, Transitional
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string Comment { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
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
        [Size(256)]
        [Workspace]
        public string Description { get; set; }

        #region Allors
        [Id("5c0f0069-b7f9-47f1-8346-c30f14afbc0c")]
        [AssociationId("0f924664-8b58-45f4-b6f3-d8201610de8f")]
        [RoleId("3560f38b-1945-4eb1-9b9a-c3e84d267647")]
        #endregion
        [Workspace]
        public int Quantity { get; set; }

        #region Allors
        [Id("B48D0207-26CD-4A63-922F-69EC62704200")]
        [AssociationId("0ED387D1-A458-428B-A422-4CAC0D140841")]
        [RoleId("A4B9AD09-CB3D-460D-B0F2-232C0E124170")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
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
        [Id("DF6D8AA3-21C6-4FDF-A2F2-F50C2F7AA447")]
        [AssociationId("63AEABDE-A66E-44E2-8090-532297DD4F41")]
        [RoleId("8441F20E-887C-4852-B8A1-7BAC2A69E197")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        [Workspace]
        public RequestItemObjectState CurrentObjectState { get; set; }

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
        [Id("AF1D8408-2972-4C59-B6BD-819645672E61")]
        [AssociationId("85D24550-8E80-42ED-BD52-2B7D511D83AC")]
        [RoleId("15FD4D94-C198-429A-876F-80C590CA6378")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestItemVersion PreviousVersion { get; set; }

        #region Allors
        [Id("0E0D556B-BB62-4E88-A127-C6CD1C9CB4AB")]
        [AssociationId("9D051007-86F0-4C25-A932-B9C6F365D20D")]
        [RoleId("18D6FA02-5504-4934-A66D-C89594BE6511")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestItemVersion[] AllVersions { get; set; }

        #region Allors
        [Id("13BD89A9-2D31-4ACA-A2E1-CA16B2E0F307")]
        [AssociationId("2E4BBCB8-0F3A-4EB8-BA35-262BCBB2BDAC")]
        [RoleId("FF5FD4D4-5F53-4F83-A9E1-83DC730BB029")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public RequestItemVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("F4CAB729-E8BD-4755-A9BF-D9032C0CF19A")]
        [AssociationId("09B05189-B945-4686-8031-05C2263B820E")]
        [RoleId("85D5C2A9-8D43-41AB-8AF0-EC3BF5CAA3EA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public RequestItemVersion[] AllStateVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


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
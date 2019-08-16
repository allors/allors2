namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("7041c691-d896-4628-8f50-1c24f5d03414")]
    #endregion
    public partial class C1 : I1, DerivationCounted, Object
    {
        #region inherited properties
        public I1 I1I1Many2One { get; set; }

        public I12[] I1I12Many2Manies { get; set; }

        public I2[] I1I2Many2Manies { get; set; }

        public I2 I1I2Many2One { get; set; }

        public string I1AllorsString { get; set; }

        public I12 I1I12Many2One { get; set; }

        public DateTime I1AllorsDateTime { get; set; }

        public I2[] I1I2One2Manies { get; set; }

        public C2[] I1C2One2Manies { get; set; }

        public C1 I1C1One2One { get; set; }

        public int I1AllorsInteger { get; set; }

        public C2[] I1C2Many2Manies { get; set; }

        public I1[] I1I1One2Manies { get; set; }

        public I1[] I1I1Many2Manies { get; set; }

        public bool I1AllorsBoolean { get; set; }

        public decimal I1AllorsDecimal { get; set; }

        public I12 I1I12One2One { get; set; }

        public I2 I1I2One2One { get; set; }

        public C2 I1C2One2One { get; set; }

        public C1[] I1C1One2Manies { get; set; }

        public byte[] I1AllorsBinary { get; set; }

        public C1[] I1C1Many2Manies { get; set; }

        public double I1AllorsDouble { get; set; }

        public I1 I1I1One2One { get; set; }

        public C1 I1C1Many2One { get; set; }

        public I12[] I1I12One2Manies { get; set; }

        public C2 I1C2Many2One { get; set; }

        public Guid I1AllorsUnique { get; set; }

        public byte[] I12AllorsBinary { get; set; }

        public C2 I12C2One2One { get; set; }

        public double I12AllorsDouble { get; set; }

        public I1 I12I1Many2One { get; set; }

        public string I12AllorsString { get; set; }

        public I12[] I12I12Many2Manies { get; set; }

        public decimal I12AllorsDecimal { get; set; }

        public I2[] I12I2Many2Manies { get; set; }

        public C2[] I12C2Many2Manies { get; set; }

        public I1[] I12I1Many2Manies { get; set; }

        public I12[] I12I12One2Manies { get; set; }

        public string Name { get; set; }

        public C1[] I12C1Many2Manies { get; set; }

        public I2 I12I2Many2One { get; set; }

        public Guid I12AllorsUnique { get; set; }

        public int I12AllorsInteger { get; set; }

        public I1[] I12I1One2Manies { get; set; }

        public C1 I12C1One2One { get; set; }

        public I12 I12I12One2One { get; set; }

        public I2 I12I2One2One { get; set; }

        public I12[] Dependencies { get; set; }

        public I2[] I12I2One2Manies { get; set; }

        public C2 I12C2Many2One { get; set; }

        public I12 I12I12Many2One { get; set; }

        public bool I12AllorsBoolean { get; set; }

        public I1 I12I1One2One { get; set; }

        public C1[] I12C1One2Manies { get; set; }

        public C1 I12C1Many2One { get; set; }

        public DateTime I12AllorsDateTime { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public int DerivationCount { get; set; }

        #endregion

        #region Allors
        [Id("97f31053-0e7b-42a0-90c2-ce6f09c56e86")]
        [AssociationId("70e42b8b-09e2-4cb1-a632-ff3785ee1c8d")]
        [RoleId("e5cd692c-ab97-4cf8-9f8a-1de733526e74")]
        [Size(-1)]
        [Workspace]
        #endregion
        public byte[] C1AllorsBinary { get; set; }

        #region Allors
        [Id("b4ee673f-bba0-4e24-9cda-3cf993c79a0a")]
        [AssociationId("948aa9e6-5cb3-48dc-a3b7-3f8770269dae")]
        [RoleId("ad456144-a19e-4c89-845b-9391dbc8f372")]
        [Workspace]
        #endregion
        public bool C1AllorsBoolean { get; set; }

        #region Allors
        [Id("ef75cc4e-8787-4f1c-ae5c-73577d721467")]
        [AssociationId("8c8baa81-0c59-485c-b416-c7e6ec972595")]
        [RoleId("610129f7-0c35-4649-9f4b-14698d0d1c77")]
        [Workspace]
        #endregion
        public DateTime C1AllorsDateTime { get; set; }

        #region Allors
        [Id("87eb0d19-73a7-4aae-aeed-66dc9163233c")]
        [AssociationId("96e8dfaf-3e1e-4c59-88f3-d47be6c96b74")]
        [RoleId("43ccd07d-b9c4-465c-b2f9-083a36315e85")]
        [Precision(10)]
        [Scale(2)]
        [Workspace]
        #endregion
        public decimal C1AllorsDecimal { get; set; }

        #region Allors
        [Id("f268783d-42ed-41c1-b0b0-b8a60e30a601")]
        [AssociationId("6ed0694c-a74f-44c3-835b-897f56343576")]
        [RoleId("459d20d8-dadd-44e1-aa8a-396e6eab7538")]
        [Workspace]
        #endregion
        public double C1AllorsDouble { get; set; }

        #region Allors
        [Id("f4920d94-8cd0-45b6-be00-f18d377368fd")]
        [AssociationId("c4202876-b670-4193-a459-3f0376e24c38")]
        [RoleId("2687f5be-eebe-4ffb-a8b2-538134cb6f73")]
        [Indexed]
        [Workspace]
        #endregion
        public int C1AllorsInteger { get; set; }

        #region Allors
        [Id("20713860-8abd-4d71-8ccc-2b4d1b88bce3")]
        [AssociationId("974aa133-255b-431f-a15d-b6a126d362b5")]
        [RoleId("6dc98925-87a7-4959-8095-90eedef0e9a0")]
        [Size(256)]
        [Workspace]
        #endregion
        public string C1AllorsString { get; set; }

        #region Allors
        [Id("a64abd21-dadf-483d-9499-d19aa8e33791")]
        [AssociationId("099e3d39-16b5-431a-853b-942a354c3a52")]
        [RoleId("c186bb2f-8e19-468d-8a01-561384e5187d")]
        [Size(-1)]
        [Workspace]
        #endregion
        public string AllorsStringMax { get; set; }

        #region Allors
        [Id("cef13620-b7d7-4bfe-8d3b-c0f826da5989")]
        [AssociationId("6c18bd8f-9084-470b-9dfe-30263c98771b")]
        [RoleId("2721249b-dadd-410d-b4e0-9d4a48e615d1")]
        [Workspace]
        #endregion
        public Guid C1AllorsUnique { get; set; }

        #region Allors
        [Id("8c198447-e943-4f5a-b749-9534b181c664")]
        [AssociationId("154222cb-0eb8-459d-839c-9c8857bd1c7e")]
        [RoleId("c403f160-6486-4207-b32c-aa9ade27a28c")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public C1[] C1C1Many2Manies { get; set; }

        #region Allors
        [Id("a8e18ea7-cbf2-4ea7-ae14-9f4bcfdb55de")]
        [AssociationId("8a546f48-fc09-48ae-997d-4a6de0cd458a")]
        [RoleId("e6b21250-194b-4424-8b92-221c6d0e6228")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public C1 C1C1Many2One { get; set; }

        #region Allors
        [Id("a0ac5a65-2cbd-4c51-9417-b10150bc5699")]
        [AssociationId("d595765b-5e67-46f2-b19c-c58563dd1ae0")]
        [RoleId("3d121ffa-0ff5-4627-9ec3-879c2830ff04")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public C1[] C1C1One2Manies { get; set; }

        #region Allors
        [Id("79c00218-bb4f-40e9-af7d-61af444a4a54")]
        [AssociationId("2276c942-dd96-41a6-b52f-cd3862c4692f")]
        [RoleId("40ee2908-2556-4bdf-a82f-2ea33e181b91")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public C1 C1C1One2One { get; set; }

        #region Allors
        [Id("f29d4a52-9ba5-40f6-ba99-050cbd03e554")]
        [AssociationId("122dc72f-cc92-440c-84e5-fe8340020c43")]
        [RoleId("608db13d-1778-44a8-94f0-b86fc0f6992d")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public C2[] C1C2Many2Manies { get; set; }

        #region Allors
        [Id("5490dc63-a8f6-4a86-91ef-fef97a86f119")]
        [AssociationId("3f307d57-1f39-4aba-822d-9881cef7223c")]
        [RoleId("66a06e06-95e4-43ad-9b45-56687f8a2051")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public C2 C1C2Many2One { get; set; }

        #region Allors
        [Id("9f6538c2-e6dd-4c27-80ed-2748f645cb95")]
        [AssociationId("3ddac067-46f1-4302-bb1b-aa0e05dd55ae")]
        [RoleId("c749e58c-0f1d-4946-b35d-878221aac72f")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public C2[] C1C2One2Manies { get; set; }

        #region Allors
        [Id("e97fc754-c736-4359-9662-19dce9429f89")]
        [AssociationId("5bd37271-01c0-4cd3-94d5-0284700b3567")]
        [RoleId("392f5a47-f181-475c-b5c9-f0b729c8413f")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public C2 C1C2One2One { get; set; }

        #region Allors
        [Id("94a2b37d-9431-4496-b992-630cda5b9851")]
        [AssociationId("a4a31323-7193-4709-828e-88b2c0f3f8aa")]
        [RoleId("f225d708-c98f-44ff-9ed8-847cb1ddaacb")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I12[] C1I12Many2Manies { get; set; }

        #region Allors
        [Id("bcf4df45-6616-4cdf-8ada-f944f9c7ff1a")]
        [AssociationId("2128418c-6918-4be8-8a02-2bea142b7fc4")]
        [RoleId("b5b4892d-e1d3-4a4b-a8a4-ac6ed0ff930e")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I12 C1I12Many2One { get; set; }

        #region Allors
        [Id("98c5f58b-1777-4d9a-8828-37dbf7051510")]
        [AssociationId("3218ac29-2eac-4dc9-acad-2c708c3df994")]
        [RoleId("51b3b28e-9017-4a1e-b5ba-06a9b14d88d6")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I12[] C1I12One2Manies { get; set; }

        #region Allors
        [Id("b9f2c4c7-6979-40cf-82a2-fa99a5d9e9a4")]
        [AssociationId("911a9327-0237-4254-99a7-afff0d6a0369")]
        [RoleId("50bf56c3-f05f-4172-86e1-aefead4a3a8c")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I12 C1I12One2One { get; set; }

        #region Allors
        [Id("815878f6-16f2-42f2-9b24-f394ddf789c2")]
        [AssociationId("eca51eab-3815-410f-b4c5-f7e2a1318791")]
        [RoleId("39f62f9e-52d3-47c5-8fd4-44e91d9b78be")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I1[] C1I1Many2Manies { get; set; }

        #region Allors
        [Id("7bb216f2-8e9c-4dcd-890b-579130ab0a8b")]
        [AssociationId("531e89ab-a295-4f72-8496-cdd0d8605d37")]
        [RoleId("8af8fbc6-2f59-4026-9093-5b335dfb8b7f")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I1 C1I1Many2One { get; set; }

        #region Allors
        [Id("e0656d9a-75a6-4e59-aaa1-3ff03d440059")]
        [AssociationId("637c5967-fb6c-45d4-81c4-de5559df785f")]
        [RoleId("89e4802f-7c61-4deb-a243-f78e79578082")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I1[] C1I1One2Manies { get; set; }

        #region Allors
        [Id("0e7f529b-bc91-4a40-a7e7-a17341c6bf5b")]
        [AssociationId("1d1374c3-a28d-4904-b98a-3a14ceb2f7ea")]
        [RoleId("da5ccb42-7878-45a9-9350-17f0f0a52fd4")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I1 C1I1One2One { get; set; }

        #region Allors
        [Id("cda97972-84c8-48e3-99d8-fd7c99c5dbc9")]
        [AssociationId("8ef5784c-6f76-431e-b59d-075813ad7863")]
        [RoleId("ce5170b0-347a-49b7-9925-a7a5c5eb2c75")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I2[] C1I2Many2Manies { get; set; }

        #region Allors
        [Id("d0341bed-2732-4bcb-b1bb-9f9589de5d03")]
        [AssociationId("dacd7dfa-6650-438d-b564-49fbf89fea8d")]
        [RoleId("2db69dd4-008b-4a17-aba5-6a050f35f7e3")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I2 C1I2Many2One { get; set; }

        #region Allors
        [Id("82f5fb26-c260-41bc-a784-a2d5e35243bd")]
        [AssociationId("f5329d84-1301-44ea-85b4-dc7d98554694")]
        [RoleId("ca30ba2a-627f-43d1-b467-fe0d7cd015cc")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I2[] C1I2One2Manies { get; set; }

        #region Allors
        [Id("6def7988-4bcf-4964-9de6-c6ede41d5e5a")]
        [AssociationId("75e47fbe-6ce1-4cc1-a20f-51a861df9cc3")]
        [RoleId("e7d1e28d-69ad-4d3a-b35a-2d0aaacb67db")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I2 C1I2One2One { get; set; }

        #region Allors
        [Id("09A6A387-A1B5-4038-B074-3A01C81CBDA2")]
        [Workspace]
        #endregion
        public void ClassMethod() { }

        #region Allors
        [Id("26FE4FD7-68C3-4DDA-8A44-87857B35B000")]
        #endregion
        public void Sum() { }

        #region inherited methods


        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void InterfaceMethod() { }



        public void SuperinterfaceMethod() { }

        #endregion
    }
}
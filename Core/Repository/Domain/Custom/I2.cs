namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0")]
    #endregion
    public partial interface I2 : Object, I12
    {
        #region Allors
        [Id("01d9ff41-d503-421e-93a6-5563e1787543")]
        [AssociationId("359ca62a-c74c-4936-a62d-9b8774174e8d")]
        [RoleId("141b832f-7321-43b8-8033-dbad3f80edc3")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        I2 I2I2Many2One { get; set; }

        #region Allors
        [Id("1f763206-c575-4e34-9e6b-997d434d3f42")]
        [AssociationId("923f6373-cbf8-46b1-9b4b-185015ff59ac")]
        [RoleId("9edd1eb9-2b9a-4375-a669-68c1859eace2")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        C1 I2C1Many2One { get; set; }

        #region Allors
        [Id("23e9c15f-097f-4452-9bac-d7cf2a65134a")]
        [AssociationId("278afe09-d0e7-4a41-a60b-b3a01fd14c93")]
        [RoleId("e538ab5e-80f2-4a34-81e7-c9b92414dda1")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        I12 I2I12Many2One { get; set; }

        #region Allors
        [Id("35040d7c-ab7f-4a99-9d09-e01e24ca3cb9")]
        [AssociationId("d1f0ae79-1820-47a5-8869-496c3578a53d")]
        [RoleId("0d2c6dbe-9bb2-414c-8f19-5381fe69ac64")]
        [Workspace]
        #endregion
        bool I2AllorsBoolean { get; set; }

        #region Allors
        [Id("40b8edb3-e8c4-46c0-855b-4b18e0e8d7f3")]
        [AssociationId("078e1b17-f239-44b2-87d6-6350dd37ac1d")]
        [RoleId("805d7871-bc51-4572-be01-e47ac8fef22a")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        C1[] I2C1One2Manies { get; set; }

        #region Allors
        [Id("49736daf-d0bd-4216-97fa-958cfa21a4f0")]
        [AssociationId("02a80ccd-31c9-422c-8ad9-d96916dd7741")]
        [RoleId("6ac5d426-9156-4467-8a04-85ccb6c964e2")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        C1 I2C1One2One { get; set; }

        #region Allors
        [Id("4f095abd-8803-4610-87f0-2847ddd5e9f4")]
        [AssociationId("5371c058-628e-4a1c-b654-ad0b7013eb17")]
        [RoleId("ec80b71e-a933-4eb3-ab14-00b26c3bc805")]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        #endregion
        decimal I2AllorsDecimal { get; set; }

        #region Allors
        [Id("5ebbc734-23dd-494f-af2d-8e75caaa3e26")]
        [AssociationId("4d6c09d6-5644-47bb-a50a-464350053833")]
        [RoleId("3aab87f3-2eab-4f81-9c1b-fd2e162a93b8")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        I2[] I2I2Many2Manies { get; set; }

        #region Allors
        [Id("62a8a93d-3744-49de-9f9a-9997b6ef4da6")]
        [AssociationId("f9be65e7-6e36-42df-bb85-5198d0c12b74")]
        [RoleId("e3ae23bc-5934-4c0d-a709-adb00110772d")]
        [Size(-1)]
        [Workspace]
        #endregion
        byte[] I2AllorsBinary { get; set; }

        #region Allors
        [Id("663559c4-ef64-4e78-89b4-bfa00691c627")]
        [AssociationId("9513c57f-478a-423e-ba15-b9132bc28cd0")]
        [RoleId("3f03fb6f-b0ba-4c78-b86a-9c4a1c574dd4")]
        [Workspace]
        #endregion
        Guid I2AllorsUnique { get; set; }

        #region Allors
        [Id("6bb406bc-627b-444c-9c16-df9878e05e9c")]
        [AssociationId("16647879-8af1-4f1c-8ef5-2cec85aa31f4")]
        [RoleId("edee2f1c-3e94-45b5-80f4-160faa2074c4")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        I1 I2I1Many2One { get; set; }

        #region Allors
        [Id("81d9eb2f-55a7-4d1c-853d-4369eb691ba5")]
        [AssociationId("db4d3b11-77bd-408e-ad41-4a03272a88e1")]
        [RoleId("bdcffe2b-ffa7-4eb1-be24-8d8ab0b4dce2")]
        [Workspace]
        #endregion
        DateTime I2AllorsDateTime { get; set; }

        #region Allors
        [Id("83dc0581-e04a-4f51-a44e-4fef63d44356")]
        [AssociationId("b1c5cbb7-3d5f-48b8-b182-aa8a0cc3e72a")]
        [RoleId("9598153e-9c1c-438a-a8a8-9822092a6a07")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        I12[] I2I12One2Manies { get; set; }

        #region Allors
        [Id("87499e99-ed77-44c1-89d6-b4f570b6f217")]
        [AssociationId("e5201e06-3fbf-4b9c-aa65-1ee4ee9fabfb")]
        [RoleId("e4c9f00e-7c3d-4b58-92f0-ccce24b55589")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        I12 I2I12One2One { get; set; }

        #region Allors
        [Id("92fdb313-0b90-48f6-b054-a4ab38f880ba")]
        [AssociationId("a45ffec8-5e4e-4b21-9d68-9b0050472ed2")]
        [RoleId("17e159a2-f5a6-4828-9fef-796fcc9085e8")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        C2[] I2C2Many2Manies { get; set; }

        #region Allors
        [Id("9bed0518-1946-4e23-9d4b-e4cda439984c")]
        [AssociationId("7b4a8937-258c-4129-a282-89d5ab924d68")]
        [RoleId("2e78a543-949f-4130-b659-80a9a60ad6ab")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        I1[] I2I1Many2Manies { get; set; }

        #region Allors
        [Id("9f361b97-0b04-496d-ac60-718760c2a4e2")]
        [AssociationId("c51f6fd4-c290-41b6-b594-19e9bcbbee6a")]
        [RoleId("f60f8fa4-4e73-472d-b0b0-67f202c1e969")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        C2 I2C2Many2One { get; set; }

        #region Allors
        [Id("9f91841c-f63f-4ffa-bee6-62e100f3cd15")]
        [AssociationId("3164fd30-297e-4e2a-86d6-fad6754f1d59")]
        [RoleId("7afb53c1-2fe3-44b6-b1d2-d5a9f6100076")]
        [Size(256)]
        [Workspace]
        #endregion
        string I2AllorsString { get; set; }

        #region Allors
        [Id("b39fdd23-d7dd-473f-9705-df2f29be5ffe")]
        [AssociationId("8ddc9cbf-8e5c-4166-a2b0-6127c142da78")]
        [RoleId("7cdd2b76-6c35-4e81-a1da-f5d0a300014b")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        C2[] I2C2One2Manies { get; set; }

        #region Allors
        [Id("b640bf16-0dc0-4203-aa76-f456371239ae")]
        [AssociationId("257fa0c6-43ea-4fe9-8142-dbc172d1e138")]
        [RoleId("26deb364-bd5e-4b5d-b28a-19689ab3c00d")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        I1 I2I1One2One { get; set; }

        #region Allors
        [Id("bbb01166-2671-4ca1-8b1e-12e6ae8aeb03")]
        [AssociationId("ee0766c7-0ef6-4ca0-b4a1-c399bc8df823")]
        [RoleId("d8f011c4-3057-4384-9045-9c34b13db5c3")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        I1[] I2I1One2Manies { get; set; }

        #region Allors
        [Id("cb9f21e0-a841-45de-8ba4-991b4ceca616")]
        [AssociationId("1127ff1b-1657-4e18-bdc9-bc90cd8a3c15")]
        [RoleId("d838e921-ff63-4e4f-afd8-42dc29d23555")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        I12[] I2I12Many2Manies { get; set; }

        #region Allors
        [Id("cc4c704c-ab7e-45d4-baa9-b67cfff9448e")]
        [AssociationId("d15cb643-1ace-4dfe-b0af-e02e4273bbbb")]
        [RoleId("12c2c263-7839-4734-9307-bcde6930a2b7")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        I2 I2I2One2One { get; set; }

        #region Allors
        [Id("d30dd036-6d28-48df-873b-3a76da8c029e")]
        [AssociationId("012e0afc-ebc7-4ae4-9fa0-49c72f3daebf")]
        [RoleId("69c063b7-156f-4b7f-89eb-10c7eaf39ad5")]
        [Workspace]
        #endregion
        int I2AllorsInteger { get; set; }

        #region Allors
        [Id("deb9cbd3-386f-4599-802c-be50945b9f1d")]
        [AssociationId("3fcc8e73-5f3c-4ce0-8f45-daa813278d7e")]
        [RoleId("c7d68f0d-24b1-40c9-9431-78763b776bee")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        I2[] I2I2One2Manies { get; set; }

        #region Allors
        [Id("f364c9fe-ad36-4305-80fd-4921451c70a5")]
        [AssociationId("db6935b0-684c-48ce-97d0-6b7183a73adb")]
        [RoleId("6ed084f6-8809-46d9-a3ec-4b086ddafb0a")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        C1[] I2C1Many2Manies { get; set; }

        #region Allors
        [Id("f85c2d97-10b9-478d-9b82-2700d95d5cb1")]
        [AssociationId("bfb08e5e-afc6-4f27-975f-5fb9af5bacc4")]
        [RoleId("666c65ad-8bf7-40be-a51a-e69d3e0bfe01")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        C2 I2C2One2One { get; set; }

        #region Allors
        [Id("fbad33e7-ede1-41fc-97e9-ddf33a0f6459")]
        [AssociationId("c138d77b-e8bf-4945-962e-f74e338caad4")]
        [RoleId("12ea1f33-0eed-4476-9cab-1fd62ed146a3")]
        [Workspace]
        #endregion
        double I2AllorsDouble { get; set; }
    }
}

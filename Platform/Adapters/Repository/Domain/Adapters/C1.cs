namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("7041c691-d896-4628-8f50-1c24f5d03414")]
    #endregion
    public partial class C1 : System.Object, I1, I12
    {
        #region inherited properties
        public I34[] I1I34one2manies { get; set; }

        public I2[] I1I2one2manies { get; set; }

        public I2 I1I2many2one { get; set; }

        public C2 I1C2many2one { get; set; }

        public C2 I1C2one2one { get; set; }

        public decimal I1DecimalBetweenA { get; set; }

        public S1 I1S1one2one { get; set; }

        public I12 I1I12many2one { get; set; }

        public string I1AllorsString { get; set; }

        public DateTime I1DateTimeLessThan { get; set; }

        public C2[] I1C2one2manies { get; set; }

        public string I1StringLarge { get; set; }

        public double I1FloatLessThan { get; set; }

        public DateTime I1AllorsDateTime { get; set; }

        public C1 I1C1many2one { get; set; }

        public I12 I1I12one2one { get; set; }

        public decimal I1DecimalGreaterThan { get; set; }

        public C1 I1C1one2one { get; set; }

        public I2[] I1I2many2manies { get; set; }

        public int I1IntegerBetweenA { get; set; }

        public I34 I1I34many2one { get; set; }

        public double I1FloatBetweenA { get; set; }

        public int I1IntegerLessThan { get; set; }

        public int I1AllorsInteger { get; set; }

        public S2 I1S2one2one { get; set; }

        public bool I1AllorsBoolean { get; set; }

        public I1 I1I1many2one { get; set; }

        public C1[] I1C1many2manies { get; set; }

        public I2 I1I2one2one { get; set; }

        public decimal I1AllorsDecimal { get; set; }

        public S1[] I1S1many2manies { get; set; }

        public DateTime I1DateTimeGreaterThan { get; set; }

        public I34[] I1I34many2manies { get; set; }

        public I34 I1I34one2one { get; set; }

        public I1[] I1I1one2manies { get; set; }

        public I1[] I1I1many2manies { get; set; }

        public S2[] I1S2many2manies { get; set; }

        public I12[] I1I12many2manies { get; set; }

        public string I1StringEquals { get; set; }

        public I12[] I1I12one2manies { get; set; }

        public S2[] I1S2one2manies { get; set; }

        public C2[] I1C2many2manies { get; set; }

        public byte[] I1AllorsBinary { get; set; }

        public decimal I1DecimalBetweenB { get; set; }

        public double I1FloatGreaterThan { get; set; }

        public int I1IntegerBetweenB { get; set; }

        public DateTime I1DateTimeBetweenA { get; set; }

        public double I1AllorsDouble { get; set; }

        public S1[] I1S1one2manies { get; set; }

        public I1 I1I1one2one { get; set; }

        public int I1IntegerGreaterThan { get; set; }

        public S1 I1S1many2one { get; set; }

        public double I1FloatBetweenB { get; set; }

        public decimal I1DecimalLessThan { get; set; }

        public DateTime I1DateTimeBetweenB { get; set; }

        public Guid I1AllorsUnique { get; set; }

        public C1[] I1C1one2manies { get; set; }

        public S2 I1S2many2one { get; set; }

        public decimal S1AllorsDecimal { get; set; }

        public int S1AllorsInteger { get; set; }

        public byte[] S1AllorsBinary { get; set; }

        public Guid S1AllorsUnique { get; set; }

        public string S1StringLarge { get; set; }

        public S2 S1S2many2one { get; set; }

        public S2[] S1S2one2manies { get; set; }

        public double S1AllorsDouble { get; set; }

        public string S1AllorsString { get; set; }

        public C1 S1C1many2one { get; set; }

        public C1 S1C1one2one { get; set; }

        public bool S1AllorsBoolean { get; set; }

        public C1[] S1C1many2manies { get; set; }

        public S2[] S1S2many2manies { get; set; }

        public S2 S1S2one2one { get; set; }

        public DateTime S1AllorsDateTime { get; set; }

        public C1[] S1C1one2manies { get; set; }

        public string Name { get; set; }

        public double S1234AllorsDouble { get; set; }

        public decimal S1234AllorsDecimal { get; set; }

        public int S1234AllorsInteger { get; set; }

        public S1234 S1234many2one { get; set; }

        public C2 S1234C2one2one { get; set; }

        public C2[] S1234C2many2manies { get; set; }

        public S1234[] S1234one2manies { get; set; }

        public C2[] S1234C2one2manies { get; set; }

        public S1234[] S1234many2manies { get; set; }

        public string ClassName { get; set; }

        public DateTime S1234AllorsDateTime { get; set; }

        public S1234 S1234one2one { get; set; }

        public C2 S1234C2many2one { get; set; }

        public string S1234AllorsString { get; set; }

        public bool S1234AllorsBoolean { get; set; }

        public bool I12AllorsBoolean { get; set; }

        public int I12AllorsInteger { get; set; }

        public I34[] I12I34one2manies { get; set; }

        public C3 C3many2one { get; set; }

        public C2 I12C2many2one { get; set; }

        public double I12AllorsDouble { get; set; }

        public I34 I12I34many2one { get; set; }

        public I34[] I12I34many2manies { get; set; }

        public C3 I12C3one2one { get; set; }

        public C2[] I12C2many2manies { get; set; }

        public decimal I12AllorsDecimal { get; set; }

        public C2 I12C2one2one { get; set; }

        public C3[] I12C3one2manies { get; set; }

        public C3[] I12C3many2manies { get; set; }

        public string PrefetchTest { get; set; }

        public DateTime I12AllorsDateTime { get; set; }

        public string I12AllorsString { get; set; }

        public I34 I12I34one2one { get; set; }

        public C2[] I12C2one2manies { get; set; }

        public string S12AllorsString { get; set; }

        public DateTime S12AllorsDateTime { get; set; }

        public C2[] S12C2many2manies { get; set; }

        public C2 S12C2many2one { get; set; }

        public C2 S12C2one2one { get; set; }

        public C2[] S12C2one2manies { get; set; }

        public bool S12AllorsBoolean { get; set; }

        public double S12AllorsDouble { get; set; }

        public int S12AllorsInteger { get; set; }

        public decimal S12AllorsDecimal { get; set; }

        #endregion

        #region Allors
        [Id("024db9e0-b51f-4d8b-a2d0-0a041dcebd62")]
        [AssociationId("0498b5fc-62c0-4f80-ac09-4567929e387f")]
        [RoleId("1b372270-7d7e-46ba-9840-f3143bd38afe")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        public decimal C1DecimalBetweenA { get; set; }
        #region Allors
        [Id("03fc18eb-46be-411a-9b1e-4a1953843d92")]
        [AssociationId("dbd53cdf-83bf-4401-a1ef-f239984892e4")]
        [RoleId("04d17334-dac3-48b3-83fe-214436908185")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public I2 C1I2one2one { get; set; }
        #region Allors
        [Id("0aefa669-9c8a-4fbf-98a4-230d93df8341")]
        [AssociationId("f969b93b-0e2d-4b39-889b-c079e37ef5fe")]
        [RoleId("ef5ba41b-41f2-44ba-8bc0-079d738a9463")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        public decimal C1DecimalBetweenB { get; set; }
        #region Allors
        [Id("0e57dd07-bb58-4620-a898-3060af007f60")]
        [AssociationId("2979d83b-03bf-4ef0-a5a5-61ffd01505f7")]
        [RoleId("3f2df5f5-325f-48d0-becf-e1cb49b44770")]
        [Size(256)]
        #endregion
        public string Argument { get; set; }
        #region Allors
        [Id("10df748e-3b9c-48f4-82dc-85498f199567")]
        [AssociationId("775863d7-ea9a-4b2c-a8f2-792bbf88288b")]
        [RoleId("53e32337-f227-4a13-8fda-fa9cd0f0d2e6")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public S1[] C1S1one2manies { get; set; }
        #region Allors
        [Id("13761939-4842-45ba-af73-2a5976e2d6e3")]
        [AssociationId("94f67011-ef73-44bc-bcf4-6c45b793dec3")]
        [RoleId("84ab3497-a2c7-4a60-a278-b24686e6a9fa")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public I12 C1I12one2one { get; set; }
        #region Allors
        [Id("20713860-8abd-4d71-8ccc-2b4d1b88bce3")]
        [AssociationId("91cd1f90-9173-44ed-899a-c2d8f29979af")]
        [RoleId("73131747-7ce6-4801-a6ad-1a6f65b00ebe")]
        [Size(256)]
        #endregion
        public string C1AllorsString { get; set; }
        #region Allors
        [Id("2cd8b843-f1f5-413d-9d6d-0d2b9b3c5cf6")]
        [AssociationId("aeae79d4-1981-4784-be1c-09937fcb4f81")]
        [RoleId("443257a7-ca1d-4e34-bb25-00ad68debf48")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public C1 C1C1many2one { get; set; }
        #region Allors
        [Id("2cee32ad-4e62-4112-9775-f84b0298e93a")]
        [AssociationId("9d262256-759f-4809-9c99-a8f6f41990d1")]
        [RoleId("302ba7eb-b6f3-41e3-bc26-4927bec900a7")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public S2 C1S2many2one { get; set; }
        #region Allors
        [Id("2fa10f1e-d7f6-4f75-92a8-15d7b02b8c19")]
        [AssociationId("287d5a30-a910-434d-a005-9af604fe6fd2")]
        [RoleId("f199e3bc-a801-418b-99ea-7e693fe86f2b")]
        #endregion
        public double C1FloatBetweenA { get; set; }
        #region Allors
        [Id("2fc66f19-7fd4-4dc1-95ef-7931864ad083")]
        [AssociationId("55e9a302-365d-4ae8-b735-8ebe16d487df")]
        [RoleId("37ec9021-267f-4722-b651-2a31be381f88")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public C1 Many2One { get; set; }
        #region Allors
        [Id("2ff1c9ba-0017-466e-9f11-776086e6d0b0")]
        [AssociationId("b93df650-406c-4eb6-8352-dfdb6db0eefc")]
        [RoleId("b2faf733-aba6-46c6-84a0-bbf6aafeb613")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        public C1[] C1C1many2manies { get; set; }
        #region Allors
        [Id("3673e4f6-8b40-44e7-be25-d73907b5806a")]
        [AssociationId("f9ba3e92-f1df-418d-94de-9ad0ddcbd24a")]
        [RoleId("fc639289-9a1e-449e-832a-9b7dda4a80be")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public S1[] C1S1many2manies { get; set; }
        #region Allors
        [Id("392e8c95-bbfc-4d24-b751-36c17a7b0ee6")]
        [AssociationId("180c4969-ae4d-4c94-a65a-72ad2c827ffd")]
        [RoleId("b00db511-b17b-41fc-a670-4801a17343f5")]
        #endregion
        public double C1FloatBetweenB { get; set; }
        #region Allors
        [Id("3fea182f-07b0-4c36-8170-960b484801f6")]
        [AssociationId("7e11a549-e213-4e62-8fe5-db6f591ad355")]
        [RoleId("dd5acc79-e2c5-4c0c-b054-4fd6deae1c39")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public I1 C1I1one2one { get; set; }
        #region Allors
        [Id("49970761-ebe1-4623-a822-5ee1d1f3fafc")]
        [AssociationId("a95f82ab-19e7-464b-9a5a-9458e22c6da3")]
        [RoleId("c3cf4120-65e4-4069-a357-714774ce208f")]
        #endregion
        public int C1IntegerLessThan { get; set; }
        #region Allors
        [Id("4b970db5-d0ec-4765-9f9b-6e9aafc9dbcc")]
        [AssociationId("f3f72266-709b-4ca7-a40b-ec47616c1758")]
        [RoleId("42b7d196-d105-4e0b-ba8c-8b95dd1c9039")]
        [Size(100000)]
        #endregion
        public string C1StringLarge { get; set; }
        #region Allors
        [Id("4c0362ad-4d0e-4e57-a057-1852ddd8eed8")]
        [AssociationId("c3103e76-c1e2-49d5-bea8-94eccf855934")]
        [RoleId("34ad3d07-3545-44b4-96f0-55d186a933a5")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public I2[] C1I2one2manies { get; set; }
        #region Allors
        [Id("4c776502-77d7-45d9-b101-62dee27c0c2e")]
        [AssociationId("e7ae4683-03c7-4c79-96ed-0b0cfea26672")]
        [RoleId("bcd957f3-a9bd-41eb-bb2f-2ae595d6e5f1")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public C1 C1C1one2one { get; set; }
        #region Allors
        [Id("4c95279f-fb68-49d1-b9c2-27c612c4c28e")]
        [AssociationId("d4d5cb0d-098a-41bd-80ed-7e0b6f9a6038")]
        [RoleId("47dfabc0-a7b6-4152-aa74-f42c2cde35ac")]
        #endregion
        public double C1FloatGreaterThan { get; set; }
        #region Allors
        [Id("4dab4e16-b8a2-46c1-949d-62aead9a9c9f")]
        [AssociationId("0fc84cfc-8dd2-47e8-bafa-08163a12aa44")]
        [RoleId("aa0d80fc-2aa0-43a7-bad1-2b08e6d95b2d")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public I2 C1I2many2one { get; set; }
        #region Allors
        [Id("599420c6-0757-49f6-8ae7-4cb0714ca791")]
        [AssociationId("74d7cb73-963f-48d1-9519-5578337ece83")]
        [RoleId("4b4223f6-5c83-40a7-8ecb-6d403d501b0d")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public I12 C1I12many2one { get; set; }
        #region Allors
        [Id("6459deba-24e6-4867-a555-75f672f33893")]
        [AssociationId("9b5c74c8-6b73-494e-8e84-6163463e6860")]
        [RoleId("4bf1e140-447b-4d05-95ab-4c7b5b27da46")]
        #endregion
        public DateTime C1DateTimeLessThan { get; set; }
        #region Allors
        [Id("65cff232-60fb-4ed9-9f36-2aebbdc3fc79")]
        [AssociationId("6c51970a-30b0-4881-b63b-ce51a7a64e03")]
        [RoleId("80e71d74-6abc-4503-bddf-b86da1848ac3")]
        [Indexed]
        [Size(-1)]
        #endregion
        public byte[] IndexedMaxBinary { get; set; }
        #region Allors
        [Id("68fa3256-c5ba-42bb-b424-9349f1c6efa3")]
        [AssociationId("2984cad9-47c1-4606-9f5f-5fc2c0f40a3d")]
        [RoleId("307ff3be-adf5-4914-9eaf-974161ff61f8")]
        [Indexed]
        [Size(-1)]
        #endregion
        public string IndexedMaxString { get; set; }
        #region Allors
        [Id("6aadb05d-6b80-47c5-b625-18b86e762c94")]
        [AssociationId("5ea6c4e9-2b4e-470d-9052-f67f80975268")]
        [RoleId("c07633eb-a03e-44ba-ae2f-cd4bfa3898f5")]
        #endregion
        public DateTime C1DateTimeBetweenA { get; set; }
        #region Allors
        [Id("71abe169-dea4-4834-8d37-34cbcffa6cee")]
        [AssociationId("d3c1edf3-a518-43fc-874c-034b96a4315f")]
        [RoleId("e20dbd0c-169d-4aa3-8bdf-a1b6888fc809")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public C2[] C1C2many2manies { get; set; }
        #region Allors
        [Id("724f101c-db45-44f3-b9ca-c8f3b0c28d29")]
        [AssociationId("0eeff5f8-e436-47f9-bd61-f5d4484fb609")]
        [RoleId("79e7fd13-f39a-48d1-a0f0-c67b1565e9f2")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public S1 C1S1many2one { get; set; }
        #region Allors
        [Id("79fbfbc3-50e3-4e45-a5bf-8a253bb6f0c6")]
        [AssociationId("441a45da-fe17-4aab-8ba7-8f8471d08138")]
        [RoleId("456eb032-135b-4fc9-a154-f17f514b2730")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public I1[] C1I1many2manies { get; set; }
        #region Allors
        [Id("7b058b52-dc6b-4f8c-af72-28c9b0c0fde4")]
        [AssociationId("39b2eaf5-a229-4a08-9adb-7046ee1a8ee4")]
        [RoleId("c641a991-9526-43bd-b064-a47e0b490a06")]
        #endregion
        public double C1FloatLessThan { get; set; }
        #region Allors
        [Id("7fce490e-78af-46a9-a87d-de233073ab3c")]
        [AssociationId("f974c30a-b37f-4ce3-9cc6-a1931d0455f8")]
        [RoleId("40b9821b-bb3d-4dc7-b62e-fdf5f4a9c03e")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public I1 C1I1many2one { get; set; }
        #region Allors
        [Id("8679b3aa-cdad-4ee1-b4fb-edcefd660edb")]
        [AssociationId("0bb934fe-7da6-4a71-be93-c0a9708d73b5")]
        [RoleId("f3d4ce2d-20d7-47ff-98ea-198fccd853d8")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        public decimal C1DecimalGreaterThan { get; set; }
        #region Allors
        [Id("87eb0d19-73a7-4aae-aeed-66dc9163233c")]
        [AssociationId("4572bf9c-50ea-4783-ab76-b88b7c46adbc")]
        [RoleId("5568fb11-4cda-4528-82e3-9ebc8d2c4d2c")]
        [Precision(10)]
        [Scale(2)]
        #endregion
        public decimal C1AllorsDecimal { get; set; }
        #region Allors
        [Id("92cbd254-9763-41e1-9c73-4a378aab4b8e")]
        [AssociationId("b45956b3-634f-4620-83e2-4606c6002e47")]
        [RoleId("a8cd0a6a-3d77-48ab-85f4-5064e0e57074")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public S2 C1S2one2one { get; set; }
        #region Allors
        [Id("934421bd-6cac-4e99-9457-43117a9f3c52")]
        [AssociationId("c5f79c2e-99bf-42fc-aecd-344f43dc01fe")]
        [RoleId("9362ace7-c31e-4077-a1eb-97548d2392a8")]
        #endregion
        public DateTime C1DateTimeBetweenB { get; set; }
        #region Allors
        [Id("97f31053-0e7b-42a0-90c2-ce6f09c56e86")]
        [AssociationId("86e72d26-4ea6-4637-9959-c0f4aceba0e6")]
        [RoleId("850808bb-fac8-4a90-af6c-321e4722f92f")]
        [Size(-1)]
        #endregion
        public byte[] C1AllorsBinary { get; set; }
        #region Allors
        [Id("9d8c9863-dd8d-4c85-a5e6-58042ff3619d")]
        [AssociationId("e8f5d99c-7dfb-44b4-8951-9cc37d84f5a6")]
        [RoleId("708c391d-94a5-4c08-8cdc-ecb74cf7ac76")]
        #endregion
        public DateTime C1DateTimeGreaterThan { get; set; }
        #region Allors
        [Id("9df07ff8-7a29-4d41-a08e-d46efdd15e32")]
        [AssociationId("da3963b0-9ac1-4bab-bfac-5e29712b563e")]
        [RoleId("e77e8e25-491c-448e-8792-6ba90c1a374a")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public S1 C1S1one2one { get; set; }
        #region Allors
        [Id("ab6d11cc-ec86-4828-8875-2e9a779ba627")]
        [AssociationId("47893ddf-f5c4-4f72-8be1-18366fcb190e")]
        [RoleId("0ca9b283-3af4-421b-acf5-f4c26794461f")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public C1[] C1C1one2manies { get; set; }
        #region Allors
        [Id("ac0cfbe2-a2ff-4781-83aa-5d4e459d939f")]
        [AssociationId("441e5063-314e-410f-a478-74f643f18924")]
        [RoleId("59e3f607-eb50-474a-9738-9aa08bacb2f4")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public I1[] C1I1one2manies { get; set; }
        #region Allors
        [Id("ac2096a9-b58b-41d3-a1d3-920f0b41cb2f")]
        [AssociationId("a75af8b0-b169-4dc0-af9f-9e779ed8ed79")]
        [RoleId("b6f608f0-08a8-4af0-bb73-29508c1e7046")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public C2 C1C2many2one { get; set; }
        #region Allors
        [Id("ad1b1fb1-b30c-431f-b975-5505f6311a18")]
        [AssociationId("8def87bd-ae8e-46b5-a98a-a44f49c71650")]
        [RoleId("181907ac-c5a3-446a-85af-b624c496e6c4")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public I12[] C1I12one2manies { get; set; }
        #region Allors
        [Id("b2071550-cc1b-4543-b98f-006e7564a74b")]
        [AssociationId("313bf740-637d-4f6a-af82-74a488bde357")]
        [RoleId("2a00cf34-3dda-4c12-9adc-10843460df40")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public S2[] C1S2many2manies { get; set; }
        #region Allors
        [Id("b4e3d3d1-65b2-4803-954f-1e09f39e5594")]
        [AssociationId("acace7a9-505a-4571-a984-9f92b73a974b")]
        [RoleId("e59784f1-7943-4f5c-a8ca-30b3483949d0")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public C2 C1C2one2one { get; set; }
        #region Allors
        [Id("b4ee673f-bba0-4e24-9cda-3cf993c79a0a")]
        [AssociationId("228c0dcf-0f5a-4cde-8769-f91f2b4cc58d")]
        [RoleId("1cb9182b-4cf7-4050-a4d2-4eea351712ab")]
        #endregion
        public bool C1AllorsBoolean { get; set; }
        #region Allors
        [Id("c58903fb-443b-4de9-b010-15f3f09ff5df")]
        [AssociationId("4fd8ee39-c718-4776-aa82-2c44f0926ab5")]
        [RoleId("32595220-456a-411b-8cff-d0ab6622ba0d")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public I12[] C1I12many2manies { get; set; }
        #region Allors
        [Id("c92fbc53-ae5e-450e-9681-ca17833e6e2f")]
        [AssociationId("4f0d537c-7880-475e-b924-c6ef99cd4f29")]
        [RoleId("40c8a502-826d-4153-a373-581e2a24d4cd")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public I2[] C1I2many2manies { get; set; }
        #region Allors
        [Id("cef13620-b7d7-4bfe-8d3b-c0f826da5989")]
        [AssociationId("b282abf5-a18a-484f-9c51-094c5ab3273c")]
        [RoleId("6ff183b3-1859-4489-b459-8bddc8f50c8c")]
        #endregion
        public Guid C1AllorsUnique { get; set; }
        #region Allors
        [Id("d3f73a6d-8f95-44c6-bbc8-ddc468b803f7")]
        [AssociationId("70e33bc7-18da-4a9d-ab35-17f5f58bb86c")]
        [RoleId("65f82cbe-390c-4f01-bead-04ce52e404d3")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        public C3 C1C3one2one { get; set; }
        #region Allors
        [Id("da4d6a24-6b0f-4841-b355-80ee1ba10c59")]
        [AssociationId("1c8f32c3-c0a3-41c0-998a-7b933c8985c2")]
        [RoleId("fff5e5d3-82da-4bf6-86c0-9c2cc048b6f6")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        public C3[] C1C3many2manies { get; set; }
        #region Allors
        [Id("dc55a574-5546-4a68-b886-706c39bc4e80")]
        [AssociationId("8c8974cb-bd1c-4508-84b5-24f1b2320ed0")]
        [RoleId("e6426399-ae58-4565-94c9-bf610f39ff01")]
        [Size(256)]
        #endregion
        public string C1StringEquals { get; set; }
        #region Allors
        [Id("e2153298-73b0-4f5f-bba0-00c832b044b3")]
        [AssociationId("45d13e0f-e377-4579-9fcd-159de81707b3")]
        [RoleId("1e0a3de9-1d97-4174-857a-ce2dc447a5c8")]
        #endregion
        public int C1IntegerGreaterThan { get; set; }
        #region Allors
        [Id("e3af3413-4631-4052-ac57-955651a319fc")]
        [AssociationId("d56f0a2e-0e55-4d9a-a5ff-e67186c3ff0a")]
        [RoleId("f3d25497-5895-4a84-a10c-6537b232890d")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        public C3 C3may2one { get; set; }
        #region Allors
        [Id("e3dedb1d-6738-46f7-8a25-77213c90a8f9")]
        [AssociationId("5641c330-6cdb-4c2d-82cf-59520bc4a14f")]
        [RoleId("233d4e00-4f1c-4b55-aef5-b13dd4f9861c")]
        #endregion
        public int C1IntegerBetweenB { get; set; }
        #region Allors
        [Id("ef75cc4e-8787-4f1c-ae5c-73577d721467")]
        [AssociationId("b61679b1-3118-4353-969a-8e0406d6b7db")]
        [RoleId("d6e19a7c-23a8-4450-a0b7-c882111fb087")]
        #endregion
        public DateTime C1AllorsDateTime { get; set; }
        #region Allors
        [Id("ef909fec-7a03-4a3c-a3f4-6097a51ff1f0")]
        [AssociationId("ecf38e01-7977-459a-9beb-e5c1a32d4d6d")]
        [RoleId("08030ead-35df-460c-be3e-a707d8e89ffb")]
        #endregion
        public int C1IntegerBetweenA { get; set; }
        #region Allors
        [Id("f268783d-42ed-41c1-b0b0-b8a60e30a601")]
        [AssociationId("3ef93ecb-bc60-4320-b735-4a5bf524a8e9")]
        [RoleId("3492623a-7fe3-48ec-a95e-2690ced58f88")]
        #endregion
        public double C1AllorsDouble { get; set; }
        #region Allors
        [Id("f39739d2-e8fc-406e-be6a-c92acee07686")]
        [AssociationId("19596842-fb74-4af2-9be0-3da44d4a8e2c")]
        [RoleId("fbf79604-b7c9-41c1-8f70-846b41f0cace")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public C2[] C1C2one2manies { get; set; }
        #region Allors
        [Id("f47b9392-1391-416e-9a49-23ab0627133e")]
        [AssociationId("96459a63-0d8f-4cc5-a3f0-5b3268756c5c")]
        [RoleId("b25a2c55-e620-48b4-8d29-dc3f94b493ec")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public S2[] C1S2one2manies { get; set; }
        #region Allors
        [Id("f4920d94-8cd0-45b6-be00-f18d377368fd")]
        [AssociationId("0466f49d-b882-4a87-9f77-2f3b392a1d29")]
        [RoleId("b6c919ad-97c9-4d08-a3ba-179d3c8a313c")]
        [Indexed]
        #endregion
        public int C1AllorsInteger { get; set; }
        #region Allors
        [Id("fc56ca04-9737-4b51-939e-4854e5507953")]
        [AssociationId("de7f8997-c48e-48b9-9ea2-43fb6597a67b")]
        [RoleId("27f4e9f4-07e2-462d-917c-66fe659b789a")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        public decimal C1DecimalLessThan { get; set; }
        #region Allors
        [Id("fee2d1a8-bb65-4bfe-b25f-407c629dec18")]
        [AssociationId("61b277c2-6e17-4b2f-b642-cf0a711f3edd")]
        [RoleId("1cf5bfab-6bf7-4e04-bedd-0ac72ae6b30b")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        public C3[] C1C3one2manies { get; set; }


        #region inherited methods
        #endregion

    }
}

namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("fefcf1b6-ac8f-47b0-bed5-939207a2833e")]
    #endregion
	public partial interface I1 :  Object, S1, S1234 
    {


        #region Allors
        [Id("00a70a04-4fc8-4585-83ce-0f7f0e0db7ab")]
        [AssociationId("50a8e3e6-093e-46b9-9818-456507ca86a9")]
        [RoleId("9ffd70c8-f440-47b7-9e24-3b561b03f001")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        I34[] I1I34one2manies { get; set; }


        #region Allors
        [Id("036e3008-07f8-4a15-bca2-eb21837778a0")]
        [AssociationId("10444cb0-848c-4253-b7e2-4323cef98699")]
        [RoleId("6a8eefe5-fe39-4345-bd5e-f97851c4a086")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        I2[] I1I2one2manies { get; set; }


        #region Allors
        [Id("0b0f8c40-266c-424a-8276-0e8e2673d1a7")]
        [AssociationId("c2ead9ab-c2c3-4fc0-8285-0f07b6351384")]
        [RoleId("3ea290ce-f353-418c-b15a-ece3b29a7ec7")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        I2 I1I2many2one { get; set; }


        #region Allors
        [Id("0d63e4c7-28de-4d47-8f23-7ee1d3606751")]
        [AssociationId("913230df-5047-4e95-9706-84d78c1270aa")]
        [RoleId("73c62ba0-88b8-46eb-a2b7-350b5ba3fff9")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        C2 I1C2many2one { get; set; }


        #region Allors
        [Id("14a93943-13f6-481d-98c7-19fb55625af9")]
        [AssociationId("729272d3-09c5-4e55-8d65-126f03e99fd2")]
        [RoleId("4009851b-5819-414e-a0f2-6d9141ecdfa8")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        C2 I1C2one2one { get; set; }


        #region Allors
        [Id("19e09e31-31ac-44cc-ad1e-a015f4747aeb")]
        [AssociationId("c9cc42f1-4a1b-4c93-a82b-0e19ed88242a")]
        [RoleId("908f646c-af46-49b2-981c-b0fe97986f0c")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        decimal I1DecimalBetweenA { get; set; }


        #region Allors
        [Id("1d41941b-3b1d-48d7-bc6f-e8811cbd96e4")]
        [AssociationId("20d5d1d7-3451-4831-870d-4dabb4ed53b0")]
        [RoleId("43142b12-ccbf-4675-8893-bef0b48ffd4b")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        S1 I1S1one2one { get; set; }


        #region Allors
        [Id("28b92468-27e5-4471-b3a5-37b8ec4f794e")]
        [AssociationId("8ccd89ce-ae46-45f3-b06b-6a6f5415ca39")]
        [RoleId("35f9c368-08b8-4516-9f40-d659ba17e35f")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        I12 I1I12many2one { get; set; }


        #region Allors
        [Id("28ceffc2-c776-4a0a-9825-a6d1bcb265dc")]
        [AssociationId("9dc2a58d-4e7d-418d-98ba-3ffe725de9ad")]
        [RoleId("ba8195d0-5c26-4402-97d7-54024f9f547c")]
        [Size(256)]
        #endregion
        string I1AllorsString { get; set; }


        #region Allors
        [Id("29244f33-6d79-44aa-9ed2-8cc01b5070b7")]
        [AssociationId("c2007753-31aa-4e75-91cd-7e581a593bc4")]
        [RoleId("a9c4e920-08b2-45e2-bb28-faf2ee495067")]
        #endregion
        DateTime I1DateTimeLessThan { get; set; }


        #region Allors
        [Id("2cd562b6-7f54-49af-b853-2244f10ec60e")]
        [AssociationId("826b8178-01ec-431e-9f9d-6c1b47bac805")]
        [RoleId("34dd065a-9973-44ca-8106-9ebc83c9c879")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        C2[] I1C2one2manies { get; set; }


        #region Allors
        [Id("2e98ec7e-486f-4b96-ac15-5149fe6c4e0e")]
        [AssociationId("7a9005f0-c244-4797-98bd-7ea613edb0fe")]
        [RoleId("46c7a9a7-5e8e-4177-8835-be8fbd886d9e")]
        [Size(100000)]
        #endregion
        string I1StringLarge { get; set; }


        #region Allors
        [Id("2f739fa2-c169-4721-8d2d-79f27a6e57c6")]
        [AssociationId("31b9b9af-df6c-425e-835c-ceb2d512afb6")]
        [RoleId("6aee72d6-be71-4560-baf2-97e64cfd3eb8")]
        #endregion
        double I1FloatLessThan { get; set; }


        #region Allors
        [Id("32fc21cc-4be7-4a0e-ac71-df135be95e68")]
        [AssociationId("cd473438-a39c-4c21-acf3-9394a0037434")]
        [RoleId("6131d63e-e7fa-4859-bbd0-be684f203a3e")]
        #endregion
        DateTime I1AllorsDateTime { get; set; }


        #region Allors
        [Id("33f13167-3a14-4b06-a1d8-87076918b285")]
        [AssociationId("d138c296-e332-414c-9dc4-9eeff746e7ec")]
        [RoleId("13fa334e-a315-4dbb-9d2a-1a8979254754")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        C1 I1C1many2one { get; set; }


        #region Allors
        [Id("381c61c1-312d-47ea-8314-8ac051378a81")]
        [AssociationId("fa0426fa-bf13-4437-928c-691d60b67472")]
        [RoleId("3cfdc695-8713-426a-8050-87de8e608f44")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        I12 I1I12one2one { get; set; }


        #region Allors
        [Id("39f1c13c-7d77-429f-ac9b-1491e949aa3a")]
        [AssociationId("b7839acd-88b5-4b28-b4e4-0e9f107e9ffd")]
        [RoleId("81f5b98b-c108-41d3-866b-879e00a431cb")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        decimal I1DecimalGreaterThan { get; set; }


        #region Allors
        [Id("4401d0b8-2450-45a8-92d2-ff3961e129b2")]
        [AssociationId("a4c7fc6f-75ee-43cc-ae9d-cf9d95aa0c37")]
        [RoleId("3ba6e068-4da7-4cf0-a901-ac894dde7085")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        C1 I1C1one2one { get; set; }


        #region Allors
        [Id("4a30d40e-ade3-4304-b17b-185abc8b7fde")]
        [AssociationId("74d59fed-2449-4b6e-8667-0a93cebc1368")]
        [RoleId("3e854566-3741-4067-b86f-5977a40c9fc8")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        I2[] I1I2many2manies { get; set; }


        #region Allors
        [Id("518da995-1f6b-4632-94f1-11cea5e72717")]
        [AssociationId("11fe4c27-f656-46aa-bc30-341fe88d682a")]
        [RoleId("730be644-122c-45d0-96b3-73c68a9846c2")]
        #endregion
        int I1IntegerBetweenA { get; set; }


        #region Allors
        [Id("528ece9c-81f2-4ea4-8d42-50d9a3fe1eea")]
        [AssociationId("737aa4c0-5bdb-4871-b79d-1c57b5373835")]
        [RoleId("0c896206-a7e3-4800-955d-1fbb61f49610")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        I34 I1I34many2one { get; set; }


        #region Allors
        [Id("58d75a73-61d3-4ad7-bd1a-b3e673d8ee31")]
        [AssociationId("44d71cd6-e6fd-4629-84c0-d1240eeb3d4a")]
        [RoleId("47be578b-ce67-47d8-8bd7-4810210c60bd")]
        #endregion
        double I1FloatBetweenA { get; set; }


        #region Allors
        [Id("5901c4d4-420f-47a3-87e3-ac04b4601efc")]
        [AssociationId("b5fca65e-55e3-48b2-8cf8-e14d8d8894b1")]
        [RoleId("1cfea0c3-1627-45f1-ac5a-43bfdfe29211")]
        #endregion
        int I1IntegerLessThan { get; set; }


        #region Allors
        [Id("5cb44331-fd8c-4f73-8994-161f702849b6")]
        [AssociationId("fc1f2194-baa6-4fd8-9c62-b1f61f5ad634")]
        [RoleId("aaf7057e-faa9-43d6-8364-08e381155719")]
        #endregion
        int I1AllorsInteger { get; set; }


        #region Allors
        [Id("68549750-b8f9-4a29-a078-803e7348e142")]
        [AssociationId("fe93fafe-0b9d-4184-ac89-6c42e0e983cc")]
        [RoleId("b4630a88-e49e-4a76-93cf-d3861902d69a")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        S2 I1S2one2one { get; set; }


        #region Allors
        [Id("6c3d04be-6f95-44b8-863a-245e150e3110")]
        [AssociationId("b06e8d7f-2cc7-486e-9e72-428965f335ab")]
        [RoleId("1ed86e48-fa9c-4590-ba1b-f2a6345ff572")]
        #endregion
        bool I1AllorsBoolean { get; set; }


        #region Allors
        [Id("6e7c286c-42e0-45d7-8ad8-ac0ed91dbbb5")]
        [AssociationId("21938cf4-639c-47a1-a0f8-0c8015dfea39")]
        [RoleId("ed80c525-937e-4f2f-8e80-fa259912d7ab")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        I1 I1I1many2one { get; set; }


        #region Allors
        [Id("7014e84c-62c4-48ba-b4ec-ab52a897f443")]
        [AssociationId("b131af64-a3d7-406d-98eb-7f7ed084e119")]
        [RoleId("c78ad636-2e8f-4a97-b68f-e9ed09876115")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        C1[] I1C1many2manies { get; set; }


        #region Allors
        [Id("70312f37-52e9-4cf6-9dd6-b357628ea3ed")]
        [AssociationId("d2e511e6-68df-46e2-a342-18073b892909")]
        [RoleId("74935b78-53ed-4598-b45b-f491d197998d")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        I2 I1I2one2one { get; set; }


        #region Allors
        [Id("818b4013-5ef1-4455-9f0d-9a39fa3425bb")]
        [AssociationId("7dae54a7-847a-4b7e-b965-e4597f44905a")]
        [RoleId("680be670-2b68-4a46-b34c-2056f9e0f31a")]
        [Precision(10)]
        [Scale(2)]
        #endregion
        decimal I1AllorsDecimal { get; set; }


        #region Allors
        [Id("82a81e9e-7a13-43d3-bb8f-227edfe26a1f")]
        [AssociationId("e9da0489-5ce1-48e2-8911-4088830a6762")]
        [RoleId("451523e4-942d-48c0-8959-48e51ea438db")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        S1[] I1S1many2manies { get; set; }


        #region Allors
        [Id("9095f55b-de23-49d7-a28e-918c22c5cfd2")]
        [AssociationId("969b2592-6bde-4af3-b75a-7e1a4d1fd2f6")]
        [RoleId("00ff69ce-c661-4aa7-88a6-1883ed06295c")]
        #endregion
        DateTime I1DateTimeGreaterThan { get; set; }


        #region Allors
        [Id("912eeb1b-c5d6-4ea3-9e66-6d92cc455ef6")]
        [AssociationId("96696293-9326-4a9b-9a85-97897d517007")]
        [RoleId("4b1de97f-e65e-44a0-bbfd-f0fd6c9b7297")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        I34[] I1I34many2manies { get; set; }


        #region Allors
        [Id("9291fb85-9d1f-4c5d-96ec-797be51557ce")]
        [AssociationId("f89a79a7-c333-4b65-ac59-5e298cd60a65")]
        [RoleId("441a8475-351f-4500-a2ba-784b87d66bc5")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        I34 I1I34one2one { get; set; }


        #region Allors
        [Id("95fff847-922f-4d6f-9e98-37013bdf6b06")]
        [AssociationId("8faa2719-125a-40e5-837d-c69b1e31fec1")]
        [RoleId("bd8cc57b-d61c-4712-a25e-897c545f1d80")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        I1[] I1I1one2manies { get; set; }


        #region Allors
        [Id("9735d027-4249-4540-9658-f3ec06d3b868")]
        [AssociationId("82864251-210d-43b3-9fc7-47c7cdf012a2")]
        [RoleId("0d008a4b-9396-4fa1-b020-4ddf06d0a0ca")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        I1[] I1I1many2manies { get; set; }


        #region Allors
        [Id("973d6e4f-57ff-454a-9621-bd5dccb65525")]
        [AssociationId("2db0a4fd-f65b-460c-b286-ac8558e2acfc")]
        [RoleId("87593b08-18e6-4358-bbae-5766cc2e59d8")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        S2[] I1S2many2manies { get; set; }


        #region Allors
        [Id("9b05ecb0-c3d5-4b11-98dc-653aef9f65cc")]
        [AssociationId("f343fb09-2b0c-48fb-9bfa-826c19420b39")]
        [RoleId("b32bfb71-9a4b-4a28-bb21-8e0886a28c39")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        I12[] I1I12many2manies { get; set; }


        #region Allors
        [Id("9f70c4eb-2e36-4ae1-8ed2-b3fab908e392")]
        [AssociationId("6439b6c0-8fd8-4f62-a6c9-def7bd64e5e7")]
        [RoleId("3ff3567a-41d1-447c-b6f1-1695a92e02c8")]
        [Size(256)]
        #endregion
        string I1StringEquals { get; set; }


        #region Allors
        [Id("a458ad6e-0f4a-473b-a233-04b8e7fadf62")]
        [AssociationId("39d37d93-d921-4922-a73c-783ae90f7367")]
        [RoleId("2ca3ff16-fb9e-4c70-bc0c-37805e6233e6")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        I12[] I1I12one2manies { get; set; }


        #region Allors
        [Id("a77bcd80-82df-4b76-a1bc-8e78106d7d53")]
        [AssociationId("3ef21f1e-20fd-4046-9014-dcc5043ec2a3")]
        [RoleId("c2ac2352-acec-4b8a-9569-9dd3e45a8fb6")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        S2[] I1S2one2manies { get; set; }


        #region Allors
        [Id("b4f171d3-1463-41bc-8230-e53e5a717b89")]
        [AssociationId("e56a1b63-1f5b-489a-8cc2-b56945875f97")]
        [RoleId("13c15568-8869-4516-93bf-d962688e1195")]
        [Multiplicity(Multiplicity.ManyToMany)]
        #endregion
        C2[] I1C2many2manies { get; set; }


        #region Allors
        [Id("b9c67658-4abc-41f3-9434-c8512a482179")]
        [AssociationId("689fc6d4-edb2-450d-8b05-98da35974a53")]
        [RoleId("a19db58e-703d-408a-92d1-f2b7191b0b61")]
        [Size(-1)]
        #endregion
        byte[] I1AllorsBinary { get; set; }


        #region Allors
        [Id("c04d1e56-2686-495b-a02d-cda84f7cd2ff")]
        [AssociationId("97561659-0ddd-42c3-a575-13d111c39bda")]
        [RoleId("f1c5aefc-bb7f-4017-82d9-b077b8107adf")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        decimal I1DecimalBetweenB { get; set; }


        #region Allors
        [Id("c3496e43-335b-43b8-9fed-44439c9ae0d1")]
        [AssociationId("92e14e06-fc51-4e01-bd8c-37d5d4b70621")]
        [RoleId("147642e9-ebb7-470e-9e57-4a47f8d4dbf4")]
        #endregion
        double I1FloatGreaterThan { get; set; }


        #region Allors
        [Id("c892a286-fe92-4b8b-98ba-c5e02fb96279")]
        [AssociationId("3de80cf6-a470-499e-b69c-fa42a3bb6f5f")]
        [RoleId("0b658ec0-9440-4261-9d8b-b54a3540c492")]
        #endregion
        int I1IntegerBetweenB { get; set; }


        #region Allors
        [Id("c95ac96b-4385-4e31-8719-f120c76ab67a")]
        [AssociationId("7d094e06-7b45-4cf9-a65c-ff111a44ecf0")]
        [RoleId("761b1ac8-87b2-440c-ab1b-06be3a6d5ab1")]
        #endregion
        DateTime I1DateTimeBetweenA { get; set; }


        #region Allors
        [Id("cdb758bf-ecaf-4d99-88fb-58df9258c13c")]
        [AssociationId("08f1b9c8-6afa-49f1-a5f6-c9c23dc37c33")]
        [RoleId("cde919c6-3192-4337-8b53-6cc03b364368")]
        #endregion
        double I1AllorsDouble { get; set; }


        #region Allors
        [Id("d24b5b74-6ea2-4788-857c-90e0ba1433a5")]
        [AssociationId("61cb8d4d-4e16-4941-b1f4-baf345306959")]
        [RoleId("551bf180-4c14-44ed-ad0c-4b17501ea50e")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        S1[] I1S1one2manies { get; set; }


        #region Allors
        [Id("ddbfe021-3310-4d8e-a4ef-438306aaf191")]
        [AssociationId("f2901823-6eac-46d9-8590-b755bfa166e7")]
        [RoleId("a7a81a4d-efca-4601-820e-7f1cd6584ad8")]
        [Multiplicity(Multiplicity.OneToOne)]
        #endregion
        I1 I1I1one2one { get; set; }


        #region Allors
        [Id("e8f1c37a-6bae-4ff5-b385-39bff287bf78")]
        [AssociationId("0c088e73-c60f-404d-8410-fc1b4aab3a30")]
        [RoleId("9543f2dd-2475-4544-8d6c-eaf06e5bbb18")]
        #endregion
        int I1IntegerGreaterThan { get; set; }


        #region Allors
        [Id("ee44a1bb-a5c7-4b05-a06b-8ff9ca9d4f98")]
        [AssociationId("7243691d-2788-480e-aa53-b77d03efed9b")]
        [RoleId("104f0133-ef22-40e9-8db6-2671ad276d09")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        S1 I1S1many2one { get; set; }


        #region Allors
        [Id("eec19d8e-727c-437a-95db-b301cd1cd65a")]
        [AssociationId("75c77005-2e6c-464d-8e0c-530a6a9a43b5")]
        [RoleId("2699f05a-4f69-4781-908a-57d43112bbf4")]
        #endregion
        double I1FloatBetweenB { get; set; }


        #region Allors
        [Id("f1a1ef6a-8275-4b57-8cd0-8e79ee5a517d")]
        [AssociationId("987e84d3-9e09-46a4-83de-645cb58581b7")]
        [RoleId("c80e5776-84d5-4854-bb0a-fd6c6977c1fd")]
        [Precision(19)]
        [Scale(2)]
        #endregion
        decimal I1DecimalLessThan { get; set; }


        #region Allors
        [Id("f5a6b7d9-9f49-44a8-b303-1a2969195bd1")]
        [AssociationId("50959998-2354-4882-9a43-faccfdfc36af")]
        [RoleId("beab6646-0eff-42fe-8d43-f5e788473b8b")]
        #endregion
        DateTime I1DateTimeBetweenB { get; set; }


        #region Allors
        [Id("f9d7411e-7993-4e43-a7e2-726f1e44e29c")]
        [AssociationId("15ba7641-40ed-47d3-ab5a-e3c1961932c3")]
        [RoleId("9570a106-ceab-41d1-bcb5-b073e2ad82cf")]
        #endregion
        Guid I1AllorsUnique { get; set; }


        #region Allors
        [Id("fbc1fd9f-853a-4b7d-b618-447b765b3bcb")]
        [AssociationId("2cc6f888-93b0-42ac-b027-c2289ae8d19b")]
        [RoleId("49a2ce37-3e19-4b1a-9825-32db34f6a9df")]
        [Multiplicity(Multiplicity.OneToMany)]
        #endregion
        C1[] I1C1one2manies { get; set; }


        #region Allors
        [Id("fe51c02e-ed28-4628-9da1-7bc2131c8992")]
        [AssociationId("cc148e75-8f68-4e0c-9db4-4cb54616b74f")]
        [RoleId("1767a4b7-365c-4fa4-8f5d-ff4d612dae5b")]
        [Multiplicity(Multiplicity.ManyToOne)]
        #endregion
        S2 I1S2many2one { get; set; }

    }
}
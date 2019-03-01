namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("72c07e8a-03f5-4da8-ab37-236333d4f74e")]
    #endregion
    public partial class C2 : Object, DerivationCounted, I2 
    {
        #region inherited properties

        public int DerivationCount { get; set; }

        public I2 I2I2Many2One { get; set; }

        public C1 I2C1Many2One { get; set; }

        public I12 I2I12Many2One { get; set; }

        public bool I2AllorsBoolean { get; set; }

        public C1[] I2C1One2Manies { get; set; }

        public C1 I2C1One2One { get; set; }

        public decimal I2AllorsDecimal { get; set; }

        public I2[] I2I2Many2Manies { get; set; }

        public byte[] I2AllorsBinary { get; set; }

        public Guid I2AllorsUnique { get; set; }

        public I1 I2I1Many2One { get; set; }

        public DateTime I2AllorsDateTime { get; set; }

        public I12[] I2I12One2Manies { get; set; }

        public I12 I2I12One2One { get; set; }

        public C2[] I2C2Many2Manies { get; set; }

        public I1[] I2I1Many2Manies { get; set; }

        public C2 I2C2Many2One { get; set; }

        public string I2AllorsString { get; set; }

        public C2[] I2C2One2Manies { get; set; }

        public I1 I2I1One2One { get; set; }

        public I1[] I2I1One2Manies { get; set; }

        public I12[] I2I12Many2Manies { get; set; }

        public I2 I2I2One2One { get; set; }

        public int I2AllorsInteger { get; set; }

        public I2[] I2I2One2Manies { get; set; }

        public C1[] I2C1Many2Manies { get; set; }

        public C2 I2C2One2One { get; set; }

        public double I2AllorsDouble { get; set; }

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

        #endregion

        #region Allors
        [Id("07eaa992-322a-40e9-bf2c-aa33b69f54cd")]
        [AssociationId("172c107a-e140-4462-9a62-5ef91e6ead2a")]
        [RoleId("152c92f0-485e-4a28-b321-d6ed3b730fc0")]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        #endregion
        public decimal C2AllorsDecimal { get; set; }

        #region Allors
        [Id("0c8209e3-b2fc-4c7a-acd2-6b5b8ac89bf4")]
        [AssociationId("56bb9554-819f-418a-9ce1-a91fa704b371")]
        [RoleId("9292cb86-3e04-4cd4-b3fd-a5af7a5ce538")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public C1 C2C1One2One { get; set; }

        #region Allors
        [Id("12896fc2-c9e9-4a89-b875-0aeb92e298e5")]
        [AssociationId("781b282e-b86f-4747-9d5e-d0f7c08b0899")]
        [RoleId("f41ddb05-4a96-40fa-859b-8b3d6dfcd86b")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public C2 C2C2Many2One { get; set; }

        #region Allors
        [Id("1444d919-6ca1-4642-8d18-9d955c817581")]
        [AssociationId("9263c1e7-0cda-4129-a16d-da5351adafcb")]
        [RoleId("cc1f2cae-2a5d-4584-aa08-4b03fc2176d2")]
        [Workspace]
        #endregion
        public Guid C2AllorsUnique { get; set; }

        #region Allors
        [Id("165cc83e-935d-4d0d-aec7-5da155300086")]
        [AssociationId("bc437b29-f883-41c1-b36f-20be37bc9b30")]
        [RoleId("b2f83414-aa5c-44fd-a382-56119727785a")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I12 C2I12Many2One { get; set; }

        #region Allors
        [Id("1d0c57c9-a3d1-4134-bc7d-7bb587d8250f")]
        [AssociationId("07c026ad-3515-4df0-bee7-ab61d5a9217d")]
        [RoleId("c0562ba5-0402-44ea-acd0-9e78d20e7576")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I12 C2I12One2One { get; set; }

        #region Allors
        [Id("1d98eda7-6dba-43f1-a5ce-44f7ed104cf9")]
        [AssociationId("cae17f3c-a605-4dce-b38d-01c23eea29df")]
        [RoleId("d3e84546-02fc-40be-b550-dbd54cd8a139")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I1[] C2I1Many2Manies { get; set; }

        #region Allors
        [Id("262ad367-a52c-4d8b-94e2-b477bb098423")]
        [AssociationId("31be0ad7-0886-406a-a69f-7e38b4526199")]
        [RoleId("c52984df-80f8-4622-84e0-0e9f97cfaff3")]
        [Workspace]
        #endregion
        public double C2AllorsDouble { get; set; }

        #region Allors
        [Id("2ac55066-c748-4f90-9d0f-1090fe02cc76")]
        [AssociationId("02a5ac2c-d0ac-482d-abee-117ed7eaa5ba")]
        [RoleId("28f373c6-62b6-4f5c-b794-c10138043a63")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I1[] C2I1One2Manies { get; set; }

        #region Allors
        [Id("38063edc-271a-410d-b857-807a9100c7b5")]
        [AssociationId("6bedcc6b-af15-4f27-93e8-4404d23dfd99")]
        [RoleId("642f5531-896d-482f-b746-4ecf08f27027")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I2 C2I2One2One { get; set; }

        #region Allors
        [Id("42f9f4b6-3b35-4168-93cb-35171dbf83f4")]
        [AssociationId("622f9b4f-efc8-454f-9dd6-884bed5b5f4b")]
        [RoleId("f5545dfc-e19a-456a-8469-46708ea5bb68")]
        [Workspace]
        #endregion
        public int C2AllorsInteger { get; set; }

        #region Allors
        [Id("4a963639-72c3-4e9f-9058-bcfc8fe0bc9e")]
        [AssociationId("e8c9548b-3d75-4f2b-af4f-f953572c587c")]
        [RoleId("a1a975a4-7d1e-4734-962e-2f717386783a")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I2[] C2I2Many2Manies { get; set; }

        #region Allors
        [Id("50300577-b5fb-4c16-9ac5-41151543f958")]
        [AssociationId("1f16f92e-ba99-4553-bd1d-b95ba360468a")]
        [RoleId("6210478c-86e3-4d8c-8e3c-3b29da3175ca")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I12[] C2I12Many2Manies { get; set; }

        #region Allors
        [Id("60680366-4790-4443-a941-b30cd4bd3848")]
        [AssociationId("8fa68cfd-8e0c-40c6-881b-4ebe88487ae7")]
        [RoleId("bfa632a3-f334-4c92-a1b1-21cfa726ab90")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public C2[] C2C2One2Manies { get; set; }

        #region Allors
        [Id("61daaaae-dd22-405e-aa98-6321d2f8af04")]
        [AssociationId("a0291a20-3519-44e6-bb8d-b53682c02c52")]
        [RoleId("bff48eef-9e8f-45b7-83ff-7b63dac407f1")]
        [Workspace]
        #endregion
        public bool C2AllorsBoolean { get; set; }

        #region Allors
        [Id("65a246a7-cd78-45eb-90db-39f542e7c6cf")]
        [AssociationId("eb4f1289-1c6c-4964-a9ba-50f991a96564")]
        [RoleId("6ff71b5b-723d-424f-9e2f-fb37bb8118fe")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I1 C2I1Many2One { get; set; }

        #region Allors
        [Id("67780894-fa62-48ba-8f47-7f54106090cd")]
        [AssociationId("38cd28ba-c584-4d06-b521-dcc8094c5ed3")]
        [RoleId("128eb00f-03fc-432e-bec6-8fcfb265a3a9")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I1 C2I1One2One { get; set; }

        #region Allors
        [Id("70600f67-7b18-4b5c-b11e-2ed180c5b2d6")]
        [AssociationId("a373cb01-731b-48be-a387-d057fdb70684")]
        [RoleId("572738e4-956b-404d-97e8-4bb431ce7692")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public C1[] C2C1Many2Manies { get; set; }
        
        #region Allors
        [Id("770eb33c-c8ef-4629-a3a0-20decd92ff62")]
        [AssociationId("de757393-f81a-413c-897b-a47efd48cc79")]
        [RoleId("8ac9a5cd-35a4-4ca3-a1af-ab3f489c7a52")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I12[] C2I12One2Manies { get; set; }

        #region Allors
        [Id("7a9129c9-7b6d-4bdd-a630-cfd1392549b7")]
        [AssociationId("87f7a34c-476f-4670-a670-30451c05842d")]
        [RoleId("19f3caa1-c8d1-4257-b4ad-2f8ccb809524")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public I2[] C2I2One2Manies { get; set; }

        #region Allors
        [Id("86ad371b-0afd-420b-a855-38ebb3f39f38")]
        [AssociationId("23f5e29b-c36b-416f-93da-9ef2d79fc0f1")]
        [RoleId("cdf7b6ee-fa50-44a1-9433-d04d61ef3aeb")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public C2 C2C2One2One { get; set; }

        #region Allors
        [Id("9c7cde3f-9b61-4c79-a5d7-afe1067262ce")]
        [AssociationId("71d6109e-1c04-4598-88fa-f06308beb45b")]
        [RoleId("8a96d544-e96f-45b5-aeee-d9381946ff31")]
        [Size(256)]
        [Workspace]
        #endregion
        public string C2AllorsString { get; set; }

        #region Allors
        [Id("a5315151-aa0f-42a3-9d5b-2c7f2cb92560")]
        [AssociationId("f2bf51b6-0375-4d77-8881-d4d313d682ef")]
        [RoleId("54dce296-9454-440e-9cf3-1327ea439f0e")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public C1 C2C1Many2One { get; set; }

        #region Allors
        [Id("bc6c7fe0-6501-428c-a929-da87a9f4b885")]
        [AssociationId("794d2637-293c-49cc-a052-246a779825e9")]
        [RoleId("73d243be-d8d0-42c7-b354-fd9786b4eaaa")]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public C2[] C2C2Many2Manies { get; set; }

        #region Allors
        [Id("ce23482d-3a22-4202-98e7-5934fd9abd2d")]
        [AssociationId("6d752249-af37-4f22-9e59-bfae9e6537ee")]
        [RoleId("6e9490f2-740f-498c-9c0f-d601c76f28ad")]
        [Workspace]
        #endregion
        public DateTime C2AllorsDateTime { get; set; }

        #region Allors
        [Id("e08d75a9-9b67-4d20-a476-757f8fb17376")]
        [AssociationId("7d45be10-724e-46c4-8dac-4acdf7f515ad")]
        [RoleId("888cd015-7323-45da-83fe-eb297e8ede51")]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public I2 C2I2Many2One { get; set; }

        #region Allors
        [Id("f748949e-de5a-4f2e-85e2-e15516d9bf24")]
        [AssociationId("92c02837-9e6c-45ad-8772-b97a17afad8c")]
        [RoleId("2c172bc6-a87b-4945-b02f-e00a38eb866d")]
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        #endregion
        public C1[] C2C1One2Manies { get; set; }

        #region Allors
        [Id("fa8ad982-9953-47dd-9905-81cc4572300e")]
        [AssociationId("604eec66-6a75-465b-93d8-349dcbccb2bd")]
        [RoleId("e701ac90-488a-476f-9b13-ea361e8ff450")]
        [Size(-1)]
        [Workspace]
        #endregion
        public byte[] C2AllorsBinary { get; set; }

        #region Allors
        [Id("8695D9F6-AE00-4001-9990-851260F3ABE7")]
        [AssociationId("267F5530-584E-40F1-9A90-DF5CBD33ECB8")]
        [RoleId("5345AADA-A434-42D0-930E-1DA112711C52")]
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        #endregion
        public S1 S1One2One { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void OnPreFinalize()
        {
            throw new NotImplementedException();
        }

        public void OnFinalize()
        {
            throw new NotImplementedException();
        }

        public void OnPostFinalize()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
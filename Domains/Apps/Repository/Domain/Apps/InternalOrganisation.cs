namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("c81441c8-9ac9-440e-a926-c96230b2701f")]
    #endregion
    public partial class InternalOrganisation: AccessControlledObject, UniquelyIdentifiable, Auditable
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid UniqueId { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Allors
        [Id("00bf781c-c874-44fe-ae60-d6609075b1c0")]
        [AssociationId("3b99af1e-e6c3-498b-aabd-78d6e82c8819")]
        [RoleId("b9a508e2-2931-4ddc-ab34-947d19c2d742")]
        #endregion
        [Size(256)]
        [Workspace]
        public string PurchaseOrderNumberPrefix { get; set; }

        #region Allors
        [Id("01d4f5d8-da57-4524-b35f-69a1a4adfa1c")]
        [AssociationId("84ff4f9a-b1d3-4e2c-aff6-52a9f75e874a")]
        [RoleId("ee1b0251-ba57-490a-bdc9-c4e8fd6142ce")]
        #endregion
        [Workspace]
        [Size(256)]
        public string TransactionReferenceNumber { get; set; }

        #region Allors
        [Id("0994b73e-8d4c-4fa4-aca2-287449b22ca7")]
        [AssociationId("17a9138e-76c8-42e1-85b8-7af73b551a22")]
        [RoleId("09fbb64d-c32e-4734-8df9-6e741a5070a5")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Indexed]
        public JournalEntryNumber[] JournalEntryNumbers { get; set; }

        #region Allors
        [Id("1a2533cb-9b75-4597-83ab-9bbfc49e0103")]
        [AssociationId("b4cb12ba-0ea1-41e5-945a-030503bf2c7b")]
        [RoleId("031ba0aa-32e4-470c-9a79-fae65cace2f2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public Country EuListingState { get; set; }

        #region Allors
        [Id("1a986cbf-b7db-4850-af06-d96e1339beb7")]
        [AssociationId("fc69f819-2ca0-4ac4-b36e-cfff791679a1")]
        [RoleId("d6e816ae-50c5-41c7-8877-9f4c30208d47")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        [Required]
        public Counter PurchaseInvoiceCounter { get; set; }

        #region Allors
        [Id("219a1d97-9615-47c5-bc4d-20a7d37313bd")]
        [AssociationId("4fd6a9e2-174c-41db-b519-44c317de0f96")]
        [RoleId("45f4d069-5faf-45cf-b097-21f58fab4097")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public AccountingPeriod ActualAccountingPeriod { get; set; }

        #region Allors
        [Id("23aee857-9cea-481c-a4a3-72dd8b808d71")]
        [AssociationId("61c64e66-4647-439d-9efa-28500319e8ca")]
        [RoleId("4d68b2fe-d5b4-45f8-9e68-377d75f3401d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InvoiceSequence InvoiceSequence { get; set; }

        #region Allors
        [Id("293758d7-cc0a-4f1c-b122-84f609a828c2")]
        [AssociationId("afe9d80b-2984-4e46-b45c-e5a25af3bccd")]
        [RoleId("97aa32a6-69c0-4cc5-9e42-98907ff6c45f")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public PaymentMethod[] ActivePaymentMethods { get; set; }

        #region Allors
        [Id("37b4bf2c-5b09-42b0-84d9-59b57793cf37")]
        [AssociationId("22aff7e0-1b45-4f06-b281-19cbf0d1c511")]
        [RoleId("dcd31b64-e449-4833-91cb-8237bdb71b78")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaximumAllowedPaymentDifference { get; set; }

        #region Allors
        [Id("39a09487-dcf4-4bc8-8494-859d7a8cc3dd")]
        [AssociationId("da98358b-fb19-4b40-ad32-ffc0b48583fe")]
        [RoleId("9b20db3b-ac0e-4374-8262-3ee22f8067ee")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Media LogoImage { get; set; }

        #region Allors
        [Id("3b32c442-9cbc-41d8-8eb2-2ae41beca2c4")]
        [AssociationId("eda352cd-00cc-4d04-99cf-f7ad667cb20a")]
        [RoleId("b8211575-f4c9-44c8-89b2-bd122704f098")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public CostCenterSplitMethod CostCenterSplitMethod { get; set; }

        #region Allors
        [Id("44f165ed-a6ca-4979-9046-a0f7391bef7d")]
        [AssociationId("5e581649-0b45-498f-b274-fdcc05bb3894")]
        [RoleId("1069343b-d6d5-4418-abf9-688b9dcec6d0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Counter PurchaseOrderCounter { get; set; }

        #region Allors
        [Id("496f6d33-2259-442e-924f-636d73cec52f")]
        [AssociationId("4ac05596-41d8-402a-8308-d9f458d604e0")]
        [RoleId("0739dac1-6507-4306-95c9-78f10532a78e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public LegalForm LegalForm { get; set; }

        #region Allors
        [Id("4fc741ef-fe95-49a8-8bcd-8ff43092db88")]
        [AssociationId("0e58cd98-94e6-42ab-8501-394f8b8d3624")]
        [RoleId("d5096d5d-a443-4667-95dd-7184f348e55c")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccount SalesPaymentDifferencesAccount { get; set; }

        #region Allors
        [Id("538fc59e-42da-471a-96a4-d8a93b2de229")]
        [AssociationId("d0f6dc1f-a056-4a8c-b138-d68a5cf10247")]
        [RoleId("948f1ebc-a637-4c1b-ae35-dcc7462c95d0")]
        #endregion
        [Required]
        [Size(256)]
        [Workspace]
        public string Name { get; set; }

        #region Allors
        [Id("5b64038f-5ad9-46a6-9af6-b95819ac9830")]
        [AssociationId("753f6fa9-ff10-402c-9812-d2c738d35dbb")]
        [RoleId("8411c910-14d4-4629-aa39-c58602a799d4")]
        #endregion
        [Size(256)]
        [Workspace]
        public string PurchaseTransactionReferenceNumber { get; set; }

        #region Allors
        [Id("5b64cf9d-e990-491e-b009-3481d73db67e")]
        [AssociationId("40296302-a559-4014-ba68-929d4238f4d8")]
        [RoleId("5b966f3d-cc80-44f4-b255-87182aa796d4")]
        #endregion
        [Required]
        [Workspace]
        public int FiscalYearStartMonth { get; set; }

        #region Allors
        [Id("63c9ceb1-d583-41e1-a9a9-0c2576e9adfc")]
        [AssociationId("fd1ac2ff-6869-44bd-9b4e-05a3b14fbad9")]
        [RoleId("41058049-5c44-47b7-bbb9-34ab0bdcfbcb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public CostOfGoodsSoldMethod CostOfGoodsSoldMethod { get; set; }

        #region Allors
        [Id("77ae5145-791a-4ef0-94cc-6c9683b02f13")]
        [AssociationId("0da6cc82-b538-4346-bb48-19b02223a566")]
        [RoleId("bef210d2-7af6-4653-872b-a5eebba2af87")]
        #endregion
        [Workspace]
        public bool VatDeactivated { get; set; }

        #region Allors
        [Id("7e210c5e-a68b-4ea0-b019-1dd452d8e407")]
        [AssociationId("b9c41192-1666-44c0-9365-b24df29a2cdf")]
        [RoleId("dfe3042c-babb-416b-a414-5dda0a2958c0")]
        #endregion
        [Required]
        [Workspace]
        public int FiscalYearStartDay { get; set; }

        #region Allors
        [Id("848f3098-ce8b-400c-9775-85c00ac68f28")]
        [AssociationId("44649646-ccf8-48b7-9ebf-a09df75d23fc")]
        [RoleId("3d5a74b3-cb62-4803-8115-fde43a648af5")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccount[] GeneralLedgerAccounts { get; set; }

        #region Allors
        [Id("859d95c2-7321-4408-bcd1-405dc0b31efc")]
        [AssociationId("47674006-74f4-497e-b9b6-1d5cff1ada0f")]
        [RoleId("0dc922be-86d0-469e-a9d2-5bf19a910994")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Counter AccountingTransactionCounter { get; set; }

        #region Allors
        [Id("86dd32e2-74e7-4ced-bfbd-4e1fdc723588")]
        [AssociationId("ab8ace70-5d23-4ba0-83f7-471a8dbefeb6")]
        [RoleId("fa1a381c-a895-44d2-a028-860486b1d1af")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Counter IncomingShipmentCounter { get; set; }

        #region Allors
        [Id("89f4907d-4a10-428d-9e6b-ef9fb045c019")]
        [AssociationId("a0fd3167-5d4e-400f-b593-1497fce5d024")]
        [RoleId("0ab03957-8140-4e1c-8a13-a9647f6c9e47")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccount RetainedEarningsAccount { get; set; }

        #region Allors
        [Id("9d6aaa81-9f97-427e-9f46-1f1e93748248")]
        [AssociationId("b251ce10-b4ac-45da-bc57-0a42e75f3660")]
        [RoleId("6ffd36cd-37de-444b-93b6-4128de34254f")]
        #endregion
        [Size(256)]
        [Workspace]
        public string PurchaseInvoiceNumberPrefix { get; set; }

        #region Allors
        [Id("ab2004c1-fd91-4298-87cd-532a6fe5efb0")]
        [AssociationId("b53b80a4-92f5-4e08-a9fb-86bf2bd9572e")]
        [RoleId("7fe218f4-975d-40f4-82b6-b43cb308aff4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccount SalesPaymentDiscountDifferencesAccount { get; set; }

        #region Allors
        [Id("ad7c8532-59d2-4668-bd9f-6c67ddc4e4bc")]
        [AssociationId("7028e722-f29d-42f6-b7c5-d73c2e99c907")]
        [RoleId("48454368-06c6-4e64-876b-b5e4268407a4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Counter SubAccountCounter { get; set; }

        #region Allors
        [Id("afbaffe6-b03c-463e-b074-08b32641b482")]
        [AssociationId("4a02dff3-4bd3-4453-b2fa-e6e79f1b18b0")]
        [RoleId("334cb83a-410e-4abb-8b51-9dd19e2fc21b")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public AccountingTransactionNumber[] AccountingTransactionNumbers { get; set; }

        #region Allors
        [Id("b8af8dce-d0e8-4e16-8d72-e56b920a04b4")]
        [AssociationId("57168fae-0b31-4370-afff-ab5a02c9a8ee")]
        [RoleId("d4c44bcf-c38b-46b9-86f6-462a48714389")]
        #endregion
        [Size(256)]
        [Workspace]
        public string TransactionReferenceNumberPrefix { get; set; }

        #region Allors
        [Id("ba00c0d2-6067-4584-bdc4-e6c72be77232")]
        [AssociationId("687afb94-445e-41e2-bf22-deaac5b40e5a")]
        [RoleId("9fa5512e-098a-40fd-a821-38a5f5f280f5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Counter QuoteCounter { get; set; }

        #region Allors
        [Id("A49663B5-A432-41FA-BBCA-8368D1B9D53D")]
        [AssociationId("27E7D099-3D94-4DCF-AFD2-9143A4815ECC")]
        [RoleId("A82306DA-FFA4-48D5-81EC-B657E2CB99C1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Counter RequestCounter { get; set; }

        #region Allors
        [Id("d0ebaa65-260a-4511-a137-89f25016f12c")]
        [AssociationId("70e0b1a9-4c82-4c47-a6b8-df7a62423a08")]
        [RoleId("1413c3f9-a226-4356-8bcc-6f8ae1963a6e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccount PurchasePaymentDifferencesAccount { get; set; }

        #region Allors
        [Id("d2ad57d5-de30-4bc0-90a7-9aea7a9da8c7")]
        [AssociationId("dbd28e59-d5d6-4d95-aef1-6881e0fe2d48")]
        [RoleId("2e28606f-5708-4c0f-bdfa-04019a0e97d9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccount SuspenceAccount { get; set; }

        #region Allors
        [Id("d48ef8bb-064b-4360-8162-a138fb601761")]
        [AssociationId("91252cf4-eca3-4b9d-b06d-5df795c3709c")]
        [RoleId("3ef1f904-5b02-4f00-8486-2e452668be67")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccount NetIncomeAccount { get; set; }

        #region Allors
        [Id("d5645df8-2b10-435d-8e47-57b5d268541a")]
        [AssociationId("4d145acb-007a-46b9-98a8-a86888221e28")]
        [RoleId("c2fe67e4-a100-4d96-b4ff-df1ec73db5fe")]
        #endregion
        [Required]
        [Workspace]
        public bool DoAccounting { get; set; }

        #region Allors
        [Id("dcf24d2f-7bf2-43fd-82b4-bd30fd545022")]
        [AssociationId("b4b8b2e6-141c-416f-89a8-746c72c26e5c")]
        [RoleId("dc7127c4-c4ca-49a3-95ae-970de554d4f3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Facility DefaultFacility { get; set; }

        #region Allors
        [Id("dd008dfe-a219-42ab-bc08-d091da3f8ea4")]
        [AssociationId("77ce8418-a00b-46d0-ab7b-4a782b7387da")]
        [RoleId("a5ac9bc1-5323-41ec-a791-d4ecc7d0eee8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccount PurchasePaymentDiscountDifferencesAccount { get; set; }

        #region Allors
        [Id("e9af1ca5-d24f-4af2-8687-833744941b24")]
        [AssociationId("ca041d3d-7adf-43da-ac42-c749633bd9b8")]
        [RoleId("4b50a77e-4343-415b-ad45-6cd074b681b5")]
        #endregion
        [Size(256)]
        [Workspace]
        public string QuoteNumberPrefix { get; set; }

        #region Allors
        [Id("ec8e7400-0088-4237-af32-a687e1c45d77")]
        [AssociationId("3ee2e0b2-6835-490b-937a-a853c85dd3e4")]
        [RoleId("c70fd771-87a6-4e97-98ea-34c2c0265450")]
        #endregion
        [Size(256)]
        [Workspace]
        public string PurchaseTransactionReferenceNumberPrefix { get; set; }

        #region Allors
        [Id("f14f1865-7820-4ed5-8ca9-dffcbeb6b1ec")]
        [AssociationId("4b8a7531-3996-4af2-a921-3b5653dc46ba")]
        [RoleId("0b574be0-2369-41df-9f64-71235c0b9e9a")]
        #endregion
        [Size(256)]
        [Workspace]
        public string TaxNumber { get; set; }

        #region Allors
        [Id("f353e7ef-d24d-4a27-8ec9-e930ef936240")]
        [AssociationId("cf556dfd-4b43-48e4-8243-df2650d8ce97")]
        [RoleId("6b7bda76-5fe7-4526-8c79-b42324fd4090")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public GeneralLedgerAccount CalculationDifferencesAccount { get; set; }

        #region Allors
        [Id("fe96e14b-9dbd-4497-935f-f605abd2ada7")]
        [AssociationId("68f9e5fd-7398-456b-bcb8-27b23dbce3f1")]
        [RoleId("3fc3517d-0d36-4401-a56f-ac3a83f1f892")]
        #endregion
        [Workspace]
        [Size(256)]
        public string IncomingShipmentNumberPrefix { get; set; }

        #region Allors
        [Id("06FABCD6-EFA4-45DD-B76C-8F791A0E10EF")]
        [AssociationId("83D3C799-B76E-481D-90D4-FCC5DF9FABCE")]
        [RoleId("BF7CB693-CCE3-4BDA-9A7F-C3165AD5134D")]
        #endregion
        [Size(256)]
        [Workspace]
        public string RequestNumberPrefix { get; set; }

        #region Allors
        [Id("271F9F21-7C92-488F-8334-3A656FAC680E")]
        [AssociationId("A998530A-228C-4BB5-8A59-9AC019AD57D4")]
        [RoleId("B917056D-E97F-4DD2-8CD4-F5816A58BB59")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Person[] CurrentSalesReps { get; set; }

        #region Allors
        [Id("224518F0-2014-4BF4-B10A-406821A0FD39")]
        [AssociationId("36FDEA56-5918-4F68-895C-65449FE4C579")]
        [RoleId("8DC69D34-FB39-4979-8EEA-CBF09C0996F0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Party[] CurrentCustomers { get; set; }

        #region Allors
        [Id("D4B532F9-12F0-4B51-AF8E-0E2160A2488E")]
        [AssociationId("CF65C981-5A4D-4FD5-947F-C82EF07C4634")]
        [RoleId("86D03D25-89C9-48D6-90B9-962B566741C3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Organisation[] CurrentSuppliers { get; set; }

        #region Allors
        [Id("033F506F-8004-4931-926B-1DB70556216D")]
        [AssociationId("B40B34F7-CCBB-43E8-9F33-18BF34A6705D")]
        [RoleId("6A06220D-A811-4E02-82E1-F019A5347E4D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PartyContactMechanism[] PartyContactMechanisms { get; set; }

        #region Allors
        [Id("53DA7C1E-FF77-461E-825A-02B491FF9B34")]
        [AssociationId("708F6E6A-977E-4FFD-B949-B64AE7A0D1CE")]
        [RoleId("DD093680-BF81-4D92-93CD-4358A61430A3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public BankAccount[] BankAccounts { get; set; }

        #region Allors
        [Id("B2280EDA-17F4-4F91-831B-6BA4295E81BF")]
        [AssociationId("483D505C-1A3E-453D-946D-1B1D9B5B801E")]
        [RoleId("E5B558F3-E038-4871-8EFF-72899BF326C2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PaymentMethod DefaultPaymentMethod { get; set; }

        #region Allors
        [Id("A963075C-8323-4F80-8BA3-287F13EE5682")]
        [AssociationId("298E8343-1783-4A2B-B1F1-81040A9FAEC2")]
        [RoleId("38F86651-28D0-42CE-BB80-9C195B034146")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public VatRegime VatRegime { get; set; }

        #region Allors
        [Id("7E926007-DED3-4B53-B9C5-4B4D25EC9AF7")]
        [AssociationId("56A50C32-AB99-40A1-9393-99CBFBD61B6E")]
        [RoleId("18E2BEB9-343B-4C82-9D39-DB013749DB1F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public Person[] SalesReps { get; set; }

        #region Allors
        [Id("0ac44c21-6a2c-4162-9d77-fe1b16b60b73")]
        [AssociationId("4d61b711-7aab-4162-bb31-74db09f666fe")]
        [RoleId("0da9c561-a6fc-4fea-aee3-5c24a2b08aea")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public GeneralLedgerAccount GlAccount { get; set; }

        #region Allors
        [Id("AAD85445-9900-40DB-ADA5-88BB3C9A8FDB")]
        [AssociationId("A36ED0CB-BDAD-4277-B63F-282DA0FA7B23")]
        [RoleId("E5DDB898-C95A-4BDC-944F-35B364531D3B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism BillingAddress { get; set; }

        #region Allors
        [Id("404F25DA-7919-44BF-A7AA-AF3029A965B3")]
        [AssociationId("35304A2B-6D16-46AD-8F5F-30768D704720")]
        [RoleId("C4F943DD-668F-48A0-AF74-94812718A02C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism OrderAddress { get; set; }

        #region Allors
        [Id("60ED370B-58FF-4AF4-804C-AAFEC472E696")]
        [AssociationId("00848EEB-AE9F-4F59-AD62-51B3C5CBA184")]
        [RoleId("0E154A04-9B20-4CDF-8EDC-19FACA4B48C1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PostalAddress ShippingAddress { get; set; }

        #region Allors
        [Id("5080E99D-C84E-4DD9-90D4-964148A94087")]
        [AssociationId("9A4578CE-5F50-4033-9564-A743E9169F68")]
        [RoleId("2068F7A9-8102-49E9-BD89-F68F49B3F690")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism BillingInquiriesFax { get; set; }

        #region Allors
        [Id("FDBE8828-224B-46DD-AB8D-731A96F80F56")]
        [AssociationId("3D3BABA7-AAD3-499D-93F3-BFB61661D6D8")]
        [RoleId("534249ED-2469-41D3-859F-8536631F7647")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism BillingInquiriesPhone { get; set; }

        #region Allors
        [Id("C37854EB-DD2E-4FF6-9E31-F122EF88463F")]
        [AssociationId("79F5325E-D6BC-47F5-BF69-B691889C1BFD")]
        [RoleId("4E150C7E-3AB4-45DE-AD87-93837F02DE27")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism CellPhoneNumber { get; set; }

        #region Allors
        [Id("0ED4C5A7-5278-4EA5-832D-58FF5493473D")]
        [AssociationId("900C4C3D-2320-4135-8F1D-57C428AD14F3")]
        [RoleId("FE3ED29E-D222-4FB8-9CA0-0F6FEB159955")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism GeneralFaxNumber { get; set; }

        #region Allors
        [Id("7C6DB738-64D0-4863-A8EF-1044E1D34F00")]
        [AssociationId("A06E1B6B-99B6-4A8B-AE1E-0BEBD80A0696")]
        [RoleId("F5BAA47C-0706-4087-A9B8-2977C797FB4A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism GeneralPhoneNumber { get; set; }

        #region Allors
        [Id("98F473EE-C461-4092-8501-FE81541C520D")]
        [AssociationId("2E5E9015-E4B1-449D-A020-D2D193CE847B")]
        [RoleId("9611CDD3-3240-4C91-B6CC-C4CDCB45A978")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism HeadQuarter { get; set; }

        #region Allors
        [Id("99C1A36C-D1FD-4E6A-BE9D-7B6D8330098A")]
        [AssociationId("FDD0CB4D-2C51-459F-B732-82444FE471E0")]
        [RoleId("2D5479D8-07C9-401B-A375-B673550B5746")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism HomeAddress { get; set; }

        #region Allors
        [Id("11EACBD7-08CB-4F1F-8BD8-534BB895C122")]
        [AssociationId("405331CB-6B84-40F8-858A-434D298A6AD7")]
        [RoleId("4C0AF33A-2138-41EE-B598-89F86291381A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism InternetAddress { get; set; }

        #region Allors
        [Id("AE070C65-2A6D-4354-A07B-BED331F30963")]
        [AssociationId("2B613BF6-D3D6-4CD6-925D-7C4BA288BFC1")]
        [RoleId("2E74F2E1-6D5A-4F6B-AABD-DFFC3CA714D6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism OrderInquiriesFax { get; set; }

        #region Allors
        [Id("141D0137-5EE8-46CE-ADDC-AE68F2529AE4")]
        [AssociationId("3347100B-EC76-4A55-95D7-0B03DC957661")]
        [RoleId("1556ADB1-F70E-494D-9BD4-95ACADB7CE9B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism OrderInquiriesPhone { get; set; }

        #region Allors
        [Id("AF63708E-039D-4869-90E0-D125D99E3C6A")]
        [AssociationId("A21A0D1F-F54F-440C-BB37-D12DDF05B0C8")]
        [RoleId("1B55526C-B260-4BD7-8B1C-54C31A61059E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism PersonalEmailAddress { get; set; }

        #region Allors
        [Id("6B950FD7-28FA-4EC6-B8C8-50CA7C903E30")]
        [AssociationId("E1CB63CE-EDB6-4F91-A8AE-353C063F410D")]
        [RoleId("9D8A0755-552E-4095-AE22-34581451BB7A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism SalesOffice { get; set; }

        #region Allors
        [Id("05C17892-1B62-4541-B9F1-848E87BFE05B")]
        [AssociationId("C6202255-FE24-4DDB-8B8B-97E1F9D1D43E")]
        [RoleId("273E96C4-079F-4CB1-8C4D-7C55E206A9A8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism ShippingInquiriesFax { get; set; }

        #region Allors
        [Id("74802F61-F8A4-43A1-A854-0B2AE9B3AD96")]
        [AssociationId("55BB135B-2880-4F8B-AF32-C88A26F552AE")]
        [RoleId("79D36B1F-950F-4364-9600-BB7A80488DAD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        public ContactMechanism ShippingInquiriesPhone { get; set; }

        #region Allors
        [Id("9a2ab89e-c3bc-4b6b-a82d-417dc21c8f9e")]
        [AssociationId("fcc6e653-3787-44a0-8a3a-35e80e232a02")]
        [RoleId("31549cf9-6418-4d19-96b0-5813cc964491")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public Party[] ActiveCustomers { get; set; }

        #region Allors
        [Id("cd40057a-5211-4289-a4ef-c30aa4049957")]
        [AssociationId("5dd54980-fa14-434c-80fc-64dec203fd8b")]
        [RoleId("ca87441c-6ce7-4041-bdd4-ca83f3b19289")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        public Person[] ActiveEmployees { get; set; }

        #region Allors
        [Id("e09976e8-dc99-4539-9b0b-0bbe98cc5404")]
        [AssociationId("0d828c12-82bd-4b37-96c8-68997a7c2f48")]
        [RoleId("3d362cd1-d49c-422f-9722-7276a6ee07c4")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        public Party[] ActiveSuppliers { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion
    }
}
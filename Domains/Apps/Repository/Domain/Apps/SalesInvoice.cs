namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6173fc23-115f-4356-a0ce-867872c151ac")]
    #endregion
    public partial class SalesInvoice : Invoice, Versioned
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }
        public decimal TotalShippingAndHandlingCustomerCurrency { get; set; }
        public Currency CustomerCurrency { get; set; }
        public string Description { get; set; }
        public ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }
        public decimal TotalFeeCustomerCurrency { get; set; }
        public Fee Fee { get; set; }
        public decimal TotalExVatCustomerCurrency { get; set; }
        public string CustomerReference { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TotalDiscount { get; set; }
        public BillingAccount BillingAccount { get; set; }
        public decimal TotalIncVat { get; set; }
        public decimal TotalSurcharge { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalVatCustomerCurrency { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal TotalIncVatCustomerCurrency { get; set; }
        public decimal TotalShippingAndHandling { get; set; }
        public decimal TotalBasePriceCustomerCurrency { get; set; }
        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        public decimal TotalExVat { get; set; }
        public InvoiceTerm[] InvoiceTerms { get; set; }
        public decimal TotalSurchargeCustomerCurrency { get; set; }
        public string InvoiceNumber { get; set; }
        public string Message { get; set; }
        public VatRegime VatRegime { get; set; }
        public decimal TotalDiscountCustomerCurrency { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalFee { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public Locale Locale { get; set; }
        public string Comment { get; set; }
        public Guid UniqueId { get; set; }
        public string PrintContent { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region ObjectStates
        #region SalesInvoiceState
        #region Allors
        [Id("0617CB28-67B8-4BEF-A9A3-6A06C292A7F0")]
        [AssociationId("3D8A3977-CB49-45F5-BF67-44A41CFC2998")]
        [RoleId("C1FBB023-9835-4F30-9D68-937814D22518")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesInvoiceState PreviousSalesInvoiceState { get; set; }

        #region Allors
        [Id("E706A4A4-BB19-431A-8949-1B22B0F8AA68")]
        [AssociationId("2B9FC82C-EC1C-4E1D-ABB2-B22EB77CAEA1")]
        [RoleId("09731CBF-2BF2-408A-A3C5-2B766C1183C0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesInvoiceState LastSalesInvoiceState { get; set; }

        #region Allors
        [Id("931B8FC4-A0EC-4450-A469-EB585839B05A")]
        [AssociationId("A84C1C41-7AD3-41D9-9BE0-CFBD6660BA84")]
        [RoleId("FCACD413-67ED-4323-A2F2-47D94718120A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesInvoiceState SalesInvoiceState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("FD3391B6-1B75-43F6-ADDB-A97F5E8F3BC6")]
        [AssociationId("FE12E157-9CC4-4C6B-88C0-7777406E67DA")]
        [RoleId("000F6B7C-FE00-4748-84C1-77C48F44B006")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesInvoiceVersion CurrentVersion { get; set; }

        #region Allors
        [Id("EF051A68-7FB9-4461-B16D-34F1B99F34C4")]
        [AssociationId("2C4596DA-0CD4-4F06-BDE9-0433A34C38AA")]
        [RoleId("1126B3E9-D287-4EA3-9D33-37B7BF06CDB4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesInvoiceVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("09064adb-7094-48e9-992c-2eab319d640f")]
        [AssociationId("5ade34c0-1f3c-4ecf-933d-72360173f03d")]
        [RoleId("17bb6982-04c0-42e8-9ae3-56bd50736cbb")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPrice { get; set; }

        #region Allors
        [Id("27faaa2c-d4db-4cab-aa04-8ec4997d73d2")]
        [AssociationId("2e9fab52-2029-4ee3-8eba-ffd9764bcf67")]
        [RoleId("9dd23ce4-d760-45af-94e4-c2ac94b0aea3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("2d0e924b-ff24-4630-9151-ac9bfc844c0c")]
        [AssociationId("0a159385-7570-494e-976d-4ee493235cb3")]
        [RoleId("239e91ee-5606-4131-a351-ebbd5908d9be")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Party PreviousBillToCustomer { get; set; }

        #region Allors
        [Id("3eb16102-21cc-4b71-a8e2-4f016da4cfb0")]
        [AssociationId("d6e7328a-c306-4649-a7cc-d6b53535845a")]
        [RoleId("35ae04c4-8a23-4531-8736-370ce29c970f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public SalesInvoiceType SalesInvoiceType { get; set; }

        #region Allors
        [Id("401d58f3-286e-4fe4-88a0-e0bf9e245599")]
        [AssociationId("c0b50430-9566-42b0-b533-ec48b8cfd355")]
        [RoleId("5c382076-deb8-4456-8cbd-e1f45bb4e5e3")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal InitialProfitMargin { get; set; }

        #region Allors
        [Id("4a7695a8-c649-4122-9336-8a1e2e5665ea")]
        [AssociationId("fc3ab94b-20e1-4156-aa69-381bb6e8a0b6")]
        [RoleId("550b5478-6929-47b5-b124-2e529ca59cf3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PaymentMethod PaymentMethod { get; set; }

        #region Allors
        [Id("5c1f4c88-f67d-4f82-a7de-28868a5f030d")]
        [AssociationId("32125426-057d-441f-b9c9-2162d58fea83")]
        [RoleId("801d63a0-31ae-4000-802a-b827e4122c62")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public SalesOrder SalesOrder { get; set; }

        #region Allors
        [Id("5c3903fa-105b-4c57-8281-1486b0411a3a")]
        [AssociationId("2d1495cc-54f2-4ff7-bbfc-6e3aafb2e319")]
        [RoleId("dc40bbae-ac9b-468b-add4-35dfb53a469b")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal InitialMarkupPercentage { get; set; }

        #region Allors
        [Id("67f49b02-f129-4e18-9411-b8b3d17f151b")]
        [AssociationId("faffb97a-02d7-4e1d-97c6-fc9275ee5fe6")]
        [RoleId("5a4b5008-2fdd-43a9-a92b-d7d8b3e6678f")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaintainedMarkupPercentage { get; set; }

        #region Allors
        [Id("6cb5e21c-6344-46a9-bab5-355cdfbead81")]
        [AssociationId("8e8100ae-dbaa-425c-9dfe-4dccb1d2335a")]
        [RoleId("9f01863e-afc8-47d6-adf1-7c861cd97229")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public Person[] SalesReps { get; set; }

        #region Allors
        [Id("6e2b9a8a-9d59-4041-a9ea-f3f8286f110c")]
        [AssociationId("ee7aba21-39d6-4a4c-8b18-c7c141c8abdc")]
        [RoleId("12db3958-c666-475e-85db-124c6549664d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public Shipment Shipment { get; set; }

        #region Allors
        [Id("76982824-9c87-4f93-b2c1-ae312b200bdb")]
        [AssociationId("a2832845-c225-4c46-8ce5-c17b9cdcb04b")]
        [RoleId("d097a56f-225e-46be-9474-b35872532e52")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaintainedProfitMargin { get; set; }

        #region Allors
        [Id("7f833ad2-3146-4660-a9d4-8a70d3ce01db")]
        [AssociationId("b466881e-156a-488f-9f26-c2850b7dd7fc")]
        [RoleId("aa621b67-049a-44e8-9f70-07e2a0c696b8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Party PreviousShipToCustomer { get; set; }

        #region Allors
        [Id("816d66a7-7cab-4ce3-9912-c7cc9d6c294c")]
        [AssociationId("8b3c78de-7281-4f94-aeda-1dc6bd345df3")]
        [RoleId("056822e6-4333-44ae-8479-d05c1b1b2974")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party BillToCustomer { get; set; }

        #region Allors
        [Id("89557826-c9d1-4aa1-8789-79fb425cdb87")]
        [AssociationId("7d157e5a-efbb-453e-bd95-27a9b0ab305f")]
        [RoleId("751ada5f-ff41-43ae-8609-0c1457642375")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public SalesInvoiceItem[] SalesInvoiceItems { get; set; }

        #region Allors
        [Id("ab59d448-e9a4-48c3-9288-5a9b7c524870")]
        [AssociationId("0b3fb144-b9bf-4651-b227-2f00a5c95c38")]
        [RoleId("124b784c-0b1d-46c6-8369-ae3886b51a47")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPriceCustomerCurrency { get; set; }

        #region Allors
        [Id("af0a72c8-003c-44a6-8c6f-086f26542e3d")]
        [AssociationId("d434a95b-9053-4471-864b-3d139b78915d")]
        [RoleId("6c44f465-7d50-4a1b-bffa-9693f9afbde2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party ShipToCustomer { get; set; }

        #region Allors
        [Id("ddd9b372-4687-4a6e-b62b-4e0521f8c4b7")]
        [AssociationId("3e5b5599-82bc-4bc3-8ef0-9b2301a1ad40")]
        [RoleId("33265997-e42c-4955-839c-d2ce054b2d33")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism BilledFromContactMechanism { get; set; }

        #region Allors
        [Id("deb1b4ad-39a4-480a-8ef2-3f05c6505077")]
        [AssociationId("98bd67fc-c675-425a-800d-79cea6a4a193")]
        [RoleId("1ed1e917-2729-4d14-8b28-686991e11d6c")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalPurchasePrice { get; set; }

        #region Allors
        [Id("ed091c3c-1f38-498a-8ca5-ca8b8ddfc5c4")]
        [AssociationId("2531dbb0-e34e-41c2-b6e2-95e3a39cf54d")]
        [RoleId("e279aec5-e503-46c5-9563-b13f58274f71")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SalesChannel SalesChannel { get; set; }

        #region Allors
        [Id("f2f85b74-b28f-4627-9dca-94142789c0bc")]
        [AssociationId("e1bf6299-0009-44ad-84d3-725df91d5f63")]
        [RoleId("e64f29b4-aa97-463f-acf1-fc9bd2a2bd8f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public Party[] Customers { get; set; }

        #region Allors
        [Id("f808aafb-3c7d-4a26-af5c-44b76ee45e86")]
        [AssociationId("d487d63e-8094-4085-bb73-d2f24e586c26")]
        [RoleId("462acdc2-69e1-42e5-ba10-6f74f04da7a5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("fd12507e-96b7-4b15-a43d-ab418d4795d6")]
        [AssociationId("b8044f1e-b8fa-42fc-995d-06ac47423b8e")]
        [RoleId("8dd43185-e3a9-44d7-ab1e-2a1222a234cf")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Store Store { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion

        #region Allors
        [Id("55A60B80-2052-47E6-BD41-2AF414ABB885")]
        #endregion
        [Workspace]
        public void Send() { }

        #region Allors
        [Id("96AF8F69-F1A4-420A-8D9D-AF61EB061620")]
        #endregion
        [Workspace]
        public void CancelInvoice() { }

        #region Allors
        [Id("F6EC229C-288C-4830-9DE5-8D5236DE4781")]
        #endregion
        [Workspace]
        public void WriteOff() { }
    }
}
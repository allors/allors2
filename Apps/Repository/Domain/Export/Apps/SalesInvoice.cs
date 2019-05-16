namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("6173fc23-115f-4356-a0ce-867872c151ac")]
    #endregion
    public partial class SalesInvoice : Invoice, Versioned, Localised, Deletable
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }
        
        public Currency Currency { get; set; }
        public string Description { get; set; }
        public ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }
        
        public Fee Fee { get; set; }
        
        public string CustomerReference { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TotalDiscount { get; set; }
        public BillingAccount BillingAccount { get; set; }
        public decimal TotalIncVat { get; set; }
        public decimal TotalSurcharge { get; set; }
        public decimal TotalBasePrice { get; set; }
        
        public DateTime InvoiceDate { get; set; }
        public DateTime EntryDate { get; set; }
        
        public decimal TotalShippingAndHandling { get; set; }
        
        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        public decimal TotalExVat { get; set; }
        public SalesTerm[] SalesTerms { get; set; }
        
        public string InvoiceNumber { get; set; }
        public string Message { get; set; }
        public VatRegime VatRegime { get; set; }
        
        public decimal TotalVat { get; set; }
        public decimal TotalFee { get; set; }
        public InvoiceItem[] ValidInvoiceItems { get; set; }

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public Locale Locale { get; set; }
        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public PrintDocument PrintDocument { get; set; }

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
        [Id("D18CE961-CC03-47E2-A179-A5D93E7C65CD")]
        [AssociationId("422EFE1A-1163-487D-92F6-9C84B8EB0AC6")]
        [RoleId("05B7F51F-4E58-4174-B51D-AE8AFC5C355D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation BilledFrom { get; set; }

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
        [Id("B7797410-75C3-4BE2-AD1A-5CF6D7438A72")]
        [AssociationId("77D32952-C469-4B5E-8E9F-949355188E3D")]
        [RoleId("E16BF028-0D06-4A42-A193-FA1753696A24")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BilledFromContactPerson { get; set; }

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
        [Id("2d0e924b-ff24-4630-9151-ac9bfc844c0c")]
        [AssociationId("0a159385-7570-494e-976d-4ee493235cb3")]
        [RoleId("239e91ee-5606-4131-a351-ebbd5908d9be")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Party PreviousBillToCustomer { get; set; }

        #region Allors
        [Id("8FCCF6B4-69EE-4087-8F42-9BDAC9971FA3")]
        [AssociationId("CFA8E6AC-66B7-4F14-A3D2-A848A8E53F40")]
        [RoleId("24A2D620-6EB8-4C60-87DC-02FAEDC2BD97")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("F03DD1C5-A2BA-4231-8274-7594E2AF5507")]
        [AssociationId("5F60DE58-1FBF-4975-B0F3-5E633EC658F4")]
        [RoleId("FBAF9CBF-8505-4302-B9CD-728CEB8CA90C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToContactPerson { get; set; }

        #region Allors
        [Id("B51570D0-05F5-45D1-B820-6FA693A60EB9")]
        [AssociationId("CFF27770-62ED-44FD-B3BD-B0ECAC5200CF")]
        [RoleId("316B4B5C-AAEB-4794-8823-9D1F69647AE8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party BillToEndCustomer { get; set; }

        #region Allors
        [Id("27faaa2c-d4db-4cab-aa04-8ec4997d73d2")]
        [AssociationId("2e9fab52-2029-4ee3-8eba-ffd9764bcf67")]
        [RoleId("9dd23ce4-d760-45af-94e4-c2ac94b0aea3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism BillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("B1DA9A4E-F969-44DC-BFD8-409FE5E33AA9")]
        [AssociationId("4256B0C4-8C58-4798-AFB6-3F2127B06A87")]
        [RoleId("14B4572A-BC72-4594-9C6A-35156169061B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("6AFCE702-9659-480B-A0C0-CC7FBC92E05E")]
        [AssociationId("D9D8EFE4-DF38-48CA-A680-A97E73E4C181")]
        [RoleId("7F6EFCA5-4189-4F48-9269-45FDB88EA108")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToEndCustomer { get; set; }

        #region Allors
        [Id("3A27066B-1EC4-4B72-9920-95E31C3539D9")]
        [AssociationId("5278ADC4-7C60-403C-B675-87FAA6B5892B")]
        [RoleId("4E2A4AE2-63FB-4F90-976D-7334F51A8D7B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("08A314AE-A3DF-4993-AE09-11E6E5A0B316")]
        [AssociationId("B52DA9AF-0E09-4E13-91DE-0DFB0C9D0BD5")]
        [RoleId("1C3F445B-1263-45F8-88DF-07F65E222321")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToEndCustomerContactPerson { get; set; }

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
        [Id("7f833ad2-3146-4660-a9d4-8a70d3ce01db")]
        [AssociationId("b466881e-156a-488f-9f26-c2850b7dd7fc")]
        [RoleId("aa621b67-049a-44e8-9f70-07e2a0c696b8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Party PreviousShipToCustomer { get; set; }

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
        [Id("D8127DD5-BD5C-4608-9D69-B2F585A45066")]
        [AssociationId("C3EFD659-8A91-4D22-8DCD-1B8AFBDC3E89")]
        [RoleId("2FFC7A31-C54D-4B97-B4E2-45860EF8628D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToContactPerson { get; set; }

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
        [Id("09064adb-7094-48e9-992c-2eab319d640f")]
        [AssociationId("5ade34c0-1f3c-4ecf-933d-72360173f03d")]
        [RoleId("17bb6982-04c0-42e8-9ae3-56bd50736cbb")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPrice { get; set; }

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
        [Id("C6F95A7F-C812-42A3-B215-7A110D9D6862")]
        [AssociationId("516841A8-3BC4-43F0-90AE-D72C71671F8A")]
        [RoleId("3FA3D203-E308-4B79-922B-39E9C4D560C5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseInvoice PurchaseInvoice { get; set; }

        #region Allors
        [Id("8F2706C3-445B-4B75-941E-A5E07CBEBF02")]
        [AssociationId("0E53F127-669D-4808-8D36-D0C9FBC64756")]
        [RoleId("79506919-A450-42F3-9109-250665C17A6A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public SalesOrder[] SalesOrders { get; set; }

        #region Allors
        [Id("91D368B2-D4BE-47F4-A5D7-DE122AB518C5")]
        [AssociationId("55BD82FA-16AC-432F-BB52-4927B621F256")]
        [RoleId("B3D4E558-25AD-4A3A-A345-DDA3C0BDC91B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Workspace]
        public WorkEffort[] WorkEfforts { get; set; }

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
        [Id("89557826-c9d1-4aa1-8789-79fb425cdb87")]
        [AssociationId("7d157e5a-efbb-453e-bd95-27a9b0ab305f")]
        [RoleId("751ada5f-ff41-43ae-8609-0c1457642375")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public SalesInvoiceItem[] SalesInvoiceItems { get; set; }
        
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
        [Id("fd12507e-96b7-4b15-a43d-ab418d4795d6")]
        [AssociationId("b8044f1e-b8fa-42fc-995d-06ac47423b8e")]
        [RoleId("8dd43185-e3a9-44d7-ab1e-2a1222a234cf")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Store Store { get; set; }

        #region Allors
        [Id("968409E5-9FA6-4085-9707-6ECF38A08272")]
        [AssociationId("6751C1AE-5EE4-41A9-AB1E-D61CA45DA56B")]
        [RoleId("F08C7F44-6ACC-4402-AB46-931410A6B0AB")]
        [Indexed]
        #endregion
        [Required]
        [Workspace]
        public bool IsRepeatingInvoice { get; set; }

        #region Allors
        [Id("CE515051-9555-4810-8175-17B152919C2A")]
        [AssociationId("5403D269-EFF6-40C5-A8D5-DA275601399C")]
        [RoleId("B341F11F-E2AB-48C7-B431-1ADED2F827FE")]
        [Indexed]
        #endregion
        [Required]
        [Workspace]
        public decimal AdvancePayment{ get; set; }

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

        #region Allors
        [Id("99F56814-6F90-4A6A-996B-84DDE7956544")]
        #endregion
        [Workspace]
        public void Copy() { }

        #region Allors
        [Id("1D9B28F2-1439-41F8-A556-59BDEFB4683E")]
        #endregion
        [Workspace]
        public void Credit() { }

        #region Allors
        [Id("6C9A6C2B-193A-48A8-9AAB-A6FFE2D64FC0")]
        #endregion
        [Workspace]
        public void Delete() { }

        #region Allors
        [Id("033FF876-BBC5-47B3-B2C9-CEDE9869C231")]
        #endregion
        [Workspace]
        public void Reopen() { }

        #region inherited methods
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
            
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Print() { }

        public void SetPaid() { }
        #endregion
    }
}
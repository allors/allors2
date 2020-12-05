// <copyright file="SalesInvoice.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("6173fc23-115f-4356-a0ce-867872c151ac")]
    #endregion
    public partial class SalesInvoice : Invoice, Versioned, Localised
    {
        #region inherited properties

        public Media[] ElectronicDocuments { get; set; }

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }

        public Currency AssignedCurrency { get; set; }

        public Currency DerivedCurrency { get; set; }

        public string Description { get; set; }

        public OrderAdjustment[] OrderAdjustments { get; set; }

        public string CustomerReference { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal TotalDiscount { get; set; }

        public BillingAccount BillingAccount { get; set; }

        public decimal TotalIncVat { get; set; }

        public decimal GrandTotal { get; set; }

        public decimal TotalSurcharge { get; set; }

        public decimal TotalBasePrice { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime EntryDate { get; set; }

        public decimal TotalShippingAndHandling { get; set; }

        public decimal TotalExVat { get; set; }

        public SalesTerm[] SalesTerms { get; set; }

        public string InvoiceNumber { get; set; }

        public string Message { get; set; }

        public VatRegime AssignedVatRegime { get; set; }

        public VatRegime DerivedVatRegime { get; set; }

        public decimal TotalVat { get; set; }

        public decimal TotalFee { get; set; }

        public decimal TotalExtraCharge { get; set; }

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

        public IrpfRegime AssignedIrpfRegime { get; set; }

        public IrpfRegime DerivedIrpfRegime { get; set; }

        public decimal TotalIrpf { get; set; }

        public int SortableInvoiceNumber { get; set; }

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
        public ContactMechanism AssignedBilledFromContactMechanism { get; set; }

        #region Allors
        [Id("d33afc39-5420-4970-ba8c-e8331d80bf47")]
        [AssociationId("5231ed2a-6c05-4ea3-9466-59299f1a9e4a")]
        [RoleId("1c0294d7-2b4c-4f26-a948-ca2317d3d243")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public ContactMechanism DerivedBilledFromContactMechanism { get; set; }

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
        [Workspace]
        public ContactMechanism AssignedBillToContactMechanism { get; set; }

        #region Allors
        [Id("d687515a-608b-4b6a-a403-73f872e7c94c")]
        [AssociationId("b269e3ed-cd5a-4c63-b08c-1ab49989a71f")]
        [RoleId("597ed3ec-12b3-4bce-8b87-22f34ebcbb54")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Derived]
        [Workspace]
        public ContactMechanism DerivedBillToContactMechanism { get; set; }

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
        public ContactMechanism AssignedBillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("07d9a1d1-b720-4b89-9711-d1293d40d7a6")]
        [AssociationId("56410bfd-0823-4e0a-92e6-167d4e359049")]
        [RoleId("ce31fb47-584c-47a9-8b0f-81039c55d3e3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public ContactMechanism DerivedBillToEndCustomerContactMechanism { get; set; }

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
        public PostalAddress AssignedShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("e2b9903b-f9b5-487e-9b92-17a599672d58")]
        [AssociationId("38fff4d2-a7da-46a0-bf57-34a5701e7fa5")]
        [RoleId("78e0f802-9bed-4ea4-91af-023ff50cc39f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public PostalAddress DerivedShipToEndCustomerAddress { get; set; }

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
        [Derived]
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
        public PostalAddress AssignedShipToAddress { get; set; }

        #region Allors
        [Id("0b355653-b252-4b55-8887-355e24b21308")]
        [AssociationId("4aa86e9d-c29e-47d7-8570-2caa534773cb")]
        [RoleId("2ab3175e-12f8-4b53-b0fc-5ffba48990df")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public PostalAddress DerivedShipToAddress { get; set; }

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
        public PaymentMethod AssignedPaymentMethod { get; set; }

        #region Allors
        [Id("1592d38d-61a7-4ecd-a59b-efb973c6b974")]
        [AssociationId("c02ed08d-980c-43e0-a47f-ce0acb720ac4")]
        [RoleId("9fa3d050-60dc-46fa-97aa-95111b3e4e86")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public PaymentMethod DerivedPaymentMethod { get; set; }

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
        [Derived]
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
        public decimal AdvancePayment { get; set; }

        #region Allors
        [Id("17ED2D23-0D51-4F04-BBD4-572318E91D82")]
        [AssociationId("74FF8692-8E25-4C2D-8E85-86A7B7242E82")]
        [RoleId("240DD9BB-89A9-4B37-B577-4E3B4D02E466")]
        [Indexed]
        #endregion
        [Derived]
        [Required]
        [Workspace]
        public int PaymentDays { get; set; }

        #region Allors
        [Id("DB18BDE8-D70F-4E24-8866-D1D46CB0D82B")]
        [AssociationId("BEC312FE-AB08-4717-8AE6-FF8A2B5F8B39")]
        [RoleId("F7B9A2F8-77C0-4219-874C-E2D8836FC25F")]
        #endregion
        [Derived]
        [Workspace]
        public DateTime DueDate { get; set; }

        #region Allors
        [Id("96D069CB-E463-4C94-A294-17A57D6CD418")]
        [AssociationId("4CE582B9-B1FA-4379-8D3E-63F91E319066")]
        [RoleId("F14D7BF1-0620-4350-BF20-FCB430E3F42B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public VatClause AssignedVatClause { get; set; }

        #region Allors
        [Id("4B8F6EB0-DCDC-4F6D-BFD8-7DE8CE159377")]
        [AssociationId("EE906D09-6027-4164-BDC3-83D6809E3AAD")]
        [RoleId("65633D16-E039-4A9A-A703-4904C07A7180")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public VatClause DerivedVatClause { get; set; }

        #region Allors
        [Id("c6d3c0f6-25b2-40d9-87d9-9924074a8ad1")]
        [AssociationId("6af55f84-c33f-416b-b66f-43e31537af4c")]
        [RoleId("c6fdbc99-ffdb-4947-9e2b-bf8946802581")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Derived]
        [Workspace]
        public Locale DerivedLocale { get; set; }

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
        [Id("033FF876-BBC5-47B3-B2C9-CEDE9869C231")]
        #endregion
        [Workspace]
        public void Reopen() { }

        #region Allors
        [Id("A2E784E3-B0D0-42FE-8E3C-7217E8948D95")]
        #endregion
        [Workspace]
        public void SetPaid() { }

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

        public void Delete() { }

        public void Create() { }

        public void Revise() { }

        #endregion
    }
}

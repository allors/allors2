// <copyright file="SalesInvoiceVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("0982A8D9-4F69-4F4A-A7C2-2AC7ABBE9924")]
    #endregion
    public partial class SalesInvoiceVersion : InvoiceVersion
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string InternalComment { get; set; }

        public Currency AssignedCurrency { get; set; }

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

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public IrpfRegime AssignedIrpfRegime { get; set; }

        public IrpfRegime DerivedIrpfRegime { get; set; }

        public decimal TotalIrpfInPreferredCurrency { get; set; }

        public decimal TotalExVatInPreferredCurrency { get; set; }

        public decimal TotalVatInPreferredCurrency { get; set; }

        public decimal TotalIncVatInPreferredCurrency { get; set; }

        public decimal GrandTotalInPreferredCurrency { get; set; }

        public decimal TotalSurchargeInPreferredCurrency { get; set; }

        public decimal TotalDiscountInPreferredCurrency { get; set; }

        public decimal TotalShippingAndHandlingInPreferredCurrency { get; set; }

        public decimal TotalFeeInPreferredCurrency { get; set; }

        public decimal TotalExtraChargeInPreferredCurrency { get; set; }

        public decimal TotalBasePriceInPreferredCurrency { get; set; }

        public decimal TotalListPriceInPreferredCurrency { get; set; }

        #endregion

        #region Allors
        [Id("1D302901-A2D1-4D13-945A-3F61A96E474B")]
        [AssociationId("B38DE94E-9711-403C-877D-D4E626169894")]
        [RoleId("1A5449ED-61C3-445B-BF9C-58A1C917BE96")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation BilledFrom { get; set; }

        #region Allors
        [Id("55772E8F-4911-4746-83F8-654F10859D6B")]
        [AssociationId("220143D4-E491-4759-ABA1-387A36976A69")]
        [RoleId("32518558-717F-458C-A371-093CC7B83887")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism AssignedBilledFromContactMechanism { get; set; }

        #region Allors
        [Id("b365caa0-82af-43c1-b9c2-4cab0e02c70e")]
        [AssociationId("998b344c-9bac-4a2b-9715-8037f7d53037")]
        [RoleId("7bb71a9d-01f8-48fa-af7a-2ea0d5a66ac0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism DerivedBilledFromContactMechanism { get; set; }

        #region Allors
        [Id("F1B241E8-26F6-4203-B1BF-E7F03942D76F")]
        [AssociationId("D866F4DA-44EA-4373-AAE0-41367CD5D5E9")]
        [RoleId("C7542A43-618C-41CB-BB10-8E6D3B9A2BCC")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BilledFromContactPerson { get; set; }

        #region Allors
        [Id("92C03EDC-A5B4-4AFD-BCAE-533FC0D77DA4")]
        [AssociationId("7AB0251A-2182-4F8A-A2CA-97D3AC0B5707")]
        [RoleId("D9FB8901-47D2-42B3-9F55-3B8F7976758E")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party BillToCustomer { get; set; }

        #region Allors
        [Id("C90E49CC-63DC-41A6-B5C9-24B38EFDD8E3")]
        [AssociationId("81EFC319-7002-4A09-B417-EB3DA1484134")]
        [RoleId("305868AF-F70A-486B-885C-650E7E2E17ED")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Party PreviousBillToCustomer { get; set; }

        #region Allors
        [Id("5A126FD7-5CFD-4633-8F8B-E41879A9AEA2")]
        [AssociationId("346326FD-3176-4F7F-B301-E1E006801B06")]
        [RoleId("ACDA393E-B5CB-402F-861D-4202EB92D94F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism AssignedBillToContactMechanism { get; set; }

        #region Allors
        [Id("696b52bf-0b72-4796-ab7e-1bb59c26a4f1")]
        [AssociationId("12425ab5-bbf8-4660-a0b5-b550ac5cb4ba")]
        [RoleId("c39bf18f-e0b0-4773-b8e1-b1b4093c05f3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism DerivedBillToContactMechanism { get; set; }

        #region Allors
        [Id("666B66B6-3271-47B4-A92E-DB26DE179F61")]
        [AssociationId("73CF4C1F-6DC4-4DBE-8E39-857E8583ED95")]
        [RoleId("BA9621FE-9FA3-4808-ABCF-2A9255A68E3D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToContactPerson { get; set; }

        #region Allors
        [Id("3901A676-CC14-4BA4-AA83-944C9B23DE11")]
        [AssociationId("45B60768-1742-497A-A4DF-FE5A6553EA73")]
        [RoleId("6264CA16-8031-4B97-8626-DC75D2FE9863")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party ShipToCustomer { get; set; }

        #region Allors
        [Id("C98647B8-E3DC-4EF9-9109-E7239D6AF534")]
        [AssociationId("DF2B1D5F-E901-42E0-9CF0-723BD3AA20A3")]
        [RoleId("2F5AA3D7-2324-4321-A14B-4262BD372FB1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Party PreviousShipToCustomer { get; set; }

        #region Allors
        [Id("417B7D53-AA3B-4881-A491-1FC539D2C013")]
        [AssociationId("078DC8A6-B8C9-4509-970C-CE5EEE1A9D4E")]
        [RoleId("74BECE1A-A07E-4CBC-A704-488F7A696FC6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipToAddress { get; set; }

        #region Allors
        [Id("c5e72ba5-ba0e-4558-88b1-3b3689f6af69")]
        [AssociationId("8da6cdca-60b2-4ae7-a054-19267a10d412")]
        [RoleId("fb051b36-bbef-4286-8483-577f25bea3ed")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress DerivedShipToAddress { get; set; }

        #region Allors
        [Id("D4DD296B-E609-4345-B51C-6FC3D15E8CD9")]
        [AssociationId("0C6C74F4-A6AD-4347-8CBF-B6160DBA5A19")]
        [RoleId("BA1C2C4F-5A29-4031-8643-77DEEA6034D8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToContactPerson { get; set; }

        #region Allors
        [Id("A90BD0E6-E6B4-4B39-BAF3-D890E5500582")]
        [AssociationId("A9E6AEC1-CF3E-48B5-8002-46BCAD07EBBC")]
        [RoleId("916FA6A2-C7AE-4143-8D91-FD34D04FB4A8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party BillToEndCustomer { get; set; }

        #region Allors
        [Id("A888A8F0-8E16-4318-BB09-9C195D716108")]
        [AssociationId("28E7F58F-AD46-4667-8CDB-B8C87DF89FDC")]
        [RoleId("D2163813-58B4-47B0-BA81-C04C76B0D97A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism AssignedBillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("864d5150-10ae-48cb-966c-7c3ae4dc37bc")]
        [AssociationId("edc39151-3159-4eb8-979d-ed5f7b5298d4")]
        [RoleId("f41a118d-cd70-48e6-adda-629b41b1da2f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism DerivedBillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("17B0CD8A-8F59-4C7A-B464-7ED1403788C9")]
        [AssociationId("F70D1262-23CE-4D5F-B7F1-476D4B618CE6")]
        [RoleId("D3E379B2-9561-4BB3-AFFC-133BE8F20D12")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("5CA9F52A-2C35-4BBF-8682-7D39E122FDD3")]
        [AssociationId("205D435B-6CA0-482A-979D-A885DC72455D")]
        [RoleId("80B571F7-636D-487D-B3D6-D663F6E9F6DE")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToEndCustomer { get; set; }

        #region Allors
        [Id("F24B4583-0031-4A87-8395-B0BB6122BE99")]
        [AssociationId("547165C5-FAE0-4A17-8042-01C2252E71A1")]
        [RoleId("6377E891-1D9D-45C3-9EF4-E4F1CC439670")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("85090aa2-13d3-4836-a647-e8e07fa5f30c")]
        [AssociationId("f0ee04ca-51f0-4cfa-a74d-4b1317e2ba56")]
        [RoleId("cda9a03c-e8d3-4cf9-a5fb-13c9e9d74d32")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress DerivedShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("1F2605FB-7D41-4691-8BE5-DAD06265702D")]
        [AssociationId("43498F13-CBE7-47F5-B868-A9A4CA57A5BC")]
        [RoleId("9040AD86-13B6-4C45-BC8D-DA123344DEDA")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("CEAFD664-2CA9-41B7-BC13-12B66AE8BD7C")]
        [AssociationId("B925DA62-4BEC-4BB9-9A09-9DECDDEBC9C4")]
        [RoleId("84CDA3B1-8290-4053-9AB2-A13D2AE7F4C5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SalesInvoiceState SalesInvoiceState { get; set; }

        #region Allors
        [Id("A1521081-CF30-4558-A73E-2E71287E9826")]
        [AssociationId("76AD97CF-8675-4F5F-8EB4-4EDFB1C5BC16")]
        [RoleId("8E9FAEA0-B75A-46F1-9C10-4FCD4DE686BB")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPrice { get; set; }

        #region Allors
        [Id("C4538A99-07A4-49CF-8DA5-A608626B1381")]
        [AssociationId("F3A68DF7-63FB-49D4-955F-FF79007685EA")]
        [RoleId("E98D3184-E065-4C3D-8A52-587E24FFCCC9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SalesInvoiceType SalesInvoiceType { get; set; }

        #region Allors
        [Id("5DF1979F-4121-432C-92E4-CD7794105F4C")]
        [AssociationId("591D6293-27DE-41ED-BC77-B07288444C2D")]
        [RoleId("559B7C56-D5A7-47E0-89DA-F4A4A3144D2B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PaymentMethod AssignedPaymentMethod { get; set; }

        #region Allors
        [Id("8f0b6875-e860-419d-bb9c-3a226a171528")]
        [AssociationId("adc59b43-bd23-4274-80f0-0c658f8d103b")]
        [RoleId("535e120c-c910-4aa0-88a0-37dee789048a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PaymentMethod DerivedPaymentMethod { get; set; }

        #region Allors
        [Id("3E595F3B-D845-4141-A4F8-E055B01AFDBE")]
        [AssociationId("0A113ECD-9E72-45FB-B642-5A0C124F0508")]
        [RoleId("946229B3-5952-4068-97B6-A6572FED9558")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public SalesInvoiceItem[] SalesInvoiceItems { get; set; }

        #region Allors
        [Id("33E0CEBA-6951-40F3-A5EA-BE0DD5217564")]
        [AssociationId("D1DCE009-6D9C-476C-8745-4B86E54C20BA")]
        [RoleId("77DFBD1A-DA8A-411B-B90C-7CFAEB480FB4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SalesChannel SalesChannel { get; set; }

        #region Allors
        [Id("A407D8AC-1951-4DCC-9224-399139D6AFEB")]
        [AssociationId("96463DA6-A6C2-4CE2-B65D-C4AAA5F7A52D")]
        [RoleId("083E74D4-8B16-4224-A509-76374FF01694")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Party[] Customers { get; set; }

        #region Allors
        [Id("B87EFDD6-DE6B-4432-AD22-5E3FBB0218FE")]
        [AssociationId("AA11DEBF-01DA-4CA0-8636-3B2EF16C486E")]
        [RoleId("852756F7-E901-4278-B461-60C72ACED7E9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Store Store { get; set; }

        #region Allors
        [Id("4D8D082D-2A8A-4D9B-9580-57E9A8166A57")]
        [AssociationId("0FA7DF62-8830-4D43-A639-2E5D76E6F9C4")]
        [RoleId("F914A6EE-F5DB-449D-B141-3AF3D5D847BD")]
        #endregion
        [Workspace]
        public int PaymentDays { get; set; }

        #region Allors
        [Id("F5DB1C05-8EDA-40AA-89EC-388394F8EEEA")]
        [AssociationId("8DF335B8-5A32-4032-992D-FBE234FE9D76")]
        [RoleId("8E2239DF-B6FC-4277-97AF-FE6608F83097")]
        #endregion
        public DateTime DueDate { get; set; }

        #region Allors
        [Id("906EB925-D04D-44F6-9E1C-13438200C1F4")]
        [AssociationId("7012361F-63A2-43DB-907B-CC09D2F4B451")]
        [RoleId("B52C8305-6135-46A7-84BB-D19385614D4C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public VatClause DerivedVatClause { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}

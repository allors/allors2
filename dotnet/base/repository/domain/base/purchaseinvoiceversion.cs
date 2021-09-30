// <copyright file="PurchaseInvoiceVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("C23DBDD0-8933-4582-8995-8767EFDA82D5")]
    #endregion
    public partial class PurchaseInvoiceVersion : InvoiceVersion
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
        [Id("277D12EB-729E-419A-A9EB-35F30DFFBA15")]
        [AssociationId("0B791CFD-9124-44AF-BC2B-D26A79FFF3F1")]
        [RoleId("2827C4E6-E649-48EA-86CD-C7B7689EB7A7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Party BilledFrom { get; set; }

        #region Allors
        [Id("57494359-88E5-4032-8E14-D568E191C281")]
        [AssociationId("139A375E-C55F-4060-9F69-5C624EE37491")]
        [RoleId("D9D68F8F-6849-456F-B762-13C80AF29B99")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism AssignedBilledFromContactMechanism { get; set; }

        #region Allors
        [Id("f7567ff7-a7dc-46db-b671-782aee127dd6")]
        [AssociationId("4a646aaa-b092-4653-9ed3-0d581619ce83")]
        [RoleId("316e7f77-abed-45f4-a53a-d298a64111f0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism DerivedBilledFromContactMechanism { get; set; }

        #region Allors
        [Id("2C2F790B-DC49-4133-BD8F-47917D9E5DC5")]
        [AssociationId("9DDB3A19-DDCD-44AA-8287-9DFC20E3AD74")]
        [RoleId("0382C35D-6D21-47C4-89AA-3244B977B4EA")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BilledFromContactPerson { get; set; }

        #region Allors
        [Id("004A1FE5-5EB7-470D-AD91-F62DBAA8E972")]
        [AssociationId("593AC76B-92B7-4E1C-92A5-46510A642A15")]
        [RoleId("5E947253-E0CF-4DF8-A90F-FD3939DDD083")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation BilledTo { get; set; }

        #region Allors
        [Id("A05AA071-B199-4B8D-A234-5A91435A14E7")]
        [AssociationId("6C90241F-E2B0-4857-99DC-F0422228132E")]
        [RoleId("6ADB73CE-B5FB-4027-AD0B-8313CCE09C5F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BilledToContactPerson { get; set; }

        #region Allors
        [Id("2565D7A0-93FF-4023-9CDD-8D85B9589B8E")]
        [AssociationId("44908EB5-5BB6-4C10-8107-C34F2A54989B")]
        [RoleId("D00E1864-9B3B-4FA9-9AE1-6C9BA60136C9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToCustomer { get; set; }

        #region Allors
        [Id("33BE845B-F955-4054-BB02-51D2FB49DF76")]
        [AssociationId("729ACD74-815F-4085-BE5B-C4E4E7F34EF7")]
        [RoleId("99260598-58CB-40EE-99F5-C7C9F69EC64A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PostalAddress AssignedShipToCustomerAddress { get; set; }

        #region Allors
        [Id("cb8b6da5-88d7-440f-997f-480ffb3630d6")]
        [AssociationId("c7f79895-7955-4073-a7fc-b8375a7e295f")]
        [RoleId("dcd508c6-c64b-46ef-a8ee-0a41ca887cbe")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PostalAddress DerivedShipToCustomerAddress { get; set; }

        #region Allors
        [Id("9B9F9513-E27A-4C35-8882-FFAFAD89221F")]
        [AssociationId("A81E823A-DAD4-4A16-BD75-D422D7B9B2D7")]
        [RoleId("2F70AE94-06F6-4440-863A-1BFB947262CE")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToCustomerContactPerson { get; set; }

        #region Allors
        [Id("499721BD-3B97-47BE-8BF6-0A030D9E1EF4")]
        [AssociationId("F8F83C87-2ACD-4C92-BAF3-B6B5C50A4337")]
        [RoleId("CAC2DF45-2406-4612-BF86-BE9E56DE4696")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party BillToEndCustomer { get; set; }

        #region Allors
        [Id("BFB058D0-945E-4EAA-B3F4-1C375BB8433B")]
        [AssociationId("EE77D411-2EF8-43EF-AC76-7B61EAF0E758")]
        [RoleId("9844CA83-259F-47EB-9868-98BAB206651D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism AssignedBillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("dbfc3d12-de43-451c-beb6-c1aaa4d8d8e4")]
        [AssociationId("ec0fab5b-f51e-41b1-acdb-9e1c498c8608")]
        [RoleId("369b41f0-f72f-4766-a8ae-6d0f78beb3e0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism DerivedBillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("A10C9C5C-A631-4922-A6CC-183659AACA3C")]
        [AssociationId("9C23719F-64BB-4F19-8E0C-2DDF1CDCEB77")]
        [RoleId("1024253B-0D7B-4DB3-98AB-CCC59E45C8A2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("A2F6B49E-2DC6-4690-98CC-692076F06AE8")]
        [AssociationId("0B2193AA-FA92-4582-9131-56A58DD95425")]
        [RoleId("62FA5015-C01A-4DAD-9B39-5DBD7A640594")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PaymentMethod AssignedBillToCustomerPaymentMethod { get; set; }

        #region Allors
        [Id("ac7eac93-e9fd-442a-b2d9-9c9b9ff0a38f")]
        [AssociationId("679bb57f-af0c-4475-8b34-f9cbc6611090")]
        [RoleId("5cad0de6-04aa-411c-b79b-6230a3a0ba9d")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PaymentMethod DerivedBillToCustomerPaymentMethod { get; set; }

        #region Allors
        [Id("C17CCA21-98E3-4919-A10C-2A1DB169C8E9")]
        [AssociationId("DB0EF67B-79D5-4BB3-90BA-CF5562B8DD63")]
        [RoleId("4C09FD21-FD73-4E69-8E03-B0FE4DC9BD1A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToEndCustomer { get; set; }

        #region Allors
        [Id("C9D07662-ED11-4A82-92C9-B1731BD9DDE9")]
        [AssociationId("C79EE6E3-9C7B-4627-8643-F57725954E10")]
        [RoleId("7972673F-FCF4-4922-8730-F078FC69B192")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("569996ca-4f7e-43aa-b55e-9caced64d9b0")]
        [AssociationId("1aa590d7-acf9-419d-8466-5acc84ecba57")]
        [RoleId("c763aa34-3c2b-4d10-83b4-6010e8aeef8a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress DerivedShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("689A34F7-F438-4C20-BAD1-E9F8A36CFA6C")]
        [AssociationId("72B2CBB4-F220-4DF3-9E98-A091539CE9A8")]
        [RoleId("87105F3F-51D8-4007-A2C5-158865A691AC")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("7751055A-3C59-4723-B7DF-42C377624BE0")]
        [AssociationId("5FB242DC-FB8B-4347-A0F2-65BCC0BBC056")]
        [RoleId("DAF79CD7-EAD0-4091-8545-24ADEEC919AF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public PurchaseInvoiceState PurchaseInvoiceState { get; set; }

        #region Allors
        [Id("65E39E93-6445-459A-99E8-0ED388B85B4B")]
        [AssociationId("AC00032A-8EE6-4B18-B387-44A02AD8F1A0")]
        [RoleId("7DAB2DA2-EFE4-4B91-B425-07FB2A59B216")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public PurchaseInvoiceItem[] PurchaseInvoiceItems { get; set; }

        #region Allors
        [Id("4A072DD0-0886-4CFF-9DC4-D213854160E7")]
        [AssociationId("741297D3-BB5D-4789-986C-509B7D95A6EC")]
        [RoleId("CD477E7F-C66A-477E-BC30-EEC813CC0211")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public PurchaseInvoiceType PurchaseInvoiceType { get; set; }

        #region Allors
        [Id("B3EEBEFD-F12E-4A70-A0DD-A58C89491C03")]
        [AssociationId("592E9F43-C799-48CA-BE4F-B7272D11037B")]
        [RoleId("4A1AD446-2330-4681-89F8-2651B2BF9D61")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        public PurchaseOrder[] PurchaseOrders { get; set; }

        #region Allors
        [Id("635FC89C-3AD3-4661-BD24-2EA65002940F")]
        [AssociationId("1D60D829-0348-42F2-977B-BF9DA3631629")]
        [RoleId("E1F530C1-1579-4671-904F-A6A3821E4E17")]
        #endregion
        public DateTime DueDate { get; set; }

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

// <copyright file="SalesOrderVersion.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Allors.Repository.Attributes;

    #region Allors
    [Id("FDD29AA9-D2F5-4FA0-8F32-08AD09505577")]
    #endregion
    public partial class SalesOrderVersion : OrderVersion
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

        public string CustomerReference { get; set; }

        public OrderAdjustment[] OrderAdjustments { get; set; }

        public decimal TotalExVat { get; set; }

        public SalesTerm[] SalesTerms { get; set; }

        public decimal TotalVat { get; set; }

        public decimal TotalSurcharge { get; set; }

        public OrderItem[] ValidOrderItems { get; set; }

        public string OrderNumber { get; set; }

        public decimal TotalDiscount { get; set; }

        public string Message { get; set; }

        public DateTime EntryDate { get; set; }

        public OrderKind OrderKind { get; set; }

        public decimal TotalIncVat { get; set; }

        public decimal GrandTotal { get; set; }

        public VatRegime AssignedVatRegime { get; set; }

        public VatRegime DerivedVatRegime { get; set; }

        public decimal TotalShippingAndHandling { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public decimal TotalBasePrice { get; set; }

        public decimal TotalFee { get; set; }

        public decimal TotalExtraCharge { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public IrpfRegime AssignedIrpfRegime { get; set; }

        public IrpfRegime DerivedIrpfRegime { get; set; }

        public decimal TotalIrpf { get; set; }

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
        [Id("DDF41C7B-BF5F-4F60-B39C-5618AB328C42")]
        [AssociationId("0C168DFD-C188-4643-B4B2-3AA770AA4235")]
        [RoleId("E1A66FF1-3308-4413-B5A7-8341FFAAE664")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public ContactMechanism AssignedTakenByContactMechanism { get; set; }

        #region Allors
        [Id("0a5b1349-898a-42a5-ab8a-66e15cd75424")]
        [AssociationId("a4977ae2-2bb7-449a-98c3-204b1f55b361")]
        [RoleId("10c93413-58be-4f2f-9388-57bb78c1a020")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public ContactMechanism DerivedTakenByContactMechanism { get; set; }

        #region Allors
        [Id("7CA78BDF-9FE3-4494-948B-758CCFBCDE32")]
        [AssociationId("266CB723-D1AA-4A9E-8944-CB6254DF906B")]
        [RoleId("78951DAA-929A-4F5C-8C17-15D989078870")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public Person TakenByContactPerson { get; set; }

        #region Allors
        [Id("E665DCB6-6E92-43BA-8E16-EF893F938292")]
        [AssociationId("07907501-28BC-419A-9EC7-94FE8FAC9C15")]
        [RoleId("B31F3564-C8C7-4481-AACF-FE6B2BB21829")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public SalesOrderState SalesOrderState { get; set; }

        #region Allors
        [Id("CC560B2D-BC23-4CE9-A17D-72C155171FE8")]
        [AssociationId("685FFD5D-F2F3-4E59-83C5-A157AED69092")]
        [RoleId("E3DE1799-BE55-48F3-B0EB-4F1C2F0132D5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderShipmentState SalesOrderShipmentState { get; set; }

        #region Allors
        [Id("9E3A522A-7F6E-4999-A305-CBBD4CDBB8C4")]
        [AssociationId("9C76D7DF-3AE7-471D-94B9-37832FA89E09")]
        [RoleId("7B58D05B-3C9B-4365-B41C-D9F34D60C63E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderInvoiceState SalesOrderInvoiceState { get; set; }

        #region Allors
        [Id("3DA584E0-FAC1-4D24-8E5C-1F631AF2CF52")]
        [AssociationId("79B8255A-C90A-42A0-AE26-684558BBA9DD")]
        [RoleId("5463FAEB-1181-4A5E-9550-07C2A8F6ABC8")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderPaymentState SalesOrderPaymentState { get; set; }

        #region Allors
        [Id("943F35C6-F418-4BAD-9F30-ABEF4F19DB48")]
        [AssociationId("7D854417-AB88-4DB6-9345-A1D0C293A7A2")]
        [RoleId("E2FA3139-2B44-4CA8-9F4D-BFB7A9BEED6D")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipFromAddress { get; set; }

        #region Allors
        [Id("65e2e3a5-3138-4996-9149-566d72ced8ec")]
        [AssociationId("946fcc76-1c87-47b4-ae6b-7ae1cf81d93e")]
        [RoleId("9ce0bbd9-c469-4377-a84a-2fd10e5bb9da")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress DerivedShipFromAddress { get; set; }

        #region Allors
        [Id("13CBB0CC-126E-4A1F-B873-CF48B6BAA869")]
        [AssociationId("5CE6A286-BAC2-4F13-8B4C-841E5690E965")]
        [RoleId("A2388761-37B7-4AB8-A3EF-D88E2BAA74BB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party ShipToCustomer { get; set; }

        #region Allors
        [Id("6A3438D1-2259-4443-A7ED-7B5BE5B7D897")]
        [AssociationId("37C32179-B695-421F-9E2C-D688CE5FB704")]
        [RoleId("B1C9B8CB-0D9B-4131-BA82-A39382DBFEB8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Party PreviousShipToCustomer { get; set; }

        #region Allors
        [Id("428F36FF-9B95-4742-8EB9-83107C87B088")]
        [AssociationId("95CBDF7E-08B2-4AD2-A471-CFB01C9325E9")]
        [RoleId("DA22DD44-069E-443B-851A-5D553F93E8FF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipToAddress { get; set; }

        #region Allors
        [Id("d13d2f66-8d64-4d88-9bef-aad023c178a6")]
        [AssociationId("5ba8e3cd-a0c6-4ca4-a267-68d6cff4935d")]
        [RoleId("a93d7da3-f9e8-4312-821d-b01eb2edbedb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress DerivedShipToAddress { get; set; }

        #region Allors
        [Id("CDA57AE8-DB34-4FBB-94C3-3BEDD1771077")]
        [AssociationId("7EA2D188-D4D7-4C66-8686-500086DD6829")]
        [RoleId("9C7F053A-3BA4-4C93-81CA-A2A3BB1FA877")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToContactPerson { get; set; }

        #region Allors
        [Id("F8D33874-EB71-4FF3-9B13-9CCBEF7EC698")]
        [AssociationId("CB85B78D-F002-4C83-963F-1889B2FDD09F")]
        [RoleId("16C08118-F6D0-4754-9F61-BC4F2F42197A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public Party BillToCustomer { get; set; }

        #region Allors
        [Id("E1AD653D-7C83-4F3D-879C-EF3053DF1288")]
        [AssociationId("D71CDBDD-FEA5-48C7-988A-DCDF357A95AF")]
        [RoleId("5B9CC284-64D9-4F57-8D17-494D70885DD7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Party PreviousBillToCustomer { get; set; }

        #region Allors
        [Id("A23756EA-C46A-4E6B-B637-4FEDAD3B8FDD")]
        [AssociationId("5873EA8E-5F89-4476-83EF-B544DC6F5C24")]
        [RoleId("6C9D8EF0-966B-4C6F-81B7-81308E41EC72")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism AssignedBillToContactMechanism { get; set; }

        #region Allors
        [Id("7c7187bb-78fb-4275-a758-88b9eb7f797f")]
        [AssociationId("443e12e1-413c-4255-bd08-b91c042f9e45")]
        [RoleId("aaa85aec-808a-4df1-bf85-6e844faa0874")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism DerivedBillToContactMechanism { get; set; }

        #region Allors
        [Id("218C89A0-0250-42B0-BFA4-97F0909053C6")]
        [AssociationId("2F121C97-614D-4EBC-8D3E-05C53C5EE81F")]
        [RoleId("1FA0F0A9-B55E-4E97-B326-33EF6FAA5284")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Person BillToContactPerson { get; set; }

        #region Allors
        [Id("F9D9A7E8-CFD7-43E3-9F75-60CBD4DA10E6")]
        [AssociationId("34B2511C-5BF1-451A-8B17-F5608AF329E9")]
        [RoleId("E9219CA9-5325-42DA-8BE6-91FFE9F1A622")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party BillToEndCustomer { get; set; }

        #region Allors
        [Id("74750CA9-3586-42E3-92CB-E0602D62EE88")]
        [AssociationId("8066761B-0B5E-4552-A9FF-F61962BF7E85")]
        [RoleId("FB157007-DDDC-410C-9679-43B3B853913B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism AssignedBillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("0c9f2477-c158-4484-9c70-ab96b27552ab")]
        [AssociationId("3ee74504-9003-4ea3-8524-a95ae6fa431c")]
        [RoleId("d807490c-ddcf-471c-9147-e7716d70039f")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism DerivedBillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("1DB7F93E-215F-4786-9E12-F3F36F2FA01A")]
        [AssociationId("C5596743-E8BB-460E-81F8-CE759ED9C758")]
        [RoleId("A9EA6C08-1C0D-4CBB-8763-AA2BED2BE3DE")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("1A4D1494-2F21-4FBD-99DF-E8F756CA0274")]
        [AssociationId("BB664F03-F62D-47E2-BA12-C5C60BADF427")]
        [RoleId("E712A192-B099-4486-B6C8-7432FF133E90")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToEndCustomer { get; set; }

        #region Allors
        [Id("6DE36A70-AC5A-402E-8CB1-AF23BB1247D2")]
        [AssociationId("2DA0EAC5-ED8C-49E2-AAA0-27FAE5707366")]
        [RoleId("4E4EEAE0-7EE4-4C6A-B802-AD949B03BACB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("20351a45-6ebb-43cb-ab11-e4697e1e4683")]
        [AssociationId("5449bd2c-9712-4bc2-8e10-3fa193aae3a2")]
        [RoleId("8ad85c7c-43e9-49bb-b348-e0ed2929d135")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress DerivedShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("9BC22E95-660A-46E6-AEB8-5B552A531E98")]
        [AssociationId("A2465545-E8FD-429D-AC6C-BC36C99862CC")]
        [RoleId("252ABAD1-3D6B-407D-829F-991CFF605C08")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToEndCustomerContactPerson { get; set; }

        #region Allors
        [Id("C8FABEAB-4F58-4D30-8CDB-3532E7E88EAF")]
        [AssociationId("CE8CC5D3-5B6D-4E40-A83F-C043F0C16FA8")]
        [RoleId("90F3309F-14A3-4FB2-A6BC-5BA1FAB06BA9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party PlacingCustomer { get; set; }

        #region Allors
        [Id("9DCE181F-7EA5-44CD-BB6D-C739153410BC")]
        [AssociationId("B6491023-64C4-4C52-8ED4-24D2A4183172")]
        [RoleId("EEEBC76B-ED9B-422F-9898-496E8272640F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism AssignedPlacingCustomerContactMechanism { get; set; }

        #region Allors
        [Id("5c8c869d-8367-400a-a9de-30cfcb0d47da")]
        [AssociationId("6633e942-532f-499d-b1c8-1b6d5b5ae579")]
        [RoleId("7189c6b0-f9b8-4dd9-8650-5387bdf71cb9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism DerivedPlacingCustomerContactMechanism { get; set; }

        #region Allors
        [Id("E325F42E-3532-4578-A86C-DC36B6634289")]
        [AssociationId("7DAE0F96-1B75-4139-B53F-7A7D0AD3ED08")]
        [RoleId("A860B161-BF78-4AB6-8B40-AED73B330FD5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person PlacingCustomerContactPerson { get; set; }

        #region Allors
        [Id("9A852FDF-F111-4FE9-B980-B0DF377B87FB")]
        [AssociationId("6C4C5B0A-98AA-4420-B197-4FDF10D26CBB")]
        [RoleId("92FED72A-F153-403B-8AEB-D6031FEF16A9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ShipmentMethod AssignedShipmentMethod { get; set; }

        #region Allors
        [Id("f1f51759-7673-48a8-bc95-a504d6b15cb6")]
        [AssociationId("d2a44007-dce9-4198-a298-99b84c2db03b")]
        [RoleId("8b326774-bbf2-4602-9746-600d8e7e6d27")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ShipmentMethod DerivedShipmentMethod { get; set; }

        #region Allors
        [Id("896D1356-CEBA-4DB2-8960-92F4DD6DEE62")]
        [AssociationId("B4B96708-D125-4F5C-AE52-D4F9DA31B2D3")]
        [RoleId("BCEE3E78-C94B-4D95-8E2B-496CF8642605")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPrice { get; set; }

        #region Allors
        [Id("E1A92EFA-406F-4A47-906A-BB2FCBD581B4")]
        [AssociationId("FF94D40F-0C30-4415-B676-65CD1310EE08")]
        [RoleId("5081007D-3AD8-4443-9F9D-9E59DE9C8223")]
        #endregion
        public bool PartiallyShip { get; set; }

        #region Allors
        [Id("F5BB7592-6BF9-4190-BAE2-C1C092F92EB5")]
        [AssociationId("1381071F-E30F-4F90-950F-8CCC8E679D10")]
        [RoleId("83D8D69D-547F-4A66-B2FF-FCD0B2B7E265")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public Party[] Customers { get; set; }

        #region Allors
        [Id("C101196C-6DA1-4B1C-885E-5B0967CE1DC9")]
        [AssociationId("58F1B956-40C4-4952-AF16-E1D1988A613B")]
        [RoleId("E9A535BF-CCB5-41B2-B8F4-364AA9DA3019")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Store Store { get; set; }

        #region Allors
        [Id("AF9225E8-CE1A-4FEE-9D05-476D2810C9CF")]
        [AssociationId("D614AA16-1119-44F8-B566-571DD9BFA665")]
        [RoleId("745CD1C7-D51C-4093-8276-50548AC49BFD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PaymentMethod AssignedPaymentMethod { get; set; }

        #region Allors
        [Id("9f2b8264-2411-448b-9d50-6525b52f3932")]
        [AssociationId("1bb39b2a-4c2f-4d61-97cd-0385bc6866b0")]
        [RoleId("902e3b35-de68-42c2-9182-44478c7fdd9e")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PaymentMethod DerivedPaymentMethod { get; set; }

        #region Allors
        [Id("CCFFB39F-7F02-4C6E-B75C-F348DEB37DC7")]
        [AssociationId("6DFA9054-1444-470B-A02A-A33F1BD3BB20")]
        [RoleId("1D16EFD9-38EB-4155-ABED-400F0BFB5BB2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public SalesChannel SalesChannel { get; set; }

        #region Allors
        [Id("A4E43616-87BE-4FFE-89FA-B8F7AB2A0C74")]
        [AssociationId("173DF1DF-2A3F-443F-9F57-5DFE627D5945")]
        [RoleId("FE8332FD-F397-4977-ACD0-F4E174849327")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SalesInvoice ProformaInvoice { get; set; }

        #region Allors
        [Id("BCFB58A6-840A-4AB3-8145-BED773459CD3")]
        [AssociationId("DB0A3DA9-1A19-4A22-B8D2-B6D342132D29")]
        [RoleId("EAE4AFD8-2A9A-4786-BF44-75A316920E1C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public SalesOrderItem[] SalesOrderItems { get; set; }

        #region Allors
        [Id("C440AF2E-CB6A-4614-B0D8-9D09DFD5E568")]
        [AssociationId("0611384B-1B81-4A9E-9301-E1521B24434F")]
        [RoleId("FAFB9B2B-32C6-4154-87B3-4FDB6064DCF4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ProductQuote Quote { get; set; }

        #region Allors
        [Id("11698673-09EA-480F-B287-3D742FC89A93")]
        [AssociationId("530060AF-6B2B-4332-B20F-7693717C509B")]
        [RoleId("DF713725-3C65-4128-BB88-79885D113034")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Facility OriginFacility { get; set; }

        #region Allors
        [Id("20518386-6E32-4521-A97E-E5654E06D1E1")]
        [AssociationId("4F97C053-CEC7-4CE2-B5DE-D3F6FD25430D")]
        [RoleId("FCEF8AB3-7D43-4ADB-9C2A-602E29129D9E")]
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

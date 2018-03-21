namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("FDD29AA9-D2F5-4FA0-8F32-08AD09505577")]
    #endregion
    public partial class SalesOrderVersion : OrderVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string InternalComment { get; set; }
        public Currency Currency { get; set; }
        public decimal TotalBasePriceCustomerCurrency { get; set; }
        public decimal TotalIncVatCustomerCurrency { get; set; }
        public decimal TotalDiscountCustomerCurrency { get; set; }
        public string CustomerReference { get; set; }
        public Fee Fee { get; set; }
        public decimal TotalExVat { get; set; }
        public SalesTerm[] SalesTerms { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalSurcharge { get; set; }
        public OrderItem[] ValidOrderItems { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalVatCustomerCurrency { get; set; }
        public decimal TotalDiscount { get; set; }
        public string Message { get; set; }
        public decimal TotalShippingAndHandlingCustomerCurrency { get; set; }
        public DateTime EntryDate { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public OrderKind OrderKind { get; set; }
        public decimal TotalIncVat { get; set; }
        public decimal TotalSurchargeCustomerCurrency { get; set; }
        public VatRegime VatRegime { get; set; }
        public decimal TotalFeeCustomerCurrency { get; set; }
        public decimal TotalShippingAndHandling { get; set; }
        public ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalExVatCustomerCurrency { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalFee { get; set; }
        public SurchargeAdjustment SurchargeAdjustment { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("E665DCB6-6E92-43BA-8E16-EF893F938292")]
        [AssociationId("07907501-28BC-419A-9EC7-94FE8FAC9C15")]
        [RoleId("B31F3564-C8C7-4481-AACF-FE6B2BB21829")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
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
        [Id("86036639-7EF0-4BC0-A290-FCB6CED11429")]
        [AssociationId("01C7103C-E204-40FD-88FE-899F5A94024F")]
        [RoleId("144F76A1-B96F-4940-A251-7C91BFB55532")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public ContactMechanism TakenByContactMechanism { get; set; }

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
        [Id("F8D33874-EB71-4FF3-9B13-9CCBEF7EC698")]
        [AssociationId("CB85B78D-F002-4C83-963F-1889B2FDD09F")]
        [RoleId("16C08118-F6D0-4754-9F61-BC4F2F42197A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public Party BillToCustomer { get; set; }

        #region Allors
        [Id("A23756EA-C46A-4E6B-B637-4FEDAD3B8FDD")]
        [AssociationId("5873EA8E-5F89-4476-83EF-B544DC6F5C24")]
        [RoleId("6C9D8EF0-966B-4C6F-81B7-81308E41EC72")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism BillToContactMechanism { get; set; }

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
        public ContactMechanism BillToEndCustomerContactMechanism { get; set; }

        #region Allors
        [Id("D9CB30F2-D004-46A7-A6CE-858DFA2767CF")]
        [AssociationId("D5D55E32-5549-4FD3-8A92-F72D2F090151")]
        [RoleId("BCCD0621-DA52-41A8-91C4-67AF57AA8914")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalPurchasePrice { get; set; }

        #region Allors
        [Id("9A852FDF-F111-4FE9-B980-B0DF377B87FB")]
        [AssociationId("6C4C5B0A-98AA-4420-B197-4FDF10D26CBB")]
        [RoleId("92FED72A-F153-403B-8AEB-D6031FEF16A9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ShipmentMethod ShipmentMethod { get; set; }

        #region Allors
        [Id("A07A5A7F-C93F-4272-AC34-5634E724885B")]
        [AssociationId("1B8EE600-23DE-47B9-BAE9-675FC4A4C7DF")]
        [RoleId("5778D5DD-C3B6-4960-8907-A283EF4385D3")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPriceCustomerCurrency { get; set; }

        #region Allors
        [Id("D6D89DE7-F1F8-4D68-907D-FFE3C41C2BE4")]
        [AssociationId("B77AEEDC-E356-457F-9030-C10DB1A81D4F")]
        [RoleId("98D2944F-6380-4DD2-8C21-F308D3089C34")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal MaintainedProfitMargin { get; set; }

        #region Allors
        [Id("428F36FF-9B95-4742-8EB9-83107C87B088")]
        [AssociationId("95CBDF7E-08B2-4AD2-A471-CFB01C9325E9")]
        [RoleId("DA22DD44-069E-443B-851A-5D553F93E8FF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("6A3438D1-2259-4443-A7ED-7B5BE5B7D897")]
        [AssociationId("37C32179-B695-421F-9E2C-D688CE5FB704")]
        [RoleId("B1C9B8CB-0D9B-4131-BA82-A39382DBFEB8")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Party PreviousShipToCustomer { get; set; }

        #region Allors
        [Id("DEF7B6D8-0704-4CE0-80A4-3B1E23671B20")]
        [AssociationId("13C6727C-DF28-4E64-94BF-D57BD4CF01D7")]
        [RoleId("1445DB47-368E-425C-BEB1-C943C5AD3FDB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Workspace]
        [Derived]
        [Indexed]
        public Person[] SalesReps { get; set; }

        #region Allors
        [Id("4644AA05-40B0-4B66-90A8-6B2D5156FF1F")]
        [AssociationId("D7264C89-EEBF-4A49-9870-18D8F5294983")]
        [RoleId("93E3BD24-078E-44A5-9498-1CDCA73B39BD")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal InitialProfitMargin { get; set; }

        #region Allors
        [Id("896D1356-CEBA-4DB2-8960-92F4DD6DEE62")]
        [AssociationId("B4B96708-D125-4F5C-AE52-D4F9DA31B2D3")]
        [RoleId("BCEE3E78-C94B-4D95-8E2B-496CF8642605")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPrice { get; set; }

        #region Allors
        [Id("E1A92EFA-406F-4A47-906A-BB2FCBD581B4")]
        [AssociationId("FF94D40F-0C30-4415-B676-65CD1310EE08")]
        [RoleId("5081007D-3AD8-4443-9F9D-9E59DE9C8223")]
        #endregion
        [Required]
        public bool PartiallyShip { get; set; }

        #region Allors
        [Id("F5BB7592-6BF9-4190-BAE2-C1C092F92EB5")]
        [AssociationId("1381071F-E30F-4F90-950F-8CCC8E679D10")]
        [RoleId("83D8D69D-547F-4A66-B2FF-FCD0B2B7E265")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        public Party[] Customers { get; set; }

        #region Allors
        [Id("C101196C-6DA1-4B1C-885E-5B0967CE1DC9")]
        [AssociationId("58F1B956-40C4-4952-AF16-E1D1988A613B")]
        [RoleId("E9A535BF-CCB5-41B2-B8F4-364AA9DA3019")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public Store Store { get; set; }

        #region Allors
        [Id("F925342E-1132-4E73-B38A-CE37FF5E61CE")]
        [AssociationId("E61FD96B-DDB2-468B-AF43-A6F2B13F6DCB")]
        [RoleId("FBD9EF51-F5A4-44D3-87BD-D88FEF275180")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal MaintainedMarkupPercentage { get; set; }

        #region Allors
        [Id("DDF41C7B-BF5F-4F60-B39C-5618AB328C42")]
        [AssociationId("0C168DFD-C188-4643-B4B2-3AA770AA4235")]
        [RoleId("E1A66FF1-3308-4413-B5A7-8341FFAAE664")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Indexed]
        public ContactMechanism BillFromContactMechanism { get; set; }

        #region Allors
        [Id("AF9225E8-CE1A-4FEE-9D05-476D2810C9CF")]
        [AssociationId("D614AA16-1119-44F8-B566-571DD9BFA665")]
        [RoleId("745CD1C7-D51C-4093-8276-50548AC49BFD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PaymentMethod PaymentMethod { get; set; }

        #region Allors
        [Id("9DCE181F-7EA5-44CD-BB6D-C739153410BC")]
        [AssociationId("B6491023-64C4-4C52-8ED4-24D2A4183172")]
        [RoleId("EEEBC76B-ED9B-422F-9898-496E8272640F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism PlacingContactMechanism { get; set; }

        #region Allors
        [Id("E1AD653D-7C83-4F3D-879C-EF3053DF1288")]
        [AssociationId("D71CDBDD-FEA5-48C7-988A-DCDF357A95AF")]
        [RoleId("5B9CC284-64D9-4F57-8D17-494D70885DD7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Party PreviousBillToCustomer { get; set; }

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
        [Id("C8FABEAB-4F58-4D30-8CDB-3532E7E88EAF")]
        [AssociationId("CE8CC5D3-5B6D-4E40-A83F-C043F0C16FA8")]
        [RoleId("90F3309F-14A3-4FB2-A6BC-5BA1FAB06BA9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party PlacingCustomer { get; set; }

        #region Allors
        [Id("A4E43616-87BE-4FFE-89FA-B8F7AB2A0C74")]
        [AssociationId("173DF1DF-2A3F-443F-9F57-5DFE627D5945")]
        [RoleId("FE8332FD-F397-4977-ACD0-F4E174849327")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
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
        [Id("8344BD0A-1537-4B19-BBD7-F4448B6E987D")]
        [AssociationId("EBE0B829-C153-42A8-A356-3416E91112DF")]
        [RoleId("98A278FF-6C83-41E0-8C9A-F811AD5714B4")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal InitialMarkupPercentage { get; set; }

        #region Allors
        [Id("C440AF2E-CB6A-4614-B0D8-9D09DFD5E568")]
        [AssociationId("0611384B-1B81-4A9E-9301-E1521B24434F")]
        [RoleId("FAFB9B2B-32C6-4154-87B3-4FDB6064DCF4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ProductQuote Quote { get; set; }
        
        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
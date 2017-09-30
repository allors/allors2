namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("0982A8D9-4F69-4F4A-A7C2-2AC7ABBE9924")]
    #endregion
    public partial class SalesInvoiceVersion : InvoiceVersion
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

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("CEAFD664-2CA9-41B7-BC13-12B66AE8BD7C")]
        [AssociationId("B925DA62-4BEC-4BB9-9A09-9DECDDEBC9C4")]
        [RoleId("84CDA3B1-8290-4053-9AB2-A13D2AE7F4C5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
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
        [Id("A888A8F0-8E16-4318-BB09-9C195D716108")]
        [AssociationId("28E7F58F-AD46-4667-8CDB-B8C87DF89FDC")]
        [RoleId("D2163813-58B4-47B0-BA81-C04C76B0D97A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("C90E49CC-63DC-41A6-B5C9-24B38EFDD8E3")]
        [AssociationId("81EFC319-7002-4A09-B417-EB3DA1484134")]
        [RoleId("305868AF-F70A-486B-885C-650E7E2E17ED")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Party PreviousBillToCustomer { get; set; }

        #region Allors
        [Id("C4538A99-07A4-49CF-8DA5-A608626B1381")]
        [AssociationId("F3A68DF7-63FB-49D4-955F-FF79007685EA")]
        [RoleId("E98D3184-E065-4C3D-8A52-587E24FFCCC9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public SalesInvoiceType SalesInvoiceType { get; set; }

        #region Allors
        [Id("2F0029E9-30CE-4BF9-8DE0-52272AB7CD11")]
        [AssociationId("ED254E03-75B9-4764-AE50-8BAC61ACF6C9")]
        [RoleId("8165C1C1-E413-4845-A799-A51B0C2AD367")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal InitialProfitMargin { get; set; }

        #region Allors
        [Id("5DF1979F-4121-432C-92E4-CD7794105F4C")]
        [AssociationId("591D6293-27DE-41ED-BC77-B07288444C2D")]
        [RoleId("559B7C56-D5A7-47E0-89DA-F4A4A3144D2B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PaymentMethod PaymentMethod { get; set; }

        #region Allors
        [Id("196B4892-99A5-426A-A517-4C91A576D4E8")]
        [AssociationId("BBDA3400-A9E2-4533-8B4E-23FE015E18C0")]
        [RoleId("1C20659F-6AA2-48B2-93BD-4241A1171732")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public SalesOrder SalesOrder { get; set; }

        #region Allors
        [Id("F3D9B3C4-115F-4A3D-98CA-6B6CAD5A7C77")]
        [AssociationId("19DC22BC-0001-417A-B9D8-2A15EFFF68D2")]
        [RoleId("4A685604-5036-4703-86D6-7001914D7C86")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal InitialMarkupPercentage { get; set; }

        #region Allors
        [Id("3FBB5A5F-F73F-4B83-9054-50DF08BE08F9")]
        [AssociationId("8082B62F-E4FE-49BD-A146-0F1C8B4F4FCC")]
        [RoleId("87621CCF-823D-4CBF-A670-191DFCD5CDF8")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaintainedMarkupPercentage { get; set; }

        #region Allors
        [Id("F72F61C4-91CF-488F-8C3B-77A3D394E124")]
        [AssociationId("175E92F2-6B98-4811-9346-08D81A78B7F2")]
        [RoleId("573F3F93-4017-4BDA-ABD9-6676021E30FE")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        public Person[] SalesReps { get; set; }

        #region Allors
        [Id("29EEB96F-C89E-4C86-A1C4-44DED257BE60")]
        [AssociationId("43166A2F-D204-44E9-BF62-9E6A64274827")]
        [RoleId("FD7395D9-4589-49A8-91A3-075BDB323497")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public Shipment Shipment { get; set; }

        #region Allors
        [Id("522C18FE-D45B-437D-8A41-D5DF4615C010")]
        [AssociationId("4EC1D58A-8EC3-4C87-BA1C-26C3B133FB9F")]
        [RoleId("8B06C770-0C4B-4BF9-BBC6-F4AF007DAB50")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaintainedProfitMargin { get; set; }

        #region Allors
        [Id("C98647B8-E3DC-4EF9-9109-E7239D6AF534")]
        [AssociationId("DF2B1D5F-E901-42E0-9CF0-723BD3AA20A3")]
        [RoleId("2F5AA3D7-2324-4321-A14B-4262BD372FB1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Party PreviousShipToCustomer { get; set; }

        #region Allors
        [Id("92C03EDC-A5B4-4AFD-BCAE-533FC0D77DA4")]
        [AssociationId("7AB0251A-2182-4F8A-A2CA-97D3AC0B5707")]
        [RoleId("D9FB8901-47D2-42B3-9F55-3B8F7976758E")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party BillToCustomer { get; set; }

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
        [Id("DB04DFF4-CA6D-4F6A-96A4-C4D98EAB9A4F")]
        [AssociationId("89D1DE27-63B8-453A-843F-DC4CB27E4930")]
        [RoleId("7C337FE8-24B2-4EDD-9039-F667D5B3BBAC")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalListPriceCustomerCurrency { get; set; }

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
        [Id("55772E8F-4911-4746-83F8-654F10859D6B")]
        [AssociationId("220143D4-E491-4759-ABA1-387A36976A69")]
        [RoleId("32518558-717F-458C-A371-093CC7B83887")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism BilledFromContactMechanism { get; set; }

        #region Allors
        [Id("EAA04987-9B9D-4C8D-85D1-7A5CF4B1EC49")]
        [AssociationId("98FE57FA-AD71-4172-8294-F82660FD436A")]
        [RoleId("A25EC8D1-F7D1-4490-9210-3950FBE62E61")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal TotalPurchasePrice { get; set; }

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
        [Derived]
        [Indexed]
        [Workspace]
        public Party[] Customers { get; set; }

        #region Allors
        [Id("417B7D53-AA3B-4881-A491-1FC539D2C013")]
        [AssociationId("078DC8A6-B8C9-4509-970C-CE5EEE1A9D4E")]
        [RoleId("74BECE1A-A07E-4CBC-A704-488F7A696FC6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("B87EFDD6-DE6B-4432-AD22-5E3FBB0218FE")]
        [AssociationId("AA11DEBF-01DA-4CA0-8636-3B2EF16C486E")]
        [RoleId("852756F7-E901-4278-B461-60C72ACED7E9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Store Store { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
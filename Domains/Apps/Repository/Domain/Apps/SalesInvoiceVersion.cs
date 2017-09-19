namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("")]
    #endregion
    public partial class SalesInvoiceVersion : InvoiceVersion
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
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
        public DateTime TimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        SalesInvoiceObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalListPrice { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        InternalOrganisation BilledFromInternalOrganisation { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        Party PreviousBillToCustomer { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        SalesInvoiceType SalesInvoiceType { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal InitialProfitMargin { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        PaymentMethod PaymentMethod { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        SalesOrder SalesOrder { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal InitialMarkupPercentage { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal MaintainedMarkupPercentage { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Person[] SalesReps { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        Shipment Shipment { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal MaintainedProfitMargin { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        Party PreviousShipToCustomer { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        Party BillToCustomer { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        SalesInvoiceItem[] SalesInvoiceItems { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalListPriceCustomerCurrency { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party ShipToCustomer { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        ContactMechanism BilledFromContactMechanism { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalPurchasePrice { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        SalesChannel SalesChannel { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Derived]
        [Indexed]
        [Workspace]
        Party[] Customers { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        Store Store { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("6E73D211-6ADE-4DE8-B116-8D3B1BC060D2")]
    #endregion
    public partial class SalesInvoiceVersion : ISalesInvoice
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
        public SalesInvoiceObjectState CurrentObjectState { get; set; }
        public decimal TotalListPrice { get; set; }
        public InternalOrganisation BilledFromInternalOrganisation { get; set; }
        public ContactMechanism BillToContactMechanism { get; set; }
        public Party PreviousBillToCustomer { get; set; }
        public SalesInvoiceType SalesInvoiceType { get; set; }
        public decimal InitialProfitMargin { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public decimal InitialMarkupPercentage { get; set; }
        public decimal MaintainedMarkupPercentage { get; set; }
        public Person[] SalesReps { get; set; }
        public Shipment Shipment { get; set; }
        public decimal MaintainedProfitMargin { get; set; }
        public Party PreviousShipToCustomer { get; set; }
        public Party BillToCustomer { get; set; }
        public SalesInvoiceItem[] SalesInvoiceItems { get; set; }
        public decimal TotalListPriceCustomerCurrency { get; set; }
        public Party ShipToCustomer { get; set; }
        public ContactMechanism BilledFromContactMechanism { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public SalesChannel SalesChannel { get; set; }
        public Party[] Customers { get; set; }
        public PostalAddress ShipToAddress { get; set; }
        public Store Store { get; set; }
        #endregion

        #region Allors
        [Id("D95CEC00-6877-4B0D-AE44-F1A658B21A54")]
        [AssociationId("A2B68986-AB9B-496B-AD5D-C1B25AEAB27D")]
        [RoleId("86DECBC0-0CE7-4629-B38E-B745CA570E6F")]
        #endregion
        [Workspace]
        public DateTime TimeStamp { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
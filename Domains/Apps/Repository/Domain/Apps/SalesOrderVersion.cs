namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("20B9BA2A-5ACD-4466-8CE8-3FC1F69C50FD")]
    #endregion
    public partial class SalesOrderVersion : ISalesOrder
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public string InternalComment { get; set; }
        public Currency CustomerCurrency { get; set; }
        public decimal TotalBasePriceCustomerCurrency { get; set; }
        public decimal TotalIncVatCustomerCurrency { get; set; }
        public decimal TotalDiscountCustomerCurrency { get; set; }
        public string CustomerReference { get; set; }
        public Fee Fee { get; set; }
        public decimal TotalExVat { get; set; }
        public OrderTerm[] OrderTerms { get; set; }
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
        public ContactMechanism TakenByContactMechanism { get; set; }
        public Party ShipToCustomer { get; set; }
        public Party BillToCustomer { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public ShipmentMethod ShipmentMethod { get; set; }
        public decimal TotalListPriceCustomerCurrency { get; set; }
        public decimal MaintainedProfitMargin { get; set; }
        public PostalAddress ShipToAddress { get; set; }
        public Party PreviousShipToCustomer { get; set; }
        public ContactMechanism BillToContactMechanism { get; set; }
        public Person[] SalesReps { get; set; }
        public decimal InitialProfitMargin { get; set; }
        public decimal TotalListPrice { get; set; }
        public bool PartiallyShip { get; set; }
        public Party[] Customers { get; set; }
        public Store Store { get; set; }
        public decimal MaintainedMarkupPercentage { get; set; }
        public ContactMechanism BillFromContactMechanism { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ContactMechanism PlacingContactMechanism { get; set; }
        public Party PreviousBillToCustomer { get; set; }
        public SalesChannel SalesChannel { get; set; }
        public Party PlacingCustomer { get; set; }
        public SalesInvoice ProformaInvoice { get; set; }
        public SalesOrderItem[] SalesOrderItems { get; set; }
        public SalesOrderObjectState CurrentObjectState { get; set; }
        public decimal InitialMarkupPercentage { get; set; }
        public InternalOrganisation TakenByInternalOrganisation { get; set; }
        public ProductQuote Quote { get; set; }
        #endregion

        #region Allors
        [Id("155FB4ED-6456-4D40-84BA-B61369965423")]
        [AssociationId("DAF49B7B-96E6-4AE7-AD5F-2189994B4F7F")]
        [RoleId("B53C3CC0-9F77-4A79-95C3-2D149630E6AA")]
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
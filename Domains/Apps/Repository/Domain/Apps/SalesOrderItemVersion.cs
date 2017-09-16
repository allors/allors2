namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("75A7D08C-0B1C-4CEB-B5C7-7F94EEA38431")]
    #endregion
    public partial class SalesOrderItemVersion : ISalesOrderItem
    {
        #region inherited properties

        public string InternalComment { get; set; }
        public BudgetItem BudgetItem { get; set; }
        public decimal PreviousQuantity { get; set; }
        public decimal QuantityOrdered { get; set; }
        public string Description { get; set; }
        public PurchaseOrder CorrespondingPurchaseOrder { get; set; }
        public decimal TotalOrderAdjustmentCustomerCurrency { get; set; }
        public decimal TotalOrderAdjustment { get; set; }
        public QuoteItem QuoteItem { get; set; }
        public DateTime AssignedDeliveryDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public OrderTerm[] OrderTerms { get; set; }
        public string ShippingInstruction { get; set; }
        public OrderItem[] Associations { get; set; }
        public string Message { get; set; }
        public decimal InitialProfitMargin { get; set; }
        public decimal QuantityShortFalled { get; set; }
        public OrderItem[] OrderedWithFeatures { get; set; }
        public decimal MaintainedProfitMargin { get; set; }
        public decimal RequiredProfitMargin { get; set; }
        public NonSerialisedInventoryItem PreviousReservedFromInventoryItem { get; set; }
        public decimal QuantityShipNow { get; set; }
        public decimal RequiredMarkupPercentage { get; set; }
        public decimal QuantityShipped { get; set; }
        public PostalAddress ShipToAddress { get; set; }
        public decimal QuantityPicked { get; set; }
        public Product PreviousProduct { get; set; }
        public SalesOrderItemObjectState CurrentObjectState { get; set; }
        public decimal UnitPurchasePrice { get; set; }
        public Party ShipToParty { get; set; }
        public PostalAddress AssignedShipToAddress { get; set; }
        public decimal QuantityReturned { get; set; }
        public decimal QuantityReserved { get; set; }
        public Person SalesRep { get; set; }
        public Party AssignedShipToParty { get; set; }
        public decimal QuantityPendingShipment { get; set; }
        public decimal MaintainedMarkupPercentage { get; set; }
        public decimal InitialMarkupPercentage { get; set; }
        public NonSerialisedInventoryItem ReservedFromInventoryItem { get; set; }
        public Product Product { get; set; }
        public ProductFeature ProductFeature { get; set; }
        public decimal QuantityRequestsShipping { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string Comment { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public decimal TotalDiscountAsPercentage { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public decimal UnitVat { get; set; }
        public decimal TotalVatCustomerCurrency { get; set; }
        public VatRegime VatRegime { get; set; }
        public decimal TotalVat { get; set; }
        public decimal UnitSurcharge { get; set; }
        public decimal UnitDiscount { get; set; }
        public decimal TotalExVatCustomerCurrency { get; set; }
        public VatRate DerivedVatRate { get; set; }
        public decimal ActualUnitPrice { get; set; }
        public decimal TotalIncVatCustomerCurrency { get; set; }
        public decimal UnitBasePrice { get; set; }
        public decimal CalculatedUnitPrice { get; set; }
        public decimal TotalSurchargeCustomerCurrency { get; set; }
        public decimal TotalIncVat { get; set; }
        public decimal TotalSurchargeAsPercentage { get; set; }
        public decimal TotalDiscountCustomerCurrency { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalSurcharge { get; set; }
        public VatRegime AssignedVatRegime { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalExVat { get; set; }
        public decimal TotalBasePriceCustomerCurrency { get; set; }
        public PriceComponent[] CurrentPriceComponents { get; set; }
        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        #endregion

        #region Allors
        [Id("2BF19BD8-65C0-4458-A760-79155071DB0E")]
        [AssociationId("F7155754-2275-4F49-94E3-14A0142AB753")]
        [RoleId("FF19D579-0A68-420D-8E9B-3819CCA9390D")]
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
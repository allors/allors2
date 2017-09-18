namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("")]
    #endregion
    public partial class PurchaseOrderItemVersion : OrderItemVersion
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
        PurchaseOrderItemObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal QuantityReceived { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        Product Product { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        Part Part { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
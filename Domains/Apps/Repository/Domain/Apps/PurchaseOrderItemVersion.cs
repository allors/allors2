namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("FB750088-6AB2-4DED-9AC0-4E4E8ABE88A8")]
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

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

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
        [Id("8CAD423E-4A73-4D4B-9261-5462E44D916F")]
        [AssociationId("A1058803-118D-4186-9444-4ACB131B47CA")]
        [RoleId("4B131BD1-C0FF-41B0-8CCF-75A18768E15A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public PurchaseOrderItemState PurchaseOrderItemState { get; set; }

        #region Allors
        [Id("CD44EDAB-1CCE-4F2B-A92E-13BC74339EFE")]
        [AssociationId("ADA763B8-E4DD-4165-857D-56013BDCDFF7")]
        [RoleId("A2326D8F-2E51-4F86-B78D-FFC77FFBC94F")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityReceived { get; set; }

        #region Allors
        [Id("9530B17D-4ABF-45EB-8D45-8E1B4C0E770B")]
        [AssociationId("81217277-FCC2-4388-85D7-6E19C80ABB43")]
        [RoleId("F551BBF3-700C-4A62-9FFE-29A2F9E2A8A5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Product Product { get; set; }

        #region Allors
        [Id("E366DDED-1EB3-4864-B000-B3FA5BC40023")]
        [AssociationId("DEF6077A-2EC0-4085-AF52-EEBC735A6326")]
        [RoleId("025A27DA-069B-457D-8A8F-0224E45E241F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Part Part { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
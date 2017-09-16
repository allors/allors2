namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("D48A4723-638B-4682-98F5-93FA876CAEE0")]
    #endregion
    public partial class PurchaseOrderVersion : IPurchaseOrder
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
        public PurchaseOrderItem[] PurchaseOrderItems { get; set; }
        public Party PreviousTakenViaSupplier { get; set; }
        public Party TakenViaSupplier { get; set; }
        public PurchaseOrderObjectState CurrentObjectState { get; set; }
        public ContactMechanism TakenViaContactMechanism { get; set; }
        public ContactMechanism BillToContactMechanism { get; set; }
        public InternalOrganisation ShipToBuyer { get; set; }
        public Facility Facility { get; set; }
        public PostalAddress ShipToAddress { get; set; }
        public InternalOrganisation BillToPurchaser { get; set; }
        #endregion

        #region Allors
        [Id("4690DD5A-868C-4E68-84B1-0F65854BB39B")]
        [AssociationId("6EF16BF2-9313-40ED-939C-E66097EA97F5")]
        [RoleId("7BC43DEF-38C1-4D9B-A0E0-25DAB6BC02A3")]
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
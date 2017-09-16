namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("45958296-1517-45F1-957D-618C9CFB50D9")]
    #endregion
    public partial class SalesInvoiceItemVersion : ISalesInvoiceItem
    {
        #region inherited properties

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
        public string InternalComment { get; set; }
        public AgreementTerm[] InvoiceTerms { get; set; }
        public decimal TotalInvoiceAdjustment { get; set; }
        public InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }
        public IInvoiceItem AdjustmentFor { get; set; }
        public SerialisedInventoryItem SerializedInventoryItem { get; set; }
        public string Message { get; set; }
        public decimal TotalInvoiceAdjustmentCustomerCurrency { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public ProductFeature ProductFeature { get; set; }
        public SalesInvoiceItemObjectState CurrentObjectState { get; set; }
        public decimal RequiredProfitMargin { get; set; }
        public decimal InitialMarkupPercentage { get; set; }
        public decimal MaintainedMarkupPercentage { get; set; }
        public Product Product { get; set; }
        public decimal UnitPurchasePrice { get; set; }
        public SalesOrderItem SalesOrderItem { get; set; }
        public SalesInvoiceItemType SalesInvoiceItemType { get; set; }
        public Person SalesRep { get; set; }
        public decimal InitialProfitMargin { get; set; }
        public decimal MaintainedProfitMargin { get; set; }
        public TimeEntry[] TimeEntries { get; set; }
        public decimal RequiredMarkupPercentage { get; set; }
        #endregion

        #region Allors
        [Id("4BD0B49F-4EA9-40DD-9361-EE57F28BEB1C")]
        [AssociationId("7711AB6C-427B-4314-8D11-35909B833BF0")]
        [RoleId("35E1F223-4A26-4D2B-A799-8F206AA46A0F")]
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
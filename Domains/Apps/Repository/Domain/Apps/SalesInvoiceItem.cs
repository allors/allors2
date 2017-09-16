namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a98f8aca-d711-47e8-ac9c-25b607cbaef1")]
    #endregion
    public partial class SalesInvoiceItem : InvoiceItem, ISalesInvoiceItem 
    {
        #region inherited properties
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
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public string Comment { get; set; }
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
        [Id("61008480-5266-42B1-BD09-477C514F5FC5")]
        [AssociationId("EC5E9E80-DFED-4EFB-9923-CC066FA6975A")]
        [RoleId("8FCEDD24-9BF7-4CC9-8387-C661BEE650C5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesInvoiceItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("BDDB0313-E741-4AC4-82C3-D0CB98653EA6")]
        [AssociationId("85452B29-63DB-4C48-9FEB-9458752D2A8A")]
        [RoleId("DF134157-AAD4-4101-A050-60E9C1928FD1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesInvoiceItemVersion PreviousVersion { get; set; }

        #region Allors
        [Id("3409316D-FD54-408C-BC1C-9468CCE4B72E")]
        [AssociationId("DC9E9D4F-27AC-4022-ACB4-F4916BF010BF")]
        [RoleId("C646543C-2FF4-4DA5-99D6-EACAE7A28FDA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesInvoiceItemVersion[] AllVersions { get; set; }

        #region Allors
        [Id("E7D84E59-2992-4F25-8C68-866DC933536A")]
        [AssociationId("81554067-8A7A-403A-8127-F73FF0847C8C")]
        [RoleId("FDB32E72-ABEA-470C-B5BD-9646C8154B1B")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesInvoiceItemVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("95F7B58B-D57C-4E22-9947-A1B62F60103C")]
        [AssociationId("4DD00467-7AD2-4C7E-AD19-B10A5D4B5468")]
        [RoleId("B1F4CEAF-2CBF-4466-98A1-06A46B9506E2")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesInvoiceItemVersion[] AllStateVersions { get; set; }

        #region inherited methods

        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        #endregion
    }
}
namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("1ee19062-e36d-4836-b0e6-928a3957bd57")]
    #endregion
    public partial class PurchaseInvoiceItem : InvoiceItem, Versioned
    {
        #region inherited properties

        public string InternalComment { get; set; }
        public AgreementTerm[] InvoiceTerms { get; set; }
        public decimal TotalInvoiceAdjustment { get; set; }
        public InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }
        public InvoiceItem AdjustmentFor { get; set; }
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

        #endregion

        #region Allors
        [Id("56e47122-faaa-4211-806c-1c19695fe434")]
        [AssociationId("826db2b1-3048-4237-8e83-0c472a166d49")]
        [RoleId("893de8bc-93eb-4864-89ba-efdb66b32fd5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public PurchaseInvoiceItemType PurchaseInvoiceItemType { get; set; }

        #region Allors
        [Id("65eebcc4-d5ef-4933-8640-973b67c65127")]
        [AssociationId("40703e06-25f8-425d-aa95-3c73fafbfa81")]
        [RoleId("05f86785-08d8-4282-9734-6230e807181b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public Part Part { get; set; }

        #region Allors
        [Id("dbe5c72f-63e0-47a5-a5f5-f8a3ff83fd57")]
        [AssociationId("f8082d94-30fa-4a58-8bb0-bc5bb4f045ef")]
        [RoleId("69360188-077f-49f0-ba88-abb1f546d72c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public PurchaseInvoiceItemObjectState CurrentObjectState { get; set; }

        #region Versioning
        #region Allors
        [Id("F41279D9-A0F9-44EB-857D-3C76D9CBE634")]
        [AssociationId("46FD1730-7AED-4B5F-8858-279FFE7F30CC")]
        [RoleId("5D1A28E5-0694-417C-B508-7AE40FA60BBC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseInvoiceItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("E17BF428-BA56-451B-90D9-371CDA61E0E6")]
        [AssociationId("EDCC5A53-993F-40B6-81B1-F9070E04D584")]
        [RoleId("0CC8F3F3-A774-41E2-9242-FCEBE88D93B7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseInvoiceItemVersion[] AllVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion
    }
}
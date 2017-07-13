namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("1ee19062-e36d-4836-b0e6-928a3957bd57")]
    #endregion
    public partial class PurchaseInvoiceItem : InvoiceItem 
    {
        #region inherited properties
        public AgreementTerm[] InvoiceTerms { get; set; }

        public decimal TotalInvoiceAdjustment { get; set; }

        public InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }

        public InvoiceItem AdjustmentFor { get; set; }

        public SerializedInventoryItem SerializedInventoryItem { get; set; }

        public string Message { get; set; }

        public decimal TotalInvoiceAdjustmentCustomerCurrency { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Quantity { get; set; }

        public string Description { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

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

        public string Comment { get; set; }

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
        [Id("99b3395c-bb6a-4a4f-b22f-900e76e22520")]
        [AssociationId("7ed44fc9-fc12-4a68-8938-1573ec28da2f")]
        [RoleId("7b17b8a8-fda9-4707-a7a3-e263b51bcd4f")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]

        public PurchaseInvoiceItemStatus CurrentInvoiceItemStatus { get; set; }
        #region Allors
        [Id("c850d9db-682d-4a05-b62e-ab67eb19bd0d")]
        [AssociationId("b79e9e5b-f4a5-4bd0-bc46-ab55eea2f027")]
        [RoleId("136970e5-2ec7-4036-9abc-c84747d59d54")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]

        public PurchaseInvoiceItemStatus[] InvoiceItemStatuses { get; set; }
        #region Allors
        [Id("dbe5c72f-63e0-47a5-a5f5-f8a3ff83fd57")]
        [AssociationId("f8082d94-30fa-4a58-8bb0-bc5bb4f045ef")]
        [RoleId("69360188-077f-49f0-ba88-abb1f546d72c")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]

        public PurchaseInvoiceItemObjectState CurrentObjectState { get; set; }


        #region inherited methods


        public void OnBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        #endregion

    }
}
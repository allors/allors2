namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("ab648bd0-6e31-4ab0-a9ee-cf4a6f02033d")]
    #endregion
    public partial class PurchaseOrderItem : OrderItem, Versioned
    {
        #region inherited properties        
        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

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
        public SalesTerm[] SalesTerms { get; set; }
        public string ShippingInstruction { get; set; }
        public OrderItem[] Associations { get; set; }
        public string Message { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
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

        #region ObjectStates
        #region PurchaseOrderItemState
        #region Allors
        [Id("830C4CBE-1621-4CC3-BC8D-CFC853B2C70A")]
        [AssociationId("F7C036F4-74C4-4F6D-9132-585943660AFA")]
        [RoleId("D99148EB-A519-4087-96A9-0AFF7D2A7BDF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderItemState PreviousPurchaseOrderItemState { get; set; }

        #region Allors
        [Id("D6EE10B5-A4D6-4B7E-BCE3-8A338A7B8CB2")]
        [AssociationId("FC235F49-8279-4737-B279-C42A7BBBD337")]
        [RoleId("466724D6-6FDE-43CF-9D4B-EBC0F5518EAC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderItemState LastPurchaseOrderItemState { get; set; }

        #region Allors
        [Id("C2A16E15-2AED-405D-8C0C-FF14DA14AF69")]
        [AssociationId("622B7DA4-3451-4327-A243-07EE2A60C3B5")]
        [RoleId("A7280BF3-A74B-4794-8B2E-50F07EAC05B4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseOrderItemState PurchaseOrderItemState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("93C91DE0-2083-410F-A373-90C2C4AE999D")]
        [AssociationId("4C796167-3E33-4451-AC9A-AB6A9B986770")]
        [RoleId("626C4447-EBDD-4F59-9CDA-3305588D6409")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("F9961466-5C49-4497-AD2A-26FEED74BE66")]
        [AssociationId("9166D40D-C51F-402F-8BA9-18295F22FCD2")]
        [RoleId("62A364EC-3E11-47C8-A4A6-4F85B44CF7EA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("64e30c56-a77d-4ecf-b21e-e480dd5a25d8")]
        [AssociationId("448695c9-c23b-4ae0-98d7-802a8ae4e9f8")]
        [RoleId("9586b58f-8ae0-4b26-81b6-085a9e28aa77")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityReceived { get; set; }

        #region Allors
        [Id("adfe14e7-fbf6-465f-b6e5-1eb3e8583179")]
        [AssociationId("682538a3-d3e7-432b-9264-38197462cee1")]
        [RoleId("fecc85a0-871b-4846-b8f1-c2a5728fbbd2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Product Product { get; set; }

        #region Allors
        [Id("e2dc0027-220b-4935-bc5a-cb2e2b6be248")]
        [AssociationId("3d24da0d-fdd6-46e3-909b-7710e84e2d68")]
        [RoleId("76ed288c-be72-44e2-8b83-0a0f5a616e52")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Part Part { get; set; }
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Cancel(){}

        public void Reject(){}

        public void Confirm(){}

        public void Approve(){}

        public void Delete(){}
        #endregion

        #region Allors
        [Id("10FCCE86-96CC-440F-903A-2BB909373DC0")]
        #endregion
        public void Complete() { }

    }
}
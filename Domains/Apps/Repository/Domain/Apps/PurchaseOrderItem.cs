namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes;

    #region Allors
    [Id("ab648bd0-6e31-4ab0-a9ee-cf4a6f02033d")]
    #endregion
    public partial class PurchaseOrderItem : OrderItem 
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

        #endregion

        #region Allors
        [Id("0d6cc324-fa0e-4a8c-8afd-802a6301a6c7")]
        [AssociationId("68ad7777-1d14-4635-8f36-1c1e68bd1989")]
        [RoleId("ddbd34f4-264a-4465-b57c-a3f9c76e6a52")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        public PurchaseOrderItemStatus[] OrderItemStatuses { get; set; }
        
        #region Allors
        [Id("43035995-bea3-488b-9e81-e85e929faa57")]
        [AssociationId("f9d773a8-772b-4981-a360-944f14a5ef94")]
        [RoleId("f7034bc1-6cc0-4e03-ab3c-da64d427df9b")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        public PurchaseOrderItemObjectState CurrentObjectState { get; set; }
        
        #region Allors
        [Id("47af92f0-f773-40e2-b0ed-4b3677eddbb7")]
        [AssociationId("6eb5977f-2a79-49e1-ac87-16a53de7e40b")]
        [RoleId("e2ee216b-ae28-4ddf-b354-aa7a75f4cc4e")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        public PurchaseOrderItemStatus[] ShipmentStatuses { get; set; }
        
        #region Allors
        [Id("5e2f5c1a-99e7-4906-8cdd-e78ac4f4bce0")]
        [AssociationId("de791292-84df-4297-959f-d3bc61a2e137")]
        [RoleId("5f9865d9-b7b2-42e3-b13d-013b8945e843")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        public PurchaseOrderItemStatus[] PaymentStatuses { get; set; }
        
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
        [Id("6c187e2c-d7ab-4d3d-b8d9-732af7310e7e")]
        [AssociationId("50d321f7-fa51-4d08-a12d-e7b8702d2c33")]
        [RoleId("0aab6049-05b6-494a-ac11-df251374f8f4")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public PurchaseOrderItemStatus CurrentShipmentStatus { get; set; }
        
        #region Allors
        [Id("adfe14e7-fbf6-465f-b6e5-1eb3e8583179")]
        [AssociationId("682538a3-d3e7-432b-9264-38197462cee1")]
        [RoleId("fecc85a0-871b-4846-b8f1-c2a5728fbbd2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Product Product { get; set; }
        
        #region Allors
        [Id("bbe10173-c24c-4514-86ec-96bd0741efa6")]
        [AssociationId("d12015c4-7462-4dec-95b6-2c233cbb8607")]
        [RoleId("75c95f93-74c1-47b9-9bcc-457edc48a4b3")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public PurchaseOrderItemStatus CurrentOrderItemStatus { get; set; }
        
        #region Allors
        [Id("cca92fe0-8711-46fd-b08d-bf313cc585a6")]
        [AssociationId("db50db5b-59d8-46b9-9c59-d1b9a93fec11")]
        [RoleId("425b29d1-4001-46e0-821c-6da18051d3ee")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public PurchaseOrderItemStatus CurrentPaymentStatus { get; set; }

        #region Allors
        [Id("e2dc0027-220b-4935-bc5a-cb2e2b6be248")]
        [AssociationId("3d24da0d-fdd6-46e3-909b-7710e84e2d68")]
        [RoleId("76ed288c-be72-44e2-8b83-0a0f5a616e52")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Part Part { get; set; }

        #region Allors
        [Id("10FCCE86-96CC-440F-903A-2BB909373DC0")]
        #endregion
        public void Complete() { }
        
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

        public void Finish(){}

        public void Delete(){}




        #endregion
    }
}
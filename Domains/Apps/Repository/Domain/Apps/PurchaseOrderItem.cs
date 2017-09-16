namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("ab648bd0-6e31-4ab0-a9ee-cf4a6f02033d")]
    #endregion
    public partial class PurchaseOrderItem : OrderItem, IPurchaseOrderItem 
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
        public PurchaseOrderItemObjectState CurrentObjectState { get; set; }
        public decimal QuantityReceived { get; set; }
        public Product Product { get; set; }
        public Part Part { get; set; }

        #endregion
        
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
        [Id("B318BBD0-3624-4A0A-9B73-204C52BCAAF5")]
        [AssociationId("7EBC774F-D297-43F6-8C7D-29CB6BA8AF4C")]
        [RoleId("3AEC1268-9144-41F0-A756-DAF3DD9E254E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderItemVersion PreviousVersion { get; set; }

        #region Allors
        [Id("F9961466-5C49-4497-AD2A-26FEED74BE66")]
        [AssociationId("9166D40D-C51F-402F-8BA9-18295F22FCD2")]
        [RoleId("62A364EC-3E11-47C8-A4A6-4F85B44CF7EA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderItemVersion[] AllVersions { get; set; }

        #region Allors
        [Id("4930CE0C-FCD0-438A-8047-AE81B288CC57")]
        [AssociationId("60122E0C-6AAB-4E86-A1DF-CD4088A623B6")]
        [RoleId("CD0961FE-59F3-4DB3-A1C3-90F3170337C3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderItemVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("33D1217C-5DB1-4056-81D7-3DE4A59FB356")]
        [AssociationId("411E999C-0326-4882-A78D-9AF7DEBAE066")]
        [RoleId("ADF5CB03-9D4F-4803-B26E-6BD9F5BF05E5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderItemVersion[] AllStateVersions { get; set; }

        #region Allors
        [Id("34445903-6770-4CE1-8732-6EA941481D7C")]
        [AssociationId("D3C258C1-64C7-4E11-9C71-CAA222636C32")]
        [RoleId("D15D72E2-6341-4ECA-8E8B-2AD8E11571BF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderItemVersion CurrentPaymentStateVersion { get; set; }

        #region Allors
        [Id("A9EB50BE-F754-4DB8-B192-090001D3900C")]
        [AssociationId("DF826EAE-7CE4-45C4-B258-A332797051F6")]
        [RoleId("AFF99A32-1383-41AB-99E1-5923CEC18855")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderItemVersion[] AllPaymentStateVersions { get; set; }

        #region Allors
        [Id("29285DB6-FD40-41CD-A5BF-413D2248396F")]
        [AssociationId("51C23A8C-10B3-4065-996A-EE9645F5D7C2")]
        [RoleId("B7FEC778-06BB-4866-A3D1-A19317C8AB5E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderItemVersion CurrentShipmentStateVersion { get; set; }

        #region Allors
        [Id("4E77A38F-B790-4AD1-BB61-9CE92C611887")]
        [AssociationId("4D03DD01-D238-4385-A122-CC818D9DF884")]
        [RoleId("325543AF-937B-4A53-9587-2DD36C9A21FB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderItemVersion[] AllShipmentStateVersions { get; set; }

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

        #region Allors
        [Id("10FCCE86-96CC-440F-903A-2BB909373DC0")]
        #endregion
        public void Complete() { }
    }
}
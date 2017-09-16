namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("80de925c-04cc-412c-83a5-60405b0e63e6")]
    #endregion
    public partial class SalesOrderItem : OrderItem, ISalesOrderItem 
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
        public decimal InitialProfitMargin { get; set; }
        public decimal QuantityShortFalled { get; set; }
        public OrderItem[] OrderedWithFeatures { get; set; }
        public decimal MaintainedProfitMargin { get; set; }
        public decimal RequiredProfitMargin { get; set; }
        public NonSerialisedInventoryItem PreviousReservedFromInventoryItem { get; set; }
        public decimal QuantityShipNow { get; set; }
        public decimal RequiredMarkupPercentage { get; set; }
        public decimal QuantityShipped { get; set; }
        public PostalAddress ShipToAddress { get; set; }
        public decimal QuantityPicked { get; set; }
        public Product PreviousProduct { get; set; }
        public SalesOrderItemObjectState CurrentObjectState { get; set; }
        public decimal UnitPurchasePrice { get; set; }
        public Party ShipToParty { get; set; }
        public PostalAddress AssignedShipToAddress { get; set; }
        public decimal QuantityReturned { get; set; }
        public decimal QuantityReserved { get; set; }
        public Person SalesRep { get; set; }
        public Party AssignedShipToParty { get; set; }
        public decimal QuantityPendingShipment { get; set; }
        public decimal MaintainedMarkupPercentage { get; set; }
        public decimal InitialMarkupPercentage { get; set; }
        public NonSerialisedInventoryItem ReservedFromInventoryItem { get; set; }
        public Product Product { get; set; }
        public ProductFeature ProductFeature { get; set; }
        public decimal QuantityRequestsShipping { get; set; }

        #endregion

        #region Allors
        [Id("6672E7FD-B5B7-41D6-8AFF-799045EBFC26")]
        [AssociationId("F70356B6-2437-4579-BA36-C92C12F77FDA")]
        [RoleId("EF171C82-7A4B-4F58-801F-78C778B35F92")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("B9883D8D-A848-4995-AD3E-AE2B3F3E3CF6")]
        [AssociationId("294EAB92-DBDA-46C2-890D-8002D64C7F8D")]
        [RoleId("9CA34AFF-F3EE-4FD1-9300-A64FE3A5F0A9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderItemVersion PreviousVersion { get; set; }

        #region Allors
        [Id("97271467-852D-44E9-8D6E-6C5CC8EAABF0")]
        [AssociationId("4C380563-E4F0-4B9B-A7EC-5C332E6E9D43")]
        [RoleId("80FE4F88-FFC0-4795-BCBF-F7E5A9ED0258")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderItemVersion[] AllVersions { get; set; }

        #region Allors
        [Id("3F235F9A-9ED6-4443-A5BF-A95130611450")]
        [AssociationId("35A46E38-9091-443F-B1F1-03DFDFA06B09")]
        [RoleId("611A90F4-54E7-4CD1-B0BD-B6D6F0C33A8F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderItemVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("63301F4B-44B8-49F9-8B86-7908D0E50D02")]
        [AssociationId("1C7C04A6-B0E4-40AF-A0D9-B4E39302CB35")]
        [RoleId("6D5F4301-17B0-45A9-A7D1-340799F49A93")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderItemVersion[] AllStateVersions { get; set; }

        #region Allors
        [Id("C005EA81-2892-4438-9815-942DFDDA54FA")]
        [AssociationId("CD053C72-DD3C-48CC-A085-A87C8CBC0F50")]
        [RoleId("4E28831D-797F-4E0A-8B6A-606D6CFA0664")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderItemVersion CurrentPaymentStateVersion { get; set; }

        #region Allors
        [Id("14660AC3-7B4A-4B5D-A1C1-85C16B784BB9")]
        [AssociationId("A1D03C6F-AABA-4CA8-9D50-9F77A53B045D")]
        [RoleId("6AB58716-EBFA-4BD1-8DF8-A433896DD372")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderItemVersion[] AllPaymentStateVersions { get; set; }

        #region Allors
        [Id("BCC4F9AC-4980-4423-B6B5-734BEC98D139")]
        [AssociationId("2AD48A29-66DF-4A42-ACEF-6521E5C728B4")]
        [RoleId("E7405EA5-29D2-422B-9995-65D26C563E18")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesOrderItemVersion CurrentShipmentStateVersion { get; set; }

        #region Allors
        [Id("4E6955A1-E89E-4913-B273-99F8E64A679C")]
        [AssociationId("D72BA3F3-25F3-4760-B3AF-9703152668FE")]
        [RoleId("C56400D2-C618-4CCA-8197-947D3EFAAEF5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderItemVersion[] AllShipmentStateVersions { get; set; }

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
        [Id("323F3F47-9577-47C6-A77F-DC11CBAEA91C")]
        #endregion
        [Workspace]
        public void Continue() { }

    }
}
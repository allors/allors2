namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("")]
    #endregion
    public partial class SalesOrderItemVersion : OrderItemVersion
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
        public DateTime TimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal InitialProfitMargin { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal QuantityShortFalled { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        OrderItem[] OrderedWithFeatures { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal MaintainedProfitMargin { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal RequiredProfitMargin { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        NonSerialisedInventoryItem PreviousReservedFromInventoryItem { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal QuantityShipNow { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        decimal RequiredMarkupPercentage { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal QuantityShipped { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal QuantityPicked { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        Product PreviousProduct { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        SalesOrderItemObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal UnitPurchasePrice { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        Party ShipToParty { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        PostalAddress AssignedShipToAddress { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal QuantityReturned { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal QuantityReserved { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Derived]
        [Indexed]
        Person SalesRep { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Party AssignedShipToParty { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal QuantityPendingShipment { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal MaintainedMarkupPercentage { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        decimal InitialMarkupPercentage { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        NonSerialisedInventoryItem ReservedFromInventoryItem { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        Product Product { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal QuantityRequestsShipping { get; set; }
        
        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
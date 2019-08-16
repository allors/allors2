namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("FB750088-6AB2-4DED-9AC0-4E4E8ABE88A8")]
    #endregion
    public partial class PurchaseOrderItemVersion : OrderItemVersion
    {
        #region inherited properties

        public string InternalComment { get; set; }
        public BudgetItem BudgetItem { get; set; }
        public decimal PreviousQuantity { get; set; }
        public decimal QuantityOrdered { get; set; }
        public string Description { get; set; }
        public PurchaseOrder CorrespondingPurchaseOrder { get; set; }

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

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

        public User LastModifiedBy { get; set; }

        public decimal TotalDiscountAsPercentage { get; set; }

        public DiscountAdjustment DiscountAdjustment { get; set; }

        public decimal UnitVat { get; set; }



        public VatRegime VatRegime { get; set; }

        public decimal TotalVat { get; set; }

        public decimal UnitSurcharge { get; set; }

        public decimal UnitDiscount { get; set; }



        public VatRate VatRate { get; set; }

        public decimal AssignedUnitPrice { get; set; }



        public decimal UnitBasePrice { get; set; }

        public decimal UnitPrice { get; set; }



        public decimal TotalIncVat { get; set; }

        public decimal TotalSurchargeAsPercentage { get; set; }



        public decimal TotalDiscount { get; set; }

        public decimal TotalSurcharge { get; set; }

        public VatRegime AssignedVatRegime { get; set; }

        public decimal TotalBasePrice { get; set; }

        public decimal TotalExVat { get; set; }



        public SurchargeAdjustment SurchargeAdjustment { get; set; }


        #endregion

        #region Allors
        [Id("8CAD423E-4A73-4D4B-9261-5462E44D916F")]
        [AssociationId("A1058803-118D-4186-9444-4ACB131B47CA")]
        [RoleId("4B131BD1-C0FF-41B0-8CCF-75A18768E15A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public PurchaseOrderItemState PurchaseOrderItemState { get; set; }

        #region Allors
        [Id("D0FE7B1C-6736-4968-A443-67DB8ECC79F9")]
        [AssociationId("66721A9E-F745-4CFB-B607-0FC94ED7CCB4")]
        [RoleId("BAC3243A-63A0-4046-9190-4B48079BA2C6")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseOrderItemShipmentState PurchaseOrderItemShipmentState { get; set; }

        #region Allors
        [Id("B2F7A10C-0000-48E2-8C04-2A5740C1DCDF")]
        [AssociationId("E8CAEF6F-3F3C-4A7F-88D6-8973B04003B4")]
        [RoleId("84E247EA-F866-4E9A-A487-8F237E519302")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseOrderItemPaymentState PurchaseOrderItemPaymentState { get; set; }

        #region Allors
        [Id("CD44EDAB-1CCE-4F2B-A92E-13BC74339EFE")]
        [AssociationId("ADA763B8-E4DD-4165-857D-56013BDCDFF7")]
        [RoleId("A2326D8F-2E51-4F86-B78D-FFC77FFBC94F")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityReceived { get; set; }

        #region Allors
        [Id("E366DDED-1EB3-4864-B000-B3FA5BC40023")]
        [AssociationId("DEF6077A-2EC0-4085-AF52-EEBC735A6326")]
        [RoleId("025A27DA-069B-457D-8A8F-0224E45E241F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Part Part { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {

        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion
    }
}
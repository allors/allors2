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

        public LocalisedText[] LocalisedComments { get; set; }

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

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

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

        #region PurchaseOrderItemShipmentState
        #region Allors
        [Id("4E1A2881-B08E-4BC0-9B20-F08869DC4D45")]
        [AssociationId("77662E61-3A3D-4E40-A72D-58142AAA21C3")]
        [RoleId("2CEAE9A0-26F2-420E-B134-0ABEB150FB8F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderItemShipmentState PreviousPurchaseOrderItemShipmentState { get; set; }

        #region Allors
        [Id("3E1D4FC9-0364-45CF-AFB3-6583A27673FA")]
        [AssociationId("CAADD703-51CE-41C5-B1AE-68BA50810B6C")]
        [RoleId("87A295BC-C6C2-44F4-8E26-98A32CBD4577")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderItemShipmentState LastPurchaseOrderItemShipmentState { get; set; }

        #region Allors
        [Id("5F0A45CF-7EDD-4DFC-AA79-DC40E3470F7F")]
        [AssociationId("8E23AC73-F24C-4B3F-95B1-2944CC892928")]
        [RoleId("11EE08AC-0172-4CF3-8182-CE27CD510BBA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public PurchaseOrderItemShipmentState PurchaseOrderItemShipmentState { get; set; }
        #endregion

        #region PurchaseOrderItemPaymentState
        #region Allors
        [Id("E0331172-5CCE-44ED-9FFF-6CA1AD03EA0E")]
        [AssociationId("DC173772-DC85-41FE-9F50-A206EB5D16C7")]
        [RoleId("823BC7BF-67B4-42A5-B675-D727CACDF477")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderItemPaymentState PreviousPurchaseOrderItemPaymentState { get; set; }

        #region Allors
        [Id("FB765D9B-1A7A-4723-AB6A-FC99D38D302B")]
        [AssociationId("2767529C-F4F4-4A2D-B8CF-2909CBBDA9D8")]
        [RoleId("76658A90-702D-43B4-BAFE-0F15E1E2CB97")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderItemPaymentState LastPurchaseOrderItemPaymentState { get; set; }

        #region Allors
        [Id("37881CFB-C845-400A-A634-3811F190F401")]
        [AssociationId("6B9DD01F-0751-4EEF-BD33-6D89547B6A66")]
        [RoleId("B1DAEEB8-B586-4903-A523-AD0B4D8A85DF")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Workspace]
        public PurchaseOrderItemPaymentState PurchaseOrderItemPaymentState { get; set; }
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
        [Workspace]
        public decimal QuantityReceived { get; set; }

        #region Allors
        [Id("e2dc0027-220b-4935-bc5a-cb2e2b6be248")]
        [AssociationId("3d24da0d-fdd6-46e3-909b-7710e84e2d68")]
        [RoleId("76ed288c-be72-44e2-8b83-0a0f5a616e52")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Part Part { get; set; }

        #region Allors
        [Id("E0FC1C78-EE7A-499E-8D48-BFD846CCA47C")]
        [AssociationId("95D94E82-6AE3-4C41-B283-B278AAFA4E41")]
        [RoleId("C0F176F7-3D0B-44C8-A132-4EE65326568E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SerialisedItem SerialisedItem { get; set; }

        #region Allors
        [Id("5FE65A7F-F7EC-4910-AED7-35C88DED80C7")]
        [AssociationId("AA17A0CF-63A3-4774-93F2-F61ED4C019EC")]
        [RoleId("C0987F1A-C202-440B-B352-7A879F30ADBC")]
        #endregion
        [Size(256)]
        [Workspace]
        public string SerialNumber { get; set; }

        #region Allors
        [Id("56E8D200-1D63-4619-B617-CFA95B9CE07A")]
        [AssociationId("34D75527-43E6-4114-BB1B-5F26D32AA0EF")]
        [RoleId("136B01CA-6C76-4350-B9D1-4B293DC19EC1")]
        #endregion
        [Required]
        [Workspace]
        public bool CanInvoice { get; set; }

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

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
        [Workspace]
        #endregion
        public void QuickReceive() { }
    }
}
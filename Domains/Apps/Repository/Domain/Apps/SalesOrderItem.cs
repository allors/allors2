namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("80de925c-04cc-412c-83a5-60405b0e63e6")]
    #endregion
    public partial class SalesOrderItem : OrderItem, Versioned
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
        #region SalesOrderItemState
        #region Allors
        [Id("552F50AC-5ACD-4F8F-A6CD-68E0C3426F3B")]
        [AssociationId("3A01771D-E007-4D5B-B9D6-8426BF7FA9FA")]
        [RoleId("FF4AAFCC-26A2-4ADB-9FFC-AA96A76F5F55")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemState PreviousSalesOrderItemState { get; set; }

        #region Allors
        [Id("FEBA7CC1-E449-4542-8159-71840DDE093B")]
        [AssociationId("BAF4EDA9-82B0-4591-8546-746CA111AF35")]
        [RoleId("5FD567A2-0220-471B-8726-6457AD93DB6D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemState LastSalesOrderItemState { get; set; }

        #region Allors
        [Id("0A44ABC2-41C2-444C-A20D-D2B5E387A611")]
        [AssociationId("0CD34735-D410-4B5C-A3CE-3F58FCBE59B0")]
        [RoleId("2B4DDB7B-6B88-4781-8D04-8BBD530E1B23")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderItemState SalesOrderItemState { get; set; }
        #endregion

        #region SalesOrderItemPaymentState
        #region Allors
        [Id("C4C0E26C-C324-4391-B660-9DE9545C41DF")]
        [AssociationId("B3570EAB-9F6D-43FC-A172-30F9012D5BD3")]
        [RoleId("5EF86E54-5FD8-44F5-B91C-BE10A03867D3")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemPaymentState PreviousSalesOrderItemPaymentState { get; set; }

        #region Allors
        [Id("5B59E71C-C568-4433-A398-0EAD573FAF92")]
        [AssociationId("4C0185B0-624B-41CB-A072-7847C3F92FD8")]
        [RoleId("159907BE-6D0F-436B-843E-0C5C20D39621")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemPaymentState LastSalesOrderItemPaymentState { get; set; }

        #region Allors
        [Id("4DB38F8C-0903-4E05-9B4F-28EF7C3D9C01")]
        [AssociationId("5E11EFB6-A6C3-4F76-A507-D939D50F59F2")]
        [RoleId("101FF303-80CB-4DF6-BF23-B6FBC7B99BB1")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderItemPaymentState SalesOrderItemPaymentState { get; set; }
        #endregion

        #region SalesOrderItemShipmentState
        #region Allors
        [Id("16139169-9E1C-4B3A-9635-660CA07F3190")]
        [AssociationId("437E24B6-4EA8-46DF-90CE-25B70F27E9FE")]
        [RoleId("DD01B461-220A-4AA1-AE8A-240B568BEF97")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemShipmentState PreviousSalesOrderItemShipmentState { get; set; }

        #region Allors
        [Id("5F81EA61-A2C2-4E88-99C1-FE7DE0EBAB43")]
        [AssociationId("69C7EF09-49FD-40AC-93CF-AB7EB36B3C32")]
        [RoleId("60CB1A99-F414-4B2C-B737-392CF3662FCB")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesOrderItemShipmentState LastSalesOrderItemShipmentState { get; set; }

        #region Allors
        [Id("6F794926-3A3F-4701-ACA6-3D622BADAED6")]
        [AssociationId("12B40C80-F9D5-4134-B8C4-63A9CBFF86D6")]
        [RoleId("38913CF7-1102-43A5-AC6A-E9F523F27A6E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesOrderItemShipmentState SalesOrderItemShipmentState { get; set; }
        #endregion
        #endregion

        #region Versioning
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
        [Id("97271467-852D-44E9-8D6E-6C5CC8EAABF0")]
        [AssociationId("4C380563-E4F0-4B9B-A7EC-5C332E6E9D43")]
        [RoleId("80FE4F88-FFC0-4795-BCBF-F7E5A9ED0258")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesOrderItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("0229942b-e102-4e97-af8d-97ee8383203e")]
        [AssociationId("e1e640d4-4096-42df-9c12-6bf54e5314db")]
        [RoleId("16e9993a-6604-41e9-9ed0-053480d45d46")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal InitialProfitMargin { get; set; }

        #region Allors
        [Id("1e1ed439-ae25-4446-83e6-295d8627a7b5")]
        [AssociationId("67bc37d9-0d6f-4227-81c9-8f03a1e0da47")]
        [RoleId("d8ab230a-92d2-44cb-8e45-502285dd9a5e")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal QuantityShortFalled { get; set; }

        #region Allors
        [Id("1ea02a2c-280a-4a48-9ffb-1517789c56f1")]
        [AssociationId("851f33e4-6c43-468d-ab0d-0f5f83bdb179")]
        [RoleId("213d2b36-dbfd-4e2d-a854-82ba271f0d94")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public OrderItem[] OrderedWithFeatures { get; set; }

        #region Allors
        [Id("1edd9008-537a-43ba-b4a1-56d3c3211c36")]
        [AssociationId("db9987b4-b71c-4ece-b4c1-53bb27a02dff")]
        [RoleId("ae3bd8e0-1f58-45e1-a6dc-191d7668e358")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal MaintainedProfitMargin { get; set; }

        #region Allors
        [Id("2bd8163c-b2cd-49bc-922a-b8c859c24031")]
        [AssociationId("3f66929d-a2f1-4e9b-a701-4364e3a25e1d")]
        [RoleId("1fbf819e-b7fe-4ce3-86af-efea369db2fa")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        public decimal RequiredProfitMargin { get; set; }

        #region Allors
        [Id("3e798309-d5d5-4860-87ec-ba3766e96c9e")]
        [AssociationId("4626b586-07e1-468c-877a-d1a8f1b196c5")]
        [RoleId("b2aef5ac-45f7-41aa-8e1b-f2d79d3d9fad")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public NonSerialisedInventoryItem PreviousReservedFromInventoryItem { get; set; }

        #region Allors
        [Id("40111efb-f609-4726-85c1-a9dd7160df72")]
        [AssociationId("1f0c1c76-4e49-4c31-9b35-cfa5d842039b")]
        [RoleId("4e3983f7-09db-4e06-bf47-a2a4dbcdfa40")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityShipNow { get; set; }

        #region Allors
        [Id("48e40504-bb22-4b11-949d-569b3a556416")]
        [AssociationId("979f6fa2-29f4-43c0-86d7-761509719112")]
        [RoleId("6b6dab9b-1583-4f3d-8d97-1a8a53af9e75")]
        #endregion
        [Workspace]
        [Precision(19)]
        [Scale(2)]
        public decimal RequiredMarkupPercentage { get; set; }

        #region Allors
        [Id("545eb094-63d8-4d25-a069-7c3e91f26eb7")]
        [AssociationId("686d5956-c2dc-46d5-b812-52020d392f0f")]
        [RoleId("3a8adaf6-82e6-45a6-bd5f-61860125d77b")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityShipped { get; set; }

        #region Allors
        [Id("5cc50f26-361b-46d7-a8e6-a9f53f7d2722")]
        [AssociationId("0d8906e9-3bfd-4d9b-8b24-8526fdfb2e33")]
        [RoleId("000b641f-00be-4b9c-84aa-a8c968024ece")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("64edd3e6-0b78-4b34-8a11-aa9c0a1b1f35")]
        [AssociationId("667a1304-05ae-410a-94a1-5e87a40dc53b")]
        [RoleId("d15456ab-66c1-4ecc-bfc5-910b7d9c4869")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityPicked { get; set; }

        #region Allors
        [Id("6826e05e-eb9a-4dc4-a653-0230dec934a9")]
        [AssociationId("aa2b8b0a-672c-423b-9ca8-2fd40f8d1306")]
        [RoleId("793f4946-ed12-49ca-9764-8df534941cca")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Product PreviousProduct { get; set; }

        #region Allors
        [Id("75a13fdc-90b2-4550-9b2f-fc0a9387d569")]
        [AssociationId("94922e2d-1570-4667-af8f-5d4415fd6d78")]
        [RoleId("39d62b69-520c-456d-b3f1-6ca640ffc4cb")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal UnitPurchasePrice { get; set; }

        #region Allors
        [Id("7a8255f5-4283-4803-9f96-60a9adc2743b")]
        [AssociationId("2c9b2182-7b93-46c9-86ac-d13add6d52b5")]
        [RoleId("7596f471-e54c-4491-8af6-02f0e8d7d015")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Workspace]
        public Party ShipToParty { get; set; }

        #region Allors
        [Id("7ae1b939-b387-4e6e-9da2-bc0364e04f7b")]
        [AssociationId("808f88ba-3866-4785-812c-c062c5f268a4")]
        [RoleId("64639736-a7d0-47cb-8afb-fa751a19670d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress AssignedShipToAddress { get; set; }

        #region Allors
        [Id("8145fbd3-a30f-44a0-9520-6b72ac20a82d")]
        [AssociationId("59383e9d-690e-46aa-9cc0-1dd39db14f60")]
        [RoleId("31087f2f-10e8-4558-9e0a-a5dbceb3204a")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityReturned { get; set; }

        #region Allors
        [Id("85d11891-5ffe-488f-9f23-5b2c7bc1c480")]
        [AssociationId("283cdb9a-e7e3-4486-92da-5e94653505a8")]
        [RoleId("fd06dd18-c1d4-40c7-b62e-273a8522f580")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityReserved { get; set; }

        #region Allors
        [Id("911abda0-2eb0-477e-80be-e9e7d358205e")]
        [AssociationId("23af5657-ed05-43c2-aeed-d268204528d2")]
        [RoleId("42a88fb9-84bc-4e35-83ff-6cb5c0cf3c96")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        [Derived]
        [Indexed]
        public Person SalesRep { get; set; }

        #region Allors
        [Id("b2d2645e-0d3f-473e-b277-6f890b9b911e")]
        [AssociationId("68281397-74f8-4356-b9fc-014f792ab914")]
        [RoleId("1292e876-1c61-42cb-8f01-8b3eb6cf0fa0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Party AssignedShipToParty { get; set; }

        #region Allors
        [Id("b2f7dabb-8b87-41bc-8903-166d77bba1c5")]
        [AssociationId("ad7dfb12-d00d-4a93-a011-7cb09c1e9aa9")]
        [RoleId("ba9a9c6c-4df0-4488-b5fa-6181e45c6f18")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityPendingShipment { get; set; }

        #region Allors
        [Id("b8d116ca-dbab-4119-84ca-c0e196d9c018")]
        [AssociationId("3f2cc31e-84e9-4e49-bfbe-0a436e2236be")]
        [RoleId("7cee282f-ff61-42fc-9a2e-54164e8b6390")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal MaintainedMarkupPercentage { get; set; }

        #region Allors
        [Id("d5639e07-37b8-46db-9e35-fa98301d31dd")]
        [AssociationId("43ee44b6-2e51-4ade-8bb7-9b10a780ba2e")]
        [RoleId("38e21291-b24a-4331-b781-f7950df3f501")]
        #endregion
        [Workspace]
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        public decimal InitialMarkupPercentage { get; set; }

        #region Allors
        [Id("d7c25b48-d82f-4250-b09d-1e935eab665b")]
        [AssociationId("67e9a9d9-74ff-4b04-9aa1-dd08c5348a3e")]
        [RoleId("4bfc1720-a2f6-4204-974b-42ca42c0d2e1")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public NonSerialisedInventoryItem ReservedFromInventoryItem { get; set; }

        #region Allors
        [Id("e8980105-2c4d-41de-bd67-802a8c0720f1")]
        [AssociationId("8b747457-bf7a-4274-b245-d04607b2a5ba")]
        [RoleId("90d69cb4-d485-418f-9608-44063f116ff4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("ed586b2f-c687-4d97-9416-52f7156b7b11")]
        [AssociationId("cb5c31c4-2daa-405b-8dc9-5ea6c87f66b3")]
        [RoleId("c5b07ead-1a71-407e-91f8-4ec39853888a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("f148e660-1e09-4e76-97fb-de62a7ee7482")]
        [AssociationId("0105885d-f722-44bd-9f57-6231c38191b5")]
        [RoleId("9132a260-1b35-4b5a-b14c-8dceb6383581")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal QuantityRequestsShipping { get; set; }

        #region Allors
        [Id("9400A92D-DCA3-4F60-BC5C-05D4F32C337D")]
        [AssociationId("02F3103F-D254-40B1-9159-6254F9F8A01C")]
        [RoleId("0164D6CA-F07C-4C6E-9D50-EFE4E7D97464")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace]
        public SalesInvoiceItemType ItemType { get; set; }

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
        [Id("323F3F47-9577-47C6-A77F-DC11CBAEA91C")]
        #endregion
        [Workspace]
        public void Continue() { }
    }
}
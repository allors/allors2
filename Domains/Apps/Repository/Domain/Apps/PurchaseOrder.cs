namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("062bd939-9902-4747-a631-99ea10002156")]
    #endregion
    public partial class PurchaseOrder : Order 
    {
        #region inherited properties

        public string InternalComment { get; set; }
        public Currency CustomerCurrency { get; set; }
        public decimal TotalBasePriceCustomerCurrency { get; set; }
        public decimal TotalIncVatCustomerCurrency { get; set; }
        public decimal TotalDiscountCustomerCurrency { get; set; }
        public string CustomerReference { get; set; }
        public Fee Fee { get; set; }
        public decimal TotalExVat { get; set; }
        public OrderTerm[] OrderTerms { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalSurcharge { get; set; }
        public OrderItem[] ValidOrderItems { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalVatCustomerCurrency { get; set; }
        public decimal TotalDiscount { get; set; }
        public string Message { get; set; }
        public decimal TotalShippingAndHandlingCustomerCurrency { get; set; }
        public DateTime EntryDate { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public OrderKind OrderKind { get; set; }
        public decimal TotalIncVat { get; set; }
        public decimal TotalSurchargeCustomerCurrency { get; set; }
        public VatRegime VatRegime { get; set; }
        public decimal TotalFeeCustomerCurrency { get; set; }
        public decimal TotalShippingAndHandling { get; set; }
        public ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalExVatCustomerCurrency { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalFee { get; set; }
        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public Guid UniqueId { get; set; }
        public string PrintContent { get; set; }
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public string Comment { get; set; }
        public Locale Locale { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Allors
        [Id("15ea478f-b71d-412f-8ee4-abe554b9a7d8")]
        [AssociationId("e48c8211-2539-41ba-9250-27a08799b31b")]
        [RoleId("6ef2d258-4291-4a9f-b7f0-9f154b789775")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        public PurchaseOrderItem[] PurchaseOrderItems { get; set; }

        #region Allors
        [Id("1638a432-3a4f-4cca-906e-660b9164838b")]
        [AssociationId("04f4151a-1adf-426a-9fb1-a0f8cc782b0e")]
        [RoleId("20131db5-50af-42a8-9ac8-fd250c1aa8b6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Party PreviousTakenViaSupplier { get; set; }

        #region Allors
        [Id("36607a9e-d411-4726-a63c-7622b928bfe8")]
        [AssociationId("a8573588-3898-4422-92a2-056448200216")]
        [RoleId("31a6a1a2-92ee-4ffd-9eb8-d69e8f2183fd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public Party TakenViaSupplier { get; set; }

        #region Allors
        [Id("3b1f04bd-c9ad-4fca-981c-2ca243fdc292")]
        [AssociationId("e50e5e9b-f312-4520-81d1-1dbd7f856d0f")]
        [RoleId("64e08e15-4d4d-465b-9d9e-c9e7c971a56d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public PurchaseOrderObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("4830cfc5-0375-4996-8cd8-27e36c102b65")]
        [AssociationId("efa439f8-787e-43d7-bd1b-400cba7e3a62")]
        [RoleId("583bfc51-0bb7-4ea5-914c-33a5c2d64196")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public ContactMechanism TakenViaContactMechanism { get; set; }

        #region Allors
        [Id("7eceb1b6-1395-4655-a558-6d72ad4b380e")]
        [AssociationId("b6e1159c-fcb7-47f1-822b-4ab75e5dac14")]
        [RoleId("ab3ee3c7-dc02-4acf-a34e-6b25783e11fc")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("b390a733-e322-4ada-9ead-75a8c9976337")]
        [AssociationId("6082b0af-f5ed-493c-bb2b-ad4764053819")]
        [RoleId("c725b348-df8f-4a64-adc2-c3d8b3b986b5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public InternalOrganisation ShipToBuyer { get; set; }

        #region Allors
        [Id("ccf88515-6441-4d0f-a2e7-8f5ed7c0533e")]
        [AssociationId("ce230886-53a7-4360-b545-a20d3cf47f1f")]
        [RoleId("2f7e7d1b-6a61-41a6-a05f-375e8a5feeb2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Facility Facility { get; set; }

        #region Allors
        [Id("d74bd1fd-f243-4b5d-8061-1eafe7c25beb")]
        [AssociationId("5465663b-6757-4b1d-9f91-233bfd86bc5d")]
        [RoleId("35c28c9f-852a-4ebb-bc2b-1dce9e3812fa")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("f05e0ba5-4321-4d88-8f2c-8994de5b44b7")]
        [AssociationId("38d76559-6a9c-48c7-bde5-1a2e685b9a40")]
        [RoleId("a0b2ec91-5b7e-4abb-91fb-91836cb88490")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public InternalOrganisation BillToPurchaser { get; set; }
        
        #region Versioning
        #region Allors
        [Id("25006AFF-3E2E-44D5-9F42-38A34868EA87")]
        [AssociationId("5FE211C2-FB70-417F-8067-A96459CD99EC")]
        [RoleId("F08FB76C-9697-4DB3-BA9A-ED79E76CFCE5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderVersion CurrentVersion { get; set; }

        #region Allors
        [Id("868F6D44-31F8-4719-A37E-CE9D9135BE33")]
        [AssociationId("046989EF-DAE7-4469-B084-2317200E3356")]
        [RoleId("1E7A4AC4-8327-4B8F-95D9-FA52CBFCB820")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderVersion PreviousVersion { get; set; }

        #region Allors
        [Id("101C600D-775C-44A1-B065-F464D68CF14A")]
        [AssociationId("EDFD1E31-9DC8-499F-B8DD-63BE0C7AA85D")]
        [RoleId("BBB7F2E6-C39B-4C7E-9556-FFA22A4C5EFC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderVersion[] AllVersions { get; set; }

        #region Allors
        [Id("CE878192-5C69-4FED-88C6-2D1086047D2A")]
        [AssociationId("13FB65ED-7A7C-492F-9DB1-1B8C4178E302")]
        [RoleId("0A54E624-4932-4B9C-B06C-037EF26C17A4")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("D58404C7-3ED0-479B-BBB2-6A3C06D7CF37")]
        [AssociationId("32EAAF5E-3A8F-409D-B224-2D9CA172D3CA")]
        [RoleId("ED8E145F-991F-4607-A251-264CCBAB59C0")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderVersion[] AllStateVersions { get; set; }

        #region Allors
        [Id("D2FF3565-5A56-4D35-BACE-4C6ED391CA78")]
        [AssociationId("87C9E19C-219F-43B8-AC3F-E6A650378BE9")]
        [RoleId("3B64328C-DA0E-43E4-9A3F-F066BA3B637F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderVersion CurrentPaymentStateVersion { get; set; }

        #region Allors
        [Id("BE944CC7-018B-4349-A3D4-6BB885C23CAB")]
        [AssociationId("B5631854-E7E8-4260-AF8C-FF0FAA022E63")]
        [RoleId("F3CD2882-4768-4D40-8EED-36F570F1F32C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderVersion[] AllPaymentStateVersions { get; set; }

        #region Allors
        [Id("CEC13F61-B3DC-46A9-8261-57611CCA0C9C")]
        [AssociationId("7D4FD625-BA3A-4597-A3FC-D18A8F5F0E31")]
        [RoleId("E76535ED-CC40-4F17-9D53-78BEB220688D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseOrderVersion CurrentShipmentStateVersion { get; set; }

        #region Allors
        [Id("6CE31CF8-9CFB-4103-87DF-D4F530D2622A")]
        [AssociationId("D5AFECAB-E928-4153-930F-21B59358A08E")]
        [RoleId("B203660A-AD61-4473-9088-56E0C32CA948")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderVersion[] AllShipmentStateVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        public void Approve(){}

        public void Reject(){}

        public void Hold(){}

        public void Continue(){}

        public void Confirm(){}

        public void Cancel(){}

        public void Complete(){}

        public void Finish(){}
        public void AddNewOrderItem() { }

        #endregion
    }
}
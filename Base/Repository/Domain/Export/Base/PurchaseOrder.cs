namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("062bd939-9902-4747-a631-99ea10002156")]
    #endregion
    public partial class PurchaseOrder : Order, Versioned, WorkItem
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }
        public Currency Currency { get; set; }
        
        
        
        public string CustomerReference { get; set; }
        public Fee Fee { get; set; }
        public decimal TotalExVat { get; set; }
        public SalesTerm[] SalesTerms { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalSurcharge { get; set; }
        public OrderItem[] ValidOrderItems { get; set; }
        public string OrderNumber { get; set; }
        
        public decimal TotalDiscount { get; set; }
        public string Message { get; set; }

        public string Description { get; set; }

        
        public DateTime EntryDate { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public OrderKind OrderKind { get; set; }
        public decimal TotalIncVat { get; set; }
        
        public VatRegime VatRegime { get; set; }
        
        public decimal TotalShippingAndHandling { get; set; }
        public ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }
        public DateTime OrderDate { get; set; }
        
        public DateTime DeliveryDate { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalFee { get; set; }
        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public PrintDocument PrintDocument { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public Locale Locale { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string WorkItemDescription { get; set; }

        #endregion

        #region ObjectStates
        #region PurchaseOrderState
        #region Allors
        [Id("6E96C786-EC92-4A1A-BE23-51D3DA29048F")]
        [AssociationId("B8049D62-0C4D-4549-A4C3-39EF1205045A")]
        [RoleId("FA2D8980-AEAE-413C-B0C7-27A71237AB76")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderState PreviousPurchaseOrderState { get; set; }

        #region Allors
        [Id("249741FA-C0AB-4259-B647-32144DEEEA33")]
        [AssociationId("E520A31E-597B-41E9-B1CA-CFDB5BE7E502")]
        [RoleId("01CC2EFE-2124-416A-9017-D138C4558D3E")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderState LastPurchaseOrderState { get; set; }

        #region Allors
        [Id("C57CDFF9-C921-49D0-9781-A1B4D03F4C85")]
        [AssociationId("78AE70C5-640E-49B3-BCC4-326E40692E88")]
        [RoleId("3EDCECF4-7E41-443A-A95A-1A029B6BC78C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseOrderState PurchaseOrderState { get; set; }
        #endregion

        #region PurchaseOrderPaymentState
        #region Allors
        [Id("7F7CA63A-57D9-47DA-BC75-1FA9AE5FEC96")]
        [AssociationId("8C3192C9-22CC-4713-9383-0F3398D9C5A6")]
        [RoleId("22E7B961-B97A-499E-A8B3-B8A4D6DDB761")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderPaymentState PreviousPurchaseOrderPaymentState { get; set; }

        #region Allors
        [Id("A0C5FC70-EDE7-4571-A6F3-9EC2E9A8731F")]
        [AssociationId("F418F02B-CDA0-434B-9A79-2EA506DB883E")]
        [RoleId("8AE9F57E-D7C9-402F-86AC-FF0F0C997C59")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderPaymentState LastPurchaseOrderPaymentState { get; set; }

        #region Allors
        [Id("65182F74-EF4E-4E35-9E3E-3AC6E42CEFE1")]
        [AssociationId("06A3EAA3-0EA2-4272-8D15-5B6DC46BEFBE")]
        [RoleId("ECA4A68F-7666-465C-B740-8AD5E8436920")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseOrderPaymentState PurchaseOrderPaymentState { get; set; }
        #endregion

        #region PurchaseOrderShipmentState
        #region Allors
        [Id("03BF41DE-6170-424C-9D56-A6999144992F")]
        [AssociationId("5DC7CAC2-DD85-4771-9574-5DA5F835D1FD")]
        [RoleId("A45FE919-3142-4361-9103-E8B34F2A362C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderShipmentState PreviousPurchaseOrderShipmentState { get; set; }

        #region Allors
        [Id("2E65C1AD-7581-4AF0-ADF2-EEDA88422B1B")]
        [AssociationId("FAF78BE5-FF49-4D20-BFA0-F97F597EDA8E")]
        [RoleId("69914FDE-46A5-470A-AB7B-EF1F9BE7236D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseOrderShipmentState LastPurchaseOrderShipmentState { get; set; }

        #region Allors
        [Id("D7DBE9EE-544F-463A-80E9-522AF0E00325")]
        [AssociationId("D22FE58D-6060-44FE-818F-DEEFCFE95EB0")]
        [RoleId("135CCB53-56D5-4C1E-ADAB-0D53A62D44F7")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseOrderShipmentState PurchaseOrderShipmentState { get; set; }
        #endregion

        #endregion

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
        [Id("101C600D-775C-44A1-B065-F464D68CF14A")]
        [AssociationId("EDFD1E31-9DC8-499F-B8DD-63BE0C7AA85D")]
        [RoleId("BBB7F2E6-C39B-4C7E-9556-FFA22A4C5EFC")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseOrderVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("1982BFC9-9B79-4C1A-984D-8784EE02895F")]
        [AssociationId("1F148FD6-EF72-4504-8762-34E779F03EE8")]
        [RoleId("3FEC218D-70FE-4BB5-9E2B-9A751F12671F")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation OrderedBy { get; set; }

        #region Allors
        [Id("15ea478f-b71d-412f-8ee4-abe554b9a7d8")]
        [AssociationId("e48c8211-2539-41ba-9250-27a08799b31b")]
        [RoleId("6ef2d258-4291-4a9f-b7f0-9f154b789775")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public PurchaseOrderItem[] PurchaseOrderItems { get; set; }

        #region Allors
        [Id("1638a432-3a4f-4cca-906e-660b9164838b")]
        [AssociationId("04f4151a-1adf-426a-9fb1-a0f8cc782b0e")]
        [RoleId("20131db5-50af-42a8-9ac8-fd250c1aa8b6")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Organisation PreviousTakenViaSupplier { get; set; }

        #region Allors
        [Id("36607a9e-d411-4726-a63c-7622b928bfe8")]
        [AssociationId("a8573588-3898-4422-92a2-056448200216")]
        [RoleId("31a6a1a2-92ee-4ffd-9eb8-d69e8f2183fd")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Organisation TakenViaSupplier { get; set; }

        #region Allors
        [Id("4830cfc5-0375-4996-8cd8-27e36c102b65")]
        [AssociationId("efa439f8-787e-43d7-bd1b-400cba7e3a62")]
        [RoleId("583bfc51-0bb7-4ea5-914c-33a5c2d64196")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ContactMechanism TakenViaContactMechanism { get; set; }

        #region Allors
        [Id("73A7B96E-5DA1-465D-9754-CDB3184DC20E")]
        [AssociationId("93BFE5E9-7E4A-48DA-8448-926FBD04FF54")]
        [RoleId("6DEF4049-1FCC-4855-A003-DF975392EEFD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person TakenViaContactPerson { get; set; }

        #region Allors
        [Id("7eceb1b6-1395-4655-a558-6d72ad4b380e")]
        [AssociationId("b6e1159c-fcb7-47f1-822b-4ab75e5dac14")]
        [RoleId("ab3ee3c7-dc02-4acf-a34e-6b25783e11fc")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("63AE6DC7-F484-49D8-87D1-6F137232D385")]
        [AssociationId("534C5310-36DE-4616-AAE1-ECF6093609D9")]
        [RoleId("B12DB94A-27CB-48FC-B19E-630E59A739EF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person BillToContactPerson { get; set; }

        #region Allors
        [Id("ccf88515-6441-4d0f-a2e7-8f5ed7c0533e")]
        [AssociationId("ce230886-53a7-4360-b545-a20d3cf47f1f")]
        [RoleId("2f7e7d1b-6a61-41a6-a05f-375e8a5feeb2")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Indexed]
        [Workspace]
        public Facility Facility { get; set; }

        #region Allors
        [Id("d74bd1fd-f243-4b5d-8061-1eafe7c25beb")]
        [AssociationId("5465663b-6757-4b1d-9f91-233bfd86bc5d")]
        [RoleId("35c28c9f-852a-4ebb-bc2b-1dce9e3812fa")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("8C27B92F-FB4C-4FAA-8F1C-3CF80AD746E8")]
        [AssociationId("50CC1084-F52D-4FE2-AE92-1753147927F3")]
        [RoleId("84E02756-377F-4270-82BD-97FFF4824ACD")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person ShipToContactPerson { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
            
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Approve() { }

        public void Reject() { }

        public void Hold() { }

        public void Continue() { }

        public void Confirm() { }

        public void Cancel() { }

        public void Complete() { }
        public void Invoice() { }
        public void Reopen() { }

        public void Print() { }
        #endregion

        #region Allors
        [Id("2CED78A3-0A7D-475B-82EE-5374D6E65944")]
        #endregion
        [Workspace]
        public void Send() { }

        #region Allors
        [Id("08E9783F-4DEE-428B-ADDD-785775AFAA46")]
        #endregion
        [Workspace]
        public void QuickReceive() { }
    }
}
namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("5B8C17C9-17BF-4B80-9246-AF7125EAE976")]
    #endregion
    public partial class PurchaseOrderVersion : OrderVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public User CreatedBy { get; set; }

        public User LastModifiedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

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

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("A6D9BF95-9717-4336-A261-4773FBD93CA8")]
        [AssociationId("3184D552-1468-4F10-B734-336BEABA1D39")]
        [RoleId("E7F10255-1913-4361-8F2A-4C1AAA176CB4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public PurchaseOrderState PurchaseOrderState { get; set; }

        #region Allors
        [Id("C14D2C09-056E-4D36-B0CB-5F5471C28CA1")]
        [AssociationId("223428A0-6A20-4D3F-A3AA-8490E325330A")]
        [RoleId("FEC45864-A3A4-41CD-B8BB-F603AE6F3732")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public PurchaseOrderShipmentState PurchaseOrderShipmentState { get; set; }

        #region Allors
        [Id("EABEB07B-871B-4FCC-A38C-51CBAEBD4F35")]
        [AssociationId("925F905A-CFE4-4272-A4E0-31BD3BE3D2E9")]
        [RoleId("9EE86C5B-43E5-45E1-9453-BF3FD548771C")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public PurchaseOrderPaymentState PurchaseOrderPaymentState { get; set; }

        #region Allors
        [Id("CAE880EC-B266-4CB2-9FD4-2A0F8B0ACBF8")]
        [AssociationId("A1AB9BBA-921A-4CA7-B1A0-A3500BBF769C")]
        [RoleId("5E33CAE0-4522-4A44-8085-3353E0BABB21")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public PurchaseOrderItem[] PurchaseOrderItems { get; set; }

        #region Allors
        [Id("774CEA12-501D-4C7A-885B-A198079CF74E")]
        [AssociationId("498F1CC3-1097-4216-BB18-DD2892CDEF15")]
        [RoleId("6E35415E-33B4-4914-A14A-AF16E641412E")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public Party TakenViaSupplier { get; set; }

        #region Allors
        [Id("86664C8E-E336-42F3-A65A-4D7B22EF92F7")]
        [AssociationId("3D87A511-4BA6-4969-B27F-87C7E1E1BAF4")]
        [RoleId("31C09E4D-7922-47EC-A3E9-7C2E9DF59DB0")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        public Party PreviousTakenViaSupplier { get; set; }

        #region Allors
        [Id("8AC728F2-F766-47C4-93B7-15B5D5DC2FF6")]
        [AssociationId("7824A2B4-F57A-4D5A-942D-EBAB64D674C1")]
        [RoleId("7A484CAE-17FB-46D0-A49E-44A5EA970FAB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public ContactMechanism TakenViaContactMechanism { get; set; }

        #region Allors
        [Id("C7B99EF4-7DE8-4214-A598-E6E46608E166")]
        [AssociationId("3789439A-992A-4947-8EE0-A4E40440F113")]
        [RoleId("B55B7BCC-386E-47A2-B328-17B6F6A3EF21")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Person TakenViaContactPerson { get; set; }

        #region Allors
        [Id("A368FB1C-8467-40E9-BC33-47BA5AEA9A0B")]
        [AssociationId("155B4626-22C8-4FE8-AC24-E75F91ECF52E")]
        [RoleId("90426B16-51F2-4D93-AEF2-4A109348831A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public ContactMechanism BillToContactMechanism { get; set; }

        #region Allors
        [Id("555F06E3-2C07-4A62-A4D5-E52E64A92362")]
        [AssociationId("DF0499B7-1BAA-42FB-BECD-F023D8EC43BA")]
        [RoleId("D431167C-CA76-42BF-A11F-6D8E79EEFDDC")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Person BillToContactPerson { get; set; }

        #region Allors
        [Id("69DDEF12-B6AA-4040-991D-CF1D20A0D5EC")]
        [AssociationId("10D5F3CB-64ED-4010-92FD-13539CAD3F78")]
        [RoleId("D2C6D6DB-F38B-49E4-9AC5-187CD41D3863")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Facility Facility { get; set; }

        #region Allors
        [Id("57C5DCE6-ACA0-4D03-89B2-4D7CC3AE6E45")]
        [AssociationId("8DBE214D-F4B6-445A-838A-F04B02839F46")]
        [RoleId("41AAB6C9-FFA9-4AA3-8867-27101B8D3D08")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("88691341-493F-4F23-8329-32AC6FC7682E")]
        [AssociationId("C689156A-7A51-4838-A790-360DB50F7C69")]
        [RoleId("3862640B-4ADE-4C52-9FF4-E108FC889736")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
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

        #endregion
    }
}

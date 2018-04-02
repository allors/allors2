namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7d7e4b6d-eebd-460c-b771-a93cd8d64bce")]
    #endregion
    public partial class PurchaseInvoice : Invoice, Versioned
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }
        public decimal TotalShippingAndHandlingCustomerCurrency { get; set; }
        public Currency Currency { get; set; }
        public string Description { get; set; }
        public ShippingAndHandlingCharge ShippingAndHandlingCharge { get; set; }
        public decimal TotalFeeCustomerCurrency { get; set; }
        public Fee Fee { get; set; }
        public decimal TotalExVatCustomerCurrency { get; set; }
        public string CustomerReference { get; set; }
        public DiscountAdjustment DiscountAdjustment { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TotalDiscount { get; set; }
        public BillingAccount BillingAccount { get; set; }
        public decimal TotalIncVat { get; set; }
        public decimal TotalSurcharge { get; set; }
        public decimal TotalBasePrice { get; set; }
        public decimal TotalVatCustomerCurrency { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal TotalIncVatCustomerCurrency { get; set; }
        public decimal TotalShippingAndHandling { get; set; }
        public decimal TotalBasePriceCustomerCurrency { get; set; }
        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        public decimal TotalExVat { get; set; }
        public SalesTerm[] SalesTerms { get; set; }
        public decimal TotalSurchargeCustomerCurrency { get; set; }
        public string InvoiceNumber { get; set; }
        public string Message { get; set; }
        public VatRegime VatRegime { get; set; }
        public decimal TotalDiscountCustomerCurrency { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalFee { get; set; }

        public Person ContactPerson { get; set; }

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string Comment { get; set; }
        public string PrintContent { get; set; }

        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region ObjectStates
        #region PurchaseInvoiceState
        #region Allors
        [Id("EDC12BA8-41F6-4E3A-8430-9592201A821E")]
        [AssociationId("F633172D-B01C-4E06-8FAB-02D811E46A43")]
        [RoleId("A6421D9C-7F24-4009-A76B-EACABCE8138C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseInvoiceState PreviousPurchaseInvoiceState { get; set; }

        #region Allors
        [Id("96B88C50-E18C-4776-86CF-D3126A4E5C1B")]
        [AssociationId("3B7BCFD7-D56D-4F02-84BC-6EB4E55293FB")]
        [RoleId("63FD899E-5617-4CED-998A-7FAC30AF007D")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public PurchaseInvoiceState LastPurchaseInvoiceState { get; set; }

        #region Allors
        [Id("AAB01767-7EA3-48E4-85ED-153DED6CB873")]
        [AssociationId("481BBBE1-1429-4CBB-9D1B-1478B3A76AEB")]
        [RoleId("E19CF950-9F3C-4696-91CE-EEA33B4BC054")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseInvoiceState PurchaseInvoiceState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("E1F38604-4DB9-4D34-A34E-9B64649ABDE9")]
        [AssociationId("18F624C0-1535-4259-970A-336B8D3265DE")]
        [RoleId("E11301DE-46C3-4FC4-84F2-B2930CDB3872")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseInvoiceVersion CurrentVersion { get; set; }

        #region Allors
        [Id("AC26A490-1260-4E2D-B621-E827C12FAA39")]
        [AssociationId("FD0DD85A-3792-45FE-B3B4-4AFB7D920C35")]
        [RoleId("30411BBB-CB85-4043-8ADB-4641C2DB21FD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseInvoiceVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("d4bbc5ed-08a4-4d89-ad53-7705ae71d029")]
        [AssociationId("8ce81b66-22e5-4195-a270-5e9f761ff51e")]
        [RoleId("58245287-7a75-45c4-a000-d3944ec9319a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public Party BilledFrom { get; set; }

        #region Allors
        [Id("045918FA-CC14-4616-A2B0-519E2ACEBA31")]
        [AssociationId("5DB9DFF5-4964-4DE6-AEA6-FB6111D35502")]
        [RoleId("2B69921B-A93D-49F7-A950-D65F9EDB2A94")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public InternalOrganisation BilledTo { get; set; }

        #region Allors
        [Id("ABB96E7A-F5D1-4173-AEDB-62B217A22495")]
        [AssociationId("1AC98B51-187C-46E4-9909-7DA70F75334B")]
        [RoleId("7C6964CA-365C-402E-9046-E913EACEB020")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party BillToCustomer { get; set; }

        #region Allors
        [Id("B9ED27E2-9429-40AF-8B1E-4C4210023F5F")]
        [AssociationId("5C01982A-11AC-4983-8B27-229BF86A3A45")]
        [RoleId("39C7038E-2687-4F9B-9282-9D10E3929603")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public ContactMechanism BillToCustomerContactMechanism { get; set; }

        #region Allors
        [Id("F3CF7DEC-452C-4A68-9653-E7BB8987F8A1")]
        [AssociationId("DDBB0961-232F-427D-9957-CAD860377ACE")]
        [RoleId("31AB05CD-6611-4E0A-B655-F98F47DEEF16")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PaymentMethod BillToCustomerPaymentMethod { get; set; }

        #region Allors
        [Id("F37F0194-EDF4-49DA-BDB2-AA9C2A601809")]
        [AssociationId("87C2A5CD-1C7C-499E-8757-43191CBC9FE9")]
        [RoleId("3A4150A9-B63E-4422-960C-D8AEF707E679")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToCustomer { get; set; }

        #region Allors
        [Id("3E8E3FAF-2FFE-483A-8F7C-3ABF4BC29BD6")]
        [AssociationId("99126CF2-C82D-4501-8F96-3A354AACD105")]
        [RoleId("35580F2F-9148-4505-BA61-954FE8FE2855")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PostalAddress ShipToAddress { get; set; }

        #region Allors
        [Id("0E1BB984-6D42-4473-9BA1-A3EBDEF84A54")]
        [AssociationId("DCBFDD56-D47A-4229-9762-07C35D2CB826")]
        [RoleId("A2BA0164-3E36-494D-96D3-CBEA0C737279")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public Party ShipToEndCustomer { get; set; }

        #region Allors
        [Id("B4EC8C7B-E4BA-4428-9898-6FB9B6C048A4")]
        [AssociationId("9A43B583-CE97-4379-B5FB-5BDE616F0F10")]
        [RoleId("E1698EAF-2904-4C69-83EC-22FD04CE8BD3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public PostalAddress ShipToEndCustomerAddress { get; set; }

        #region Allors
        [Id("4cf09eb7-820f-4677-bfc0-92a48d0a938b")]
        [AssociationId("5a71ca58-db28-4edc-9065-32396380bd80")]
        [RoleId("fa280c8d-ac7b-4d99-80dd-fba155d4aef9")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        [Workspace]
        public PurchaseInvoiceItem[] PurchaseInvoiceItems { get; set; }

        #region Allors
        [Id("e444b5e7-0128-49fc-86cb-a6fe39c280ae")]
        [AssociationId("d6240de5-9b99-4525-b7d0-ef28a3381821")]
        [RoleId("6c911870-2737-4997-87a6-65ca55c17c55")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public PurchaseInvoiceType PurchaseInvoiceType { get; set; }

        #region Allors
        [Id("147372B8-ADC3-442E-BF60-968A0B13FBDD")]
        [AssociationId("8C96181E-4527-4A79-BA6A-9B36E132A8F2")]
        [RoleId("CBCAB42E-08A8-49C7-94F1-D89E7B20B9DD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public PurchaseOrder PurchaseOrder { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

        #region Allors
        [Id("B188B7B5-BA61-4FF5-9D9A-812E22F8A289")]
        #endregion
        [Workspace]
        public void Approve() { }

        #region Allors
        [Id("07A2BE5F-5686-4B0A-8B05-8875FA277622")]
        #endregion
        [Workspace]
        public void CancelInvoice() { }

        #region Allors
        [Id("79C4C934-91B0-457C-8E4D-7ADED062F188")]
        #endregion
        [Workspace]
        public void Finish() { }

        #region Allors
        [Id("422DD593-DECC-40FD-9216-D5A25458B59F")]
        #endregion
        [Workspace]
        public void CreateSalesInvoice() { }

    }
}
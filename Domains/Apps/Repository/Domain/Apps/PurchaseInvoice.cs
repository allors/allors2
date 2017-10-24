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
    public Currency CustomerCurrency { get; set; }
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
    public InvoiceTerm[] InvoiceTerms { get; set; }
    public decimal TotalSurchargeCustomerCurrency { get; set; }
    public string InvoiceNumber { get; set; }
    public string Message { get; set; }
    public VatRegime VatRegime { get; set; }
    public decimal TotalDiscountCustomerCurrency { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalFee { get; set; }
    public Permission[] DeniedPermissions { get; set; }
    public SecurityToken[] SecurityTokens { get; set; }
    public Locale Locale { get; set; }
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
    [Id("4cf09eb7-820f-4677-bfc0-92a48d0a938b")]
    [AssociationId("5a71ca58-db28-4edc-9065-32396380bd80")]
    [RoleId("fa280c8d-ac7b-4d99-80dd-fba155d4aef9")]
    #endregion
    [Multiplicity(Multiplicity.OneToMany)]
    [Indexed]
    public PurchaseInvoiceItem[] PurchaseInvoiceItems { get; set; }

    #region Allors
    [Id("d4bbc5ed-08a4-4d89-ad53-7705ae71d029")]
    [AssociationId("8ce81b66-22e5-4195-a270-5e9f761ff51e")]
    [RoleId("58245287-7a75-45c4-a000-d3944ec9319a")]
    #endregion
    [Multiplicity(Multiplicity.ManyToOne)]
    [Indexed]
    [Required]
    public Party BilledFromParty { get; set; }

    #region Allors
    [Id("e444b5e7-0128-49fc-86cb-a6fe39c280ae")]
    [AssociationId("d6240de5-9b99-4525-b7d0-ef28a3381821")]
    [RoleId("6c911870-2737-4997-87a6-65ca55c17c55")]
    #endregion
    [Multiplicity(Multiplicity.ManyToOne)]
    [Indexed]
    [Required]
    public PurchaseInvoiceType PurchaseInvoiceType { get; set; }

    #region inherited methods

    public void OnBuild() { }

    public void OnPostBuild() { }

    public void OnPreDerive() { }

    public void OnDerive() { }

    public void OnPostDerive() { }

    #endregion

    #region Allors
    [Id("BA5FFA5C-CD22-4C8B-8D6F-2512E4CED65D")]
    #endregion
    public void Ready() { }

    #region Allors
    [Id("B188B7B5-BA61-4FF5-9D9A-812E22F8A289")]
    #endregion
    public void Approve() { }

    #region Allors
    [Id("07A2BE5F-5686-4B0A-8B05-8875FA277622")]
    #endregion
    public void Cancel() { }
  }
}
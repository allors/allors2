namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("7d7e4b6d-eebd-460c-b771-a93cd8d64bce")]
    #endregion
    public partial class PurchaseInvoice : Invoice
    {
        #region inherited properties
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
        public ObjectState PreviousObjectState { get; set; }
        public ObjectState LastObjectState { get; set; }
        public string Comment { get; set; }
        public Guid UniqueId { get; set; }
        public string PrintContent { get; set; }
        public User CreatedBy { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Allors
        [Id("4cf09eb7-820f-4677-bfc0-92a48d0a938b")]
        [AssociationId("5a71ca58-db28-4edc-9065-32396380bd80")]
        [RoleId("fa280c8d-ac7b-4d99-80dd-fba155d4aef9")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        PurchaseInvoiceItem[] PurchaseInvoiceItems { get; set; }

        #region Allors
        [Id("86859b7b-e627-43fe-ba75-711d4c104807")]
        [AssociationId("ba1aeb33-0351-4fbf-b80c-881cdf4ded5c")]
        [RoleId("7caa47ab-1f54-4fad-87b8-639b37269635")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        InternalOrganisation BilledToInternalOrganisation { get; set; }

        #region Allors
        [Id("bc059d0f-e9bd-41e8-82ff-9615a01ec24a")]
        [AssociationId("770c0376-8552-4d0c-b45f-b759018c3c85")]
        [RoleId("5658422f-4097-49db-b97c-79bab6f337b4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        PurchaseInvoiceObjectState CurrentObjectState { get; set; }

        #region Allors
        [Id("d4bbc5ed-08a4-4d89-ad53-7705ae71d029")]
        [AssociationId("8ce81b66-22e5-4195-a270-5e9f761ff51e")]
        [RoleId("58245287-7a75-45c4-a000-d3944ec9319a")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        Party BilledFromParty { get; set; }

        #region Allors
        [Id("e444b5e7-0128-49fc-86cb-a6fe39c280ae")]
        [AssociationId("d6240de5-9b99-4525-b7d0-ef28a3381821")]
        [RoleId("6c911870-2737-4997-87a6-65ca55c17c55")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        PurchaseInvoiceType PurchaseInvoiceType { get; set; }
        
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
        [Id("625E5C92-B2CD-4759-A4FA-AB6D5D7DB88E")]
        [AssociationId("CC3F8BF8-BEC1-4432-A01F-1C6C3E81AF36")]
        [RoleId("785779C8-373A-4709-A491-B2C07EE27652")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseInvoiceVersion PreviousVersion { get; set; }

        #region Allors
        [Id("AC26A490-1260-4E2D-B621-E827C12FAA39")]
        [AssociationId("FD0DD85A-3792-45FE-B3B4-4AFB7D920C35")]
        [RoleId("30411BBB-CB85-4043-8ADB-4641C2DB21FD")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseInvoiceVersion[] AllVersions { get; set; }

        #region Allors
        [Id("83227DDE-96D5-4AD2-A760-53D361847C27")]
        [AssociationId("7875E005-C9F2-42DF-B61F-B2DE98C025DA")]
        [RoleId("3A9D7171-9A7F-4340-B757-1516151BDD31")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public PurchaseInvoiceVersion CurrentStateVersion { get; set; }

        #region Allors
        [Id("6853BD9B-2061-4931-A6E4-7FB04011C98B")]
        [AssociationId("B9D42D1B-B47E-48C3-A4C0-8E7B1140E727")]
        [RoleId("F801D70E-3452-4FE3-BA0C-B97BD18F667A")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public PurchaseInvoiceVersion[] AllStateVersions { get; set; }
        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}

        public void AddNewInvoiceItem() { }

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
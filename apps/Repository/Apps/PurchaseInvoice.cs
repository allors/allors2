namespace Allors.Repository.Domain
{
    using System;

    #region Allors
    [Id("7d7e4b6d-eebd-460c-b771-a93cd8d64bce")]
    #endregion
    public partial class PurchaseInvoice : Invoice 
    {
        #region inherited properties
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

        public string PrintContent { get; set; }

        public Guid UniqueId { get; set; }

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
        [Id("86859b7b-e627-43fe-ba75-711d4c104807")]
        [AssociationId("ba1aeb33-0351-4fbf-b80c-881cdf4ded5c")]
        [RoleId("7caa47ab-1f54-4fad-87b8-639b37269635")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public InternalOrganisation BilledToInternalOrganisation { get; set; }
        
        #region Allors
        [Id("8f9e98b7-c87c-47c7-a267-3044c7414534")]
        [AssociationId("1b4b2f6b-7294-428f-b0ea-beb43050557a")]
        [RoleId("bea637e9-c320-4bdb-ac4b-d571e3fa0c8d")]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Derived]
        [Indexed]
        public PurchaseInvoiceStatus CurrentInvoiceStatus { get; set; }
        
        #region Allors
        [Id("bc059d0f-e9bd-41e8-82ff-9615a01ec24a")]
        [AssociationId("770c0376-8552-4d0c-b45f-b759018c3c85")]
        [RoleId("5658422f-4097-49db-b97c-79bab6f337b4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        public PurchaseInvoiceObjectState CurrentObjectState { get; set; }
        
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
        
        #region Allors
        [Id("ed3987d4-3dd1-4483-bcbb-8f1f0b18ff84")]
        [AssociationId("1c1d90ff-5910-4f39-b6ad-aa12a6e6c60e")]
        [RoleId("d23c55ff-857b-40bc-b041-15f0ceb910a5")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Derived]
        [Indexed]
        public PurchaseInvoiceStatus[] InvoiceStatuses { get; set; }

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
        
        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}







        #endregion
    }
}
namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("C23DBDD0-8933-4582-8995-8767EFDA82D5")]
    #endregion
    public partial class PurchaseInvoiceVersion : InvoiceVersion
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

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("7751055A-3C59-4723-B7DF-42C377624BE0")]
        [AssociationId("5FB242DC-FB8B-4347-A0F2-65BCC0BBC056")]
        [RoleId("DAF79CD7-EAD0-4091-8545-24ADEEC919AF")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        [Required]
        public PurchaseInvoiceState PurchaseInvoiceState { get; set; }

        #region Allors
        [Id("65E39E93-6445-459A-99E8-0ED388B85B4B")]
        [AssociationId("AC00032A-8EE6-4B18-B387-44A02AD8F1A0")]
        [RoleId("7DAB2DA2-EFE4-4B91-B425-07FB2A59B216")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public PurchaseInvoiceItem[] PurchaseInvoiceItems { get; set; }

        #region Allors
        [Id("277D12EB-729E-419A-A9EB-35F30DFFBA15")]
        [AssociationId("0B791CFD-9124-44AF-BC2B-D26A79FFF3F1")]
        [RoleId("2827C4E6-E649-48EA-86CD-C7B7689EB7A7")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public Party BilledFromParty { get; set; }

        #region Allors
        [Id("4A072DD0-0886-4CFF-9DC4-D213854160E7")]
        [AssociationId("741297D3-BB5D-4789-986C-509B7D95A6EC")]
        [RoleId("CD477E7F-C66A-477E-BC30-EEC813CC0211")]
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
    }
}
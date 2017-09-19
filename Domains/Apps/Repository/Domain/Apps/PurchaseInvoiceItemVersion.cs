namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("")]
    #endregion
    public partial class PurchaseInvoiceItemVersion : InvoiceItemVersion
    {
        #region inherited properties
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string InternalComment { get; set; }
        public AgreementTerm[] InvoiceTerms { get; set; }
        public decimal TotalInvoiceAdjustment { get; set; }
        public InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }
        public InvoiceItem AdjustmentFor { get; set; }
        public SerialisedInventoryItem SerializedInventoryItem { get; set; }
        public string Message { get; set; }
        public decimal TotalInvoiceAdjustmentCustomerCurrency { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
        #endregion

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        PurchaseInvoiceItemType PurchaseInvoiceItemType { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        Part Part { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        [Indexed]
        PurchaseInvoiceItemObjectState CurrentObjectState { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("")]
    #endregion
    public partial interface InvoiceItemVersion : Version
    {
        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Size(-1)]
        [Workspace]
        string InternalComment { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        AgreementTerm[] InvoiceTerms { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalInvoiceAdjustment { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        InvoiceItem AdjustmentFor { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        SerialisedInventoryItem SerializedInventoryItem { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Size(-1)]
        [Workspace]
        string Message { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal TotalInvoiceAdjustmentCustomerCurrency { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal AmountPaid { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        decimal Quantity { get; set; }

        #region Allors
        [Id("")]
        [AssociationId("")]
        [RoleId("")]
        #endregion
        [Size(256)]
        [Workspace]
        string Description { get; set; }
    }
}
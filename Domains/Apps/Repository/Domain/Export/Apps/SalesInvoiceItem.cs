namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("a98f8aca-d711-47e8-ac9c-25b607cbaef1")]
    #endregion
    public partial class SalesInvoiceItem : InvoiceItem, Versioned
    {
        #region inherited properties

        public ObjectState[] PreviousObjectStates { get; set; }

        public ObjectState[] LastObjectStates { get; set; }

        public ObjectState[] ObjectStates { get; set; }

        public string InternalComment { get; set; }

        public SalesTerm[] SalesTerms { get; set; }

        public decimal TotalInvoiceAdjustment { get; set; }

        public InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }

        public InvoiceItem AdjustmentFor { get; set; }

        public string Message { get; set; }

        public decimal TotalInvoiceAdjustmentCustomerCurrency { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Quantity { get; set; }

        public string Description { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public string Comment { get; set; }

        public LocalisedText[] LocalisedComments { get; set; }

        public decimal TotalDiscountAsPercentage { get; set; }

        public DiscountAdjustment DiscountAdjustment { get; set; }

        public decimal UnitVat { get; set; }

        public decimal TotalVatCustomerCurrency { get; set; }

        public VatRegime VatRegime { get; set; }

        public decimal TotalVat { get; set; }

        public decimal UnitSurcharge { get; set; }

        public decimal UnitDiscount { get; set; }

        public decimal TotalExVatCustomerCurrency { get; set; }

        public VatRate DerivedVatRate { get; set; }

        public decimal ActualUnitPrice { get; set; }

        public decimal TotalIncVatCustomerCurrency { get; set; }

        public decimal UnitBasePrice { get; set; }

        public decimal CalculatedUnitPrice { get; set; }

        public decimal TotalSurchargeCustomerCurrency { get; set; }

        public decimal TotalIncVat { get; set; }

        public decimal TotalSurchargeAsPercentage { get; set; }

        public decimal TotalDiscountCustomerCurrency { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalSurcharge { get; set; }

        public VatRegime AssignedVatRegime { get; set; }

        public decimal TotalBasePrice { get; set; }

        public decimal TotalExVat { get; set; }

        public decimal TotalBasePriceCustomerCurrency { get; set; }

        public PriceComponent[] CurrentPriceComponents { get; set; }

        public SurchargeAdjustment SurchargeAdjustment { get; set; }
        #endregion

        #region ObjectStates
        #region SalesInvoiceItemState
        #region Allors
        [Id("6033F4A9-9ABA-457C-9A44-218415E01B79")]
        [AssociationId("A71CB471-C8CB-42F1-A1AF-E32F71BEC61F")]
        [RoleId("D6C62ED7-07B7-44FC-A774-1BD6B54554D9")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesInvoiceItemState PreviousSalesInvoiceItemState { get; set; }

        #region Allors
        [Id("7623707D-C0F7-47D8-9B39-E34A55FC087B")]
        [AssociationId("E80895A6-EAAE-46B3-8823-3B2CD4DA8324")]
        [RoleId("48627D83-BEEB-42B1-B299-BE11451AF90C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Derived]
        public SalesInvoiceItemState LastSalesInvoiceItemState { get; set; }

        #region Allors
        [Id("AC0B80C8-84C6-4A2D-8CE1-B94994537998")]
        [AssociationId("81BF99E7-5831-42BE-B7D8-64FB11D3C626")]
        [RoleId("06151951-E93B-44B6-8152-84FBAB29057C")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace]
        public SalesInvoiceItemState SalesInvoiceItemState { get; set; }
        #endregion
        #endregion

        #region Versioning
        #region Allors
        [Id("61008480-5266-42B1-BD09-477C514F5FC5")]
        [AssociationId("EC5E9E80-DFED-4EFB-9923-CC066FA6975A")]
        [RoleId("8FCEDD24-9BF7-4CC9-8387-C661BEE650C5")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToOne)]
        [Workspace]
        public SalesInvoiceItemVersion CurrentVersion { get; set; }

        #region Allors
        [Id("3409316D-FD54-408C-BC1C-9468CCE4B72E")]
        [AssociationId("DC9E9D4F-27AC-4022-ACB4-F4916BF010BF")]
        [RoleId("C646543C-2FF4-4DA5-99D6-EACAE7A28FDA")]
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Workspace]
        public SalesInvoiceItemVersion[] AllVersions { get; set; }
        #endregion

        #region Allors
        [Id("D3D47236-8B21-420B-883A-C035EB0DBAE0")]
        [AssociationId("64108AA6-8478-49D1-ACB2-34CCB7F790DB")]
        [RoleId("490E7A59-C0CA-47FF-881B-5D2F4474BD5F")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("6df95cf4-115f-4f43-aaea-52313c47d824")]
        [AssociationId("93ba1265-4050-41c1-aaf8-d09786889245")]
        [RoleId("0abd9811-a8ac-42bf-9113-4f9760cfe9eb")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public SerialisedInventoryItem SerialisedInventoryItem { get; set; }

        #region Allors
        [Id("0854aece-6ca1-4b8d-99a9-6d424de8dfd4")]
        [AssociationId("cebb5430-809a-4d46-bc7b-563ee72f0848")]
        [RoleId("f1f68b89-b95f-43c9-82d5-cb9eec635869")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("103d42a5-fdee-4689-af19-2ea4c8060de3")]
        [AssociationId("ee01bcc4-b926-444d-8982-8c56158327f1")]
        [RoleId("a1643b4c-c95e-427c-a6b8-44860bc79d6e")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal RequiredProfitMargin { get; set; }

        #region Allors
        [Id("1a18b2f1-a31e-4ec3-8981-5f65af2ff907")]
        [AssociationId("398e3c8d-1b7f-40c5-a4f1-4a086d369199")]
        [RoleId("514101cb-6833-4935-81e7-79c64b417a26")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal InitialMarkupPercentage { get; set; }

        #region Allors
        [Id("2f6e0b52-d37c-4caf-91d0-862666195247")]
        [AssociationId("898628e9-2191-4a2f-b05d-517b5ac90e5c")]
        [RoleId("6a71a9f7-f572-4cd2-b8ca-86e3c85a5d71")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaintainedMarkupPercentage { get; set; }

        #region Allors
        [Id("4f9e110d-fca8-4956-9d2f-178843eb9b9f")]
        [AssociationId("95aa4883-8bd0-4cd7-a060-4efabaef6530")]
        [RoleId("02e0ee54-d063-4b00-87be-c2d3747ef3a6")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal UnitPurchasePrice { get; set; }

        #region Allors
        [Id("6dd4e8ee-48ed-400d-a129-99a3a651586a")]
        [AssociationId("f99e5e01-943c-4de9-862c-c472d2d873f2")]
        [RoleId("6cb182c2-b481-4e26-869e-609990ea68b3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InvoiceItemType InvoiceItemType { get; set; }

        #region Allors
        [Id("90866201-03a1-44b2-9318-5048639b58c8")]
        [AssociationId("0618fddc-dee4-4cd4-9d4d-b7356be9dc65")]
        [RoleId("d61277d3-b916-4783-9de0-48f9eb6808c4")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace]
        public Person[] SalesReps { get; set; }

        #region Allors
        [Id("a04f506f-7ac9-4ab9-8f3f-1aba1ae76a67")]
        [AssociationId("7774b9a7-e842-4b3d-b608-5d039b0811fb")]
        [RoleId("b7b589f5-59f2-4004-862f-0fb6c790137d")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal InitialProfitMargin { get; set; }

        #region Allors
        [Id("ba9acc7e-635d-4387-98eb-67ea26e9e2db")]
        [AssociationId("0198d048-f14e-419d-ac2f-1f7f8e2d0bbc")]
        [RoleId("7d6f6274-24bb-47e4-892b-ce95cd197d77")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaintainedProfitMargin { get; set; }

        #region Allors
        [Id("bd485f1f-6937-4270-8695-6f9a50e671c3")]
        [AssociationId("4314e405-2692-4cda-9617-804b43d7090f")]
        [RoleId("b8ab5103-31c0-41cb-b6a0-e8f3e18a7945")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public TimeEntry[] TimeEntries { get; set; }

        #region Allors
        [Id("bfd8c2d5-57f9-4650-97ae-2f2b1819b3a9")]
        [AssociationId("6dbc805e-2360-49ef-bdd5-644a454cae40")]
        [RoleId("b6e9179b-b7d8-4ad8-9aee-0ca3adef40af")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal RequiredMarkupPercentage { get; set; }

        #region Allors
        [Id("09E97D2C-55A5-4D96-8028-2B36FCB90994")]
        [AssociationId("59EB1A75-3521-4506-A044-CB93554C6C9F")]
        [RoleId("E15A86AE-6ABA-4D24-8021-BA68A466DAC4")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Details { get; set; }

        #region Allors
        [Id("BB115D9A-53F8-4A3C-95F0-403A883C84FE")]
        [AssociationId("563F27E6-37AD-486A-9F90-85751C6458EE")]
        [RoleId("DDB0F028-B9D6-4D8D-88D4-245ADA2B90EB")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Facility Facility{ get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
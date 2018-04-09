namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("F838CABD-3769-47D8-8623-66D2723B5D1B")]
    #endregion
    public partial class SalesInvoiceItemVersion : InvoiceItemVersion
    {
        #region inherited properties

        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string InternalComment { get; set; }
        public SalesTerm[] SalesTerms { get; set; }
        public decimal TotalInvoiceAdjustment { get; set; }
        public InvoiceVatRateItem[] InvoiceVatRateItems { get; set; }
        public InvoiceItem AdjustmentFor { get; set; }
        public SerialisedInventoryItem SerializedInventoryItem { get; set; }
        public string Message { get; set; }
        public decimal TotalInvoiceAdjustmentCustomerCurrency { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }

        public Guid DerivationId { get; set; }

        public DateTime DerivationTimeStamp { get; set; }

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

        #region Allors
        [Id("D5986C17-F56D-4402-BBFA-FE8389E89AEA")]
        [AssociationId("BFC9E9F5-B3A0-48EB-B7EA-4F13E730FEC9")]
        [RoleId("F181E01C-9B88-491C-9B78-50AC5BC7A777")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public SalesInvoiceItemState SalesInvoiceItemState { get; set; }

        #region Allors
        [Id("30D936D7-5C26-4E70-B880-B7E125B931E7")]
        [AssociationId("23F6AF1C-2B3A-46EC-9C5F-9421D33DA99A")]
        [RoleId("936534D3-CBB3-464C-838B-764D241008E9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public ProductFeature ProductFeature { get; set; }

        #region Allors
        [Id("801584AF-0927-4F4B-84C8-730343FFF17F")]
        [AssociationId("E689025E-F721-4946-B232-71B416098B17")]
        [RoleId("07DA2F62-06C4-42E2-9918-40D6A825F755")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal RequiredProfitMargin { get; set; }

        #region Allors
        [Id("AB12792B-4158-40B4-B015-3EB1A20935FB")]
        [AssociationId("F42002FC-069A-4A08-A5D0-EDB266A0212E")]
        [RoleId("0B42C0C0-7CC4-46E0-A1E3-AB9547DDE5A5")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal InitialMarkupPercentage { get; set; }

        #region Allors
        [Id("FE62C039-2066-4762-8D2C-A0A38DD75992")]
        [AssociationId("6D76734D-CFB1-4E85-93F3-8164AF2F71EC")]
        [RoleId("C95A6D0E-2124-43FF-BF99-4437F610DB2F")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaintainedMarkupPercentage { get; set; }

        #region Allors
        [Id("D223A126-F816-4628-842D-469CBEBB9CB2")]
        [AssociationId("1CC67F90-460E-4FA9-A6ED-081F04F0D4DC")]
        [RoleId("3A1A4A50-9A57-47AD-B152-5FE0F40E945B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Product Product { get; set; }

        #region Allors
        [Id("A6A9CADD-E3DA-445B-BF50-C5222AD32370")]
        [AssociationId("45B1F258-9115-4224-B413-3DB8BB02C2C3")]
        [RoleId("9BCABC24-7B87-483E-88A9-6C7ED131DCEB")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal UnitPurchasePrice { get; set; }

        #region Allors
        [Id("7A952F88-BC4E-4F23-A0D5-44D47E30666E")]
        [AssociationId("9D5B95BE-20AA-4D1A-81A2-6FD69B1A6365")]
        [RoleId("26EC36C3-42D1-4941-90D3-2E1B45132E1B")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        [Workspace]
        public InvoiceItemType InvoiceItemType { get; set; }

        #region Allors
        [Id("5CCCCC43-D7E1-47B7-8E6B-826EFFB6E578")]
        [AssociationId("A66F7125-2D42-49D6-9BB0-DC4A3A5A0065")]
        [RoleId("FAE867DA-754F-46A3-8269-16578FBF7B1A")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace]
        public Person SalesRep { get; set; }

        #region Allors
        [Id("29C25636-E541-40F3-8507-8DBEC61A4A58")]
        [AssociationId("764CE804-04FA-483D-B222-E3F095CCE5D0")]
        [RoleId("6E003CF0-7E1D-4682-AF18-4B804F0C86AB")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal InitialProfitMargin { get; set; }

        #region Allors
        [Id("500AA5E4-F9E2-4514-9ADD-5A18BB932C4E")]
        [AssociationId("DE12B137-F003-4EDC-B091-0C7D2DD42650")]
        [RoleId("279626D8-39D5-4E8A-BCCF-C1D9D6FD0585")]
        #endregion
        [Derived]
        [Required]
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal MaintainedProfitMargin { get; set; }

        #region Allors
        [Id("B77A583B-5F97-4843-A1C0-603467BADE51")]
        [AssociationId("4DCD87D0-864A-44A1-AFE3-5C7DE75CC78D")]
        [RoleId("D6FAA55B-BB79-4C38-AB05-EB30B008D0CA")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public TimeEntry[] TimeEntries { get; set; }

        #region Allors
        [Id("D8589BDF-698B-40C1-A9F2-06631823F7DB")]
        [AssociationId("135E7966-F436-4F27-BE4F-A721758D5457")]
        [RoleId("0B527A4E-7E5B-4A83-A88F-41F5371B390F")]
        #endregion
        [Precision(19)]
        [Scale(2)]
        [Workspace]
        public decimal RequiredMarkupPercentage { get; set; }

        #region Allors
        [Id("85A108D7-5444-43EA-91F5-DF2DDBA1B862")]
        [AssociationId("8B53845D-10FC-4EB6-A59E-FB4DC0DA0322")]
        [RoleId("75F1244C-A709-4653-BE7F-C3F32D7861E4")]
        #endregion
        [Size(-1)]
        [Workspace]
        public string Details { get; set; }

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }
        #endregion
    }
}
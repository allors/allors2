namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("e3e87d40-b4f0-4953-9716-db13b35d716b")]
    #endregion
    public partial class NonUnifiedGood : Good
    {
        #region inherited properties
        public string Comment { get; set; }
        public LocalisedText[] LocalisedComments { get; set; }
        public Guid UniqueId { get; set; }
        public Permission[] DeniedPermissions { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string Name { get; set; }
        public LocalisedText[] LocalisedNames { get; set; }
        public string Description { get; set; }
        public LocalisedText[] LocalisedDescriptions { get; set; }
        public string InternalComment { get; set; }
        public Document[] Documents { get; set; }
        public Media[] ElectronicDocuments { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public string Keywords { get; set; }
        public Media PrimaryPhoto { get; set; }
        public Media[] Photos { get; set; }
        public DateTime SupportDiscontinuationDate { get; set; }
        public DateTime SalesDiscontinuationDate { get; set; }
        public PriceComponent[] VirtualProductPriceComponents { get; set; }
        public string IntrastatCode { get; set; }
        public ProductCategory[] ProductCategoriesExpanded { get; set; }
        public Product ProductComplement { get; set; }
        public Product[] Variants { get; set; }
        public DateTime IntroductionDate { get; set; }
        public EstimatedProductCost[] EstimatedProductCosts { get; set; }
        public Product[] ProductObsolescences { get; set; }
        public VatRate VatRate { get; set; }
        public PriceComponent[] BasePrices { get; set; }
        public ProductIdentification[] ProductIdentifications { get; set; }
        public string BarCode { get; set; }
        public Part Part { get; set; }
        public Product[] ProductSubstitutions { get; set; }
        public Product[] ProductIncompatibilities { get; set; }

        #endregion

        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}





        public void Delete(){}
        #endregion
    }
}
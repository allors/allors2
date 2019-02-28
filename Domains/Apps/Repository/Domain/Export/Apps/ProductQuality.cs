namespace Allors.Repository
{
    using System;

    using Attributes;

    #region Allors
    [Id("d14fa0d2-8743-4d3c-8109-2ab9161cb310")]
    #endregion
    public partial class ProductQuality : ProductFeature, Enumeration 
    {
        #region inherited properties
        public EstimatedProductCost[] EstimatedProductCosts { get; set; }

        public PriceComponent[] BasePrices { get; set; }

        public string Description { get; set; }

        public ProductFeature[] DependentFeatures { get; set; }

        public ProductFeature[] IncompatibleFeatures { get; set; }

        public VatRate VatRate { get; set; }

        public Guid UniqueId { get; set; }

        public Permission[] DeniedPermissions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        #endregion


        #region inherited methods


        public void OnBuild(){}

        public void OnPostBuild(){}

        public void OnInit()
        {
            
        }

        public void OnPreDerive(){}

        public void OnDerive(){}

        public void OnPostDerive(){}


        #endregion

        #region Allors
        [Id("106519EC-0E05-4B19-9D7F-9CA455C3A931")]
        #endregion
        [Workspace]
        public void Delete() { }
    }
}